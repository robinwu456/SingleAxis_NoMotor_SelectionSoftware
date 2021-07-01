using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ModelSelection {
        public string series;
        public string model;
        public decimal lead;

        private FormMain formMain;
        public ModelSelection(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            formMain.cboSeries.SelectedValueChanged += CboSeries_SelectedValueChanged;
            formMain.cboModel.SelectedValueChanged += CboModel_SelectedValueChanged;

            // 選型方式
            formMain.optConditionSelection.CheckedChanged += UpdateSelections;
            formMain.optModelSelection.CheckedChanged += UpdateSelections;

            // 使用環境
            foreach (Control control in formMain.panelUseEnv.Controls.All())
                if (control is RadioButton)
                    (control as RadioButton).CheckedChanged += UpdateSelections;

            // 傳動方式
            foreach (Control control in formMain.panelModelType.Controls.All())
                if (control is RadioButton)
                    (control as RadioButton).CheckedChanged += UpdateSelections;
        }

        private void CboSeries_SelectedValueChanged(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(formMain.cboSeries.Text))
                return;

            var models = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                .Where(row => row["Model"].ToString().StartsWith(formMain.cboSeries.Text))
                                                .Select(row => row["Model"].ToString())
                                                .Distinct();
            formMain.cboModel.Items.Clear();
            models.ToList().ForEach(model => formMain.cboModel.Items.Add(model));
            formMain.cboModel.SelectedIndex = 0;
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(formMain.cboModel.Text))
                return;

            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString());
            formMain.cboLead.Items.Clear();
            leads.ToList().ForEach(lead => formMain.cboLead.Items.Add(lead));
            formMain.cboLead.SelectedIndex = 0;
        }

        public void UpdateSelections(object sender, EventArgs e) {
            IEnumerable<string> series = null;

            if (formMain.page2.modelSelectionMode == Page2.ModelSelectionMode.ConditionSelection) {
                // 條件選型
                Model.ModelType curType = formMain.page2.curSelectModelType;
                Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
                series = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                               .Where(row => (Model.ModelType)Convert.ToInt32(row["Type"].ToString()) == curType)
                                                               .Where(row => (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString()) == curEnv)
                                                               .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                               .Distinct();
            } else if (formMain.page2.modelSelectionMode == Page2.ModelSelectionMode.ModelSelection) {
                // 型號選型
                series = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                               .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                               .Distinct();
            }

            formMain.cboSeries.Items.Clear();
            series.ToList().ForEach(s => formMain.cboSeries.Items.Add(s));
        }
    }
}
