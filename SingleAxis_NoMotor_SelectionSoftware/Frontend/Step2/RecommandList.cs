using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class RecommandList {
        public List<Model> curRecommandList;        
        // 打勾選取型號
        public (string model, double lead) curSelectModel;
        public (string model, double lead) curCheckedModel;

        private FormMain formMain;
        private const decimal serviceLifeDistanceAlarmStandard = 3000;  // 運行距離標準(km)
        private const decimal serviceLifeTimeAlarmStandard = 3;         // 運行壽命標準(年)
        // 顏色區分
        private Dictionary<string, Func<Model, bool>> redFontConditions = new Dictionary<string, Func<Model, bool>>() {
            { "T_max安全係數", model => model.tMaxSafeCoefficient >= Model.tMaxStandard },
            { "運行距離", model => model.serviceLifeDistance >= serviceLifeDistanceAlarmStandard },
            { "運行壽命", model => model.serviceLifeTime.year >= serviceLifeTimeAlarmStandard },
            { "皮帶馬達安全係數", model => model.beltMotorSafeCoefficient == -1 || model.beltMotorSafeCoefficient < Model.beltMotorStandard },
            { "皮帶T_max安全係數", model => model.beltSafeCoefficient == -1 || model.beltSafeCoefficient >= Model.tMaxStandard_beltMotor },
            { "力矩警示", model => model.isMomentVerifySuccess },
        };

        public RecommandList(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        private void InitEvents() {
            formMain.dgvRecommandList.SelectionChanged += DgvRecommandList_SelectionChanged;
            formMain.dgvRecommandList.CellClick += DgvRecommandList_CellClick;
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
                formMain.step2.runCondition.UpdateCondition(sender, e);
            }
        }

        private void DgvRecommandList_SelectionChanged(object sender, EventArgs e) {
            // 畫圖
            formMain.step2.chartInfo.PaintGraph();

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
                formMain.sideTable.UpdateModelInfo();
            }

            // 驗證使用瓦數
            if (curRow != null && curRow.Cells["馬達瓦數"].Value != null) {
                if (formMain.cboPower.Text.Contains("標準")) {
                    int usePower = Convert.ToInt32(curRow.Cells["馬達瓦數"].Value.ToString());
                    formMain.cboMotorParamsMotorPowerSelection.Text = usePower.ToString();
                }
            }

            // 驗證選擇項目異常
            VerifySelectedModelAlarm();

            // 項目更改分頁收起
            if (sender != null)
                formMain._explorerBar.ValueChanged(sender, e);

            // 有效行程選項更新
            if (curSelectModel.model != null)
                formMain.step2.effectiveStroke.CmdEffectiveStroke_Click(null, null);
        }

        public void Refresh() {
            // 型號選型結果顯示恢復隱藏
            //formMain.dgvCalcSelectedModel.Visible = false;
            formMain.dgvCalcSelectedModel.DataSource = null;
            formMain.dgvCalcSelectedModel.Rows.Clear();
        }

        public void DisplayRecommandList() {
            formMain.Invoke(new Action(() => {
                formMain.dgvRecommandList.Rows.Clear();
                // 皮帶欄位顯示修正
                formMain.dgvRecommandList.Columns["皮帶馬達安全係數"].Visible = formMain.optRepeatabilityBelt.Checked;
                formMain.dgvRecommandList.Columns["皮帶T_max安全係數"].Visible = formMain.optRepeatabilityBelt.Checked;
            }));

            formMain.Invoke(new Action(() => {
                foreach (Model model in curRecommandList) {
                    try {
                        int index = formMain.dgvRecommandList.Rows.Add();
                        formMain.dgvRecommandList.Rows[index].Height = 35;
                        formMain.dgvRecommandList.Rows[index].Cells["鎖定"].Value = false;
                        formMain.dgvRecommandList.Rows[index].Cells["項次"].Value = model.name;
                        formMain.dgvRecommandList.Rows[index].Cells["導程"].Value = model.lead;
                        formMain.dgvRecommandList.Rows[index].Cells["荷重"].Value = model.load;
                        formMain.dgvRecommandList.Rows[index].Cells["最高轉速"].Value = model.rpm;
                        formMain.dgvRecommandList.Rows[index].Cells["運行速度"].Value = model.vMax;
                        formMain.dgvRecommandList.Rows[index].Cells["加速度"].Value = model.accelSpeed;
                        formMain.dgvRecommandList.Rows[index].Cells["最大行程"].Value = model.maxStroke;
                        formMain.dgvRecommandList.Rows[index].Cells["運行時間"].Value = model.moveTime;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩A"].Value = model.moment_A;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩B"].Value = model.moment_B;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩C"].Value = model.moment_C;
                        formMain.dgvRecommandList.Rows[index].Cells["力矩警示"].Value = model.isMomentVerifySuccess ? "Pass" : "Fail";
                        formMain.dgvRecommandList.Rows[index].Cells["馬達瓦數"].Value = model.usePower;
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶馬達安全係數"].Value = model.beltMotorSafeCoefficient == -1 ? "無" : model.beltMotorSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["T_max安全係數"].Value = model.tMaxSafeCoefficient;
                        formMain.dgvRecommandList.Rows[index].Cells["皮帶T_max安全係數"].Value = model.beltSafeCoefficient == -1 ? "無" : model.beltSafeCoefficient.ToString();
                        formMain.dgvRecommandList.Rows[index].Cells["是否推薦"].Value = Properties.Resources.inCondition;
                        formMain.dgvRecommandList.Rows[index].Cells["更詳細資訊"].Value = Properties.Resources.detail_disable_in_condition;

                        // 運行距離
                        if (model.slideTrackServiceLifeDistance < 0)
                            formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "Error";
                        else {
                            if (model.serviceLifeDistance > 20000)
                                formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = "2萬公里以上";
                            else
                                formMain.dgvRecommandList.Rows[index].Cells["運行距離"].Value = ((float)model.serviceLifeDistance / 10000f).ToString("#0.0") + "萬公里";
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
                        formMain.dgvRecommandList.Rows[index].Cells["運行壽命"].Value = useTime;

                        // 顏色區分
                        foreach (var con in redFontConditions) {
                            formMain.dgvRecommandList.Rows[index].Cells[con.Key].Style.ForeColor = con.Value(model) ? Color.Black : Color.Red;
                            formMain.dgvRecommandList.Rows[index].Cells[con.Key].Style.SelectionForeColor = con.Value(model) ? Color.Black : Color.Red;
                        }
                    } catch (Exception ex) {
                        break;
                    }
                }
            }));

            try {
                formMain.Invoke(new Action(() => {
                    if (formMain.optCalcAllModel.Checked) {
                        // 欄位寬度更新
                        foreach (DataGridViewColumn col in formMain.dgvRecommandList.Columns)
                            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;

                        // 畫圖
                        formMain.step2.chartInfo.PaintGraph();
                    } else {
                        // 驗證Vmax
                        Model curModel = curRecommandList.First();
                        if (formMain.optMaxSpeedType_mms.Checked) {
                            formMain.txtMaxSpeed.Text = curModel.vMax.ToString();
                        } else if (formMain.optMaxSpeedType_rpm.Checked) {
                            int curRpm = formMain.step2.calc.MMS_TO_RPM(curModel.vMax, curModel.lead);
                            formMain.txtMaxSpeed.Text = curRpm.ToString();
                        }

                        // 細項顯示
                        DisplaySelectedModel();
                    }

                    // 選第一列
                    if (curSelectModel.model != null) {
                        if (formMain.dgvReducerInfo.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["columnModel"].Value.Equals(curSelectModel.model))) {
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
        }

        public void DisplaySelectedModel() {
            Model curModel = curRecommandList.First();
            // 使用壽命時間
            string useTime = "";
            if (curModel.serviceLifeTime.year >= 10) {
                useTime = "10年以上";
            } else {
                if (curModel.serviceLifeTime.year > 0)
                    useTime += curModel.serviceLifeTime.year + "年";
                if (curModel.serviceLifeTime.month > 0)
                    useTime += curModel.serviceLifeTime.month + "個月";
            }
            // 使用壽命距離
            string useDistance = "";
            if (curModel.slideTrackServiceLifeDistance < 0)
                useDistance = "Error";
            else {
                if (curModel.serviceLifeDistance > 20000)
                    useDistance = "2萬公里以上";
                else
                    useDistance = ((float)curModel.serviceLifeDistance / 10000f).ToString("#0.0") + "萬公里";
            }
            Dictionary<string, string> item = new Dictionary<string, string>();
            if (formMain.optRepeatabilityBelt.Checked && curModel.beltMotorSafeCoefficient != -1 && curModel.beltSafeCoefficient != -1)
                item = new Dictionary<string, string>() {
                    { "項次", curModel.name.ToString() + "-L" + curModel.lead.ToString() },
                    { "T_max安全係數", curModel.tMaxSafeCoefficient.ToString() },
                    { "皮帶馬達安全係數", curModel.beltMotorSafeCoefficient == -1 ? "null" : curModel.beltMotorSafeCoefficient.ToString() },
                    { "皮帶T_max安全係數", curModel.beltSafeCoefficient == -1 ? "null" : curModel.beltSafeCoefficient.ToString() },
                    { "力矩警示", curModel.isMomentVerifySuccess ? "Pass" : "Fail" },
                    { "運行距離", useDistance },
                    { "運行壽命", useTime },
                };
            else
                item = new Dictionary<string, string>() {
                    { "項次", curModel.name.ToString() + "-L" + curModel.lead.ToString() },
                    { "T_max安全係數", curModel.tMaxSafeCoefficient.ToString() },
                    { "力矩警示", curModel.isMomentVerifySuccess ? "Pass" : "Fail" },
                    { "運行距離", useDistance },
                    { "運行壽命", useTime },
                };
            DataTable dt = new DataTable();
            dt.Columns.Add("Item");
            dt.Columns.Add("Value");
            foreach (var i in item) {
                DataRow dr = dt.NewRow();
                dr["Item"] = i.Key;
                dr["Value"] = i.Value;
                dt.Rows.Add(dr);
            }
            formMain.dgvCalcSelectedModel.DataSource = dt;

            // 顏色區分壽命規範
            int serviceYear = -1;
            long serviceDistance = -1;
            foreach (DataGridViewRow row in formMain.dgvCalcSelectedModel.Rows) {
                if (row.Cells["Item"].Value.ToString() == "運行距離") {
                    serviceDistance = curModel.serviceLifeDistance;
                    row.Cells["Value"].Style.ForeColor = serviceDistance >= serviceLifeDistanceAlarmStandard ? Color.Black : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "運行壽命") {
                    int.TryParse(row.Cells["Value"].Value.ToString().Split('年')[0], out serviceYear);
                    row.Cells["Value"].Style.ForeColor = serviceYear >= serviceLifeTimeAlarmStandard ? Color.Black : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "扭矩確認") {
                    if (formMain.optRepeatabilityBelt.Checked)
                        row.Cells["Value"].Style.ForeColor = curModel.is_tMax_OK && curModel.isMotorOK && curModel.is_belt_tMax_OK ? Color.Green : Color.Red;
                    else
                        row.Cells["Value"].Style.ForeColor = curModel.is_tMax_OK ? Color.Green : Color.Red;
                }
                if (row.Cells["Item"].Value.ToString() == "T_max安全係數")
                    row.Cells["Value"].Style.ForeColor = curModel.tMaxSafeCoefficient >= Model.tMaxStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "T_Rms安全係數")
                    row.Cells["Value"].Style.ForeColor = curModel.tRmsSafeCoefficient > Model.tRmsStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "皮帶馬達安全係數" && row.Cells["Value"].Value.ToString() != "null")
                    row.Cells["Value"].Style.ForeColor = curModel.beltMotorSafeCoefficient < Model.beltMotorStandard ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "皮帶T_max安全係數" && row.Cells["Value"].Value.ToString() != "null")
                    row.Cells["Value"].Style.ForeColor = curModel.beltSafeCoefficient >= Model.tMaxStandard_beltMotor ? Color.Black : Color.Red;
                if (row.Cells["Item"].Value.ToString() == "力矩警示") {
                    row.Cells["Value"].Style.ForeColor = curModel.isMomentVerifySuccess ? Color.Black : Color.Red;
                    if (!curModel.isMomentVerifySuccess)
                        formMain.sideTable.UpdateMsg("力矩判定異常，請洽詢Toyo業務人員", SideTable.MsgStatus.Alarm);
                }
            }

            foreach (DataGridViewColumn col in formMain.dgvCalcSelectedModel.Columns) {
                // 欄位寬度更新
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // 型號選行取消選取
            formMain.dgvCalcSelectedModel.CurrentCell = null;

            // 更新側邊欄數值
            formMain.sideTable.UpdateSelectedConditionValue("T_max係數", curModel.tMaxSafeCoefficient.ToString(), curModel.tMaxSafeCoefficient < Model.tMaxStandard);
            formMain.sideTable.UpdateSelectedConditionValue("力矩警示", curModel.isMomentVerifySuccess ? "Pass" : "Fail", !curModel.isMomentVerifySuccess);
            formMain.sideTable.UpdateSelectedConditionValue("運行距離", useDistance, serviceDistance < serviceLifeDistanceAlarmStandard);
            formMain.sideTable.UpdateSelectedConditionValue("運行壽命", useTime, serviceYear < serviceLifeTimeAlarmStandard);

            // 畫圖
            formMain.step2.chartInfo.PaintGraph();

            // 型號選行下一步顯示
            bool isSuccess = curModel.isMomentVerifySuccess && curModel.tMaxSafeCoefficient >= Model.tMaxStandard && serviceYear >= 3 && serviceDistance >= 3000;
            if (formMain.optRepeatabilityBelt.Checked && curModel.beltMotorSafeCoefficient != -1 && curModel.beltSafeCoefficient != -1)
                isSuccess = isSuccess && curModel.beltMotorSafeCoefficient < Model.beltMotorStandard && curModel.beltSafeCoefficient >= Model.tMaxStandard_beltMotor;
            formMain.step2.SetSelectedModelConfirmBtnVisible(isSuccess);
        }

        public void VerifySelectedModelAlarm() {
            Dictionary<string, string> alarmMsg = new Dictionary<string, string>() {
                { "力矩警示", "力矩判定異常，請洽詢Toyo業務人員" },
                { "T_max安全係數", "扭矩安全係數過低，請調整荷重或馬達參數" },
                { "運行距離", "運行距離過短" },
                { "運行壽命", "運行壽命過短" },
            };

            var curRow = formMain.dgvRecommandList.CurrentRow;
            foreach (DataGridViewCell cell in curRow.Cells) {
                if (cell.Style.ForeColor == Color.Red) {
                    formMain.sideTable.UpdateMsg(alarmMsg[formMain.dgvRecommandList.Columns[cell.ColumnIndex].Name], SideTable.MsgStatus.Alarm);
                    return;
                }
            }
            formMain.sideTable.ClearMsg();
        }
    }
}
