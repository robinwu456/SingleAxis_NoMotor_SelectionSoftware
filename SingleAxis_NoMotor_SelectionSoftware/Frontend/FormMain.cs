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
        //public ExplorerBar _explorerBar;
        public SideTable sideTable;
        public enum Step { 
            /// <summary>
            /// 條件選擇、型號選擇
            /// </summary>
            Step1,
            /// <summary>
            /// 使用環境
            /// </summary>
            Step2, 
            /// <summary>
            /// 傳動方式
            /// </summary>
            Step3, 
            /// <summary>
            /// 型號選擇
            /// </summary>
            Step4, 
            /// <summary>
            /// 安裝方式
            /// </summary>
            Step5, 
            /// <summary>
            /// 推薦規格
            /// </summary>
            Step6 
        }
        public Step curStep = Step.Step1;
        public Page1 page1;
        public Page2 page2;
        public Page3 page3;        

        //// Step3, 4 enabled
        //public bool enabledStep3 = false;
        //public bool enabledStep4 = false;

        private string version;

        public FormMain() {
            InitializeComponent();

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);
            // 一頁式頁籤
            //_explorerBar = new ExplorerBar(this);
            //// 偵測頁籤自動收起事件新增
            //foreach (Control c in Controls.All()) {
            //    if (c is RadioButton)
            //        ((RadioButton)c).CheckedChanged += _explorerBar.ValueChanged;
            //    else if (c is ComboBox)
            //        ((ComboBox)c).SelectedValueChanged += _explorerBar.ValueChanged;
            //    else if (c is TextBox)
            //        ((TextBox)c).TextChanged += _explorerBar.ValueChanged;
            //}

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

            //// 分頁收起
            //_explorerBar.explorerBarPanel.ForEach(panel => {
            //    if (panel.index != 1)
            //        panel.isCollapse = true;
            //});
            // 測邊表格更新
            sideTable.Update(null, null);
            // 右側表格歸位
            panelSideTable.Size = sideTable.tableSize;
            panelSideTable.Location = sideTable.tablePosition;
            sideTable.ResizeSideTable();

            //step1.Load();
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            sideTable.ResizeSideTable();
        }
    }
}
