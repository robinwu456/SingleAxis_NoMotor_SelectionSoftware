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
            foreach (Control control in formMain.explorerBarPanel2_content.Controls.All()) {
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
        }        

        private void InputCondition_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            TextBox txt = sender as TextBox;
            // 有效數值驗證
            Control lbAlarm = formMain.explorerBarPanel2_content.Controls.All().First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
            if (!decimal.TryParse(txt.Text, out value))
                e.Cancel = true;
        }
        private void InputCondition_KeyDown(object sender, KeyEventArgs e) {
            TextBox txt = sender as TextBox;
            // 有效數值驗證
            Control lbAlarm = formMain.explorerBarPanel2_content.Controls.All().First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
        }

        private void TxtLoad_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (formMain.optCalcAllModel.Checked)
                return;

            // 最大荷重驗證
            ValidatingLoad();
        }

        private void TxtLoad_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            if (formMain.optCalcAllModel.Checked)
                return;

            // 最大荷重驗證
            ValidatingLoad();
        }

        public bool ValidatingLoad() {
            if (!decimal.TryParse(formMain.txtLoad.Text, out decimal keyLoad))
                return true;
            if (!double.TryParse(formMain.cboLead.Text, out double lead))
                return true;

            Condition con = new Condition();
            if (formMain.optHorizontalUse.Checked)
                con.setupMethod = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                con.setupMethod = Model.SetupMethod.WallHang;
            else if (formMain.optUpsideDownUse.Checked)
                con.setupMethod = Model.SetupMethod.Vertical;

            string model = formMain.cboModel.Text;

            double maxLoad = formMain.step2.calc.GetMaxLoad(model, lead, con);
            formMain.labelLoadAlarm.Text = "最大: " + maxLoad.ToString() + "kg";
            if (maxLoad == -1)
                return true;

            formMain.labelLoadAlarm.Visible = (double)keyLoad > maxLoad;
            if ((double)keyLoad > maxLoad) {
                formMain.txtLoad.Text = ((int)maxLoad).ToString();
                return false;
            }
            return true;
        }

        public bool VerifyAllInputValidate() {
            // 數值有效驗證
            foreach (Control control in formMain.explorerBarPanel2_content.Controls.All()) {
                if (control is TextBox) {
                    TextBox txt = control as TextBox;
                    if (!decimal.TryParse(txt.Text, out decimal value))
                        return false;
                }
            }
            return true;
        }

        private void TxtStroke_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            if (formMain.optCalcAllModel.Checked)
                return;

            ValidatingStroke();
        }

        private void TxtStroke_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!decimal.TryParse(formMain.txtStroke.Text, out decimal keyStroke))
                return;
            if (keyStroke < 50)
                formMain.txtStroke.Text = "50";

            if (formMain.optCalcAllModel.Checked)
                return;

            ValidatingStroke();
        }
        private bool ValidatingStroke() {
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

            int maxStroke = formMain.step2.calc.GetMaxStroke(model, lead, reducerRatio);
            formMain.labelStrokeAlarm.Text = "最大: " + maxStroke.ToString() + "mm";
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
                if (formMain.step2.calc.IsContainsReducerRatio(model)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
                    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
                    lead /= reducerRatio;
                }

                if (formMain.optMaxSpeedType_mms.Checked) {
                    double resultVmax = formMain.step2.calc.GetVmax_mms(model, lead, reducerRatio, (int)stroke);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultVmax)
                        formMain.txtMaxSpeed.Text = resultVmax.ToString();

                    // RPM 顯示                
                    formMain.lbRpm.Text = "RPM: " + formMain.step2.calc.GetRpmByMMS(lead, Convert.ToDouble(formMain.txtMaxSpeed.Text)).ToString();
                } else if (formMain.optMaxSpeedType_rpm.Checked) {
                    double resultVmax = formMain.step2.calc.GetVmax_mms(model, lead, reducerRatio, (int)stroke);
                    int resultRpm = formMain.step2.calc.MMS_TO_RPM(resultVmax, lead);
                    // vMax key過大修正
                    if (Convert.ToDouble(formMain.txtMaxSpeed.Text) > resultRpm)
                        formMain.txtMaxSpeed.Text = resultRpm.ToString();

                    // RPM 顯示
                    //lbRpm.Text = "RPM: " + calc.GetRpmByMMS(lead, Convert.ToDouble(txtMaxSpeed.Text)).ToString();
                    if (formMain.txtMaxSpeed.Text.Contains("."))
                        formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                    double _mms = formMain.step2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), lead);
                    formMain.lbRpm.Text = "RPM: " + formMain.step2.calc.GetRpmByMMS(lead, _mms).ToString();
                }
            }));
        }
    }
}
