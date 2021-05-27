using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {    
    public class RunCondition {
        public Condition curCondition = new Condition();

        private FormMain formMain;

        public RunCondition(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        private void InitEvents() {
            // 希望壽命
            formMain.optNoExpectServiceLife.CheckedChanged += ExpectServiceLife_CheckedChanged;
            formMain.optExpectServiceLife.CheckedChanged += ExpectServiceLife_CheckedChanged;

            // 更新條件
            formMain.optStandardEnv.CheckedChanged += UpdateCondition;
            formMain.optDustFreeEnv.CheckedChanged += UpdateCondition;
            formMain.cboModelType.SelectedIndexChanged += UpdateCondition;
            formMain.optHorizontalUse.CheckedChanged += UpdateCondition;
            formMain.optWallHangingUse.CheckedChanged += UpdateCondition;
            formMain.optUpsideDownUse.CheckedChanged += UpdateCondition;
            formMain.optCalcAllModel.CheckedChanged += UpdateCondition;
            formMain.optCalcSelectedModel.CheckedChanged += UpdateCondition;
            formMain.cboSeries.SelectedIndexChanged += UpdateCondition;
            formMain.cboModel.SelectedIndexChanged += UpdateCondition;
            formMain.cboLead.SelectedIndexChanged += UpdateCondition;
            formMain.txtMomentA.TextChanged += UpdateCondition;
            formMain.txtMomentB.TextChanged += UpdateCondition;
            formMain.txtMomentC.TextChanged += UpdateCondition;
            formMain.txtLoad.TextChanged += UpdateCondition;
            formMain.txtStroke.TextChanged += UpdateCondition;
            formMain.txtRunTime.TextChanged += UpdateCondition;
            formMain.txtTimesPerMinute.TextChanged += UpdateCondition;
            formMain.txtHourPerDay.TextChanged += UpdateCondition;
            formMain.txtDayPerYear.TextChanged += UpdateCondition;
            formMain.optNoExpectServiceLife.CheckedChanged += UpdateCondition;
            formMain.optExpectServiceLife.CheckedChanged += UpdateCondition;
            formMain.txtExpectServiceLifeTime.TextChanged += UpdateCondition;
            formMain.txtMaxSpeed.TextChanged += UpdateCondition;
            formMain.txtAccelSpeed.TextChanged += UpdateCondition;
            formMain.cboPower.SelectedIndexChanged += UpdateCondition;
            formMain.optMotorParamsModifySimple.CheckedChanged += UpdateCondition;
            formMain.optMotorParamsModifyAdvance.CheckedChanged += UpdateCondition;
            formMain.cboMotorParamsMotorPowerSelection.SelectedIndexChanged += UpdateCondition;
            formMain.txtRatedTorque.TextChanged += UpdateCondition;
            formMain.txtRotateInertia.TextChanged += UpdateCondition;
            formMain.txtMaxTorque.TextChanged += UpdateCondition;
            formMain.optRepeatabilityScrew.CheckedChanged += UpdateCondition;
            formMain.optMaxSpeedType_mms.CheckedChanged += UpdateCondition;
            formMain.dgvReducerInfo.CellValueChanged += UpdateCondition;
            formMain.optExpectServiceLife.CheckedChanged += UpdateCondition;
        }

        private void ExpectServiceLife_CheckedChanged(object sender, EventArgs e) {
            formMain.panelExpectServiceLifeTime.Enabled = formMain.optExpectServiceLife.Checked;
        }

        public void UpdateCondition(object sender, EventArgs e) {
            // 不是Step2時不修正條件
            if (formMain.curStep != FormMain.Step.Step2)
                return;
            // 全數值驗證
            if (!formMain.step2.inputValidate.VerifyAllInputValidate())
                return;
            // 控制項為Disable時，不修正條件
            if (sender != null && ((Control)sender).Enabled == false)
                return;
            // 推薦規格選項變更不影響條件
            if (sender is DataGridView)
                return;

            // 使用環境
            if (formMain.optStandardEnv.Checked)
                curCondition.useEnvironment = Model.UseEnvironment.Standard;
            else if (formMain.optDustFreeEnv.Checked)
                curCondition.useEnvironment = Model.UseEnvironment.DustFree;
            // 機構型態
            curCondition.modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text);
            // 安裝方式
            if (formMain.optHorizontalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.WallHang;
            else if (formMain.optUpsideDownUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Vertical;
            // 最高速度
            if (formMain.optMaxSpeedType_mms.Checked)
                curCondition.vMax = Convert.ToDouble(formMain.txtMaxSpeed.Text);
            else if (formMain.optMaxSpeedType_rpm.Checked) {
                if (formMain.txtMaxSpeed.Text.Contains("."))
                    formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                if (formMain.step2.calc.IsContainsReducerRatio(formMain.cboModel.Text)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text).Cells["columnReducerRatio"].Value.ToString();
                    curCondition.vMax = formMain.step2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text) / Convert.ToDouble(dgvReducerRatioValue));
                } else
                    curCondition.vMax = formMain.step2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text));
            }
            curCondition.vMaxCalcMode = !formMain.chkAdvanceMode.Checked ? Condition.CalcVmax.Max : Condition.CalcVmax.Custom;
            // 力矩參數
            curCondition.moment_A = Convert.ToInt32(formMain.txtMomentA.Text);
            curCondition.moment_B = Convert.ToInt32(formMain.txtMomentB.Text);
            curCondition.moment_C = Convert.ToInt32(formMain.txtMomentC.Text);
            // 行程
            curCondition.stroke = Convert.ToInt32(formMain.txtStroke.Text);
            // 荷重
            curCondition.load = Convert.ToDouble(formMain.txtLoad.Text);
            // 運行時間
            curCondition.moveTime = Convert.ToDouble(formMain.txtRunTime.Text);
            // 使用頻率
            curCondition.useFrequence = new Condition.UseFrequence() {
                // 趟/分
                countPerMinute = Convert.ToInt32(formMain.txtTimesPerMinute.Text),
                // 小時/日
                hourPerDay = Convert.ToInt32(formMain.txtHourPerDay.Text),
                // 日/年
                dayPerYear = Convert.ToInt32(formMain.txtDayPerYear.Text)
            };
            // 希望壽命
            curCondition.expectServiceLifeTime = formMain.optExpectServiceLife.Checked ? Convert.ToInt32(formMain.txtExpectServiceLifeTime.Text) : -1;
            // 驗正加速時間
            if (formMain.optRepeatabilityScrew.Checked)
                curCondition.accelTime = 0.2;
            else if (formMain.optRepeatabilityBelt.Checked)
                curCondition.accelTime = 0.4;
            // 加速度
            formMain.step2.inputValidate.TxtAccelSpeed_Validating(null, null);
            if (formMain.chkAdvanceMode.Checked)
                curCondition.accelSpeed = Convert.ToDouble(formMain.txtAccelSpeed.Text);
            else
                curCondition.accelSpeed = 0;
            // 傳動方式
            if (formMain.optRepeatabilityScrew.Checked)
                curCondition.RepeatabilityCondition = repeatability => repeatability <= 0.01;
            else if (formMain.optRepeatabilityBelt.Checked)
                curCondition.RepeatabilityCondition = repeatability => repeatability >= 0.04;
            // 馬達瓦數
            if (formMain.optCalcAllModel.Checked) {
                // 全部計算只能標準或自訂
                if (formMain.cboPower.Text == "標準")
                    curCondition.powerSelection = Condition.PowerSelection.Standard;
                else if (formMain.cboPower.Text == "自訂") {
                    if (formMain.optMotorParamsModifySimple.Checked) {
                        curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                        curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                    } else if (formMain.optMotorParamsModifyAdvance.Checked)
                        curCondition.powerSelection = Condition.PowerSelection.Custom;
                }
            } else if (formMain.optCalcSelectedModel.Checked) {
                // 單項計算可選擇該型號適用瓦數
                if (formMain.cboPower.Text.Contains("標準")) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(new Regex(@"\d+").Match(formMain.cboPower.Text).Value);
                } else if (formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifySimple.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                } else if (formMain.cboPower.Text == "自訂" && formMain.optMotorParamsModifyAdvance.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.Custom;
                }
            }
            // 馬達參數自訂
            if (curCondition.powerSelection == Condition.PowerSelection.Custom) {
                if (formMain.optMotorParamsModifySimple.Checked) {
                    var p = formMain.step2.calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));
                    curCondition.ratedTorque = p.ratedTorque;
                    curCondition.maxTorque = p.maxTorque;
                    curCondition.rotateInertia = p.rotateInertia;

                    formMain.step2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.step2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.step2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                } else {
                    curCondition.ratedTorque = Convert.ToDouble(formMain.txtRatedTorque.Text);
                    curCondition.maxTorque = Convert.ToDouble(formMain.txtMaxTorque.Text);
                    curCondition.rotateInertia = Convert.ToDouble(formMain.txtRotateInertia.Text);

                    formMain.step2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.step2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.step2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                }
            }

            curCondition.curCheckedModel = formMain.step2.recommandList.curCheckedModel;

            // 單項計算
            if (!formMain.optCalcAllModel.Checked) {
                (string model, double lead) calcModel = (formMain.cboModel.Text, Convert.ToDouble(formMain.cboLead.Text));
                curCondition.calcModel = calcModel;
            } else
                curCondition.calcModel = (null, -1);
            // 減速比
            curCondition.reducerRatio.Clear();
            formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => {
                //curCondition.reducerRatio.Add(row.Cells["columnModel"].Value.ToString(), Convert.ToInt32(row.Cells["columnReducerRatio"].Value.ToString()));
                curCondition.reducerRatio[row.Cells["columnModel"].Value.ToString()] = Convert.ToInt32(row.Cells["columnReducerRatio"].Value.ToString());
            });

            // 型號選行顯示
            formMain.step2.SetSelectedModelConfirmBtnVisible(false);

            // 有效行程
            formMain.step2.effectiveStroke.IsShowEffectiveStroke(false);

            // 修正條件時，下一步隱藏
            formMain.cmdConfirmStep2.Visible = false;
        }
    }
}
