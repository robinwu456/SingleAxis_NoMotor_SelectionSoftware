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
        private string outputFileName = "./Test/Result.csv";
        private string[] testDataFileNames = Directory.GetFiles("./Test/現有測試數據");
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
            string resultLog = "";
            foreach (string fileName in testDataFileNames) {
                DataTable data = FileUtil.ReadCsv(fileName);

                Model.SetupMethod setupMethod = Model.SetupMethod.水平;
                if (fileName.Contains("水平"))
                    setupMethod = Model.SetupMethod.水平;
                else if (fileName.Contains("橫掛"))
                    setupMethod = Model.SetupMethod.橫掛;
                else if (fileName.Contains("垂直"))
                    setupMethod = Model.SetupMethod.垂直;

                string output = "";
                if (fileName.Contains("螺桿"))
                    output = "螺桿" + " " + setupMethod.ToString() + "\r\n項次,導程,行程,荷重,A,B,C,,預計滑軌壽命,計算滑軌壽命,滑軌壽命誤差,,預計T_max,計算T_max,T_max誤差,,預計T_Rms,計算T_Rms,T_Rms誤差,,預計螺桿壽命,計算螺桿壽命,螺桿壽命誤差\r\n";
                else if (fileName.Contains("皮帶")) {
                    string title = "";
                    if (fileName.Contains("減速機構"))
                        title = "皮帶 減速機構";
                    else if (fileName.Contains("減速機2"))
                        title = "皮帶 減速機2";
                    else if (fileName.Contains("減速機4"))
                        title = "皮帶 減速機4";
                    else if (fileName.Contains("直接驅動"))
                        title = "皮帶 直接驅動";
                    output = title + " " + setupMethod.ToString() + "\r\n項次,導程,行程,荷重,A,B,C,,預計滑軌壽命,計算滑軌壽命,滑軌壽命誤差,,預計馬達能力預估-轉動慣量合計,計算馬達能力預估-轉動慣量合計,馬達能力預估-轉動慣量合計誤差,,預計T_max,計算T_max,T_max誤差,,預計皮帶安全係數,計算皮帶安全係數,皮帶安全係數誤差\r\n";
                }

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

                    // 計算
                    var result = formMain.page2.calc.GetRecommandResult(con)["List"] as List<Model>;
                    while (result.Count == 0)
                        Thread.Sleep(100);
                    Model model = result.First();
                    string curTestInfo = string.Format("{0}-L{1}-{2}kg-A{3}-B{4}-C{5}", model.name, model.lead, model.load, model.moment_A, model.moment_B, model.moment_C);
                    FileUtil.LogModelInfo(model, con.setupMethod, false, "./Test/所有測試結果數據/" + curTestInfo);
                    Console.WriteLine(curTestInfo);
                    string log = "";
                    if (fileName.Contains("螺桿"))
                        log = string.Format("{0},{1},{2},{3},{4},{5},{6},,{7},{8},{9},,{10},{11},{12},,{13},{14},{15},,{16},{17},{18}\r\n",
                                                    model.name,
                                                    model.lead,
                                                    model.stroke,
                                                    model.load,
                                                    model.moment_A,
                                                    model.moment_B,
                                                    model.moment_C,
                                                    row["滑軌壽命"].ToString(),
                                                    model.slideTrackServiceLifeDistance,
                                                    (100 - (Math.Min(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance))).ToString("#00.00") + "%",
                                                    row["T_max安全係數"].ToString(),
                                                    model.tMaxSafeCoefficient,
                                                    (100 - (Math.Min(Convert.ToDouble(row["T_max安全係數"].ToString()), model.tMaxSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(row["T_max安全係數"].ToString()), model.tMaxSafeCoefficient))).ToString("#00.00") + "%",
                                                    row["T_Rms安全係數"].ToString(),
                                                    model.tRmsSafeCoefficient,
                                                    (100 - (Math.Min(Convert.ToDouble(row["T_Rms安全係數"].ToString()), model.tRmsSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(row["T_Rms安全係數"].ToString()), model.tRmsSafeCoefficient))).ToString("#00.00") + "%",
                                                    row["螺桿壽命"].ToString(),
                                                    model.screwServiceLifeDistance,
                                                    (100 - (Math.Min(Convert.ToDouble(row["螺桿壽命"].ToString()), model.screwServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["螺桿壽命"].ToString()), model.screwServiceLifeDistance))).ToString("#00.00") + "%"
                        );
                    else if (fileName.Contains("皮帶"))
                        log = string.Format("{0},{1},{2},{3},{4},{5},{6},,{7},{8},{9},,{10},{11},{12},,{13},{14},{15},,{16},{17},{18}\r\n",
                                                    model.name,
                                                    model.lead,
                                                    model.stroke,
                                                    model.load,
                                                    model.moment_A,
                                                    model.moment_B,
                                                    model.moment_C,
                                                    row["滑軌壽命"].ToString(),
                                                    model.slideTrackServiceLifeDistance,
                                                    (100 - (Math.Min(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance) * 100 / Math.Max(Convert.ToDouble(row["滑軌壽命"].ToString()), model.slideTrackServiceLifeDistance))).ToString("#00.00") + "%",
                                                    row["馬達能力預估-轉動慣量合計"].ToString(),
                                                    model.rotateInertia_total,
                                                    (100 - (Math.Min(Convert.ToDouble(row["馬達能力預估-轉動慣量合計"].ToString()), model.rotateInertia_total) * 100 / Math.Max(Convert.ToDouble(row["馬達能力預估-轉動慣量合計"].ToString()), model.rotateInertia_total))).ToString("#00.00") + "%",
                                                    row["T_max安全係數"].ToString(),
                                                    model.tMaxSafeCoefficient,
                                                    (100 - (Math.Min(Convert.ToDouble(row["T_max安全係數"].ToString()), model.tMaxSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(row["T_max安全係數"].ToString()), model.tMaxSafeCoefficient))).ToString("#00.00") + "%",                                                    
                                                    row["皮帶安全係數"].ToString(),
                                                    model.beltSafeCoefficient,
                                                    (100 - (Math.Min(Convert.ToDouble(row["皮帶安全係數"].ToString()), model.beltSafeCoefficient) * 100 / Math.Max(Convert.ToDouble(row["皮帶安全係數"].ToString()), model.beltSafeCoefficient))).ToString("#00.00") + "%"
                        );
                    output += log;

                    curCalcIndex++;
                }

                resultLog += output + "\r\n";
            }

            FileUtil.FileWrite(outputFileName, resultLog);

            MessageBox.Show("測試完成");
            Process.Start(new DirectoryInfo(outputFileName).FullName);
        }
    }
}
