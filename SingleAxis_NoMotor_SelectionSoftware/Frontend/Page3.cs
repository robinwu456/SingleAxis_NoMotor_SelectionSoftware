using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Page3 {
        private FormMain formMain;

        public Page3(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        public void Load() {
            // 結果型號
            string model = formMain.page2.recommandList.curSelectModel.model;
            double lead = formMain.page2.recommandList.curSelectModel.lead;
            if (formMain.page2.recommandList.curSelectModel.model.IsContainsReducerRatioType())
                formMain.lbResult.Text = string.Format("{0} - {1} - {2}", model.Split('-')[0], formMain.page2.effectiveStroke.effectiveStroke, model.Split('-')[1]);
            else {
                if (model.Contains("-"))
                    formMain.lbResult.Text = string.Format("{0} - {1}{2} - {3}", model.Split('-')[0], formMain.page2.calc.GetLeadText(model, lead), lead, formMain.page2.effectiveStroke.effectiveStroke);
                else
                    formMain.lbResult.Text = string.Format("{0} - {1}{2} - {3}", model, formMain.page2.calc.GetLeadText(model, lead), lead, formMain.page2.effectiveStroke.effectiveStroke);
            }

            // 結果圖片
            object img;
            if (formMain.page2.recommandList.curSelectModel.model.IsContainsReducerRatioType())
                img = Properties.Resources.ResourceManager.GetObject(formMain.page2.recommandList.curSelectModel.model.Split('-')[0], CultureInfo.InvariantCulture);
            else
                img = Properties.Resources.ResourceManager.GetObject(formMain.page2.recommandList.curSelectModel.model, CultureInfo.InvariantCulture);
            formMain.picBoxResultImg.Image = img as Image;
        }

        private void InitEvents() {
            // 確認按鈕
            formMain.cmdConfirmStep5.Click += CmdConfirmStep5_Click;
            formMain.cmdResetStep5.Click += CmdResetStep5_Click;
        }        

        private void CmdConfirmStep5_Click(object sender, EventArgs e) {
            
        }

        private void CmdResetStep5_Click(object sender, EventArgs e) {
            formMain.tabMain.SelectTab("tabStart");
            //formMain.sideTable.ClearModelImg();
            //formMain.sideTable.ClearModelInfo();
            formMain.panelSideTable.Visible = false;
            //formMain.page2.recommandList.Refresh();
            //formMain.sideTable.Update(null, null);
            formMain.panelCalcAllMode.Visible = false;
        }
    }
}
