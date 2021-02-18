using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class Step2 {
        private FormMain formMain;
        private Calculation calc = new Calculation();

        public Step2(FormMain formMain) {
            this.formMain = formMain;
            InitEvents();

            //formMain.tabControlAdvanceOptions.ItemSize = new Size(0, 1);
        }

        private void InitEvents() {
            // 進階選項
            //formMain.toggleAdvanceOptions.CheckedChanged += ToggleAdvanceOptions_CheckedChanged;

            // 計算
            formMain.cmdCalc.Click += CmdCalc_Click;
        }

        private void CmdCalc_Click(object sender, EventArgs e) {
            // 條件
            Condition con = new Condition();
            // 開始計算
            var result = calc.GetRecommandResult(con);
            // 規件規格
            List<Model> recommandList = result["List"] as List<Model>;
            // 回傳訊息
            string msg = result["Msg"] as string;
            // 是否跳出Alarm
            bool isAlarm = (bool)result["Alarm"];
        }

        private void ToggleAdvanceOptions_CheckedChanged(object sender, EventArgs e) {
            //formMain.spAdvanceOptions.Panel1Collapsed = formMain.toggleAdvanceOptions.Checked;
            //formMain.spAdvanceOptions.Panel2Collapsed = !formMain.spAdvanceOptions.Panel1Collapsed;
        }
    }
}
