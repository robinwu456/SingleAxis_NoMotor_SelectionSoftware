using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Binarymission.WinForms.Controls.NavigationControls;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormMain : Form {
        public SideTable sideTable;
        public Page1 page1;
        public Page2 page2;
        public Page3 page3;

        private string version;
        //private DateTime softwareDeadline = DateTime.MaxValue;              // 軟體無到期日
        private DateTime softwareDeadline = new DateTime(2021, 10, 17);     // 軟體到期日

        public FormMain() {
            InitializeComponent();

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);

            // 圖片切換
            ImgSwitch imgSwitch = new ImgSwitch(this);

            // 測邊欄
            sideTable = new SideTable(this);

            // Step
            page1 = new Page1(this);
            page2 = new Page2(this);
            page3 = new Page3(this);

            // 測試
            try {
                Test test = new Test(this);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }

            // scroll頁面觸發
            foreach (Control control in this.Controls.All()) {
                if (control is ComboBox)
                    control.MouseWheel += DisabledComboBoxMouseWheel;
                control.MouseWheel += Control_MouseWheel;
            }
        }

        private void Control_MouseWheel(object sender, MouseEventArgs e) {
            // check is hover on dgv
            if (dgvRecommandList.ClientRectangle.Contains(dgvRecommandList.PointToClient(Control.MousePosition)))
                dgvRecommandList.Focus();
            else
                explorerBar.Focus();
        }
        private void DisabledComboBoxMouseWheel(object sender, EventArgs e) {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;
        }

        private void FormMain_Load(object sender, EventArgs e) {
            // 取得當前版本
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.FileVersion;
            if (softwareDeadline == DateTime.MaxValue)
                lbTitle.Text += string.Format(" v{0}", version);
            else
                lbTitle.Text += string.Format(" v{0} ( 軟體到期日：{1} )", version, softwareDeadline.ToString("MM/dd"));

            // 到期日判斷
            VerifyDateTimeLimit();
            Opacity = 100;

            // 側邊欄隱藏
            panelSideTable.Anchor = AnchorStyles.Right;
            tabSideTableImg.Multiline = true;
            panelCalcAllMode.Anchor = AnchorStyles.Right;
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            sideTable.ResizeSideTable();
        }

        private void VerifyDateTimeLimit() {
            if (softwareDeadline == DateTime.MaxValue)
                return;

            if (DateTime.Now > softwareDeadline.AddDays(1)) {
                MessageBox.Show("已過使用期限", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (page2.inputValidate.threadShowRPMCounting != null)
                page2.inputValidate.threadShowRPMCounting.Abort();
            if (page2.inputValidate.threadCalcVmaxRange != null)
                page2.inputValidate.threadCalcVmaxRange.Abort();
            if (page2.inputValidate.threadCalcAccelSpeedRange != null)
                page2.inputValidate.threadCalcAccelSpeedRange.Abort();
        }
    }
}
