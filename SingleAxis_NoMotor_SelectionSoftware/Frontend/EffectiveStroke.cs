using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class EffectiveStroke {
        // 確定有效行程
        public decimal effectiveStroke;

        private FormMain formMain;

        public EffectiveStroke(FormMain formMain) {
            this.formMain = formMain;
        }

        public decimal GetEffectiveStroke() {
            // 依照型號導程搜尋所有行程
            IEnumerable<(decimal stroke, decimal rpm)> strokeList;
            if (formMain.page2.recommandList.curSelectModel.model.IsContainsReducerRatioType())
                strokeList = formMain.page2.calc.strokeRpm.Rows.Cast<DataRow>()
                                                                   .Where(row => formMain.page2.recommandList.curSelectModel.model.StartsWith(row["型號"].ToString()))
                                                                   .Select(row => (stroke: Convert.ToDecimal(row["行程"].ToString()), rpm: Convert.ToDecimal(row["轉速"].ToString())));
            else
                strokeList = formMain.page2.calc.strokeRpm.Rows.Cast<DataRow>()
                                                                   .Where(row => formMain.page2.recommandList.curSelectModel.model.Equals(row["型號"].ToString()))
                                                                   .Select(row => (stroke: Convert.ToDecimal(row["行程"].ToString()), rpm: Convert.ToDecimal(row["轉速"].ToString())));
            decimal runStroke = Convert.ToDecimal(formMain.txtStroke.Text);
            effectiveStroke = strokeList.First(item => item.stroke >= runStroke).stroke;
            return effectiveStroke;
        }
    }
}
