using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step2 {
        private FormMain formMain;

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();

            formMain.tabControlAdvanceOptions.ItemSize = new Size(0, 1);
        }

        private void InitEvents() {
            // 進階選項
            formMain.toggleAdvanceOptions.CheckedChanged += ToggleAdvanceOptions_CheckedChanged;
        }

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
