using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class GlassyPanel : Panel {
        private const int WS_EX_TRANSPARENT = 0x20;
        public GlassyPanel() {
            SetStyle(ControlStyles.Opaque, true);
        }

        private int opacity = 50;
        [System.ComponentModel.DefaultValue(50)]
        public int Opacity {
            get {
                return this.opacity;
            }
            set {
                if (value < 0 || value > 100)
                    throw new ArgumentException("value must be between 0 and 100");
                this.opacity = value;
            }
        }
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }
        protected override void OnPaint(PaintEventArgs e) {
            using (var brush = new SolidBrush(Color.FromArgb(this.opacity * 255 / 100, this.BackColor))) {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}
