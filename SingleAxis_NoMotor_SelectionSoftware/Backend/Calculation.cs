using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    class Calculation : CalculationModel {
        private int calcCountPerThread = 10;   // 單執行緒運算的筆數

        // 計算推薦規格
        public Dictionary<string, object> GetRecommandResult(Condition condition) {
            // 取所有Model
            List<Model> models = GetAllModels(condition);

            // 首要篩選條件
            IEnumerable<Model> con;
            con = models.Where(model => model.supportedSetup.Contains(condition.setupMethod));  // 安裝方式
            // 重複定位精度
            if (condition.repeatability != -1)
                con = con.Where(model => condition.RepeatabilityCondition(model.repeatability));

            // pipeLine處理
            Dictionary<string, object> calcResult = PipelineCalc(models, condition);

            return calcResult;
        }

        // 平行運算
        private Dictionary<string, object> PipelineCalc(List<Model> models, Condition condition) {
            List<Model> pipeLineAllModels = models;
            Dictionary<string, object> pipeLineResult = new Dictionary<string, object>() { { "List", new List<Model>() }, { "Alarm", false }, { "Msg", "" }, };
            bool isPipeLineCalcError = false;
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
                        Dictionary<string, object> resultPerThread = CalcMethod(m, condition);
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

            string nullModelAlarmMsg = "";
            if (resultModels.Count == 0 && pipeLineResult["Msg"].ToString() == "")
                pipeLineResult["Msg"] = nullModelAlarmMsg;

            pipeLineResult["List"] = resultModels;
            pipeLineAllModels = resultModels;

            return pipeLineResult;
        }

        private Dictionary<string, object> CalcMethod(List<Model> models, Condition con) {
            Dictionary<string, object> pipeLineResult = new Dictionary<string, object>() {
                { "List", new List<Model>() },
                { "Msg", "" },
                { "Alarm", false },
            };

            foreach (Model model in models) {
                // 滑軌壽命計算            
                model.slideTrackServiceLifeDistance = GetSlideTrackEstimatedLife(model, con);
                //// 螺桿壽命計算
                //model.screwServiceLifeDistance = GetScrewEstimatedLife(model, conditions);
                //// 扭矩計算
                //(bool is_tMax_OK, bool is_tRms_OK) confirmResult = TorqueConfirm(model, conditions);
                //model.is_tMax_OK = confirmResult.is_tMax_OK;
                //model.is_tRms_OK = confirmResult.is_tRms_OK;

                ////// 線速度 m/s => mm/s
                ////model.vMax *= 1000;
                ////// 加速度 m/s => mm/s
                ////model.accelSpeed *= 1000;

                //// 結果壽命取最小值
                //model.serviceLifeDistance = Math.Min(model.slideTrackServiceLifeDistance, model.screwServiceLifeDistance);

                //// 算壽命時間
                //model.serviceLifeTime = GetServiceLifeTime(model, conditions);

                (pipeLineResult["List"] as List<Model>).Add(model);
            }

            return pipeLineResult;
        }
    }
}
