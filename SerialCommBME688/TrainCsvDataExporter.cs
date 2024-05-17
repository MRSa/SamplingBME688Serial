using System.Diagnostics;
using System.Text;

namespace SamplingBME688Serial
{
    class TrainCsvDataExporter
    {
        private const int VALID_DATA_INDEX = 10;
        private String outputFileName;
        private ICreateModelConsole console;
        private IDataHolder? port1;
        private IDataHolder? port2;

        public TrainCsvDataExporter(String outputFileName, IDataHolder? port1, IDataHolder? port2, ICreateModelConsole console)
        {
            this.outputFileName = outputFileName;
            this.port1 = port1;
            this.port2 = port2;
            this.console = console;
        }

        public bool outputDataSourceCSVFileSingle(int sensorId, int startPosition, int outputDataCount, int duplicateTimes, bool useLogData)
        {
            // --- CSVファイルにデータを出力する (only mode)
            try
            {
                if ((sensorId < 1) || (sensorId > 2))
                {
                    // 指定したセンサ番号が間違っていた場合 (1 か 2 ではなかった時)
                    console.appendText("the specified sensor id is wrong : " + sensorId + "\r\n");
                    return (false);
                }

                IDataHolder? port = (sensorId == 1) ? port1 : port2;
                if (port == null)
                {
                    // ----- 指定されたポートのデータがないので中止
                    console.appendText("the dataset does not exist at sensor" + sensorId + ".\r\n");
                    return (false);
                }

                // ----- データのCSVファイルへ吐き出す処理
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap = port.getGasRegDataSet();
                using (StreamWriter writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
                {
                    for (int index = 0; index < duplicateTimes; index++)
                    {
                        foreach (KeyValuePair<String, List<List<GraphDataValue>>> baseItem in dataSetMap)
                        {
                            // ---- 出力カウント数が０よりも小さい場合は、全データを出力する
                            if (outputDataCount < 0)
                            {
                                outputDataCount = baseItem.Value.Count;
                            }
                            String oneLineData;
                            for (int position = startPosition; position < (startPosition + outputDataCount); position++)
                            {
                                oneLineData = "";
                                List<GraphDataValue> portItem = baseItem.Value[position];
                                foreach (GraphDataValue item in portItem)
                                {
                                    if (useLogData)
                                    {
                                        oneLineData += item.gas_registance_log + ",";
                                    }
                                    else
                                    {
                                        oneLineData += item.gas_registance + ",";
                                    }
                                }
                                oneLineData += baseItem.Key;
                                writer.WriteLine(oneLineData);
                            }
                        }
                    }
                }
                console.appendText(" Write to CSV (" + sensorId + ") : " + dataSetMap.Count + " items x " + outputDataCount + " points x " + duplicateTimes + " times = " + (dataSetMap.Count * duplicateTimes * outputDataCount) + " lines.\r\n");
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] outputDataSourceCSVFileSingle() " + ee.Message);
                console.appendText("ERROR> " + ee.Message + "\r\n");
                return (false);
            }
            return (true);
        }

        public bool outputDataSourceCSVFile1and2(int startPosition, int outputDataCount, int duplicateTimes, bool useLogData)
        {
            // --- CSVファイルにデータを出力する (1 and 2 mode)
            try
            {
                if ((port1 == null) || (port2 == null))
                {
                    // ----- データがそろわないので中止
                    console.appendText("the dataset is not exist (1 and/or 2).\r\n");
                    return (false);
                }

                // ----- データのCSVファイルへ吐き出す処理
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap1 = port1.getGasRegDataSet();
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap2 = port2.getGasRegDataSet();
                using (StreamWriter writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
                {
                    for (int index = 0; index < duplicateTimes; index++)
                    {
                        foreach (KeyValuePair<String, List<List<GraphDataValue>>> item1 in dataSetMap1)
                        {
                            List<List<GraphDataValue>> item2 = dataSetMap2[item1.Key];
                            if (outputDataCount < 0)
                            {
                                // ---- 出力カウント数が０よりも小さい場合は、全データを出力する
                                outputDataCount = Math.Min(item1.Value.Count, item2.Count);
                            }
                            String oneLineData;
                            for (int position = startPosition; position < (startPosition + outputDataCount); position++)
                            {
                                oneLineData = "";
                                List<GraphDataValue> port1Item = item1.Value[position];
                                foreach (GraphDataValue item in port1Item)
                                {
                                    if (useLogData)
                                    {
                                        oneLineData += item.gas_registance_log + ",";
                                    }
                                    else
                                    {
                                        oneLineData += item.gas_registance + ",";
                                    }
                                }

                                List<GraphDataValue> port2Item = item2[position];
                                foreach (GraphDataValue item in port2Item)
                                {
                                    if (useLogData)
                                    {
                                        oneLineData += item.gas_registance_log + ",";
                                    }
                                    else
                                    {
                                        oneLineData += item.gas_registance + ",";
                                    }
                                }
                                oneLineData += item1.Key;
                                writer.WriteLine(oneLineData);
                            }
                        }
                     }
                }
                console.appendText(" Write to CSV : " + dataSetMap1.Count + " items x " + outputDataCount + " points x " + duplicateTimes + " times = " + (dataSetMap1.Count * duplicateTimes * outputDataCount) + " lines.\r\n");
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] outputDataSourceCSVFile1and2() " + ee.Message);
                console.appendText("ERROR> " + ee.Message + "\r\n");
                return (false);
            }
            return (true);
        }

