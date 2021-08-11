using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CalcMoment {
        // 力舉參數
        public double w;
        public double mr;
        public double mp_a, my_a, mr_a;
        public double mp_c, my_c, mr_c;
        public double mp_d, my_d, mr_d;

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

            if (model.isUseBaltCalc) {
                // 加速區
                model.gravityMr_a = w * model.moment_C / 1000;
                model.gravityMp_a = w * model.moment_B / 1000;
                model.gravityMy_a = 0;
                model.accelSpeedMr_a = 0;
                model.accelSpeedMp_a = model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_a = model.load * model.accelSpeed * model.moment_C / 1000;
                mr_a = model.gravityMr_a + model.accelSpeedMr_a;
                mp_a = model.gravityMp_a + model.accelSpeedMp_a;
                my_a = model.gravityMy_a + model.accelSpeedMy_a;

                // 等速區
                mp_c = (float)w * (float)model.moment_B / 1000f;
                my_c = 0;

                // 減速區
                model.gravityMr_d = w * model.moment_C / 1000;
                model.gravityMp_d = w * model.moment_B / 1000;
                model.gravityMy_d = 0;
                model.accelSpeedMr_d = 0;
                model.accelSpeedMp_d = -1 * model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_d = -1 * model.load * model.accelSpeed * model.moment_C / 1000;
                mr_d = model.gravityMr_d + model.accelSpeedMr_d;
                mp_d = model.gravityMp_d + model.accelSpeedMp_d;
                my_d = model.gravityMy_d + model.accelSpeedMy_d;
            } else {
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
        }

        private void Calc_WallHang() {
            w = model.load * 9.8;
            mr = w * model.moment_A / 1000f;

            if (model.isUseBaltCalc) {
                // 加速區
                model.gravityMr_a = w * model.moment_A / 1000;
                model.gravityMp_a = 0;
                model.gravityMy_a = w * model.moment_B / 1000;
                model.accelSpeedMr_a = 0;
                model.accelSpeedMp_a = model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_a = model.load * model.accelSpeed * model.moment_C / 1000;
                mr_a = model.gravityMr_a + model.accelSpeedMr_a;
                mp_a = model.gravityMp_a + model.accelSpeedMp_a;
                my_a = model.gravityMy_a + model.accelSpeedMy_a;

                // 等速區
                mp_c = 0;
                my_c = (float)w * (float)model.moment_B / 1000f;

                // 減速區
                model.gravityMr_d = w * model.moment_A / 1000;
                model.gravityMp_d = 0;
                model.gravityMy_d = w * model.moment_B / 1000;
                model.accelSpeedMr_d = 0;
                model.accelSpeedMp_d = -1 * model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_d = -1 * model.load * model.accelSpeed * model.moment_C / 1000;
                mr_d = model.gravityMr_d + model.accelSpeedMr_d;
                mp_d = model.gravityMp_d + model.accelSpeedMp_d;
                my_d = model.gravityMy_d + model.accelSpeedMy_d;
            } else {
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
        }

        private void Calc_Vertical() {
            if (model.modelType.IsBeltType())
                w = model.load * 9.8;
            else
                w = 0;
            mr = 0;

            if (model.isUseBaltCalc) {
                // 加速區
                model.gravityMr_a = 0;
                model.gravityMp_a = w * model.moment_A / 1000;
                model.gravityMy_a = w * model.moment_C / 1000;
                model.accelSpeedMr_a = 0;
                model.accelSpeedMp_a = model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_a = model.load * model.accelSpeed * model.moment_C / 1000;
                mr_a = model.gravityMr_a + model.accelSpeedMr_a;
                mp_a = model.gravityMp_a + model.accelSpeedMp_a;
                my_a = model.gravityMy_a + model.accelSpeedMy_a;

                // 等速區
                mp_c = (float)w * (float)model.moment_A / 1000f;
                my_c = (float)w * (float)model.moment_C / 1000f;

                // 減速區
                model.gravityMr_d = 0;
                model.gravityMp_d = w * model.moment_A / 1000;
                model.gravityMy_d = w * model.moment_C / 1000;
                model.accelSpeedMr_d = 0;
                model.accelSpeedMp_d = -1 * model.load * model.accelSpeed * model.moment_A / 1000;
                model.accelSpeedMy_d = -1 * model.load * model.accelSpeed * model.moment_C / 1000;
                mr_d = model.gravityMr_d + model.accelSpeedMr_d;
                mp_d = model.gravityMp_d + model.accelSpeedMp_d;
                my_d = model.gravityMy_d + model.accelSpeedMy_d;
            } else {
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
}
