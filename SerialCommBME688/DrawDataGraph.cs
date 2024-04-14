using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace SamplingBME688Serial
{
    internal class DrawDataGraph
    {
        private Dictionary<int, DataGridViewRow> selectedData;
        private Dictionary<String, List<List<double>>> dataSet1;
        private Dictionary<String, List<List<double>>> dataSet2;

        private const float heightMargin = 20;
        private const float widthMargin = 30;
        private const float area = 10;

        public void setDataToDraw(ref Dictionary<int, DataGridViewRow> selectedData, Dictionary<String, List<List<double>>> dataSet1, Dictionary<String, List<List<double>>> dataSet2)
        {
            this.selectedData = selectedData;
            this.dataSet1 = dataSet1;
            this.dataSet2 = dataSet2;


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
                    List<List<double>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];
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
            float axisArea = drawArea.Width / area;


            Pen axisPen = new Pen(Color.LightGray);
            float lineTop = drawArea.Top + heightMargin;
            float lineBottom = drawArea.Height - heightMargin;


            SolidBrush brush = new SolidBrush(Color.LightGray);
            Font font = new Font("Yu Gothic UI", 10);

            int index = 0;
            while (index <= area)
            {
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)index);
                g.DrawLine(axisPen, lineX, lineTop, lineX, lineBottom);
                SizeF size = g.MeasureString($"{index}", font);
                g.DrawString($"{index}", font, brush, lineX - (size.Width / 2.0f), lineBottom + bottomMargin);
                index++;
            }
            axisPen.Dispose();
        }

        public void drawUsage(Graphics g, RectangleF drawArea)
        {
            //Debug.WriteLine(DateTime.Now + " ----- drawUsage -----");
            try
            {







            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawUsage()" + ex.Message);
            }
            //Debug.WriteLine(" ----- ");
        }


        public void drawGraph(Graphics g, RectangleF drawArea)
        {
            Debug.WriteLine(DateTime.Now + " ----- drawGraph -----");
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
                    List<List<double>> targetDataSet = (sensorId == 1) ? dataSet1[categoryName] : dataSet2[categoryName];

                    int startCount = 0;
                    int middleCount = targetDataSet.Count / 2;
                    int finishCount = targetDataSet.Count - 1;

                    List<double> startDataset = targetDataSet[startCount];
                    drawLines(g, drawArea, new Pen(Color.AliceBlue), categoryName + " (" + sensorIdStr + ")", startDataset);

                    List<double> middleDataset = targetDataSet[middleCount];
                    drawLines(g, drawArea, new Pen(Color.DarkBlue), "", middleDataset);

                    List<double> finishDataset = targetDataSet[finishCount];
                    drawLines(g, drawArea, new Pen(Color.LightBlue), "", finishDataset);


                    Debug.WriteLine($"{rowData.Cells[0].Value}[Sensor{rowData.Cells[1].Value}] ({startCount} / {middleCount} / {finishCount})");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawGraph()" + ex.Message);
            }
            Debug.WriteLine(" ----- ");

        }

        private void drawLines(Graphics g, RectangleF drawArea, Pen pen, String label, List<double> dataset)
        {
            Debug.WriteLine(" ");

            float maxValue = 20.0f;
            float axisArea = drawArea.Width / area;
            float areaSize = drawArea.Height - heightMargin - heightMargin;

            int index = 0;
            PointF[] points = new PointF[dataset.Count];
            foreach (double data in dataset)
            {
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)index);
                float posY =((float)(maxValue - data)) * (areaSize / maxValue) + heightMargin;
                Debug.WriteLine($" drawLines() ({lineX},{posY}) : {areaSize}");
                points[index] = new PointF(lineX, posY);
                index++;
            }
            g.DrawLines(pen, points);
            g.DrawString(label, new Font("Yu Gothic UI", 10), new SolidBrush(Color.DarkGray), new PointF((drawArea.Left + widthMargin), points[0].Y + 5));
        }
    }

}
