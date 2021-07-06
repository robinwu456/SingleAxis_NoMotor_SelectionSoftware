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

namespace SingleAxis_NoMotor_SelectionSoftware {
    public partial class FormMain : Form {
        public SideTable sideTable;
        public Page1 page1;
        public Page2 page2;
        public Page3 page3;

        private string version;

        public FormMain() {
            InitializeComponent();

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);

            // 測邊欄
            sideTable = new SideTable(this);

            // Step
            page1 = new Page1(this);
            page2 = new Page2(this);
            page3 = new Page3(this);
        }        

        private void FormMain_Load(object sender, EventArgs e) {
            // 取得當前版本
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.FileVersion;
            lbTitle.Text += " v" + version;
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            sideTable.ResizeSideTable();
        }
    }
}
