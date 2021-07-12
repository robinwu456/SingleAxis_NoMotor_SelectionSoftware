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

        public InputValidate(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        private void InitEvents() {
            // 數值有效驗證
            foreach (Control control in formMain.panelCalc.Controls.All()) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;
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
            formMain.txtMaxSpeed.TextChanged += TxtMaxSpeed_TextChanged;
            formMain.optMaxSpeedType_mms.CheckedChanged += OptMaxSpeedType_mms_CheckedChanged;

            // 加速度驗證
            formMain.txtAccelSpeed.Validating += TxtAccelSpeed_Validating;

            // 使用頻率驗證
            formMain.txtHourPerDay.Validating += UseFrequence_Validating;
            formMain.txtDayPerYear.Validating += UseFrequence_Validating;
            formMain.txtHourPerDay.KeyDown += UseFrequence_KeyDown;
            formMain.txtDayPerYear.KeyDown += UseFrequence_KeyDown;
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

        public void TxtAccelSpeed_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            int curAccelSpeed = Convert.ToInt32(formMain.txtAccelSpeed.Text);
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                string model = formMain.cboModel.Text;
                double lead = Convert.ToDouble(formMain.cboLead.Text);
                int reducerRatio = 1;
                if (formMain.page2.calc.IsContainsReducerRatio(model)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
                    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
                    lead /= reducerRatio;
                }

                int maxAccelSpeed = formMain.page2.calc.GetMaxAccelSpeed(model, lead, reducerRatio, Convert.ToInt32(formMain.txtStroke.Text));
                if (curAccelSpeed > maxAccelSpeed)
                    formMain.txtAccelSpeed.Text = maxAccelSpeed.ToString();
            }
        }

        public void InputCondition_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            TextBox txt = sender as TextBox;
            // Alarm顯示修正
            if (txt == formMain.txtHourPerDay)
                formMain.lbHoursPerDayAlarm.Text = "請輸入有效時間";
            if (txt == formMain.txtDayPerYear)
                formMain.lbDaysPerYearAlarm.Text = "請輸入有效天數";

            // 有效數值驗證
            Control lbAlarm = formMain.panelCalc.Controls.All().First(c => c.Tag == txt.Name);
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

            // 有效數值驗證
            Control lbAlarm = formMain.panelCalc.Controls.All().First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
        }

        private void TxtLoad_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection)
                return;

            // 最大荷重驗證
            ValidatingLoad();
        }

        private void TxtLoad_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection)
                return;

            // 最大荷重驗證
            ValidatingLoad();
        }

        public bool ValidatingLoad(bool isShowAlarm = true) {
            if (!decimal.TryParse(formMain.txtLoad.Text, out decimal keyLoad))
                return true;
            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return true;

            Condition con = new Condition();
            if (formMain.optHorizontalUse.Checked)
                con.setupMethod = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                con.setupMethod = Model.SetupMethod.WallHang;
            else if (formMain.optVerticalUse.Checked)
                con.setupMethod = Model.SetupMethod.Vertical;

            string model = formMain.cboModel.Text;

            double maxLoad = formMain.page2.calc.GetMaxLoad(model, lead, con);
            formMain.labelLoadAlarm.Text = "最大: " + maxLoad.ToString() + "kg";
            if (maxLoad == -1)
                return true;

            if (isShowAlarm)
                formMain.labelLoadAlarm.Visible = (double)keyLoad > maxLoad;
            if ((double)keyLoad > maxLoad) {
                formMain.txtLoad.Text = ((int)maxLoad).ToString();
                return false;
            }
            return true;
        }

        public bool VerifyAllInputValidate(bool isShowAlarm = false) {
            bool isVerifySuccess = true;

            // TextBox數值有效驗證            
            foreach (Control control in formMain.panelCalc.Controls.All()) {
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

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection)
                return;

            ValidatingStroke();
        }

        private void TxtStroke_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal keyStroke))
                return;
            //if (keyStroke < 50)
            //    formMain.txtStroke.Text = "50";

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection)
                return;

            ValidatingStroke();
        }
        public bool ValidatingStroke(bool isShowAlarm = true) {
            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal keyStroke))
                return true;
            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return true;

            string model = formMain.cboModel.Text;
            int reducerRatio = 1;
            //if (calc.IsContainsReducerRatio(model)) {
            //    string dgvReducerRatioValue = dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
            //    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
            //}

            int maxStroke = formMain.page2.calc.GetMaxStroke(model, lead, reducerRatio);
            formMain.labelStrokeAlarm.Text = "最大: " + maxStroke.ToString() + "mm";
            if (isShowAlarm)
                formMain.labelStrokeAlarm.Visible = (double)keyStroke > maxStroke;
            if (keyStroke > maxStroke) {
                formMain.txtStroke.Text = maxStroke.ToString();
                return false;
            }

            return true;
        }

        private void TxtMaxSpeed_KeyDown(object sender, KeyEventArgs e) {
            // 一般計算
            if (e.KeyCode != Keys.Enter)
                return;

            ValidatingVmax();
        }
        private void TxtMaxSpeed_TextChanged(object sender, EventArgs e) {
            if (vMaxGetRPMCounter != null)
                vMaxGetRPMCounter.Dispose();

            vMaxGetRPMCounter = new System.Threading.Timer(new TimerCallback(ValidatingVmax), null, 2000, Timeout.Infinite);
        }

        private void OptMaxSpeedType_mms_CheckedChanged(object sender, EventArgs e) {
            ValidatingVmax();
        }

        private void ValidatingVmax(object state = null) {
            formMain.Invoke(new Action(() => {
                if (!decimal.TryParse(formMain.txtStroke.Text, out decimal stroke))
                    return;
                if (!double.TryParse(formMain.cboLead.Text, out double lead))
                    return;
                if (!decimal.TryParse(formMain.txtMaxSpeed.Text, out decimal vMax))
                    return;

                string model = formMain.cboModel.Text;
                int reducerRatio = 1;
                if (formMain.page2.calc.IsContainsReducerRatio(model)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
                    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
                    lead /= reducerRatio;
                }

                if (formMain.optMaxSpeedType_mms.Checked) {
                    double resultVmax = formMain.page2.calc.GetVmax_mms(model, lead, reducerRatio, (int)stroke);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultVmax)
                        formMain.txtMaxSpeed.Text = resultVmax.ToString();

                    // RPM 顯示                
                    formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)).ToString();
                } else if (formMain.optMaxSpeedType_rpm.Checked) {
                    double resultVmax = formMain.page2.calc.GetVmax_mms(model, lead, reducerRatio, (int)stroke);
                    int resultRpm = formMain.page2.calc.MMS_TO_RPM(resultVmax, lead);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultRpm)
                        formMain.txtMaxSpeed.Text = resultRpm.ToString();

                    // RPM 顯示
                    //lbRpm.Text = "RPM: " + calc.GetRpmByMMS(lead, Convert.ToDouble(txtMaxSpeed.Text)).ToString();
                    if (formMain.txtMaxSpeed.Text.Contains("."))
                        formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                    double _mms = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead);
                    formMain.lbRpm.Text = "RPM: " + formMain.page2.calc.GetRpmByMMS(lead, _mms).ToString();
                }
            }));
        }
    }
}
