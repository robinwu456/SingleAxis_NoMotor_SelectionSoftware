using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Binarymission.WinForms.Controls.NavigationControls;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormMain : Form {
        public SideTable sideTable;
        private CustomPanelBtnColorSwitch customPanelBtnColorSwitch;

        public enum Step { Step1, Step2, Step3, Step4, Step5 }
        public Step curStep = Step.Step1;
        public Step1 step1;
        public Step2 step2;
        public Step3 step3;
        public Step4 step4;
        public Step5 step5;

        public FormMain() {
            InitializeComponent();

            // 確認panel
            cmdConfirm.Click += CmdConfirm_Click;
            cmdReset.Click += CmdReset_Click;

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);
            // 測邊欄
            sideTable = new SideTable(this);
            // 按鈕hover變色
            customPanelBtnColorSwitch = new CustomPanelBtnColorSwitch(this);
            // Step
            step1 = new Step1(this);
        }        

        private void FormMain_Load(object sender, EventArgs e) {
            sideTable.Update(null, null);
            //sideTable.UpdateMsg("*您所填入的荷重已經超出範圍1~200kg內，請重新填寫。", SideTable.MsgStatus.Alarm);
        }

        private void CmdConfirm_Click(object sender, EventArgs e) {
            curStep = (Step)((int)curStep + 1);
            sideTable.Update(null, null);
            binaryExplorerBar.Panels[(int)curStep].CurrentExplorerBarPanelState = BinaryExplorerBarPanelState.Expanded;
            MoveConfirmPanelToStep(curStep);
        }

        private void CmdReset_Click(object sender, EventArgs e) {
            curStep = Step.Step1;
            sideTable.Update(null, null);
            binaryExplorerBar.Panels.Cast<BinaryExplorerBarPanel>().ToList().ForEach(panel => panel.CurrentExplorerBarPanelState = BinaryExplorerBarPanelState.Collapsed);
            binaryExplorerBar.Panels[(int)curStep].CurrentExplorerBarPanelState = BinaryExplorerBarPanelState.Expanded;            
            MoveConfirmPanelToStep(curStep);
        }

        private void MoveConfirmPanelToStep(Step step) {
            BinaryExplorerBarPanel targetPanel = Controls.Find("panelStep" + ((int)step + 1), true)[0] as BinaryExplorerBarPanel;
            panelConfirmBtns.Parent.Controls.Remove(panelConfirmBtns);
            targetPanel.Controls.Add(panelConfirmBtns);
            binaryExplorerBar.ScrollControlIntoView(panelConfirmBtns);
        }

        private bool binaryExplorerBar_BinaryExplorerBarPanelTitleClicked(object sender, BinaryExplorerBarPanel thePanelObject) {
            int thePanelObjectStepIndex = Convert.ToInt32(thePanelObject.Name.Replace("panelStep", "")) - 1;
            // 目前Step以前才可以開關panel
            return thePanelObjectStepIndex <= (int)curStep;
        }
    }
}
