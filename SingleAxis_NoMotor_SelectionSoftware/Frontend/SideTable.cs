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
        private List<string> selectionTableItems_calcAll = new List<string>(){
            "使用環境",
            "安裝方式",
            "機構型態",
            "有效行程",
        };
        private List<string> selectionTableItems_calcSelectModel = new List<string>(){
            "使用環境",
            "安裝方式",
            "機構型態",
            "T_max係數",
            "力矩警示",
            "運行距離",
            "運行壽命",
            "有效行程",
        };

        private int tableLayoutDefaultRowHeight = 21;
        private Color tableConditionValueForeColor = Color.FromArgb(42, 88, 111);

        public SideTable(FormMain formMain) {
            this.formMain = formMain;
            UpdateItem();            

            // 移除表格，在父panel新增
            formMain.panelSideTable.Parent.Controls.Remove(formMain.panelSideTable);
            formMain.splitContainerBase.Panel2.Controls.Add(formMain.panelSideTable);            
            formMain.panelSideTable.BringToFront();
        }

        // 選項欄生成        
        public void UpdateItem() {
            // 清除欄位所有原件
            formMain.tableSelections.Controls.All().ToList().ForEach(control => formMain.tableSelections.Controls.Remove(control));
            formMain.tableSelections.RowCount = 1;

            // init label 
            formMain.tableSelections.RowStyles.Clear();
            List<string> selectionTableItems = formMain.optCalcAllModel.Checked ? selectionTableItems_calcAll : selectionTableItems_calcSelectModel;
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
                value.ForeColor = tableConditionValueForeColor;
                value.AutoSize = false;
                value.Dock = DockStyle.Fill;
                value.TextAlign = ContentAlignment.MiddleLeft;
                value.Text = "";
                formMain.tableSelections.Controls.Add(value, 1, selectionTableItems.IndexOf(item));
            });
            ResizeSideTable();
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
            int middleLocation = formMain.explorerBar.Size.Height / 2;
            formMain.panelSideTable.Location = new Point(formMain.panelSideTable.Location.X, middleLocation - formMain.panelSideTable.Size.Height / 2);
        }

        public void Update(object sender, EventArgs e) {
            UpdateTableSelections();
        }

        public void UpdateModelInfo() {
            switch (formMain.curStep) {
                case FormMain.Step.Step2:
                    formMain.lbSideTableModelInfo.Text = string.Format("{0}-L{1}", formMain.step2.recommandList.curSelectModel.model, formMain.step2.recommandList.curSelectModel.lead);
                    break;
                case FormMain.Step.Step3:
                    formMain.lbSideTableModelInfo.Text = string.Format("{0}-L{1}-{2}", formMain.step2.recommandList.curSelectModel.model, formMain.step2.recommandList.curSelectModel.lead, formMain.step2.effectiveStroke.effectiveStroke);
                    break;
            }
        }

        public void UpdateMsg(string msg, MsgStatus msgStatus) {
            formMain.lbSideTableMsg.Text = msg;
            formMain.lbSideTableMsg.ForeColor = msgStatus == MsgStatus.Normal ? Color.FromArgb(42, 88, 111) : Color.Red;
        }

        public void UpdateModelImg(string model) {
            var obj = Properties.Resources.ResourceManager.GetObject(model, CultureInfo.InvariantCulture);
            formMain.picModelImg.Image = obj as Image;
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
            formMain.sideTable.UpdateSelectedConditionValue("T_max係數", "");
            formMain.sideTable.UpdateSelectedConditionValue("力矩警示", "");
            formMain.sideTable.UpdateSelectedConditionValue("運行距離", "");
            formMain.sideTable.UpdateSelectedConditionValue("運行壽命", "");
            formMain.sideTable.UpdateSelectedConditionValue("有效行程", "");
        }

        public void UpdateTableSelections() {            
            // step1
            if (formMain.curStep >= FormMain.Step.Step1) {
                UpdateSelectedConditionValue("使用環境", formMain.panelSetupEnv.Controls.Cast<Control>().ToList()
                                                              .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                              .Text);
                UpdateSelectedConditionValue("安裝方式", formMain.panelSetupMode.Controls.Cast<Control>().ToList()
                                                               .First(control => control.GetType().Equals(typeof(RadioButton)) && ((RadioButton)control).Checked)
                                                               .Text);
                UpdateSelectedConditionValue("機構型態", formMain.cboModelType.Text);
            }
            // step2
            if (formMain.curStep < FormMain.Step.Step2)
                if (formMain.optCalcSelectedModel.Checked)
                    ClearSelectedModelInfo();
            // step5
            formMain.panelSideTableIcon.Visible = formMain.curStep == FormMain.Step.Step5;
        }
        public void UpdateSelectedConditionValue(string key, string value, bool isAlarm = false) {
            Label lbValue = formMain.panelSideTableSelections.Controls.Find("labelDataResult_" + key + "_value", true)[0] as Label;
            // 字串斷行處理
            List<string> selectionTableItems = formMain.optCalcAllModel.Checked ? selectionTableItems_calcAll : selectionTableItems_calcSelectModel;
            RowStyle rowStyle = formMain.tableSelections.RowStyles[selectionTableItems.IndexOf(key)];
            float oldRowHeight = rowStyle.Height;
            rowStyle.Height = value.Length > 7 ? tableLayoutDefaultRowHeight * 2 : tableLayoutDefaultRowHeight;
            if (value.Length > 7)
                value = value.Insert(7, "\r\n");
            lbValue.ForeColor = isAlarm ? Color.Red : tableConditionValueForeColor;
            lbValue.Text = value;
            if (oldRowHeight != rowStyle.Height)
                ResizeSideTable();
        }
    }
}
