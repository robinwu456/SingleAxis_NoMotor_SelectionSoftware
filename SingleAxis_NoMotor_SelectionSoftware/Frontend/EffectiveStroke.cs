using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class EffectiveStroke {
        // 確定有效行程
        public decimal effectiveStroke {
            get {
                return _effectiveStroke;
            }
            set {
                if (_effectiveStroke != value) {
                    _effectiveStroke = value;
                    UpdateCondition(null, null);
                }
            }
        }
        private decimal _effectiveStroke;

        private FormMain formMain;
        private int effectiveStrokeTmpIndex = 0;    // 有效行程打勾用

        public EffectiveStroke(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();
        }

        public void IsShowEffectiveStroke(bool isShow) {
            formMain.Invoke(new Action(() => {
                if (isShow) {
                    formMain.panelEffectiveStroke.Visible = true;
                    formMain.txtEffectiveStroke.Text = formMain.txtStroke.Text;
                    CmdEffectiveStroke_Click(null, null);
                    formMain.sideTable.UpdateSelectedConditionValue("有效行程", formMain.page2.effectiveStroke.effectiveStroke.ToString() + "mm");
                } else {
                    formMain.panelEffectiveStroke.Visible = false;
                    formMain.sideTable.UpdateSelectedConditionValue("有效行程", "");
                    formMain.page2.recommandList.curSelectModel = (null, -1);
                }
            }));
        }

        private void InitEvents() {
            // 有效行程確認
            formMain.optEffectiveStroke1.Click += OptEffectiveStroke_CheckedChanged;
            formMain.optEffectiveStroke2.Click += OptEffectiveStroke_CheckedChanged;
            formMain.cmdEffectiveStroke.Click += CmdEffectiveStroke_Click;
            formMain.txtEffectiveStroke.KeyDown += TxtEffectiveStroke_KeyDown;
            formMain.txtEffectiveStroke.Leave += CmdEffectiveStroke_Click;
        }
        private void UpdateCondition(object sender, EventArgs e) {
            formMain.sideTable.UpdateModelInfo();
            formMain.sideTable.UpdateTableSelections();
        }

        private void OptEffectiveStroke_CheckedChanged(object sender, EventArgs e) {
            // 只有一個選項時不計數
            if (!formMain.panelEffectiveStroke2.Visible)
                return;

            // 判斷選擇與目前打勾一樣時，不計數
            RadioButton curOpt = (RadioButton)sender;
            if ((curOpt == formMain.optEffectiveStroke1 && effectiveStrokeTmpIndex % 2 == 0) ||
                (curOpt == formMain.optEffectiveStroke2 && effectiveStrokeTmpIndex % 2 == 1))
                return;

            effectiveStrokeTmpIndex = (effectiveStrokeTmpIndex + 1) % 2;

            if (effectiveStrokeTmpIndex % 2 == 1) {
                formMain.optEffectiveStroke1.Checked = false;
                formMain.optEffectiveStroke2.Checked = true;

                // 確定有效行程
                effectiveStroke = Convert.ToDecimal(formMain.optEffectiveStroke2.Text);
            } else if (effectiveStrokeTmpIndex % 2 == 0) {
                formMain.optEffectiveStroke1.Checked = true;
                formMain.optEffectiveStroke2.Checked = false;

                // 確定有效行程
                effectiveStroke = Convert.ToDecimal(formMain.optEffectiveStroke1.Text);
            }

            // 側邊欄顯示
            formMain.sideTable.UpdateSelectedConditionValue("有效行程", formMain.page2.effectiveStroke.effectiveStroke.ToString() + "mm");
        }
        public void CmdEffectiveStroke_Click(object sender, EventArgs e) {
            if (!formMain.page2.inputValidate.VerifyAllInputValidate())
                return;

            // 依照型號導程搜尋所有行程
            var strokeList = formMain.page2.calc.strokeRpm.Rows.Cast<DataRow>()
                                                               .Where(row => row["Model"].ToString() == formMain.page2.recommandList.curSelectModel.model)
                                                               .Select(row => (stroke: Convert.ToDecimal(row["Stroke"].ToString()), rpm: Convert.ToDecimal(row["RPM"].ToString())));
            // 最高RPM
            decimal maxRpm = strokeList.First().rpm;
            // 最高RPM的最大行程
            decimal maxRpmMaxStroke = strokeList.Last(item => item.rpm == maxRpm).stroke;
            (decimal stroke_opt1, decimal stroke_opt2) strokeOptions = (-1, -1);
            decimal runStroke = Convert.ToDecimal(formMain.txtStroke.Text);
            decimal keyEffectiveStroke = Convert.ToDecimal(formMain.txtEffectiveStroke.Text);

            // Key值不可小於Step2移動行程
            if (keyEffectiveStroke < runStroke) {
                formMain.txtEffectiveStroke.Text = runStroke.ToString();
                CmdEffectiveStroke_Click(null, null);
            }

            if (runStroke > maxRpmMaxStroke) {
                if (runStroke <= strokeList.Max().stroke)
                    // 如果運轉行程大於最高RPM的最大行程，則取大一階行程
                    strokeOptions = (strokeList.First(item => item.stroke >= runStroke).stroke, -1);
                else
                    // 取最大
                    strokeOptions = (strokeList.Max().stroke, -1);
            } else {
                // 取大一階/小一階值
                decimal lowerThenKeyEffectiveStroke;
                decimal higherThenKeyEffectiveStroke;
                try {
                    lowerThenKeyEffectiveStroke = strokeList.Last(item => item.rpm == maxRpm && item.stroke < keyEffectiveStroke).stroke;
                } catch (Exception) {
                    lowerThenKeyEffectiveStroke = strokeList.First(item => item.stroke > runStroke).stroke;
                }
                try {
                    higherThenKeyEffectiveStroke = strokeList.First(item => item.rpm == maxRpm && item.stroke >= keyEffectiveStroke && item.stroke > runStroke).stroke;
                } catch (Exception) {
                    higherThenKeyEffectiveStroke = maxRpmMaxStroke;
                }
                // 選項判斷
                if (lowerThenKeyEffectiveStroke < runStroke) {
                    // 如果小一階比運行行程小，則選項只有大一階行程
                    strokeOptions = (higherThenKeyEffectiveStroke, -1);
                } else {
                    if (lowerThenKeyEffectiveStroke == runStroke) {
                        // 小一階等於運行行程，則選項只有小一階
                        strokeOptions = (lowerThenKeyEffectiveStroke, -1);
                    } else if (lowerThenKeyEffectiveStroke > runStroke) {
                        if (lowerThenKeyEffectiveStroke == higherThenKeyEffectiveStroke)
                            // 超過最高轉速行程時，只有最高轉速行程選項
                            strokeOptions = (lowerThenKeyEffectiveStroke, -1);
                        else
                            // 正常時則顯示大/小一階
                            strokeOptions = (lowerThenKeyEffectiveStroke, higherThenKeyEffectiveStroke);
                    }
                }
            }
            // 選項顯示
            formMain.optEffectiveStroke1.Text = strokeOptions.stroke_opt1.ToString();
            formMain.optEffectiveStroke2.Text = strokeOptions.stroke_opt2.ToString();
            formMain.panelEffectiveStroke2.Visible = strokeOptions.stroke_opt2 != -1;
            formMain.optEffectiveStroke1.Checked = true;
            formMain.optEffectiveStroke2.Checked = false;
            OptEffectiveStroke_CheckedChanged(formMain.optEffectiveStroke1, null);

            // 確定有效行程
            effectiveStroke = Convert.ToDecimal(formMain.optEffectiveStroke1.Text);
            formMain.sideTable.UpdateSelectedConditionValue("有效行程", formMain.page2.effectiveStroke.effectiveStroke.ToString() + "mm");
        }

        private void TxtEffectiveStroke_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            CmdEffectiveStroke_Click(null, null);
        }
    }
}
