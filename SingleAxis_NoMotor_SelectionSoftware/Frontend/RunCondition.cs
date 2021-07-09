﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SingleAxis_NoMotor_SelectionSoftware {    
    public class RunCondition {
        public Condition curCondition = new Condition();
        public CustomScrollBar scrollBarStroke;
        public CustomScrollBar scrollBarLoad;

        private FormMain formMain;

        #region txt placeHolder
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        #endregion

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
            //formMain.cboModelType.SelectedIndexChanged += UpdateCondition;
            formMain.optHorizontalUse.CheckedChanged += UpdateCondition;
            formMain.optWallHangingUse.CheckedChanged += UpdateCondition;
            formMain.optVerticalUse.CheckedChanged += UpdateCondition;
            //formMain.optConditionSelection.CheckedChanged += UpdateCondition;
            //formMain.optModelSelection.CheckedChanged += UpdateCondition;
            //formMain.cboSeries.SelectedIndexChanged += UpdateCondition;
            formMain.cboModel.SelectedIndexChanged += UpdateCondition;
            //formMain.cboModel.TextChanged += UpdateCondition;
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
            //formMain.optRepeatabilityScrew.CheckedChanged += UpdateCondition;
            formMain.optMaxSpeedType_mms.CheckedChanged += UpdateCondition;
            formMain.dgvReducerInfo.CellValueChanged += UpdateCondition;
            formMain.optExpectServiceLife.CheckedChanged += UpdateCondition;

            // Custom scrollBar init
            scrollBarStroke = new CustomScrollBar(formMain, formMain.scrollBarPanelStroke, formMain.scrollBarThumbStroke, null, null);
            scrollBarStroke.Name = "scrollBarStroke";
            scrollBarStroke.minValue = 50;
            scrollBarStroke.maxValue = 6000;            
            scrollBarStroke.bindingTextBox = formMain.txtStroke;
            scrollBarStroke.picThumbHover = Properties.Resources.scrollBarThumb_hover;
            formMain.lbScrollbarMinStroke.Text = scrollBarStroke.minValue.ToString();
            formMain.lbScrollbarMaxStroke.Text = scrollBarStroke.maxValue.ToString();

            scrollBarLoad = new CustomScrollBar(formMain, formMain.scrollBarPanelLoad, formMain.scrollBarThumbLoad, null, null);
            scrollBarLoad.Name = "scrollBarStroke";
            scrollBarLoad.minValue = 0;
            scrollBarLoad.maxValue = 500;            
            scrollBarLoad.bindingTextBox = formMain.txtLoad;
            scrollBarLoad.picThumbHover = Properties.Resources.scrollBarThumb_hover;
            formMain.lbScrollbarMinLoad.Text = scrollBarLoad.minValue.ToString();
            formMain.lbScrollbarMaxLoad.Text = scrollBarLoad.maxValue.ToString();

            // txt placeHolder init
            SendMessage(formMain.txtRatedTorque.Handle, EM_SETCUEBANNER, 0, "額定轉矩");
            SendMessage(formMain.txtRotateInertia.Handle, EM_SETCUEBANNER, 0, "轉動慣量");
            SendMessage(formMain.txtMaxTorque.Handle, EM_SETCUEBANNER, 0, "最大轉矩");
            SendMessage(formMain.txtStroke.Handle, EM_SETCUEBANNER, 0, "移動行程");
            SendMessage(formMain.txtLoad.Handle, EM_SETCUEBANNER, 0, "荷重");
            SendMessage(formMain.txtRunTime.Handle, EM_SETCUEBANNER, 0, "運行時間");
            SendMessage(formMain.txtTimesPerMinute.Handle, EM_SETCUEBANNER, 0, "往返次數");
            SendMessage(formMain.txtHourPerDay.Handle, EM_SETCUEBANNER, 0, "運轉次數");
            SendMessage(formMain.txtDayPerYear.Handle, EM_SETCUEBANNER, 0, "運轉天數");
            SendMessage(formMain.txtExpectServiceLifeTime.Handle, EM_SETCUEBANNER, 0, "希望壽命");
            SendMessage(formMain.txtMaxSpeed.Handle, EM_SETCUEBANNER, 0, "運行速度");
            SendMessage(formMain.txtAccelSpeed.Handle, EM_SETCUEBANNER, 0, "加速度");
        }

        private void ExpectServiceLife_CheckedChanged(object sender, EventArgs e) {
            formMain.panelExpectServiceLifeTime.Enabled = formMain.optExpectServiceLife.Checked;
        }

        public void UpdateCondition(object sender, EventArgs e) {
            if (formMain.page2 == null)
                return;

            //// 型號選型驗證型號CBO
            //if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection)
            //    if (formMain.cboModel.Text == "" || formMain.cboLead.Text == "")
            //        return;

            // 全數值驗證
            if (!formMain.page2.inputValidate.VerifyAllInputValidate())
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
            //curCondition.modelType = (Model.ModelType)Enum.Parse(typeof(Model.ModelType), formMain.cboModelType.Text);
            curCondition.modelType = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value;
            // 安裝方式
            if (formMain.optHorizontalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Horizontal;
            else if (formMain.optWallHangingUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.WallHang;
            else if (formMain.optVerticalUse.Checked)
                curCondition.setupMethod = Model.SetupMethod.Vertical;
            // 最高速度
            if (formMain.optMaxSpeedType_mms.Checked)
                curCondition.vMax = Convert.ToDouble(formMain.txtMaxSpeed.Text);
            else if (formMain.optMaxSpeedType_rpm.Checked) {
                if (formMain.txtMaxSpeed.Text.Contains("."))
                    formMain.txtMaxSpeed.Text = formMain.txtMaxSpeed.Text.Split('.')[0];
                if (formMain.page2.calc.IsContainsReducerRatio(formMain.cboModel.Text)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == formMain.cboModel.Text).Cells["columnReducerRatio"].Value.ToString();
                    curCondition.vMax = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text) / Convert.ToDouble(dgvReducerRatioValue));
                } else
                    curCondition.vMax = formMain.page2.calc.RPM_TO_MMS(Convert.ToInt32(formMain.txtMaxSpeed.Text), Convert.ToDouble(formMain.cboLead.Text));
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
            if (formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType())
                curCondition.accelTime = 0.4;
            else
                curCondition.accelTime = 0.2;
            // 加速度
            formMain.page2.inputValidate.TxtAccelSpeed_Validating(null, null);
            if (formMain.chkAdvanceMode.Checked)
                curCondition.accelSpeed = Convert.ToDouble(formMain.txtAccelSpeed.Text);
            else
                curCondition.accelSpeed = 0;
            // 傳動方式
            if (formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType())
                curCondition.RepeatabilityCondition = repeatability => repeatability >= 0.04;
            else
                curCondition.RepeatabilityCondition = repeatability => repeatability <= 0.01;
            // 馬達瓦數
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection) {
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
            } else if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                // 單項計算可選擇該型號適用瓦數
                if (formMain.cboPower.Text.Contains("標準-")) {
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
                    var p = formMain.page2.calc.GetMotorParams(Convert.ToInt32(formMain.cboMotorParamsMotorPowerSelection.Text));
                    curCondition.ratedTorque = p.ratedTorque;
                    curCondition.maxTorque = p.maxTorque;
                    curCondition.rotateInertia = p.rotateInertia;

                    formMain.page2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.page2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.page2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                } else {
                    curCondition.ratedTorque = Convert.ToDouble(formMain.txtRatedTorque.Text);
                    curCondition.maxTorque = Convert.ToDouble(formMain.txtMaxTorque.Text);
                    curCondition.rotateInertia = Convert.ToDouble(formMain.txtRotateInertia.Text);

                    formMain.page2.motorPower.customMotorParams.ratedTorque = curCondition.ratedTorque;
                    formMain.page2.motorPower.customMotorParams.maxTorque = curCondition.maxTorque;
                    formMain.page2.motorPower.customMotorParams.rotateInertia = curCondition.rotateInertia;
                }
            }

            curCondition.curCheckedModel = formMain.page2.recommandList.curCheckedModel;

            // 單項計算
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
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

            //// 型號選行顯示
            //formMain.step2.SetSelectedModelConfirmBtnVisible(false);

            //// 有效行程
            //formMain.page2.effectiveStroke.IsShowEffectiveStroke(false);

            // 修正條件時，下一步隱藏
            formMain.cmdConfirmStep2.Visible = false;

            // 修正條件時，側邊欄壽命清除
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection)
                formMain.sideTable.ClearSelectedModelInfo();
        }
    }
}
