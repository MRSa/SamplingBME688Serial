﻿using SamplingBME688Serial;
using System.Diagnostics;
using System.Text;

namespace SerialCommBME688
{
    internal class Bme688DataHolder : IDataEntryNotify, IDataHolder
    {
        private const int NUMBER_OF_INDEX = 10;
        private int sensorId;
        private int expectedIndexNumber = 0;
        private int receivedDataCount = 0;
        private IMessageCallback callback;
        private IDataReceiveNotify notify;
        private PostJsonData sendData;
        private Dictionary<string, Bme688DataSetGroup> dataSetMap = new Dictionary<string, Bme688DataSetGroup>();

        Bme688DataSetGroup? targetDataSet = null;

        public Bme688DataHolder(IMessageCallback callback, IDataReceiveNotify notify, int sensorId)
        {
            this.callback = callback;
            this.sensorId = sensorId;
            this.notify = notify;
            this.sendData = new PostJsonData(sensorId, NUMBER_OF_INDEX, this);
        }

        public string entryData(string category,
                               string sendUrl,
                               bool isSingleEntry,
                               int gas_index, 
                               int meas_index,
                               Int64 serial_number,
                               int data_status, 
                               int gas_wait, 
                               double temperature,
                               double humidity,
                               double pressure,
                               double gas_registance,
                               double gas_registance_log,
                               double gas_registance_diff
                               )
        {
            string result = "";
            try
            {
                if (sendUrl.Length > 0)
                {
                    // URLが指定されていた時(DATABASEに格納する指示があるとき)は、まず最初に送信部分にデータを送っておく
                    try
                    {
                        sendData.receivedData(
                            sendUrl,
                            isSingleEntry,
                            category,
                            sensorId,
                            gas_index,
                            meas_index,
                            serial_number,
                            data_status,
                            gas_wait,
                            temperature,
                            humidity,
                            pressure,
                            gas_registance,
                            gas_registance_log,
                            gas_registance_diff);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
                    }
                }

                // Debug.WriteLine(" entryData() : " + category + " [" + gas_index + "] (" + dataSetMap.Count + ") " + sendUrl);
                if (gas_index == 0)
                {
                    // 初回データを受信したとき
                    // すでに受信履歴があるか、確認
                    if (targetDataSet != null)
                    {
                        if (targetDataSet.dataGroupName == category)
                        {
                            // 同じものがあるので、そのまま使い、次のデータを待つ
                            targetDataSet.finishReceivedDataSet();
                            targetDataSet.startReceiveDataSet(temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                            expectedIndexNumber = 1;
                            receivedDataCount++;
                            result = category + " (" + receivedDataCount + ") " + DateTime.Now + "\r\n";
                            return (result);
                        }

                        // Mapに格納する
                        dataSetMap.Add(targetDataSet.dataGroupName, targetDataSet);
                        targetDataSet = null;
                    }

                    // 受信データがないとき...
                    receivedDataCount = 1;

                    // Dictionary にあるかどうか
                    dataSetMap.TryGetValue(category, out targetDataSet);
                    if (targetDataSet == null)
                    {
                        targetDataSet = new Bme688DataSetGroup(category, NUMBER_OF_INDEX);
                        result = category + " [" + receivedDataCount + "] \r\n";
                    }
                    else
                    {
                        receivedDataCount++;
                        result = category + "  " + receivedDataCount + " \r\n";
                    }
                    targetDataSet.startReceiveDataSet(temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                    expectedIndexNumber = 1;
                    return (result);
                }

                if (expectedIndexNumber > gas_index)
                {
                    // データが欠損して、待っている番号よりも小さい値が来た場合...
                    // (前データを上書きしてしまうが...)
                    result = " INDEX MISMATCH :" + expectedIndexNumber + " - " + gas_index + " (" + sensorId + ") " + DateTime.Now + "\r\n";
                }

                if (result.Length == 0)
                {
                    result = ".";
                }
                if (targetDataSet != null)
                {
                    // 待っていたデータが来たので、そのまま入れる
                    targetDataSet.setReceivedData(gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                    //Debug.WriteLine("ENTRY" + sensorId + "[" + targetDataSet.dataGroupName + "]  : " + gas_index + " " + temperature + " " + humidity + " " + gas_registance + ""); // これを入れるとかなり遅くなる...
                }

                expectedIndexNumber++;
                if (expectedIndexNumber == NUMBER_OF_INDEX)
                {
                    // 最大データを入れ終わったとき
                    expectedIndexNumber = 0;
                    result = ".\r\n";
                }
                return (result);
            }
            catch (Exception e)
            {
                result = "OCCUR Exception : " + e.Message + " " + "\r\n" + e.StackTrace + " (" + sensorId + ")" + "\r\n";
            }
            return (result);
        }

        public string finishReceivedData()
        {
            string result = "";
            try
            {
                Debug.WriteLine(DateTime.Now + " ----- finishReceivedData [" + sensorId + "] -----");
                {
                    try
                    {
                        // 受信データを保管する。(MAPに格納されていない場合は)
                        if (targetDataSet != null)
                        {
                            if (!dataSetMap.ContainsKey(targetDataSet.dataGroupName))
                            {
                                dataSetMap.Add(targetDataSet.dataGroupName, targetDataSet);
                            }
                        }
                        else
                        {
                            // -----
                            Debug.WriteLine(DateTime.Now + " ----- finishReceivedData [" + sensorId + "] 'targetDataSet' is null.");
                        }
                        targetDataSet = null;
                    }
                    catch (Exception ex)
                    {
                        result = result +" - " + ex.Message + "\r\n";
                        Debug.WriteLine(ex.Message);
                    }
                }
                expectedIndexNumber = 0;

                // 受信停止時、とりあえずカテゴリごとの有効データ数を画面表示する
                result = "\r\n----- " + DateTime.Now + "\r\n";
                foreach (KeyValuePair<string, Bme688DataSetGroup> item in dataSetMap)
                {
                    result = result + item.Key + "/" + item.Value.dataGroupName + " ";

                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();
                    result = result + "[" + dataSet.Count + "] ";  // データ件数

                    int validDataSet = 0;
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if (collectedData.lack_data == 0)
                        {
                            // データが欠損していない数を取得する
                            validDataSet++;
                        }
                    }
                   result = result + " valid:" + validDataSet + "\r\n";
                }
                result = result + "-----\r\n\r\n";

                // 終了報告をする
                finishReceivedDataSet();
            }
            catch (Exception ee)
            {
                result = result + " " + ee.Message + "\r\n";
                Debug.WriteLine(ee.Message);
            }
            return (result);
        }

        private void finishReceivedDataSet()
        {
            try
            {
                Dictionary<string, Bme688DataSetGroup> datamap = dataSetMap;
                foreach (KeyValuePair<string, Bme688DataSetGroup> item in datamap)
                {
                    string category = item.Key;
                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();
                    int sampleCount = dataSet.Count;
                    int validCount = 0;
                    double temperature_max = dataSet[0].temperature_max;
                    double temperature_min = dataSet[0].temperature_min;
                    double humidity_max = dataSet[0].humidity_max;
                    double humidity_min = dataSet[0].humidity_min;
                    double pressure_max = dataSet[0].pressure_max;
                    double pressure_min = dataSet[0].pressure_min;
                    double gas_registance_max = dataSet[0].gas_registance_max;
                    double gas_registance_min = dataSet[0].gas_registance_min;
                    double gas_registance_log_max = dataSet[0].gas_registance_log_max;
                    double gas_registance_log_min = dataSet[0].gas_registance_log_min;
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if (collectedData.lack_data == 0)
                        {
                            validCount++;
                        }
                        if (temperature_max < collectedData.temperature_max)
                        {
                            temperature_max = collectedData.temperature_max;
                        }
                        if (temperature_min > collectedData.temperature_min)
                        {
                            temperature_min = collectedData.temperature_min;
                        }
                        if (humidity_max < collectedData.humidity_max)
                        {
                            humidity_max = collectedData.humidity_max;
                        }
                        if (humidity_min > collectedData.humidity_min)
                        {
                            humidity_min = collectedData.humidity_min;
                        }
                        if (pressure_max < collectedData.pressure_max)
                        {
                            pressure_max = collectedData.pressure_max;
                        }
                        if (pressure_min > collectedData.pressure_min)
                        {
                            pressure_min = collectedData.pressure_min;
                        }
                        if (gas_registance_max < collectedData.gas_registance_max)
                        {
                            gas_registance_max = collectedData.gas_registance_max;
                        }
                        if (gas_registance_min > collectedData.gas_registance_min)
                        {
                            gas_registance_min = collectedData.gas_registance_min;
                        }
                        if (gas_registance_log_max < collectedData.gas_registance_log_max)
                        {
                            gas_registance_log_max = collectedData.gas_registance_log_max;
                        }
                        if (gas_registance_log_min > collectedData.gas_registance_log_min)
                        {
                            gas_registance_log_min = collectedData.gas_registance_log_min;
                        }
                    }

                    //  表示用のデータを通知する
                    Bme688DataSummary dataSummary = new Bme688DataSummary(category,
                                                                          sensorId,
                                                                          sampleCount,
                                                                          validCount,
                                                                          temperature_max,
                                                                          temperature_min,
                                                                          humidity_max,
                                                                          humidity_min,
                                                                          pressure_max,
                                                                          pressure_min,
                                                                          gas_registance_max,
                                                                          gas_registance_min,
                                                                          gas_registance_log_max,
                                                                          gas_registance_log_min);
                    notify.finishReceivedData(ref dataSummary);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
        }

        public void exportCsvData(Stream myStream, bool isWriteHeader, int startPosition, int endPosition)
        {
            // （欠損のない）受信データを指定した範囲について、全部ファイルにCSV形式で保管する
            try
            {
                Debug.WriteLine("exportCsvData() : canWrite: " + myStream.CanWrite);

                StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);

                writer.AutoFlush = true;
                if (isWriteHeader)
                {
                    writer.WriteLine("; sensorId, category, index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff");
                }

                foreach (KeyValuePair<string, Bme688DataSetGroup> item in dataSetMap)
                {
                    string category = item.Key;
                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();

                    // ----- 出力データの位置を決める
                    int dataSize = dataSet.Count;
                    float fromPosition = ((float)dataSize * ((float)startPosition / 100.0f));
                    float toPosition = ((float)dataSize * ((float)endPosition / 100.0f));

                    Debug.WriteLine(DateTime.Now + " Export : " + item.Key + " from: " + ((int)fromPosition) + " to: " + ((int)toPosition) + " count: " + dataSize);
                    int index = 0;
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if ((collectedData.lack_data == 0)&&(index >= (int) (fromPosition))&&(index <= ((int) toPosition)))
                        {
                            // データが指定範囲内かつ欠損していない場合、ファイルに出力する
                            for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                            {
                                try
                                {
                                    Bme688Data data = collectedData.getBme688Data(dataIndex);
                                    writer.WriteLine(sensorId + "," + category + "," + dataIndex + "," + data.temperature + "," + data.humidity + "," + data.pressure + "," + data.gas_registance + "," + data.gas_registance_log + "," + data.gas_registance_diff + ",;");
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(DateTime.Now + " Bme688Data() : " + ee.Message + " ");
                                }
                            }
                        }
                        index++;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvData() : " + e.Message + " ");

            }
        }

        public void exportCsvDataOnlyGasRegistance(StreamWriter writer, List<string> categoryList, int validCount, int numOfDuplicate)
        {
            // センサデータ（対数のみ）をCSV形式で出力する（ヘッダ部分は呼び出し側で出力する）
            try
            {
                int duplicateCount = (numOfDuplicate <= 0) ? 1 : numOfDuplicate;
                Debug.WriteLine("exportCsvDataOnlyGasRegistance() : start  " + " duplicate: " + duplicateCount + " valid: " + validCount);

                Dictionary<string, List<List<double>>> dataSet = getGasRegLogDataSet();
                for (int duplicate = 0; duplicate < duplicateCount; duplicate++)
                {
                    for (int dataSetIndex = 0; dataSetIndex < validCount; dataSetIndex++)
                    {
                        for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                        {
                            writer.Write(dataIndex + ", ");
                            foreach (string category in categoryList)
                            {
                                double dataValue = dataSet[category].ElementAt(dataSetIndex).ElementAt(dataIndex);
                                writer.Write(dataValue + ", ");
                            }
                            writer.WriteLine(";" + " (Sensor" + sensorId + ")"); // 改行
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.WriteLine(" index over ");
                // Debug.WriteLine(DateTime.Now + ex.StackTrace);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvDataOnlyGasRegistance() : " + e.Message + " ");
                Debug.WriteLine(DateTime.Now + e.StackTrace);
            }
            Debug.WriteLine("exportCsvDataOnlyGasRegistance() : finish " + " duplicate: " + numOfDuplicate + " valid: " + validCount);
        }

        public bool isDataReceived()
        {
            return (dataSetMap.Count > 0);
        }

        public void reset()
        {
            dataSetMap.Clear();
        }

        public Dictionary<string, List<List<double>>> getGasRegLogDataSet()
        {
            // GasRegistanceの対数値を（カテゴリごとに詰めて）応答する
            Dictionary<string, List<List<double>>> data = new Dictionary<string, List<List<double>>>();
            try
            {
                foreach (KeyValuePair<string, Bme688DataSetGroup> item in dataSetMap)
                {
                    string category = item.Key;
                    List <List<double>> outputData = new List<List<double>>();
                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if (collectedData.lack_data == 0)
                        {
                            // データが欠損していない場合、データを詰める
                            List<double> collected = new List<double>();
                            for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                            {
                                try
                                {
                                    Bme688Data collectedValue = collectedData.getBme688Data(dataIndex);
                                    collected.Add(collectedValue.gas_registance_log);
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(DateTime.Now + " Bme688Data() : " + ee.Message + " ");
                                }
                            }
                            outputData.Add(collected);
                        }
                    }
                    data[category] = outputData;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
            return (data);
        }

        public Dictionary<string, List<List<GraphDataValue>>> getGasRegDataSet()
        {
            // GasRegistanceの値を（カテゴリごとに詰めて）応答する
            Dictionary<string, List<List<GraphDataValue>>> data = new Dictionary<string, List<List<GraphDataValue>>>();
            try
            {
                foreach (KeyValuePair<string, Bme688DataSetGroup> item in dataSetMap)
                {
                    string category = item.Key;
                    List<List<GraphDataValue>> outputData = new List<List<GraphDataValue>>();
                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if (collectedData.lack_data == 0)
                        {
                            // データが欠損していない場合、データを詰める
                            List<GraphDataValue> collected = new List<GraphDataValue>();
                            for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                            {
                                try
                                {
                                    Bme688Data collectedValue = collectedData.getBme688Data(dataIndex);
                                    collected.Add(new GraphDataValue(collectedValue.gas_registance, collectedValue.gas_registance_log, collectedValue.pressure, collectedValue.temperature, collectedValue.humidity));
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(DateTime.Now + " Bme688Data() : " + ee.Message + " ");
                                }
                            }
                            outputData.Add(collected);
                        }
                    }
                    data[category] = outputData;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
            return (data);
        }



        public List<string> getCollectedCategoryList()
        {
            List<string> categoryList = new List<string>();
            foreach (KeyValuePair<string, Bme688DataSetGroup> item in dataSetMap)
            {
                categoryList.Add(item.Key);
            }
            return (categoryList);
        }

        public int getDataIndexCount()
        {
            return (NUMBER_OF_INDEX);
        }

        public void dataEntryNotify(bool isSuccess, string message)
        {
            try
            {
                if ((!isSuccess) || ((message != null) && (message.Length > 0)))
                {
                    callback.messageCallback(" Entry:[" + isSuccess + "] " + message + "\r\n");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " dataEntryNotify(" + isSuccess + ") : " + e.Message);
            }
        }
    }
}
