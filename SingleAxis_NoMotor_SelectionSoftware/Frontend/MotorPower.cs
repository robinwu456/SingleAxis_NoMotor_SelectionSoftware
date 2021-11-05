using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class MotorPower {
        // 馬達自訂參數
        public (double ratedTorque, double maxTorque, double rotateInertia, int loadInertiaMomentRatio) customMotorParams = (-1, -1, -1, -1);
        public int customMotorParamPower = 50;

        private FormMain formMain;
        public MotorPower(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetFocus();
        private Control GetFocusedControl() {
            Control focusedControl = null;
            // To get hold of the focused control:
            IntPtr focusedHandle = GetFocus();
            if (focusedHandle != IntPtr.Zero)
                // Note that if the focused Control is not a .Net control, then this will return null.
                focusedControl = Control.FromHandle(focusedHandle);
            return focusedControl;
        }

        private void InitEvents() {
            formMain.cboPower.SelectedValueChanged += UpdateMotorOptionsEnabled;
            formMain.chkMotorAdvanceMode.CheckedChanged += UpdateMotorOptionsEnabled;
            formMain.cboMotorParamsMotorPowerSelection.SelectedValueChanged += CboMotorParamsMotorPowerSelection_SelectedValueChanged;
            formMain.panelMotorParams.Controls.All().Where(control => control is TextBox).ToList().ForEach(control => control.TextChanged += MotorParam_TextChanged);
        }

        private void MotorParam_TextChanged(object sender, EventArgs e) {
            if (formMain.panelMotorParams.Controls.All().Any(control => control == GetFocusedControl()))
                formMain.cboMotorParamsMotorPowerSelection.Text = "自訂";
        }

        private void CboMotorParamsMotorPowerSelection_SelectedValueChanged(object sender, EventArgs e) {
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection && !formMain.cboPower.Text.Contains("自訂"))
                return;

            if (int.TryParse(formMain.cboMotorParamsMotorPowerSelection.Text, out int power)) {
                var motorParams = formMain.page2.calc.GetMotorParams(power);

                formMain.txtRatedTorque.Text = motorParams.ratedTorque.ToString();
                formMain.txtMaxTorque.Text = motorParams.maxTorque.ToString();
                formMain.txtRotateInertia.Text = motorParams.rotateInertia.ToString("0." + new string('#', 339));
                formMain.txtLoadInertiaMomentRatio.Text = motorParams.loadInertiaMomentRatio.ToString();
            }
        }

        private void UpdateMotorOptionsEnabled(object sender, EventArgs e) {
            formMain.panelMotorAdvanceMode.Enabled = formMain.cboPower.Text.Contains("自訂");
            formMain.panelPowerSelection.Enabled = formMain.cboPower.Text == "自訂" && !formMain.chkMotorAdvanceMode.Checked;
            formMain.panelMotorParams.Enabled = formMain.cboPower.Text == "自訂" && formMain.chkMotorAdvanceMode.Checked;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                if (formMain.cboPower.Text.Contains("標準")) {
                    int power = Convert.ToInt32(new Regex(@"\d+").Match(formMain.cboPower.Text).Groups[0].Value);
                    var motorParams = formMain.page2.calc.GetMotorParams(power);

                    formMain.txtRatedTorque.Text = motorParams.ratedTorque.ToString();
                    formMain.txtMaxTorque.Text = motorParams.maxTorque.ToString();
                    formMain.txtRotateInertia.Text = motorParams.rotateInertia.ToString("0." + new string('#', 339));
                    formMain.txtLoadInertiaMomentRatio.Text = motorParams.loadInertiaMomentRatio.ToString();
                }
            }
        }

        public void Load() {
            // 馬達參數功率選擇
            var powers = formMain.page2.calc.motorInfo.Rows.Cast<DataRow>().Select(row => row["馬達瓦數"]).ToList();
            //powers.Add("自訂");
            formMain.cboMotorParamsMotorPowerSelection.DataSource = powers.ToArray();
        }

        public void UpdateMotorCalcMode() {
            formMain.cboPower.DataSource = null;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
                formMain.cboPower.DataSource = new string[] { "標準", "自訂" };
            } else {
                if (string.IsNullOrEmpty(formMain.cboModel.Text) || string.IsNullOrEmpty(formMain.cboLead.Text))
                    return;

                formMain.cboPower.Items.Clear();
                formMain.page2.calc.modelInfo.Rows.Cast<DataRow>().First(row => row["型號"].ToString().StartsWith(formMain.cboModel.Text) && Convert.ToDouble(row["導程"].ToString()) == Convert.ToDouble(formMain.cboLead.Text))
                                                       ["馬達瓦數"].ToString().Split('&').ToList()
                                                       .ForEach(power => formMain.cboPower.Items.Add("標準-" + power + "W"));
                formMain.cboPower.Items.Add("自訂");
                formMain.cboPower.SelectedIndex = 0;
            }
        }
    }
}
