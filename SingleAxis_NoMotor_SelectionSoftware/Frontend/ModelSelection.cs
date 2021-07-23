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
            //formMain.cboReducerRatio.SelectedValueChanged += CboReducerRatio_SelectedValueChanged;
            formMain.cboLead.SelectedValueChanged += CboLead_SelectedValueChanged;
        }        

        //private void CboReducerRatio_SelectedValueChanged(object sender, EventArgs e) {
        //    if (!formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text))
        //        return;

        //    DataGridViewRow curRow = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().First(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text);
        //    //DataGridViewComboBoxCell cboReducerRatio = curRow.Cells["columnReducerRatio"].Value as DataGridViewComboBoxCell;
        //    curRow.Cells["columnReducerRatio"].Value = formMain.cboReducerRatio.Text;
        //}

        public void InitModelSelectionCbo() {
            var allDiffModel = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                               .Select(row => row["Model"].ToString())
                                                               .Distinct();
            formMain.cboModel.DataSource = allDiffModel.ToList();
            formMain.cboModel.Text = "";
            formMain.sideTable.ClearMsg();
            formMain.cboLead.DataSource = null;
            formMain.cboPower.Items.Clear();
        }

        private void CboModel_LostFocus(object sender, EventArgs e) {
            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString());
            if (leads.Count() == 0) {
                // 型號搜尋不到時
                formMain.cboLead.DataSource = null;
                formMain.sideTable.ClearModelInfo();
                formMain.sideTable.ClearModelImg();
                formMain.sideTable.ClearMsg();
                formMain.sideTable.ClearSelectedModelInfo();
                formMain.cboReducerRatio.DataSource = null;
                formMain.cmdConfirmStep2.Visible = false;
            } else {
                // 型號搜尋到時
                formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                formMain.sideTable.UpdateModelImg(formMain.cboModel.Text);
            }
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            // 導程
            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["Model"].ToString().Equals(formMain.cboModel.Text))
                                               .Select(row => row["Lead"].ToString());
            formMain.cboLead.DataSource = leads.ToList();

            // 減速比
            //var reducerRatios = formMain.page2.calc.reducerInfo.Rows.Cast<DataRow>()
            //                                                        .Where(row => row["Model"].ToString() == formMain.cboModel.Text)
            //                                                        .Select(row => row["ReducerRatio"].ToString().Split('、'));
            var reducerRatios = formMain.page2.calc.beltInfo.Rows.Cast<DataRow>().Where(row => row["Model"].ToString().StartsWith(formMain.cboModel.Text))
                                                                                 .Select(row => row["Model"].ToString().Split('-')[1]);
            formMain.panelModelSelectionReducerRatio.Visible = reducerRatios.Count() != 0;            
            if (reducerRatios.Count() != 0)
                formMain.cboReducerRatio.DataSource = reducerRatios.First().ToList();

            // 馬達選項更新
            formMain.page2.motorPower.UpdateMotorCalcMode();
            formMain.page2. motorPower.Load();

            // 使用環境更新
            Model.UseEnvironment curModelUseEnv = formMain.page2.calc.GetModelUseEnv(formMain.cboModel.Text);
            if (curModelUseEnv == Model.UseEnvironment.標準)
                formMain.optStandardEnv.Checked = true;
            else if (curModelUseEnv == Model.UseEnvironment.無塵)
                formMain.optDustFreeEnv.Checked = true;

            // 傳動方式更新
            Model.ModelType curModelType = formMain.page2.calc.GetModelType(formMain.cboModel.Text);
            formMain.page2.modelTypeOptMap.First(pair => pair.Value == curModelType).Key.Checked = true;
            formMain.sideTable.UpdateMsg(formMain.page2.calc.GetModelTypeComment(curModelType, curModelUseEnv), SideTable.MsgStatus.Normal);

            // 更新側邊型號
            if (leads.Count() != 0) {
                formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                formMain.sideTable.UpdateModelImg(formMain.cboModel.Text);
            } else {
                formMain.sideTable.ClearModelInfo();
                formMain.sideTable.ClearModelImg();
            }

            // 歐規皮帶導程隱藏
            formMain.panelModelSelectionLead.Visible = !curModelType.IsContainsReducerRatioType();
            formMain.page2.ReplaceItem();
        }

        private void CboLead_SelectedValueChanged(object sender, EventArgs e) {
            if (formMain.cboModel.Text == "" || formMain.cboLead.Text == "")
                return;

            Condition con = new Condition();
            if (formMain.optHorizontalUse.Checked)
                con.setupMethod = Model.SetupMethod.水平;
            else if (formMain.optWallHangingUse.Checked)
                con.setupMethod = Model.SetupMethod.橫掛;
            else if (formMain.optVerticalUse.Checked)
                con.setupMethod = Model.SetupMethod.垂直;

            // 更新荷重
            double maxLoad = formMain.page2.calc.GetMaxLoad(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text), con);
            if (maxLoad == int.MaxValue)
                maxLoad = RunCondition.defaultMaxLoad;
            formMain.page2.runCondition.scrollBarLoad.maxValue = (int)maxLoad;
            // 更新行程
            int maxStroke = formMain.page2.calc.GetMaxStroke(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
            formMain.page2.runCondition.scrollBarStroke.maxValue = maxStroke;

        }
    }
}
