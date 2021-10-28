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
            formMain.cboLead.SelectedValueChanged += CboLead_SelectedValueChanged;
        }        

        public void InitModelSelectionCbo() {
            var allDiffModel = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                               .Select(row => row["型號"].ToString().Split('-')[0])
                                                               .Distinct();
            formMain.cboModel.DataSource = allDiffModel.ToList();
            formMain.cboModel.Text = "";
            formMain.sideTable.ClearMsg();
            formMain.sideTable.UpdateSelectedConditionValue("機構型態", "");            
            formMain.cboLead.DataSource = null;
            formMain.cboPower.Items.Clear();
        }

        private void CboModel_LostFocus(object sender, EventArgs e) {
            var leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                               .Where(row => row["型號"].ToString().StartsWith(formMain.cboModel.Text))
                                               .Select(row => row["導程"].ToString());
            if (leads.Count() == 0) {
                // 型號搜尋不到時
                formMain.cboLead.DataSource = null;
                formMain.sideTable.ClearModelInfo();
                formMain.sideTable.ClearModelImg();
                formMain.sideTable.ClearMsg();
                formMain.sideTable.ClearSelectedModelInfo();
                formMain.cboReducerRatio.DataSource = null;
                //formMain.cmdConfirmStep2.Visible = false;
                formMain.page2.ChangeNextStepBtnVisible(false);
            } else {
                // 型號搜尋到時
                //Model.ModelType curModelType = formMain.page2.calc.GetModelType(formMain.cboModel.Text);
                if (formMain.cboModel.Text.IsContainsReducerRatioType())
                    formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToInt32(formMain.cboReducerRatio.Text));
                else
                    formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                formMain.sideTable.UpdateModelImg(formMain.cboModel.Text);
            }
        }

        private void CboModel_SelectedValueChanged(object sender, EventArgs e) {
            // 導程
            List<string> leads = new List<string>();
            if (formMain.cboModel.Text.StartsWith("M"))
                leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                   .Where(row => row["型號"].ToString().StartsWith(formMain.cboModel.Text))
                                                   .Select(row => row["導程"].ToString()).ToList();
            else
                leads = formMain.page2.calc.modelInfo.Rows.Cast<DataRow>()
                                                   .Where(row => row["型號"].ToString().Equals(formMain.cboModel.Text))
                                                   .Select(row => row["導程"].ToString()).ToList();
            formMain.cboLead.DataSource = leads;

            // 減速比
            //var reducerRatios = formMain.page2.calc.reducerInfo.Rows.Cast<DataRow>()
            //                                                        .Where(row => row["型號"].ToString() == formMain.cboModel.Text)
            //                                                        .Select(row => row["ReducerRatio"].ToString().Split('、'));
            var reducerRatios = formMain.page2.calc.beltInfo.Rows.Cast<DataRow>().Where(row => row["型號"].ToString().StartsWith(formMain.cboModel.Text) && row["型號"].ToString().Contains("-"));
            formMain.panelModelSelectionReducerRatio.Visible = reducerRatios.Count() != 0;
            if (reducerRatios.Count() != 0) {
                //formMain.cboReducerRatio.DataSource = reducerRatios.First().ToList();
                //formMain.cboReducerRatio.DataSource = reducerRatios.ToList();
                formMain.cboReducerRatio.DataSource = reducerRatios.Select(row => row["型號"].ToString().Split('-')[1]).ToList();
            }

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
            formMain.sideTable.UpdateMsg(formMain.page2.calc.GetModelTypeComment(curModelType), SideTable.MsgStatus.Normal);
            formMain.sideTable.Update(null, null);

            // 更新側邊型號
            if (leads.Count() != 0) {
                if (formMain.cboModel.Text.IsContainsReducerRatioType())
                    formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToInt32(formMain.cboReducerRatio.Text));
                else
                    formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                formMain.sideTable.UpdateModelImg(formMain.cboModel.Text);
            } else {
                formMain.sideTable.ClearModelInfo();
                formMain.sideTable.ClearModelImg();
            }

            // 歐規皮帶導程隱藏
            formMain.panelModelSelectionLead.Visible = !formMain.cboModel.Text.IsContainsReducerRatioType();
            //formMain.page2.ReplaceItem();
        }

        private void CboLead_SelectedValueChanged(object sender, EventArgs e) {
            if (formMain.cboModel.Text == "" || formMain.cboLead.Text == "")
                return;
            if (formMain.cboModel.Text.IsContainsReducerRatioType() && formMain.cboReducerRatio.Text == "")
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

            if (formMain.cboModel.Text.IsContainsReducerRatioType())
                formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToInt32(formMain.cboReducerRatio.Text));
            else
                formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));

            // 安裝方式
            Model.SetupMethod[] modelTypeSupportSetupMethod = formMain.page2.calc.GetSupportSetup(formMain.cboModel.Text);
            formMain.page2.setupMethodOptMap.ToList().ForEach(pair => pair.Key.Enabled = modelTypeSupportSetupMethod.Contains(pair.Value));
            if (formMain.page2.setupMethodOptMap.ToList().Any(pair => pair.Key.Checked && !pair.Key.Enabled))
                formMain.page2.setupMethodOptMap.ToList().First(pair => pair.Key.Enabled).Key.Checked = true;
            formMain.page2.setupMethodPicOptMap.ToList().ForEach(pair => pair.Value.Visible = pair.Key.Enabled);
        }

        private void CboReducerRatio_SelectedValueChanged(object sender, EventArgs e) {
            formMain.sideTable.UpdateModeInfo(formMain.cboModel.Text, Convert.ToInt32(formMain.cboReducerRatio.Text));
            formMain.cboLead.SelectedIndex = formMain.cboReducerRatio.SelectedIndex;

            // 更新荷重
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                Condition con = new Condition();
                if (formMain.optHorizontalUse.Checked)
                    con.setupMethod = Model.SetupMethod.水平;
                else if (formMain.optWallHangingUse.Checked)
                    con.setupMethod = Model.SetupMethod.橫掛;
                else if (formMain.optVerticalUse.Checked)
                    con.setupMethod = Model.SetupMethod.垂直;
                double maxLoad = formMain.page2.calc.GetMaxLoad(formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text), con);
                if (maxLoad == int.MaxValue)
                    maxLoad = RunCondition.defaultMaxLoad;
                formMain.page2.runCondition.scrollBarLoad.maxValue = (int)maxLoad;
            }
        }
    }
}
