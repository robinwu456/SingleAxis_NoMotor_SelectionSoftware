using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    class CustomTabControl : TabControl {

        public bool IsBorderShow { get; set; } = true;

        private const int TCM_ADJUSTRECT = 0x1328;

        protected override void WndProc(ref Message m) {
            if (IsBorderShow) {
                // Hide the tab headers at run-time
                if (m.Msg == TCM_ADJUSTRECT && !DesignMode) {
                    m.Result = (IntPtr)1;
                    return;
                }
            } else {
                // Hide the tab headers at run-time
                if (m.Msg == TCM_ADJUSTRECT) {
                    m.Result = (IntPtr)1;
                    return;
                }
            }

            // call the base class implementation
            base.WndProc(ref m);
        }
    }
}
