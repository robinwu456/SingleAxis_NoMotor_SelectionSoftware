using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public DataTable beltInfo = FileUtil.ReadCsv(Config.BELT_INFO_FILENAME);
        public DataTable modelTypeInfo = FileUtil.ReadCsv(Config.MODEL_TYPE_INFO_FILENAME);
        public DataTable reducerRotateInertiaInfo = FileUtil.ReadCsv(Config.ReducerRotateInertia);

        public List<Model> GetAllModels(Condition condition) {
            List<Model> models = new List<Model>();
            // 搜尋荷重
            foreach (DataRow row in modelInfo.Rows) {
                Model model = new Model();
                model.name = row["型號"].ToString();
                model.lead = Convert.ToDouble(row["導程"].ToString());
                //if (condition.reducerRatio.Keys.Contains(model.name))
                //    model.lead = Convert.ToDouble((model.lead / (float)condition.reducerRatio[model.name]).ToString("#0.00"));
                // 安裝方式
                model.supportedSetup = row["安裝方式"].ToString().Split('&').ToList().Select(setup => (Model.SetupMethod)Enum.Parse(typeof(Model.SetupMethod), setup)).ToList();
                // 使用環境
                model.useEnvironment = (Model.UseEnvironment)Enum.Parse(typeof(Model.UseEnvironment), row["使用環境"].ToString());
                // 機構型態
                model.modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), row["型號類別"].ToString());

                // 力舉參數
                if (condition.c != 0) {
                    model.c = condition.c;
                    model.mr_C = condition.mr_C;
                    model.mp_C = condition.mp_C;
                    model.my_C = condition.my_C;
                    model.dynamicLoadRating = condition.dynamicLoadRating;
                    model.outerDiameter = condition.outerDiameter;
                } else {
                    model.c = Convert.ToDouble(row["C"].ToString());
                    model.mr_C = Convert.ToDouble(row["MR_C"].ToString());
                    model.mp_C = Convert.ToDouble(row["MP_C"].ToString());
                    model.my_C = Convert.ToDouble(row["MY_C"].ToString());
                    model.dynamicLoadRating = Convert.ToInt32(row["動額定負載"].ToString());
                    model.outerDiameter = Convert.ToInt32(row["外徑"].ToString());
                }

                // 重複定位精度
                model.repeatability = Convert.ToDouble(row["重複定位精度"].ToString());

                // 是否套用皮帶公式
                model.isUseBaltCalc = beltInfo.Rows.Cast<DataRow>().Select(info => info["型號"].ToString()).Contains(model.name);

                // 測試不判斷行程過短
                isCheckStrokeTooShort = condition.calcMode != Condition.CalcMode.Test;

                // 皮帶資訊
                if (model.isUseBaltCalc) {
                    if (condition.subWheel_P3 != null) {
                        model.mainWheel_P1 = condition.mainWheel_P1;
                        model.subWheel_P2 = condition.subWheel_P2;
                        model.subWheel_P3 = condition.subWheel_P3;
                        model.subWheel_P4 = condition.subWheel_P4;
                        model.beltWidth = condition.beltWidth;
                        model.beltLength = condition.beltLength;
                        model.beltUnitDensity = condition.beltUnitDensity;
                        model.beltAllowableTension = condition.beltAllowableTension;
                        model.beltCalcType = condition.beltCalcType;
                        model.reducerRotateInertia = condition.reducerRotateInertia;
                    } else {
                        Func<DataRow, bool> con = con = x => x["型號"].ToString().Equals(model.name);
                        var beltInfoRows = beltInfo.Rows.Cast<DataRow>().Where(con);
                        // 主動輪P1
                        model.mainWheel_P1 = new BeltWheel(
                            beltInfoRows.Select(x => Convert.ToDouble(x["主動輪P1輪徑"].ToString())).First(),
                            beltInfoRows.Select(x => Convert.ToDouble(x["主動輪P1輪寬"].ToString())).First()
                        );
                        // 從動輪P2
                        model.subWheel_P2 = new SubBeltWheel(
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P2輪徑"].ToString())).First(),
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P2輪寬"].ToString())).First()
                        );
                        // 從動輪P3
                        model.subWheel_P3 = new SubBeltWheel(
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P3輪徑"].ToString())).First(),
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P3輪寬"].ToString())).First()
                        );
                        // 從動輪P4
                        model.subWheel_P4 = new SubBeltWheel(
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P4輪徑"].ToString())).First(),
                            beltInfoRows.Select(x => Convert.ToDouble(x["從動輪P4輪寬"].ToString())).First()
                        );
                        //// 負載慣量與力矩比
                        //model.loadInertiaMomentRatio = beltInfoRows.Select(x => Convert.ToDouble(x["負載慣量與力矩比"].ToString())).First();
                        // 皮帶寬
                        model.beltWidth = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶寬"].ToString())).First();
                        // 皮帶長度
                        model.beltLength = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶長度"].ToString())).First();
                        // 皮帶單位密度
                        model.beltUnitDensity = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶單位密度"].ToString())).First();
                        // 皮帶容許拉力
                        model.beltAllowableTension = beltInfoRows.Select(x => Convert.ToDouble(x["皮帶容許拉力"].ToString())).First();
                        // 傳動方式
                        model.beltCalcType = (Model.BeltCalcType)Enum.Parse(typeof(Model.BeltCalcType), beltInfoRows.Select(x => x["傳動方式"].ToString()).First());
                        // 馬達尺寸
                        if (model.beltCalcType == Model.BeltCalcType.減速機2 || model.beltCalcType == Model.BeltCalcType.減速機4) {
                            if (condition.powerSelection == Condition.PowerSelection.Standard)
                                model.motorSize = beltInfoRows.Select(x => Convert.ToInt32(x["減速機外框尺寸"].ToString())).First();
                            else if (condition.powerSelection == Condition.PowerSelection.SelectedPower)
                                model.motorSize = Convert.ToInt32(motorInfo.Rows.Cast<DataRow>().First(x => Convert.ToInt32(x["馬達瓦數"].ToString()) == condition.selectedPower)["減速機外框尺寸"].ToString());
                        }
                    }
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
                var rows = momentData.Rows.Cast<DataRow>().Where(row => row["型號"].ToString() == model &&
                                                                        Convert.ToDouble(row["導程"].ToString()) == lead &&
                                                                        row["安裝方式"].ToString() == setupMethod.ToString()
                                                                            );
                return Convert.ToInt32(rows.First()[moment.ToString()]);
            } catch (Exception ex) {
                throw new Exception(string.Format("1|力矩表搜尋不到{0}導程{1}", model, lead));
            }
        }

        public double GetVmax_ms(Model model, double lead, int stroke) {
            try {
                return GetVmax_mms(model, lead, stroke) / 1000f;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        public double GetVmax_mms(Model model, double lead, int stroke) {
            try {
                // 依照行程取RPM
                int rpm = GetRpmByStroke(model.name, lead, stroke);
                if (model.isUseBaltCalc)
                    //return GetBeltVmax_ms(model.name, lead, stroke, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType) * 1000;
                    return Math.Round(RPM_TO_MMS(rpm, lead), 2);    // 四捨五入取小數第一位
                else
                    // 轉速換算Vmax
                    return Math.Round(RPM_TO_MMS(rpm, lead), 2);    // 四捨五入取小數第一位
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw new Exception(ex.Message);
            }
        }

        // 依照行程取RPM
        public int GetRpmByStroke(string model, double lead, int stroke) {
            if (model == "")
                return -1;

            // 取同型號集合
            IEnumerable<DataRow> strokeRpms;
            if (IsContainsReducerRatio(model))
                //strokeRpms = strokeRpm.Rows.Cast<DataRow>().Where(row => model.StartsWith(row["型號"].ToString()));
                strokeRpms = strokeRpm.Rows.Cast<DataRow>().Where(row => row["型號"].ToString().StartsWith(model) && Convert.ToDouble(row["導程"].ToString()) == lead);
            else
                strokeRpms = strokeRpm.Rows.Cast<DataRow>().Where(row => row["型號"].ToString() == model && Convert.ToDouble(row["導程"].ToString()) == (int)Math.Round(lead, 0));

            try {
                if (!strokeRpms.Any(row => Convert.ToInt32(row["行程"].ToString()) >= stroke))
                    return Convert.ToInt32(strokeRpms.Last()["轉速"].ToString());

                // 依照行程取RPM
                int strokeRpm = Convert.ToInt32(strokeRpms.First(row => Convert.ToInt32(row["行程"].ToString()) >= stroke)["轉速"]);
                return strokeRpm;
            } catch (Exception ex) {
                //Console.WriteLine(ex);
                return Convert.ToInt32(strokeRpms.Last()["轉速"].ToString());
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
            return (int)Math.Round(mms * 60f / (float)lead, 0);
            //return (int)(mms * 60f / (float)lead);

            //return (int)(Math.Round(mms / (float)lead, 0) * 60);
        }

        public int GetMaxStroke(string model, double lead) {
            IEnumerable<int> strokes;
            if (IsContainsReducerRatio(model))
                //strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => model.StartsWith(row["型號"].ToString()))
                strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => row["型號"].ToString().StartsWith(model) && Convert.ToDouble(row["導程"].ToString()) == lead)
                                                        .Select(row => Convert.ToInt32(row["行程"].ToString()));
            else
                strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => row["型號"].ToString() == model && Convert.ToDouble(row["導程"].ToString()) == (int)Math.Round(lead, 0))
                                                        .Select(row => Convert.ToInt32(row["行程"].ToString()));
            return strokes.Max();
        }

        public int GetSeriesMaxStroke(string series) {
            IEnumerable<int> strokes;
            //strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => Regex.Replace(row["型號"].ToString(), @"\d+", "").Equals(series))
            //                                        .Select(row => Convert.ToInt32(row["行程"].ToString()));

            strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => GetModelType(row["型號"].ToString()).ToString().Equals(series))
                                                    .Select(row => Convert.ToInt32(row["行程"].ToString()));

            return strokes.Max();
        }

        public int GetSeriesMinStroke(Model.ModelType modelType) {
            //return modelType.IsSeriesDM() ? 30 : 50;
            IEnumerable<int> strokes;
            strokes = strokeRpm.Rows.Cast<DataRow>().Where(row => Regex.Replace(row["型號"].ToString(), @"\d", "").StartsWith(modelType.ToString()))
                                                    .Select(row => Convert.ToInt32(row["行程"].ToString()));
            return strokes.Min();
        }

        public double GetMaxLoad(string model, double lead, Condition conditions) {
            // 取最大荷重
            double maxLoad = int.MaxValue;
            string data = "";

            try {
                Func<DataRow, bool> VerifyModel;
                if (model.IsContainsReducerRatioType())
                    VerifyModel = (row) => row["型號"].ToString().StartsWith(model);
                else
                    VerifyModel = (row) => row["型號"].ToString() == model;

                if (momentData.Rows.Cast<DataRow>().Where(row => VerifyModel(row) && Convert.ToDouble(row["導程"].ToString()) == lead &&
                                                    row["安裝方式"].ToString() == conditions.setupMethod.ToString()).Count() == 0)
                    return maxLoad;

                data = momentData.Rows.Cast<DataRow>()
                                             .Where(row => VerifyModel(row) && Convert.ToDouble(row["導程"].ToString()) == lead && 
                                                           row["安裝方式"].ToString() == conditions.setupMethod.ToString())
                                             .Select(row => row["最大荷重"].ToString())
                                             .First();
                if (!string.IsNullOrEmpty(data))
                    maxLoad = Convert.ToDouble(data);
            } catch (Exception ex) {
                //Console.WriteLine(ex);
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
        public double GetMaxCountPerMinute(Model model, Condition conditions) {
            //double _vMax = 0;
            //if (conditions.vMaxCalcMode == Condition.CalcVmax.Max) {
            //    if (model.isUseBaltCalc) {
            //        //_vMax = GetBeltVmax_ms(model.name, model.lead, 1, conditions.stroke, model.mainWheel, model.subWheel1, model.subWheel2) * 1000;
            //        //if (conditions.reducerRatio.Keys.Contains(model.name))
            //        //    _vMax = GetBeltVmax_ms(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke, model.mainWheel, model.subWheel1, model.subWheel2, model.beltCalcType);
            //        //else
            //        //    _vMax = GetBeltVmax_ms(model.name, model.lead, 1, conditions.stroke, model.mainWheel, model.subWheel1, model.subWheel2, model.beltCalcType);
            //        _vMax = GetBeltVmax_ms(model.name, model.lead, conditions.stroke, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType);
            //    } else {
            //        //if (conditions.reducerRatio.Keys.Contains(model.name))
            //        //    _vMax = GetVmax_ms(model, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
            //        //else
            //        //    _vMax = GetVmax_ms(model, model.lead, 1, conditions.stroke);
            //        _vMax = GetVmax_ms(model, model.lead, conditions.stroke);
            //    }
            //} else if (conditions.vMaxCalcMode == Condition.CalcVmax.Custom) {
            //    _vMax = conditions.vMax / 1000f;

            //    // 非皮帶機構才判斷
            //    if (!model.isUseBaltCalc) {
            //        // RPM驗證
            //        int strokeRpm;
            //        int vMaxRpm = GetRpmByMMS(model.lead, model.vMax * 1000);
            //        //if (conditions.reducerRatio.Keys.Contains(model.name))
            //        //    strokeRpm = GetRpmByStroke(model.name, model.lead, conditions.reducerRatio[model.name], conditions.stroke);
            //        //else
            //        //    strokeRpm = GetRpmByStroke(model.name, model.lead, 1, conditions.stroke);
            //        strokeRpm = GetRpmByStroke(model.name, model.lead, conditions.stroke);
            //        int _rpm = Math.Min(strokeRpm, vMaxRpm);
            //        _vMax = RPM_TO_MMS(_rpm, model.lead) / 1000f;
            //    }
            //}

            //double _accelSpeed = (float)conditions.accelSpeed / 1000f;

            ////double accelTime = _vMax / _accelSpeed;            

            //double accelTime = 0;
            //if (conditions.accelSpeed != 0) {
            //    //conditions.accelTime = conditions.vMax / conditions.accelSpeed;
            //} else {
            //    accelTime = conditions.accelTime;
            //}

            //if (isCheckStrokeTooShort) {
            //    // 行程過短驗證
            //    if (strokeTooShortModifyItem == Converter.ModifyItem.Vmax)
            //        //_vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, (int)_vMax, accelTime, model.stroke);
            //        _vMax = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, _vMax, accelTime, model.stroke);
            //    else if (strokeTooShortModifyItem == Converter.ModifyItem.AccelSpeed) {
            //        accelTime = Converter.CheckStrokeTooShort_CalcByAccelTime(strokeTooShortModifyItem, _vMax, accelTime, model.stroke);
            //    }
            //}

            //double decelTime = accelTime;
            ////double constantTime = ((2f * (float)conditions.stroke / 1000f / _vMax) - accelTime - decelTime) / 2f;
            //double constantTime = ((2f * (float)model.stroke / 1000f / _vMax) - accelTime - decelTime) / 2f;
            ////double constantTime = ((2f * (float)conditions.stroke / _vMax) - accelTime - decelTime) / 2f;
            //// 單趟來回需要的秒數
            ////double totalTime = (accelTime + constantTime + decelTime + conditions.stopTime) * 2f;
            //double totalTime = (accelTime + constantTime + decelTime) * 2f;

            double totalTime = (model.accelTime + model.constantTime + model.decelTime) * 2f;

            double maxCountPerMinute = 60f / totalTime;

            return maxCountPerMinute;
        }

        // 取馬達資訊
        public (double ratedTorque, double maxTorque, double rotateInertia, int loadInertiaMomentRatio) GetMotorParams(int power) {
            Func<DataRow, bool> con = row => Convert.ToInt32(row["馬達瓦數"].ToString()) == power;
            double ratedTorque = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["額定轉矩"].ToString())).First();
            double maxTorque = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["最大轉矩"].ToString())).First();
            double rotateInertia = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToDouble(row["轉動慣量"].ToString())).First();
            int loadInertiaMomentRatio = motorInfo.Rows.Cast<DataRow>().Where(con).Select(row => Convert.ToInt32(row["負載慣量與力矩比"].ToString())).First();
            return (ratedTorque, maxTorque, rotateInertia, loadInertiaMomentRatio);
        }

        public bool IsContainsReducerRatio(string model) {
            //return reducerInfo.Rows.Cast<DataRow>().Any(row => row["型號"].ToString() == model);
            return beltInfo.Rows.Cast<DataRow>().Any(row => row["型號"].ToString().StartsWith(model) && row["型號"].ToString().Contains("-"));
        }

        // 皮帶Vmax(m/s)
        public double GetBeltVmax_ms(string model, double lead, int stroke, BeltWheel mainWheel_P1, SubBeltWheel subWheel_P2, SubBeltWheel subWheel_P3, Model.BeltCalcType beltCalcType) {
            if (beltCalcType == Model.BeltCalcType.減速機構) {
                // 依照行程取RPM
                int rpm = GetRpmByStroke(model, lead, stroke);
                double reducerRpmRatio = subWheel_P2.diameter / mainWheel_P1.diameter;
                double subWheelRpm = (int)(rpm / reducerRpmRatio);
                double vMax_belt = Math.PI * subWheel_P3.diameter * (subWheelRpm / 60) / 1000;
                return vMax_belt;
            } else if (beltCalcType == Model.BeltCalcType.減速機2 || beltCalcType == Model.BeltCalcType.減速機4) {
                // 依照行程取RPM
                int rpm = GetRpmByStroke(model, lead, stroke);
                double reducerRpmRatio = Convert.ToDouble(model.Split('-')[1]);
                double subWheelRpm = (int)(rpm / reducerRpmRatio);
                double vMax_belt = Math.PI * subWheel_P3.diameter * (subWheelRpm / 60) / 1000;
                return vMax_belt;
            } else {
                // 依照行程取RPM
                int rpm = GetRpmByStroke(model, lead, stroke);
                double vMax_belt = Math.PI * subWheel_P3.diameter * ((float)rpm / 60f) / 1000f;
                return vMax_belt;
            }
        }

        // 皮帶Vmax(m/s)
        public double GetBeltVmaxByRpm_ms(string model, int rpm, BeltWheel mainWheel_P1, SubBeltWheel subWheel_P2, SubBeltWheel subWheel_P3, Model.BeltCalcType beltCalcType) {
            if (beltCalcType == Model.BeltCalcType.減速機構) {
                // 依照行程取RPM
                double reducerRpmRatio = subWheel_P2.diameter / mainWheel_P1.diameter;
                double subWheelRpm = (int)(rpm / reducerRpmRatio);
                double vMax_belt = Math.PI * subWheel_P3.diameter * (subWheelRpm / 60) / 1000;
                return vMax_belt;
            } else if (beltCalcType == Model.BeltCalcType.減速機2 || beltCalcType == Model.BeltCalcType.減速機4) {
                // 依照行程取RPM
                double reducerRpmRatio = Convert.ToDouble(model.Split('-')[1]);
                double subWheelRpm = (int)(rpm / reducerRpmRatio);
                double vMax_belt = Math.PI * subWheel_P3.diameter * (subWheelRpm / 60) / 1000;
                return vMax_belt;
            } else {
                // 依照行程取RPM
                double vMax_belt = Math.PI * subWheel_P3.diameter * ((float)rpm / 60f) / 1000f;
                return vMax_belt;
            }
        }

        /// <summary>
        /// 皮帶RPM回推
        /// </summary>
        /// <param name="vMax_belt">m/s</param>
        /// <param name="mainWheel_P1"></param>
        /// <param name="subWheel_P2"></param>
        /// <param name="subWheel_P3"></param>
        /// <param name="beltCalcType"></param>
        /// <returns></returns>
        public int GetBeltRPM(string model, double vMax_belt, BeltWheel mainWheel_P1, SubBeltWheel subWheel_P2, SubBeltWheel subWheel_P3, Model.BeltCalcType beltCalcType) {
            if (beltCalcType == Model.BeltCalcType.減速機構) {
                double reducerRpmRatio = subWheel_P2.diameter / mainWheel_P1.diameter;
                double subWheelRpm = vMax_belt * 1000 * 60 / Math.PI / subWheel_P3.diameter;
                int rpm = (int)(Math.Round(subWheelRpm * reducerRpmRatio, 0));
                return rpm;
            } else if (beltCalcType == Model.BeltCalcType.減速機2 || beltCalcType == Model.BeltCalcType.減速機4) {
                double reducerRpmRatio = Convert.ToDouble(model.Split('-')[1]);
                double subWheelRpm = vMax_belt * 1000 * 60 / Math.PI / subWheel_P3.diameter;
                int rpm = (int)(subWheelRpm * reducerRpmRatio);
                return rpm;
            } else {
                int rpm = (int)(vMax_belt * 1000 * 60 / Math.PI / subWheel_P3.diameter);
                return rpm;
            }
        }

        // 圖表點資訊
        public List<PointF> GetChartPoints(Model model) {
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

            //double accelTime = model.vMax / model.accelSpeed;
            //double decelTime = accelTime;
            //double constantTime = ((2f * (float)conditions.stroke / conditions.vMax) - conditions.accelTime - conditions.decelTime) / 2f;
            //double constantTime = ((2f * (float)model.stroke / model.vMax) - accelTime - decelTime) / 2f;

            List<PointF> points = new List<PointF>() { new PointF(0, 0) };
            // 加速時間
            points.Add(new PointF((float)model.accelTime, (float)model.vMax));
            // 等速時間
            points.Add(new PointF((float)model.accelTime + (float)model.constantTime, (float)model.vMax));
            // 減速時間
            points.Add(new PointF((float)model.accelTime + (float)model.constantTime + (float)model.decelTime, 0));
            //// 停等時間
            //points.Add(new PointF((float)model.accelTime + (float)model.constantTime + (float)model.decelTime + (float)model.stopTime, 0));

            return points;
        }

        public (
            double accelTime,
            double constantTime,
            double runTime,
            double accelSpeed,
            double maxSpeed,
            double cycleTime
        ) GetChartInfo(Model model) {
            double runTime = model.accelTime + model.constantTime + model.decelTime;
            double cycleTime = runTime * 2;

            double accelTime = Convert.ToDouble(model.accelTime.ToString("0.000"));
            double constantTime = Convert.ToDouble(model.constantTime.ToString("0.000"));
            runTime = Convert.ToDouble(runTime.ToString("0.000"));            
            runTime = Convert.ToDouble(runTime.ToString("0.000"));
            cycleTime = Convert.ToDouble(cycleTime.ToString("0.000"));
            double vMax = Convert.ToDouble(model.vMax.ToString("0.000"));
            double accelSpeed = Convert.ToDouble(model.accelSpeed.ToString("0.000"));

            return (accelTime, constantTime, runTime, accelSpeed, vMax, cycleTime);
        }

        public int GetMaxAccelSpeed(Model model, int stroke, Model.ModelType modelType) {
            if (model.name == "")
                return -1;

            int rpm = GetRpmByStroke(model.name, model.lead, stroke);
            double vMax = 0;
            if (modelType.IsBeltType())
                //vMax = (lead * (double)rpm) / 60f;
                vMax = GetBeltVmax_ms(model.name, model.lead, stroke, model.mainWheel_P1, model.subWheel_P2, model.subWheel_P3, model.beltCalcType) * 1000;
            else
                vMax = GetVmax_mms(model, model.lead, stroke);
            double repeatability = Convert.ToDouble(modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString().StartsWith(model.name))["重複定位精度"].ToString());
            double minAccelTime = repeatability <= 0.01 ? 0.2 : 0.4;
            int maxAccelSpeed = (int)(vMax / minAccelTime);
            return maxAccelSpeed;
        }

        public Model.ModelType[] GetNotNullModelType() {
            Model.ModelType[] allModelTypes = modelInfo.Rows.Cast<DataRow>().Select(row => (Model.ModelType)Enum.Parse(typeof(Model.ModelType), row["型號類別"].ToString())).Distinct().ToArray();
            return allModelTypes;
        }

        public Model.ModelType[] GetDustfreeModelType() {
            Model.ModelType[] allModelTypes = modelInfo.Rows.Cast<DataRow>().Where(row => row["使用環境"].ToString() == "無塵")
                                                                            .Select(row => (Model.ModelType)Enum.Parse(typeof(Model.ModelType), row["型號類別"].ToString()))
                                                                            .Distinct()
                                                                            .ToArray();
            return allModelTypes;
        }

        public string GetModelTypeComment(Model.ModelType modelType) {
            string comment = modelTypeInfo.Rows.Cast<DataRow>().First(row => row["型號類別"].ToString() == modelType.ToString())["敘述"].ToString();
            //if (comment == "")
            //    comment = modelTypeInfo.Rows.Cast<DataRow>().First(row => row["型號類別"].ToString() == modelType.ToString())["標準敘述"].ToString();
            return comment;
        }

        public Model.SetupMethod[] GetSupportSetup(Model.ModelType modelType) {
            var setups = modelInfo.Rows.Cast<DataRow>().Where(row => row["型號類別"].ToString() == modelType.ToString())
                                                       .Select(row => row["安裝方式"].ToString().Split('&'))
                                                       .Distinct();
            List<Model.SetupMethod> supportSetups = new List<Model.SetupMethod>();
            setups.ToList().ForEach(setup => {
                setup.ToList().ForEach(s => {
                    Model.SetupMethod _s = (Model.SetupMethod)Enum.Parse(typeof(Model.SetupMethod), s);
                    if (!supportSetups.Contains(_s))
                        supportSetups.Add(_s);
                });
            });

            return supportSetups.ToArray();
        }

        public Model.SetupMethod[] GetSupportSetup(string modelName, double lead) {
            string[] setups;
            if (modelName.IsContainsReducerRatioType())
                setups = modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString().StartsWith(modelName))["安裝方式"].ToString().Split('&');
            else
                setups = modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString() == modelName && Convert.ToDouble(row["導程"].ToString()) == lead)["安裝方式"].ToString().Split('&');
            List<Model.SetupMethod> supportSetups = new List<Model.SetupMethod>();
            setups.ToList().ForEach(s => {
                Model.SetupMethod _s = (Model.SetupMethod)Enum.Parse(typeof(Model.SetupMethod), s);
                if (!supportSetups.Contains(_s))
                    supportSetups.Add(_s);
            });

            return supportSetups.ToArray();
        }

        public Model.ModelType GetModelType(string model) {
            var types = modelInfo.Rows.Cast<DataRow>().Where(row => row["型號"].ToString().StartsWith(model));
            if (types.Count() == 0)
                return Model.ModelType.Null;
            string type = types.First()["型號類別"].ToString();
            Model.ModelType modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), type);
            return modelType;

            //if (modelInfo.Rows.Cast<DataRow>().Any(row => row["型號"].ToString().StartsWith(model))) {
            //    string type = modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString().StartsWith(model))["型號類別"].ToString();
            //    Model.ModelType modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), type);
            //    return modelType;
            //} else
            //    return Model.ModelType.Null;
        }

        public Model.UseEnvironment GetModelUseEnv(string model) {
            string env = modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString().StartsWith(model))["使用環境"].ToString();
            Model.UseEnvironment useEnvironment = (Model.UseEnvironment)Enum.Parse(typeof(Model.UseEnvironment), env);
            return useEnvironment;
        }

        public string GetLeadText(string model, double lead) {
            return modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString() == model && Convert.ToDouble(row["導程"].ToString()) == lead)["導程顯示"].ToString();
        }

        public bool IsStrokeTooShort_CheckByAccelTime(int stroke, double vMax, double accelTime) {
            return ((2f * (float)stroke / (vMax * 1000)) - accelTime - accelTime) / 2f < 0;
        }

        public bool IsStrokeTooShort_CheckByAccelSpeed(int stroke, double vMax, double accelSpeed) {
            return ((2f * (float)stroke / (vMax * 1000)) - (vMax / accelSpeed) - (vMax / accelSpeed)) / 2f < 0;
        }
    }
}

