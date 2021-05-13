using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step3 {
        private FormMain formMain;

        public Step3(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            // 確認按鈕
            formMain.cmdConfirmStep3.Click += CmdConfirmStep3_Click;
        }

        private void CmdConfirmStep3_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step3) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep4);
            }
        }
    }
}
