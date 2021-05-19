using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step3 {
        private FormMain formMain;
        private int effectiveStrokeTmpIndex = 0;    // 有效行程打勾用

        public Step3(FormMain formMain) {
            this.formMain = formMain;

            InitEvents();
        }

        private void InitEvents() {
            // 有效行程確認
            formMain.optEffectiveStroke1.Click += OptEffectiveStroke_CheckedChanged;
            formMain.optEffectiveStroke2.Click += OptEffectiveStroke_CheckedChanged;
            formMain.cmdEffectiveStroke.Click += CmdEffectiveStroke_Click;
            formMain.txtEffectiveStroke.KeyDown += TxtEffectiveStroke_KeyDown;
            formMain.txtEffectiveStroke.Leave += CmdEffectiveStroke_Click;

            // 確認按鈕
            formMain.cmdConfirmStep3.Click += CmdConfirmStep3_Click;
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
            } else if (effectiveStrokeTmpIndex % 2 == 0) {
                formMain.optEffectiveStroke1.Checked = true;
                formMain.optEffectiveStroke2.Checked = false;
            }
        }

        public void Load() {
            // 有效行程賦值
            formMain.txtEffectiveStroke.Text = formMain.txtStroke.Text;

            // 計算有效行程
            CmdEffectiveStroke_Click(null, null);
        }        

        private void CmdEffectiveStroke_Click(object sender, EventArgs e) {
            // 依照型號導程搜尋所有行程
            var strokeList = formMain.step2.calc.strokeRpm.Rows.Cast<DataRow>()
                                                               .Where(row => row["Model"].ToString() == formMain.step2.recommandList.curSelectModel.model)
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
                // 如果運轉行程大於最高RPM的最大行程，則取大一階行程
                strokeOptions = (strokeList.First(item => item.stroke >= runStroke).stroke, -1);
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
                    strokeOptions = (higherThenKeyEffectiveStroke, -1);
                } else {
                    if (lowerThenKeyEffectiveStroke == runStroke) {
                        strokeOptions = (lowerThenKeyEffectiveStroke, -1);
                    } else if (lowerThenKeyEffectiveStroke > runStroke) {
                        if (lowerThenKeyEffectiveStroke == higherThenKeyEffectiveStroke)
                            strokeOptions = (lowerThenKeyEffectiveStroke, -1);
                        else
                            strokeOptions = (lowerThenKeyEffectiveStroke, higherThenKeyEffectiveStroke);
                    }
                }
            }

            formMain.optEffectiveStroke1.Text = strokeOptions.stroke_opt1.ToString();
            formMain.optEffectiveStroke2.Text = strokeOptions.stroke_opt2.ToString();
            formMain.panelEffectiveStroke2.Visible = strokeOptions.stroke_opt2 != -1;
            formMain.optEffectiveStroke1.Checked = true;
            formMain.optEffectiveStroke2.Checked = false;
        }

        private void TxtEffectiveStroke_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter)
                return;

            CmdEffectiveStroke_Click(null, null);
        }

        private void CmdConfirmStep3_Click(object sender, EventArgs e) {
            if (formMain.curStep == FormMain.Step.Step3) {
                formMain.curStep = (FormMain.Step)((int)formMain.curStep + 1);
                formMain.sideTable.Update(null, null);
                formMain._explorerBar.UpdateCurStep(formMain.curStep);
                formMain.explorerBar.ScrollControlIntoView(formMain.panelConfirmBtnsStep4);
            }
        }
    }
}
