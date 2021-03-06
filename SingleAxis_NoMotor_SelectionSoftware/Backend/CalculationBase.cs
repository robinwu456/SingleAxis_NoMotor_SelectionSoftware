﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using StrokeTooShortConverterLibraries;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CalculationBase {
        public bool isCheckStrokeTooShort = true;  // 是否判斷行程過短  
        public Converter.ModifyItem strokeTooShortModifyItem = Converter.ModifyItem.Vmax;  // 行程過短修正項目      

        // 資料庫
        public DataTable modelInfo = FileUtil.ReadCsv(Config.MODEL_INFO_FILENAME);
        public DataTable strokeRpm = FileUtil.ReadCsv(Config.STROKE_RPM_FILENAME);
        public DataTable momentData = FileUtil.ReadCsv(Config.MOMENT_FILENAME);
        public DataTable motorInfo = FileUtil.ReadCsv(Config.MOTOR_INFO_FILENAME);
        public DataTable reducerInfo = FileUtil.ReadCsv(Config.REDUCER_INFO_FILENAME);
        public DataTable beltInfo = FileUtil.ReadCsv(Config.BELT_INFO_FILENAME);

        // 套用新扭矩公式的型號
        public string[] beltModels = { "ETB10", "ETB14M", "ETB17M", "ETB22M",
                                       "ECB10", "ECB14", "ECB17", "ECB22", };

        public List<Model> GetAllModels(Condition condition) {
            List<Model> models = new List<Model>();
            // 搜尋荷重
            foreach (DataRow row in modelInfo.Rows) {
                Model model = new Model();
                model.name = row["Model"].ToString();
                model.lead = Convert.ToDouble(row["Lead"].ToString());
                if (condition.reducerRatio.Keys.Contains(model.name))
                    model.lead = Convert.ToDouble((model.lead / (float)condition.reducerRatio[model.name]).ToString("#0.00"));
                // 安裝方式
                model.supportedSetup = row["Setup"].ToString().Split('&').ToList().Select(setup => (Model.SetupMethod)Convert.ToInt32(setup)).ToList();
                // 使用環境
                model.useEnvironment = (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString());
                // 機構型態
                model.modelType = (Model.ModelType)Convert.ToInt32(row["Type"].ToString());

                // 力舉參數
                model.c = Convert.ToDouble(row["C"].ToString());
                model.mr_C = Convert.ToDouble(row["MR_C"].ToString());
                model.mp_C = Convert.ToDouble(row["MP_C"].ToString());
                model.my_C = Convert.ToDouble(row["MY_C"].ToString());
                model.dynamicLoadRating = Convert.ToInt32(row["DynamicLoadRating"].ToString());
                model.outerDiameter = Convert.ToInt32(row["OuterDiameter"].ToString());

                // 重複定位精度
                model.repeatability = Convert.ToDouble(row["Repeatability"].ToString());

                // 皮帶資訊
                if (beltModels.Any(m => model.name.StartsWith(m))) {
                    Func<DataRow, bool> con = con = x => x["Model"].ToString().Equals(model.name);
                    var beltInfoRows = beltInfo.Rows.Cast<DataRow>().Where(con);
                    // 主動輪
                    model.mainWheel = new BeltWheel(
                        beltInfoRows.Select(x => Convert.ToDouble(x["主動輪徑"].ToString())).First(),
                        beltInfoRows.Select(x => Convert.ToDouble(x["主動輪寬"].ToString())).First()
                    );
                    // 從動輪1
                    model.subWheel1 = new SubBeltWheel(
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪1輪徑"].ToString())).First(),
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪1輪寬"].ToString())).First()
                    );
                    // 從動輪2
                    model.subWheel2 = new SubBeltWheel(
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪2輪徑"].ToString())).First(),
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪2輪寬"].ToString())).First()
                    );
                    // 從動輪3
                    model.subWheel3 = new SubBeltWheel(
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪3輪徑"].ToString())).First(),
                        beltInfoRows.Select(x => Convert.ToDouble(x["從動輪3輪寬"].ToString())).First()
                    );
                    // 負載慣量與力矩比
                    model.loadInertiaMomentRatio = beltInfoRows.Select(x => Convert.ToDouble(x["負載慣量與力矩比"].ToString())).First();
                    // 皮帶寬
                    model.beltWidth = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶寬"].ToString())).First();
                    // 皮帶長度
                    model.beltLength = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶長度"].ToString())).First();
                    // 皮帶容許拉力
                    model.beltAllowableTension = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶容許拉力"].ToString())).First();
                }

                models.Add(model);
            }
            return models;
        }

        // 力矩值基本驗證
        public void VerifyMomentParam(int load_A, int load_B, int load_C) {
            string errorMsg = "";

            // 都不為0驗證
            if (load_A == 0 && load_B == 0 && load_C == 0)
                throw new Exception(CustomExtensions.GetLang("MomentLimit_0"));

            // 驗證可以是0且不可小於10
            errorMsg = CustomExtensions.GetLang("MomentLimit_10");
            if (load_A != 0 && load_A < 10)
                throw new Exception(string.Format(errorMsg, "A"));
            if (load_B != 0 && load_B < 10)
                throw new Exception(string.Format(errorMsg, "B"));
            if (load_C != 0 && load_C < 10)
                throw new Exception(string.Format(errorMsg, "C"));
        }

        // 取最大力矩值
        public int GetMaxMomentParam(string model, double lead, Model.SetupMethod setupMethod, Model.Moment moment) {
            // 減速比換算
            //double newLead = (float)lead / (float)reducerRatio;
            //newLead = Convert.ToDouble(newLead.ToString("#0.00"));

            try {
                var rows = momentData.Rows.Cast<DataRow>().Where(row => row["Model"].ToString() == model &&
                                                                        Convert.ToDouble(row["Lead"].ToString()) == lead &&
                                                                        Convert.ToInt32(row["Setup"].ToString()) == (int)setupMethod
                                                                            );
                return Convert.ToInt32(rows.First()[moment.ToString()]);
            } catch (Exception ex) {
                throw new Exception(string.Format("1|力矩表搜尋不到{0}導程{1}", model, lead));
            }
        }

        public double GetVmax_ms(string model, double lead, int reducer, int stroke) {
            try {
                return GetVmax_mms(model, lead, reducer, stroke) / 1000f;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        public double GetVmax_mms(string model, double lead, int reducer, int stroke) {
            try {
                // 依照行程取RPM
                int rpm = GetRpmByStroke(model, lead, reducer, stroke);
                // 轉速換算Vmax
                return Math.Round(RPM_TO_MMS(rpm, lead), 2);    // 四捨五入取小數第一位
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        // 依照行程取RPM
        public int GetRpmByStroke(string model, double lead, int reducer, int stroke) {
            // 取同型號集合
            IEnumerable<DataRow> strokeRpms = strokeRpm.Rows.Cast<DataRow>()
                                                            .Where(row => row["Model"].ToString() == model &&
                                                                          Convert.ToDouble(row["Lead"].ToString()) == (int)Math.Round(lead * reducer, 0));
            try {
                // 依照行程取RPM
                int strokeRpm = Convert.ToInt32(strokeRpms.First(row => Convert.ToInt32(row["Stroke"].ToString()) >= stroke)["RPM"]);
                //int vMaxRpm = MMS_TO_RPM(vMax, lead);
                //return Math.Min(vMaxRpm, strokeRpm);
                return strokeRpm;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                //throw new Exception(string.Format("1|{0}導程{1} 行程過大，搜尋不到RPM", model, lead));
                return Convert.ToInt32(strokeRpms.Last()["RPM"].ToString());
            }
        }

        // 依照vMax取RPM
        public int GetRpmByMMS(double lead, double mms) {
            try {
                // 依照限速度取RPM
                int vMaxRpm = MMS_TO_RPM(mms, lead);
                return vMaxRpm;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                //throw new Exception(string.Format("1|{0}導程{1} 行程過大，搜尋不到RPM", model, lead));
                return -1;
            }
        }

        public double RPM_TO_MMS(int rpm, double lead) {
            return (double)lead * (double)rpm / 60f;
        }

        public int MMS_TO_RPM(double mms, double lead) {
            return (int)(mms * 60f / (float)lead);
        }

        public int GetMaxStroke(string model, double lead, int reducer) {
            IEnumerable<int> strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => row["Model"].ToString() == model &&
                                                                                   Convert.ToDouble(row["Lead"].ToString()) == (int)Math.Round(lead * reducer, 0))
                                                                     .Select(row => Convert.ToInt32(row["Stroke"].ToString()));
            return strokes.Max();
        }

        public double GetMaxLoad(string model, double lead, Condition conditions) {
            // 取最大荷重
            double maxLoad = -1;
            string data = "";
            //lead = conditions.reducerRatio.Keys.Contains(model) ?
            //                        (int)Math.Round((lead * (double)conditions.reducerRatio[model]), 0) : lead;

            try {
                data = momentData.Rows.Cast<DataRow>()
                                             .Where(row => row["Model"].ToString() == model &&
                                                           Convert.ToDouble(row["Lead"].ToString()) == lead &&
                                                           Convert.ToInt32(row["Setup"].ToString()) == (int)conditions.setupMethod)
                                             .Select(row => row["MaxLoad"].ToString())
                                             .First();
                if (!string.IsNullOrEmpty(data))
                    maxLoad = Convert.ToDouble(data);
            } catch (Exception ex) {
                Console.WriteLine(ex);
                //throw new Exception("error model: " + model + ", " + ex);
            }
            return maxLoad;
        }

        public double Get_P(double w, double mr, double mp, double my, double mr_C, double mp_C, double my_C, double c) {
            double P = w / c +
                       Math.Abs(mr / mr_C) +
                       Math.Abs(mp / mp_C) +
                       Math.Abs(my / my_C);
            return P;
        }

        public double Get_Fw(double vMax) {
            double p1 = vMax < 0.25 ?
                                    -1f * (((1.5 - 1f) / (0.25 - 0f) * (0.25 - vMax)) - 1.5)
                                    : 1;
            double p2 = vMax >= 0.25 && vMax < 1 ?
                                                 -1 * (((2f - 1.5) / (1f - 0.25) * (1f - vMax)) - 2f)
                                                 : 1;
            double p3 = vMax >= 1 && vMax < 2 ?
                                              -1f * ((3.5 - 2f) / (2f - 1f) * (2f - vMax) - 3.5)
                                              : 1;
            double p4 = vMax >= 2 ? 3.5 : 1;

            return p1 * p2 * p3 * p4;
        }

        // 1分鐘最多可以跑多少趟
        public int GetMaxCountPerMinute(Model model, Condition conditions) {
            double _vMax = 0;
            if (conditions.vMaxCalcMode == Condition.CalcVmax.Max) {
                if (beltModels.Any(m => model.name.StartsWith(m))) {
                    _vMax = GetBeltVmax(model.name, model.lead, 1, conditions.stroke, model.mainWheel, model.subWheel1, model.subWheel2);
                } else {
                    if (conditions.reducerRatio.Keys.Contains(model.name))
                        _vMax = GetVmax_mms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
                    else
                        _vMax = GetVmax_mms(model.name, model.lead, 1, conditions.stroke);
                }
            } else if (conditions.vMaxCalcMode == Condition.CalcVmax.Custom) {
                if (conditions.reducerRatio.Keys.Contains(model.name))
                    _vMax = conditions.vMax > GetVmax_mms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke) ? GetVmax_ms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke) : conditions.vMax; // 驗證最大Vmax
                else
                    _vMax = conditions.vMax > GetVmax_mms(model.name, model.lead, 1, conditions.stroke) ? GetVmax_ms(model.name, model.lead, 1, conditions.stroke) : conditions.vMax; // 驗證最大Vmax
            }

            double _accelSpeed = (float)conditions.accelSpeed / 1000f;

            //double accelTime = _vMax / _accelSpeed;
            double accelTime = 0;
            if (conditions.accelSpeed != 0) {
                conditions.accelTime = conditions.vMax / conditions.accelSpeed;
            } else {
                accelTime = conditions.accelTime;
            }

            double decelTime = accelTime;
            double constantTime = ((2f * (float)conditions.stroke / 1000f / _vMax) - accelTime - decelTime) / 2f;
            // 單趟來回需要的秒數
            //double totalTime = (accelTime + constantTime + decelTime + conditions.stopTime) * 2f;
            double totalTime = (accelTime + constantTime + decelTime) * 2f;

            int maxCountPerMinute = (int)(60f / totalTime);

            return maxCountPerMinute;
        }

        // 取馬達資訊
        public (double ratedTorque, double maxTorque, double rotateInertia) GetMotorParams(int power) {
            Func<DataRow, bool> con = row => Convert.ToInt32(row["Power"].ToString()) == power;
            double ratedTorque = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["RatedTorque"].ToString())).First();
            double maxTorque = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["MaxTorque"].ToString())).First();
            double rotateInertia = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["RotateInertia"].ToString())).First();
            return (ratedTorque, maxTorque, rotateInertia);
        }

        public bool IsContainsReducerRatio(string model) {
            return reducerInfo.Rows.Cast<DataRow>().Any(row => row["Model"].ToString() == model);
        }

        // 皮帶Vmax(m/s)
        public double GetBeltVmax(string model, double lead, int reducer, int stroke, BeltWheel mainWheel, SubBeltWheel subWheel1, SubBeltWheel subWheel2) {
            // 依照行程取RPM
            int rpm = GetRpmByStroke(model, lead, reducer, stroke);
            double reducerRpmRatio = subWheel1.diameter / mainWheel.diameter;
            double subWheelRpm = (int)(rpm / reducerRpmRatio);
            double vMax_belt = Math.PI * subWheel2.diameter * (subWheelRpm / 60) / 1000;
            return vMax_belt;
        }

        // 圖表點資訊
        public List<PointF> GetChartPoints(Condition conditions) {
            //if (isCheckStrokeTooShort)
            //    // 行程過短驗證
            //    conditions.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);

            //if (isCheckStrokeTooShort) {
            //    // 行程過短驗證
            //    if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
            //        conditions.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);
            //    else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed)
            //        conditions.accelSpeed = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);
            //}

            double accelTime = conditions.vMax / conditions.accelSpeed;
            double decelTime = accelTime;
            //double constantTime = ((2f * (float)conditions.stroke / conditions.vMax) - conditions.accelTime - conditions.decelTime) / 2f;
            double constantTime = ((2f * (float)conditions.stroke / conditions.vMax) - accelTime - decelTime) / 2f;

            List<PointF> points = new List<PointF>() { new PointF(0, 0) };
            // 加速時間
            points.Add(new PointF((float)accelTime, (float)conditions.vMax));
            // 等速時間
            points.Add(new PointF((float)accelTime + (float)constantTime, (float)conditions.vMax));
            // 減速時間
            points.Add(new PointF((float)accelTime + (float)constantTime + (float)decelTime, 0));
            // 停等時間
            points.Add(new PointF((float)accelTime + (float)constantTime + (float)decelTime + (float)conditions.stopTime, 0));

            return points;
        }
        public (
            double accelTime,
            double constantTime,
            double runTime,
            double accelSpeed,
            double maxSpeed,
            double cycleTime
        ) GetChartInfo(Condition conditions) {
            //if (isCheckStrokeTooShort)
            //    // 行程過短驗證
            //    conditions.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);

            //if (isCheckStrokeTooShort) {
            //    // 行程過短驗證
            //    if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
            //        conditions.vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);
            //    else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed)
            //        conditions.accelSpeed = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)conditions.vMax, (int)conditions.accelSpeed, (int)conditions.stroke);
            //}

            double accelTime = conditions.vMax / conditions.accelSpeed;
            double decelTime = accelTime;
            double constantTime = ((2f * (float)conditions.stroke / conditions.vMax) - accelTime - decelTime) / 2f;

            double runTime = accelTime + constantTime + decelTime;
            double cycleTime = runTime * 2;

            accelTime = Convert.ToDouble(accelTime.ToString("0.000"));
            decelTime = Convert.ToDouble(decelTime.ToString("0.000"));
            constantTime = Convert.ToDouble(constantTime.ToString("0.000"));
            runTime = Convert.ToDouble(runTime.ToString("0.000"));
            conditions.vMax = Convert.ToDouble(conditions.vMax.ToString("0.000"));
            cycleTime = Convert.ToDouble(cycleTime.ToString("0.000"));

            return (accelTime, constantTime, runTime, conditions.accelSpeed, conditions.vMax, cycleTime);
        }

        public int GetMaxAccelSpeed(string model, double lead, int reducer, int stroke) {
            int rpm = GetRpmByStroke(model, lead, reducer, stroke);
            double vMax = (lead * (double)rpm) / 60f;
            double repeatability = Convert.ToDouble(modelInfo.Rows.Cast<DataRow>().First(row => row["Model"].ToString() == model)["Repeatability"].ToString());
            double minAccelTime = repeatability <= 0.01 ? 0.2 : 0.4;
            int maxAccelSpeed = (int)(vMax / minAccelTime);
            return maxAccelSpeed;
        }
    }
}
