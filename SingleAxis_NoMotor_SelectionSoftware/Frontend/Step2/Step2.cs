using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step2 {
        private FormMain formMain;
        private Calculation calc = new Calculation();

        public int minHeight = 787;
        private int maxHeight = 1300;

        private Condition curCondition = new Condition();
        private List<Model> curRecommandList;
        private Thread threadCalc;

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
            //formMain.tabControlAdvanceOptions.ItemSize = new Size(0, 1);
        }

        public void UpdateCondition() {
            curCondition.setupMethod = Model.SetupMethod.Horizontal;
            //curCondition.vMax = 100;
            curCondition.moment_A = 100;
            curCondition.modelType = Model.ModelType.Screw;
            curCondition.stroke = 70;
            curCondition.load = 12;
            curCondition.moveTime = 5;
            curCondition.useFrequence = new Condition.UseFrequence() { countPerMinute = 1, hourPerDay = 8, dayPerYear = 240 };
            curCondition.accelTime = 0.2;
            curCondition.RepeatabilityCondition = repeatability => repeatability <= 0.01;
            curCondition.powerSelection = Condition.PowerSelection.Standard;
            curCondition.calcCloseToStandardItem = Condition.CalcAccordingItem.None;
            curCondition.calcModel = (null, -1);
        }

        private void InitEvents() {
            // 進階選項
            //formMain.toggleAdvanceOptions.CheckedChanged += ToggleAdvanceOptions_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
            formMain.sideTable.Update(null, null);
            formMain._explorerBar.UpdateCurStep(formMain.curStep);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep3);
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 版面修正
            formMain.explorerBarPanel2.Size = new Size(formMain.explorerBarPanel2.Size.Width, maxHeight);
            formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep2);

            // 條件
            UpdateCondition();

            threadCalc = new Thread(() => {
                Thread.Sleep(100);

                // 開始計算
                var result = calc.GetRecommandResult(curCondition);
                // 規件規格
                curRecommandList = result["List"] as List<Model>;
                // 回傳訊息
                string msg = result["Msg"] as string;
                // 是否跳出Alarm
                bool isAlarm = (bool)result["Alarm"];

                // 表單顯示
                DisplayRecommandList();
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

        private void DisplayRecommandList() {
            formMain.Invoke(new Action(() => {
                formMain.dgvRecommandList.Rows.Clear();
            }));

            foreach (Model model in curRecommandList) {
                formMain.Invoke(new Action(() => {
                    int index = formMain.dgvRecommandList.Rows.Add();
                    formMain.dgvRecommandList.Rows[index].Height = 35;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnLock"].Value = false;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnModel"].Value = model.name;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnLead"].Value = model.lead;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnLoad"].Value = model.load;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnRpm"].Value = model.rpm;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnVmax"].Value = model.vMax;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnAccelSpeed"].Value = model.accelSpeed;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnStroke"].Value = model.maxStroke;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnRunTime"].Value = model.moveTime;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnMomentA"].Value = model.moment_A;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnMomentB"].Value = model.moment_B;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnMomentC"].Value = model.moment_C;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnPower"].Value = model.usePower;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnTmax"].Value = model.tMaxSafeCoefficient;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnTrms"].Value = model.tRmsSafeCoefficient;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnIsRecommand"].Value = Properties.Resources.inCondition;
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnDetail"].Value = Properties.Resources.detail_disable_in_condition;

                    // 運行距離
                    if (model.slideTrackServiceLifeDistance < 0)
                        formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnServiceDistance"].Value = "Error";
                    else {
                        if (model.serviceLifeDistance > 20000)
                            formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnServiceDistance"].Value = "2萬公里以上";
                        else
                            formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnServiceDistance"].Value = ((float)model.serviceLifeDistance / 10000f).ToString("#0.0") + "萬公里";
                    }

                    // 使用壽命時間
                    string useTime = "";
                    if (model.serviceLifeTime.year >= 10)
                        useTime = "10年以上";
                    else {
                        if (model.serviceLifeTime.year > 0)
                            useTime += model.serviceLifeTime.year + "年";
                        if (model.serviceLifeTime.month > 0)
                            useTime += model.serviceLifeTime.month + "個月";
                        if (model.serviceLifeTime.year == 0 && model.serviceLifeTime.month == 0)
                            useTime = "1個月以下";
                    }
                    formMain.dgvRecommandList.Rows[index].Cells["dgvRecommandColumnServiceLife"].Value = useTime;
                }));
            }
        }

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            //formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            //formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
