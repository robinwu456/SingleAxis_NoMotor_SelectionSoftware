using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ExplorerBar {
        public List<ExplorerBarPanel> explorerBarPanel = new List<ExplorerBarPanel>();
        public static bool isClickTitleCollapse = false;    // 點擊標提列是否可以折疊

        private FormMain formMain;
        private Panel explorerBar;

        public ExplorerBar(FormMain formMain) {
            this.formMain = formMain;
            this.explorerBar = formMain.explorerBar;            
            // Search explorerBar panel
            SearchExplorerBarPanel();

            // 驗證Step3, 4 enabled
            if (!formMain.enabledStep3)
                explorerBarPanel.First(panel => panel.index == (int)FormMain.Step.Step3 + 1).explorerBarPanel.Visible = false;
            if (!formMain.enabledStep4)
                explorerBarPanel.First(panel => panel.index == (int)FormMain.Step.Step4 + 1).explorerBarPanel.Visible = false;
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

        // 選項值修正時，後面Step頁籤收起
        public void ValueChanged(object sender, EventArgs e) {
            // 取該Control所在的頁籤
            ExplorerBarPanel GetExplorerBarOfControl(Control _control) {
                if (_control.Parent.Name.StartsWith("explorerBarPanel"))
                    return explorerBarPanel.First(p => p.name == _control.Parent.Parent.Name);
                else
                    return GetExplorerBarOfControl(_control.Parent);
            }
            // 取該Control所在的頁籤
            Control control = sender as Control;
            if (!control.Enabled)
                return;
            ExplorerBarPanel controlsExplorerBar = GetExplorerBarOfControl(control);
            // 收起該頁籤以後的頁籤
            var collapsePanels = explorerBarPanel.Where(p => p.index > controlsExplorerBar.index).ToList();
            collapsePanels.ForEach(p => p.isCollapse = true);
            formMain.curStep = (FormMain.Step)controlsExplorerBar.index - 1;
            //Console.WriteLine("修正項: {0}, 目前Step: {1}", control.Name, formMain.curStep);

            // 側邊欄訊息清除
            if (collapsePanels.Any(panel => panel.index == 2)) {
                formMain.sideTable.ClearModelImg();
                formMain.sideTable.ClearModelInfo();
                formMain.sideTable.ClearMsg();
                formMain.step2.recommandList.Refresh();
            }
        }

        private void SearchExplorerBarPanel() {
            // 用名稱搜尋explorer bar panel
            foreach (Control control in explorerBar.Controls)
                if (control.Name.StartsWith("explorerBarPanel") && !control.Name.Contains("_"))
                    explorerBarPanel.Add(new ExplorerBarPanel(control as Panel, formMain));
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

            // 點擊標提折疊事件
            panelTitle.Click += PanelTitle_Click;
            foreach (Control control in panelTitle.Controls)
                control.Click += PanelTitle_Click;

            OnCollapseChanged += UpdateCollapse;
        }

        private void PanelTitle_Click(object sender, EventArgs e) {
            if (ExplorerBar.isClickTitleCollapse)
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
        }
    }
}
