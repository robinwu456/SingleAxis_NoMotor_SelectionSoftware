using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;

namespace SingleAxis_NoMotor_SelectionSoftware {
    public class ChartInfo {
        private FormMain formMain;

        public ChartInfo(FormMain formMain) {
            this.formMain = formMain;

            //formMain.chart.Paint += PaintChartArrow;
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
            var models = formMain.page2.recommandList.curRecommandList.Where(model => model.name == curRow.Cells["項次"].Value.ToString() && model.lead == Convert.ToDouble(curRow.Cells["導程"].Value.ToString()));
            if (models.Count() == 0)
                return;
            Model curModel = models.First();
            List<PointF> points = formMain.page2.calc.GetChartPoints(curModel);

            // 畫圖
            ChartArea chartArea = formMain.chart.ChartAreas[0];
            // X
            chartArea.AxisX.Minimum = points.Select(p => p.X).Min();
            chartArea.AxisX.Maximum = points.Select(p => p.X).Max();
            chartArea.AxisX.Interval = chartArea.AxisX.Maximum / 10;
            chartArea.AxisX.LabelStyle.Format = "N2";
            // Y
            chartArea.AxisY.Minimum = points.Select(p => p.Y).Min();
            chartArea.AxisY.Maximum = Convert.ToDouble(points.Select(p => p.Y).Max().ToString("#0.000"));
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

            turningPoints = formMain.chart.Series[0].Points.Select(p => new PointF(
                (float)chartArea.AxisX.ValueToPixelPosition(p.XValue),
                (float)chartArea.AxisY.ValueToPixelPosition(p.YValues[0])
            )).ToArray();

            // 取圖資訊
            var chartInfo = formMain.page2.calc.GetChartInfo(curModel);
            formMain.lbAccelTime.Text = "加/減速時間(s)：" + chartInfo.accelTime;
            formMain.lbConstantTime.Text = "等速時間(s)：" + chartInfo.constantTime;
            formMain.lbRunTime.Text = "運行時間(s)：" + chartInfo.runTime;
            formMain.lbAccelSpeed.Text = "加速度(mm/s²)：" + chartInfo.accelSpeed;
            formMain.lbMaxSpeed.Text = "運行速度(mm/s)：" + chartInfo.maxSpeed;
            formMain.lbCycleTime.Text = "往返時間(s)：" + chartInfo.cycleTime;

            //PaintChartArrow(null, new PaintEventArgs(formMain.chart.CreateGraphics(), new Rectangle(0, 0, 402, 279)));
        }

