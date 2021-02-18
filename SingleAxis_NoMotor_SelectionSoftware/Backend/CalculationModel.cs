using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrokeTooShortConverterLibraries;

namespace SingleAxis_NoMotor_SelectionSoftware {
    class CalculationModel : CalculationBase {
        // 滑軌壽命計算
        protected long GetSlideTrackEstimatedLife(Model model, Condition condition) {
            // 力舉參數驗證
            VerifyMomentParam(condition.moment_A, condition.moment_B, condition.moment_C);
            if (condition.isMomentLimitByCatalog) {
                if (condition.setupMethod == Model.SetupMethod.Horizontal) {
                    model.moment_A = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.A);
                    model.moment_B = 0;
                    model.moment_C = 0;
                } else if (condition.setupMethod == Model.SetupMethod.WallHang) {
                    model.moment_A = 0;
                    model.moment_B = 0;
                    model.moment_C = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.C);
                } else if (condition.setupMethod == Model.SetupMethod.Vertical) {
                    model.moment_A = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.A);
                    model.moment_B = 0;
                    model.moment_C = 0;
                }
            } else {
                model.moment_A = condition.moment_A;
                model.moment_B = condition.moment_B;
                model.moment_C = condition.moment_C;
            }

            if (condition.vMaxCalcMode == Condition.CalcVmax.Max) {
                if (condition.reducerRatio.Keys.Contains(model.name))
                    model.vMax = GetVmax_ms(model.name, model.lead, condition.reducerRatio[model.name], condition.stroke);
                else
                    model.vMax = GetVmax_ms(model.name, model.lead, 1, condition.stroke);
            } else if (condition.vMaxCalcMode == Condition.CalcVmax.Custom) {
                model.vMax = condition.vMax / 1000f;

                // RPM驗證
                int strokeRpm;
                int vMaxRpm = GetRpmByMMS(model.lead, model.vMax * 1000);
                if (condition.reducerRatio.Keys.Contains(model.name))
                    strokeRpm = GetRpmByStroke(model.name, model.lead, condition.reducerRatio[model.name], condition.stroke);
                else
                    strokeRpm = GetRpmByStroke(model.name, model.lead, 1, condition.stroke);
                model.rpm = Math.Min(strokeRpm, vMaxRpm);
                model.vMax = RPM_TO_MMS(model.rpm, model.lead) / 1000f;
            }

            // 取最高線速度
            if (condition.reducerRatio.Keys.Contains(model.name))
                model.vMax_max = GetVmax_mms(model.name, model.lead, condition.reducerRatio[model.name], condition.stroke);
            else
                model.vMax_max = GetVmax_mms(model.name, model.lead, 1, condition.stroke);

            // 最大行程驗證
            model.maxStroke = GetMaxStroke(model.name, model.lead, condition.reducerRatio.Keys.Contains(model.name) ? condition.reducerRatio[model.name] : 1);
            model.stroke = condition.stroke > model.maxStroke ? model.maxStroke : condition.stroke;
            model.accelSpeed = condition.accelSpeed / 1000f;
            model.load = condition.load;
            // 最大荷重驗證
            model.maxLoad = GetMaxLoad(model.name, model.lead, condition);
            if (model.maxLoad != -1 && model.load > model.maxLoad)
                model.load = model.maxLoad;

            if (model.accelSpeed != 0) {
                model.accelTime = model.vMax / model.accelSpeed;
            } else {
                model.accelTime = condition.accelTime;
            }

            if (isCheckStrokeTooShort) {
                // 行程過短驗證
                if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
                    model.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
                else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed) {
                    model.accelTime = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
                }
            }
            // 小數點位數修正
            model.accelSpeed = model.vMax / model.accelTime;
            model.accelSpeed = Convert.ToDouble(model.accelSpeed.ToString("#0.000"));
            model.vMax = Convert.ToDouble(model.vMax.ToString("#0.000"));

            model.decelTime = model.accelTime;
            model.constantTime = ((2f * (float)model.stroke / 1000f / model.vMax) - model.accelTime - model.decelTime) / 2f;
            model.moveTime = Convert.ToDouble((model.accelTime + model.constantTime + model.decelTime).ToString("#0.000"));

            CalcMoment calcMoment = new CalcMoment(model, condition.setupMethod);
            model.w = calcMoment.w;
            model.mr = calcMoment.mr;

            // 加速區                        
            model.mp_a = calcMoment.mp_a;
            model.my_a = calcMoment.my_a;
            model.p_a = Get_P(model.w, model.mr, model.mp_a, model.my_a, model.mr_C, model.mp_C, model.my_C, model.c);

            // 等速區
            model.mp_c = calcMoment.mp_c;
            model.my_c = calcMoment.my_c;
            model.p_c = Get_P(model.w, model.mr, model.mp_c, model.my_c, model.mr_C, model.mp_C, model.my_C, model.c);

            // 減速區
            model.mp_d = calcMoment.mp_d;
            model.my_d = calcMoment.my_d;
            model.p_d = Get_P(model.w, model.mr, model.mp_d, model.my_d, model.mr_C, model.mp_C, model.my_C, model.c);

            // 行程參數            
            model.accelDistance = Math.Pow(model.vMax, 2) / (2 * model.accelSpeed) * 1000;
            model.constantDistance = model.vMax * model.constantTime * 1000;
            model.decelDistance = model.accelDistance;

            model.pm = Math.Pow((Math.Pow(model.p_a, 3) * model.accelDistance +
                                       Math.Pow(model.p_c, 3) * model.constantDistance +
                                       Math.Pow(model.p_d, 3) * model.decelDistance) /
                                       (float)model.stroke, (float)1 / (float)3) * model.c;

            model.fw = Get_Fw(model.vMax);

            model.slideTrackServiceLifeDistance = (long)Math.Round(Math.Pow(model.c / (model.pm * model.fw), 3) * 10000, 0);

            return model.slideTrackServiceLifeDistance;
        }
    }
}
