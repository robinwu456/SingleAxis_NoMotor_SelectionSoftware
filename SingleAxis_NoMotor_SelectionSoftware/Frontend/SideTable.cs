﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class SideTable {
        public enum MsgStatus { Normal, Alarm }
     
        private FormMain formMain;
        // 側邊欄訊息 - 形狀選型
        private List<string> selectionTableItems_calcAll = new List<string>(){
            "使用環境",
            "安裝方式",
            "機構型態",
            "運行距離",
            "運行壽命",
        };
        // 側邊欄訊息 - 型號選型
        private List<string> selectionTableItems_calcSelectModel = new List<string>(){
            "使用環境",
            "安裝方式",
            "機構型態",
            "運行距離",
            "運行壽命",
        };

        private int tableLayoutDefaultRowHeight = 21;
        private Color tableConditionValueForeColor = Color.FromArgb(42, 88, 111);

        public SideTable(FormMain formMain) {
            this.formMain = formMain;
            UpdateItem();            
        }

        public void RePosition() {
            // 移除表格，在父panel新增
            formMain.panelSideTable.Parent.Controls.Remove(formMain.panelSideTable);
            formMain.splitContainerBase.Panel2.Controls.Add(formMain.panelSideTable);
            formMain.panelSideTable.BringToFront();
            formMain.panelSideTable.Visible = true;

            // 側邊欄規位
            //formMain.panelSideTable.Size = sizeSideTable;
            //formMain.panelSideTable.Location = positionSideTable;
            //formMain.panelSideTableSelections.Size = sizeTableSelection;

            // 全選項目
            formMain.panelCalcAllMode.Parent.Controls.Remove(formMain.panelCalcAllMode);
            formMain.splitContainerBase.Panel2.Controls.Add(formMain.panelCalcAllMode);
            formMain.panelCalcAllMode.BringToFront();
        }

        // 選項欄生成
        public void UpdateItem() {
            if (formMain.page1 == null)
                return;

            // Get items
            List<string> selectionTableItems = new List<string>();
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                selectionTableItems = selectionTableItems_calcAll;
            else
                selectionTableItems = selectionTableItems_calcSelectModel;

            // resize table
            formMain.panelSideTable.Visible = false;
            formMain.tableSelections.Controls.All().ToList().ForEach(control => formMain.tableSelections.Controls.Remove(control));
            formMain.tableSelections.RowCount = selectionTableItems.Count;
            formMain.tableSelections.RowStyles.Clear();
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
                value.ForeColor = tableConditionValueForeColor;
                value.AutoSize = false;
                value.Dock = DockStyle.Fill;
                value.TextAlign = ContentAlignment.MiddleLeft;
                value.Text = "";
                formMain.tableSelections.Controls.Add(value, 1, selectionTableItems.IndexOf(item));
            });
            ResizeSideTable();
            formMain.panelSideTable.Visible = true;
        }

        public void ResizeSideTable() {
            // set selection table size
            int hiddenBorderDistance = 1;
            int allLabelHeight = (int)formMain.tableSelections.RowStyles.Cast<RowStyle>().Sum(row => row.Height);
            formMain.tableSelections.Size = new Size(
                formMain.panelSideTableSelections.Size.Width + hiddenBorderDistance * 2, // +左右邊框隱藏
                allLabelHeight + hiddenBorderDistance * 2 + 1 + 8   // +上(+1)下(+2)邊框隱藏 +8(高度補償)
            );
            formMain.tableSelections.Location = new Point(0 - hiddenBorderDistance, 0 - hiddenBorderDistance);
            // set table size
            int minHeight = 309;
            formMain.panelSideTable.Size = new Size(formMain.panelSideTable.Size.Width, minHeight + allLabelHeight + 8 + 21);    // +8(高度補償)
            // 高度置中
            int middleLocation;
            if (formMain.tabMain.SelectedTab.Name == "tabResult")
                middleLocation = formMain.explorerBar_step5.Size.Height / 2;
            else
                middleLocation = formMain.explorerBar.Size.Height / 2;
            formMain.panelSideTable.Location = new Point(formMain.panelSideTable.Location.X, middleLocation - formMain.panelSideTable.Size.Height / 2);

            // 全選模式
            formMain.panelCalcAllMode.Location = new Point(formMain.panelSideTable.Location.X, formMain.panelSideTable.Location.Y - 77);
        }

        public void Update(object sender, EventArgs e) {
            //if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection)
                UpdateSelectedConditionValue("使用環境", formMain.panelUseEnv.Controls.Cast<Control>().ToList()
                                                                  .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                                  .Text);
            UpdateSelectedConditionValue("安裝方式", formMain.panelSetup.Controls.Cast<Control>().ToList()
                                                           .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                           .Text);
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection ||
                (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection && formMain.cboModel.Text != ""))
                UpdateSelectedConditionValue("機構型態", formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.ToString());

            formMain.panelSideTableIcon.Visible = formMain.tabMain.SelectedIndex == formMain.tabMain.TabPages.Count - 1;
        }

        public void UpdateMsg(string msg, MsgStatus msgStatus) {
            formMain.lbSideTableMsg.Text = msg;
            formMain.lbSideTableMsg.ForeColor = msgStatus == MsgStatus.Normal ? Color.FromArgb(42, 88, 111) : Color.Red;
        }

        public void UpdateModelImg(string model) {
            formMain.tabSideTableImg.SelectTab("型號");
            if (model.IsContainsReducerRatioType()) {
                var obj = Properties.Resources.ResourceManager.GetObject(model.Split('-')[0], CultureInfo.InvariantCulture);
                formMain.picModelImg.Image = obj as Image;
            } else {
                var obj = Properties.Resources.ResourceManager.GetObject(model, CultureInfo.InvariantCulture);
                formMain.picModelImg.Image = obj as Image;
            }
        }

        public void UpdateModelImg(Model.ModelType modelType) {
            formMain.tabSideTableImg.SelectTab(modelType.ToString());
        }

        public void UpdateModeInfo(string model, double lead) {
            try {
                if (model.IsContainsReducerRatioType()) {
                    int reducer = (int)lead;
                    formMain.lbSideTableModelInfo.Text = string.Format("{0}-{1}", model.Split('-')[0], reducer.ToString("#00"));
                } else {
                    if (model.Contains("-"))
                        formMain.lbSideTableModelInfo.Text = string.Format("{0}-{1}{2}", model.Split('-')[0], formMain.page2.calc.GetLeadText(model, lead), lead);
                    else
                        formMain.lbSideTableModelInfo.Text = string.Format("{0}-{1}{2}", model, formMain.page2.calc.GetLeadText(model, lead), lead);
                }
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        public void ClearMsg() {
            formMain.lbSideTableMsg.Text = "";
        }

        public void ClearModelInfo() {
            formMain.lbSideTableModelInfo.Text = "";
        }

        public void ClearModelImg() {
            formMain.picModelImg.Image = null;
        }

        public void ClearSelectedModelInfo() {
            formMain.sideTable.UpdateSelectedConditionValue("運行距離", "");
            formMain.sideTable.UpdateSelectedConditionValue("運行壽命", "");
        }

        public void UpdateSelectedConditionValue(string key, string value, bool isAlarm = false) {
            Label lbKey = formMain.panelSideTableSelections.Controls.Find("labelDataResult_" + key, true)[0] as Label;
            Label lbValue = formMain.panelSideTableSelections.Controls.Find("labelDataResult_" + key + "_value", true)[0] as Label;
            // 字串斷行處理
            List<string> selectionTableItems = new List<string>();
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                selectionTableItems = selectionTableItems_calcAll;
            else
                selectionTableItems = selectionTableItems_calcSelectModel;
            RowStyle rowStyle = formMain.tableSelections.RowStyles[selectionTableItems.IndexOf(key)];
            float oldRowHeight = rowStyle.Height;
            rowStyle.Height = key.Length > 7 || value.Length > 7 ? tableLayoutDefaultRowHeight * 2 : tableLayoutDefaultRowHeight;
            // 數值驗證
            lbValue.ForeColor = isAlarm ? Color.Red : tableConditionValueForeColor;
            lbValue.Text = value;

            if (oldRowHeight != rowStyle.Height)
                ResizeSideTable();
        }
    }
}
