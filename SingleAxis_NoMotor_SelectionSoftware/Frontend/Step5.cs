using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step5 {
        private FormMain formMain;

        public Step5(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        public void Load() {
            // 結果型號
            formMain.lbResult.Text = string.Format("{0} - L{1} - {2}", 
                formMain.step2.recommandList.curSelectModel.model, 
                formMain.step2.recommandList.curSelectModel.lead, 
                formMain.step2.effectiveStroke.effectiveStroke);

            // 結果圖片
            var img = Properties.Resources.ResourceManager.GetObject(formMain.step2.recommandList.curSelectModel.model, CultureInfo.InvariantCulture);
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
            formMain.tabMain.SelectTab(0);
            formMain.curStep = FormMain.Step.Step1;
            formMain.sideTable.Update(null, null);
            formMain.sideTable.ClearModelImg();
            formMain.sideTable.ClearModelInfo();
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep1);
        }
    }
}
