using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class RecommandList {
        public List<Model> curRecommandList;        
        // 打勾選取型號
        public (string model, double lead) curSelectModel;
        public (string model, double lead) curCheckedModel;

        private FormMain formMain;
        private Color colorSelectionBg = Color.FromArgb(208, 252, 248);
        // 顏色區分
        private Dictionary<string, Func<Model, bool>> redFontConditions = new Dictionary<string, Func<Model, bool>>();        
        // 錯誤訊息
        private Dictionary<string, string> alarmMsg = new Dictionary<string, string>() {
            { "T_max安全係數", "扭矩安全係數過低" },
            { "皮帶馬達安全係數", "皮帶馬達安全係數過低" },
            { "皮帶T_max安全係數", "皮帶扭矩安全係數過低" },
            { "力矩警示", "力矩警示異常" },
            { "荷重", "超過最大荷重" },
            { "最大行程", "超過最大行程" },
            { "運行壽命", "每分鐘趟數過大" },
            { "運行壽命2", "未達希望壽命" },
            { "運行時間", "運行時間過短" },
            { "運行速度", "運行速度過高自動調整" },
            { "加速度", "加速時間過短自動調整" },
            { "最高轉速", "轉速過高自動調整" },
        };
        private string[] yellowBgConditions = new string[] {    // 黃底欄位(提示但可以下一步)
            "運行速度",
            "加速度",
            "最高轉速",
        };

        public RecommandList(FormMain formMain) {
            this.formMain = formMain;

            redFontConditions = new Dictionary<string, Func<Model, bool>>() {
                { "T_max安全係數", model => model.tMaxSafeCoefficient >= (model.isUseBaltCalc ? model.tMaxStandard_beltMotor : model.tMaxStandard) },
                { "皮帶馬達安全係數", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < model.beltMotorStandard },
                { "皮帶T_max安全係數", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= model.tMaxStandard_belt },
                { "力矩警示", model => model.isMomentVerifySuccess },
                { "荷重", model => model.maxLoad == -1 || (model.maxLoad != -1 && model.maxLoad >= model.load) },
                { "最大行程", model => model.maxStroke >= Convert.ToInt32(formMain.txtStroke.Text) },
                { "運行壽命", model => model.serviceLifeTime != (-1, -1, -1) },
                { "運行壽命2", model => model.serviceLifeTime != (-1, -1, -1) && model.serviceLifeTime.year >= formMain.page2.runCondition.curCondition.expectServiceLifeTime },
                { "運行時間", model => model.moveTime <= Convert.ToDouble(formMain.txtRunTime.Text) },

                // 黃底特例
                { "運行速度", model => formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection || !formMain.chkAdvanceMode.Checked || (
                    (formMain.cboMaxSpeedUnit.Text == "mm/s" && model.vMax == Convert.ToDouble(formMain.txtMaxSpeed.Text)) ||
                    (formMain.cboMaxSpeedUnit.Text == "RPM" && model.rpm == Convert.ToDouble(formMain.txtMaxSpeed.Text))
                ) },
                { "加速度", model => formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection || !formMain.chkAdvanceMode.Checked || model.accelSpeed == Convert.ToDouble(formMain.txtAccelSpeed.Text) },
                { "最高轉速", model => formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ModelSelection || !formMain.chkAdvanceMode.Checked || (
                    (formMain.cboMaxSpeedUnit.Text == "mm/s" && model.vMax == Convert.ToDouble(formMain.txtMaxSpeed.Text)) ||
                    (formMain.cboMaxSpeedUnit.Text == "RPM" && model.rpm == Convert.ToDouble(formMain.txtMaxSpeed.Text))
                ) },
            };
            InitEvents();
        }

        private void InitEvents() {
            formMain.dgvRecommandList.SelectionChanged += DgvRecommandList_SelectionChanged;
            formMain.dgvRecommandList.CellClick += DgvRecommandList_CellClick;
            // 表格列點兩下開啟Log
            formMain.dgvRecommandList.CellDoubleClick += (sender, e) => Process.Start(new FileInfo(Config.LOG_PARAM_FILENAME).FullName);
        }

        private void DgvRecommandList_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex >= 0) {
                // 取消所有打勾，除了當前選的
                foreach (DataGridViewRow row in formMain.dgvRecommandList.Rows)
                    if (formMain.dgvRecommandList.Rows.IndexOf(row) != e.RowIndex)
                        (row.Cells["鎖定"] as DataGridViewCheckBoxCell).Value = false;

                DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell)formMain.dgvRecommandList.Rows[e.RowIndex].Cells["鎖定"];

                // 打勾/取消打勾
                if (e.ColumnIndex == 0)                    
                    cell.Value = !(bool)cell.Value;

                // 先前有打勾，選其他的打勾
                if (e.ColumnIndex > 0)
                    if (curCheckedModel.model != null)
                        cell.Value = true;

                // 有打勾則記錄當前打勾項目，沒有責清除選取項目
                if ((bool)cell.Value)
                    curCheckedModel = (formMain.dgvRecommandList.Rows[e.RowIndex].Cells["項次"].Value.ToString(), Convert.ToDouble(formMain.dgvRecommandList.Rows[e.RowIndex].Cells["導程"].Value.ToString()));                    
                else
                    curCheckedModel = (null, -1);

                // 更新運轉條件
                formMain.page2.runCondition.UpdateCondition(sender, e);
            }
        }

        private void DgvRecommandList_SelectionChanged(object sender, EventArgs e) {
            if (formMain.dgvRecommandList.CurrentRow == null)
                return;

            //// 畫圖
            //formMain.page2.chartInfo.PaintGraph();

            // 記錄當前選取項目
            var curRow = formMain.dgvRecommandList.CurrentRow;
            if (curRow != null && curRow.Cells["項次"].Value != null && curRow.Cells["導程"].Value != null)
                curSelectModel = (curRow.Cells["項次"].Value.ToString(), Convert.ToDouble(curRow.Cells["導程"].Value.ToString()));

            // 更新型號圖片            
            if (curRow != null && curRow.Cells["項次"].Value != null) {
                string curModel = formMain.dgvRecommandList.CurrentRow.Cells["項次"].Value.ToString();
                formMain.sideTable.UpdateModelImg(curModel);
            }

            // 更新目前條件型號
            if (curRow != null && curRow.Cells["項次"].Value != null && curRow.Cells["導程"].Value != null) {
                string curModel = formMain.dgvRecommandList.CurrentRow.Cells["項次"].Value.ToString();
                double lead = Convert.ToDouble(formMain.dgvRecommandList.CurrentRow.Cells["導程"].Value.ToString());
                if (curModel.IsContainsReducerRatioType())
                    formMain.sideTable.UpdateModeInfo(curModel, Convert.ToInt32(curModel.Split('-')[1]));
                else
                    formMain.sideTable.UpdateModeInfo(curModel, lead);
                // 細項顯示
                DisplaySelectedModel(curModel, lead);
            }            

            // 驗證使用瓦數
            if (curRow != null && curRow.Cells["馬達瓦數"].Value != null) {
                if (formMain.cboPower.Text.Contains("標準")) {
                    //int usePower = Convert.ToInt32(curRow.Cells["馬達瓦數"].Value.ToString());
                    //formMain.cboMotorParamsMotorPowerSelection.Text = usePower.ToString();
                    if (int.TryParse(curRow.Cells["馬達瓦數"].Value.ToString(), out int usePower))
                        formMain.cboMotorParamsMotorPowerSelection.Text = usePower.ToString();                    
                }
            }

            // 驗證選擇項目異常
            VerifySelectedModelAlarm();

            // 全部選型驗證選擇型號是否有項目Fail，有則不顯示下一步
            if (curSelectModel.model != null) {
                try {
                    bool isAllConditionSuccess = redFontConditions.All(con => con.Value(curRecommandList.First(model => model.name == curSelectModel.model && model.lead == curSelectModel.lead)) || yellowBgConditions.Contains(con.Key));
                    formMain.page2.ChangeNextStepBtnVisible(isAllConditionSuccess);
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            }

            // Log所有參數
            if (curSelectModel.model != null) {
                try {
                    if (!Directory.Exists(Config.LOG_FILENAME))
                        Directory.CreateDirectory(Config.LOG_FILENAME);
                    FileUtil.LogModelInfo(curRecommandList.First(model => model.name == curSelectModel.model && model.lead == curSelectModel.lead), formMain.page2.runCondition.curCondition.setupMethod, true);
                } catch (Exception ex) {
                    Console.WriteLine("log error: " + ex);
                }
            }
        }

        public void Refresh() {
            // 型號選型結果顯示恢復隱藏
            curSelectModel = (null, -1);
            curCheckedModel = (null, -1);
            formMain.dgvRecommandList.Rows.Clear();
        }

        public void DisplayRecommandList() {
            formMain.Invoke(new Action(() => {
                formMain.dgvRecommandList.Rows.Clear();
                //// 皮帶欄位顯示修正
                //formMain.dgvRecommandList.Columns["皮帶馬達安全係數"].Visible = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType();
                //formMain.dgvRecommandList.Columns["皮帶T_max安全係數"].Visible = formMain.page2.modelTypeOptMap.First(pair => pair.Key.Checked).Value.IsBeltType();
            }));

            formMain.Invoke(new Action(() => {
                foreach (Model model in curRecommandList) {
                    try {
                        int index = formMain.dgvRecommandList.Rows.Add();
                        formMain.dgvRecommandList.Rows[index].Height = 35;
                        formMain.dgvRecommandList.Rows[index].Cells["鎖定"].Value = false;
                        formMain.dgvRecommandList.Rows[index].Cells["項次"].Value = model.name;
                        formMain.dgvRecommandList.Rows[index].Cells["重複定位精度"].Value = "±" + model.repeatability;
                        formMain.dgvRecommandList.Rows[index].Cells["導程"].Value = model.lead;
                        formMain.dgvRecommandList.Rows[index].Cells["荷重"].Value = model.load;
                        //formMain.dgvRecommandList.Rows[index].Cells["最高轉速"].Value = model.showRpm;
                        formMain.dgvRecommandList.Rows[index].Cells["最高轉速"].Value = model.rpm;
                        //formMain.dgvRecommandList.Rows[index].Cells["運行速度"].Value = Convert.ToDouble(model.vMax.ToString("#0.000"));
                        formMain.dgvRecommandList.Rows[index].Cells["運行速度"].Value = Convert.ToInt32(model.vMax.ToString("#0"));
                        formMain.dgvRecommandList.Rows[index].Cells["加速度"].Value = model.accelSpeed;
                        formMain.dgvRecommandList.Rows[index].Cells["最大行程"].Value = model.maxStroke;
                        formMain.dgvRecommandList.Rows[index].Cells["運行時間"].Value = model.moveTime;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩A"].Value = model.moment_A;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩B"].Value = model.moment_B;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩C"].Value = model.moment_C;
                        // 推桿式不顯示力矩
                        if (model.modelType.IsRodType()) {
                            formMain.dgvRecommandList.Rows[index].Cells["力矩A"].Value = "-";
                            formMain.dgvRecommandList.Rows[index].Cells["力矩B"].Value = "-";
                            formMain.dgvRecommandList.Rows[index].Cells["力矩C"].Value = "-";
                        }
                        // 垂直不顯示力矩B
                        if (formMain.page2.runCondition.curCondition.setupMethod == Model.SetupMethod.垂直)
                            formMain.dgvRecommandList.Rows[index].Cells["力矩B"].Value = "-";
                        formMain.dgvRecommandList.Rows[index].Cells["力矩警示"].Value = model.isMomentVerifySuccess ? "Pass" : "Fail";
                        formMain.dgvRecommandList.Rows[index].Cells["馬達瓦數"].Value = model.usePower == 0 ? "-" : model.usePower.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶馬達安全係數"].Value = model.beltMotorSafeCoefficient == -1 ? "無" : model.beltMotorSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["T_max安全係數"].Value = model.tMaxSafeCoefficient;
                        formMain.dgvRecommandList.Rows[index].Cells["T_Rms安全係數"].Value = model.tRmsSafeCoefficient == -1 ? "無" : model.tRmsSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶T_max安全係數"].Value = model.beltSafeCoefficient == -1 ? "無" : model.beltSafeCoefficient.ToString();
                        //formMain.dgvRecommandList.Rows[index].Cells["是否推薦"].Value = redFontConditions.Any(con => con.Value(model) == false) ? Properties.Resources.notInCondition : Properties.Resources.inCondition;
                        formMain.dgvRecommandList.Rows[index].Cells["更詳細資訊"].Value = Properties.Resources.detail_disable_in_condition;

                        // 運行距離
                        if (model.serviceLifeDistance < 0)
                            formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "Error";
                        else {
                            if (model.serviceLifeDistance > 10000) {
                                if (formMain.chkCalcAllMode.Checked && formMain.chkIsCalcMaxLoad.Checked)
                                    formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = model.serviceLifeDistance + "km";
                                else
                                    formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "10000km↑";
                            } else
                                formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = model.serviceLifeDistance + "km";
                        }

                        // 使用壽命時間
                        string useTime = "";
                        if (model.serviceLifeTime == (-1, -1, -1))
                            useTime = "-";
                        else {
                            if (model.serviceLifeTime.year >= 10)
                                useTime = "10年↑";
                            else {
                                if (model.serviceLifeTime.year > 0)
                                    useTime += model.serviceLifeTime.year + "年";
                                else
                                    useTime = "1年↓";
                            }
                        }
                        formMain.dgvRecommandList.Rows[index].Cells["運行壽命"].Value = useTime;

                        // 顏色區分
                        foreach (var con in redFontConditions) {
                            int colIndex = formMain.dgvRecommandList.Columns.Cast<DataGridViewColumn>().First(col => con.Key.StartsWith(col.Name)).Index;
                            if (yellowBgConditions.Contains(con.Key)) {
                                formMain.dgvRecommandList.Rows[index].Cells[colIndex].Style.BackColor = con.Value(model) ? Color.White : Color.Yellow;
                                formMain.dgvRecommandList.Rows[index].Cells[colIndex].Style.SelectionBackColor = con.Value(model) ? colorSelectionBg : Color.Yellow;
                                formMain.dgvRecommandList.Rows[index].Cells[colIndex].ToolTipText = con.Value(model) ? "" : alarmMsg[con.Key];
                            } else {
                                // 該項紅字
                                formMain.dgvRecommandList.Rows[index].Cells[colIndex].Style.ForeColor = con.Value(model) ? Color.Black : Color.Red;
                                formMain.dgvRecommandList.Rows[index].Cells[colIndex].Style.SelectionForeColor = con.Value(model) ? Color.Black : Color.Red;                                
                            }                            
                        }
                        // 項次紅字
                        bool hasErrorItem = redFontConditions.Any(con => !yellowBgConditions.Contains(con.Key) &&!con.Value(model));
                        formMain.dgvRecommandList.Rows[index].Cells["項次"].Style.ForeColor = hasErrorItem ? Color.Red : Color.Black;
                        formMain.dgvRecommandList.Rows[index].Cells["項次"].Style.SelectionForeColor = hasErrorItem ? Color.Red : Color.Black;
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                        break;
                    }
                }
            }));

            try {
                formMain.Invoke(new Action(() => {
                    if (formMain.page1.modelSelectionMode == Page1.ModelSelectionMode.ConditionSelection) {
                        // 欄位寬度更新
                        foreach (DataGridViewColumn col in formMain.dgvRecommandList.Columns)
                            if (col.Name == "項次" || col.Name == "導程")
                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

                        //// 畫圖
                        //formMain.page2.chartInfo.PaintGraph();
                    } else {
                        // 回填進階選項
                        if (formMain.chkAdvanceMode.Checked) {
                            Model curModel = curRecommandList.First();
                            if (formMain.cboMaxSpeedUnit.Text == "mm/s") {
                                //formMain.txtMaxSpeed.Text = Convert.ToDouble(curModel.vMax.ToString("#0.000")).ToString();
                                formMain.txtMaxSpeed.Text = Convert.ToDouble((int)curModel.vMax).ToString();
                            } else if (formMain.cboMaxSpeedUnit.Text == "RPM") {
                                //int curRpm = formMain.page2.calc.MMS_TO_RPM(curModel.vMax, curModel.lead);
                                int curRpm = curModel.rpm;
                                formMain.txtMaxSpeed.Text = curRpm.ToString();
                            }
                            formMain.txtAccelSpeed.Text = curModel.accelSpeed.ToString();
                        }
                    }
                    //// 細項顯示
                    //DisplaySelectedModel();

                    // 選第一列
                    if (curSelectModel.model != null) {
                        //if (formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["columnModel"].Value.Equals(curSelectModel.model))) {
                        if (formMain.page2.calc.IsContainsReducerRatio(curSelectModel.model)) {
                            try {
                                formMain.dgvRecommandList.CurrentCell = formMain.dgvRecommandList[0, formMain.dgvRecommandList.Rows.Cast<DataGridViewRow>()
                                                                                        .Where(row => row.Cells["項次"].Value.ToString() == curSelectModel.model)
                                                                                        .Select(row => formMain.dgvRecommandList.Rows.IndexOf(row))
                                                                                        .First()
                                                                                    ];
                            } catch (Exception ex) {
                                Console.WriteLine(ex);
                            }
                        } else {
                            // 找不到項目就直接選第一列
                            try {
                                formMain.dgvRecommandList.CurrentCell = formMain.dgvRecommandList[0, formMain.dgvRecommandList.Rows.Cast<DataGridViewRow>()
                                                                                        .Where(row => row.Cells["項次"].Value.ToString() == curSelectModel.model &&
                                                                                                        Convert.ToInt32(row.Cells["導程"].Value.ToString()) == curSelectModel.lead)
                                                                                        .Select(row => formMain.dgvRecommandList.Rows.IndexOf(row))
                                                                                        .First()
                                                                                    ];
                            } catch (Exception ex) {
                                Console.WriteLine(ex);
                                formMain.dgvRecommandList.CurrentCell = formMain.dgvRecommandList[0, 0];
                            }
                        }
                    } else
                        formMain.dgvRecommandList.CurrentCell = formMain.dgvRecommandList[0, 0];

                    // 若先有勾取項目，勾第一列
                    if (curCheckedModel.model != null) {
                        formMain.dgvRecommandList.Rows[0].Cells[0].Value = true;
                        curCheckedModel = (formMain.dgvRecommandList.Rows[0].Cells["項次"].Value.ToString(), Convert.ToDouble(formMain.dgvRecommandList.Rows[0].Cells["導程"].Value.ToString()));
                    }

                    // 顯示右邊型號資訊用途
                    DgvRecommandList_SelectionChanged(null, null);
                }));                
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }

            // 欄寬設定
            formMain.Invoke(new Action(() => {
                foreach (DataGridViewColumn col in formMain.dgvRecommandList.Columns)
                    if (col.Name == "荷重")
                        col.AutoSizeMode = formMain.chkIsCalcMaxLoad.Checked ? DataGridViewAutoSizeColumnMode.AllCells : DataGridViewAutoSizeColumnMode.None;
            }));
        }

        public void DisplaySelectedModel(string model, double lead) {
            var models = curRecommandList.Where(m => m.name == model && m.lead == lead);
            if (models.Count() == 0)
                return;
            Model curModel = models.First();

            // 使用壽命時間
            string useTime = "";
            if (curModel.serviceLifeTime.year >= 10) {
                useTime = "10年↑";
            } else {
                if (curModel.serviceLifeTime.year > 0)
                    useTime += curModel.serviceLifeTime.year + "年";
            }
            // 使用壽命距離
            string useDistance = "";
            if (curModel.serviceLifeDistance < 0)
                useDistance = "Error";
            else {
                if (curModel.serviceLifeDistance > 10000)
                    useDistance = "10000km↑";
                else
                    useDistance = (int)curModel.serviceLifeDistance + "km";
            }
            formMain.sideTable.UpdateSelectedConditionValue("運行距離", useDistance, false);
            formMain.sideTable.UpdateSelectedConditionValue("運行壽命", useTime, useTime == "-");

            // 畫圖
            formMain.page2.chartInfo.PaintGraph();
        }

        // 右側訊息紅字
        public void VerifySelectedModelAlarm() {
            var curRow = formMain.dgvRecommandList.CurrentRow;
            if (curRow.Cells["項次"].Value == null)
                return;
            var selectModel = curRecommandList.Where(model => model.name == curRow.Cells["項次"].Value.ToString() && model.lead == Convert.ToDouble(curRow.Cells["導程"].Value.ToString()));
            if (selectModel.Count() == 0)
                return;
            Model curModel = selectModel.First();
            //var errorMsgs = redFontConditions.Where(con => !yellowBgConditions.Contains(con.Key) && !con.Value(curModel)).Select(con => alarmMsg[con.Key]);
            //var yellowMsgs = formMain.dgvRecommandList.CurrentRow.Cells.Cast<DataGridViewCell>().Where(cell => cell.Style.BackColor == Color.Yellow).Select(cell => alarmMsg[formMain.dgvRecommandList.Columns[cell.ColumnIndex].Name]);
            //errorMsgs = errorMsgs.Concat(yellowMsgs);
            var errorMsgs = redFontConditions.Where(con => !yellowBgConditions.Contains(con.Key) && !con.Value(curModel)).Select(con => alarmMsg[con.Key]);
            Model.UseEnvironment curEnv = formMain.optStandardEnv.Checked ? Model.UseEnvironment.標準 : Model.UseEnvironment.無塵;
            if (errorMsgs.Count() == 0)
                formMain.sideTable.UpdateMsg(formMain.page2.calc.GetModelTypeComment(formMain.page2.curSelectModelType), SideTable.MsgStatus.Normal);
            else
                formMain.sideTable.UpdateMsg(string.Join("、", errorMsgs), SideTable.MsgStatus.Alarm);
        }
    }
}
