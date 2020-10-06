using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step1 {
        private FormMain formMain;        

        public Step1(FormMain formMain) {
            this.formMain = formMain;
            formMain.cboModelType.DataSource = Config.MODEL_TYPE_TXT;

            InitEvents();
        }

        private void InitEvents() {
            formMain.picStandardEnv.MouseDown += PicStandardEnv_MouseDown;
            formMain.picDustFree.MouseDown += PicDustFree_MouseDown;

            // 側邊欄更新
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optDustFreeEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optUpsideDownUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optWallHangingUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optHorizontalUse.CheckedChanged += formMain.sideTable.Update;
            formMain.cboModelType.SelectedValueChanged += formMain.sideTable.Update;
        }

        private void PicDustFree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optDustFreeEnv.Checked = true;
        }

        private void PicStandardEnv_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optStandardEnv.Checked = true;
        }
    }
}
