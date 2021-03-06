﻿using System;
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
        public ExplorerBar _explorerBar;
        public SideTable sideTable;
        public enum Step { Step1, Step2, Step3, Step4, Step5 }
        public Step curStep = Step.Step1;
        public Step1 step1;
        public Step2 step2;
        public Step3 step3;
        public Step4 step4;
        public Step5 step5;        

        // Step3, 4 enabled
        public bool enabledStep3 = false;
        public bool enabledStep4 = false;

        private string version;

        public FormMain() {
            InitializeComponent();

            // 標題列
            ToyoBorder toyoBorder = new ToyoBorder(this);
            // 一頁式頁籤
            _explorerBar = new ExplorerBar(this);
            // 偵測頁籤自動收起事件新增
            foreach (Control c in Controls.All()) {
                if (c is RadioButton)
                    ((RadioButton)c).CheckedChanged += _explorerBar.ValueChanged;
                else if (c is ComboBox)
                    ((ComboBox)c).SelectedValueChanged += _explorerBar.ValueChanged;
                else if (c is TextBox)
                    ((TextBox)c).TextChanged += _explorerBar.ValueChanged;
            }

            // 測邊欄
            sideTable = new SideTable(this);
            // Step
            step1 = new Step1(this);
            step2 = new Step2(this);
            step3 = new Step3(this);
            step4 = new Step4(this);
            step5 = new Step5(this);
        }        

        private void FormMain_Load(object sender, EventArgs e) {
            // 取得當前版本
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            version = fvi.FileVersion;
            lbTitle.Text += " v" + version;

            // 分頁收起
            _explorerBar.explorerBarPanel.ForEach(panel => {
                if (panel.index != 1)
                    panel.isCollapse = true;
            });
            // 測邊表格更新
            sideTable.Update(null, null);
            // 右側表格歸位
            panelSideTable.Size = new Size(224, 474);
            panelSideTable.Location = new Point(1120, 129);

            step1.Load();
        }

        private void FormMain_Resize(object sender, EventArgs e) {
            sideTable.ResizeSideTable();
        }
    }
}
