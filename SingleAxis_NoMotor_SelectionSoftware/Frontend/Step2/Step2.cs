﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step2 {
        private FormMain formMain;
        public Calculation calc = new Calculation();

        public int minHeight = 787;
        private int maxHeight = 1300;        
        
        private Thread threadCalc;                

        // Step2各項目
        public MotorPower motorPower;
        public ChartInfo chartInfo;
        public RunCondition runCondition;
        public InputValidate inputValidate;
        public RecommandList recommandList;

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();

            // 輸入驗證
            inputValidate = new InputValidate(formMain);
            // 馬達條件
            motorPower = new MotorPower(formMain);
            // 圖表
            chartInfo = new ChartInfo(formMain);
            // 運轉條件
            runCondition = new RunCondition(formMain);
            // 推薦規格列表
            recommandList = new RecommandList(formMain);

            // 減速比
            calc.reducerInfo.Rows.Cast<DataRow>().ToList().ForEach(row => {
                DataGridViewRow newRow = new DataGridViewRow();
                DataGridViewTextBoxCell txtCell = new DataGridViewTextBoxCell();
                DataGridViewComboBoxCell cboCell = new DataGridViewComboBoxCell();
                txtCell.Value = row["Model"].ToString();
                cboCell.DataSource = row["ReducerRatio"].ToString().Split('、');
                cboCell.Value = row["ReducerRatio"].ToString().Split('、')[0];
                newRow.Cells.Add(txtCell);
                newRow.Cells.Add(cboCell);
                formMain.dgvReducerInfo.Rows.Add(newRow);
            });
        }

        public void Load() {
            // 馬達選項更新
            motorPower.UpdateMotorCalcMode();
            motorPower.Load();

            // 進階選項驗證
            formMain.chkAdvanceMode.Checked = false;
            formMain.panelAdvanceMode.Enabled = formMain.optCalcSelectedModel.Checked;
            ChkAdvanceMode_CheckedChanged(null, null);

            // 減速比顯示
            formMain.panelReducer.Visible = ((Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text)) == Model.ModelType.歐規皮帶滑台;

            // 驗證最大荷重
            inputValidate.ValidatingLoad(isShowAlarm: false);

            // 依照機構型態修正預設行程
            formMain.txtStroke.Text = formMain.cboModelType.Text.Contains("皮帶") ? "1000" : "70";

            // 驗證最大行程
            inputValidate.ValidatingStroke(isShowAlarm: false);

            // 重置版面
            recommandList.Refresh();
            chartInfo.Refresh();
            formMain.cmdCalcSelectedModelConfirmStep2.Visible = false;
        }        

        private void InitEvents() {
            // 進階選項
            formMain.chkAdvanceMode.CheckedChanged += ChkAdvanceMode_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;
            formMain.cmdCalcSelectedModelConfirmStep2.Click += CmdConfirmStep2_Click;
        }

        private void ChkAdvanceMode_CheckedChanged(object sender, EventArgs e) {
            formMain.panelAdvanceParams.Visible = formMain.chkAdvanceMode.Checked;
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step2) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep3);
            }
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 版面修正
            if (formMain.optCalcAllModel.Checked) {
                formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, maxHeight);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep2);
            }
            formMain.panelSelectCalcResult.Visible = formMain.optCalcSelectedModel.Checked;

            // 最後更新使用條件
            runCondition.UpdateCondition(null, null);

            threadCalc = new Thread(() => {
                Thread.Sleep(100);

                // 開始計算
                var result = calc.GetRecommandResult(runCondition.curCondition);
                // 規件規格
                recommandList.curRecommandList = result["List"] as List<Model>;
                // 回傳訊息
                string msg = result["Msg"] as string;
                // 是否跳出Alarm
                bool isAlarm = (bool)result["Alarm"];

                // 搜尋不到型號驗證
                if (recommandList.curRecommandList.Count == 0) {
                    formMain.Invoke(new Action(() => MessageBox.Show("此使用條件無法計算，請嘗試調整使用條件。")));

                    // 訊息顯示
                    if (!string.IsNullOrEmpty(result["Msg"] as string)) {
                        // 訊息斷行顯示
                        string alarmMsg = result["Msg"] as string;
                        string showMsg = "";
                        alarmMsg.Split('|').ToList().ForEach(alarm => {
                            if (string.IsNullOrEmpty(alarm))
                                return;
                            int index = alarmMsg.Split('|').ToList().IndexOf(alarm) + 1;
                            //showMsg += index + ". " + alarm + "\r\n";
                            showMsg += alarm + "\r\n";
                        });
                        //MessageBox.Show(showMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(showMsg, SideTable.MsgStatus.Alarm)));
                    }

                    return;
                }
                // 清空訊息
                formMain.Invoke(new Action(formMain.sideTable.ClearMsg));

                // 表單顯示
                recommandList.DisplayRecommandList();
            });
            threadCalc.Start();

            // Loading顯示
            ShowWaiting();
        }

        private void ShowWaiting() {
            new Thread(() => {
                formMain.Invoke(new Action(() => {
                    FormWaiting wait = new FormWaiting(calc.GetCalcPercent);
                    wait.GetPercent = calc.GetCalcPercent;
                    wait.ShowDialog();
                }));
            }).Start();
        }        

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            //formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            //formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
