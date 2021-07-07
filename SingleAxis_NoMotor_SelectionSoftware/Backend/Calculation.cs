using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Calculation : CalculationModel {
        //private int calcCountPerThread = 10;   // 單執行緒運算的筆數
        private int calcCountPerThread = 1000;   // 單執行緒運算的筆數
        private Dictionary<string, object> pipeLineResult = new Dictionary<string, object>();   // 即時運算完成的Model
        private List<Model> pipeLineAllModels = new List<Model>();  // 所有的Model
        bool isPipeLineCalcError = false;

        // 計算推薦規格
        public Dictionary<string, object> GetRecommandResult(Condition condition) {
            // 取所有Model
            List<Model> models = GetAllModels(condition);

            // 首要篩選條件
            IEnumerable<Model> con = models;
            // 使用環境
            con = con.Where(model => model.useEnvironment == condition.useEnvironment);
            // 機構型態
            con = con.Where(model => model.modelType == condition.modelType);
            // 安裝方式
            con = con.Where(model => model.supportedSetup.Contains(condition.setupMethod));
            // 重複定位精度(判斷螺桿、皮帶)
            con = con.Where(model => condition.RepeatabilityCondition(model.repeatability));
            // 單項計算
            if (condition.calcModel.model != null) {
                // 單項計算減速比驗證
                if (condition.reducerRatio.Keys.Contains(condition.calcModel.model)) {
                    condition.calcModel.lead /= (float)condition.reducerRatio[condition.calcModel.model];
                    condition.calcModel.lead = Convert.ToDouble(condition.calcModel.lead.ToString("#0.00"));
                }
                con = con.Where(model => model.name.Equals(condition.calcModel.model) && model.lead == condition.calcModel.lead);
            }

            models = con.ToList();

            // pipeLine處理
            Dictionary<string, object> calcResult = PipelineCalc(models, condition);

            return calcResult;
        }

        // 取得運算進度
        public (int percent, bool isError) GetCalcPercent() {
            int percent = 0;
            bool isError = isPipeLineCalcError;

            if (!pipeLineResult.Keys.Contains("List"))
                percent = 0;
            else {
                if (pipeLineAllModels.Count == 0)
                    isError = true;
                percent = (int)(((float)(pipeLineResult["List"] as List<Model>).Count / (float)pipeLineAllModels.Count) * 100);
            }

            return (percent, isError);
        }

        // 平行運算
        private Dictionary<string, object> PipelineCalc(List<Model> models, Condition condition) {
            pipeLineAllModels = models;
            pipeLineResult = new Dictionary<string, object>() { { "List", new List<Model>() }, { "Alarm", false }, { "Msg", "" }, };
            isPipeLineCalcError = false;
            // 平行運算執行緒宣告
            List<Thread> threadsPipeline = new List<Thread>();
            List<List<Model>> modelsPerPipeline = new List<List<Model>>();

            // 計算量分配            
            for (int i = 0; i <= models.Count / calcCountPerThread; i++) {
                List<Model> m = new List<Model>();
                for (int j = 0; j < models.Count - i * calcCountPerThread && j < calcCountPerThread; j++)
                    m.Add(models[i * calcCountPerThread + j]);
                modelsPerPipeline.Add(m);
            }
            // 每個執行序事件指派
            foreach (List<Model> m in modelsPerPipeline) {
                Thread t = new Thread(() => {
                    try {
                        // 總列表Add
                        Dictionary<string, object> resultPerThread = GetEstimatedLife(m, condition);
                        foreach (Model model in resultPerThread["List"] as List<Model>)
                            (pipeLineResult["List"] as List<Model>).Add(model);
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                        pipeLineResult["Msg"] = ex.Message.Split('|')[1];
                        pipeLineResult["StatusCode"] = ex.Message.Split('|')[0];
                    }
                });
                t.Name = "t" + modelsPerPipeline.IndexOf(m);
                threadsPipeline.Add(t);
            }
            // 平行運算開始
            foreach (Thread thread in threadsPipeline)
                thread.Start();

            // 等待各執行緒完成
            Thread.Sleep(100);
            while (threadsPipeline.Any(t => t.IsAlive))
                Thread.Sleep(10);

            // Model排序
            List<Model> resultModels = new List<Model>();
            foreach (Model m in models) {
                try {
                    IEnumerable<Model> r = (pipeLineResult["List"] as List<Model>).Where(model => model.name == m.name && model.lead == m.lead);
                    if (r.Count() == 0) {
                        isPipeLineCalcError = true;
                        break;
                    }
                    resultModels.Add(r.First());
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    //// 遇到bug重算
                    //goto reCalculate;
                }
            }
            int oldResultModelCount = resultModels.Count;
            string nullModelAlarmMsg = "";

            // 打勾項目記錄
            Model checkedModel = null;            
            if (condition.curCheckedModel.model != null) {
                // 減速比驗證
                Func<Model, bool> predicate;
                if (condition.reducerRatio.Keys.Contains(condition.curCheckedModel.model))
                    // 包含減速比時，不判斷導程
                    predicate = model => model.name == condition.curCheckedModel.model;
                else
                    predicate = model => model.name == condition.curCheckedModel.model && model.lead == condition.curCheckedModel.lead;
                // Init打勾項目
                if (resultModels.Any(predicate))
                    checkedModel = resultModels.First(model => model.name == condition.curCheckedModel.model && model.lead == condition.curCheckedModel.lead);
                else {
                    condition.curCheckedModel.model = null;
                    condition.curCheckedModel.lead = -1;
                }
            }

            // 篩選條件
            Dictionary<string, Func<Model, bool>> filterMap = new Dictionary<string, Func<Model, bool>>() {
                { "超過最大行程", model => model.maxStroke >= condition.stroke },
                { "超過最大荷重", model => model.maxLoad == -1 || model.maxLoad != -1 && model.maxLoad >= condition.load },
                { "線速度過大", model => model.vMax <= model.vMax_max },
                { "行程過短，建議可增加行程，或降低線速度", model => model.constantTime >= 0 },
                { "運行時間過短，請增加運行時間", model => model.moveTime <= condition.moveTime },
                { "未達希望壽命", model => model.serviceLifeTime.year >= condition.expectServiceLifeTime },
                // 以下為之前會顯示紅色項目
                { "T_max安全係數過低", model => model.tMaxSafeCoefficient >= Model.tMaxStandard },
                { "皮帶馬達安全係數過低", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < Model.beltMotorStandard },
                { "皮帶T_max安全係數過低", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= Model.tMaxStandard_beltMotor },
                { "力矩警示異常", model => model.isMomentVerifySuccess },
            };

            // log篩選
            string logFilter = "";
            foreach (Model model in resultModels) {
                string curLog = "";
                foreach (var con in filterMap) {
                    string curFilterText = con.Key.Split('，')[0];
                    if (!con.Value(model))
                        curLog += "、" + curFilterText;
                }
                if (curLog != string.Empty) {
                    curLog = curLog.Remove(0, 1);
                    logFilter += string.Format("{0}-L{1}：", model.name, model.lead) + curLog + Environment.NewLine;
                }
            }
            FileUtil.FileWrite(Config.LOG_FILTER_FILENAME, logFilter);

            foreach (var filter in filterMap) {
                oldResultModelCount = resultModels.Count;
                resultModels = resultModels.Where(filter.Value).ToList();
                // 篩選訊息
                if (resultModels.Count < oldResultModelCount)
                    nullModelAlarmMsg += filter.Key + "|";
            }

            // 驗證打勾項目是否還在列表裡
            if (checkedModel != null) {
                // 若沒有就加在第一列
                if (!resultModels.Contains(checkedModel))
                    resultModels.Insert(0, checkedModel);
                else {
                    // 若有就移去第一列
                    resultModels.Remove(checkedModel);
                    resultModels.Insert(0, checkedModel);
                }
            }

            // 查無資料則添加訊息
            if (resultModels.Count == 0 && pipeLineResult["Msg"].ToString() == "")
                pipeLineResult["Msg"] = nullModelAlarmMsg;

            pipeLineResult["List"] = resultModels;
            pipeLineAllModels = resultModels;

            return pipeLineResult;
        }

        private Dictionary<string, object> GetEstimatedLife(List<Model> models, Condition con) {
            Dictionary<string, object> pipeLineResult = new Dictionary<string, object>() {
                { "List", new List<Model>() },
                { "Msg", "" },
                { "Alarm", false },
            };

            foreach (Model model in models) {
                // 滑軌壽命計算            
                model.slideTrackServiceLifeDistance = GetSlideTrackEstimatedLife(model, con);
                // 螺桿壽命計算
                model.screwServiceLifeDistance = GetScrewEstimatedLife(model, con);
                // 扭矩計算
                (bool is_tMax_OK, bool is_tRms_OK) confirmResult = TorqueConfirm(model, con);
                model.is_tMax_OK = confirmResult.is_tMax_OK;
                model.is_tRms_OK = confirmResult.is_tRms_OK;

                // 線速度 m/s => mm/s
                model.vMax *= 1000;
                // 加速度 m/s => mm/s
                model.accelSpeed *= 1000;

                // 結果壽命
                if (model.modelType.IsBeltType())
                    // 皮帶型
                    model.serviceLifeDistance = model.slideTrackServiceLifeDistance;
                else {
                    if (model.modelType == Model.ModelType.推桿系列 || model.modelType == Model.ModelType.輔助導桿推桿系列 || model.modelType == Model.ModelType.軌道外掛推桿系列)
                        // Y系列直接用螺桿壽命
                        model.serviceLifeDistance = model.screwServiceLifeDistance;
                    else
                        // 螺桿型滑軌、螺桿壽命取最小值
                        model.serviceLifeDistance = Math.Min(model.slideTrackServiceLifeDistance, model.screwServiceLifeDistance);
                }

                // 算壽命時間
                model.serviceLifeTime = GetServiceLifeTime(model, con);

                (pipeLineResult["List"] as List<Model>).Add(model);
            }

            return pipeLineResult;
        }
    }
}
