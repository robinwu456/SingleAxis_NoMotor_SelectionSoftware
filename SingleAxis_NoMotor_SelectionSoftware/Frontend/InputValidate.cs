using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class InputValidate {
        private FormMain formMain;    
        private System.Threading.Timer vMaxGetRPMCounter;
        private TextBox[] allowDecimalPoint;

        public Thread threadShowRPMCounting;
        public Thread threadCalcVmaxRange;
        public Thread threadCalcAccelSpeedRange;

        public InputValidate(FormMain formMain) {
            this.formMain = formMain;

            // 允許小數點的輸入項
            allowDecimalPoint = new TextBox[] { 
                formMain.txtRatedTorque,
                formMain.txtRotateInertia,
                formMain.txtMaxTorque,
                formMain.txtLoad,
                formMain.txtRunTime,
                formMain.txtTimesPerMinute,
                formMain.txtMaxSpeed,
                formMain.txtMaxCalc,
            };

            InitEvents();
        }

        private void InitEvents() {
            // 數值有效驗證
            foreach (Control control in formMain.panelCalc.Controls.All()) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;
                    txt.KeyPress += InputCondition_KeyPress;
                    txt.Validating += InputCondition_Validating;
                    txt.KeyDown += InputCondition_KeyDown;
                }
            }
            // 數值有效驗證
            foreach (Control control in formMain.panelMoment.Controls.All()) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;
                    txt.KeyPress += InputCondition_KeyPress;
                    txt.Validating += InputCondition_Validating;
                    txt.KeyDown += InputCondition_KeyDown;
                }
            }

            // 最大荷重驗證
            formMain.txtLoad.Validating += TxtLoad_Validating;
            formMain.txtLoad.KeyDown += TxtLoad_KeyDown;

            // 最大行程驗證
            formMain.txtStroke.Validating += TxtStroke_Validating;
            formMain.txtStroke.KeyDown += TxtStroke_KeyDown;

            // 運行速度驗證
            formMain.txtMaxSpeed.KeyDown += TxtMaxSpeed_KeyDown;
            formMain.txtMaxSpeed.LostFocus += TxtMaxSpeed_LostFocus;
            formMain.txtAccelSpeed.KeyDown += TxtAccelSpeed_KeyDown;
            formMain.txtAccelSpeed.LostFocus += TxtAccelSpeed_LostFocus;
            //formMain.txtMaxSpeed.TextChanged += TxtMaxSpeed_TextChanged;
            //formMain.optMaxSpeedType_mms.CheckedChanged += OptMaxSpeedType_mms_CheckedChanged;
            formMain.cboMaxSpeedUnit.SelectedValueChanged += OptMaxSpeedType_mms_CheckedChanged;

            //// 加速度驗證
            //formMain.txtAccelSpeed.Validating += TxtAccelSpeed_Validating;

            // 使用頻率驗證
            formMain.txtHourPerDay.Validating += UseFrequence_Validating;
            formMain.txtDayPerYear.Validating += UseFrequence_Validating;
            formMain.txtHourPerDay.KeyDown += UseFrequence_KeyDown;
            formMain.txtDayPerYear.KeyDown += UseFrequence_KeyDown;

            // 最大值驗證
            //formMain.cboModel.SelectedValueChanged += UpdateMaxValue;
            //formMain.cboLead.SelectedValueChanged += UpdateMaxValue;
            //formMain.cboReducerRatio.SelectedValueChanged += UpdateMaxValue;
            //formMain.txtStroke.TextChanged += UpdateMaxValue;
            //formMain.txtMaxSpeed.TextChanged += UpdateMaxValue;
            //formMain.cboMaxSpeedUnit.SelectedValueChanged += UpdateMaxValue;            
        }        

        private void TxtAccelSpeed_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            TxtAccelSpeed_LostFocus(null, null);
        }

        private void TxtAccelSpeed_LostFocus(object sender, EventArgs e) {
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection || 
                !decimal.TryParse(formMain.txtStroke.Text, out decimal value) ||
                !decimal.TryParse(formMain.txtMaxSpeed.Text, out value) ||
                !decimal.TryParse(formMain.txtAccelSpeed.Text, out value) ||
                formMain.lbSideTableMsg.ForeColor == System.Drawing.Color.Red
               )
                return;

            // 加速度
            if (formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值"))
                formMain.txtAccelSpeed.Text = new Regex(@"\d+").Match(formMain.lbMaxValue_AccelSpeed.Text).Groups[0].Value;
            else if (formMain.lbMaxValue_AccelSpeed.Text.Contains("範圍")) {
                string min = new Regex(@"(\d+) ~ (\d+)").Match(formMain.lbMaxValue_AccelSpeed.Text).Groups[1].Value;
                string max = new Regex(@"(\d+) ~ (\d+)").Match(formMain.lbMaxValue_AccelSpeed.Text).Groups[2].Value;
                if (Convert.ToDecimal(formMain.txtAccelSpeed.Text) > Convert.ToDecimal(max))
                    formMain.txtAccelSpeed.Text = max;
                if (Convert.ToDecimal(formMain.txtAccelSpeed.Text) < Convert.ToDecimal(min))
                    formMain.txtAccelSpeed.Text = min;
            }
        }

        private void TxtMaxSpeed_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            TxtMaxSpeed_LostFocus(null, null);
        }

        private void TxtMaxSpeed_LostFocus(object sender, EventArgs e) {
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection ||
                !decimal.TryParse(formMain.txtStroke.Text, out decimal value) ||
                !decimal.TryParse(formMain.txtMaxSpeed.Text, out value) ||
                !decimal.TryParse(formMain.txtAccelSpeed.Text, out value) ||
                formMain.lbSideTableMsg.ForeColor == System.Drawing.Color.Red
               )
                return;

            // 最高速度
            string max = new Regex(@"\d+").Match(formMain.lbMaxValue_MaxSpeed.Text).Groups[0].Value;
            if (Convert.ToDecimal(formMain.txtMaxSpeed.Text) > Convert.ToDecimal(max))
                formMain.txtMaxSpeed.Text = max;
        }

        public void ShowConvertRPM() {
            while (true) {
                try {
                    formMain.Invoke(new Action(() => {
                        formMain.lbRpm.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection && 
                                                 formMain.chkAdvanceMode.Checked &&
                                                 double.TryParse(formMain.cboLead.Text, out double lead) && 
                                                 decimal.TryParse(formMain.txtMaxSpeed.Text, out decimal maxSpeed);

                        if (!formMain.cboMaxSpeedUnit.DroppedDown) {
                            if (double.TryParse(formMain.cboLead.Text, out lead) && decimal.TryParse(formMain.txtMaxSpeed.Text, out maxSpeed)) {
                                if (formMain.cboMaxSpeedUnit.Text == "mm/s")
                                    //RPM 顯示
                                    formMain.lbRpm.Text = string.Format("轉速：{0}RPM", formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)));
                                else
                                    // Vmax 顯示
                                    formMain.lbRpm.Text = string.Format("運行速度：{0}mm/s", formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead).ToString("#0"));
                            }
                        }
                    }));
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    break;
                }

                Thread.Sleep(100);
            }
        }

        public void CalcVmaxRange() {
            while (true) {
                try {
                    formMain.Invoke(new Action(() => {
                        formMain.lbMaxValue_MaxSpeed.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection &&
                                                               formMain.chkAdvanceMode.Checked &&
                                                               formMain.lbSideTableMsg.ForeColor != System.Drawing.Color.Red &&
                                                               decimal.TryParse(formMain.txtStroke.Text, out decimal value) &&
                                                               decimal.TryParse(formMain.txtMaxSpeed.Text, out value) &&
                                                               decimal.TryParse(formMain.txtAccelSpeed.Text, out value) &&
                                                               formMain.lbSideTableMsg.ForeColor != System.Drawing.Color.Red;
                        try {
                            double accelTime = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType() ? 0.4 : 0.2;

                            if (int.TryParse(formMain.txtStroke.Text, out int stroke) &&
                                double.TryParse(formMain.cboLead.Text, out double lead)) {
                                if (formMain.cboMaxSpeedUnit.Text == "mm/s") {
                                    int strokeTooShortMaxVmax = (int)Math.Round((double)stroke / accelTime, 0);
                                    int strokeRpmMaxVmax = (int)Math.Round(formMain.page2.calc.RPM_TO_MMS(formMain.page2.calc.GetRpmByStroke(formMain.cboModel.Text, lead, stroke), lead), 0);
                                    formMain.lbMaxValue_MaxSpeed.Text = string.Format("( 最大值：{0} {1} )", Math.Min(strokeTooShortMaxVmax, strokeRpmMaxVmax), "mm/s");
                                } else if (formMain.cboMaxSpeedUnit.Text == "RPM") {
                                    int maxVmax = (int)Math.Round((double)stroke / accelTime, 0);
                                    int maxRPM = formMain.page2.calc.MMS_TO_RPM(maxVmax, lead);
                                    int strokeRPM = formMain.page2.calc.GetRpmByStroke(formMain.cboModel.Text, lead, stroke);
                                    formMain.lbMaxValue_MaxSpeed.Text = string.Format("( 最大值：{0} {1} )", Math.Min(maxRPM, strokeRPM), "RPM");
                                }
                            }
                        } catch (Exception ex) {
                            Console.WriteLine(ex);
                        }
                    }));
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    break;
                }

                Thread.Sleep(100);
            }
        }

        public void CalcAccelSpeedRange() {
            while (true) {
                try {
                    formMain.Invoke(new Action(() => {
                        formMain.lbMaxValue_AccelSpeed.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection &&
                                                                 formMain.chkAdvanceMode.Checked &&
                                                                 formMain.lbSideTableMsg.ForeColor != System.Drawing.Color.Red &&
                                                                 decimal.TryParse(formMain.txtStroke.Text, out decimal value) &&
                                                                 decimal.TryParse(formMain.txtMaxSpeed.Text, out value) &&
                                                                 decimal.TryParse(formMain.txtAccelSpeed.Text, out value) &&
                                                                 formMain.lbSideTableMsg.ForeColor != System.Drawing.Color.Red;
                        //if (formMain.lbMaxValue_AccelSpeed.Visible) {
                        double accelTime = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType() ? 0.4 : 0.2;

                        if (int.TryParse(formMain.txtStroke.Text, out int stroke) &&
                            double.TryParse(formMain.txtMaxSpeed.Text, out double maxSpeed) &&
                            double.TryParse(formMain.cboLead.Text, out double lead)) {
                            try {
                                if (formMain.cboMaxSpeedUnit.Text == "mm/s") {
                                    double vMax = maxSpeed;
                                    int min = Convert.ToInt32((Math.Pow(vMax, 2) / stroke).ToString("#0"));                // 等速時間為0
                                    int max = Convert.ToInt32((vMax / accelTime).ToString("#0"));                                // 加速時間0.2
                                                                                                                                 // 加速度顯示
                                    if (min >= max)
                                        formMain.lbMaxValue_AccelSpeed.Text = string.Format("( 限制值：{0} mm/s²)", max);
                                    else
                                        formMain.lbMaxValue_AccelSpeed.Text = string.Format("( 範圍：{0} ~ {1} mm/s²)", min, max);
                                    //formMain.txtAccelSpeed.Enabled = !formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值") || formMain.lbSideTableMsg.ForeColor == System.Drawing.Color.Red;
                                    if (formMain.lbMaxValue_AccelSpeed.Visible) {
                                        if (formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值"))
                                            formMain.txtAccelSpeed.Text = max.ToString();
                                    }
                                } else if (formMain.cboMaxSpeedUnit.Text == "RPM") {
                                    double vMax = (maxSpeed * lead) / 60;
                                    int min = Convert.ToInt32((Math.Pow(vMax, 2) / stroke).ToString("#0"));                // 等速時間為0
                                    int max = Convert.ToInt32((vMax / accelTime).ToString("#0"));                                // 加速時間0.2
                                                                                                                                 // 加速度顯示
                                    if (min >= max)
                                        formMain.lbMaxValue_AccelSpeed.Text = string.Format("( 限制值：{0} mm/s²)", max);
                                    else
                                        formMain.lbMaxValue_AccelSpeed.Text = string.Format("( 範圍：{0} ~ {1} mm/s²)", min, max);
                                    //formMain.txtAccelSpeed.Enabled = !formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值") || formMain.lbSideTableMsg.ForeColor == System.Drawing.Color.Red;
                                    if (formMain.lbMaxValue_AccelSpeed.Visible) {
                                        //formMain.txtAccelSpeed.Enabled = !formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值");
                                        if (formMain.lbMaxValue_AccelSpeed.Text.Contains("限制值"))
                                            formMain.txtAccelSpeed.Text = max.ToString();
                                    }
                                }
                            } catch (Exception ex) {
                                Console.WriteLine(ex);
                            }
                        }
                        //}
                    }));
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                    break;
                }

                Thread.Sleep(100);
            }
        }

        private void InputCondition_KeyPress(object sender, KeyPressEventArgs e) {
            TextBox txt = sender as TextBox;

            if (!allowDecimalPoint.Contains(txt)) {
                // 限制輸入小數點
                if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
                    e.Handled = true;
            }
        }

        private void UseFrequence_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            TextBox txtUseFrequence = sender as TextBox;
            if (!decimal.TryParse(txtUseFrequence.Text, out decimal value))
                return;

            switch (txtUseFrequence.Name) {
                case "txtHourPerDay":
                    if (value > 24) {
                        formMain.lbHoursPerDayAlarm.Text = "一天不可超過24小時";
                        formMain.lbHoursPerDayAlarm.Visible = true;
                        e.Cancel = true;
                    } else
                        formMain.lbHoursPerDayAlarm.Visible = false;
                    break;
                case "txtDayPerYear":
                    if (value > 365) {
                        formMain.lbDaysPerYearAlarm.Text = "一年不可超過365天";
                        formMain.lbDaysPerYearAlarm.Visible = true;
                        e.Cancel = true;
                    } else
                        formMain.lbDaysPerYearAlarm.Visible = false;
                    break;
            }
        }

        private void UseFrequence_KeyDown(object sender, KeyEventArgs e) {
            TextBox txtUseFrequence = sender as TextBox;
            if (!decimal.TryParse(txtUseFrequence.Text, out decimal value))
                return;

            switch (txtUseFrequence.Name) {
                case "txtHourPerDay":
                    if (value > 24) {
                        formMain.lbHoursPerDayAlarm.Text = "一天不可超過24小時";
                        formMain.lbHoursPerDayAlarm.Visible = true;
                    } else
                        formMain.lbHoursPerDayAlarm.Visible = false;
                    break;
                case "txtDayPerYear":
                    if (value > 365) {
                        formMain.lbDaysPerYearAlarm.Text = "一年不可超過365天";
                        formMain.lbDaysPerYearAlarm.Visible = true;
                    } else
                        formMain.lbDaysPerYearAlarm.Visible = false;
                    break;
            }
        }

        public void InputCondition_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            TextBox txt = sender as TextBox;
            // Alarm顯示修正
            if (txt == formMain.txtHourPerDay)
                formMain.lbHoursPerDayAlarm.Text = "請輸入有效時間";
            if (txt == formMain.txtDayPerYear)
                formMain.lbDaysPerYearAlarm.Text = "請輸入有效天數";
            //if (txt == formMain.txtStroke)
            //    formMain.labelStrokeAlarm.Text = "請輸入有效行程";
            //if (txt == formMain.txtLoad)
            //    formMain.labelLoadAlarm.Text = "請輸入有效荷重";

            // 有效數值驗證
            Control lbAlarm = formMain.panelCalc.Controls.All().Concat(formMain.panelMoment.Controls.All()).First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
            //if (!decimal.TryParse(txt.Text, out value))
            //    e.Cancel = true;
        }
        private void InputCondition_KeyDown(object sender, KeyEventArgs e) {
            TextBox txt = sender as TextBox;            

            // Alarm顯示修正
            if (txt == formMain.txtHourPerDay)
                formMain.lbHoursPerDayAlarm.Text = "請輸入有效時間";
            if (txt == formMain.txtDayPerYear)
                formMain.lbDaysPerYearAlarm.Text = "請輸入有效天數";
            //if (txt == formMain.txtStroke)
            //    formMain.labelStrokeAlarm.Text = "請輸入有效行程";
            //if (txt == formMain.txtLoad)
            //    formMain.labelLoadAlarm.Text = "請輸入有效荷重";

            // 有效數值驗證
            Control lbAlarm = formMain.panelCalc.Controls.All().Concat(formMain.panelMoment.Controls.All()).First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
        }

        private void TxtLoad_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                return;

            //// 最大荷重驗證
            //ValidatingLoad();
        }

        private void TxtLoad_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                return;

            //// 最大荷重驗證
            //ValidatingLoad();
        }

        public bool VerifyAllInputValidate(bool isShowAlarm = false) {
            bool isVerifySuccess = true;

            // TextBox數值有效驗證            
            foreach (Control control in formMain.panelCalc.Controls.All().Concat(formMain.panelMoment.Controls.All())) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;

                    // 進階選項關閉時空值不驗證
                    if (txt == formMain.txtMaxSpeed && !formMain.chkAdvanceMode.Checked)
                        continue;
                    if (txt == formMain.txtAccelSpeed && !formMain.chkAdvanceMode.Checked)
                        continue;

                    // show alarm
                    if (isShowAlarm)
                        InputCondition_Validating(control, null);
                    if (!decimal.TryParse(txt.Text, out decimal value))
                        isVerifySuccess = false;
                }
            }

            // 型號選型多驗證cbo為null
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection)
                if (formMain.cboModel.Text == "" || formMain.cboLead.Text == "" || formMain.cboPower.Text == "")
                    isVerifySuccess = false;

            return isVerifySuccess;
        }

        private void TxtStroke_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                return;

            //ValidatingStroke();
        }

        private void TxtStroke_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal keyStroke))
                return;
            //if (keyStroke < 50)
            //    formMain.txtStroke.Text = "50";

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                return;

            //ValidatingStroke();
        }

        private void OptMaxSpeedType_mms_CheckedChanged(object sender, EventArgs e) {
            //ValidatingVmax();

            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                try {
                    if (formMain.cboMaxSpeedUnit.Text == "mm/s")
                        formMain.txtMaxSpeed.Text = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead).ToString("#0");
                    else
                        formMain.txtMaxSpeed.Text = formMain.page2.calc.MMS_TO_RPM(Convert.ToDouble(formMain.txtMaxSpeed.Text), lead).ToString();
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
