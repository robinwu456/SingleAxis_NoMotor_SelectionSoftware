using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public abstract class TorqueConfirm_Belt : TorqueConfirm {
        public TorqueConfirm_Belt(Model model, Condition condition) : base(model, condition) { }

        public override void Calc() {
            GetMotorParam();

            // 馬達能力預估
            MotorConfirm();
            // 馬達最大扭矩確認
            MotorTorqueConfirm();
            // 皮帶最大扭矩確認
            BeltTorqueConfirm();
        }

        /// <summary>
        /// 馬達能力預估
        /// </summary>
        public abstract void MotorConfirm();

        /// <summary>
        /// 馬達最大扭矩確認
        /// </summary>
        public abstract void MotorTorqueConfirm();

        /// <summary>
        /// 皮帶最大扭矩確認
        /// </summary>
        public abstract void BeltTorqueConfirm();
    }
}
