using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ImgSwitch : ImgInit {
        private FormMain formMain;

        public ImgSwitch(FormMain formMain) : base(formMain) {
            this.formMain = formMain;

            InitEvent();
        }

        private void InitEvent() {
            // Switch按鈕換圖事件委派
            Action<PictureBox> CmdSwitchEventsDelegate = new Action<PictureBox>((cmd) => {
                // 圖片切換(hover)
                cmd.MouseEnter += (sender, e) => SetSwitchBtnImg(sender, ButtonStatus.Hover);
                cmd.MouseDown += (sender, e) => SetSwitchBtnImg(sender, ButtonStatus.Press);
                cmd.MouseUp += (sender, e) => SetSwitchBtnImg(sender, ButtonStatus.Hover);
                cmd.MouseLeave += (sender, e) => SetSwitchBtnImg(sender, ButtonStatus.Normal);
            });

            foreach (var pic in img.Keys) {
                // 停懸事件
                CmdSwitchEventsDelegate(pic);
                if (cmdOptMap.Keys.Contains(pic)) {
                    // 圖片Click事件
                    pic.Click += (sender, e) => cmdOptMap[sender as PictureBox].Checked = true;
                    cmdOptMap[pic].CheckedChanged += UpdateImg_OptTabMain;
                }
            }
        }

        private void SetSwitchBtnImg(object sender, ButtonStatus buttonStatus) {
            PictureBox cmd = sender as PictureBox;
            if (cmdOptMap.Keys.Contains(cmd))
                cmd.Image = cmdOptMap[cmd].Checked ? img[cmd][ButtonStatus.Enable] : img[cmd][buttonStatus];
            else
                cmd.Image = img[cmd][buttonStatus];
        }

        public void UpdateImg_OptTabMain(object sender, EventArgs e) {
            // Enabled圖片切換
            PictureBox pic = cmdOptMap.First(pair => pair.Value == sender as RadioButton).Key;
            pic.Image = cmdOptMap[pic].Checked ? img[pic][ButtonStatus.Enable] : img[pic][ButtonStatus.Normal];
        }
    }
}
