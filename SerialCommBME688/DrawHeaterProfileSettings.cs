using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace SamplingBME688Serial
{
    internal class DrawHeaterProfileSettings
    {
        private const string fontName = "Yu Gothic UI";
        private const int fontSize = 10;
        private const float heightMargin = 20;
        private const float widthMargin = 55;
        private const float areaX = 11.0f;
        private const float areaY = 10.0f;

        private double upperLimitY = 400.0d;
        private double lowerLimitY = 0.0d;

        // 背景の描画実処理
        public void drawBackground(Graphics g, RectangleF drawArea)
        {
            // 背景領域 （白色）
            g.FillRectangle(Brushes.White, drawArea.Left, drawArea.Top, drawArea.Width, drawArea.Height);
            g.DrawRectangle(new Pen(Color.Black), drawArea.Left, drawArea.Top, drawArea.Width, drawArea.Height);
        }

        // 軸の表示
        public void drawAixs(Graphics g, RectangleF drawArea, int totalTimeMs)
        {
            float bottomMargin = 5;
            float axisArea = drawArea.Width / areaX;
            float timeCount = totalTimeMs / (areaX - 1);

            Pen axisPen = new Pen(Color.LightGray);
            float lineTop = drawArea.Top + heightMargin;
            float lineBottom = drawArea.Height - heightMargin;

            SolidBrush textBrush = new SolidBrush(Color.Gray);
            Font font = new Font(fontName, fontSize);

            // ----- X軸 (縦線)の描画 -----
            for (int posX = 0; posX <= areaX; posX++)
            {
                // ----- X軸ラベル
                String labelX = (posX * timeCount / 1000.0f).ToString("F2");

                // ----- 縦の線
                float lineX = drawArea.Left + widthMargin + axisArea * ((float)posX);
                g.DrawLine(axisPen, lineX, lineTop, lineX, lineBottom);

                // ----- X軸のラベル表示座標位置 (X軸)
                SizeF size = g.MeasureString(labelX, font);
                float textPointY = lineBottom + bottomMargin;
                if (textPointY + size.Height > drawArea.Bottom)
                {
                    // 描画領域を下に抜ける場合は、文字を書く場所はちょっと上にする
                    textPointY = drawArea.Bottom - size.Height;
                }

                //  X軸のラベルを書く (単位: 秒)
                g.DrawString(labelX, font, textBrush, lineX - (size.Width / 2.0f), textPointY);
            }
            // ----- X軸の単位を描画
            String unitLabel = "sec.";
            SizeF labelSize = g.MeasureString(unitLabel, font);
            float labelPosX = drawArea.Left + widthMargin + axisArea * (areaX - 1.0f);
            float labelPosY = lineBottom + bottomMargin + labelSize.Height;
            g.DrawString(unitLabel, font, textBrush, labelPosX, labelPosY);

            // ---- Y軸の線を引く
            float startX = drawArea.Left + widthMargin;
            float finishX = startX + (axisArea * (areaX - 1));
            float rangeStep = (lineBottom - lineTop) / areaY;
            for (int posY = 0; posY <= areaY; posY++)
            {
                float lineY = lineBottom - posY * rangeStep;
                g.DrawLine(axisPen, startX, lineY, finishX, lineY);
            }

            // --- ラベル表示の余白
            float labelMarginX = 4.0f;
            float labelMarginY = 10.0f;

            // --- Y軸のラベル（単位）を書く
            g.DrawString("℃", font, textBrush, drawArea.Left + labelMarginX, drawArea.Top + labelMarginX);

            // --- Y軸のラベル（下限）の数値を書く
            String lowerLabel = lowerLimitY.ToString("F0");
            SizeF lowerLabelSize = g.MeasureString(lowerLabel, font);
            g.DrawString(lowerLabel, font, textBrush, drawArea.Left + widthMargin - lowerLabelSize.Width - labelMarginX, lineBottom - labelMarginY);

            // --- Y軸のラベル（上限）の数値を書く
            String upperLabel = upperLimitY.ToString("F0");
            SizeF upperLabelSize = g.MeasureString(upperLabel, font);
            g.DrawString(upperLabel, font, textBrush, drawArea.Left + widthMargin - upperLabelSize.Width - labelMarginX, drawArea.Top + labelMarginY);

            axisPen.Dispose();
        }

        public void drawGraph(Graphics g, RectangleF drawArea, HeaterProfileDataGrid dataGrid)
        {
            try
            {
                float lineBottom = drawArea.Height - heightMargin;
                float temperatureRange = drawArea.Height - drawArea.Top - (heightMargin * 2.0f);

                float axisArea = drawArea.Width / areaX;
                float startX = drawArea.Left + widthMargin;
                float finishX = startX + (axisArea * (areaX - 1));
                float lineLeft = drawArea.Left + widthMargin;
                float lineRight = lineLeft + (axisArea * (areaX - 1));

                double totalTimeMs = Convert.ToDouble(dataGrid.HeaterProfile[1].Step0 + dataGrid.HeaterProfile[1].Step1 + dataGrid.HeaterProfile[1].Step2 +
                    dataGrid.HeaterProfile[1].Step3 + dataGrid.HeaterProfile[1].Step4 + dataGrid.HeaterProfile[1].Step5 + dataGrid.HeaterProfile[1].Step6 +
                    dataGrid.HeaterProfile[1].Step7 + dataGrid.HeaterProfile[1].Step8 + dataGrid.HeaterProfile[1].Step9);

                double timeCount = lineRight - lineLeft;

                PointF[] points = new PointF[21];

                int pointIndex = 0;
                int radius = 5;
                Pen pen = new Pen(Color.DarkBlue);
                SolidBrush brush = new SolidBrush(Color.DarkBlue);

                // Start Position
                int sensorX_S = (int)lineLeft;
                int sensorY_S = (int)(lineBottom - dataGrid.HeaterProfile[0].Step0 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_S, sensorY_S);

                // Sensor 0
                double positionX = dataGrid.HeaterProfile[1].Step0;
                int sensorX_0 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_0 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step1 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_0, sensorY_S);
                points[pointIndex++] = new PointF(sensorX_0, sensorY_0);

                // Sensor 1
                positionX += dataGrid.HeaterProfile[1].Step1;
                int sensorX_1 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_1 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step2 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_1, sensorY_0);
                points[pointIndex++] = new PointF(sensorX_1, sensorY_1);

                // Sensor 2
                positionX += dataGrid.HeaterProfile[1].Step2;
                int sensorX_2 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_2 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step3 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_2, sensorY_1);
                points[pointIndex++] = new PointF(sensorX_2, sensorY_2);

                // Sensor 3
                positionX += dataGrid.HeaterProfile[1].Step3;
                int sensorX_3 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_3 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step4 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_3, sensorY_2);
                points[pointIndex++] = new PointF(sensorX_3, sensorY_3);

                // Sensor 4
                positionX += dataGrid.HeaterProfile[1].Step4;
                int sensorX_4 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_4 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step5 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_4, sensorY_3);
                points[pointIndex++] = new PointF(sensorX_4, sensorY_4);

                // Sensor 5
                positionX += dataGrid.HeaterProfile[1].Step5;
                int sensorX_5 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_5 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step6 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_5, sensorY_4);
                points[pointIndex++] = new PointF(sensorX_5, sensorY_5);

                // Sensor 6
                positionX += dataGrid.HeaterProfile[1].Step6;
                int sensorX_6 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_6 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step7 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_6, sensorY_5);
                points[pointIndex++] = new PointF(sensorX_6, sensorY_6);

                // Sensor 7
                positionX += dataGrid.HeaterProfile[1].Step7;
                int sensorX_7 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_7 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step8 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_7, sensorY_6);
                points[pointIndex++] = new PointF(sensorX_7, sensorY_7);

                // Sensor 8
                positionX += dataGrid.HeaterProfile[1].Step8;
                int sensorX_8 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_8 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step9 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_8, sensorY_7);
                points[pointIndex++] = new PointF(sensorX_8, sensorY_8);

                // Sensor 9
                positionX += dataGrid.HeaterProfile[1].Step9;
                int sensorX_9 = (int)(timeCount * (positionX / totalTimeMs) + lineLeft);
                int sensorY_9 = (int)(lineBottom - dataGrid.HeaterProfile[0].Step9 / upperLimitY * temperatureRange);
                points[pointIndex++] = new PointF(sensorX_9, sensorY_8);
                points[pointIndex++] = new PointF(sensorX_9, sensorY_9);

                // ラインを引く
                g.DrawLines(pen, points);


                // 点を打つ
                Rectangle rect0 = new Rectangle((int) (sensorX_0 - radius), (int) (sensorY_S - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect0);

                Rectangle rect1 = new Rectangle((int)(sensorX_1 - radius), (int)(sensorY_0 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect1);

                Rectangle rect2 = new Rectangle((int)(sensorX_2 - radius), (int)(sensorY_1 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect2);

                Rectangle rect3 = new Rectangle((int)(sensorX_3 - radius), (int)(sensorY_2 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect3);

                Rectangle rect4 = new Rectangle((int)(sensorX_4 - radius), (int)(sensorY_3 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect4);

                Rectangle rect5 = new Rectangle((int)(sensorX_5 - radius), (int)(sensorY_4 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect5);

                Rectangle rect6 = new Rectangle((int)(sensorX_6 - radius), (int)(sensorY_5 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect6);

                Rectangle rect7 = new Rectangle((int)(sensorX_7 - radius), (int)(sensorY_6 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect7);

                Rectangle rect8 = new Rectangle((int)(sensorX_8 - radius), (int)(sensorY_7 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect8);

                Rectangle rect9 = new Rectangle((int)(sensorX_9 - radius), (int)(sensorY_8 - radius), (radius * 2), (radius * 2));
                g.FillEllipse(brush, rect9);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " drawGraph()" + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
        }
    }
}
