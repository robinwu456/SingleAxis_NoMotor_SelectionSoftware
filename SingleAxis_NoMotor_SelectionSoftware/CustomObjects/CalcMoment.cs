using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CalcMoment {
        // 力舉參數
        public double w;
        public double mr;
        public double mp_a, my_a;
        public double mp_c, my_c;
        public double mp_d, my_d;

        private Dictionary<Model.SetupMethod, Action> CalcMethod = new Dictionary<Model.SetupMethod, Action>();
        private Model model;

        public CalcMoment(Model model, Model.SetupMethod setupMethod) {
            this.model = model;

            CalcMethod = new Dictionary<Model.SetupMethod, Action>() {
                { Model.SetupMethod.水平, Calc_Horizontal },
                { Model.SetupMethod.橫掛, Calc_WallHang },
                { Model.SetupMethod.垂直, Calc_Vertical },
            };

            CalcMethod[setupMethod]();
        }

        private void Calc_Horizontal() {
            w = model.load * 9.8;
            mr = w * model.moment_C / 1000;

            // 加速區                        
            mp_a = (model.load * model.accelSpeed * model.moment_A / 1000) + (w * model.moment_B / 1000);
            my_a = model.load * model.accelSpeed * model.moment_C / 1000;

            // 等速區
            mp_c = (float)w * (float)model.moment_B / 1000f;
            my_c = 0;

            // 減速區
            mp_d = -1 * (model.load * model.accelSpeed * model.moment_A / 1000) + (w * model.moment_B / 1000);
            my_d = -1 * model.load * model.accelSpeed * model.moment_C / 1000;
        }

        private void Calc_WallHang() {
            w = model.load * 9.8;
            mr = w * model.moment_A / 1000f;

            // 加速區                        
            mp_a = model.load * model.accelSpeed * model.moment_A / 1000;
            my_a = (model.load * model.accelSpeed * model.moment_C / 1000) + (w * model.moment_B / 1000);

            // 等速區
            mp_c = 0;
            my_c = (float)w * (float)model.moment_B / 1000f;

            // 減速區
            mp_d = -1 * model.load * model.accelSpeed * model.moment_A / 1000;
            my_d = -1 * (model.load * model.accelSpeed * model.moment_C / 1000) + (w * model.moment_B / 1000);
        }

        private void Calc_Vertical() {
            if (model.modelType.IsBeltType())
                w = model.load * 9.8;
            else
                w = 0;
            mr = 0;

            // 加速區                        
            mp_a = (model.load * model.accelSpeed * model.moment_A / 1000) + (model.load * 9.8f * model.moment_A / 1000);
            my_a = (model.load * model.accelSpeed * model.moment_C / 1000) + (model.load * 9.8f * model.moment_C / 1000);

            // 等速區
            mp_c = (float)model.load * 9.8f * (float)model.moment_A / 1000f;
            my_c = (float)model.load * 9.8f * (float)model.moment_C / 1000f;

            // 減速區
            mp_d = -1 * (model.load * model.accelSpeed * model.moment_A / 1000) + (model.load * 9.8f * model.moment_A / 1000);
            my_d = -1 * (model.load * model.accelSpeed * model.moment_C / 1000) + (model.load * 9.8f * model.moment_C / 1000);
        }
    }
}
