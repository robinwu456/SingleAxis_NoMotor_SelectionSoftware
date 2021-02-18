using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step4 {
        private FormMain formMain;

        public Step4(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            // 確認按鈕
            formMain.cmdConfirmStep4.Click += CmdConfirmStep4_Click;
        }

        private void CmdConfirmStep4_Click(object sender, EventArgs e) {
            formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
            formMain.sideTable.Update(null, null);
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep5);
        }
    }
}
