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
            "機構型態",
            "線槽",
            "動子",
            "編碼器",
            "有效行程",
            "編碼器廠牌",
            "線纜線長",
        };

        private int tableLayoutDefaultRowHeight = 21;

        public SideTable(FormMain formMain) {
            this.formMain = formMain;
            Init();
        }

        // 選項欄生成
        private void Init() {
            // init label            
            formMain.tableSelections.RowStyles.Clear();
            formMain.tableSelections.RowCount = selectionTableItems.Count;
            selectionTableItems.ForEach(item => {
                RowStyle row = new RowStyle(SizeType.Absolute, tableLayoutDefaultRowHeight);
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
            ResizeSideTable();
        }

        private void ResizeSideTable() {
            // set selection table size
            int hiddenBorderDistance = 1;
            int allLabelHeight = (int)formMain.tableSelections.RowStyles.Cast<RowStyle>().Sum(row => row.Height);
            formMain.tableSelections.Size = new Size(
                formMain.panelSideTableSelections.Size.Width + hiddenBorderDistance * 2, // +左右邊框影藏
                allLabelHeight + hiddenBorderDistance * 2 + 1 + 8   // +上(+1)下(+2)邊框隱藏 +8(高度補償)
            );
            formMain.tableSelections.Location = new Point(0 - hiddenBorderDistance, 0 - hiddenBorderDistance);
            // set table size
            int minHeight = 298;
            formMain.panelSideTable.Size = new Size(formMain.panelSideTable.Size.Width, minHeight + allLabelHeight + 8);    // +8(高度補償)
            // 高度置中
            int middleLocation = formMain.panel1.Size.Height / 2;
            formMain.panelSideTable.Location = new Point(formMain.panelSideTable.Location.X, middleLocation - formMain.panelSideTable.Size.Height / 2);
        }

        public void Update(object sender, EventArgs e) {
            UpdateTableSelections();
        }

        public void UpdateMsg(string msg, MsgStatus msgStatus) {

        }

        private void UpdateTableSelections() {
            void UpdateValue(string key, string value) {
                Label lbValue = formMain.panelSideTableSelections.Controls.Find("labelDataResult_" + key + "_value", true)[0] as Label;
                // 字串斷行處理
                RowStyle rowStyle = formMain.tableSelections.RowStyles[selectionTableItems.IndexOf(key)];
                rowStyle.Height = value.Length > 7 ? tableLayoutDefaultRowHeight * 2 : tableLayoutDefaultRowHeight;
                if (value.Length > 7)
                    value = value.Insert(7, "\r\n");
                lbValue.Text = value;
                ResizeSideTable();
            }
            // step1
            if (formMain.curStep >= FormMain.Step.Step1) {
                UpdateValue("使用環境", formMain.panelSetupEnv.Controls.Cast<Control>().ToList()
                                                              .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                              .Text);
                UpdateValue("安裝方式", formMain.panelSetupMode.Controls.Cast<Control>().ToList()
                                                               .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                               .Text);
                UpdateValue("機構型態", formMain.cboModelType.Text);
            }
        }
    }
}
