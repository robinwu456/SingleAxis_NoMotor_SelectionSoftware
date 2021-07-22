using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public abstract class TorqueConfirm : CalculationBase {
        private Model model;
        private Condition condition;

        public TorqueConfirm(Model model, Condition condition) {
            this.model = model;
            this.condition = condition;
        }

        /// <summary>
        /// 扭矩計算
        /// </summary>
        public abstract void Calc();

        /// <summary>
        /// 取馬達參數
        /// </summary>
        protected void GetMotorParam() {
            if (condition.powerSelection == Condition.PowerSelection.Standard) {
                // 取得適用功率
                Func<DataRow, bool> GetLeadCondition = row => Convert.ToDouble(row["Lead"].ToString()) == model.lead;
                //// 導程減速比轉換
                //if (IsContainsReducerRatio(model.name))
                //    GetLeadCondition = row => Convert.ToDouble(row["Lead"].ToString()) == model.lead;
                model.availablePowers = modelInfo.Rows.Cast<DataRow>().First(row => row["Model"].ToString() == model.name && GetLeadCondition(row))["Power"].ToString().Split('&').ToList()
                                                                      .Select(power => Convert.ToInt32(power)).ToList();
                // 取馬達參數
                model.usePower = model.availablePowers.Max();   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["Power"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RatedTorque"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["MaxTorque"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RotateInertia"]).First());
            } else if (condition.powerSelection == Condition.PowerSelection.Custom) {
                //model.usePower = -1;
                model.ratedTorque = condition.ratedTorque;
                model.maxTorque = condition.maxTorque;
                model.rotateInertia = condition.rotateInertia;
            } else if (condition.powerSelection == Condition.PowerSelection.SelectedPower) {
                // 取馬達參數
                model.usePower = condition.selectedPower;   // 適用馬達的最大瓦數
                //model.usePower = conditions.curSelectModel;   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["Power"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RatedTorque"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["MaxTorque"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RotateInertia"]).First());
            }

            model.stopTime = condition.stopTime;
            model.screwLength = model.stroke + 100;
        }
    }
}
