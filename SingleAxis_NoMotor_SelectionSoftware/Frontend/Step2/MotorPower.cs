using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class MotorPower {
        // 馬達自訂參數
        public (double ratedTorque, double maxTorque, double rotateInertia) customMotorParams = (-1, -1, -1);
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
            var motorParams = formMain.step2.calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));

            formMain.txtRatedTorque.Text = motorParams.ratedTorque.ToString();
            formMain.txtMaxTorque.Text = motorParams.maxTorque.ToString();
            formMain.txtRotateInertia.Text = motorParams.rotateInertia.ToString("0." + new string('#', 339));
        }

        private void UpdateMotorOptionsEnabled(object sender, EventArgs e) {
            formMain.panelPowerModifyMode.Enabled = formMain.cboPower.Text.Contains("自訂");
            formMain.panelPowerSelection.Enabled = formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifySimple.Checked;
            formMain.panelMotorParams.Enabled = formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifyAdvance.Checked;
        }

        public void Load() {
            // 馬達參數功率選擇
            formMain.cboMotorParamsMotorPowerSelection.DataSource = formMain.step2.calc.motorInfo.Rows.Cast<DataRow>().Select(row => row["Power"]).ToArray();
        }

        public void UpdateMotorCalcMode() {
            formMain.cboPower.DataSource = null;

            if (formMain.optCalcAllModel.Checked) {
                formMain.cboPower.DataSource = new string[] { "標準", "自訂" };
            } else {
                formMain.cboPower.Items.Clear();
                formMain.step2.calc.modelInfo.Rows.Cast<DataRow>().First(row => row["Model"].ToString() == formMain.cboModel.Text && Convert.ToInt32(row["Lead"].ToString()) == Convert.ToInt32(formMain.cboLead.Text))
                                                       ["Power"].ToString().Split('&').ToList()
                                                       .ForEach(power => formMain.cboPower.Items.Add("標準-" + power + "W"));
                formMain.cboPower.Items.Add("自訂");
                formMain.cboPower.SelectedIndex = 0;
            }
        }
    }
}
