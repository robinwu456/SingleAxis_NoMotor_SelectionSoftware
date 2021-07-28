using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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

        private void InitEvents() {
            formMain.cboPower.SelectedValueChanged += UpdateMotorOptionsEnabled;
            formMain.optMotorParamsModifySimple.CheckedChanged += UpdateMotorOptionsEnabled;
            formMain.cboMotorParamsMotorPowerSelection.SelectedValueChanged += CboMotorParamsMotorPowerSelection_SelectedValueChanged;
        }

        private void CboMotorParamsMotorPowerSelection_SelectedValueChanged(object sender, EventArgs e) {
            var motorParams = formMain.page2.calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));

            formMain.txtRatedTorque.Text = motorParams.ratedTorque.ToString();
            formMain.txtMaxTorque.Text = motorParams.maxTorque.ToString();
            formMain.txtRotateInertia.Text = motorParams.rotateInertia.ToString("0." + new string('#', 339));
            formMain.txtLoadInertiaMomentRatio.Text = motorParams.loadInertiaMomentRatio.ToString();
        }

        private void UpdateMotorOptionsEnabled(object sender, EventArgs e) {
            formMain.panelPowerModifyMode.Enabled = formMain.cboPower.Text.Contains("自訂");
            formMain.panelPowerSelection.Enabled = formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifySimple.Checked;
            formMain.panelMotorParams.Enabled = formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifyAdvance.Checked;
        }

        public void Load() {
            // 馬達參數功率選擇
            formMain.cboMotorParamsMotorPowerSelection.DataSource = formMain.page2.calc.motorInfo.Rows.Cast<DataRow>().Select(row => row["馬達瓦數"]).ToArray();
        }

        public void UpdateMotorCalcMode() {
            formMain.cboPower.DataSource = null;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection) {
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
