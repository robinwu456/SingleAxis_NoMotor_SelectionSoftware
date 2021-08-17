using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace SingleAxis_NoMotor_SelectionSoftware {
    class Test {
        private FormMain formMain;
        private string outputFileName = "./Test/Result_{0}.csv";
        private string[] testDataFileNames = Directory.GetFiles("./Test/現有測試數據").Reverse().ToArray();
        //private string[] testDataFileNames = {
        //    //"./Test/現有測試數據/螺桿測試數據_水平.csv",
        //    //"./Test/現有測試數據/螺桿測試數據_橫掛.csv",
        //    //"./Test/現有測試數據/螺桿測試數據_垂直.csv",

        //    //"./Test/現有測試數據/皮帶測試數據_減速機構_水平.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機構_橫掛.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機構_垂直.csv",

        //    //"./Test/現有測試數據/皮帶測試數據_減速機2_水平.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機2_橫掛.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機2_垂直.csv",

        //    //"./Test/現有測試數據/皮帶測試數據_減速機4_水平.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機4_橫掛.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_減速機4_垂直.csv",

        //    //"./Test/現有測試數據/皮帶測試數據_直接驅動_水平.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_直接驅動_橫掛.csv",
        //    //"./Test/現有測試數據/皮帶測試數據_直接驅動_垂直.csv",
        //};
        private int curCalcIndex = 1;

        public Test(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        private void InitEvents() {
            formMain.lbTitle.DoubleClick += StartTest;
        }

        private void StartTest(object sender, EventArgs e) {
            // 測試主程序
            new Thread(Run).Start();

            // 測試進度
            new Thread(() => {
                formMain.Invoke(new Action(() => {
                    FormWaiting wait = new FormWaiting(GetCalcPercent);
                    wait.ShowDialog();
                }));
            }).Start();
        }

        public (int percent, bool isError) GetCalcPercent() {
            int allDataCount = testDataFileNames.Select(file => FileUtil.ReadCsv(file).Rows.Count).Sum();
            int percent = curCalcIndex * 100 / allDataCount;

            return (percent, false);
        }

        private void Run() {
            // 清除所有測試數據
            Directory.GetFiles("./Test/所有測試結果數據").ToList().ForEach(testLog => File.Delete(testLog));

            string resultLog_螺桿 = "";
            string resultLog_減速機構 = "";
            string resultLog_減速機2 = "";
            string resultLog_減速機4 = "";
            string resultLog_直接驅動 = "";
            string resultLog_highDiff = "";

            curCalcIndex = 1;

            foreach (string fileName in testDataFileNames) {
                DataTable data = FileUtil.ReadCsv(fileName);

                Model.SetupMethod setupMethod = Model.SetupMethod.水平;
                if (fileName.Contains("水平"))
                    setupMethod = Model.SetupMethod.水平;
                else if (fileName.Contains("橫掛"))
                    setupMethod = Model.SetupMethod.橫掛;
                else if (fileName.Contains("垂直"))
                    setupMethod = Model.SetupMethod.垂直;

                string title = "";
                if (fileName.Contains("螺桿"))
                    title = "螺桿" + " " + setupMethod.ToString() + "\r\n項次,導程,行程,荷重,A,B,C,,預計滑軌壽命,計算滑軌壽命,滑軌壽命誤差,,預計T_max,計算T_max,T_max誤差,,預計T_Rms,計算T_Rms,T_Rms誤差,,預計螺桿壽命,計算螺桿壽命,螺桿壽命誤差\r\n";
                else if (fileName.Contains("皮帶")) {
                    if (fileName.Contains("減速機構"))
                        title = "皮帶 減速機構";
                    else if (fileName.Contains("減速機2"))
                        title = "皮帶 減速機2";
                    else if (fileName.Contains("減速機4"))
                        title = "皮帶 減速機4";
                    else if (fileName.Contains("直接驅動"))
                        title = "皮帶 直接驅動";
                    title = title + " " + setupMethod.ToString() + "\r\n項次,導程,行程,荷重,A,B,C,,預計滑軌壽命,計算滑軌壽命,滑軌壽命誤差,,預計馬達能力預估-轉動慣量合計,計算馬達能力預估-轉動慣量合計,馬達能力預估-轉動慣量合計誤差,,預計T_max,計算T_max,T_max誤差,,預計皮帶安全係數,計算皮帶安全係數,皮帶安全係數誤差\r\n";
                }

                string output = "";
                string hightDiffOutput = "";
                output += title;
                foreach (DataRow row in data.Rows) {
                    Condition con = new Condition();
                    con.isTesting = true;
                    con.calcModel = (row["項次"].ToString(), Convert.ToDouble(row["導程"].ToString()));
                    con.setupMethod = setupMethod;
                    con.modelType = Model.ModelType.Null;
                    con.vMaxCalcMode = Condition.CalcVmax.Custom;
                    con.vMax = Convert.ToDouble(row["Vmax"].ToString()) * 1000;
                    con.stroke = Convert.ToInt32(row["總行程"].ToString());
                    con.load = Convert.ToDouble(row["荷重"].ToString());
                    con.moveTime = 999999999;
                    con.accelTime = Convert.ToDouble(row["加減速時間"].ToString());
                    con.stopTime = Convert.ToDouble(row["停等時間"].ToString());
                    //con.accelSpeed = Convert.ToDouble(row["加減速度"].ToString()) * 1000;
                    con.RepeatabilityCondition = a => 1 > 0;
                    con.useFrequence = new Condition.UseFrequence() {
                        // 趟/分
                        countPerMinute = 1,
                        // 小時/日
                        hourPerDay = 8,
                        // 日/年
                        dayPerYear = 240
                    };
                    con.expectServiceLifeTime = 0;
                    // 力矩參數
                    con.c = Convert.ToDouble(row["Dyn"].ToString());
                    con.mr_C = Convert.ToDouble(row["MR_C"].ToString());
                    con.mp_C = Convert.ToDouble(row["MP_C"].ToString());
                    con.my_C = Convert.ToDouble(row["MY_C"].ToString());
                    if (fileName.Contains("螺桿")) {
                        con.dynamicLoadRating = Convert.ToInt32(row["動額定負載"].ToString());
                        con.outerDiameter = Convert.ToInt32(row["外徑"].ToString());
                    }
                    con.moment_A = Convert.ToInt32(row["A"].ToString());
                    con.moment_B = Convert.ToInt32(row["B"].ToString());
                    con.moment_C = Convert.ToInt32(row["C"].ToString());
                    // 馬達參數
                    con.powerSelection = Condition.PowerSelection.Custom;
                    con.ratedTorque = Convert.ToDouble(row["額定轉矩"].ToString());
                    con.maxTorque = Convert.ToDouble(row["最大轉矩"].ToString());
                    con.rotateInertia = Convert.ToDouble(row["轉動慣量"].ToString());

                    // 皮帶參數
                    if (fileName.Contains("皮帶")) {
                        if (fileName.Contains("減速機構"))
                            con.beltCalcType = Model.BeltCalcType.減速機構;
                        else if (fileName.Contains("減速機2"))
                            con.beltCalcType = Model.BeltCalcType.減速機2;
                        else if (fileName.Contains("減速機4"))
                            con.beltCalcType = Model.BeltCalcType.減速機4;
                        else if (fileName.Contains("直接驅動"))
                            con.beltCalcType = Model.BeltCalcType.直接驅動;

                        if (con.beltCalcType == Model.BeltCalcType.減速機2 || con.beltCalcType == Model.BeltCalcType.減速機4) {
                            con.reducerRotateInertia = Convert.ToDouble(row["減速機轉動慣量"].ToString());
                            con.reducerRpmRatio = Convert.ToDouble(row["減速機轉速比"].ToString());
                        }
                        con.beltWidth = Convert.ToDouble(row["皮帶寬"].ToString());
                        con.beltLength = Convert.ToDouble(row["皮帶長度"].ToString());
                        con.beltUnitDensity = Convert.ToDouble(row["皮帶單位密度"].ToString());
                        con.loadInertiaMomentRatio = Convert.ToInt32(row["負載慣量與力矩比"].ToString());
                        con.beltAllowableTension = Convert.ToDouble(row["皮帶容許拉力"].ToString());
                        if (con.beltCalcType == Model.BeltCalcType.減速機構 || con.beltCalcType == Model.BeltCalcType.減速機4) {
                            // 主動輪P1
                            con.mainWheel_P1 = new BeltWheel(
                                Convert.ToDouble(row["主動輪P1輪徑"].ToString()),
                                Convert.ToDouble(row["主動輪P1輪寬"].ToString())
                            );
                            // 從動輪P2
                            con.subWheel_P2 = new SubBeltWheel(
                                Convert.ToDouble(row["從動輪P2輪徑"].ToString()),
                                Convert.ToDouble(row["從動輪P2輪寬"].ToString())
                            );
                        } else {
                            con.mainWheel_P1 = new BeltWheel(0, 0);
                            con.subWheel_P2 = new SubBeltWheel(0, 0);
                        }
                        // 從動輪P3
                        con.subWheel_P3 = new SubBeltWheel(
                            Convert.ToDouble(row["從動輪P3輪徑"].ToString()),
                            Convert.ToDouble(row["從動輪P3輪寬"].ToString())
                        );
                        // 從動輪P4
                        con.subWheel_P4 = new SubBeltWheel(
                            Convert.ToDouble(row["從動輪P4輪徑"].ToString()),
                            Convert.ToDouble(row["從動輪P4輪寬"].ToString())
                        );
                    }

                    string curTestInfo = string.Format("{0}-L{1}-{2}-{3}-{4}kg-A{5}-B{6}-C{7}", con.calcModel.model, con.calcModel.lead, con.beltCalcType.ToString(), con.setupMethod.ToString(), con.load, con.moment_A, con.moment_B, con.moment_C);
                    Console.WriteLine(curTestInfo);

                    // 計算
                    var result = formMain.page2.calc.GetRecommandResult(con);
                    List<Model> resultModel = result["List"] as List<Model>;
                    while (resultModel.Count == 0) {
                        // 訊息顯示
                        if (!string.IsNullOrEmpty(result["Msg"] as string)) {
                            // 訊息斷行顯示
                            string alarmMsg = result["Msg"] as string;
                            //formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(alarmMsg, SideTable.MsgStatus.Alarm)));
                            formMain.Invoke(new Action(() => MessageBox.Show(alarmMsg)));
                            return;
                        }
                        Thread.Sleep(100);
                    }
                    Model model = resultModel.First();                    
                    string logFileName = "./Test/所有測試結果數據/" + curTestInfo;
                    FileUtil.LogModelInfo(model, con.setupMethod, false, logFileName);                    
                    string log = "";
                    string highDiffLog = "";
                    if (fileName.Contains("螺桿")) {
                        double diffPercent_slideDistance = 100 - (Math.Min(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance));
                        double diffPercent_tMax = 100 - (Math.Min(Convert.ToDouble(Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00")), model.tMaxSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00")), model.tMaxSafeCoefficient));
                        double diffPercent_tRms = 100 - (Math.Min(Convert.ToDouble(Convert.ToDouble(row["T_Rms安全係數"].ToString()).ToString("#0.00")), model.tRmsSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(Convert.ToDouble(row["T_Rms安全係數"].ToString()).ToString("#0.00")), model.tRmsSafeCoefficient));
                        double diffPercent_screwDistance = 100 - (Math.Min(Convert.ToDouble(row["螺桿壽命"].ToString()), model.screwServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["螺桿壽命"].ToString()), model.screwServiceLifeDistance));
                        log = string.Format("{0},{1},{2},{3},{4},{5},{6},,{7},{8},{9},,{10},{11},{12},,{13},{14},{15},,{16},{17},{18},,{19}\r\n",
                                                    model.name,
                                                    model.lead,
                                                    model.stroke,
                                                    model.load,
                                                    model.moment_A,
                                                    model.moment_B,
                                                    model.moment_C,
                                                    row["滑軌壽命"].ToString(),
                                                    model.slideTrackServiceLifeDistance,
                                                    diffPercent_slideDistance.ToString("#00.00") + "%",
                                                    Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00"),
                                                    model.tMaxSafeCoefficient,
                                                    diffPercent_tMax.ToString("#00.00") + "%",
                                                    Convert.ToDouble(row["T_Rms安全係數"].ToString()).ToString("#0.00"),
                                                    model.tRmsSafeCoefficient,
                                                    diffPercent_tRms.ToString("#00.00") + "%",
                                                    row["螺桿壽命"].ToString(),
                                                    model.screwServiceLifeDistance,
                                                    diffPercent_screwDistance.ToString("#00.00") + "%",
                                                    string.Format("=hyperlink(\"{0}\"，\"開啟Log\")", new FileInfo(logFileName + ".log").FullName)
                        );
                        if (diffPercent_slideDistance > 1 || diffPercent_tMax > 1 || diffPercent_tRms > 1 || diffPercent_screwDistance > 1)
                            highDiffLog += log;
                    } else if (fileName.Contains("皮帶")) {
                        double diffPercent_slideDistance = 100 - (Math.Min(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance));
                        double diffPercent_rotateInertia_total = 100 - (Math.Min(Convert.ToDouble(row["馬達能力預估-轉動慣量合計"].ToString()), model.rotateInertia_total) * 100 / Math.Max(Convert.ToDouble(row["馬達能力預估-轉動慣量合計"].ToString()), model.rotateInertia_total));
                        double diffPercent_tMax = 100 - (Math.Min(Convert.ToDouble(Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00")), model.tMaxSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00")), model.tMaxSafeCoefficient));
                        double diffPercent_beltSafeCoefficient = 100 - (Math.Min(Convert.ToDouble(Convert.ToDouble(row["皮帶安全係數"].ToString()).ToString("#0.00")), model.beltSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(Convert.ToDouble(row["皮帶安全係數"].ToString()).ToString("#0.00")), model.beltSafeCoefficient));
                        log = string.Format("{0},{1},{2},{3},{4},{5},{6},,{7},{8},{9},,{10},{11},{12},,{13},{14},{15},,{16},{17},{18},,{19}\r\n",
                                                    model.name,
                                                    model.lead,
                                                    model.stroke,
                                                    model.load,
                                                    model.moment_A,
                                                    model.moment_B,
                                                    model.moment_C,
                                                    row["滑軌壽命"].ToString(),
                                                    model.slideTrackServiceLifeDistance,
                                                    diffPercent_slideDistance.ToString("#00.00") + "%",
                                                    row["馬達能力預估-轉動慣量合計"].ToString(),
                                                    model.rotateInertia_total,
                                                    diffPercent_rotateInertia_total.ToString("#00.00") + "%",
                                                    Convert.ToDouble(row["T_max安全係數"].ToString()).ToString("#0.00"),
                                                    model.tMaxSafeCoefficient,
                                                    diffPercent_tMax.ToString("#00.00") + "%",
                                                    Convert.ToDouble(row["皮帶安全係數"].ToString()).ToString("#0.00"),
                                                    model.beltSafeCoefficient,
                                                    diffPercent_beltSafeCoefficient.ToString("#00.00") + "%",
                                                    string.Format("=hyperlink(\"{0}\"，\"開啟Log\")", new FileInfo(logFileName + ".log").FullName)
                        );
                        if (diffPercent_slideDistance > 1 || diffPercent_rotateInertia_total > 1 || diffPercent_tMax > 1 || diffPercent_beltSafeCoefficient > 1)
                            highDiffLog += log;
                    }
                    output += log;
                    hightDiffOutput += highDiffLog;

                    curCalcIndex++;
                }

                if (fileName.Contains("螺桿"))
                    resultLog_螺桿 += output + "\r\n";
                else if (fileName.Contains("減速機構"))
                    resultLog_減速機構 += output + "\r\n";
                else if (fileName.Contains("減速機2"))
                    resultLog_減速機2 += output + "\r\n";
                else if (fileName.Contains("減速機4"))
                    resultLog_減速機4 += output + "\r\n";
                else if (fileName.Contains("直接驅動"))
                    resultLog_直接驅動 += output + "\r\n";

                if (hightDiffOutput != "")
                    resultLog_highDiff += title + hightDiffOutput + "\r\n";
            }

            FileUtil.FileWrite(string.Format(outputFileName, "螺桿"), resultLog_螺桿);
            FileUtil.FileWrite(string.Format(outputFileName, "減速機構"), resultLog_減速機構);
            FileUtil.FileWrite(string.Format(outputFileName, "減速機2"), resultLog_減速機2);
            FileUtil.FileWrite(string.Format(outputFileName, "減速機4"), resultLog_減速機4);
            FileUtil.FileWrite(string.Format(outputFileName, "直接驅動"), resultLog_直接驅動);

            FileUtil.FileWrite(string.Format(outputFileName, "誤差過大"), resultLog_highDiff);

            MessageBox.Show("測試完成");
            Process.Start(new DirectoryInfo("./Test").FullName);
        }

        //private DataTable ParseTestData(string fileName) {
        //    DataTable dt = new DataTable();
        //    string content = FileUtil.FileRead(fileName);
        //    string[] rows = content.Replace("\r\n", "|").Split('|');
        //    string[] columns = rows[0].Split(',');

        //    // 新增欄位
        //    foreach (string column in columns)
        //        dt.Columns.Add();

        //    // 資料匯入
        //    for (int i = 1; i < rows.Length - 1; i++) {
        //        // 空的則跳過
        //        if (rows[i] == string.Empty)
        //            continue;

        //        DataRow newRow = dt.NewRow();
        //        foreach (DataColumn column in dt.Columns)
        //            newRow[column] = rows[i].Split(',')[dt.Columns.IndexOf(column)];
        //        dt.Rows.Add(newRow);
        //    }

        //    return dt;
        //}
    }
}
