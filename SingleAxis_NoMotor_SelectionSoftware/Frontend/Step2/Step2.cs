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
        private Calculation calc = new Calculation();

        public int minHeight = 787;
        private int maxHeight = 1300;

        private Condition curCondition = new Condition();
        private List<Model> curRecommandList;
        private Thread threadCalc;
        // 馬達自訂參數
        private (double ratedTorque, double maxTorque, double rotateInertia) customMotorParams = (-1, -1, -1);
        // 顏色區分
        Dictionary<string, Func<Model, bool>> redFontConditions = new Dictionary<string, Func<Model, bool>>() {
            { "T_max安全係數", model => model.tMaxSafeCoefficient >= Model.tMaxStandard },
            { "運行距離", model => model.serviceLifeDistance >= 0.3 },
            { "運行壽命", model => model.serviceLifeTime.year >= 3 },
            { "皮帶馬達安全係數", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < Model.beltMotorStandard },
            { "皮帶T_max安全係數", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= Model.tMaxStandard_beltMotor },
            { "力矩警示", model => model.isMomentVerifySuccess },
        };

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        public void Load() {
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

            // 預設選項
            formMain.optRepeatabilityScrew.Checked = true;
            formMain.optNoExpectServiceLife.Checked = true;
        }

        public void UpdateCondition(object sender, EventArgs e) {
            // 使用環境
            if (formMain.optStandardEnv.Checked)
                curCondition.useEnvironment = Model.UseEnvironment.Standard;
            else if (formMain.optDustFreeEnv.Checked)
                curCondition.useEnvironment = Model.UseEnvironment.DustFree;
            // 機構型態
            curCondition.modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text);
            //switch (formMain.cboModelType.Text) {
            //    case "軌道內嵌式螺桿滑台":
            //        curCondition.modelType = Model.ModelType.軌道內嵌式螺桿滑台;
            //        break;
            //    case "軌道內嵌推桿式螺桿滑台":
            //        curCondition.modelType = Model.ModelType.軌道內嵌推桿式螺桿滑台;
            //        break;
            //    case "螺桿滑台":
            //        curCondition.modelType = Model.ModelType.螺桿滑台;
            //        break;
            //    case "推桿式螺桿滑台":
            //        curCondition.modelType = Model.ModelType.推桿式螺桿滑台;
            //        break;
            //    case "皮帶滑台":
            //        curCondition.modelType = Model.ModelType.皮帶滑台;
            //        break;
            //    case "歐規皮帶滑台":
            //        curCondition.modelType = Model.ModelType.歐規皮帶滑台;
            //        break;
            //}
            // 安裝方式
            if (formMain.optHorizontalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.WallHang;
            else if (formMain.optUpsideDownUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Vertical;
            // 最高速度
            if (formMain.optMaxSpeedType_mms.Checked)
                curCondition.vMax = Convert.ToDouble(formMain.txtMaxSpeed.Text);
            else if (formMain.optMaxSpeedType_rpm.Checked) {
                if (formMain.txtMaxSpeed.Text.Contains("."))
                    formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                if (calc.IsContainsReducerRatio(formMain.cboModel.Text)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text).Cells["columnReducerRatio"].Value.ToString();
                    curCondition.vMax = calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text) / Convert.ToDouble(dgvReducerRatioValue));
                } else
                    curCondition.vMax = calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text));
            }            
            // 力矩參數
            curCondition.moment_A = Convert.ToInt32(formMain.txtMomentA.Text);
            curCondition.moment_B = Convert.ToInt32(formMain.txtMomentB.Text);
            curCondition.moment_C = Convert.ToInt32(formMain.txtMomentC.Text);
            // 行程
            curCondition.stroke = Convert.ToInt32(formMain.txtStroke.Text);
            // 荷重
            curCondition.load = Convert.ToDouble(formMain.txtLoad.Text);
            // 運行時間
            curCondition.moveTime = Convert.ToDouble(formMain.txtRunTime.Text);
            // 使用頻率
            curCondition.useFrequence = new Condition.UseFrequence() {
                // 趟/分
                countPerMinute = Convert.ToInt32(formMain.txtTimesPerMinute.Text),
                // 小時/日
                hourPerDay = Convert.ToInt32(formMain.txtHourPerDay.Text),
                // 日/年
                dayPerYear = Convert.ToInt32(formMain.txtDayPerYear.Text)
            };
            // 希望壽命
            curCondition.expectServiceLifeTime = formMain.optExpectServiceLife.Checked ? Convert.ToInt32(formMain.txtExpectServiceLifeTime.Text) : -1;
            // 驗正加速時間
            if (formMain.optRepeatabilityScrew.Checked)
                curCondition.accelTime = 0.2;
            else if (formMain.optRepeatabilityBelt.Checked)
                curCondition.accelTime = 0.4;
            // 傳動方式
            if (formMain.optRepeatabilityScrew.Checked)
                curCondition.RepeatabilityCondition = repeatability => repeatability <= 0.01;
            else if (formMain.optRepeatabilityBelt.Checked)
                curCondition.RepeatabilityCondition = repeatability => repeatability >= 0.04;
            // 馬達瓦數
            if (formMain.optCalcAllModel.Checked) {
                // 全部計算只能標準或自訂
                if (formMain.cboPower.Text == "標準")
                    curCondition.powerSelection = Condition.PowerSelection.Standard;
                else if (formMain.cboPower.Text == "自訂") {
                    if (formMain.optMotorParamsModifySimple.Checked) {
                        curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                        curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                    } else if (formMain.optMotorParamsModifyAdvance.Checked)
                        curCondition.powerSelection = Condition.PowerSelection.Custom;
                }
            } else if (formMain.optCalcSelectedModel.Checked) {
                // 單項計算可選擇該型號適用瓦數
                if (formMain.cboPower.Text.Contains("標準")) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(new Regex(@"\d+").Match(formMain.cboPower.Text).Value);
                } else if (formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifySimple.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                } else if (formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifyAdvance.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.Custom;
                }
            }
            // 馬達參數自訂
            if (curCondition.powerSelection == Condition.PowerSelection.Custom) {
                if (formMain.optMotorParamsModifySimple.Checked) {
                    var p = calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));
                    curCondition.ratedTorque = p.ratedTorque;
                    curCondition.maxTorque = p.maxTorque;
                    curCondition.rotateInertia = p.rotateInertia;

                    customMotorParams.ratedTorque = curCondition.ratedTorque;
                    customMotorParams.maxTorque = curCondition.maxTorque;
                    customMotorParams.rotateInertia = curCondition.rotateInertia;
                } else {
                    curCondition.ratedTorque = Convert.ToDouble(formMain.txtRatedTorque.Text);
                    curCondition.maxTorque = Convert.ToDouble(formMain.txtMaxTorque.Text);
                    curCondition.rotateInertia = Convert.ToDouble(formMain.txtRotateInertia.Text);

                    customMotorParams.ratedTorque = curCondition.ratedTorque;
                    customMotorParams.maxTorque = curCondition.maxTorque;
                    customMotorParams.rotateInertia = curCondition.rotateInertia;
                }
            }
            // 單項計算
            if (!formMain.optCalcAllModel.Checked) {
                (string model, double lead) calcModel = (formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                curCondition.calcModel = calcModel;
            } else
                curCondition.calcModel = (null, -1);
            // 減速比
            curCondition.reducerRatio.Clear();
            formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => {
                curCondition.reducerRatio.Add(row.Cells["columnModel"].Value.ToString(), Convert.ToInt32(row.Cells["columnReducerRatio"].Value.ToString()));
            });
        }

        private void InitEvents() {
            // 更新條件
            formMain.optStandardEnv.CheckedChanged += UpdateCondition;
            formMain.optDustFreeEnv.CheckedChanged += UpdateCondition;
            formMain.cboModelType.SelectedIndexChanged += UpdateCondition;
            formMain.optHorizontalUse.CheckedChanged += UpdateCondition;
            formMain.optWallHangingUse.CheckedChanged += UpdateCondition;
            formMain.optUpsideDownUse.CheckedChanged += UpdateCondition;
            formMain.optCalcAllModel.CheckedChanged += UpdateCondition;
            formMain.optCalcSelectedModel.CheckedChanged += UpdateCondition;
            formMain.cboSeries.SelectedIndexChanged += UpdateCondition;
            formMain.cboModel.SelectedIndexChanged += UpdateCondition;
            formMain.cboLead.SelectedIndexChanged += UpdateCondition;
            formMain.txtMomentA.TextChanged += UpdateCondition;
            formMain.txtMomentB.TextChanged += UpdateCondition;
            formMain.txtMomentC.TextChanged += UpdateCondition;
            formMain.txtLoad.TextChanged += UpdateCondition;
            formMain.txtStroke.TextChanged += UpdateCondition;
            formMain.txtRunTime.TextChanged += UpdateCondition;
            formMain.txtTimesPerMinute.TextChanged += UpdateCondition;
            formMain.txtHourPerDay.TextChanged += UpdateCondition;
            formMain.txtDayPerYear.TextChanged += UpdateCondition;
            formMain.optNoExpectServiceLife.CheckedChanged += UpdateCondition;
            formMain.optExpectServiceLife.CheckedChanged += UpdateCondition;
            formMain.txtExpectServiceLifeTime.TextChanged += UpdateCondition;
            formMain.txtMaxSpeed.TextChanged += UpdateCondition;
            formMain.txtAccelSpeed.TextChanged += UpdateCondition;
            formMain.cboPower.SelectedIndexChanged += UpdateCondition;
            formMain.optMotorParamsModifySimple.CheckedChanged += UpdateCondition;
            formMain.optMotorParamsModifyAdvance.CheckedChanged += UpdateCondition;
            formMain.cboMotorParamsMotorPowerSelection.SelectedIndexChanged += UpdateCondition;
            formMain.txtRatedTorque.TextChanged += UpdateCondition;
            formMain.txtRotateInertia.TextChanged += UpdateCondition;
            formMain.txtMaxTorque.TextChanged += UpdateCondition;
            formMain.optRepeatabilityScrew.CheckedChanged += UpdateCondition;

            // 進階選項
            //formMain.toggleAdvanceOptions.CheckedChanged += ToggleAdvanceOptions_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
            formMain.sideTable.Update(null, null);
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep3);
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 版面修正
            formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, maxHeight);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep2);            

            //// 條件
            //UpdateCondition();

            threadCalc = new Thread(() => {
                Thread.Sleep(100);

                // 開始計算
                var result = calc.GetRecommandResult(curCondition);
                // 規件規格
                curRecommandList = result["List"] as List<Model>;
                // 回傳訊息
                string msg = result["Msg"] as string;
                // 是否跳出Alarm
                bool isAlarm = (bool)result["Alarm"];

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
                    foreach (DataGridViewColumn col in formMain.dgvRecommandList.Columns) {
                        // 欄位寬度更新
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    }
                }));
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            //formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            //formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
