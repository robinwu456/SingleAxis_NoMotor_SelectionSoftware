using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ChartInfo {
        private FormMain formMain;

        public ChartInfo(FormMain formMain) {
            this.formMain = formMain;
        }

        public void Clear() {
            // 圖表清除
            formMain.chart.Series[0].Points.Clear();
            formMain.chart.Series[0].Points.AddXY(0, 0);
            formMain.chart.ChartAreas[0].AxisX.Interval = 1;
            formMain.chart.ChartAreas[0].AxisX.Maximum = 1;
            formMain.chart.ChartAreas[0].AxisY.Maximum = 2;
            // 圖資訊清除
            formMain.lbAccelTime.Text = "加/減速時間(s)：" + "0.000";
            formMain.lbConstantTime.Text = "等速時間(s)：" + "0.000";
            formMain.lbRunTime.Text = "運行時間(s)：" + "0.000";
            formMain.lbAccelSpeed.Text = "加速度(mm/s²)：" + "0.000";
            formMain.lbMaxSpeed.Text = "運行速度(mm/s)：" + "0.000";
            formMain.lbCycleTime.Text = "往返時間(s)：" + "0.000";
        }

        // 畫圖
        public void PaintGraph() {
            formMain.chart.Series[0].Points.Clear();

            DataGridViewRow curRow = formMain.dgvRecommandList.CurrentRow;
            if (curRow == null || curRow.Cells["運行速度"].Value == null || curRow.Cells["加速度"].Value == null)
                return;

            // 取座標
            Condition curConditions = new Condition();
            curConditions.stroke = Convert.ToInt32(formMain.txtStroke.Text);
            curConditions.vMax = Convert.ToDouble(curRow.Cells["運行速度"].Value.ToString());
            //curConditions.accelSpeed = Convert.ToInt32(txtAccelSpeed.Text);
            curConditions.accelSpeed = Convert.ToDouble(curRow.Cells["加速度"].Value.ToString());
            //curConditions.stopTime = Convert.ToDouble(txtStopTime.Text);
            List<PointF> points = formMain.step2.calc.GetChartPoints(curConditions);

            // 畫圖
            ChartArea chartArea = formMain.chart.ChartAreas[0];
            // X
            chartArea.AxisX.Minimum = points.Select(p => p.X).Min();
            chartArea.AxisX.Maximum = Convert.ToDouble(points.Select(p => p.X).Max().ToString("#0.000"));
            chartArea.AxisX.Interval = Convert.ToDouble((chartArea.AxisX.Maximum / 10).ToString("#0.00"));
            // Y
            chartArea.AxisY.Minimum = points.Select(p => p.Y).Min();
            chartArea.AxisY.Maximum = Convert.ToDouble(points.Select(p => p.Y).Max().ToString("#0.000"));
            //chartArea.AxisY.Interval = 100;
            // Y刻度調整
            chartArea.AxisY.CustomLabels.Clear();
            int interval_Y = 100;
            if (chartArea.AxisY.Maximum <= 1000)
                interval_Y = 100;
            else
                interval_Y = 500;
            for (int i = 0; i <= chartArea.AxisY.Maximum; i += interval_Y) {
                chartArea.AxisY.CustomLabels.Add(i, i + interval_Y * 2, (i + interval_Y).ToString());
                // 最後一個，且不是整單位
                if ((int)(chartArea.AxisY.Maximum - i) / interval_Y == 0 && (chartArea.AxisY.Maximum - i) % interval_Y != 0)
                    chartArea.AxisY.CustomLabels.Add(0, chartArea.AxisY.Maximum * 2, chartArea.AxisY.Maximum.ToString());
            }
            foreach (PointF point in points)
                formMain.chart.Series[0].Points.AddXY(Convert.ToDouble(point.X.ToString("#0.000")), Convert.ToDouble(point.Y.ToString("#0.000")));

            // 取圖資訊
            var chartInfo = formMain.step2.calc.GetChartInfo(curConditions);
            formMain.lbAccelTime.Text = "加/減速時間(s)：" + chartInfo.accelTime;
            formMain.lbConstantTime.Text = "等速時間(s)：" + chartInfo.constantTime;
            formMain.lbRunTime.Text = "運行時間(s)：" + chartInfo.runTime;
            formMain.lbAccelSpeed.Text = "加速度(mm/s²)：" + chartInfo.accelSpeed;
            formMain.lbMaxSpeed.Text = "運行速度(mm/s)：" + chartInfo.maxSpeed;
            formMain.lbCycleTime.Text = "往返時間(s)：" + chartInfo.cycleTime;
        }
    }
}
