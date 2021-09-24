using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class InputValidate {
        private FormMain formMain;    
        private System.Threading.Timer vMaxGetRPMCounter;
        private TextBox[] allowDecimalPoint;
        public Thread threadShowRPMCounting;

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
            formMain.cboModel.SelectedValueChanged += UpdateMaxValue;
            formMain.cboLead.SelectedValueChanged += UpdateMaxValue;
            formMain.cboReducerRatio.SelectedValueChanged += UpdateMaxValue;
            formMain.txtStroke.TextChanged += UpdateMaxValue;
            formMain.txtMaxSpeed.TextChanged += UpdateMaxValue;
            formMain.cboMaxSpeedUnit.SelectedValueChanged += UpdateMaxValue;            
        }

        private void TxtMaxSpeed_LostFocus(object sender, EventArgs e) {
            TxtMaxSpeed_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        }

        public void ShowConvertRPM() {
            while (true) {
                try {
                    formMain.Invoke(new Action(() => {
                        if (!formMain.cboMaxSpeedUnit.DroppedDown) {
                            if (double.TryParse(formMain.cboLead.Text, out double lead) && decimal.TryParse(formMain.txtMaxSpeed.Text, out decimal maxSpeed)) {
                                if (formMain.cboMaxSpeedUnit.Text == "mm/s")
                                    //RPM 顯示
                                    //formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)).ToString();
                                    formMain.lbRpm.Text = string.Format("轉速：{0}RPM", formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)));
                                else
                                    // Vmax 顯示
                                    //formMain.lbRpm.Text = "Vmax: " + formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead).ToString("#0");
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

        private void UpdateMaxValue(object sender, EventArgs e) {
            // 條件選型不顯示最大值
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                return;
            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal stroke))
                return;
            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return;

            var modelName = formMain.cboModel.Text;
            var model = formMain.page2.calc.GetAllModels(formMain.page2.runCondition.curCondition).First(_m => _m.name.StartsWith(modelName) && _m.lead == lead);
            var maxRPM = formMain.page2.calc.GetRpmByStroke(modelName, lead, (int)stroke);
            var maxVmax = formMain.page2.calc.RPM_TO_MMS(maxRPM, lead);
            if (formMain.cboMaxSpeedUnit.Text == "mm/s")
                formMain.lbMaxValue_MaxSpeed.Text = string.Format("( 最大值：{0} {1} )", maxVmax.ToString("#0"), formMain.cboMaxSpeedUnit.Text);
            else
                formMain.lbMaxValue_MaxSpeed.Text = string.Format("( 最大值：{0} {1} )", maxRPM, formMain.cboMaxSpeedUnit.Text);
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

        //public void TxtAccelSpeed_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
        //    int curAccelSpeed = Convert.ToInt32(formMain.txtAccelSpeed.Text);
        //    if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
        //        string model = formMain.cboModel.Text;
        //        double lead = Convert.ToDouble(formMain.cboLead.Text);
        //        //int reducerRatio = 1;
        //        //if (formMain.page2.calc.IsContainsReducerRatio(model)) {
        //        //    //string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
        //        //    //reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
        //        //    reducerRatio = Convert.ToInt32(formMain.cboReducerRatio.Text);
        //        //    lead /= reducerRatio;
        //        //}
        //        Model m = formMain.page2.calc.GetAllModels(formMain.page2.runCondition.curCondition).First(_m => _m.name.StartsWith(model) && _m.lead == lead);
        //        int maxAccelSpeed = formMain.page2.calc.GetMaxAccelSpeed(m, Convert.ToInt32(formMain.txtStroke.Text), m.modelType);
        //        if (curAccelSpeed > maxAccelSpeed)
        //            formMain.txtAccelSpeed.Text = maxAccelSpeed.ToString();
        //    }
        //}

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

        //public bool ValidatingLoad(bool isShowAlarm = true) {
        //    if (!decimal.TryParse(formMain.txtLoad.Text, out decimal keyLoad))
        //        return true;
        //    if (!double.TryParse(formMain.cboLead.Text, out double lead))
        //        return true;

        //    Condition con = new Condition();
        //    if (formMain.optHorizontalUse.Checked)
        //        con.setupMethod = Model.SetupMethod.Horizontal;
        //    else if (formMain.optWallHangingUse.Checked)
        //        con.setupMethod = Model.SetupMethod.WallHang;
        //    else if (formMain.optVerticalUse.Checked)
        //        con.setupMethod = Model.SetupMethod.Vertical;

        //    string model = formMain.cboModel.Text;

        //    double maxLoad = formMain.page2.calc.GetMaxLoad(model, lead, con);
        //    formMain.labelLoadAlarm.Text = "最大: " + maxLoad.ToString() + "kg";
        //    if (maxLoad == -1)
        //        return true;

        //    if (isShowAlarm)
        //        formMain.labelLoadAlarm.Visible = (double)keyLoad > maxLoad;
        //    if ((double)keyLoad > maxLoad) {
        //        formMain.txtLoad.Text = ((int)maxLoad).ToString();
        //        return false;
        //    }
        //    return true;
        //}

        public bool VerifyAllInputValidate(bool isShowAlarm = false) {
            bool isVerifySuccess = true;

            // TextBox數值有效驗證            
            foreach (Control control in formMain.panelCalc.Controls.All().Concat(formMain.panelMoment.Controls.All())) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;
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
        //public bool ValidatingStroke(bool isShowAlarm = true) {
        //    if (!decimal.TryParse(formMain.txtStroke.Text, out decimal keyStroke))
        //        return true;
        //    if (!double.TryParse(formMain.cboLead.Text, out double lead))
        //        return true;

        //    string model = formMain.cboModel.Text;
        //    int reducerRatio = 1;
        //    //if (calc.IsContainsReducerRatio(model)) {
        //    //    string dgvReducerRatioValue = dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
        //    //    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
        //    //}

        //    int maxStroke = formMain.page2.calc.GetMaxStroke(model, lead, reducerRatio);
        //    formMain.labelStrokeAlarm.Text = "最大: " + maxStroke.ToString() + "mm";
        //    if (isShowAlarm)
        //        formMain.labelStrokeAlarm.Visible = (double)keyStroke > maxStroke;
        //    if (keyStroke > maxStroke) {
        //        formMain.txtStroke.Text = maxStroke.ToString();
        //        return false;
        //    }

        //    return true;
        //}

        private void TxtMaxSpeed_KeyDown(object sender, KeyEventArgs e) {
            // 一般計算
            if (e.KeyCode != Keys.Enter)
                return;

            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal stroke))
                return;
            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return;
            if (!decimal.TryParse(formMain.txtMaxSpeed.Text, out decimal vMax))
                return;

            string model = formMain.cboModel.Text;

            try {
                Model m = formMain.page2.calc.GetAllModels(formMain.page2.runCondition.curCondition).First(_m => _m.name.StartsWith(model) && _m.lead == lead);
                if (formMain.cboMaxSpeedUnit.Text == "mm/s") {
                    double resultVmax = formMain.page2.calc.GetVmax_mms(m, lead, (int)stroke);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultVmax)
                        formMain.txtMaxSpeed.Text = resultVmax.ToString();

                    //// RPM 顯示                
                    //formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)).ToString();
                    //formMain.txtMaxSpeed.Text = resultVmax.ToString();
                } else if (formMain.cboMaxSpeedUnit.Text == "RPM") {
                    double resultVmax = formMain.page2.calc.GetVmax_mms(m, lead, (int)stroke);
                    int resultRpm = formMain.page2.calc.MMS_TO_RPM(resultVmax, lead);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultRpm)
                        formMain.txtMaxSpeed.Text = resultRpm.ToString();

                    //// RPM 顯示
                    //if (formMain.txtMaxSpeed.Text.Contains("."))
                    //    formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                    //double _mms = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead);
                    //formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, _mms).ToString();
                    //formMain.lbRpm.Text = "Vmax: " + _mms.ToString("#0.00");
                    //formMain.txtMaxSpeed.Text = resultRpm.ToString();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private void OptMaxSpeedType_mms_CheckedChanged(object sender, EventArgs e) {
            //ValidatingVmax();

            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                if (formMain.cboMaxSpeedUnit.Text == "mm/s")
                    formMain.txtMaxSpeed.Text = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead).ToString();
                else
                    formMain.txtMaxSpeed.Text = formMain.page2.calc.MMS_TO_RPM(Convert.ToDouble(formMain.txtMaxSpeed.Text), lead).ToString();
            }
        }

        //private void ValidatingVmax(object state = null) {
            //formMain.Invoke(new Action(() => {
            //    if (!decimal.TryParse(formMain.txtStroke.Text, out decimal stroke))
            //        return;
            //    if (!double.TryParse(formMain.cboLead.Text, out double lead))
            //        return;
            //    if (!decimal.TryParse(formMain.txtMaxSpeed.Text, out decimal vMax))
            //        return;

            //    string model = formMain.cboModel.Text;

            //    try {
            //        Model m = formMain.page2.calc.GetAllModels(formMain.page2.runCondition.curCondition).First(_m => _m.name.StartsWith(model) && _m.lead == lead);
            //        if (formMain.cboMaxSpeedUnit.Text == "mm/s") {
            //            double resultVmax = formMain.page2.calc.GetVmax_mms(m, lead, (int)stroke);
            //            // vMax key過大修正
            //            if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultVmax)
            //                formMain.txtMaxSpeed.Text = resultVmax.ToString();

            //            // RPM 顯示                
            //            formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)).ToString();
            //            //formMain.txtMaxSpeed.Text = resultVmax.ToString();
            //        } else if (formMain.cboMaxSpeedUnit.Text == "RPM") {
            //            double resultVmax = formMain.page2.calc.GetVmax_mms(m, lead, (int)stroke);
            //            int resultRpm = formMain.page2.calc.MMS_TO_RPM(resultVmax, lead);
            //            // vMax key過大修正
            //            if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultRpm)
            //                formMain.txtMaxSpeed.Text = resultRpm.ToString();

            //            // RPM 顯示
            //            if (formMain.txtMaxSpeed.Text.Contains("."))
            //                formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
            //            double _mms = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead);
            //            //formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, _mms).ToString();
            //            formMain.lbRpm.Text = "Vmax: " + _mms.ToString("#0.00");
            //            //formMain.txtMaxSpeed.Text = resultRpm.ToString();
            //        }
            //    } catch (Exception ex) {
            //        Console.WriteLine(ex);
            //    }
            //}));
        //}
    }
}