        public bool outputDataSourceCSVFile1or2(int startPosition, int outputDataCount, int duplicateTimes, bool useLogData)
        {
            // --- CSVファイルにデータを出力する (1 or 2 mode)
            try
            {
                if ((port1 == null) || (port2 == null))
                {
                    // ----- データがそろわないので中止
                    console.appendText("the dataset is not exist (1 and/or 2).\r\n");
                    return (false);
                }

                // ----- データのCSVファイルへ吐き出す処理
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap1 = port1.getGasRegDataSet();
                Dictionary<String, List<List<GraphDataValue>>> dataSetMap2 = port2.getGasRegDataSet();
                using (StreamWriter writer = new StreamWriter(outputFileName, false, Encoding.UTF8))
                {
                    for (int index = 0; index < duplicateTimes; index++)
                    {
                        foreach (KeyValuePair<String, List<List<GraphDataValue>>> item1 in dataSetMap1)
                        {
                            // ---- 出力カウント数が０よりも小さい場合は、全データを出力する
                            if (outputDataCount < 0)
                            {
                                outputDataCount = item1.Value.Count;
                            }
                            String oneLineData;
                            for (int position = startPosition; position < (startPosition + outputDataCount); position++)
                            {
                                oneLineData = "1,";
                                List<GraphDataValue> port1Item = item1.Value[position];
                                foreach (GraphDataValue item in port1Item)
                                {
                                    if (useLogData)
                                    {
                                        oneLineData += item.gas_registance_log + ",";
                                    }
                                    else
                                    {
                                        oneLineData += item.gas_registance + ",";
                                    }
                                }
                                oneLineData += item1.Key;
                                writer.WriteLine(oneLineData);
                            }
                        }

                        foreach (KeyValuePair<String, List<List<GraphDataValue>>> item2 in dataSetMap2)
                        {
                            // ---- 出力カウント数が０よりも小さい場合は、全データを出力する
                            if (outputDataCount < 0)
                            {
                                outputDataCount = item2.Value.Count;
                            }
                            String oneLineData;
                            for (int position = startPosition; position < (startPosition + outputDataCount); position++)
                            {
                                oneLineData = "2,";
                                List<GraphDataValue> port2Item = item2.Value[position];
                                foreach (GraphDataValue item in port2Item)
                                {
                                    if (useLogData)
                                    {
                                        oneLineData += item.gas_registance_log + ",";
                                    }
                                    else
                                    {
                                        oneLineData += item.gas_registance + ",";
                                    }
                                }
                                oneLineData += item2.Key;
                                writer.WriteLine(oneLineData);
                            }
                        }
                    }
                }
                console.appendText(" Write to CSV : (" + dataSetMap1.Count + " + " + dataSetMap2.Count + ") = " + (dataSetMap1.Count + dataSetMap2.Count) + " items x " + outputDataCount + " points x " + duplicateTimes + " times = " + ((dataSetMap1.Count + dataSetMap2.Count) * duplicateTimes * outputDataCount) + " lines.\r\n");
            }
            catch (Exception ee)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] outputDataSourceCSVFile1or2() " + ee.Message);
                console.appendText("ERROR> " + ee.Message + "\r\n");
                return (false);
            }
            return (true);
        }
    }
}
