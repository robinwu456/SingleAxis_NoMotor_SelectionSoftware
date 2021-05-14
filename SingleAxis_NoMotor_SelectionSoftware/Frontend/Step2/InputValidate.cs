using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class InputValidate {
        private FormMain formMain;        

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
                }
            }

            // 最大荷重驗證
            formMain.txtLoad.Validating += TxtLoad_Validating;
            formMain.txtLoad.KeyDown += TxtLoad_KeyDown;

            // 最大行程驗證
            formMain.txtStroke.Validating += TxtStroke_Validating;
            formMain.txtStroke.KeyDown += TxtStroke_KeyDown;
        }        

        private void InputCondition_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            TextBox txt = sender as TextBox;
            // 有效數值驗證
            Control lbAlarm = formMain.explorerBarPanel2_content.Controls.All().First(c => c.Tag == txt.Name);
            lbAlarm.Visible = !decimal.TryParse(txt.Text, out decimal value);
            if (!decimal.TryParse(txt.Text, out value))
                e.Cancel = true;
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
    }
}
