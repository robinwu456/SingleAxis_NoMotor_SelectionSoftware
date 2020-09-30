using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormMain : Form {

        private string toyoWebsite = "http://www.toyorobot.com/";

        public FormMain() {
            InitializeComponent();
            InitControlsEvents();
        }

        private void FormMain_Load(object sender, EventArgs e) {
            
        }

        private void InitControlsEvents() {
            cmdExplorer.MouseEnter += ControlMouseEnter;
            cmdNarrow.MouseEnter += ControlMouseEnter;
            cmdZoom.MouseEnter += ControlMouseEnter;
            cmdClose.MouseEnter += ControlMouseEnter;

            cmdExplorer.MouseLeave += ControlMouseLeave;
            cmdNarrow.MouseLeave += ControlMouseLeave;
            cmdZoom.MouseLeave += ControlMouseLeave;
            cmdClose.MouseLeave += ControlMouseLeave;

            cmdExplorer.MouseClick += ControlMouseClick;
            cmdNarrow.MouseClick += ControlMouseClick;
            cmdZoom.MouseClick += ControlMouseClick;
            cmdClose.MouseClick += ControlMouseClick;

            splitContainerBase.Panel1.MouseDown += ControlBar_MouseDown;
            splitContainerBase.Panel1.MouseUp += ControlBar_MouseUp;
            splitContainerBase.Panel1.MouseMove += ControlBar_MouseMove;
        }

        private void ControlMouseEnter(object sender, EventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Image> cmdImg = new Dictionary<PictureBox, Image>() {
                { cmdExplorer, Properties.Resources.Top_web3  },
                { cmdNarrow, Properties.Resources.Top_Narrow2 },
                { cmdZoom, WindowState == FormWindowState.Maximized ? Properties.Resources.Top_RE2 : Properties.Resources.Top_Zoom2 },
                { cmdClose, Properties.Resources.Top_Close2  },
            };

            cmd.Image = cmdImg[cmd];
        }

        private void ControlMouseLeave(object sender, EventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Image> cmdImg = new Dictionary<PictureBox, Image>() {
                { cmdExplorer, Properties.Resources.Top_web2  },
                { cmdNarrow, Properties.Resources.Top_Narrow },
                { cmdZoom, WindowState == FormWindowState.Maximized ? Properties.Resources.Top_RE : Properties.Resources.Top_Zoom },
                { cmdClose, Properties.Resources.Top_Close  },
            };

            cmd.Image = cmdImg[cmd];
        }

        private void ControlMouseClick(object sender, MouseEventArgs e) {
            PictureBox cmd = sender as PictureBox;

            Dictionary<PictureBox, Action> cmdFunc = new Dictionary<PictureBox, Action>() {
                { cmdExplorer, new Action(() => { System.Diagnostics.Process.Start(toyoWebsite); }) },
                { cmdNarrow, new Action(() => { this.WindowState = FormWindowState.Minimized; }) },
                { cmdClose, new Action(() => { this.Close(); }) },
                { cmdZoom, new Action(() => {
                    if (WindowState == FormWindowState.Maximized) {
                        WindowState = FormWindowState.Normal;
                        cmdZoom.Image = Properties.Resources.Top_Zoom;

                        // button
                        cmdExplorer.Location = new Point(757, 7);
                        cmdNarrow.Location = new Point(785, 3);
                        cmdZoom.Location = new Point(819, 3);
                        cmdClose.Location = new Point(853, 3);
                    } else if (WindowState == FormWindowState.Normal) {
                        WindowState = FormWindowState.Maximized;
                        cmdZoom.Image = Properties.Resources.Top_RE;

                        // button
                        cmdExplorer.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 144, 7);
                        cmdNarrow.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 116, 3);
                        cmdZoom.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 82, 3);
                        cmdClose.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 48, 3);
                    }
                }) },
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
                this.Left += (e.X - px);
                this.Top += (e.Y - py);
            }
        }
    }
}
