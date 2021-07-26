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
                Func<DataRow, bool> GetLeadCondition = row => Convert.ToDouble(row["導程"].ToString()) == model.lead;
                //// 導程減速比轉換
                //if (IsContainsReducerRatio(model.name))
                //    GetLeadCondition = row => Convert.ToDouble(row["導程"].ToString()) == model.lead;
                model.availablePowers = modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString() == model.name && GetLeadCondition(row))["馬達瓦數"].ToString().Split('&').ToList()
                                                                      .Select(power => Convert.ToInt32(power)).ToList();
                // 取馬達參數
                model.usePower = model.availablePowers.Max();   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["馬達瓦數"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["額定轉矩"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["最大轉矩"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["轉動慣量"]).First());
            } else if (condition.powerSelection == Condition.PowerSelection.Custom) {
                //model.usePower = -1;
                model.ratedTorque = condition.ratedTorque;
                model.maxTorque = condition.maxTorque;
                model.rotateInertia = condition.rotateInertia;
            } else if (condition.powerSelection == Condition.PowerSelection.SelectedPower) {
                // 取馬達參數
                model.usePower = condition.selectedPower;   // 適用馬達的最大瓦數
                //model.usePower = conditions.curSelectModel;   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["馬達瓦數"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["額定轉矩"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["最大轉矩"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["轉動慣量"]).First());
            }

            model.stopTime = condition.stopTime;
            model.screwLength = model.stroke + 100;

            // 小數點位數修正
            model.accelSpeed = model.vMax / model.accelTime;
            model.accelSpeed = Convert.ToDouble(model.accelSpeed.ToString("#0.000"));
            //model.vMax = Convert.ToDouble(model.vMax.ToString("#0.000"));

            //model.accelTime = model.vMax / model.accelSpeed;            
            model.decelTime = model.accelTime;
            model.constantTime = ((2f * (float)model.stroke / 1000f / model.vMax) - model.accelTime - model.decelTime) / 2f;
            model.moveTime = Convert.ToDouble((model.accelTime + model.constantTime + model.decelTime).ToString("#0.000"));

            // 加速區外力
            model.rollingFriction_accel = model.p_a * model.c * 0.003;
            model.inertialLoad_accel = (model.load + 3) * model.accelSpeed;
            if (condition.setupMethod == Model.SetupMethod.垂直)
                model.otherForce_accel = model.load * 9.8;
            model.equivalentLoad_accel = Math.Abs(model.rollingFriction_accel + model.accessoriesFriction_accel + model.inertialLoad_accel + model.otherForce_accel);

            // 等速區外力
            model.rollingFriction_constant = model.p_c * model.c * 0.003;
            model.inertialLoad_constant = 0;
            if (condition.setupMethod == Model.SetupMethod.垂直)
                model.otherForce_constant = model.load * 9.8;
            model.equivalentLoad_constant = Math.Abs(model.rollingFriction_constant + model.accessoriesFriction_constant + model.inertialLoad_constant + model.otherForce_constant);

            // 減速區外力
            model.rollingFriction_decel = model.p_d * model.c * 0.003;
            model.inertialLoad_decel = (model.load + 3) * model.accelSpeed * -1;
            if (condition.setupMethod == Model.SetupMethod.垂直)
                model.otherForce_decel = model.load * 9.8;
            model.equivalentLoad_decel = Math.Abs(model.rollingFriction_decel + model.accessoriesFriction_decel + model.inertialLoad_decel + model.otherForce_decel);

            // 停等區外力
            if (condition.setupMethod == Model.SetupMethod.垂直)
                model.otherForce_stop = model.load * 9.8;
        }
    }
}
