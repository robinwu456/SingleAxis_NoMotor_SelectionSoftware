using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class CustomPanelBtnColorSwitch {
        public FormMain formMain;

        private Dictionary<Label, CustomPanel> mapBtns = new Dictionary<Label, CustomPanel>();

        public CustomPanelBtnColorSwitch(FormMain formMain) {
            this.formMain = formMain;
            InitColorSwitchEvent();
        }

        private void InitColorSwitchEvent() {
            mapBtns = new Dictionary<Label, CustomPanel>() {
                { formMain.cmdConfirm, formMain.panelCmdConfirm },
                { formMain.cmdReset, formMain.panelCmdReset }
            };

            mapBtns.Keys.ToList().ForEach(cmd => {
                cmd.MouseEnter += Cmd_MouseEnter;
                cmd.MouseLeave += Cmd_MouseLeave;
                cmd.MouseDown += Cmd_MouseDown;
                cmd.MouseUp += Cmd_MouseUp; ;
            });
        }

        private void Cmd_MouseUp(object sender, MouseEventArgs e) {
            Label cmd = sender as Label;
            // 確認條件
            if (cmd == formMain.cmdConfirm)
                mapBtns[cmd].BackColor = Color.DarkRed;
            // 重新檢索
            if (cmd == formMain.cmdReset)
                mapBtns[cmd].BackColor = Color.DarkGray;

            mapBtns[cmd].Invalidate();
        }

        private void Cmd_MouseDown(object sender, MouseEventArgs e) {
            Label cmd = sender as Label;
            // 確認條件
            if (cmd == formMain.cmdConfirm)
                mapBtns[cmd].BackColor = Color.FromArgb(64, 0, 0);
            // 重新檢索
            if (cmd == formMain.cmdReset)
                mapBtns[cmd].BackColor = Color.Black;

            mapBtns[cmd].Invalidate();
        }

        private void Cmd_MouseLeave(object sender, EventArgs e) {
            Label cmd = sender as Label;
            // 確認條件
            if (cmd == formMain.cmdConfirm)
                mapBtns[cmd].BackColor = Color.Red;
            // 重新檢索
            if (cmd == formMain.cmdReset)
                mapBtns[cmd].BackColor = Color.Gray;

            mapBtns[cmd].Invalidate();
        }

        private void Cmd_MouseEnter(object sender, EventArgs e) {
            Label cmd = sender as Label;
            // 確認條件
            if (cmd == formMain.cmdConfirm)
                mapBtns[cmd].BackColor = Color.DarkRed;
            // 重新檢索
            if (cmd == formMain.cmdReset)
                mapBtns[cmd].BackColor = Color.DimGray;

            mapBtns[cmd].Invalidate();
        }
    }
}
