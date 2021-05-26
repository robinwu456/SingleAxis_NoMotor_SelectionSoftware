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

            InitEvents();
        }

        private void InitEvents() {
            // 使用環境
            formMain.picStandardEnv.MouseDown += PicStandardEnv_MouseDown;
            formMain.picDustFree.MouseDown += PicDustFree_MouseDown;
            formMain.optStandardEnv.CheckedChanged += OptStandardEnv_CheckedChanged;

            // 機構型態
            formMain.optRepeatabilityScrew.CheckedChanged += OptRepeatabilityScrew_CheckedChanged;
            formMain.cboModelType.SelectedValueChanged += CboModelType_SelectedValueChanged;

            // 側邊欄更新
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optDustFreeEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optUpsideDownUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optWallHangingUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optHorizontalUse.CheckedChanged += formMain.sideTable.Update;
            formMain.cboModelType.SelectedValueChanged += formMain.sideTable.Update;

            // 選型方式
            formMain.cboSeries.SelectedValueChanged += CboSeries_SelectedValueChanged;
            formMain.cboModel.SelectedValueChanged += CboModel_SelectedValueChanged;
            formMain.optCalcAllModel.CheckedChanged += OptCalcAllModel_CheckedChanged;

            // 確認按鈕
            formMain.cmdConfirmStep1.Click += CmdConfirmStep1_Click;
        }

        private void OptStandardEnv_CheckedChanged(object sender, EventArgs e) {
            // 更新機構型態
            OptRepeatabilityScrew_CheckedChanged(null, null);

            // 無塵模式時，反灰皮帶機構型態
            if (formMain.optDustFreeEnv.Checked)
                formMain.optRepeatabilityScrew.Checked = true;
            formMain.optRepeatabilityBelt.Enabled = !formMain.optDustFreeEnv.Checked;

            // 取系列
            Model.ModelType curType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text);
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
            var series = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                           .Where(row => (Model.ModelType)Convert.ToInt32(row["Type"].ToString()) == curType)
                                                           .Where(row => (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString()) == curEnv)
                                                           .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                           .Distinct();            
        }

        private void OptRepeatabilityScrew_CheckedChanged(object sender, EventArgs e) {
            ////// 側邊欄位更新
            //formMain.sideTable.UpdateItem();
            //formMain.sideTable.UpdateTableSelections();

            // 機構型態選項匯入
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
            Model.ModelType[] dbAllModelType = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                                                 .Where(row => (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString()) == curEnv)
                                                                                 .Select(row => (Model.ModelType)Convert.ToInt32(row["Type"].ToString()))
                                                                                 .Distinct()
                                                                                 .ToArray();
            formMain.cboModelType.DataSource = null;
            if (formMain.optRepeatabilityScrew.Checked)
                formMain.cboModelType.DataSource = dbAllModelType.Where(type => !type.ToString().Contains("皮帶")).ToArray();
            else
                formMain.cboModelType.DataSource = dbAllModelType.Where(type => type.ToString().Contains("皮帶")).ToArray();            
        }

        public void Load() {
            // 機構型態選項匯入
            OptRepeatabilityScrew_CheckedChanged(null, null);
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            // 側邊欄位更新
            formMain.sideTable.UpdateItem();
            formMain.sideTable.UpdateTableSelections();

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

        private void CboModelType_SelectedValueChanged(object sender, EventArgs e) {
            if (formMain.cboModelType.Text == "")
                return;

            // 取系列
            Model.ModelType curType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text);            
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
            formMain.cboSeries.DataSource = null;
            formMain.cboSeries.DataSource = formMain.step2.calc.modelInfo.Rows.Cast<DataRow>()
                                                                              .Where(row => (Model.ModelType)Convert.ToInt32(row["Type"].ToString()) == curType)
                                                                              .Where(row => (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString()) == curEnv)
                                                                              .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                                              .Distinct()
                                                                              .ToArray();
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
            formMain.sideTable.UpdateItem();
            formMain.sideTable.UpdateTableSelections();
        }
    }
}
