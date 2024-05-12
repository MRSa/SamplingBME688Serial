using Microsoft.ML.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System;

namespace SamplingBME688Serial
{
    public class OdorBothData
    {
        [LoadColumn(0)]
        public float sequence0Value;

        [LoadColumn(1)]
        public float sequence1Value;

        [LoadColumn(2)]
        public float sequence2Value;

        [LoadColumn(3)]
        public float sequence3Value;

        [LoadColumn(4)]
        public float sequence4Value;

        [LoadColumn(5)]
        public float sequence5Value;

        [LoadColumn(6)]
        public float sequence6Value;

        [LoadColumn(7)]
        public float sequence7Value;

        [LoadColumn(8)]
        public float sequence8Value;

        [LoadColumn(9)]
        public float sequence9Value;

        [LoadColumn(10)]
        public float sequence10Value;

        [LoadColumn(11)]
        public float sequence11Value;

        [LoadColumn(12)]
        public float sequence12Value;

        [LoadColumn(13)]
        public float sequence13Value;

        [LoadColumn(14)]
        public float sequence14Value;

        [LoadColumn(15)]
        public float sequence15Value;

        [LoadColumn(16)]
        public float sequence16Value;

        [LoadColumn(17)]
        public float sequence17Value;

        [LoadColumn(18)]
        public float sequence18Value;

        [LoadColumn(19)]
        public float sequence19Value;

        [LoadColumn(20)]
        public String? dataLabel;

        public OdorBothData(OdorBothData data)
        {
            this.sequence0Value = data.sequence0Value;
            this.sequence1Value = data.sequence1Value;
            this.sequence2Value = data.sequence2Value;
            this.sequence3Value = data.sequence3Value;
            this.sequence4Value = data.sequence4Value;
            this.sequence5Value = data.sequence5Value;
            this.sequence6Value = data.sequence6Value;
            this.sequence7Value = data.sequence7Value;
            this.sequence8Value = data.sequence8Value;
            this.sequence9Value = data.sequence9Value;
            this.sequence10Value = data.sequence10Value;
            this.sequence11Value = data.sequence11Value;
            this.sequence12Value = data.sequence12Value;
            this.sequence13Value = data.sequence13Value;
            this.sequence14Value = data.sequence14Value;
            this.sequence15Value = data.sequence15Value;
            this.sequence16Value = data.sequence16Value;
            this.sequence17Value = data.sequence17Value;
            this.sequence18Value = data.sequence18Value;
            this.sequence19Value = data.sequence19Value;
            this.dataLabel = data.dataLabel;
        }

        public OdorBothData(OdorOrData data1, OdorOrData data2)
        {
            this.sequence0Value = data1.sequence0Value;
            this.sequence1Value = data1.sequence1Value;
            this.sequence2Value = data1.sequence2Value;
            this.sequence3Value = data1.sequence3Value;
            this.sequence4Value = data1.sequence4Value;
            this.sequence5Value = data1.sequence5Value;
            this.sequence6Value = data1.sequence6Value;
            this.sequence7Value = data1.sequence7Value;
            this.sequence8Value = data1.sequence8Value;
            this.sequence9Value = data1.sequence9Value;
            this.sequence10Value = data2.sequence0Value;
            this.sequence11Value = data2.sequence1Value;
            this.sequence12Value = data2.sequence2Value;
            this.sequence13Value = data2.sequence3Value;
            this.sequence14Value = data2.sequence4Value;
            this.sequence15Value = data2.sequence5Value;
            this.sequence16Value = data2.sequence6Value;
            this.sequence17Value = data2.sequence7Value;
            this.sequence18Value = data2.sequence8Value;
            this.sequence19Value = data2.sequence9Value;
            this.dataLabel = data1.dataLabel;
        }

        public OdorBothData() { }
    }
}
