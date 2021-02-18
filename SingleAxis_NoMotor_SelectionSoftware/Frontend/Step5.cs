using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step5 {
        private FormMain formMain;

        public Step5(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            // 確認按鈕
            formMain.cmdConfirmStep5.Click += CmdConfirmStep5_Click;
            formMain.cmdResetStep5.Click += CmdResetStep5_Click;
        }

        private void CmdResetStep5_Click(object sender, EventArgs e) {
            formMain.curStep = FormMain.Step.Step1;
            formMain.sideTable.Update(null, null);
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep1);
        }

        private void CmdConfirmStep5_Click(object sender, EventArgs e) {
            formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
            formMain.sideTable.Update(null, null);
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep5);
        }
    }
}
