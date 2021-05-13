using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step1 {
        private FormMain formMain;        

        public Step1(FormMain formMain) {
            this.formMain = formMain;                                   

            // 機構型態
            formMain.cboModelType.DataSource = Enum.GetNames(typeof(Model.ModelType));
            // 選型方式-類別
            formMain.cboSystemType.DataSource = new string[] { "移載式", "推桿式" };

            InitEvents();
        }

        private void InitEvents() {
            formMain.picStandardEnv.MouseDown += PicStandardEnv_MouseDown;
            formMain.picDustFree.MouseDown += PicDustFree_MouseDown;
            formMain.cboModelType.SelectedValueChanged += CboModelType_SelectedValueChanged;

            // 側邊欄更新
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optDustFreeEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optUpsideDownUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optWallHangingUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optHorizontalUse.CheckedChanged += formMain.sideTable.Update;
            formMain.cboModelType.SelectedValueChanged += formMain.sideTable.Update;

            // 選型方式
            formMain.cboSystemType.SelectedValueChanged += CboSystemType_SelectedValueChanged;
            formMain.cboSeries.SelectedValueChanged += CboSeries_SelectedValueChanged;
            formMain.cboModel.SelectedValueChanged += CboModel_SelectedValueChanged;
            formMain.optCalcAllModel.CheckedChanged += OptCalcAllModel_CheckedChanged;

            // 確認按鈕
            formMain.cmdConfirmStep1.Click += CmdConfirmStep1_Click;
        }        

        public void Load() {
            // 系列選項匯入
            CboSystemType_SelectedValueChanged(null, null);
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            formMain.cboLead.DataSource = null;
            formMain.cboLead.DataSource = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString())
                                               .ToArray();
        }

        private void CboSeries_SelectedValueChanged(object sender, EventArgs e) {
            formMain.cboModel.DataSource = null;
            formMain.cboModel.DataSource = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                .Where(row => row["Model"].ToString().StartsWith(formMain.cboSeries.Text))
                                                .Select(row => row["Model"].ToString())
                                                .Distinct()
                                                .ToArray();
        }

        private void CboSystemType_SelectedValueChanged(object sender, EventArgs e) {
            if (formMain.cboSystemType.Text == "移載式") {
                // 取系列
                formMain.cboSeries.DataSource = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                     .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                     .Where(model => !model.StartsWith("GTY"))
                                                     .Distinct()
                                                     .ToArray();

                //groupBoxMoment.Visible = true;
                //groupBox5.Size = new Size(174, 248);
                //groupBoxRepeatability.Location = new Point(210, 274);

                // 力矩指定
                formMain.txtMomentA.Text = "100";
                formMain.txtMomentB.Text = "0";
                formMain.txtMomentC.Text = "0";
            } else {
                // 取系列
                formMain.cboSeries.DataSource = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                     .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                     .Where(model => model.StartsWith("GTY"))
                                                     .Distinct()
                                                     .ToArray();

                //groupBoxMoment.Visible = false;
                //groupBox5.Size = new Size(174, 55);
                //groupBoxRepeatability.Location = new Point(210, 81);
            }
        }

        private void CboModelType_SelectedValueChanged(object sender, EventArgs e) {
            //formMain.optRepeatabilityScrew.Checked = !formMain.cboModelType.Text.Contains("皮帶");
            //formMain.optRepeatabilityBelt.Checked = formMain.cboModelType.Text.Contains("皮帶");
        }

        private void CmdConfirmStep1_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step1) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelCalcBtns);
                formMain.step2.Load();
                formMain.step2.motorPower.UpdateMotorCalcMode();

                // Step2 只開起一半
                formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, formMain.step2.minHeight);
            }
        }

        private void PicDustFree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optDustFreeEnv.Checked = true;
        }

        private void PicStandardEnv_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optStandardEnv.Checked = true;
        }

        private void OptCalcAllModel_CheckedChanged(object sender, EventArgs e) {
            formMain.panelCalcMode.Enabled = formMain.optCalcSelectedModel.Checked;            
        }
    }
}
