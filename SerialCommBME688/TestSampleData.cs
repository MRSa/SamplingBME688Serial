using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public class TestSampleData
    {
        public OdorBothData getBothData1()
        {
            OdorBothData bothData = new OdorBothData
            {
                // Ristretto
                sequence0Value = 708650.5f,
                sequence1Value = 657463.87f,
                sequence2Value = 117755.29f,
                sequence3Value = 325285.91f,
                sequence4Value = 142064.38f,
                sequence5Value = 3406237.0f,
                sequence6Value = 957904.56f,
                sequence7Value = 10369620.0f,
                sequence8Value = 330642.56f,
                sequence9Value = 8241448.5f,
                sequence10Value = 330322.59f,
                sequence11Value = 308248.03f,
                sequence12Value = 71508.38f,
                sequence13Value = 182661.44f,
                sequence14Value = 117593.02f,
                sequence15Value = 1186902.37f,
                sequence16Value = 408212.09f,
                sequence17Value = 3425465.25f,
                sequence18Value = 156814.7f,
                sequence19Value = 3071040.25f
            };
            return (bothData);
        }

        public OdorBothData getBothData2()
        {
            OdorBothData bothData = new OdorBothData
            {
                // GreenTea
                sequence0Value = 5520215.5f,
                sequence1Value = 5189737.0f,
                sequence2Value = 523785.16f,
                sequence3Value = 1849627.5f,
                sequence4Value = 586819.5f,
                sequence5Value = 28018812.0f,
                sequence6Value = 6638573.5f,
                sequence7Value = 102400000.0f,
                sequence8Value = 1882785.62f,
                sequence9Value = 102400000.0f,
                sequence10Value = 4810334.5f,
                sequence11Value = 4471615.5f,
                sequence12Value = 548179.87f,
                sequence13Value = 1932075.5f,
                sequence14Value = 1015621.12f,
                sequence15Value = 23157598.0f,
                sequence16Value = 5698782.5f,
                sequence17Value = 102400000.0f,
                sequence18Value = 1596569.87f,
                sequence19Value = 90238896.0f
            };
            return (bothData);
        }

        public OdorBothData getBothData3()
        {
            OdorBothData bothData = new OdorBothData
            {
                // Ristretto (Part2)
                sequence0Value = 709386.94f,
                sequence1Value = 658097.69f,
                sequence2Value = 117755.29f,
                sequence3Value = 324667.09f,
                sequence4Value = 143016.77f,
                sequence5Value = 3401993.25f,
                sequence6Value = 962632.19f,
                sequence7Value = 10272100.0f,
                sequence8Value = 330322.59f,
                sequence9Value = 8235235.0f,
                sequence10Value = 328731.94f,
                sequence11Value = 309085.41f,
                sequence12Value = 71508.38f,
                sequence13Value = 182661.44f,
                sequence14Value = 117350.45f,
                sequence15Value = 1191043.87f,
                sequence16Value = 406752.72f,
                sequence17Value = 3434081f,
                sequence18Value = 157393.17f,
                sequence19Value = 3077963.5f
            };
            return (bothData);
        }

        public OdorBothData getBothData4()
        {
            OdorBothData bothData = new OdorBothData
            {
                // Guatemala
                sequence0Value = 726756.56f,
                sequence1Value = 667101.0f,
                sequence2Value = 118738.41f,
                sequence3Value = 329048.84f,
                sequence4Value = 146453.09f,
                sequence5Value = 3711826.0f,
                sequence6Value = 997807.56f,
                sequence7Value = 11175989.0f,
                sequence8Value = 335188.22f,
                sequence9Value = 8694083.0f,
                sequence10Value = 449615.81f,
                sequence11Value = 417448.03f,
                sequence12Value = 97672.64f,
                sequence13Value = 256000.0f,
                sequence14Value = 157248.16f,
                sequence15Value = 1832252.25f,
                sequence16Value = 571588.06f,
                sequence17Value = 5481432.0f,
                sequence18Value = 208851.72f,
                sequence19Value = 4698595f
            };
            return (bothData);
        }

        public OdorData getOdor1Data1()
        {
            OdorData odorData = new OdorData
            {
                // Ristretto
                sequence0Value = 708650.5f,
                sequence1Value = 657463.87f,
                sequence2Value = 117755.29f,
                sequence3Value = 325285.91f,
                sequence4Value = 142064.38f,
                sequence5Value = 3406237.0f,
                sequence6Value = 957904.56f,
                sequence7Value = 10369620.0f,
                sequence8Value = 330642.56f,
                sequence9Value = 8241448.5f,
            };
            return (odorData);
        }

        public OdorData getOdor1Data2()
        {
            OdorData odorData = new OdorData
            {
                // GreenTea
                sequence0Value = 5520215.5f,
                sequence1Value = 5189737.0f,
                sequence2Value = 523785.16f,
                sequence3Value = 1849627.5f,
                sequence4Value = 586819.5f,
                sequence5Value = 28018812.0f,
                sequence6Value = 6638573.5f,
                sequence7Value = 102400000.0f,
                sequence8Value = 1882785.62f,
                sequence9Value = 102400000.0f,
            };
            return (odorData);
        }

        public OdorData getOdor1Data3()
        {
            OdorData odorData = new OdorData
            {
                // Ristretto (Part2)
                sequence0Value = 709386.94f,
                sequence1Value = 658097.69f,
                sequence2Value = 117755.29f,
                sequence3Value = 324667.09f,
                sequence4Value = 143016.77f,
                sequence5Value = 3401993.25f,
                sequence6Value = 962632.19f,
                sequence7Value = 10272100.0f,
                sequence8Value = 330322.59f,
                sequence9Value = 8235235.0f,
            };
            return (odorData);
        }

        public OdorData getOdor1Data4()
        {
            OdorData odorData = new OdorData
            {
                // Guatemala
                sequence0Value = 726756.56f,
                sequence1Value = 667101.0f,
                sequence2Value = 118738.41f,
                sequence3Value = 329048.84f,
                sequence4Value = 146453.09f,
                sequence5Value = 3711826.0f,
                sequence6Value = 997807.56f,
                sequence7Value = 11175989.0f,
                sequence8Value = 335188.22f,
                sequence9Value = 8694083.0f,
            };
            return (odorData);
        }


        public OdorData getOdor2Data1()
        {
            OdorData odorData = new OdorData
            {
                // Ristretto
                sequence0Value = 330322.59f,
                sequence1Value = 308248.03f,
                sequence2Value = 71508.38f,
                sequence3Value = 182661.44f,
                sequence4Value = 117593.02f,
                sequence5Value = 1186902.37f,
                sequence6Value = 408212.09f,
                sequence7Value = 3425465.25f,
                sequence8Value = 156814.7f,
                sequence9Value = 3071040.25f
            };
            return (odorData);
        }

        public OdorData getOdor2Data2()
        {
            OdorData odorData = new OdorData
            {
                // GreenTea
                sequence0Value = 4810334.5f,
                sequence1Value = 4471615.5f,
                sequence2Value = 548179.87f,
                sequence3Value = 1932075.5f,
                sequence4Value = 1015621.12f,
                sequence5Value = 23157598.0f,
                sequence6Value = 5698782.5f,
                sequence7Value = 102400000.0f,
                sequence8Value = 1596569.87f,
                sequence9Value = 90238896.0f
            };
            return (odorData);
        }

        public OdorData getOdor2Data3()
        {
            OdorData odorData = new OdorData
            {
                // Ristretto (Part2)
                sequence0Value = 328731.94f,
                sequence1Value = 309085.41f,
                sequence2Value = 71508.38f,
                sequence3Value = 182661.44f,
                sequence4Value = 117350.45f,
                sequence5Value = 1191043.87f,
                sequence6Value = 406752.72f,
                sequence7Value = 3434081f,
                sequence8Value = 157393.17f,
                sequence9Value = 3077963.5f
            };
            return (odorData);
        }

        public OdorData getOdor2Data4()
        {
            OdorData odorData = new OdorData
            {
                // Guatemala
                sequence0Value = 449615.81f,
                sequence1Value = 417448.03f,
                sequence2Value = 97672.64f,
                sequence3Value = 256000.0f,
                sequence4Value = 157248.16f,
                sequence5Value = 1832252.25f,
                sequence6Value = 571588.06f,
                sequence7Value = 5481432.0f,
                sequence8Value = 208851.72f,
                sequence9Value = 4698595f
            };
            return (odorData);
        }

        public OdorOrData getOdorOr1Data1()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Ristretto
                sensorId = 1.0f,
                sequence0Value = 708650.5f,
                sequence1Value = 657463.87f,
                sequence2Value = 117755.29f,
                sequence3Value = 325285.91f,
                sequence4Value = 142064.38f,
                sequence5Value = 3406237.0f,
                sequence6Value = 957904.56f,
                sequence7Value = 10369620.0f,
                sequence8Value = 330642.56f,
                sequence9Value = 8241448.5f,
            };
            return (odorData);
        }

        public OdorOrData getOdorOr1Data2()
        {
            OdorOrData odorData = new OdorOrData
            {
                // GreenTea
                sensorId = 1.0f,
                sequence0Value = 5520215.5f,
                sequence1Value = 5189737.0f,
                sequence2Value = 523785.16f,
                sequence3Value = 1849627.5f,
                sequence4Value = 586819.5f,
                sequence5Value = 28018812.0f,
                sequence6Value = 6638573.5f,
                sequence7Value = 102400000.0f,
                sequence8Value = 1882785.62f,
                sequence9Value = 102400000.0f,
            };
            return (odorData);
        }

        public OdorOrData getOdorOr1Data3()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Ristretto (Part2)
                sensorId = 1.0f,
                sequence0Value = 709386.94f,
                sequence1Value = 658097.69f,
                sequence2Value = 117755.29f,
                sequence3Value = 324667.09f,
                sequence4Value = 143016.77f,
                sequence5Value = 3401993.25f,
                sequence6Value = 962632.19f,
                sequence7Value = 10272100.0f,
                sequence8Value = 330322.59f,
                sequence9Value = 8235235.0f,
            };
            return (odorData);
        }

        public OdorOrData getOdorOr1Data4()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Guatemala
                sensorId = 1.0f,
                sequence0Value = 726756.56f,
                sequence1Value = 667101.0f,
                sequence2Value = 118738.41f,
                sequence3Value = 329048.84f,
                sequence4Value = 146453.09f,
                sequence5Value = 3711826.0f,
                sequence6Value = 997807.56f,
                sequence7Value = 11175989.0f,
                sequence8Value = 335188.22f,
                sequence9Value = 8694083.0f,
            };
            return (odorData);
        }

        public OdorOrData getOdorOr2Data1()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Ristretto
                sensorId = 2.0f,
                sequence0Value = 330322.59f,
                sequence1Value = 308248.03f,
                sequence2Value = 71508.38f,
                sequence3Value = 182661.44f,
                sequence4Value = 117593.02f,
                sequence5Value = 1186902.37f,
                sequence6Value = 408212.09f,
                sequence7Value = 3425465.25f,
                sequence8Value = 156814.7f,
                sequence9Value = 3071040.25f
            };
            return (odorData);
        }

        public OdorOrData getOdorOr2Data2()
        {
            OdorOrData odorData = new OdorOrData
            {
                // GreenTea
                sensorId = 2.0f,
                sequence0Value = 4810334.5f,
                sequence1Value = 4471615.5f,
                sequence2Value = 548179.87f,
                sequence3Value = 1932075.5f,
                sequence4Value = 1015621.12f,
                sequence5Value = 23157598.0f,
                sequence6Value = 5698782.5f,
                sequence7Value = 102400000.0f,
                sequence8Value = 1596569.87f,
                sequence9Value = 90238896.0f
            };
            return (odorData);
        }

        public OdorOrData getOdorOr2Data3()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Ristretto (Part2)
                sensorId = 2.0f,
                sequence0Value = 328731.94f,
                sequence1Value = 309085.41f,
                sequence2Value = 71508.38f,
                sequence3Value = 182661.44f,
                sequence4Value = 117350.45f,
                sequence5Value = 1191043.87f,
                sequence6Value = 406752.72f,
                sequence7Value = 3434081f,
                sequence8Value = 157393.17f,
                sequence9Value = 3077963.5f
            };
            return (odorData);
        }

        public OdorOrData getOdorOr2Data4()
        {
            OdorOrData odorData = new OdorOrData
            {
                // Guatemala
                sensorId = 2.0f,
                sequence0Value = 449615.81f,
                sequence1Value = 417448.03f,
                sequence2Value = 97672.64f,
                sequence3Value = 256000.0f,
                sequence4Value = 157248.16f,
                sequence5Value = 1832252.25f,
                sequence6Value = 571588.06f,
                sequence7Value = 5481432.0f,
                sequence8Value = 208851.72f,
                sequence9Value = 4698595f
            };
            return (odorData);
        }
    }
}
