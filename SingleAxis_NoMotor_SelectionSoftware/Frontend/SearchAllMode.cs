using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class SearchAllMode {
        public Condition curCondition = new Condition();

        private FormMain formMain;

        public SearchAllMode(FormMain formMain) {
            this.formMain = formMain;

            // 計算模式
            formMain.cboMaxCalcMode.DataSource = new string[]{ "運行速度", "加速度", "加速時間" };

            // 計算單位
            formMain.cboMaxCalcUnit.DataSource = new string[] { "mm/s", "RPM" };

            InitEvents();
        }

        private void InitEvents() {
            // 條件修正
            formMain.panelSetup.Controls.All().Concat(formMain.panelCalc.Controls.All())
                .Where(c => c is TextBox || c is ComboBox)
                .ToList()
                .ForEach(c => c.TextChanged += UpdateCondition);

            formMain.chkCalcAllMode.CheckedChanged += ChkCalcAllMode_CheckedChanged;
            formMain.cboMaxCalcMode.SelectedValueChanged += CboMaxCalcMode_SelectedValueChanged;
        }

        private void CboMaxCalcMode_SelectedValueChanged(object sender, EventArgs e) {
            switch (formMain.cboMaxCalcMode.Text) {
                case "運行速度":
                    formMain.cboMaxCalcUnit.DataSource = new string[] { "mm/s", "RPM" };
                    break;
                case "加速度":
                    formMain.cboMaxCalcUnit.DataSource = new string[] { "mm/s²", "G" };
                    break;
                case "加速時間":
                    formMain.cboMaxCalcUnit.DataSource = new string[] { "s" };
                    break;
            }
        }

        private void ChkCalcAllMode_CheckedChanged(object sender, EventArgs e) {
            // 行程對照轉速
            formMain.chkRpmLimitByStroke.Visible = formMain.chkCalcAllMode.Checked;
            // 運算模式panel
            formMain.panelCalcAllMode.Visible = formMain.chkCalcAllMode.Checked;
        }

        public void UpdateCondition(object sender, EventArgs e) {
            if (formMain.page2 == null)
                return;
            // 全數值驗證
            if (!formMain.page2.inputValidate.VerifyAllInputValidate())
                return;
            // 控制項為Disable時，不修正條件
            if (sender != null && ((Control)sender).Enabled == false)
                return;

            // 計算模式
            curCondition.calcMode = Condition.CalcMode.CalcMax;

            // 安裝方式
            if (formMain.optHorizontalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.水平;
            else if (formMain.optWallHangingUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.橫掛;
            else if (formMain.optVerticalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.垂直;

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
                countPerMinute = Convert.ToDouble(formMain.txtTimesPerMinute.Text),
                // 小時/日
                hourPerDay = Convert.ToInt32(formMain.txtHourPerDay.Text),
                // 日/年
                dayPerYear = Convert.ToInt32(formMain.txtDayPerYear.Text)
            };
            // 希望壽命
            curCondition.expectServiceLifeTime = formMain.chkExpectServiceLife.Checked ? Convert.ToInt32(formMain.txtExpectServiceLifeTime.Text) : Condition.defaultExpectServiceLifeTime;

            // 計算模式
            curCondition.vMaxCalcMode = Condition.CalcVmax.Custom;
            //curCondition.accelTime = 0.2;
            switch (formMain.cboMaxCalcMode.Text) {
                case "運行速度":
                    curCondition.calcMaxItem = Condition.CalcMaxItem.Vmax;
                    curCondition.vMax = Convert.ToDouble(formMain.txtMaxCalc.Text);
                    break;
                case "加速度":
                    curCondition.calcMaxItem = Condition.CalcMaxItem.AccelSpeed;
                    curCondition.accelSpeed = Convert.ToDouble(formMain.txtMaxCalc.Text);
                    break;
                case "加速時間":
                    curCondition.calcMaxItem = Condition.CalcMaxItem.AccelTime;
                    curCondition.accelTime = Convert.ToDouble(formMain.txtMaxCalc.Text);
                    break;
            }
            switch (formMain.cboMaxCalcUnit.Text) {
                case "mm/s":
                    curCondition.calcMaxUnit = Condition.CalcMaxUnit.mms;
                    break;
                case "RPM":
                    curCondition.calcMaxUnit = Condition.CalcMaxUnit.RPM;
                    break;
                case "mm/s²":
                    curCondition.calcMaxUnit = Condition.CalcMaxUnit.mms2;
                    break;
                case "G":
                    curCondition.calcMaxUnit = Condition.CalcMaxUnit.G;
                    break;
                case "s":
                    curCondition.calcMaxUnit = Condition.CalcMaxUnit.s;
                    break;
            }

            // 馬達瓦數
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection) {
                // 全部計算只能標準或自訂
                if (formMain.cboPower.Text == "標準")
                    curCondition.powerSelection = Condition.PowerSelection.Standard;
                else if (formMain.cboPower.Text == "自訂") {
                    if (formMain.chkMotorAdvanceMode.Checked) {
                        curCondition.powerSelection = Condition.PowerSelection.Custom;
                    } else {
                        curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                        curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                    }
                }
            } else if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                // 單項計算可選擇該型號適用瓦數
                if (formMain.cboPower.Text.Contains("標準-")) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(new Regex(@"\d+").Match(formMain.cboPower.Text).Value);
                } else if (formMain.cboPower.Text == "自訂" && !formMain.chkMotorAdvanceMode.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.SelectedPower;
                    curCondition.selectedPower = Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text);
                } else if (formMain.cboPower.Text == "自訂" && formMain.chkMotorAdvanceMode.Checked) {
                    curCondition.powerSelection = Condition.PowerSelection.Custom;
                }
            }
            // 馬達參數自訂
            if (curCondition.powerSelection == Condition.PowerSelection.Custom) {
                if (formMain.chkMotorAdvanceMode.Checked) {
                    curCondition.ratedTorque = Convert.ToDouble(formMain.txtRatedTorque.Text);
                    curCondition.maxTorque = Convert.ToDouble(formMain.txtMaxTorque.Text);
                    curCondition.rotateInertia = Convert.ToDouble(formMain.txtRotateInertia.Text);
                    curCondition.loadInertiaMomentRatio = Convert.ToInt32(formMain.txtLoadInertiaMomentRatio.Text);
                    curCondition.reducerRotateInertia = Convert.ToDouble(formMain.txtReducerRotateInertia.Text);

                    formMain.page2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.page2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.page2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                    formMain.page2.motorPower.customMotorParams.loadInertiaMomentRatio = curCondition.loadInertiaMomentRatio;
                } else {
                    var p = formMain.page2.calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));
                    curCondition.ratedTorque = p.ratedTorque;
                    curCondition.maxTorque = p.maxTorque;
                    curCondition.rotateInertia = p.rotateInertia;
                    curCondition.loadInertiaMomentRatio = p.loadInertiaMomentRatio;

                    formMain.page2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.page2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.page2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                    formMain.page2.motorPower.customMotorParams.loadInertiaMomentRatio = curCondition.loadInertiaMomentRatio;
                }
            }

            curCondition.curCheckedModel = formMain.page2.recommandList.curCheckedModel;

            // 是否使用行程對照轉速
            curCondition.isRpmLimitByStroke = formMain.chkRpmLimitByStroke.Checked;
        }
    }
}
