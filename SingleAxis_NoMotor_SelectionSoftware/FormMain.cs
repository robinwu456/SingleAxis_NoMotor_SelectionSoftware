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
        public SideTable sideTable;

        public enum Step { Step1, Step2, Step3, Step4, Step5 }
        public Step curStep = Step.Step1;
        public Step1 step1;
        public Step2 step2;
        public Step3 step3;
        public Step4 step4;
        public Step5 step5;

        public FormMain() {
            InitializeComponent();
            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);
            // 測邊欄
            sideTable = new SideTable(this);
        }

        private void FormMain_Load(object sender, EventArgs e) {
            sideTable.Update();
            //sideTable.UpdateMsg("*您所填入的荷重已經超出範圍1~200kg內，請重新填寫。", SideTable.MsgStatus.Alarm);
        }        
    }
}
