using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class TorqueConfirm_Screw : TorqueConfirm {
        private Model model;
        private Condition condition;

        public TorqueConfirm_Screw(Model model, Condition condition) : base(model, condition) {
            this.model = model;
            this.condition = condition;
        }

        public override void Calc() {
            GetMotorParam();

            // 轉動慣量
            model.rotateInertia_motor = model.rotateInertia;
            model.rotateInertia_screw = (Math.PI * (7.8 * Math.Pow(10, 3) * (model.screwLength / 1000f) * Math.Pow(model.outerDiameter / 1000f, 4))) / 32f;
            model.rotateInertia_horizontalMove = (model.load + 1) * (Math.Pow((model.lead / 1000f) / (2f * Math.PI), 2));
            model.rotateInertia_total = model.rotateInertia_motor + model.rotateInertia_screw + model.rotateInertia_horizontalMove + model.rotateInertia_couplingItem + model.rotateInertia_ballBearing;

            // 軸向外力
            model.forceTotal_accel = model.rollingFriction_accel + model.accessoriesFriction_accel + model.otherForce_accel;
            model.forceTotal_constant = model.rollingFriction_constant + model.accessoriesFriction_constant + model.otherForce_constant;
            model.forceTotal_decel = model.rollingFriction_decel + model.accessoriesFriction_decel + model.otherForce_decel;
            model.forceTotal_stop = model.rollingFriction_stop + model.accessoriesFriction_stop + model.otherForce_stop;

            // 加速區扭矩
            model.inertialTorque_accel = (model.rotateInertia_total * (model.rpm - 0)) / (9.55f * model.accelTime);
            model.forceTorque_accel = (model.forceTotal_accel * (model.lead / 1000f)) / (2f * Math.PI * 0.9f);
            model.torqueTotal_accel = model.inertialTorque_accel + model.forceTorque_accel;

            // 等速區扭矩
            model.inertialTorque_constant = 0;
            model.forceTorque_constant = (model.forceTotal_constant * (model.lead / 1000f)) / (2f * Math.PI * 0.9f);
            model.torqueTotal_constant = model.inertialTorque_constant + model.forceTorque_constant;

            // 減速區扭矩
            model.inertialTorque_decel = (model.rotateInertia_total * (0 - model.rpm)) / (9.55f * model.accelTime);
            model.forceTorque_decel = (model.forceTotal_decel * (model.lead / 1000f)) / (2f * Math.PI * 0.9f);
            model.torqueTotal_decel = model.inertialTorque_decel + model.forceTorque_decel;

            // 停等區扭矩
            model.inertialTorque_stop = 0;
            model.forceTorque_stop = (model.forceTotal_stop * (model.lead / 1000f)) / (2f * Math.PI * 0.9f);
            model.torqueTotal_stop = model.inertialTorque_stop + model.forceTorque_stop;

            // T_max最大扭矩確認
            model.tMax = Math.Max(model.torqueTotal_accel, Math.Max(model.torqueTotal_constant, Math.Max(model.torqueTotal_decel, model.torqueTotal_stop)));
            model.tMaxSafeCoefficient = Math.Round(model.maxTorque / model.tMax, 2);
            model.is_tMax_OK = model.tMaxSafeCoefficient >= Model.tMaxStandard;

            // T_Rms扭矩確認
            model.tRms = Math.Pow((Math.Pow(model.torqueTotal_accel, 2) * model.accelTime +
                                 Math.Pow(model.torqueTotal_constant, 2) * model.constantTime +
                                 Math.Pow(model.torqueTotal_decel, 2) * model.decelTime +
                                 Math.Pow(model.torqueTotal_stop, 2) * model.stopTime) / (model.accelTime + model.constantTime + model.decelTime + model.stopTime), 0.5f);
            model.tRmsSafeCoefficient = model.ratedTorque / model.tRms;
            model.tRmsSafeCoefficient = Math.Round(model.tRmsSafeCoefficient, 2);
            model.is_tRms_OK = model.tRmsSafeCoefficient > Model.tRmsStandard;
        }
    }
}
