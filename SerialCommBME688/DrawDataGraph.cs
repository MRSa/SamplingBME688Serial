using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace SamplingBME688Serial
{
    internal class DrawDataGraph
    {
        private Dictionary<int, DataGridViewRow>? selectedData;
        private Dictionary<String, List<List<GraphDataValue>>> dataSet1;
        private Dictionary<String, List<List<GraphDataValue>>> dataSet2;
        private GraphDataValue lowerLimit;
        private GraphDataValue upperLimit;
        private GraphDataValue lowerLimitZoom;
        private GraphDataValue upperLimitZoom;

        private const String fontName = "Yu Gothic UI";
        private const int fontSize = 10;
        private const int POSITION_TOP = -1;
        private const int POSITION_MIDDLE = 0;
        private const int POSITION_BOTTOM = 1;
        private const float heightMargin = 20;
        private const float widthMargin = 30; // 30;
        private const float areaX = 10.0f;

        private bool useGasRegistanceLog = false;
        private double currentUpperLimit = 110000000.0f;
        private double currentLowerLimit = 0.0f;
        private const bool isDebug = false;

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
        }

        public void setDataToDraw(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<String, List<List<GraphDataValue>>> dataSet1, Dictionary<String, List<List<GraphDataValue>>> dataSet2, GraphDataValue lowerLimit, GraphDataValue upperLimit, GraphDataValue lowerLimitZoom, GraphDataValue upperLimitZoom)
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
                    String sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                    int sensorId = int.Parse(sensorIdStr);
                    String? key = rowData.Cells[0].Value.ToString();
                    String categoryName = key ?? "";
                    List<List<GraphDataValue>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];
                    Debug.WriteLine($"{index}:{rowData.Cells[0].Value}[{rowData.Cells[1].Value}]{rowData.Cells[2].Value}  {targetDataSet.Count}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " setDataToDraw()" + ex.Message);
            }
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
                g.DrawString($"{index}", font, textBrush, lineX - (size.Width / 2.0f), textPointY);
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


        public void drawGraph(Graphics g, RectangleF drawArea, int strongLineIndex, float line1Position, float line2Position, float line3Position)
        {
            Debug.WriteLine(DateTime.Now + " ----- drawGraph ----- : " + strongLineIndex);
            try
            {
                //  選択されているグラフを書く
                int strongIndex = 1;
                foreach (KeyValuePair<int, DataGridViewRow> pair in selectedData)
                {
                    int index = pair.Key;
                    DataGridViewRow rowData = pair.Value;
                    String sensorIdStr = rowData.Cells[1].Value.ToString() ?? "1";
                    int sensorId = int.Parse(sensorIdStr);
                    String? key = rowData.Cells[0].Value.ToString();
                    String categoryName = key ?? "";
                    List<List<GraphDataValue>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];

                    int lineStroke = (strongLineIndex == strongIndex) ? 5 : 0;

                    // 先頭データ
                    if (line1Position >= 0.0f)
                    {
                        int startCount = (int)(targetDataSet.Count * line1Position);  // 先頭データの場所
                        List<GraphDataValue> startDataset = targetDataSet[startCount];
                        drawLines(g, drawArea, new Pen(getColor(index, sensorId, POSITION_TOP), lineStroke), categoryName + " (" + sensorIdStr + ")", startDataset);
                    }

                    // 真ん中データ
                    if (line2Position >= 0.0f)
                    {
                        int middleCount = (int)(targetDataSet.Count * line2Position);  // 真ん中データの場所
                        List<GraphDataValue> middleDataset = targetDataSet[middleCount];
                        drawLines(g, drawArea, new Pen(getColor(index, sensorId, POSITION_MIDDLE), lineStroke), "", middleDataset);
                    }

                    // 末尾データ
                    if (line3Position >= 0.0f)
                    {
                        int finishCount = (int)(targetDataSet.Count * line3Position);  // 末尾データの場所
                        List<GraphDataValue> finishDataset = targetDataSet[finishCount];
                        drawLines(g, drawArea, new Pen(getColor(index, sensorId, POSITION_BOTTOM), lineStroke), "", finishDataset);
                    }
                    if (isDebug)
                    {
                        Debug.WriteLine($"{rowData.Cells[0].Value}[Sensor{rowData.Cells[1].Value}] ({(int)(targetDataSet.Count * line1Position)} / {(int)(targetDataSet.Count * line2Position)} / {(int)(targetDataSet.Count * line3Position)})");
                    }
                    strongIndex++;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawGraph()" + ex.Message);
            }
            Debug.WriteLine(" ----- ");

        }

        private Color getColor(int indexNo, int sensorId, int position)
        {
            Color retColor = Color.DarkBlue;
            if (sensorId == 1)
            {

                switch (position)
                {
                    case POSITION_BOTTOM:
                        retColor = Color.LightBlue;
                        break;
                    case POSITION_TOP:
                        retColor = Color.AliceBlue;
                        break;
                    case POSITION_MIDDLE:
                    default:
                        retColor = Color.DarkBlue;
                        break;
                }
            }
            else
            {
                switch (position)
                {
                    case POSITION_BOTTOM:
                        retColor = Color.LightGreen;
                        break;
                    case POSITION_TOP:
                        retColor = Color.GreenYellow;
                        break;
                    case POSITION_MIDDLE:
                    default:
                        retColor = Color.DarkGreen;
                        break;
                }
            }
            return (retColor);
        }

        private void drawLines(Graphics g, RectangleF drawArea, Pen pen, String label, List<GraphDataValue> dataset)
        {
            // Debug.WriteLine(" ");

            float axisArea = drawArea.Width / areaX;
            float areaSize = drawArea.Height - heightMargin - heightMargin;
            double maxRange = currentUpperLimit - currentLowerLimit;

            int index = 0;
            PointF[] points = new PointF[dataset.Count];
            foreach (GraphDataValue dataValue in dataset)
            {
                double data = ((useGasRegistanceLog) ? dataValue.gas_registance_log : dataValue.gas_registance) - currentLowerLimit;
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)index);
                float posY =((float)(maxRange - data)) * (areaSize / (float) maxRange) + heightMargin;
                if (isDebug)
                {
                    Debug.WriteLine($" drawLines() ({lineX},{posY}) : {areaSize}");
                }
                points[index] = new PointF(lineX, posY);
                index++;
            }
            g.DrawLines(pen, points);
            g.DrawString(label, new Font(fontName, fontSize), new SolidBrush(Color.DarkGray), new PointF((drawArea.Left + widthMargin), points[0].Y + 5));
        }
    }
}
