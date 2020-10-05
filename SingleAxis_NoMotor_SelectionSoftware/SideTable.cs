using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class SideTable {
        public enum MsgStatus { Normal, Alarm }

        private FormMain formMain;
        private List<string> selectionTableItems = new List<string>(){
            "使用環境",
            "安裝方式",
            "線槽",
            "動子",
            "編碼器",
            "有效行程",
            "編碼器廠牌",
            "線纜線長",
            //"線纜線長1",
            //"線纜線長2",
            //"線纜線長3",
        };

        public SideTable(FormMain formMain) {
            this.formMain = formMain;
            Init();
        }

        // 選項欄生成
        private void Init() {
            // init label
            int rowHeight = 21;
            formMain.tableSelections.RowCount = selectionTableItems.Count;
            selectionTableItems.ForEach(item => {
                RowStyle row = new RowStyle(SizeType.Absolute, rowHeight);
                formMain.tableSelections.RowStyles.Add(row);

                Label title = new Label();
                title.Name = "labelDataResult_" + item;
                title.Font = new Font("微軟正黑體", 9);
                title.ForeColor = Color.FromArgb(0, 0, 0);
                title.AutoSize = false;
                title.Dock = DockStyle.Fill;
                title.TextAlign = ContentAlignment.MiddleLeft;
                title.Text = item;
                formMain.tableSelections.Controls.Add(title, 0, selectionTableItems.IndexOf(item));

                Label value = new Label();
                value.Name = "labelDataResult_" + item + "_value";
                value.Font = new Font("微軟正黑體", 9);
                value.ForeColor = Color.FromArgb(42, 88, 111);
                value.AutoSize = false;
                value.Dock = DockStyle.Fill;
                value.TextAlign = ContentAlignment.MiddleLeft;
                value.Text = "";
                formMain.tableSelections.Controls.Add(value, 1, selectionTableItems.IndexOf(item));
            });
            // set selection table size
            int hiddenBorderDistance = 1;
            formMain.tableSelections.Size = new Size(
                formMain.panelSideTableSelections.Size.Width + hiddenBorderDistance * 2, // +左右邊框影藏
                rowHeight * selectionTableItems.Count + hiddenBorderDistance * 2 + 1   // +上(+1)下(+2)邊框隱藏
            );
            formMain.tableSelections.Location = new Point(0 - hiddenBorderDistance, 0 - hiddenBorderDistance);
            // set table size
            int minHeight = 298;
            formMain.panelSideTable.Size = new Size(formMain.panelSideTable.Size.Width, minHeight + formMain.tableSelections.RowCount * rowHeight);
        }

        public void Update() {
            UpdateTableSelections();
        }

        public void UpdateMsg(string msg, MsgStatus msgStatus) {

        }

        private void UpdateTableSelections() {

        }
    }
}
