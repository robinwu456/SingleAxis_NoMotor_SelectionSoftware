﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrokeTooShortConverterLibraries;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CalculationModel : CalculationBase {

        // 滑軌壽命計算
        protected long GetSlideTrackEstimatedLife(Model model, Condition condition) {
            //if (model.name == "GTH5" &&
            //    model.lead == 20 &&
            //    condition.setupMethod == Model.SetupMethod.水平 &&
            //    condition.load == 10 &&
            //    condition.moment_A == 100 &&
            //    condition.moment_B == 0 &&
            //    condition.moment_C == 0
            //    )
            //    Console.WriteLine(1);

            if (!condition.modelType.IsRodType())
                if (condition.calcMode != Condition.CalcMode.Test)
                    // 力舉參數驗證
                    VerifyMomentParam(condition.moment_A, condition.moment_B, condition.moment_C);

            if (condition.isMomentLimitByCatalog) {
                if (condition.setupMethod == Model.SetupMethod.水平) {
                    model.moment_A = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.A);
                    model.moment_B = 0;
                    model.moment_C = 0;
                } else if (condition.setupMethod == Model.SetupMethod.橫掛) {
                    model.moment_A = 0;
                    model.moment_B = 0;
                    model.moment_C = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.C);
                } else if (condition.setupMethod == Model.SetupMethod.垂直) {
                    model.moment_A = GetMaxMomentParam(model.name, model.lead, condition.setupMethod, Model.Moment.A);
                    model.moment_B = 0;
                    model.moment_C = 0;
                }
            } else {
                model.moment_A = condition.moment_A;
                model.moment_B = condition.moment_B;
                model.moment_C = condition.moment_C;
            }
            // 推桿式力矩都為10
            if (condition.modelType.IsRodType()) {
                model.moment_A = 0;
                model.moment_B = 0;
                model.moment_C = 0;
            }

            // 移動資訊計算
            VerifyMoveInfo(model, condition);

            CalcMoment calcMoment = new CalcMoment(model, condition.setupMethod);
            model.w = calcMoment.w;
            model.mr = calcMoment.mr;

            // 加速區                        
            model.mp_a = calcMoment.mp_a;
            model.my_a = calcMoment.my_a;
            model.mr_a = calcMoment.mr_a;
            if (model.isUseBaltCalc)
                model.p_a = Get_P(model.w, model.mr_a, model.mp_a, model.my_a, model.mr_C, model.mp_C, model.my_C, model.c);
            else
                model.p_a = Get_P(model.w, model.mr, model.mp_a, model.my_a, model.mr_C, model.mp_C, model.my_C, model.c);

            // 等速區
            model.mp_c = calcMoment.mp_c;
            model.my_c = calcMoment.my_c;
            model.p_c = Get_P(model.w, model.mr, model.mp_c, model.my_c, model.mr_C, model.mp_C, model.my_C, model.c);

            // 減速區
            model.mp_d = calcMoment.mp_d;
            model.my_d = calcMoment.my_d;
            model.mr_d = calcMoment.mr_d;
            if (model.isUseBaltCalc)
                model.p_d = Get_P(model.w, model.mr_d, model.mp_d, model.my_d, model.mr_C, model.mp_C, model.my_C, model.c);
            else
                model.p_d = Get_P(model.w, model.mr, model.mp_d, model.my_d, model.mr_C, model.mp_C, model.my_C, model.c);

            // 行程參數            
            model.accelDistance = Math.Pow(model.vMax, 2) / (2 * model.accelSpeed) * 1000;
            model.constantDistance = model.vMax * model.constantTime * 1000;
            model.decelDistance = model.accelDistance;

            model.pmSlide = Math.Pow((Math.Pow(model.p_a, 3) * model.accelDistance +
                                       Math.Pow(model.p_c, 3) * model.constantDistance +
                                       Math.Pow(model.p_d, 3) * model.decelDistance) /
                                       (float)model.stroke, (float)1 / (float)3) * model.c;

            model.fwSlide = Get_Fw(model.vMax);

            model.slideTrackServiceLifeDistance = (long)Math.Round(Math.Pow(model.c / (model.pmSlide * model.fwSlide), 3) * 10000, 0);

            // 力矩警示驗證
            VerifyMomentAlarm(model, condition.setupMethod);

            // 荷重為0時，壽命無限大
            if (model.load == 0)
                model.slideTrackServiceLifeDistance = 999999999;

            return model.slideTrackServiceLifeDistance;
        }        

        // 螺桿壽命計算
        protected long GetScrewEstimatedLife(Model model, Condition condition) {
            //// 使用者key
            //if (condition.vMaxCalcMode == Condition.CalcVmax.Max)
            //    model.vMax = GetVmax_ms(model, model.lead, condition.stroke);
            //else if (condition.vMaxCalcMode == Condition.CalcVmax.Custom)
            //    model.vMax = condition.vMax / 1000f;

            //// RPM驗證
            //if (condition.calcMode != Condition.CalcMode.Test) {
            //    int strokeRpm;
            //    int vMaxRpm = GetRpmByMMS(model.lead, model.vMax * 1000);
            //    strokeRpm = GetRpmByStroke(model.name, model.lead, condition.stroke);
            //    model.rpm = Math.Min(strokeRpm, vMaxRpm);
            //    model.vMax = RPM_TO_MMS(model.rpm, model.lead) / 1000f;
            //}

            //// 取最高線速度
            //model.vMax_max = GetVmax_mms(model, model.lead, condition.stroke);

            //// 最大行程驗證
            //model.maxStroke = GetMaxStroke(model.name, model.lead);
            //if (condition.calcMode == Condition.CalcMode.Test)
            //    model.stroke = condition.stroke;
            //else
            //    model.stroke = condition.stroke > model.maxStroke ? model.maxStroke : condition.stroke;
            //model.accelSpeed = condition.accelSpeed / 1000f;
            //model.load = condition.load;
            //// 最大荷重驗證
            //if (condition.curCheckedModel.model == "") {
            //    if (condition.calcMode != Condition.CalcMode.CalcMax) {
            //        double maxLoad = GetMaxLoad(model.name, model.lead, condition);
            //        if (maxLoad != int.MaxValue && model.load > maxLoad)
            //            model.load = maxLoad;
            //    }
            //}

            //if (model.accelSpeed != 0)
            //    model.accelTime = model.vMax / model.accelSpeed;
            //else
            //    model.accelTime = condition.accelTime;

            //if (isCheckStrokeTooShort) {
            //    // 行程過短驗證
            //    if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
            //        model.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
            //    else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed)
            //        model.accelTime = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
            //}
            //// 小數點位數修正
            //model.accelSpeed = model.vMax / model.accelTime;
            //model.accelSpeed = Convert.ToDouble(model.accelSpeed.ToString("#0.000"));

            //model.decelTime = model.accelTime;
            //model.constantTime = ((2f * (float)model.stroke / 1000f / model.vMax) - model.accelTime - model.decelTime) / 2f;
            //model.moveTime = Convert.ToDouble((model.accelTime + model.constantTime + model.decelTime + model.stopTime).ToString("#0.000"));

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

            model.fwScrew = 1.2;
            model.pmScrew = Math.Pow((Math.Pow(model.equivalentLoad_accel, 3) * (model.rpm / 2) * model.accelTime +
                                 Math.Pow(model.equivalentLoad_constant, 3) * model.rpm * model.constantTime +
                                 Math.Pow(model.equivalentLoad_decel, 3) * (model.rpm / 2) * model.decelTime) /
                                 (model.rpm / 2 * model.accelTime +
                                 model.rpm * model.constantTime +
                                 model.rpm / 2 * model.accelTime),
                       (float)1 / (float)3);

            model.screwServiceLifeDistance = (long)(Math.Pow(model.dynamicLoadRating / (model.pmScrew * model.fwScrew), 3) * 1000000 * ((float)model.lead / (1000f * 1000f)));

            return model.screwServiceLifeDistance;
        }

        // 扭矩計算
        protected (bool is_tMax_OK, bool is_tRms_OK) TorqueConfirm(Model model, Condition condition) {            
            // 扭矩確認
            TorqueConfirm torqueConfirm;
            if (model.isUseBaltCalc && model.beltCalcType == Model.BeltCalcType.減速機構)
                torqueConfirm = new TorqueConfirm_Belt_減速機構(model, condition);
            else if (model.isUseBaltCalc && model.beltCalcType == Model.BeltCalcType.減速機2)
                torqueConfirm = new TorqueConfirm_Belt_減速機2(model, condition);
            else if (model.isUseBaltCalc && model.beltCalcType == Model.BeltCalcType.減速機4)
                torqueConfirm = new TorqueConfirm_Belt_減速機4(model, condition);
            else if (model.isUseBaltCalc && model.beltCalcType == Model.BeltCalcType.直接驅動)
                torqueConfirm = new TorqueConfirm_Belt_直接驅動(model, condition);
            else
                torqueConfirm = new TorqueConfirm_Screw(model, condition);

            torqueConfirm.Calc();

            return (model.is_tMax_OK, model.is_tRms_OK);
        }

        // 計算壽命時間
        protected (int year, int month, int day) GetServiceLifeTime(Model model, Condition conditions) {
            int year = 0;
            int month = 0;
            int day = 0;

            // 驗證使用頻率
            double maxCountPerMinute = GetMaxCountPerMinute(model, conditions);
            double verifyCountPerMinute = conditions.useFrequence.countPerMinute > maxCountPerMinute ? maxCountPerMinute : conditions.useFrequence.countPerMinute;
            if (conditions.useFrequence.countPerMinute > maxCountPerMinute)
                return (-1, -1, -1);
            if (conditions.useFrequence.hourPerDay > 24)
                conditions.useFrequence.hourPerDay = 24;
            if (conditions.useFrequence.dayPerYear > 365)
                conditions.useFrequence.dayPerYear = 365;

            int distancePerCount = model.stroke * 2;
            double useDistancePerYear = ((float)verifyCountPerMinute * (float)distancePerCount *
                                     (float)conditions.useFrequence.hourPerDay * 60f *
                                     (float)conditions.useFrequence.dayPerYear) / (1000f * 1000f);

            year = (int)(model.serviceLifeDistance / useDistancePerYear);
            int fullDay = (int)((int)(model.serviceLifeDistance - year * useDistancePerYear) / ((float)conditions.useFrequence.hourPerDay * 60f * (float)verifyCountPerMinute * distancePerCount / 1000f / 1000f));
            month = fullDay / 30;
            day = fullDay - month * 30;

            return (year, month, day);
        }

        // 力矩警示驗證
        private void VerifyMomentAlarm(Model model, Model.SetupMethod setupMethod) {
            if (model.modelType == Model.ModelType.Y || model.modelType == Model.ModelType.YD || model.modelType == Model.ModelType.YL) {
                model.isMomentVerifySuccess = true;
                return;
            }

            //int maxMomentA = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.A);
            //int maxMomentB = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.B);
            //int maxMomentC = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.C);
            //double verifyValueA = (float)model.moment_A / (float)maxMomentA;
            //double verifyValueB = (float)model.moment_B / (float)maxMomentB;
            //double verifyValueC = (float)model.moment_C / (float)maxMomentC;
            //if (double.IsNaN(verifyValueA))
            //    verifyValueA = 0;
            //if (double.IsNaN(verifyValueB))
            //    verifyValueB = 0;
            //if (double.IsNaN(verifyValueC))
            //    verifyValueC = 0;
            //model.isMomentVerifySuccess = verifyValueA + verifyValueB + verifyValueC <= 1;

            model.isMomentVerifySuccess = true;
        }

        private void VerifyMoveInfo(Model model, Condition condition) {
            // 全選模式單位驗證
            if (condition.calcMode == Condition.CalcMode.CalcMax) {
                switch (condition.calcMaxUnit) {
                    case Condition.CalcMaxUnit.RPM:
                        if (model.modelType.IsBeltType()) {
                            model.vMax = GetBeltVmaxByRpm_ms(model.name, (int)model.vMax, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType) * 1000;
                        } else {
                            //if (IsContainsReducerRatio(model.name)) {
                            //    string dgvReducerRatioValue = formMain.cboReducerRatio.Text;
                            //    model.vMax = RPM_TO_MMS((int)model.vMax, Convert.ToDouble(formMain.cboLead.Text) / Convert.ToDouble(dgvReducerRatioValue));
                            //} else
                                model.vMax = RPM_TO_MMS((int)model.vMax, model.lead);
                        }
                        break;
                    case Condition.CalcMaxUnit.G:

                        break;
                }
            }

            // Vmax驗證
            if (condition.vMaxCalcMode == Condition.CalcVmax.Max) {
                if (model.isUseBaltCalc)
                    model.vMax = GetBeltVmax_ms(model.name, model.lead, condition.stroke, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType);
                else
                    model.vMax = GetVmax_ms(model, model.lead, condition.stroke);
            } else if (condition.vMaxCalcMode == Condition.CalcVmax.Custom) {
                model.vMax = condition.vMax / 1000f;

                // 非皮帶機構才判斷
                if (!model.isUseBaltCalc) {
                    if (condition.calcMode == Condition.CalcMode.Normal ||
                        (condition.calcMode == Condition.CalcMode.CalcMax && condition.isRpmLimitByStroke)) {
                        if (condition.calcMaxItem == Condition.CalcMaxItem.Vmax) {
                            // RPM驗證
                            int strokeRpm;
                            int vMaxRpm = GetRpmByMMS(model.lead, model.vMax * 1000);
                            strokeRpm = GetRpmByStroke(model.name, model.lead, condition.stroke);
                            model.rpm = Math.Min(strokeRpm, vMaxRpm);
                            model.vMax = RPM_TO_MMS(model.rpm, model.lead) / 1000f;
                        } else if (condition.calcMaxItem == Condition.CalcMaxItem.AccelSpeed || condition.calcMaxItem == Condition.CalcMaxItem.AccelTime) {
                            // Stroke驗證
                            model.rpm = GetRpmByStroke(model.name, model.lead, condition.stroke);
                            model.vMax = RPM_TO_MMS(model.rpm, model.lead) / 1000f;
                        }
                    }
                }
            }

            // 取最高線速度
            model.vMax_max = GetVmax_mms(model, model.lead, condition.stroke);

            // 最大行程驗證
            model.maxStroke = GetMaxStroke(model.name, model.lead);
            if (condition.calcMode == Condition.CalcMode.Test)
                model.stroke = condition.stroke;
            else
                model.stroke = condition.stroke > model.maxStroke ? model.maxStroke : condition.stroke;

            // 荷重驗證
            model.load = condition.load;            
            model.maxLoad = GetMaxLoad(model.name, model.lead, condition);
            if (condition.curCheckedModel.model == "")
                if (model.maxLoad != int.MaxValue && model.load > model.maxLoad)
                    model.load = model.maxLoad;

            // 加速度/加速時間
            if (condition.calcMode == Condition.CalcMode.Normal || condition.calcMode == Condition.CalcMode.Test) {
                model.accelSpeed = condition.accelSpeed / 1000f;
                model.accelTime = model.accelSpeed != 0 ? model.vMax / model.accelSpeed : condition.accelTime;
            } else if (condition.calcMode == Condition.CalcMode.CalcMax) {
                switch (condition.calcMaxItem) {
                    case Condition.CalcMaxItem.Vmax:
                        model.accelSpeed = Math.Pow(model.vMax * 1000, 2) / model.stroke;   // mm/s^2
                        model.accelTime = model.vMax / model.accelSpeed * 1000;             // s
                        break;
                    case Condition.CalcMaxItem.AccelSpeed:
                        model.accelSpeed = condition.accelSpeed;
                        if (condition.calcMaxUnit == Condition.CalcMaxUnit.G)
                            model.accelSpeed = model.accelSpeed * 9806;
                        if (!condition.isRpmLimitByStroke)
                            model.vMax = Math.Sqrt(model.accelSpeed * model.stroke) / 1000;     // m/s
                        model.accelTime = model.vMax / model.accelSpeed * 1000;             // s
                        break;
                    case Condition.CalcMaxItem.AccelTime:
                        model.accelTime = condition.accelTime;
                        if (!condition.isRpmLimitByStroke)
                            model.vMax = model.stroke / model.accelTime / 1000;
                        break;
                }
            }

            // 停等時間
            if (condition.stopTime != 0)
                model.stopTime = condition.stopTime;

            double recordVmax = model.vMax;

            // 行程過短驗證
            if (isCheckStrokeTooShort /*&& condition.calcMode != Condition.CalcMode.CalcMax*/) {                
                if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
                    model.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
                else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed)
                    model.accelTime = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
            }

            // rpm修正
            if (model.isUseBaltCalc)
                model.rpm = GetBeltRPM(model.name, model.vMax, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType);
            else
                model.rpm = MMS_TO_RPM(model.vMax * 1000, model.lead);
            if (condition.calcMode == Condition.CalcMode.Normal || condition.calcMode == Condition.CalcMode.Test)
                model.showRpm = recordVmax == model.vMax ? GetRpmByStroke(model.name, model.lead, model.stroke) : GetRpmByMMS(model.lead, model.vMax * 1000);
            else if (condition.calcMode == Condition.CalcMode.CalcMax)
                model.showRpm = model.rpm;

            // 小數點位數修正
            model.accelSpeed = model.vMax / model.accelTime;
            model.accelSpeed = Convert.ToDouble(model.accelSpeed.ToString("#0.000"));

            model.decelTime = model.accelTime;
            model.constantTime = ((2f * (float)model.stroke / 1000f / model.vMax) - model.accelTime - model.decelTime) / 2f;
            model.moveTime = Convert.ToDouble((model.accelTime + model.constantTime + model.decelTime + model.stopTime).ToString("#0.000"));

        }
    }
}
