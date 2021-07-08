using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ModelSelection {
        //public string series;
        //public string model;
        //public decimal lead;
        //public int reducerRatio;

        private FormMain formMain;
        public ModelSelection(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            formMain.cboModel.SelectedValueChanged += CboModel_SelectedValueChanged;
            formMain.cboModel.LostFocus += CboModel_LostFocus;
            formMain.cboReducerRatio.SelectedValueChanged += CboReducerRatio_SelectedValueChanged;
        }

        private void CboReducerRatio_SelectedValueChanged(object sender, EventArgs e) {
            DataGridViewRow curRow = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().First(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text);
            //DataGridViewComboBoxCell cboReducerRatio = curRow.Cells["columnReducerRatio"].Value as DataGridViewComboBoxCell;
            curRow.Cells["columnReducerRatio"].Value = formMain.cboReducerRatio.Text;
        }

        public void UpdateModel() {
            var allDiffModel = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                               .Select(row => row["Model"].ToString())
                                                               .Distinct();
            formMain.cboModel.DataSource = allDiffModel.ToList();
            formMain.cboModel.Text = "";
            formMain.cboLead.DataSource = null;
        }

        private void CboModel_LostFocus(object sender, EventArgs e) {
            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString());
            if (leads.Count() == 0)
                formMain.cboLead.DataSource = null;
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            // 導程
            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString());
            formMain.cboLead.DataSource = leads.ToList();

            // 減速比
            var reducerRatios = formMain.page2.calc.reducerInfo.Rows.Cast<DataRow>()
                                                                    .Where(row => row["Model"].ToString() == formMain.cboModel.Text)
                                                                    .Select(row => row["ReducerRatio"].ToString().Split('、'));
            formMain.panelModelSelectionReducerRatio.Visible = reducerRatios.Count() != 0;
            if (reducerRatios.Count() != 0)
                formMain.cboReducerRatio.DataSource = reducerRatios.First().ToList();

            // 馬達選項更新
            formMain.page2.motorPower.UpdateMotorCalcMode();
            formMain.page2. motorPower.Load();

            // 使用環境更新
            Model.UseEnvironment curModelUseEnv = formMain.page2.calc.GetModelUseEnv(formMain.cboModel.Text);
            if (curModelUseEnv == Model.UseEnvironment.Standard)
                formMain.optStandardEnv.Checked = true;
            else if (curModelUseEnv == Model.UseEnvironment.DustFree)
                formMain.optDustFreeEnv.Checked = true;

            // 傳動方式更新
            Model.ModelType curModelType = formMain.page2.calc.GetModelType(formMain.cboModel.Text);
            formMain.page2.modelTypeOptMap.First(pair => pair.Value == curModelType).Key.Checked = true;            
        }
    }
}
