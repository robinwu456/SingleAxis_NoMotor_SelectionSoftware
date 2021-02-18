using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    class Calculation : CalculationModel {
        // 計算推薦規格
        public Dictionary<string, object> GetRecommandResult(Condition con) {
            // 取所有Model
            List<Model> models = GetAllModels();

            // 基本篩選(安裝方式、重複定位精度)
            // ...

            // pipeLine處理
            Dictionary<string, object> calcResult = PipelineCalc(models, con);

            return calcResult;
        }

        // 平行運算
        private Dictionary<string, object> PipelineCalc(List<Model> models, Condition con) {            
            // 平行運算執行緒宣告
            List<Thread> threadsPipeline = new List<Thread>();
            threadsPipeline.Add(new Thread(() => CalcMethod(models, con)));   // 暫定
            // ...

            // 平行運算開始
            foreach (Thread thread in threadsPipeline)
                thread.Start();

            return null;
        }

        private Dictionary<string, object> CalcMethod(List<Model> models, Condition con) {
            Dictionary<string, object> result = new Dictionary<string, object>() {
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
            }

            return null;
        }
    }
}