        private float lineWidth = 1;
        private float arrowWidth = 3;
        private Color lineColor = Color.FromArgb(42, 88, 111);
        private float top1Pos = 0.09f;  // 中間起始位置
        private float top2Pos = 0.53f;   // 頭尾起始位置
        private float bot1Pos = 0.73f;  // 等速時間頭尾虛線結束位置
        private float bot2Pos = 0.82f;  // 運行時間頭尾虛線結束位置
        //private float bot3Pos = 0.65f;  // 單趟時間頭尾虛線結束位置
        private float chartValuePosOffset_Y = 15;   // 圖表資訊高度偏差量
        private Font chartValueFont = new Font("新細明體", 9);  // 圖表資訊文字設定
        private SolidBrush brush = new SolidBrush(Color.FromArgb(42, 88, 111)); // 圖表資訊文字顯示顏色
        private PointF[] turningPoints = new PointF[4];
        private delegate void DrawArrowLine(PointF posStart, PointF posEnd);
        private void PaintChartArrow(object sender, PaintEventArgs e) {
            DrawArrowLine DrawArrowLine = (posStart, posEnd) => {
                float toPointGap = 3;
                float arrowLength = 0.3f;
                Pen pen = new Pen(lineColor, lineWidth);

                // 箭頭繪製
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                // 線
                pen.Width = lineWidth;
                float lineStart_X = posStart.X + toPointGap + 1;
                float lineEnd_X = posEnd.X - toPointGap - 1;
                e.Graphics.DrawLine(pen, new PointF(lineStart_X, posStart.Y), new PointF(lineEnd_X, posEnd.Y));

                // 線過短不畫箭頭
                if (lineEnd_X - lineStart_X < 6.57f)
                    return;

                // 箭頭(左)
                pen.Width = arrowWidth;
                pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                e.Graphics.DrawLine(pen, new PointF(posStart.X + toPointGap, posStart.Y), new PointF(posStart.X + toPointGap + arrowLength, posStart.Y));
                // 箭頭(右)
                pen.Width = arrowWidth;
                pen.StartCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                e.Graphics.DrawLine(pen, new PointF(posEnd.X - toPointGap - arrowLength, posEnd.Y), new PointF(posEnd.X - toPointGap, posEnd.Y));
            };

            Func<string, string> GetChartInfo = new Func<string, string>((labelText) => {
                //Regex reg = new Regex(@".+：(.+)\ss");
                Regex reg = new Regex(@".+：(.+)");
                return reg.Match(labelText).Groups[1].Value;
            });

            float pixelDistancePerChar = 5.6f;  // 每個char需要長度
            // 計算字串置中開頭位置
            Func<float, float, string, float> GetMiddleStartPos = new Func<float, float, string, float>((start, end, text) => {
                float distance = end - start;
                float middlePos = distance / 2;
                int textLength = text.Length;
                float textNeedPixelDistance = pixelDistancePerChar * textLength;
                return start + (middlePos - (textNeedPixelDistance / 2));
            });

            try {
                if (formMain.chart.Series[0].Points.Count == 0)
                    return;

                float[] dashValues = { 2, 2, 2, 2 };

                Pen pen = new Pen(lineColor, lineWidth);
                pen.DashPattern = dashValues;

                // 虛線
                foreach (PointF turnPoint in turningPoints) {
                    // 頭尾
                    if (turningPoints.ToList().IndexOf(turnPoint) == 0 || turningPoints.ToList().IndexOf(turnPoint) == turningPoints.Length - 1)
                        //e.Graphics.DrawLine(pen, new PointF(turnPoint.X, formMain.chart.Size.Height * top2Pos), new PointF(turnPoint.X, formMain.chart.Size.Height * bot3Pos));
                        e.Graphics.DrawLine(pen, new PointF(turnPoint.X, formMain.chart.Size.Height * top2Pos), new PointF(turnPoint.X, formMain.chart.Size.Height * bot2Pos));
                    //// 倒數第二
                    //else if (turningPoints.ToList().IndexOf(turnPoint) == turningPoints.Length - 2)
                    //    e.Graphics.DrawLine(pen, new PointF(turnPoint.X, formMain.chart.Size.Height * top2Pos), new PointF(turnPoint.X, formMain.chart.Size.Height * bot2Pos));
                    // 中間
                    else
                        e.Graphics.DrawLine(pen, new PointF(turnPoint.X, formMain.chart.Size.Height * top1Pos), new PointF(turnPoint.X, formMain.chart.Size.Height * bot1Pos));
                }

                // 箭頭繪製
                DrawArrowLine(new PointF(turningPoints[0].X, formMain.chart.Size.Height * bot1Pos), new PointF(turningPoints[1].X, formMain.chart.Size.Height * bot1Pos));        // 加速時間
                DrawArrowLine(new PointF(turningPoints[1].X, formMain.chart.Size.Height * bot1Pos), new PointF(turningPoints[2].X, formMain.chart.Size.Height * bot1Pos));        // 等速時間
                DrawArrowLine(new PointF(turningPoints[2].X, formMain.chart.Size.Height * bot1Pos), new PointF(turningPoints[3].X, formMain.chart.Size.Height * bot1Pos));        // 減速時間
                //DrawArrowLine(new PointF(turningPoints[3].X, formMain.chart.Size.Height * bot1Pos), new PointF(turningPoints[4].X, formMain.chart.Size.Height * bot1Pos));        // 停滯時間
                DrawArrowLine(new PointF(turningPoints[0].X, formMain.chart.Size.Height * bot2Pos), new PointF(turningPoints[3].X, formMain.chart.Size.Height * bot2Pos));        // 運行時間
                //DrawArrowLine(new PointF(turningPoints[0].X, formMain.chart.Size.Height * bot3Pos), new PointF(turningPoints[4].X, formMain.chart.Size.Height * bot3Pos));        // 全長

                // 圖表資訊位置                
                e.Graphics.DrawString(GetChartInfo(formMain.lbAccelTime.Text), chartValueFont, brush,
                    GetMiddleStartPos(turningPoints[0].X, turningPoints[1].X, GetChartInfo(formMain.lbAccelTime.Text)), formMain.chart.Size.Height * bot1Pos - chartValuePosOffset_Y); // 加速時間
                e.Graphics.DrawString(GetChartInfo(formMain.lbConstantTime.Text), chartValueFont, brush,
                    GetMiddleStartPos(turningPoints[1].X, turningPoints[2].X, GetChartInfo(formMain.lbConstantTime.Text)), formMain.chart.Size.Height * bot1Pos - chartValuePosOffset_Y);  // 等速時間
                e.Graphics.DrawString(GetChartInfo(formMain.lbAccelTime.Text), chartValueFont, brush,
                    GetMiddleStartPos(turningPoints[2].X, turningPoints[3].X, GetChartInfo(formMain.lbAccelTime.Text)), formMain.chart.Size.Height * bot1Pos - chartValuePosOffset_Y); // 減速時間
                //e.Graphics.DrawString(GetChartInfo(formMain.labelStopTime.Text), chartValueFont, brush,
                //    GetMiddleStartPos(turningPoints[3].X, turningPoints[4].X, GetChartInfo(formMain.labelStopTime.Text)), formMain.chart.Size.Height * bot1Pos - chartValuePosOffset_Y);       // 停滯時間
                e.Graphics.DrawString(GetChartInfo(formMain.lbRunTime.Text), chartValueFont, brush,
                    GetMiddleStartPos(turningPoints[0].X, turningPoints[3].X, GetChartInfo(formMain.lbRunTime.Text)), formMain.chart.Size.Height * bot2Pos - chartValuePosOffset_Y);        // 運行時間
                //e.Graphics.DrawString(GetChartInfo(formMain.lbCycleTime.Text), chartValueFont, brush,
                //    GetMiddleStartPos(turningPoints[0].X, turningPoints[4].X, GetChartInfo(formMain.lbCycleTime.Text)), formMain.chart.Size.Height * bot3Pos - chartValuePosOffset_Y);      // 全長           

            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}
