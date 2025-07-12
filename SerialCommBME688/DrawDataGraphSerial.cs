using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SamplingBME688Serial
{
    internal class DrawDataGraphSerial
    {
        private Dictionary<int, DataGridViewRow>? selectedData;
        private Dictionary<string, List<List<GraphDataValue>>> dataSet1;
        private Dictionary<string, List<List<GraphDataValue>>> dataSet2;
        private GraphDataValue lowerLimit;
        private GraphDataValue upperLimit;
        private GraphDataValue lowerLimitZoom;
        private GraphDataValue upperLimitZoom;
        private int xAxisCount = 0;

        private const string fontName = "Yu Gothic UI";
        private const int fontSize = 10;
        private const float heightMargin = 20;
        private const float widthMargin = 30; // 30;
        private const float areaX = 10.0f;

        private bool useGasRegistanceLog = false;
        private double currentUpperLimit = 110000000.0d;
        private double currentLowerLimit = 0.0d;

        private double currentUpperLimitPressure = 110000.0d;
        private double currentLowerLimitPressure = 0.0d;

        private double currentUpperLimitTemperature = 85.0d;
        private double currentLowerLimitTemperature = 0.0d;

        private double currentUpperLimitHumidity = 100.0d;
        private double currentLowerLimitHumidity = 0.0d;

        public void selectGraphData(bool useGasRegistanceLog, bool isZoom)
        {
            this.useGasRegistanceLog = useGasRegistanceLog;
            if (useGasRegistanceLog)
            {
                this.currentLowerLimit = Math.Floor((isZoom) ? lowerLimitZoom.gas_registance_log : lowerLimit.gas_registance_log);
                this.currentUpperLimit = Math.Ceiling((isZoom) ? upperLimitZoom.gas_registance_log : upperLimit.gas_registance_log);
            }
            else
            {
                this.currentLowerLimit = Math.Floor((isZoom) ? lowerLimitZoom.gas_registance : lowerLimit.gas_registance);
                this.currentUpperLimit = Math.Ceiling((isZoom) ? upperLimitZoom.gas_registance : upperLimit.gas_registance);
            }
            this.currentUpperLimitPressure = Math.Ceiling((isZoom) ? upperLimitZoom.pressure : upperLimit.pressure);
            this.currentLowerLimitPressure = Math.Floor((isZoom) ? lowerLimitZoom.pressure : lowerLimit.pressure);
            this.currentUpperLimitTemperature = Math.Ceiling((isZoom) ? upperLimitZoom.temperature : upperLimit.temperature);
            this.currentLowerLimitTemperature = Math.Floor((isZoom) ? lowerLimitZoom.temperature : lowerLimit.temperature);
            this.currentUpperLimitHumidity = Math.Ceiling((isZoom) ? upperLimitZoom.humidity : upperLimit.humidity);
            this.currentLowerLimitHumidity = Math.Floor((isZoom) ? lowerLimitZoom.humidity : lowerLimit.humidity);
        }

        public void setDataToDraw(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<string, List<List<GraphDataValue>>> dataSet1, Dictionary<string, List<List<GraphDataValue>>> dataSet2, GraphDataValue lowerLimit, GraphDataValue upperLimit, GraphDataValue lowerLimitZoom, GraphDataValue upperLimitZoom)
        {
            this.selectedData = selectedData;
            this.dataSet1 = dataSet1;
            this.dataSet2 = dataSet2;
            this.upperLimit = upperLimit;
            this.lowerLimit = lowerLimit;
            this.upperLimitZoom = upperLimitZoom;
            this.lowerLimitZoom = lowerLimitZoom;

            Debug.WriteLine(DateTime.Now + " ----- setDataToDraw -----");
            try
            {
                foreach (KeyValuePair<int, DataGridViewRow> pair in selectedData)
                {
                    int index = pair.Key;
                    DataGridViewRow rowData = pair.Value;
                    string sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                    int sensorId = int.Parse(sensorIdStr);
                    string? key = rowData.Cells[0].Value.ToString();
                    string categoryName = key ?? "";
                    List<List<GraphDataValue>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];
                    //Debug.WriteLine($"{index}:{rowData.Cells[0].Value}[{rowData.Cells[1].Value}]{rowData.Cells[2].Value}  {targetDataSet.Count}");
                    xAxisCount = (xAxisCount < targetDataSet.Count) ? targetDataSet.Count : xAxisCount;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " setDataToDraw()" + ex.Message);
            }
            //Debug.WriteLine(" xAxisCount: " + xAxisCount);
            Debug.WriteLine(" ----- ");
        }

        // 背景の描画実処理
        public void drawBackground(Graphics g, RectangleF drawArea)
        {
            // 背景領域 （白色）
            g.FillRectangle(Brushes.White, drawArea.Left, drawArea.Top, drawArea.Width, drawArea.Height);
            g.DrawRectangle(new Pen(Color.Black), drawArea.Left, drawArea.Top, drawArea.Width, drawArea.Height);
        }

        // 軸の表示
        public void drawAixs(Graphics g, RectangleF drawArea)
        {
            float bottomMargin = 5;
            float axisArea = drawArea.Width / areaX;

            Pen axisPen = new Pen(Color.LightGray);
            float lineTop = drawArea.Top + heightMargin;
            float lineBottom = drawArea.Height - heightMargin;

            SolidBrush textBrush = new SolidBrush(Color.Gray);
            Font font = new Font(fontName, fontSize);

            int index = 0;
            while (index <= areaX)
            {
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)index);
                g.DrawLine(axisPen, lineX, lineTop, lineX, lineBottom);
                SizeF size = g.MeasureString($"{index}", font);
                float textPointY = lineBottom + bottomMargin;
                if (textPointY + size.Height > drawArea.Bottom)
                {
                    // 描画領域を下に抜ける場合は、文字を書く場所はちょっと上にする
                    textPointY = drawArea.Bottom - size.Height;
                }

                //  X軸のラベルを書く
                int xAxisLabel = Convert.ToInt32(Convert.ToDouble(index) * Convert.ToDouble(xAxisCount) / 9.0d);
                g.DrawString($"{xAxisLabel}", font, textBrush, lineX - (size.Width / 2.0f), textPointY);
                //Debug.WriteLine("X axis label: " + xAxisLabel + " count: " + xAxisCount + " [" + index + "]");
                index++;
            }

            float areaSize = drawArea.Height - heightMargin - heightMargin;
            float startX = drawArea.Left + widthMargin;
            float finishX = startX + (axisArea * (areaX - 1));
            float rangeStep = areaSize / 10.0f;
            index = 0;
            while (index <= 10)
            {
                float posY = rangeStep * index + heightMargin;
                g.DrawLine(axisPen, startX, posY, finishX, posY);
                index++;
            }

            // --- Y軸のラベル（下限・上限）の数値を書く
            g.DrawString($"{currentLowerLimit:F0}", font, textBrush, startX + (axisArea * (areaX - 1)) + 2, areaSize);
            g.DrawString($"{currentUpperLimit:F0}", font, textBrush, startX + (axisArea * (areaX - 1)) + 2, drawArea.Top);

            axisPen.Dispose();
        }

        public void drawUsage(Graphics g, RectangleF drawArea)
        {
/*
            //Debug.WriteLine(DateTime.Now + " ----- drawUsage -----");
            try
            {
                // 凡例を描く

            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawUsage()" + ex.Message);
            }
            //Debug.WriteLine(" ----- ");
*/
        }

        public void drawGraph(Graphics g, RectangleF drawArea, int strongLineIndex, 
            bool drawStep1, bool drawStep2, bool drawStep3, bool drawStep4, bool drawStep5, 
            bool drawStep6, bool drawStep7, bool drawStep8, bool drawStep9, bool drawStep10,
            bool drawPressure, bool drawTemperature, bool drawHumidity)
        {
            Debug.WriteLine(DateTime.Now + " ----- drawGraph ----- : " + strongLineIndex);
            try
            {
                //  選択されているグラフを書く
                int strongIndex = 1;
                int drawingIndex = 0;
                foreach (KeyValuePair<int, DataGridViewRow> pair in selectedData)
                {
                    int index = pair.Key;
                    DataGridViewRow rowData = pair.Value;
                    string sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                    int sensorId = int.Parse(sensorIdStr);
                    string? key = rowData.Cells[0].Value.ToString();
                    string categoryName = key ?? "";
                    List<List<GraphDataValue>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];

                    int lineStroke = (strongLineIndex == strongIndex) ? 2 : 0;
                    if (drawPressure)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkMagenta : Color.DarkOrchid), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        drawLinesPressure(g, drawArea, lineStyle, "Pres.(" + sensorIdStr + ")", targetDataSet);
                    }
                    if (drawTemperature)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkMagenta : Color.DarkOrchid), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        drawLinesTemperature(g, drawArea, lineStyle, sensorIdStr + "Temp.", targetDataSet);
                    }
                    if (drawHumidity)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkMagenta : Color.DarkOrchid), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        drawLinesHumidity(g, drawArea, lineStyle, sensorIdStr + "Humi.", targetDataSet);
                    }
                    if (drawStep1)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.Blue : Color.Green), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-0 " + categoryName, targetDataSet, 0);
                    }
                    if (drawStep2)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.Blue : Color.Green), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-1 " + categoryName, targetDataSet, 1);
                    }
                    if (drawStep3)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.Blue : Color.Green), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-2 " + categoryName, targetDataSet, 2);
                    }
                    if (drawStep4)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.Blue : Color.Green), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-3 " + categoryName, targetDataSet, 3);
                    }
                    if (drawStep5)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.Blue : Color.Green), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-4 " + categoryName, targetDataSet, 4);
                    }
                    if (drawStep6)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkBlue : Color.DarkGreen), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-5 " + categoryName, targetDataSet, 5);
                    }
                    if (drawStep7)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkBlue : Color.DarkGreen), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-6 " + categoryName, targetDataSet, 6);
                    }
                    if (drawStep8)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkBlue : Color.DarkGreen), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-7 " + categoryName, targetDataSet, 7);
                    }
                    if (drawStep9)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkBlue : Color.DarkGreen), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-8 " + categoryName, targetDataSet, 8);
                    }
                    if (drawStep10)
                    {
                        Pen lineStyle = new Pen(((sensorId == 1) ? Color.DarkBlue : Color.DarkGreen), lineStroke);
                        lineStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                        drawLines(g, drawArea, lineStyle, sensorIdStr + "-9 " + categoryName, targetDataSet, 9);
                    }
                    strongIndex++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawGraph()" + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
            Debug.WriteLine(" ----- ");
        }

        private void drawLines(Graphics g, RectangleF drawArea, Pen pen, string label, List<List<GraphDataValue>> dataset, int dataIndex)
        {
            // Debug.WriteLine(" ");

            float axisArea = (drawArea.Width * (areaX - 1) / areaX) / dataset.Count;
            int pointMargin = Convert.ToInt32(Math.Ceiling(dataset.Count / (drawArea.Width - widthMargin)));
            float areaSize = drawArea.Height - heightMargin - heightMargin;
            double maxRange = currentUpperLimit - currentLowerLimit;

            int pointIndex = 0;
            PointF[] points = new PointF[dataset.Count];

            foreach (List<GraphDataValue> dataValue in dataset)
            {
                double data;
                if (dataIndex >= 0)
                {
                    data = ((useGasRegistanceLog) ? dataValue[dataIndex].gas_registance_log : dataValue[dataIndex].gas_registance) - currentLowerLimit;
                }
                else if (dataIndex == -1)
                {
                    data = dataValue[0].pressure;
                }
                else if (dataIndex == -2)
                {
                    data = dataValue[0].temperature;
                }
                else // if (dataIndex == -3)
                {
                    data = dataValue[0].humidity;
                }
                float lineX = drawArea.Left + widthMargin +  axisArea * ((float)pointIndex);
                float posY =  ((float)(maxRange - data)) * (areaSize / (float) maxRange) + heightMargin;
                points[pointIndex] = new PointF(lineX, posY);
                //Debug.WriteLine(" (" + lineX + "," + posY + " [" + pointIndex + "]");
                pointIndex++;
            }
            g.DrawLines(pen, points);

            int pointPos = Convert.ToInt32(Convert.ToDouble(pointIndex) * (0.25d + 0.08d * dataIndex));
            float labelYPosition = ((points[pointPos].Y + 5.0f) > areaSize) ? (points[pointPos].Y - 20.0f) : points[pointPos].Y + 5.0f;
            float labelXPosition = drawArea.Left + widthMargin + axisArea * ((float)pointPos);
            PointF labelPosition = new PointF(labelXPosition, labelYPosition);
            Debug.WriteLine(" (" + points[pointIndex - 1].X + "," + points[pointIndex - 1].Y + " [" + pointIndex + "]  <" + drawArea.Width + "," + drawArea.Height + ">");
            g.DrawString(label, new Font(fontName, fontSize), new SolidBrush(Color.DarkGray), labelPosition);
        }

        private void drawLinesPressure(Graphics g, RectangleF drawArea, Pen pen, string label, List<List<GraphDataValue>> dataset)
        {
            float axisArea = (drawArea.Width * (areaX - 1) / areaX) / dataset.Count;
            int pointMargin = Convert.ToInt32(Math.Ceiling(dataset.Count / (drawArea.Width - widthMargin)));
            float areaSize = drawArea.Height - heightMargin - heightMargin;
            double maxRange = currentUpperLimitPressure - currentLowerLimitPressure;
            double average = 0.0d;

            int pointIndex = 0;
            PointF[] points = new PointF[dataset.Count];

            foreach (List<GraphDataValue> dataValue in dataset)
            {
                double data = dataValue[0].pressure;
                average = data + average;
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)pointIndex);
                float posY = ((float)(maxRange - data)) * (areaSize / (float)maxRange) + heightMargin;
                points[pointIndex] = new PointF(lineX, posY);
                //Debug.WriteLine(" Pres. (" + lineX + "," + posY + " [" + pointIndex + "]" + data);
                pointIndex++;
            }

            String averageStr = (average / pointIndex / 100.0d).ToString("F1");
            g.DrawLines(pen, points);

            int pointPos = Convert.ToInt32(Convert.ToDouble(pointIndex) * (0.2d));
            float labelYPosition = ((points[pointPos].Y + 5.0f) > areaSize) ? (points[pointPos].Y - 20.0f) : points[pointPos].Y + 5.0f;
            float labelXPosition = drawArea.Left + widthMargin + axisArea * ((float)pointPos);
            PointF labelPosition = new PointF(labelXPosition, labelYPosition);
            Debug.WriteLine(" (" + points[pointIndex - 1].X + "," + points[pointIndex - 1].Y + " [" + pointIndex + "]  <" + drawArea.Width + "," + drawArea.Height + ">");
            Debug.WriteLine(" Pressure Lower:" + currentLowerLimitPressure + " Upper:" + currentUpperLimitPressure + " average: " + average + " maxRange: " + maxRange);
            g.DrawString(label + "(Ave. " + averageStr + " hPa)", new Font(fontName, fontSize), new SolidBrush(Color.DarkGray), labelPosition);
        }

        private void drawLinesTemperature(Graphics g, RectangleF drawArea, Pen pen, string label, List<List<GraphDataValue>> dataset)
        {
            float axisArea = (drawArea.Width * (areaX - 1) / areaX) / dataset.Count;
            int pointMargin = Convert.ToInt32(Math.Ceiling(dataset.Count / (drawArea.Width - widthMargin)));
            float areaSize = drawArea.Height - heightMargin - heightMargin;
            double maxRange = currentUpperLimitTemperature - currentLowerLimitTemperature;
            double average = 0.0d;

            int pointIndex = 0;
            PointF[] points = new PointF[dataset.Count];

            foreach (List<GraphDataValue> dataValue in dataset)
            {
                double data = dataValue[0].temperature;
                average = data + average;
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)pointIndex);
                float posY = ((float)(maxRange - data)) * (areaSize / (float)maxRange) + heightMargin;
                points[pointIndex] = new PointF(lineX, posY);
                //Debug.WriteLine(" Pres. (" + lineX + "," + posY + " [" + pointIndex + "]" + data);
                pointIndex++;
            }

            String averageStr = (average / pointIndex).ToString("F1");
            g.DrawLines(pen, points);

            int pointPos = Convert.ToInt32(Convert.ToDouble(pointIndex) * (0.2d));
            float labelYPosition = ((points[pointPos].Y + 5.0f) > areaSize) ? (points[pointPos].Y - 20.0f) : points[pointPos].Y + 5.0f;
            float labelXPosition = drawArea.Left + widthMargin + axisArea * ((float)pointPos);
            PointF labelPosition = new PointF(labelXPosition, labelYPosition);
            Debug.WriteLine(" (" + points[pointIndex - 1].X + "," + points[pointIndex - 1].Y + " [" + pointIndex + "]  <" + drawArea.Width + "," + drawArea.Height + ">");
            Debug.WriteLine(" Temperature Lower:" + currentLowerLimitTemperature + " Upper:" + currentUpperLimitTemperature + " average: " + average + " maxRange: " + maxRange);
            g.DrawString(label + "(Ave. " + averageStr + " degC)", new Font(fontName, fontSize), new SolidBrush(Color.DarkGray), labelPosition);
        }

        private void drawLinesHumidity(Graphics g, RectangleF drawArea, Pen pen, string label, List<List<GraphDataValue>> dataset)
        {
            float axisArea = (drawArea.Width * (areaX - 1) / areaX) / dataset.Count;
            int pointMargin = Convert.ToInt32(Math.Ceiling(dataset.Count / (drawArea.Width - widthMargin)));
            float areaSize = drawArea.Height - heightMargin - heightMargin;
            double maxRange = currentUpperLimitHumidity - currentLowerLimitHumidity;
            double average = 0.0d;

            int pointIndex = 0;
            PointF[] points = new PointF[dataset.Count];

            foreach (List<GraphDataValue> dataValue in dataset)
            {
                double data = dataValue[0].humidity;
                average = data + average;
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)pointIndex);
                float posY = ((float)(maxRange - data)) * (areaSize / (float)maxRange) + heightMargin;
                points[pointIndex] = new PointF(lineX, posY);
                //Debug.WriteLine(" Pres. (" + lineX + "," + posY + " [" + pointIndex + "]" + data);
                pointIndex++;
            }

            String averageStr = (average / pointIndex).ToString("F1");
            g.DrawLines(pen, points);

            int pointPos = Convert.ToInt32(Convert.ToDouble(pointIndex) * (0.2d));
            float labelYPosition = ((points[pointPos].Y + 5.0f) > areaSize) ? (points[pointPos].Y - 20.0f) : points[pointPos].Y + 5.0f;
            float labelXPosition = drawArea.Left + widthMargin + axisArea * ((float)pointPos);
            PointF labelPosition = new PointF(labelXPosition, labelYPosition);
            //Debug.WriteLine(" (" + points[pointIndex - 1].X + "," + points[pointIndex - 1].Y + " [" + pointIndex + "]  <" + drawArea.Width + "," + drawArea.Height + ">");
            Debug.WriteLine(" Humidity Lower:" + currentLowerLimitHumidity + " Upper:" + currentUpperLimitHumidity + " average: " + average + " maxRange: " + maxRange);
            g.DrawString(label + "(Ave. " + averageStr + " %)", new Font(fontName, fontSize), new SolidBrush(Color.DarkGray), labelPosition);
        }
    }
}
