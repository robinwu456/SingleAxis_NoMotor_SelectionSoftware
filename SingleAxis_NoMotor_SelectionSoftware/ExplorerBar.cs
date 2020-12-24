using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ExplorerBar {
        private FormMain formMain;

        private Panel explorerBar;
        private List<ExplorerBarPanel> explorerBarPanel = new List<ExplorerBarPanel>();

        public ExplorerBar(FormMain formMain) {
            this.formMain = formMain;
            this.explorerBar = formMain.explorerBar;
            // Search explorerBar panel
            SearchExplorerBarPanel();
        }

        private void SearchExplorerBarPanel() {
            foreach (Control control in explorerBar.Controls)
                if (control.Name.StartsWith("explorerBarPanel") && !control.Name.Contains("_"))
                    explorerBarPanel.Add(new ExplorerBarPanel(control as Panel));
        }
    }

    public class ExplorerBarPanel {
        public string name;
        public int index;
        public Panel explorerBarPanel;
        public Panel panelTitle;
        public Panel panelConent;
        public Size unCollapseSize;
        public bool isCollapse = false;

        public ExplorerBarPanel(Panel explorerBarPanel) {
            // panel init
            this.explorerBarPanel = explorerBarPanel;
            this.name = explorerBarPanel.Name;
            this.index = Convert.ToInt32(explorerBarPanel.Name.Replace("explorerBarPanel", ""));
            panelTitle = explorerBarPanel.Controls.Find(this.name + "_title", true)[0] as Panel;
            panelConent = explorerBarPanel.Controls.Find(this.name + "_content", true)[0] as Panel;
            unCollapseSize = explorerBarPanel.Size;
            // event init
            panelTitle.Click += PanelTitle_Click;
        }

        private void PanelTitle_Click(object sender, EventArgs e) {
            isCollapse = !isCollapse;
            UpdateCollapse();
        }

        private void UpdateCollapse() {
            explorerBarPanel.Size = isCollapse ?
                (index == 1 ? 
                    new Size(explorerBarPanel.Size.Width, panelTitle.Size.Height) : 
                    new Size(explorerBarPanel.Size.Width, panelTitle.Size.Height + 10)) :
                new Size(explorerBarPanel.Size.Width, unCollapseSize.Height);
        }
    }
}
