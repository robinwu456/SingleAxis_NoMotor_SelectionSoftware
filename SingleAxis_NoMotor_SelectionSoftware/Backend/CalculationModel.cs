using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StrokeTooShortConverterLibraries;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CalculationModel : CalculationBase {        

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
            // 推桿式力矩都為10
            if (condition.modelType.ToString().Contains("推桿式")) {
                model.moment_A = 10;
                model.moment_B = 10;
                model.moment_C = 10;
            }

            if (condition.vMaxCalcMode == Condition.CalcVmax.Max) {
                if (beltModels.Any(m => model.name.StartsWith(m))) {
                    model.vMax = GetBeltVmax(model.name, model.lead, 1, condition.stroke, model.mainWheel, model.subWheel1, model.subWheel2);
                } else {
                    if (condition.reducerRatio.Keys.Contains(model.name))
                        model.vMax = GetVmax_ms(model.name, model.lead, condition.reducerRatio[model.name], condition.stroke);
                    else
                        model.vMax = GetVmax_ms(model.name, model.lead, 1, condition.stroke);
                }
            } else if (condition.vMaxCalcMode == Condition.CalcVmax.Custom) {
                model.vMax = condition.vMax / 1000f;

                // 非皮帶機構才判斷
                if (!beltModels.Any(m => model.name.StartsWith(m))) {
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
            //model.vMax = Convert.ToDouble(model.vMax.ToString("#0.000"));

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

            // 力矩警示驗證
            VerifyMomentAlarm(model, condition.setupMethod);

            return model.slideTrackServiceLifeDistance;
        }

        private void VerifyMomentAlarm(Model model, Model.SetupMethod setupMethod) {
            int maxMomentA = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.A);
            int maxMomentB = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.B);
            int maxMomentC = GetMaxMomentParam(model.name, model.lead, setupMethod, Model.Moment.C);
            double verifyValueA = (float)model.moment_A / (float)maxMomentA;
            double verifyValueB = (float)model.moment_B / (float)maxMomentB;
            double verifyValueC = (float)model.moment_C / (float)maxMomentC;
            if (double.IsNaN(verifyValueA))
                verifyValueA = 0;
            if (double.IsNaN(verifyValueB))
                verifyValueB = 0;
            if (double.IsNaN(verifyValueC))
                verifyValueC = 0;
            model.isMomentVerifySuccess = verifyValueA + verifyValueB + verifyValueC <= 1;
        }

        // 螺桿壽命計算
        protected long GetScrewEstimatedLife(Model model, Condition conditions) {
            // 使用者key
            if (conditions.vMaxCalcMode == Condition.CalcVmax.Max) {
                if (conditions.reducerRatio.Keys.Contains(model.name))
                    model.vMax = GetVmax_ms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
                else
                    model.vMax = GetVmax_ms(model.name, model.lead, 1, conditions.stroke);
            } else if (conditions.vMaxCalcMode == Condition.CalcVmax.Custom)
                model.vMax = conditions.vMax / 1000f;

            // RPM驗證
            int strokeRpm;
            int vMaxRpm = GetRpmByMMS(model.lead, model.vMax * 1000);
            if (conditions.reducerRatio.Keys.Contains(model.name))
                strokeRpm = GetRpmByStroke(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
            else
                strokeRpm = GetRpmByStroke(model.name, model.lead, 1, conditions.stroke);
            model.rpm = Math.Min(strokeRpm, vMaxRpm);
            model.vMax = RPM_TO_MMS(model.rpm, model.lead) / 1000f;

            // 取最高線速度
            if (conditions.reducerRatio.Keys.Contains(model.name))
                model.vMax_max = GetVmax_mms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
            else
                model.vMax_max = GetVmax_mms(model.name, model.lead, 1, conditions.stroke);

            // 最大行程驗證
            model.maxStroke = GetMaxStroke(model.name, model.lead, conditions.reducerRatio.Keys.Contains(model.name) ? conditions.reducerRatio[model.name] : 1);
            model.stroke = conditions.stroke > model.maxStroke ? model.maxStroke : conditions.stroke;
            model.accelSpeed = conditions.accelSpeed / 1000f;
            model.load = conditions.load;
            // 最大荷重驗證
            double maxLoad = GetMaxLoad(model.name, model.lead, conditions);
            if (maxLoad != -1 && model.load > maxLoad)
                model.load = maxLoad;

            if (model.accelSpeed != 0) {
                model.accelTime = model.vMax / model.accelSpeed;
            } else {
                model.accelTime = conditions.accelTime;
            }

            if (isCheckStrokeTooShort) {
                // 行程過短驗證

                if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
                    model.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
                else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed)
                    model.accelTime = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, model.vMax, model.accelTime, model.stroke);
            }
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
            if (conditions.setupMethod == Model.SetupMethod.Vertical)
                model.otherForce_accel = model.load * 9.8;
            model.equivalentLoad_accel = Math.Abs(model.rollingFriction_accel + model.accessoriesFriction_accel + model.inertialLoad_accel + model.otherForce_accel);

            // 等速區外力
            model.rollingFriction_constant = model.p_c * model.c * 0.003;
            model.inertialLoad_constant = 0;
            if (conditions.setupMethod == Model.SetupMethod.Vertical)
                model.otherForce_constant = model.load * 9.8;
            model.equivalentLoad_constant = Math.Abs(model.rollingFriction_constant + model.accessoriesFriction_constant + model.inertialLoad_constant + model.otherForce_constant);

            // 減速區外力
            model.rollingFriction_decel = model.p_d * model.c * 0.003;
            model.inertialLoad_decel = (model.load + 3) * model.accelSpeed * -1;
            if (conditions.setupMethod == Model.SetupMethod.Vertical)
                model.otherForce_decel = model.load * 9.8;
            model.equivalentLoad_decel = Math.Abs(model.rollingFriction_decel + model.accessoriesFriction_decel + model.inertialLoad_decel + model.otherForce_decel);

            // 停等區外力
            if (conditions.setupMethod == Model.SetupMethod.Vertical)
                model.otherForce_stop = model.load * 9.8;

            model.fw = 1.2;
            model.pm = Math.Pow((Math.Pow(model.equivalentLoad_accel, 3) * (model.rpm / 2) * model.accelTime +
                                 Math.Pow(model.equivalentLoad_constant, 3) * model.rpm * model.constantTime +
                                 Math.Pow(model.equivalentLoad_decel, 3) * (model.rpm / 2) * model.decelTime) /
                                 (model.rpm / 2 * model.accelTime +
                                 model.rpm * model.constantTime +
                                 model.rpm / 2 * model.accelTime),
                       (float)1 / (float)3);

            model.screwServiceLifeDistance = (long)(Math.Pow(model.dynamicLoadRating / (model.pm * model.fw), 3) * 1000000 * ((float)model.lead / (1000f * 1000f)));

            return model.screwServiceLifeDistance;
        }

        // 扭矩計算
        protected (bool is_tMax_OK, bool is_tRms_OK) TorqueConfirm(Model model, Condition conditions) {
            model.stopTime = conditions.stopTime;
            model.screwLength = model.stroke + 100;
            if (conditions.powerSelection == Condition.PowerSelection.Standard) {
                // 取得適用功率
                Func<DataRow, bool> GetLeadCondition = row => Convert.ToInt32(row["Lead"].ToString()) == model.lead;
                // 導程減速比轉換
                if (conditions.reducerRatio.Keys.Contains(model.name))
                    GetLeadCondition = row => Convert.ToInt32(row["Lead"].ToString()) == (int)Math.Round(model.lead * conditions.reducerRatio[model.name], 0);
                model.availablePowers = modelInfo.Rows.Cast<DataRow>().First(row => row["Model"].ToString() == model.name && GetLeadCondition(row))["Power"].ToString().Split('&').ToList()
                                                                      .Select(power => Convert.ToInt32(power)).ToList();
                // 取馬達參數
                model.usePower = model.availablePowers.Max();   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["Power"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RatedTorque"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["MaxTorque"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RotateInertia"]).First());
            } else if (conditions.powerSelection == Condition.PowerSelection.Custom) {
                //model.usePower = -1;
                model.ratedTorque = conditions.ratedTorque;
                model.maxTorque = conditions.maxTorque;
                model.rotateInertia = conditions.rotateInertia;
            } else if (conditions.powerSelection == Condition.PowerSelection.SelectedPower) {
                // 取馬達參數
                model.usePower = conditions.selectedPower;   // 適用馬達的最大瓦數
                //model.usePower = conditions.curSelectModel;   // 適用馬達的最大瓦數
                Func<DataRow, bool> con = x => Convert.ToInt32(x["Power"]).Equals(model.usePower);
                model.ratedTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RatedTorque"]).First());
                model.maxTorque = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["MaxTorque"]).First());
                model.rotateInertia = Convert.ToDouble(motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => row["RotateInertia"]).First());
            }

            if (beltModels.Any(m => model.name.StartsWith(m))) {
                // 馬達能力預估
                MotorConfirm_ETB(model);
                // 馬達最大扭矩確認
                MotorTorqueConfirm_ETB(model);
                // 皮帶最大扭矩確認
                BeltTorqueConfirm_ETB(model);
            } else {
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
                if (model.name.StartsWith("MK"))
                    model.tRmsSafeCoefficient = model.tMaxSafeCoefficient * conditions.reducerRatio[model.name] * 0.95;
                else {
                    model.tRms = Math.Pow((Math.Pow(model.torqueTotal_accel, 2) * model.accelTime +
                                     Math.Pow(model.torqueTotal_constant, 2) * model.constantTime +
                                     Math.Pow(model.torqueTotal_decel, 2) * model.decelTime +
                                     Math.Pow(model.torqueTotal_stop, 2) * model.stopTime) / (model.accelTime + model.constantTime + model.decelTime + model.stopTime), 0.5f);
                    model.tRmsSafeCoefficient = model.ratedTorque / model.tRms;
                }
                model.tRmsSafeCoefficient = Math.Round(model.tRmsSafeCoefficient, 2);
                model.is_tRms_OK = model.tRmsSafeCoefficient > Model.tRmsStandard;
            }

            return (model.is_tMax_OK, model.is_tRms_OK);
        }

        private void MotorConfirm_ETB(Model model) {
            // 皮帶輪加減速關係
            model.mainWheelRpm = model.rpm;
            model.reducerRpmRatio = model.subWheel1.diameter / model.mainWheel.diameter;
            model.subWheelRpm = (int)(model.rpm / model.reducerRpmRatio);

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
        // 馬達最大扭矩確認 ETB, ECB扭矩公式
        private void MotorTorqueConfirm_ETB(Model model) {
            // 軸向外力
            model.otherForce_accel = model.load * model.accelSpeed;
            model.otherForce_constant = 0;
            model.otherForce_decel = model.load * model.accelSpeed * -1;
            model.otherForce_stop = 0;
            model.forceTotal_accel = model.rollingFriction_accel + model.accessoriesFriction_belt_accel + model.otherForce_accel;
            model.forceTotal_constant = model.rollingFriction_constant + model.accessoriesFriction_belt_constant + model.otherForce_constant;
            model.forceTotal_decel = model.rollingFriction_decel + model.accessoriesFriction_belt_decel + model.otherForce_decel;
            model.forceTotal_stop = model.rollingFriction_stop + model.accessoriesFriction_belt_stop + model.otherForce_stop;

            // 加速區扭矩
            model.rotateInertia_loadMoving_subWheel = model.load * Math.Pow(model.vMax, 2) / Math.Pow(model.subWheelRpm * 2 * Math.PI / 60, 2);
            model.rotateInertia_loadMoving_motor = model.rotateInertia_loadMoving_subWheel / Math.Pow(model.reducerRpmRatio, 2);
            model.inertialTorque_accel = ((model.mainWheelRpm - 0) * 2 * Math.PI / 60) * model.rotateInertia_loadMoving_motor / model.accelTime;
            model.forceTorque_accel = (model.forceTotal_accel * ((model.mainWheel.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_accel = model.inertialTorque_accel + model.forceTorque_accel;

            // 等速區扭矩
            model.inertialTorque_constant = 0;
            model.forceTorque_constant = (model.forceTotal_constant * ((model.mainWheel.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_constant = model.inertialTorque_constant + model.forceTorque_constant;

            // 減速區扭矩
            model.inertialTorque_decel = (-1 * (model.mainWheelRpm - 0) * 2 * Math.PI / 60) * model.rotateInertia_loadMoving_motor / model.accelTime;
            model.forceTorque_decel = (model.forceTotal_decel * ((model.mainWheel.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_decel = model.inertialTorque_decel + model.forceTorque_decel;

            // 停置區扭矩
            model.inertialTorque_stop = 0;
            model.forceTorque_stop = (model.forceTotal_stop * ((model.mainWheel.diameter / 2) / 1000)) / Math.Pow(model.reducerRpmRatio, 2);
            model.torqueTotal_stop = model.inertialTorque_stop + model.forceTorque_stop;

            // T_max最大扭矩確認
            model.tMax = Math.Max(model.torqueTotal_accel, Math.Max(model.torqueTotal_constant, Math.Max(model.torqueTotal_decel, model.torqueTotal_stop)));
            model.tMaxSafeCoefficient = Math.Round(model.maxTorque / model.tMax, 2);
            model.is_tMax_OK = model.tMaxSafeCoefficient >= Model.tMaxStandard_beltMotor;
        }

        // 皮帶最大扭矩確認
        private void BeltTorqueConfirm_ETB(Model model) {
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

        // 計算壽命時間
        protected (int year, int month, int day) GetServiceLifeTime(Model model, Condition conditions) {
            int year = 0;
            int month = 0;
            int day = 0;

            // 驗證使用頻率
            int maxCountPerMinute = GetMaxCountPerMinute(model, conditions);
            int verifyCountPerMinute = conditions.useFrequence.countPerMinute > maxCountPerMinute ? maxCountPerMinute : conditions.useFrequence.countPerMinute;
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
    }
}
