using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class TorqueConfirm_Belt_減速機構 : TorqueConfirm_Belt {
        private Model model;
        private Condition condition;

        public TorqueConfirm_Belt_減速機構(Model model, Condition condition) : base(model, condition) {
            this.model = model;
            this.condition = condition;
        }        

        public override void MotorConfirm() {
            // 皮帶輪加減速關係
            model.mainWheelRpm = model.rpm;
            model.reducerRpmRatio = model.subWheel1.diameter / model.mainWheel.diameter;
            model.subWheelRpm = (int)(model.rpm / model.reducerRpmRatio);
            model.beltLoad = model.beltUnitDensity / 1000 * model.beltWidth * model.beltLength / 1000;

            // 轉動慣量
            model.rotateInertia_motor = model.rotateInertia * Math.Pow(1000, 2);
            model.rotateInertia_load = model.load * Math.Pow(model.subWheel2.diameter / 2, 2);
            model.rotateInertia_belt = model.beltLoad * Math.Pow(model.subWheel2.diameter / 2, 2);
            model.rotateInertia_total = model.rotateInertia_load + model.rotateInertia_belt + model.mainWheel.rotateInertia + model.subWheel1.rotateInertia + model.subWheel2.rotateInertia + model.subWheel3.rotateInertia;

            // 馬達是否適用
            model.beltMotorSafeCoefficient = Math.Round(model.rotateInertia_total / Math.Pow(model.reducerRpmRatio, 2) / model.rotateInertia_motor, 2);
            Model.beltMotorStandard = Math.Round(model.loadInertiaMomentRatio * 2, 2);
            model.isMotorOK = model.beltMotorSafeCoefficient < Model.beltMotorStandard;
        }

        public override void MotorTorqueConfirm() {
            // 軸向外力
            if (condition.setupMethod == Model.SetupMethod.垂直) {
                model.otherForce_accel = model.load * (model.accelSpeed + 9.81);
                model.otherForce_constant = model.load * 9.81;
                model.otherForce_decel = model.load * (9.81 - model.accelSpeed);
                model.otherForce_stop = model.load * 9.81;
            } else {
                model.otherForce_accel = model.load * model.accelSpeed;
                model.otherForce_constant = 0;
                model.otherForce_decel = model.load * (0 - model.accelSpeed);
                model.otherForce_stop = 0;
            }
            model.forceTotal_accel = model.rollingFriction_accel + model.accessoriesFriction_belt_accel + model.otherForce_accel;
            model.forceTotal_constant = model.rollingFriction_constant + model.accessoriesFriction_belt_constant + model.otherForce_constant;
            model.forceTotal_decel = model.rollingFriction_decel + model.accessoriesFriction_belt_decel + model.otherForce_decel;
            model.forceTotal_stop = model.rollingFriction_stop + model.accessoriesFriction_belt_stop + model.otherForce_stop;

            // 加速區扭矩
            model.rotateInertia_loadMoving_subWheel = model.load * Math.Pow(model.vMax, 2) / Math.Pow(model.subWheelRpm * 2 * Math.PI / 60, 2);
            model.rotateInertia_loadMoving_motor = model.rotateInertia_loadMoving_subWheel / Math.Pow(model.reducerRpmRatio, 2);
            model.inertialTorque_accel = ((model.mainWheelRpm - 0) * 2 * Math.PI / 60) * model.rotateInertia_loadMoving_motor / model.accelTime;
            model.forceTorque_accel = (model.forceTotal_accel * ((model.subWheel2.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_accel = model.inertialTorque_accel + model.forceTorque_accel;

            // 等速區扭矩
            model.inertialTorque_constant = 0;
            model.forceTorque_constant = (model.forceTotal_constant * ((model.subWheel2.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_constant = model.inertialTorque_constant + model.forceTorque_constant;

            // 減速區扭矩
            model.inertialTorque_decel = (-1 * (model.mainWheelRpm - 0) * 2 * Math.PI / 60) * model.rotateInertia_loadMoving_motor / model.accelTime;
            model.forceTorque_decel = (model.forceTotal_decel * ((model.subWheel2.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_decel = model.inertialTorque_decel + model.forceTorque_decel;

            // 停置區扭矩
            model.inertialTorque_stop = 0;
            model.forceTorque_stop = (model.forceTotal_stop * ((model.subWheel2.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_stop = model.inertialTorque_stop + model.forceTorque_stop;

            // T_max最大扭矩確認
            model.tMax = Math.Max(model.torqueTotal_accel, Math.Max(model.torqueTotal_constant, Math.Max(model.torqueTotal_decel, model.torqueTotal_stop)));
            model.tMaxSafeCoefficient = Math.Round(model.maxTorque / model.tMax, 2);
            model.is_tMax_OK = model.tMaxSafeCoefficient >= Model.tMaxStandard_beltMotor;
        }

        public override void BeltTorqueConfirm() {
            // 各階段最大扭矩
            model.beltTorque_accel = Math.Abs(model.forceTorque_accel * Math.Pow(model.reducerRpmRatio, 2));
            model.beltTorque_constant = Math.Abs(model.forceTorque_constant * Math.Pow(model.reducerRpmRatio, 2));
            model.beltTorque_decel = Math.Abs(model.forceTorque_decel * Math.Pow(model.reducerRpmRatio, 2));
            model.beltTorque_stop = Math.Abs(model.forceTorque_stop * Math.Pow(model.reducerRpmRatio, 2));
            // 皮帶T_max最大扭矩
            model.belt_tMax = Math.Max(model.beltTorque_accel, Math.Max(model.beltTorque_constant, Math.Max(model.beltTorque_decel, model.beltTorque_stop)));
            // 皮帶承受力
            model.beltEndurance = model.belt_tMax * 1000 / (model.subWheel3.diameter / 2);
            // 皮帶安全係數
            model.beltSafeCoefficient = Math.Round(model.beltAllowableTension / model.beltEndurance, 2);
            model.is_belt_tMax_OK = model.beltSafeCoefficient > Model.tMaxStandard_belt;
        }                        
    }
}
