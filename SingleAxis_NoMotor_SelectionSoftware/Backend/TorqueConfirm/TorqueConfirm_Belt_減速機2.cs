using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class TorqueConfirm_Belt_減速機2 : TorqueConfirm_Belt_減速機 {
        private Model model;
        private Condition condition;

        public TorqueConfirm_Belt_減速機2(Model model, Condition condition) : base(model, condition) {
            this.model = model;
            this.condition = condition;
        }

        public override void MotorConfirm() {
            // 皮帶輪加減速關係
            model.mainWheelRpm = model.rpm;
            model.reducerRpmRatio = Convert.ToDouble(model.name.Split('-')[1]);
            model.subWheelRpm = (int)(model.rpm / model.reducerRpmRatio);
            model.beltLoad = model.beltUnitDensity / 1000 * model.beltWidth * model.beltLength / 1000;

            // 轉動慣量
            model.rotateInertia_motor = model.rotateInertia * Math.Pow(1000, 2);
            model.rotateInertia_load = model.load * Math.Pow(model.subWheel_P3.diameter / 2, 2);
            model.rotateInertia_belt = model.beltLoad * Math.Pow(model.subWheel_P3.diameter / 2, 2);
            model.rotateInertia_total = model.rotateInertia_load + model.rotateInertia_belt + model.reducerRotateInertia + model.subWheel_P3.rotateInertia + model.subWheel_P4.rotateInertia;

            // 馬達是否適用
            model.beltMotorSafeCoefficient = Math.Round(model.rotateInertia_total / Math.Pow(model.reducerRpmRatio, 2) / model.rotateInertia_motor, 2);
            Model.beltMotorStandard = model.loadInertiaMomentRatio * 2;
            model.isMotorOK = model.beltMotorSafeCoefficient < Model.beltMotorStandard;
        }
    }
}
