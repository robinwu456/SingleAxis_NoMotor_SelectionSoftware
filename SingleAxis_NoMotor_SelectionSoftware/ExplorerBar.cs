﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ExplorerBar {
        public List<ExplorerBarPanel> explorerBarPanel = new List<ExplorerBarPanel>();

        private FormMain formMain;
        private Panel explorerBar;        

        public ExplorerBar(FormMain formMain) {
            this.formMain = formMain;
            this.explorerBar = formMain.explorerBar;            
            // Search explorerBar panel
            SearchExplorerBarPanel();

            //explorerBar.Scroll += ExplorerBar_Scroll;
            //explorerBar.MouseWheel += ExplorerBar_MouseWheel;

            //explorerBar.VerticalScroll.Visible = false;
            formMain.vScrollBarExplorerBar.Scroll += VScrollBarExplorerBar_Scroll;
            formMain.vScrollBarExplorerBar.MouseWheel += VScrollBarExplorerBar_MouseWheel;

            //explorerBar.AutoScroll = false;
            //explorerBar.VerticalScroll.Enabled = false;
            //explorerBar.VerticalScroll.Visible = false;
            explorerBar.VerticalScroll.Maximum = 0;
            //explorerBar.AutoScroll = true;

            formMain.vScrollBarExplorerBar.Scroll += (sender, e) => { explorerBar.VerticalScroll.Value = formMain.vScrollBarExplorerBar.Value; };            
        }

        private void VScrollBarExplorerBar_MouseWheel(object sender, MouseEventArgs e) {
            Console.WriteLine(explorerBar.VerticalScroll.Value);
            //UpdateScrollValue();
            explorerBar.VerticalScroll.Value = 800;
        }

        private void VScrollBarExplorerBar_Scroll(object sender, ScrollEventArgs e) {
            Console.WriteLine(e.NewValue);
            UpdateScrollValue();
        }

        private void ExplorerBar_MouseWheel(object sender, MouseEventArgs e) {
            //Console.WriteLine(explorerBar.VerticalScroll.Value);
            //UpdateScrollValue();
        }

        private void ExplorerBar_Scroll(object sender, ScrollEventArgs e) {
            //Console.WriteLine(e.NewValue);
            //UpdateScrollValue();
        }

        public void UpdateCurStep(FormMain.Step curStep) {
            // 目前Step以前的都打開(包刮目前)，其他的關閉
            for (int i = 0; i < explorerBarPanel.Count; i++) {
                if (i > (int)curStep)
                    explorerBarPanel.First(panel => panel.index == i + 1).isCollapse = true;
                else
                    explorerBarPanel.First(panel => panel.index == i + 1).isCollapse = false;
            }
        }

        private void SearchExplorerBarPanel() {
            // 用名稱搜尋explorer bar panel
            foreach (Control control in explorerBar.Controls)
                if (control.Name.StartsWith("explorerBarPanel") && !control.Name.Contains("_"))
                    explorerBarPanel.Add(new ExplorerBarPanel(control as Panel, formMain));
        }

        private void UpdateScrollValue() {
            formMain.explorerBar.VerticalScroll.Value = formMain.vScrollBarExplorerBar.Value;
        }
    }

    public class ExplorerBarPanel {
        public string name;
        public int index;
        public Panel explorerBarPanel;
        public Panel panelTitle;
        public Panel panelConent;
        public Size unCollapseSize;

        private bool _isCollapse = false;
        public bool isCollapse {
            get {
                return _isCollapse;
            }
            set {
                if (value != _isCollapse) {
                    _isCollapse = value;
                    OnCollapseChanged();
                }
            }
        }

        public delegate void CollapseChanged();
        public CollapseChanged OnCollapseChanged;

        private FormMain formMain;

        public ExplorerBarPanel(Panel explorerBarPanel, FormMain formMain) {
            // panel init
            this.explorerBarPanel = explorerBarPanel;
            this.formMain = formMain;
            this.name = explorerBarPanel.Name;
            this.index = Convert.ToInt32(explorerBarPanel.Name.Replace("explorerBarPanel", ""));
            panelTitle = explorerBarPanel.Controls.Find(this.name + "_title", true)[0] as Panel;
            panelConent = explorerBarPanel.Controls.Find(this.name + "_content", true)[0] as Panel;
            unCollapseSize = explorerBarPanel.Size;
            // 標提折疊事件
            panelTitle.Click += PanelTitle_Click;
            foreach (Control control in panelTitle.Controls)
                control.Click += PanelTitle_Click;
            OnCollapseChanged += UpdateCollapse;
        }

        private void PanelTitle_Click(object sender, EventArgs e) {
            if ((int)formMain.curStep >= index - 1)
                isCollapse = !isCollapse;
        }

        private void UpdateCollapse() {
            if (isCollapse) {
                if (index == 1)
                    explorerBarPanel.Size = new Size(explorerBarPanel.Size.Width, panelTitle.Size.Height);
                else
                    explorerBarPanel.Size = new Size(explorerBarPanel.Size.Width, panelTitle.Size.Height + 10);
            } else
                explorerBarPanel.Size = new Size(explorerBarPanel.Size.Width, unCollapseSize.Height);

            // scrollBar 更動
            formMain.vScrollBarExplorerBar.SmallChange = formMain.explorerBar.VerticalScroll.SmallChange;
            formMain.vScrollBarExplorerBar.LargeChange = formMain.explorerBar.VerticalScroll.LargeChange;
            formMain.vScrollBarExplorerBar.Minimum = formMain.explorerBar.VerticalScroll.Minimum;
            formMain.vScrollBarExplorerBar.Minimum = formMain.explorerBar.VerticalScroll.Minimum;
            formMain.vScrollBarExplorerBar.Maximum = formMain.explorerBar.VerticalScroll.Maximum;

            Console.WriteLine(formMain.explorerBar.VerticalScroll.Maximum);
        }
    }
}
