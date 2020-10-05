using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ToyoBorder {
        private FormMain formMain;

        private string toyoWebsite = "http://www.toyorobot.com/";

        public ToyoBorder(FormMain formMain) {
            this.formMain = formMain;
            Init();
        }

        private void Init() {
            ToyoBorderControlsEvents();
        }

        private void ToyoBorderControlsEvents() {
            formMain.cmdExplorer.MouseEnter += ControlMouseEnter;
            formMain.cmdNarrow.MouseEnter += ControlMouseEnter;
            //formMain.cmdZoom.MouseEnter += ControlMouseEnter;
            formMain.cmdClose.MouseEnter += ControlMouseEnter;

            formMain.cmdExplorer.MouseLeave += ControlMouseLeave;
            formMain.cmdNarrow.MouseLeave += ControlMouseLeave;
            //formMain.cmdZoom.MouseLeave += ControlMouseLeave;
            formMain.cmdClose.MouseLeave += ControlMouseLeave;

            formMain.cmdExplorer.MouseClick += ControlMouseClick;
            formMain.cmdNarrow.MouseClick += ControlMouseClick;
            //formMain.cmdZoom.MouseClick += ControlMouseClick;
            formMain.cmdClose.MouseClick += ControlMouseClick;

            formMain.splitContainerBase.Panel1.MouseDown += ControlBar_MouseDown;
            formMain.splitContainerBase.Panel1.MouseUp += ControlBar_MouseUp;
            formMain.splitContainerBase.Panel1.MouseMove += ControlBar_MouseMove;
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Image> cmdImg = new Dictionary<PictureBox, Image>() {
                { formMain.cmdExplorer, Properties.Resources.Top_web3  },
                { formMain.cmdNarrow, Properties.Resources.Top_Narrow2 },
                //{ formMain.cmdZoom, formMain.WindowState == FormWindowState.Maximized ? Properties.Resources.Top_RE2 : Properties.Resources.Top_Zoom2 },
                { formMain.cmdClose, Properties.Resources.Top_Close2  },
            };

            cmd.Image = cmdImg[cmd];
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Image> cmdImg = new Dictionary<PictureBox, Image>() {
                { formMain.cmdExplorer, Properties.Resources.Top_web2  },
                { formMain.cmdNarrow, Properties.Resources.Top_Narrow },
                //{ formMain.cmdZoom, formMain.WindowState == FormWindowState.Maximized ? Properties.Resources.Top_RE : Properties.Resources.Top_Zoom },
                { formMain.cmdClose, Properties.Resources.Top_Close  },
            };

            cmd.Image = cmdImg[cmd];
        }

        private void ControlMouseClick(object sender, MouseEventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Action> cmdFunc = new Dictionary<PictureBox, Action>() {
                { formMain.cmdExplorer, new Action(() => { System.Diagnostics.Process.Start(toyoWebsite); }) },
                { formMain.cmdNarrow, new Action(() => { formMain.WindowState = FormWindowState.Minimized; }) },
                { formMain.cmdClose, new Action(() => { formMain.Close(); }) },
                //{ formMain.cmdZoom, new Action(() => {
                //    if (formMain.WindowState == FormWindowState.Maximized) {
                //        formMain.WindowState = FormWindowState.Normal;
                //        formMain.cmdZoom.Image = Properties.Resources.Top_Zoom;

                //        // button
                //        formMain.cmdExplorer.Location = new Point(757, 7);
                //        formMain.cmdNarrow.Location = new Point(785, 3);
                //        formMain.cmdZoom.Location = new Point(819, 3);
                //        formMain.cmdClose.Location = new Point(853, 3);
                //    } else if (formMain.WindowState == FormWindowState.Normal) {
                //        formMain.WindowState = FormWindowState.Maximized;
                //        formMain.cmdZoom.Image = Properties.Resources.Top_RE;

                //        // button
                //        formMain.cmdExplorer.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 144, 7);
                //        formMain.cmdNarrow.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 116, 3);
                //        formMain.cmdZoom.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 82, 3);
                //        formMain.cmdClose.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 48, 3);
                //    }
                //}) },
            };

            cmdFunc[cmd]();
        }

        int px, py;
        bool isDragging = false;
        private void ControlBar_MouseDown(object sender, MouseEventArgs e) {
            px = e.X; // 記住滑鼠點下時相對於元件的 (x,y) 坐標。
            py = e.Y;
            isDragging = true;
        }

        private void ControlBar_MouseUp(object sender, MouseEventArgs e) {
            isDragging = false;
        }

        private void ControlBar_MouseMove(object sender, MouseEventArgs e) {
            if (isDragging) {
                // (e.X-px, e.Y-py) 儲存了滑鼠目前相對於點下時 的移動差距 (dx, dy)
                // 範例：
                // 如果差距為 (3,2)，則我們應該將元件向右下移動 (3,2) 的距離
                // 以便讓滑鼠相對於元件的位置永遠不變。
                formMain.Left += (e.X - px);
                formMain.Top += (e.Y - py);
            }
        }
    }
}
