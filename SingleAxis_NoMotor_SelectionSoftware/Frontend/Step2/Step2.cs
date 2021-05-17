using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step2 {
        private FormMain formMain;
        public Calculation calc = new Calculation();

        public int minHeight = 787;
        private int maxHeight = 1300;
        
        private List<Model> curRecommandList;
        private Thread threadCalc;        
        // 顏色區分
        Dictionary<string, Func<Model, bool>> redFontConditions = new Dictionary<string, Func<Model, bool>>() {
            { "T_max安全係數", model => model.tMaxSafeCoefficient >= Model.tMaxStandard },
            { "運行距離", model => model.serviceLifeDistance >= 0.3 },
            { "運行壽命", model => model.serviceLifeTime.year >= 3 },
            { "皮帶馬達安全係數", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < Model.beltMotorStandard },
            { "皮帶T_max安全係數", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= Model.tMaxStandard_beltMotor },
            { "力矩警示", model => model.isMomentVerifySuccess },
        };

        // Step2各項目
        public MotorPower motorPower;
        public ChartInfo chartInfo;
        public RunCondition runCondition;
        public InputValidate inputValidate;

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();

            // 輸入驗證
            inputValidate = new InputValidate(formMain);
            // 馬達條件
            motorPower = new MotorPower(formMain);
            // 圖表
            chartInfo = new ChartInfo(formMain);
            // 運轉條件
            runCondition = new RunCondition(formMain);            

            // 減速比
            calc.reducerInfo.Rows.Cast<DataRow>().ToList().ForEach(row => {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cboCell = new DataGridViewComboBoxCell();
                txtCell.Value = row["Model"].ToString();
                cboCell.DataSource = row["ReducerRatio"].ToString().Split('、');
                cboCell.Value = row["ReducerRatio"].ToString().Split('、')[0];
                newRow.Cells.Add(txtCell);
                newRow.Cells.Add(cboCell);
                formMain.dgvReducerInfo.Rows.Add(newRow);
            });
        }

        public void Load() {
            // 馬達選項更新
            motorPower.UpdateMotorCalcMode();
            motorPower.Load();

            // 進階選項驗證
            formMain.chkAdvanceMode.Checked = false;
            formMain.panelAdvanceMode.Enabled = formMain.optCalcSelectedModel.Checked;            

            // 減速比顯示
            formMain.panelReducer.Visible = ((Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text)) == Model.ModelType.歐規皮帶滑台;
        }        

        private void InitEvents() {
            // 進階選項
            formMain.chkAdvanceMode.CheckedChanged += ChkAdvanceMode_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            formMain.dgvRecommandList.SelectionChanged += DgvRecommandList_SelectionChanged;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;
        }

        private void ChkAdvanceMode_CheckedChanged(object sender, EventArgs e) {
            formMain.panelAdvanceParams.Visible = formMain.chkAdvanceMode.Checked;
        }

        private void DgvRecommandList_SelectionChanged(object sender, EventArgs e) {
            // 畫圖
            chartInfo.PaintGraph();            
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step2) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep3);
            }
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 版面修正
            if (formMain.optCalcAllModel.Checked) {
                formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, maxHeight);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep2);
            }
            formMain.panelSelectCalcResult.Visible = formMain.optCalcSelectedModel.Checked;

            threadCalc = new Thread(() => {
                Thread.Sleep(100);

                // 開始計算
                var result = calc.GetRecommandResult(runCondition.curCondition);
                // 規件規格
                curRecommandList = result["List"] as List<Model>;
                // 回傳訊息
                string msg = result["Msg"] as string;
                // 是否跳出Alarm
                bool isAlarm = (bool)result["Alarm"];

                // 搜尋不到型號驗證
                if (curRecommandList.Count == 0) {
                    MessageBox.Show("此使用條件無法計算，請嘗試調整使用條件。");

                    // 訊息顯示
                    if (!string.IsNullOrEmpty(result["Msg"] as string)) {
                        // 訊息斷行顯示
                        string alarmMsg = result["Msg"] as string;
                        string showMsg = "";
                        alarmMsg.Split('|').ToList().ForEach(alarm => {
                            if (string.IsNullOrEmpty(alarm))
                                return;
                            int index = alarmMsg.Split('|').ToList().IndexOf(alarm) + 1;
                            showMsg += index + ". " + alarm + "\r\n";
                        });
                        //MessageBox.Show(showMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(showMsg, SideTable.MsgStatus.Alarm)));
                    }

                    return;
                }
                // 清空訊息
                formMain.Invoke(new Action(formMain.sideTable.ClearMsg));

                // 表單顯示
                DisplayRecommandList();                
            });
            threadCalc.Start();

            // Loading顯示
            ShowWaiting();
        }

        private void ShowWaiting() {
            new Thread(() => {
                formMain.Invoke(new Action(() => {
                    FormWaiting wait = new FormWaiting(calc.GetCalcPercent);
                    wait.GetPercent = calc.GetCalcPercent;
                    wait.ShowDialog();
                }));
            }).Start();
        }

        private void DisplayRecommandList() {
            formMain.Invoke(new Action(() => {
                formMain.dgvRecommandList.Rows.Clear();
                // 皮帶欄位顯示修正
                formMain.dgvRecommandList.Columns["皮帶馬達安全係數"].Visible = formMain.optRepeatabilityBelt.Checked;
                formMain.dgvRecommandList.Columns["皮帶T_max安全係數"].Visible = formMain.optRepeatabilityBelt.Checked;
            }));

            foreach (Model model in curRecommandList) {
                try {
                    formMain.Invoke(new Action(() => {
                        int index = formMain.dgvRecommandList.Rows.Add();
                        formMain.dgvRecommandList.Rows[index].Height = 35;
                        formMain.dgvRecommandList.Rows[index].Cells["鎖定"].Value = false;
                        formMain.dgvRecommandList.Rows[index].Cells["項次"].Value = model.name;
                        formMain.dgvRecommandList.Rows[index].Cells["導程"].Value = model.lead;
                        formMain.dgvRecommandList.Rows[index].Cells["荷重"].Value = model.load;
                        formMain.dgvRecommandList.Rows[index].Cells["最高轉速"].Value = model.rpm;
                        formMain.dgvRecommandList.Rows[index].Cells["運行速度"].Value = model.vMax;
                        formMain.dgvRecommandList.Rows[index].Cells["加速度"].Value = model.accelSpeed;
                        formMain.dgvRecommandList.Rows[index].Cells["最大行程"].Value = model.maxStroke;
                        formMain.dgvRecommandList.Rows[index].Cells["運行時間"].Value = model.moveTime;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩A"].Value = model.moment_A;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩B"].Value = model.moment_B;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩C"].Value = model.moment_C;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩警示"].Value = model.isMomentVerifySuccess ? "Pass" : "Fail";
                        formMain.dgvRecommandList.Rows[index].Cells["馬達瓦數"].Value = model.usePower;
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶馬達安全係數"].Value = model.beltMotorSafeCoefficient == -1 ? "無" : model.beltMotorSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["T_max安全係數"].Value = model.tMaxSafeCoefficient;
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶T_max安全係數"].Value = model.beltSafeCoefficient == -1 ? "無" : model.beltSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["是否推薦"].Value = Properties.Resources.inCondition;
                        formMain.dgvRecommandList.Rows[index].Cells["更詳細資訊"].Value = Properties.Resources.detail_disable_in_condition;

                        // 運行距離
                        if (model.slideTrackServiceLifeDistance < 0)
                            formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "Error";
                        else {
                            if (model.serviceLifeDistance > 20000)
                                formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "2萬公里以上";
                            else
                                formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = ((float)model.serviceLifeDistance / 10000f).ToString("#0.0") + "萬公里";
                        }

                        // 使用壽命時間
                        string useTime = "";
                        if (model.serviceLifeTime.year >= 10)
                            useTime = "10年以上";
                        else {
                            if (model.serviceLifeTime.year > 0)
                                useTime += model.serviceLifeTime.year + "年";
                            if (model.serviceLifeTime.month > 0)
                                useTime += model.serviceLifeTime.month + "個月";
                            if (model.serviceLifeTime.year == 0 && model.serviceLifeTime.month == 0)
                                useTime = "1個月以下";
                        }
                        formMain.dgvRecommandList.Rows[index].Cells["運行壽命"].Value = useTime;

                        // 顏色區分
                        foreach (var con in redFontConditions) {
                            formMain.dgvRecommandList.Rows[index].Cells[con.Key].Style.ForeColor = con.Value(model) ? Color.Black : Color.Red;
                            formMain.dgvRecommandList.Rows[index].Cells[con.Key].Style.SelectionForeColor = con.Value(model) ? Color.Black : Color.Red;
                        }
                    }));
                } catch (Exception ex) {
                    break;
                }
            }

            try {
                formMain.Invoke(new Action(() => {
                    if (formMain.optCalcAllModel.Checked) {
                        // 欄位寬度更新
                        foreach (DataGridViewColumn col in formMain.dgvRecommandList.Columns)
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

                        // 畫圖
                        chartInfo.PaintGraph();
                    } else {
                        // 細項顯示
                        DisplaySelectedModel();

                        // 驗證Vmax
                        Model curModel = curRecommandList.First();
                        if (formMain.optMaxSpeedType_mms.Checked) {
                            formMain.txtMaxSpeed.Text = curModel.vMax.ToString();
                        } else if (formMain.optMaxSpeedType_rpm.Checked) {
                            int curRpm = calc.MMS_TO_RPM(curModel.vMax, curModel.lead);
                            formMain.txtMaxSpeed.Text = curRpm.ToString();
                        }
                    }
                }));
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }                
        }

        private void DisplaySelectedModel() {
            Model curModel = curRecommandList.First();
            // 使用壽命時間
            string useTime = "";
            if (curModel.serviceLifeTime.year >= 10) {
                useTime = "10年以上";
            } else {
                if (curModel.serviceLifeTime.year > 0)
                    useTime += curModel.serviceLifeTime.year + "年";
                if (curModel.serviceLifeTime.month > 0)
                    useTime += curModel.serviceLifeTime.month + "個月";
            }
            // 使用壽命距離
            string useDistance = "";
            if (curModel.slideTrackServiceLifeDistance < 0)
                useDistance = "Error";
            else {
                if (curModel.serviceLifeDistance > 20000)
                    useDistance = "2萬公里以上";
                else
                    useDistance = ((float)curModel.serviceLifeDistance / 10000f).ToString("#0.0") + "萬公里";
            }
            Dictionary<string, string> item = new Dictionary<string, string>();
            if (formMain.optRepeatabilityBelt.Checked && curModel.beltMotorSafeCoefficient != -1 && curModel.beltSafeCoefficient != -1)
                item = new Dictionary<string, string>() {
                    { "項次", curModel.name.ToString() + "-L" + curModel.lead.ToString() },
                    { "T_max安全係數", curModel.tMaxSafeCoefficient.ToString() },
                    { "皮帶馬達安全係數", curModel.beltMotorSafeCoefficient == -1 ? "null" : curModel.beltMotorSafeCoefficient.ToString() },
                    { "皮帶T_max安全係數", curModel.beltSafeCoefficient == -1 ? "null" : curModel.beltSafeCoefficient.ToString() },
                    { "力矩警示", curModel.isMomentVerifySuccess ? "Pass" : "Fail" },
                    { "運行距離", useDistance },
                    { "運行壽命", useTime },
                };
            else
                item = new Dictionary<string, string>() {
                    { "項次", curModel.name.ToString() + "-L" + curModel.lead.ToString() },
                    { "T_max安全係數", curModel.tMaxSafeCoefficient.ToString() },
                    { "力矩警示", curModel.isMomentVerifySuccess ? "Pass" : "Fail" },
                    { "運行距離", useDistance },
                    { "運行壽命", useTime },
                };
            DataTable dt = new DataTable();
            dt.Columns.Add("Item");
            dt.Columns.Add("Value");
            foreach (var i in item) {
                DataRow dr = dt.NewRow();
                dr["Item"] = i.Key;
                dr["Value"] = i.Value;
                dt.Rows.Add(dr);
            }
            formMain.dgvCalcSelectedModel.DataSource = dt;

            // 顏色區分壽命規範
            int serviceYear = -1;
            long serviceDistance = -1;
            foreach (DataGridViewRow row in formMain.dgvCalcSelectedModel.Rows) {
                if (row.Cells["Item"].Value.ToString() == "運行距離") {
                    serviceDistance = curModel.serviceLifeDistance;
                    row.Cells["Value"].Style.ForeColor = serviceDistance >= 3000 ? Color.Black : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "運行壽命") {
                    int.TryParse(row.Cells["Value"].Value.ToString().Split('年')[0], out serviceYear);
                    row.Cells["Value"].Style.ForeColor = serviceYear >= 3 ? Color.Black : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "扭矩確認") {
                    if (formMain.optRepeatabilityBelt.Checked)
                        row.Cells["Value"].Style.ForeColor = curModel.is_tMax_OK && curModel.isMotorOK && curModel.is_belt_tMax_OK ? Color.Green : Color.Red;
                    else
                        row.Cells["Value"].Style.ForeColor = curModel.is_tMax_OK ? Color.Green : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "T_max安全係數")
                    row.Cells["Value"].Style.ForeColor = curModel.tMaxSafeCoefficient >= Model.tMaxStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "T_Rms安全係數")
                    row.Cells["Value"].Style.ForeColor = curModel.tRmsSafeCoefficient > Model.tRmsStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "皮帶馬達安全係數" && row.Cells["Value"].Value.ToString() != "null")
                    row.Cells["Value"].Style.ForeColor = curModel.beltMotorSafeCoefficient < Model.beltMotorStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "皮帶T_max安全係數" && row.Cells["Value"].Value.ToString() != "null")
                    row.Cells["Value"].Style.ForeColor = curModel.beltSafeCoefficient >= Model.tMaxStandard_beltMotor ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "力矩警示") {
                    row.Cells["Value"].Style.ForeColor = curModel.isMomentVerifySuccess ? Color.Black : Color.Red;
                    if (!curModel.isMomentVerifySuccess)
                        row.Cells["Value"].ToolTipText = "力矩判定異常，請洽詢Toyo業務人員";
                }
            }

            foreach (DataGridViewColumn col in formMain.dgvCalcSelectedModel.Columns) {
                // 欄位寬度更新
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // 型號選行取消選取
            formMain.dgvCalcSelectedModel.CurrentCell = null;

            // 畫圖
            chartInfo.PaintGraph();
        }

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            //formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            //formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
