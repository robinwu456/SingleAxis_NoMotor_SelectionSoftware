using System;
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

        public int minHeight = 800;
        private int maxHeight = 1333;        
        
        private Thread threadCalc;                

        // Step2各項目
        public MotorPower motorPower;
        public ChartInfo chartInfo;
        public RunCondition runCondition;
        public InputValidate inputValidate;
        public RecommandList recommandList;
        public EffectiveStroke effectiveStroke;

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
            // 有效行程
            effectiveStroke = new EffectiveStroke(formMain);

            // 減速比
            calc.reducerInfo.Rows.Cast<DataRow>().ToList().ForEach(row => {
                DataGridViewRow dgvRow = (DataGridViewRow)formMain.dgvReducerInfo.RowTemplate.Clone();
                formMain.dgvReducerInfo.Rows.Add(dgvRow);
                dgvRow = formMain.dgvReducerInfo.Rows[calc.reducerInfo.Rows.Cast<DataRow>().ToList().IndexOf(row)];
                dgvRow.Cells["columnModel"].Value = row["Model"].ToString();
                (dgvRow.Cells["columnReducerRatio"] as DataGridViewComboBoxCell).DataSource = row["ReducerRatio"].ToString().Split('、');
                dgvRow.Cells["columnReducerRatio"].Value = row["ReducerRatio"].ToString().Split('、')[0];
            });
        }

        public void Load() {
            // 馬達選項更新
            motorPower.UpdateMotorCalcMode();
            motorPower.Load();

            // 進階選項驗證
            formMain.chkAdvanceMode.Checked = false;
            //formMain.panelAdvanceMode.Enabled = formMain.optCalcSelectedModel.Checked;
            formMain.panelAdvanceMode.Visible = formMain.optCalcSelectedModel.Checked;
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
            chartInfo.Clear();
            formMain.step2.SetSelectedModelConfirmBtnVisible(false);

            // 有效行程顯示
            effectiveStroke.IsShowEffectiveStroke(false);
        }

        public void SetSelectedModelConfirmBtnVisible(bool visible) {
            if (formMain.cmdCalcSelectedModelConfirmStep2.Visible == visible)
                return;

            formMain.panelCalcBtns.Visible = false;
            formMain.panelCalcBtns.ColumnStyles.Clear();
            if (visible) {
                ColumnStyle[] styles = {
                    new ColumnStyle(SizeType.Percent, 33.33f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 33.33f),
                };
                foreach (ColumnStyle style in styles)
                    formMain.panelCalcBtns.ColumnStyles.Add(style);
            } else {
                ColumnStyle[] styles = {
                    new ColumnStyle(SizeType.Percent, 41.67f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 41.67f),
                };
                foreach (ColumnStyle style in styles)
                    formMain.panelCalcBtns.ColumnStyles.Add(style);
            }
            formMain.cmdCalcSelectedModelConfirmStep2.Visible = visible;
            formMain.panelCalcBtns.Visible = true;
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
            if (!formMain.chkAdvanceMode.Checked)
                formMain.optMaxSpeedType_mms.Checked = true;
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            if (recommandList.curSelectModel.model == null)
                return;

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
            //formMain.dgvCalcSelectedModel.Visible = formMain.optCalcSelectedModel.Checked;

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
                    // 清空推薦規格
                    formMain.Invoke(new Action(() => {
                        formMain.dgvRecommandList.DataSource = null;
                        formMain.dgvRecommandList.Rows.Clear();
                        formMain.dgvCalcSelectedModel.DataSource = null;
                        chartInfo.Clear();
                        recommandList.curSelectModel = (null, -1);
                        // 側邊欄
                        formMain.sideTable.ClearModelImg();
                        formMain.sideTable.ClearModelInfo();
                        if (formMain.optCalcSelectedModel.Checked)
                            formMain.sideTable.ClearSelectedModelInfo();
                    }));

                    formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg("此使用條件無法計算，請嘗試調整使用條件。", SideTable.MsgStatus.Alarm)));

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
                        formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(showMsg, SideTable.MsgStatus.Alarm)));
                    }

                    // 有效行程顯示
                    formMain.step2.effectiveStroke.IsShowEffectiveStroke(false);

                    return;
                }
                // 清空訊息
                formMain.Invoke(new Action(formMain.sideTable.ClearMsg));

                // 表單顯示
                recommandList.DisplayRecommandList();

                // 有效行程顯示、賦值
                formMain.Invoke(new Action(() => {
                    effectiveStroke.IsShowEffectiveStroke(true);
                }));
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
    }
}
