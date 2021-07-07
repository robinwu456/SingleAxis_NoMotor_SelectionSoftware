﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Page2 {
        public int minHeight = 800;
        public int maxHeight = 1333;
        public Dictionary<RadioButton, Model.ModelType> modelTypeOptMap = new Dictionary<RadioButton, Model.ModelType>();
        public Dictionary<RadioButton, Model.SetupMethod> setupMethodOptMap = new Dictionary<RadioButton, Model.SetupMethod>();
        public Model.ModelType curSelectModelType = Model.ModelType.螺桿系列;
        public Calculation calc = new Calculation();
        // Step2各項目
        public MotorPower motorPower;
        public ChartInfo chartInfo;
        public RunCondition runCondition;
        public InputValidate inputValidate;
        public RecommandList recommandList;
        public EffectiveStroke effectiveStroke;
        public ModelSelection modelSelection;

        private FormMain formMain;
        private Thread threadCalc;
        private string[] titles = { 
            //"SelectionMode",    // 選型方式
            "UseEnv",           // 使用環境
            "ModelType",        // 傳動方式
            "ModelSelection",   // 型號選擇
            "Setup",            // 安裝方式
            "Moment",           // 力矩長度
            "Calc",             // 計算
            //"Chart",            // 圖表
        };

        public Page2(FormMain formMain) {
            this.formMain = formMain;

            // 機構類型opt統整
            modelTypeOptMap = new Dictionary<RadioButton, Model.ModelType>() {
                { formMain.optStandardScrewActuator, Model.ModelType.螺桿系列 },
                { formMain.optBuildInScrewActuator, Model.ModelType.軌道內崁螺桿系列 },
                { formMain.optBuildInRodTypeScrewActuator, Model.ModelType.軌道內崁推桿滑台 },
                { formMain.optNoTrackRodTypeActuator, Model.ModelType.推桿系列 },
                { formMain.optBuildOutRodTypeActuator, Model.ModelType.軌道外掛推桿系列 },
                { formMain.optSupportTrackRodTypeActuator, Model.ModelType.輔助導桿推桿系列 },
                { formMain.optStandardBeltActuator, Model.ModelType.皮帶系列 },
                { formMain.optEuropeBeltActuator, Model.ModelType.歐規皮帶系列 },
                { formMain.optBuildInBeltActuator, Model.ModelType.軌道內崁皮帶系列 },
                { formMain.optBuildInSupportTrackActuator, Model.ModelType.軌道內崁皮帶系列 },
            };

            // 安裝方式opt統整
            setupMethodOptMap = new Dictionary<RadioButton, Model.SetupMethod>() {
                { formMain.optHorizontalUse, Model.SetupMethod.Horizontal },
                { formMain.optWallHangingUse, Model.SetupMethod.WallHang },
                { formMain.optVerticalUse, Model.SetupMethod.Vertical },
            };

            InitEvents();            

            // 型號選擇
            modelSelection = new ModelSelection(formMain);
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
            // 版面更新
            UpdateLayout(null, null);

            // Alarm清除
            formMain.Controls.All().Where(control => control.Name.EndsWith("Alarm")).ToList()
                                   .ForEach(control => control.Visible = false);

            // 更新型號選擇
            formMain.sideTable.UpdateItem();
            formMain.sideTable.UpdateMsg(calc.GetModelTypeComment(curSelectModelType), SideTable.MsgStatus.Normal);

            // 偵測傳動方式有無
            DetectModelTypeData();

            // 馬達選項更新
            motorPower.UpdateMotorCalcMode();
            motorPower.Load();

            // 減速比顯示
            formMain.panelReducer.Visible = formMain.optEuropeBeltActuator.Checked;

            // 驗證最大荷重
            inputValidate.ValidatingLoad(isShowAlarm: false);

            // 依照機構型態修正預設行程
            formMain.txtStroke.Text = curSelectModelType.IsBeltType() ? "1000" : "70";

            // 驗證最大行程
            inputValidate.ValidatingStroke(isShowAlarm: false);

            // 重置版面
            recommandList.Refresh();
            chartInfo.Clear();

            // 有效行程顯示
            effectiveStroke.IsShowEffectiveStroke(false);

            // 側邊欄位置重整
            formMain.sideTable.RePosition();

            // 一開始就顯示側邊資訊
            formMain.sideTable.Update(null, null);

            // scrollbars
            runCondition.scrollBarStroke.Initialize();
            runCondition.scrollBarLoad.Initialize();
            runCondition.scrollBarStroke.Value = 70;
            runCondition.scrollBarLoad.Value = 12;
        }

        private void InitEvents() {
            // 使用環境
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optStandardEnv.CheckedChanged += DetectedUseEnvModelType;

            // 當前機構型態更新
            modelTypeOptMap.Keys.ToList().ForEach(opt => opt.CheckedChanged += ModelType_CheckedChanged);

            // 安裝方式
            setupMethodOptMap.Keys.ToList().ForEach(opt => opt.CheckedChanged += UseEnv_CheckedChanged);

            // 進階選項
            formMain.chkAdvanceMode.CheckedChanged += ChkAdvanceMode_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;

            // 上一頁
            formMain.lbPrePage.Click += PrePage_Click;
        }

        private void UseEnv_CheckedChanged(object sender, EventArgs e) {
            formMain.sideTable.Update(null, null);
        }

        private void PrePage_Click(object sender, EventArgs e) {
            formMain.tabMain.SelectTab("tabStart");

            // 側邊欄移除
            formMain.panelSideTable.Visible = false;
        }

        private void UpdateLayout(object sender, EventArgs e) {
            // 部分panel隱藏處理
            formMain.panelUseEnv.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;          // 使用環境
            formMain.panelModelType.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;       // 傳動方式
            formMain.panelModelSelection.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;  // 型號選擇
            formMain.panelCalcResult.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;      // 推薦規格

            // 項目索引修正
            int titleIndex = 0;
            titles.ToList().ForEach(title => {
                Label lbTite = formMain.explorerBar.Controls.Find("lbTitle" + title, true)[0] as Label;
                Panel panelSelection = formMain.explorerBar.Controls.Find("panel" + title, true)[0] as Panel;
                if (panelSelection.Visible)
                    titleIndex++;
                lbTite.Text = Regex.Replace(lbTite.Text, @"\d+", titleIndex.ToString());
            });
        }

        private void ModelType_CheckedChanged(object sender, EventArgs e) {
            curSelectModelType = modelTypeOptMap.First(pair => pair.Key.Checked).Value;
            // 更新顯示傳動方式
            formMain.sideTable.Update(null, null);
            // 更新顯示傳動方式敘述
            formMain.sideTable.UpdateMsg(calc.GetModelTypeComment(curSelectModelType), SideTable.MsgStatus.Normal);
            // 驗證安裝方式選項
            Model.SetupMethod[] modelTypeSupportSetupMethod = calc.GetSupportMethod(curSelectModelType);
            setupMethodOptMap.ToList().ForEach(pair => pair.Key.Enabled = modelTypeSupportSetupMethod.Contains(pair.Value));
            if (setupMethodOptMap.ToList().Any(pair => pair.Key.Checked && !pair.Key.Enabled))
                setupMethodOptMap.ToList().First(pair => pair.Key.Enabled).Key.Checked = true;
        }

        private void ChkAdvanceMode_CheckedChanged(object sender, EventArgs e) {
            formMain.panelAdvanceParams.Enabled = formMain.chkAdvanceMode.Checked;
            formMain.panelAdvanceParams.Visible = formMain.chkAdvanceMode.Checked;
            if (!formMain.chkAdvanceMode.Checked)
                formMain.optMaxSpeedType_mms.Checked = true;
            
            if (formMain.chkAdvanceMode.Checked) {
                // 切進接選項自動換算最大加速度(加速時間=0.2/0.4)
                string model = formMain.cboModel.Text;
                double lead = Convert.ToDouble(formMain.cboLead.Text);
                int reducerRatio = 1;
                if (formMain.page2.calc.IsContainsReducerRatio(model)) {
                    string dgvReducerRatioValue = formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().ToList().First(row => row.Cells["columnModel"].Value.ToString() == model).Cells["columnReducerRatio"].Value.ToString();
                    reducerRatio = Convert.ToInt32(dgvReducerRatioValue);
                    lead /= reducerRatio;
                }
                int maxAccelSpeed = formMain.page2.calc.GetMaxAccelSpeed(model, lead, reducerRatio, Convert.ToInt32(formMain.txtStroke.Text));
                formMain.txtAccelSpeed.Text = maxAccelSpeed.ToString();

                // 切進接選項Vmax自動帶100mm/s
                formMain.txtMaxSpeed.Text = "100";
            }

        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            if (recommandList.curSelectModel.model == null)
                return;
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
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
                        chartInfo.Clear();
                        recommandList.curSelectModel = (null, -1);
                        // 側邊欄
                        formMain.sideTable.ClearModelImg();
                        formMain.sideTable.ClearModelInfo();
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
                            showMsg += alarm + "\r\n";
                        });
                        formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(showMsg, SideTable.MsgStatus.Alarm)));
                    }

                    // 有效行程顯示
                    formMain.page2.effectiveStroke.IsShowEffectiveStroke(false);

                    return;
                }
                // 清空訊息
                formMain.Invoke(new Action(formMain.sideTable.ClearMsg));

                // 表單顯示
                recommandList.DisplayRecommandList();

                // 有效行程顯示、賦值
                formMain.Invoke(new Action(() => effectiveStroke.IsShowEffectiveStroke(true)));
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
        
        private void DetectModelTypeData() {
            // 搜尋有資料的機構型態
            Model.ModelType[] notNullModelTypes = calc.GetNotNullModelType();
            modelTypeOptMap.ToList().ForEach(pair => {
                bool isDataNotNull = notNullModelTypes.Contains(pair.Value);
                pair.Key.Enabled = isDataNotNull;

                // 圖片處理
                if (!isDataNotNull) {
                    PictureBox picModelType = formMain.panelModelType.Controls.Find(pair.Key.Name.Replace("opt", "pic"), true)[0] as PictureBox;
                    picModelType.Image = Properties.Resources.ComingSoon;
                }
            });
        }

        private void DetectedUseEnvModelType(object sender, EventArgs e) {
            Model.UseEnvironment useEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.Standard : Model.UseEnvironment.DustFree;
            Model.ModelType[] dustfreeModelTypes = calc.GetDustfreeModelType();
            modelTypeOptMap.ToList().ForEach(pair => {
                if (useEnv == Model.UseEnvironment.DustFree)
                    pair.Key.Visible = dustfreeModelTypes.Contains(pair.Value);
                else
                    pair.Key.Visible = true;

                // 圖片處理
                PictureBox picModelType = formMain.panelModelType.Controls.Find(pair.Key.Name.Replace("opt", "pic"), true)[0] as PictureBox;
                picModelType.Visible = pair.Key.Visible;
            });

            // 目前選的沒有無塵，就自動選擇第一個
            if (useEnv == Model.UseEnvironment.DustFree && !dustfreeModelTypes.Contains(curSelectModelType))
                formMain.optBuildInScrewActuator.Checked = true;
        }
    }
}
