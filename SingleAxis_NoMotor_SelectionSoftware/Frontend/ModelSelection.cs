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
            formMain.cboModel.DataSource = null;
            formMain.cboModel.DataSource = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                .Where(row => row["Model"].ToString().StartsWith(formMain.cboSeries.Text))
                                                .Select(row => row["Model"].ToString())
                                                .Distinct()
                                                .ToArray();
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            formMain.cboLead.DataSource = null;
            formMain.cboLead.DataSource = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString())
                                               .ToArray();
        }

        public void UpdateSelections(object sender, EventArgs e) {
            // 取系列
            Model.ModelType curType = formMain.page2.curSelectModelType;
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
            Model.SetupMethod curSetup = Model.SetupMethod.Horizontal;
            if (formMain.optHorizontalUse.Checked)
                curSetup = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                curSetup = Model.SetupMethod.WallHang;
            else if (formMain.optUpsideDownUse.Checked)
                curSetup = Model.SetupMethod.Vertical;
            var series = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                           .Where(row => (Model.ModelType)Convert.ToInt32(row["Type"].ToString()) == curType)
                                                           .Where(row => (Model.UseEnvironment)Convert.ToInt32(row["Env"].ToString()) == curEnv)
                                                           //.Where(row => row["Setup"].ToString().Split('&').Contains(((int)curSetup).ToString()))
                                                           .Select(row => new Regex(@"([A-Z]+).+").Match(row["Model"].ToString()).Groups[1].Value)
                                                           .Distinct()
                                                           .ToArray();
            formMain.cboSeries.DataSource = null;
            formMain.cboSeries.DataSource = series;
        }
    }
}
