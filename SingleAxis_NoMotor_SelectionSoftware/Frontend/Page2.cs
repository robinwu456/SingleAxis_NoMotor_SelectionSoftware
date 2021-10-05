using System;
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
        public Model.ModelType curSelectModelType = Model.ModelType.ETH;
        public Calculation calc = new Calculation();
        // Step2各項目
        public MotorPower motorPower;
        public ChartInfo chartInfo;
        public RunCondition runCondition;
        public InputValidate inputValidate;
        public RecommandList recommandList;
        public EffectiveStroke effectiveStroke;
        public ModelSelection modelSelection;
        public SearchAllMode searchAllMode;

        private FormMain formMain;
        private Thread threadCalc;
        private string[] titles = { 
            //"SelectionMode",    // 選型方式
            "UseEnv",           // 使用環境
            "ModelType",        // 傳動方式
            "ModelSelection",   // 型號選擇
            "Setup",            // 安裝方式
            //"Moment",           // 力矩長度
            "Calc",             // 計算
            //"Chart",            // 圖表
        };

        public Page2(FormMain formMain) {
            this.formMain = formMain;

            // 機構類型opt統整
            modelTypeOptMap = new Dictionary<RadioButton, Model.ModelType>() {
                { formMain.optETH, Model.ModelType.ETH },
                { formMain.optGTH, Model.ModelType.GTH },
                { formMain.optGTY, Model.ModelType.GTY },
                { formMain.optY, Model.ModelType.Y },
                { formMain.optYL, Model.ModelType.YL },
                { formMain.optYD, Model.ModelType.YD },
                { formMain.optETB, Model.ModelType.ETB },
                { formMain.optM, Model.ModelType.M },
                { formMain.optMG, Model.ModelType.MG },
                { formMain.optGCH, Model.ModelType.GCH },
                { formMain.optECH, Model.ModelType.ECH },
                { formMain.optECB, Model.ModelType.ECB },
            };

            // 安裝方式opt統整
            setupMethodOptMap = new Dictionary<RadioButton, Model.SetupMethod>() {
                { formMain.optHorizontalUse, Model.SetupMethod.水平 },
                { formMain.optWallHangingUse, Model.SetupMethod.橫掛 },
                { formMain.optVerticalUse, Model.SetupMethod.垂直 },
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
            // 全選模式
            searchAllMode = new SearchAllMode(formMain);

            // scrollbars
            runCondition.scrollBarStroke.Initialize();
            runCondition.scrollBarLoad.Initialize();
        }

        public void Load() {
            formMain.explorerBar.ScrollControlIntoView(formMain.lbPrePage);

            // 版面更新
            UpdateLayout(null, null);

            // Alarm清除
            formMain.Controls.All().Where(control => control.Name.EndsWith("Alarm")).ToList()
                                   .ForEach(control => control.Visible = false);

            // 更新型號選擇
            formMain.sideTable.UpdateItem();
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.標準 : Model.UseEnvironment.無塵;
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                formMain.sideTable.UpdateMsg(calc.GetModelTypeComment(curSelectModelType), SideTable.MsgStatus.Normal);
            else
                formMain.sideTable.ClearMsg();

            // 偵測傳動方式有無
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                DetectModelTypeData();

            // 匯入型號選擇
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection)
                modelSelection.InitModelSelectionCbo();

            // 馬達選項更新
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
                motorPower.UpdateMotorCalcMode();
                motorPower.Load();
            }

            //// 驗證最大荷重
            //inputValidate.ValidatingLoad(isShowAlarm: false);

            //// 依照機構型態修正預設行程
            //formMain.txtStroke.Text = curSelectModelType.IsBeltType() ? "1000" : "70";

            //// 驗證最大行程
            //inputValidate.ValidatingStroke(isShowAlarm: false);

            // 重置版面
            formMain.optStandardEnv.Checked = true;
            formMain.optGTH.Checked = true;
            recommandList.Refresh();
            formMain.sideTable.ClearModelImg();
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                ModelType_CheckedChanged(null, null);
            formMain.sideTable.ClearModelInfo();
            formMain.sideTable.ClearSelectedModelInfo();
            recommandList.curSelectModel = (null, -1);
            formMain.page2.ChangeNextStepBtnVisible(false);
            chartInfo.Clear(); 
            formMain.txtMaxSpeed.Text = "";
            formMain.txtAccelSpeed.Text = "";
            formMain.lbMaxValue_MaxSpeed.Text = "( 最大值：0 mm/s )";
            formMain.lbMaxValue_AccelSpeed.Text = "( 最大值：0 mm/s² )";

            //// 有效行程顯示
            //effectiveStroke.IsShowEffectiveStroke(false);

            // 側邊欄位置重整
            formMain.sideTable.RePosition();

            // 一開始就顯示側邊資訊
            formMain.sideTable.Update(null, null);

            // 進階選項顯示
            formMain.chkAdvanceMode.Checked = false;
            formMain.panelAdvanceParams.Enabled = false;
            //formMain.panelAdvanceParams.Visible = false;
            //formMain.panelAdvanceMode.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;

            // scrollbars
            runCondition.scrollBarStroke.Value = 0;
            runCondition.scrollBarLoad.Value = RunCondition.defaultMinLoad;
            formMain.txtStroke.Text = "";
            formMain.txtLoad.Text = "";
            formMain.txtRunTime.Text = "";
            runCondition.scrollBarStroke.maxValue = RunCondition.defaultMaxStroke;
            runCondition.scrollBarLoad.maxValue = RunCondition.defaultMaxLoad;

            // 運行速度單位
            //if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection)
            //    formMain.cboMaxSpeedUnit.DataSource = new string[] { "mm/s", "RPM" };
            //else
            //    formMain.cboMaxSpeedUnit.DataSource = new string[] { "mm/s" };
            formMain.cboMaxSpeedUnit.DataSource = new string[] { "mm/s", "RPM" };

            // 全選模式
            formMain.chkCalcAllMode.Checked = false;
            formMain.chkRpmLimitByStroke.Checked = false;
            //formMain.panelCalcAllMode.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;
            //formMain.chkRpmLimitByStroke.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;

            //// RPM顯示
            //formMain.lbRpm.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;

            //// 最大值顯示
            //formMain.lbMaxValue_MaxSpeed.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;
            //formMain.lbMaxValue_AccelSpeed.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;

            // RPM轉換監測
            inputValidate.threadShowRPMCounting = new Thread(inputValidate.ShowConvertRPM);
            inputValidate.threadShowRPMCounting.Start();

            inputValidate.threadCalcVmaxRange = new Thread(inputValidate.CalcVmaxRange);
            inputValidate.threadCalcVmaxRange.Start();
            inputValidate.threadCalcAccelSpeedRange = new Thread(inputValidate.CalcAccelSpeedRange);
            inputValidate.threadCalcAccelSpeedRange.Start();

            // 更新預設系列最大行程
            int maxStroke = formMain.page2.calc.GetSeriesMaxStroke(curSelectModelType.ToString());
            formMain.page2.runCondition.scrollBarStroke.maxValue = maxStroke;
        }

        private void InitEvents() {
            // 使用環境
            formMain.optStandardEnv.CheckedChanged += formMain.sideTable.Update;
            formMain.optStandardEnv.CheckedChanged += DetectedUseEnvModelType;

            // 當前機構型態更新
            modelTypeOptMap.Keys.ToList().ForEach(opt => opt.CheckedChanged += ModelType_CheckedChanged);

            // 安裝方式
            setupMethodOptMap.Keys.ToList().ForEach(opt => opt.CheckedChanged += SteupMethod_CheckedChanged);

            // 進階選項
            formMain.chkAdvanceMode.CheckedChanged += ChkAdvanceMode_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;

            // 確認按鈕
            formMain.cmdConfirmStep2.Click += CmdConfirmStep2_Click;

            // 上一頁
            formMain.lbPrePage.Click += PrePage_Click;
            formMain.lbPrePage.MouseEnter += (sender, e) => formMain.lbPrePage.Font = new Font(formMain.lbPrePage.Font, FontStyle.Underline);
            formMain.lbPrePage.MouseLeave += (sender, e) => formMain.lbPrePage.Font = new Font(formMain.lbPrePage.Font, FontStyle.Regular);

            //// 安裝方式力矩圖片顯示
            //formMain.optHorizontalUse.CheckedChanged += UpdateMomentPic;
            //formMain.optVerticalUse.CheckedChanged += UpdateMomentPic;
            //formMain.optWallHangingUse.CheckedChanged += UpdateMomentPic;

            // 回到運轉條件
            formMain.cmdChangeRunCondition.Click += CmdChangeRunCondition_Click;

            //// 全選模式
            //formMain.pictureBoxToyo.DoubleClick += PictureBoxToyo_DoubleClick;
        }

        private void PictureBoxToyo_DoubleClick(object sender, EventArgs e) {
            if (formMain.tabMain.SelectedTab.Name != "tabContent")
                return;

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                formMain.panelCalcAllMode.Visible = true;
            else
                MessageBox.Show("條件選型才能開全選模式");
        }

        private void CmdChangeRunCondition_Click(object sender, EventArgs e) {
            formMain.explorerBar.ScrollControlIntoView(formMain.lbTitleCalc);
        }

        private void SteupMethod_CheckedChanged(object sender, EventArgs e) {
            formMain.sideTable.Update(null, null);

            // 變更力矩圖片
            if (formMain.optHorizontalUse.Checked)
                formMain.picMoment.Image = Properties.Resources.moment_horizontal;
            else if (formMain.optWallHangingUse.Checked)
                formMain.picMoment.Image = Properties.Resources.moment_wallHang;
            else if (formMain.optVerticalUse.Checked)
                formMain.picMoment.Image = Properties.Resources.moment_vertical;

            // 垂直不顯示力矩B
            formMain.panelMomentB.Visible = !formMain.optVerticalUse.Checked;
        }

        private void PrePage_Click(object sender, EventArgs e) {
            formMain.tabMain.SelectTab("tabStart");

            // 側邊欄移除
            formMain.panelSideTable.Visible = false;
            formMain.panelCalcAllMode.Visible = false;

            if (inputValidate.threadShowRPMCounting != null)
                inputValidate.threadShowRPMCounting.Abort();
            if (inputValidate.threadCalcVmaxRange != null)
                inputValidate.threadCalcVmaxRange.Abort();
            if (inputValidate.threadCalcAccelSpeedRange != null)
                inputValidate.threadCalcAccelSpeedRange.Abort();
        }

        private void UpdateLayout(object sender, EventArgs e) {
            // 部分panel隱藏處理
            formMain.panelUseEnv.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection;          // 使用環境
            formMain.panelModelType.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection;       // 傳動方式
            formMain.panelModelSelection.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection;  // 型號選擇
            formMain.panelCalcResult.Visible = formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection;      // 推薦規格
            formMain.panelMoment.Visible = !curSelectModelType.IsRodType();                                                       // 力矩長度

            // 項目索引修正
            int titleIndex = 0;
            titles.ToList().ForEach(title => {
                Label lbTite = formMain.explorerBar.Controls.Find("lbTitle" + title, true)[0] as Label;
                Panel panelSelection = formMain.explorerBar.Controls.Find("panel" + title, true)[0] as Panel;
                if (panelSelection.Visible)
                    titleIndex++;
                //lbTite.Text = Regex.Replace(lbTite.Text, @"\d+", titleIndex.ToString());
                lbTite.Text = "Step." + titleIndex;
            });
        }

        private void ModelType_CheckedChanged(object sender, EventArgs e) {
            curSelectModelType = modelTypeOptMap.First(pair => pair.Key.Checked).Value;
            // 更新顯示傳動方式
            formMain.sideTable.Update(null, null);
            // 更新顯示傳動方式敘述
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.標準 : Model.UseEnvironment.無塵;
            formMain.sideTable.UpdateMsg(calc.GetModelTypeComment(curSelectModelType), SideTable.MsgStatus.Normal);
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                formMain.sideTable.UpdateModelImg(curSelectModelType);
            // 驗證安裝方式選項
            Model.SetupMethod[] modelTypeSupportSetupMethod = calc.GetSupportMethod(curSelectModelType);
            setupMethodOptMap.ToList().ForEach(pair => pair.Key.Enabled = modelTypeSupportSetupMethod.Contains(pair.Value));
            if (setupMethodOptMap.ToList().Any(pair => pair.Key.Checked && !pair.Key.Enabled))
                setupMethodOptMap.ToList().First(pair => pair.Key.Enabled).Key.Checked = true;

            //// 依照機構型態修正預設行程
            //formMain.page2.runCondition.scrollBarStroke.Value = formMain.page2.curSelectModelType.IsBeltType() ? 1000 : 70;

            // 判斷是否為Y系列，並修正panel
            UpdateLayout(null, null);

            // 減速比顯示
            //formMain.panelReducerRatio.Visible = curSelectModelType.IsContainsReducerRatioType() && formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ShapeSelection;
            formMain.panelReducerParam.Visible = curSelectModelType.IsContainsReducerRatioType();

            // 修正時清除側邊欄資訊
            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection)
                formMain.sideTable.ClearModelInfo();

            // 更新行程
            int maxStroke = formMain.page2.calc.GetSeriesMaxStroke(curSelectModelType.ToString());
            formMain.page2.runCondition.scrollBarStroke.maxValue = maxStroke;
        }

        private void ChkAdvanceMode_CheckedChanged(object sender, EventArgs e) {
            if (formMain.chkCalcAllMode.Checked) {
                formMain.chkAdvanceMode.Checked = false;
                return;
            }

            //if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
            //    formMain.panelAdvanceParams.Enabled = formMain.chkAdvanceMode.Checked;
            //    return;
            //}

            if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection &&
                (formMain.cboModel.Text == "" || formMain.cboLead.Text == "" || !decimal.TryParse(formMain.txtStroke.Text, out decimal a))) {
                formMain.chkAdvanceMode.Checked = false;
                formMain.panelAdvanceParams.Enabled = formMain.chkAdvanceMode.Checked;

                // 單位復原
                formMain.cboMaxSpeedUnit.Text = "mm/s";

                // 清空進階選項
                formMain.txtMaxSpeed.Text = "";
                formMain.txtAccelSpeed.Text = "";

                // 進階選項值驗證Alarm隱藏
                formMain.lbMaxSpeedAlarm.Visible = false;
                formMain.lbAccelSpeedAlarm.Visible = false;
                return;
            }

            formMain.panelAdvanceParams.Enabled = formMain.chkAdvanceMode.Checked;

            // 開啟進階選項
            if (formMain.chkAdvanceMode.Checked) {
                if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection) {
                    // 線速度
                    string max = new Regex(@"\d+").Match(formMain.lbMaxValue_MaxSpeed.Text).Groups[0].Value;
                    formMain.txtMaxSpeed.Text = max;

                    // 加速度
                    double accelTime = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType() ? 0.4 : 0.2;
                    if (double.TryParse(formMain.txtMaxSpeed.Text, out double maxSpeed)) {
                        max = Convert.ToInt32((maxSpeed / accelTime).ToString("#0")).ToString();                                // 加速時間0.2
                        formMain.txtAccelSpeed.Text = max.ToString();
                    }
                } else if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
                    try {
                        var curRow = formMain.dgvRecommandList.CurrentRow;
                        if (curRow.Cells["項次"].Value == null)
                            return;
                        var selectModel = formMain.page2.recommandList.curRecommandList.Where(model => model.name == curRow.Cells["項次"].Value.ToString() && model.lead == Convert.ToDouble(curRow.Cells["導程"].Value.ToString()));
                        if (selectModel.Count() == 0)
                            return;
                        Model curModel = selectModel.First();
                        formMain.txtMaxSpeed.Text = curModel.vMax.ToString("#0");
                        formMain.txtAccelSpeed.Text = curModel.accelSpeed.ToString();
                    } catch (Exception ex) {
                        Console.WriteLine("開進階選項回填錯誤");
                    }
                }
            }

            // 關閉進階選項
            if (!formMain.chkAdvanceMode.Checked) {
                // 單位復原
                formMain.cboMaxSpeedUnit.Text = "mm/s";

                // 清空進階選項
                formMain.txtMaxSpeed.Text = "";
                formMain.txtAccelSpeed.Text = "";

                // 進階選項值驗證Alarm隱藏
                formMain.lbMaxSpeedAlarm.Visible = false;
                formMain.lbAccelSpeedAlarm.Visible = false;
            }            
        }

        private void CmdConfirmStep2_Click(object sender, EventArgs e) {
            // 取得有效行程
            effectiveStroke.GetEffectiveStroke();

            // 進下一頁
            formMain.tabMain.SelectTab("tabResult");
            formMain.page3.Load();
            formMain.sideTable.Update(null, null);
            formMain.panelCalcAllMode.Visible = false;

            if (inputValidate.threadShowRPMCounting != null)
                inputValidate.threadShowRPMCounting.Abort();
            if (inputValidate.threadCalcVmaxRange != null)
                inputValidate.threadCalcVmaxRange.Abort();
            if (inputValidate.threadCalcAccelSpeedRange != null)
                inputValidate.threadCalcAccelSpeedRange.Abort();
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 全數值驗證
            if (!formMain.page2.inputValidate.VerifyAllInputValidate(true))
                return;

            // 最後更新使用條件
            if (formMain.chkCalcAllMode.Checked)
                searchAllMode.UpdateCondition(null, null);
            else
                runCondition.UpdateCondition(null, null);

            threadCalc = new Thread(() => {
                Thread.Sleep(100);

                // 開始計算
                var calcCon = formMain.chkCalcAllMode.Checked ? searchAllMode.curCondition : runCondition.curCondition;    // 計算條件
                var result = calc.GetRecommandResult(calcCon);
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
                        //formMain.dgvRecommandList.CurrentCell = null;
                        //formMain.dgvRecommandList.DataSource = null;
                        //formMain.dgvRecommandList.ClearSelection();
                        //formMain.dgvRecommandList.Rows[0].Selected = false;
                        if (formMain.dgvRecommandList.Rows.Count > 0)
                            formMain.dgvRecommandList.Rows.RemoveAt(0);
                        formMain.dgvRecommandList.Rows.Clear();
                        
                        chartInfo.Clear();
                        recommandList.curSelectModel = (null, -1);
                        // 側邊欄
                        if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
                            formMain.sideTable.ClearModelImg();
                            formMain.sideTable.ClearModelInfo();
                        }
                        //formMain.sideTable.ClearSelectedModelInfo();
                    }));

                    formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg("此使用條件無法計算，請嘗試調整使用條件。", SideTable.MsgStatus.Alarm)));

                    // 訊息顯示
                    if (!string.IsNullOrEmpty(result["Msg"] as string)) {
                        // 訊息斷行顯示
                        string alarmMsg = result["Msg"] as string;
                        formMain.Invoke(new Action(() => formMain.sideTable.UpdateMsg(alarmMsg, SideTable.MsgStatus.Alarm)));
                    }

                    //// 有效行程顯示
                    //formMain.page2.effectiveStroke.IsShowEffectiveStroke(false);

                    // 搜尋不到項目時，下一步隱藏
                    //formMain.Invoke(new Action(() => formMain.cmdConfirmStep2.Visible = false));
                    formMain.Invoke(new Action(() => formMain.page2.ChangeNextStepBtnVisible(false)));

                    // 搜尋不到項目時，進階選項空值
                    formMain.Invoke(new Action(() => {
                        formMain.txtMaxSpeed.Text = "";
                        formMain.txtAccelSpeed.Text = "";
                    }));

                    return;
                }
                // 清空訊息
                formMain.Invoke(new Action(formMain.sideTable.ClearMsg));

                // 表單顯示
                recommandList.DisplayRecommandList();

                //// 有效行程顯示、賦值
                //formMain.Invoke(new Action(() => effectiveStroke.IsShowEffectiveStroke(true)));
            });
            threadCalc.Start();

            // Loading顯示
            ShowWaiting();
        }

        private void ShowWaiting() {
            new Thread(() => {
                formMain.Invoke(new Action(() => {
                    FormWaiting wait = new FormWaiting(calc.GetCalcPercent, calc.InterruptCalc);
                    wait.GetPercent = calc.GetCalcPercent;
                    wait.InterruptCalc = calc.InterruptCalc;
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
            Model.UseEnvironment useEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.標準 : Model.UseEnvironment.無塵;
            formMain.tabModelType.SelectTab((int)useEnv);
            if (useEnv == Model.UseEnvironment.標準)
                formMain.optGTH.Checked = true;
            else
                formMain.optGCH.Checked = true;
            formMain.sideTable.UpdateMsg(calc.GetModelTypeComment(curSelectModelType), SideTable.MsgStatus.Normal);
        }

        public void ChangeNextStepBtnVisible(bool visible) {
            if (formMain.cmdConfirmStep2.Visible == visible)
                return;

            formMain.panelConfirmBtnsStep2.ColumnStyles.Clear();
            formMain.cmdConfirmStep2.Visible = visible;

            ColumnStyle[] rows = { };
            if (visible) {
                rows = new ColumnStyle[]{
                    new ColumnStyle(SizeType.Percent, 33.33f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 33.33f),
                };
            } else {
                rows = new ColumnStyle[]{
                    new ColumnStyle(SizeType.Percent, 41.67f),
                    new ColumnStyle(SizeType.Percent, 16.67f),
                    new ColumnStyle(SizeType.Percent, 41.67f),
                };
            }

            foreach (ColumnStyle style in rows)
                formMain.panelConfirmBtnsStep2.ColumnStyles.Add(style);
        }
    }
}

