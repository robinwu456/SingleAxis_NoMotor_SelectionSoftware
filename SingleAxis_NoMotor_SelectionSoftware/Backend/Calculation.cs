using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

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
            // 非測試才做基本篩選
            if (condition.calcMode != Condition.CalcMode.Test) {
                // 使用環境
                con = con.Where(model => model.useEnvironment == condition.useEnvironment || condition.useEnvironment == Model.UseEnvironment.Null);

                if (condition.calcMode == Condition.CalcMode.Normal)
                    // 機構型態
                    con = con.Where(model => model.modelType == condition.modelType || condition.modelType == Model.ModelType.Null);

                // 安裝方式
                con = con.Where(model => model.supportedSetup.Contains(condition.setupMethod));
            }
            // 單項計算
            if (condition.calcModel.model != null)
                con = con.Where(model => model.name.StartsWith(condition.calcModel.model) && model.lead == condition.calcModel.lead);

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
            Dictionary<string, Dictionary<string, Func<Model, object>>> filterMap = new Dictionary<string, Dictionary<string, Func<Model, object>>>() {
                { "超過最大行程", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("最大行程: {0}", model.maxStroke) },
                    { "Condition", model => model.maxStroke >= condition.stroke || condition.calcMode == Condition.CalcMode.CalcMax } } },
                { "超過最大荷重", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("最大荷重: {0}", model.maxLoad) },
                    { "Condition", model => model.maxLoad == -1 || model.maxLoad != -1 && model.maxLoad >= condition.load || condition.calcMode == Condition.CalcMode.CalcMax } } },
                { "運行時間過短，請增加運行時間", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("計算運行時間: {0}", model.moveTime) },
                    { "Condition", model => model.moveTime <= condition.moveTime } } },
                { "未達希望壽命", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("希望壽命: {0}年, 計算壽命: {1}年{2}個月又{3}天", condition.expectServiceLifeTime, model.serviceLifeTime.year, model.serviceLifeTime.month, model.serviceLifeTime.day) },
                    { "Condition", model => model.serviceLifeTime == (-1, -1, -1) || model.serviceLifeTime.year >= condition.expectServiceLifeTime } } },
                { "每分鐘趟數過大", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("單趟時間: {0}", model.moveTime * 2) },
                    { "Condition", model => model.serviceLifeTime != (-1, -1, -1) } } },
                // 以下為之前會顯示紅色項目
                { "T_max安全係數過低", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("標準: 大於等於{0}, 計算值: {1}", model.isUseBaltCalc ? Model.tMaxStandard_beltMotor : Model.tMaxStandard, model.tMaxSafeCoefficient) },
                    { "Condition", model => model.tMaxSafeCoefficient >= (model.isUseBaltCalc ? Model.tMaxStandard_beltMotor : Model.tMaxStandard) } } },
                { "皮帶馬達安全係數過低", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("標準: 小於{0}, 計算值: {1}", Model.beltMotorStandard, model.beltMotorSafeCoefficient) },
                    { "Condition", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < Model.beltMotorStandard } } },
                { "皮帶T_max安全係數過低", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => string.Format("標準: 大於等於{0}, 計算值: {1}", Model.tMaxStandard_belt, model.beltSafeCoefficient) },
                    { "Condition", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= Model.tMaxStandard_belt } } },
                { "力矩警示異常", new Dictionary<string, Func<Model, object>>(){
                    { "Remark",    model => "" },
                    { "Condition", model => model.isMomentVerifySuccess } } },
            };

            // log篩選
            if (Directory.Exists(Config.LOG_FAIL_MODELS_FILENAME))
                Directory.Delete(Config.LOG_FAIL_MODELS_FILENAME, true);
            Directory.CreateDirectory(Config.LOG_FAIL_MODELS_FILENAME);
            string logFilter = "";
            foreach (Model model in resultModels) {
                string curLog = "";
                foreach (var con in filterMap) {
                    string curFilterText = con.Key.Split('，')[0];
                    if (!(bool)con.Value["Condition"](model))
                        curLog += "、" + string.Format("{0}({1})", curFilterText, con.Value["Remark"](model).ToString());
                }
                if (curLog != string.Empty) {
                    curLog = curLog.Remove(0, 1);
                    logFilter += string.Format("{0}-L{1}：", model.name, model.lead) + curLog + Environment.NewLine;
                    FileUtil.LogModelInfo(model, condition.setupMethod, false, Config.LOG_FAIL_MODELS_FILENAME + string.Format("{0}-L{1}", model.name, model.lead));
                }
            }            
            FileUtil.FileWrite(Config.LOG_FILTER_FILENAME, logFilter);

            // null訊息篩選
            var errorFilter = filterMap.Where(filter => resultModels.Any(model => !(bool)filter.Value["Condition"](model))).Select(filter => filter.Key);
            nullModelAlarmMsg = string.Join("、", errorFilter);
            //List<string> errorMsg = new List<string>();
            foreach (var filter in filterMap) {
                oldResultModelCount = resultModels.Count;
                resultModels = resultModels.Where(model => condition.calcMode == Condition.CalcMode.Test || (bool)filter.Value["Condition"](model)).ToList();
                // 測試log
                if (resultModels.Count < oldResultModelCount && condition.calcMode == Condition.CalcMode.Test)
                    Console.WriteLine("測試失敗項目：" + filter.Key);
            }
            //nullModelAlarmMsg = string.Join("、", errorMsg);            

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
                if (!model.isUseBaltCalc)
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
                    if (model.modelType.IsRodType())
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
