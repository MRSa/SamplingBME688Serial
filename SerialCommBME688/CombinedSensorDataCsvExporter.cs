using SerialCommBME688;
using System.Diagnostics;
using System.Text;

namespace SamplingBME688Serial
{
    // センサ１とセンサ２のデータを合わせて１つのデータにしてCSV形式で出力する
    internal class CombinedSensorDataCsvExporter
    {
        private bool sensor1Received;
        private bool sensor2Received;
        private int dataIndexCount;
        private Dictionary<string, List<List<double>>> dataSet1;
        private Dictionary<string, List<List<double>>> dataSet2;

        public CombinedSensorDataCsvExporter(SerialReceiver sensor1, SerialReceiver sensor2)
        {
            this.sensor1Received = sensor1.isDataReceived();
            this.sensor2Received = sensor2.isDataReceived();
            this.dataIndexCount = sensor1.getDataIndexCount();

            this.dataSet1 = sensor1.getGasRegLogDataSet();
            this.dataSet2 = sensor2.getGasRegLogDataSet();
        }

        public void startExportCsvCombine(Stream myStream, int validCount, int numOfDuplicate)
        {
            try
            {
                // このままだと固まるはずなので、本当はここでコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();

                int duplicateCount = (numOfDuplicate <= 0) ? 1 : numOfDuplicate;

                StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);
                writer.AutoFlush = true;

                // combine mode なので、dataSetMap1 の itemを優先させる。

                // ----- ヘッダ部分の出力 -----
                List<string> categoryList = new List<string>();
                writer.Write("; index, ");

                if (sensor1Received)
                {
                    foreach (KeyValuePair<string, List<List<double>>> item in dataSet1)
                    {
                        try
                        {
                            if ((dataSet2.ContainsKey(item.Key)) || (!sensor2Received))
                            {
                                // センサ１とセンサ２で同じカテゴリがあった場合だけ、ヘッダ部分に出力する
                                writer.Write(item.Key + ", ");
                                categoryList.Add(item.Key);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(DateTime.Now + " --- not found the key : " + item.Key + " in sensor2... " + e.Message);
                        }
                    }
                }
                else
                {
                    // センサ２のみ受信していた場合...
                    foreach (KeyValuePair<string, List<List<double>>> item in dataSet2)
                    {
                        writer.Write(item.Key + ", ");
                        categoryList.Add(item.Key);
                    }
                }
                writer.WriteLine(" ;");
                // ----- ヘッダ部分の出力 -----


                // 指定されている繰り返し回数分、データを出力する
                for (int duplicate = 0; duplicate < duplicateCount; duplicate++)
                {
                    Dictionary<int, List<double>> valueset = new Dictionary<int, List<double>>();
                    for (int dataSetIndex = 0; dataSetIndex < validCount; dataSetIndex++)
                    {
                        int indexOffset = 0;
                        if (sensor1Received)
                        {
                            for (int dataIndex = 0; dataIndex < dataIndexCount; dataIndex++)
                            {
                                writer.Write(dataIndex + ", ");
                                foreach (string category in categoryList)
                                {
                                    double dataValue = dataSet1[category].ElementAt(dataSetIndex).ElementAt(dataIndex);
                                    writer.Write(dataValue + ", ");
                                }
                                writer.WriteLine(";" + " (Sensor1)"); // 改行
                            }
                            indexOffset = dataIndexCount;
                        }
                        if (sensor2Received)
                        {
                            for (int dataIndex = 0; dataIndex < dataIndexCount; dataIndex++)
                            {
                                writer.Write((dataIndex + indexOffset) + ", ");
                                foreach (string category in categoryList)
                                {
                                    double dataValue = dataSet2[category].ElementAt(dataSetIndex).ElementAt(dataIndex);
                                    writer.Write(dataValue + ", ");
                                }
                                writer.WriteLine(";" + " (Sensor2)"); // 改行
                            }
                            indexOffset = dataIndexCount;
                        }
                    }
                }
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportCsv() : " + e.Message);
            }
        }
    }
}
