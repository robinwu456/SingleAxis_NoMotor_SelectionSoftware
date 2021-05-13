﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step1 {
        private FormMain formMain;        

        public Step1(FormMain formMain) {
            this.formMain = formMain;
            // 機構型態
            formMain.cboModelType.DataSource = Enum.GetNames(typeof(Model.ModelType));

            InitEvents();
        }

        private void InitEvents() {
            formMain.picStandardEnv.MouseDown += PicStandardEnv_MouseDown;
            formMain.picDustFree.MouseDown += PicDustFree_MouseDown;
            formMain.cboModelType.SelectedValueChanged += CboModelType_SelectedValueChanged;

            // 側邊欄更新
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optDustFreeEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optUpsideDownUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optWallHangingUse.CheckedChanged += formMain.sideTable.Update;
            formMain.optHorizontalUse.CheckedChanged += formMain.sideTable.Update;
            formMain.cboModelType.SelectedValueChanged += formMain.sideTable.Update;

            // 確認按鈕
            formMain.cmdConfirmStep1.Click += CmdConfirmStep1_Click;
        }

        private void CboModelType_SelectedValueChanged(object sender, EventArgs e) {
            formMain.optRepeatabilityScrew.Checked = !formMain.cboModelType.Text.Contains("皮帶");
            formMain.optRepeatabilityBelt.Checked = formMain.cboModelType.Text.Contains("皮帶");
        }

        private void CmdConfirmStep1_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step1) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelCalcBtns);
                formMain.step2.Load();

                // Step2 只開起一半
                formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, formMain.step2.minHeight);
            }
        }

        private void PicDustFree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optDustFreeEnv.Checked = true;
        }

        private void PicStandardEnv_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            formMain.optStandardEnv.Checked = true;
        }
    }
}
