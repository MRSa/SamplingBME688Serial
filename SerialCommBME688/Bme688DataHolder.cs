using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialCommBME688
{
    internal class Bme688DataHolder
    {
        private const int NUMBER_OF_INDEX = 10;
        private int expectedIndexNumber = 0;
        private int receivedDataCount = 0;
        private IMessageCallback callback;
        private DataTable dataSource = new DataTable("dataSummary");
        private Dictionary<String, Bme688DataSetGroup> dataSetMap = new Dictionary<String, Bme688DataSetGroup>();

        Bme688DataSetGroup? targetDataSet = null;

        public Bme688DataHolder(IMessageCallback callback)
        {
            this.callback = callback;
        }

        public String entryData(String category,
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
            String result = "";
            try
            {
                // Debug.WriteLine(" entryData() : " + category + " [" + gas_index + "] (" + dataSetMap.Count + ") ");
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
                            result = category + " (" + receivedDataCount + ")\r\n";
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
                    result = " INDEX MISMATCH :" + expectedIndexNumber + " - " + gas_index + "\r\n";
                }

                if (result.Length == 0)
                {
                    result = ".";
                }
                if (targetDataSet != null)
                {
                    // 待っていたデータが来たので、そのまま入れる
                    targetDataSet.setReceivedData(gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                    Debug.WriteLine("ENTRY[" + targetDataSet.dataGroupName + "]  : " + gas_index + " " + temperature + " " + humidity + " " + gas_registance + "");
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
                result = "OCCUR Exception : " + e.Message + " " + "\r\n" + e.StackTrace + "\r\n";
            }
            return (result);
        }

        public String finishReceivedData()
        {
            String result = "";
            try
            {
                if (targetDataSet != null)
                {
                    try
                    {
                        // 受信データを保管する。(MAPに格納されていない場合は)
                        if (!dataSetMap.ContainsKey(targetDataSet.dataGroupName))
                        {
                            dataSetMap.Add(targetDataSet.dataGroupName, targetDataSet);
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
                result = "\r\n-----\r\n";
                foreach (KeyValuePair<String, Bme688DataSetGroup> item in dataSetMap)
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

                updateDataTable();
            }
            catch (Exception ee)
            {
                result = result + " " + ee.Message + "\r\n";
                Debug.WriteLine(ee.Message);
            }
            return (result);
        }

        public void exportCsvData(Stream myStream)
        {
            // callback.messageCallback("   --- exportCsvData() ---\r\n");
            try
            {
                Debug.WriteLine("exportCsvData() : canWrite: " + myStream.CanWrite);

                StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);

                writer.AutoFlush = true;
                writer.WriteLine("; category, index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff");

                foreach (KeyValuePair<String, Bme688DataSetGroup> item in dataSetMap)
                {
                    String category = item.Key;
                    List<Bme688DataSet> dataSet = item.Value.getCollectedDataSet();
                    foreach (Bme688DataSet collectedData in dataSet)
                    {
                        if (collectedData.lack_data == 0)
                        {
                            // データが欠損していない場合、ファイルに出力する
                            for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                            {
                                try
                                {
                                    Bme688Data data = collectedData.getBme688Data(dataIndex);
                                    writer.WriteLine(category + "," + dataIndex + "," + data.temperature + "," + data.humidity + "," + data.pressure + "," + data.gas_registance + "," + data.gas_registance_log + "," + data.gas_registance_diff + ",;");
                                }
                                catch (Exception ee)
                                {
                                    Debug.WriteLine(DateTime.Now + " Bme688Data() : " + ee.Message + " ");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvData() : " + e.Message + " ");

            }
        }


        public void exportCsvDataOnlyGasRegistance(Stream myStream, int count)
        {
            try
            {
                Debug.WriteLine("exportCsvDataOnlyGasRegistance() : canWrite: " + myStream.CanWrite + " count: " + count);

                StreamWriter writer = new StreamWriter(myStream, Encoding.UTF8);
                writer.AutoFlush = true;

                // データのヘッダー部分を出力する
                int categoryCount = 0;
                if (count == 0)
                {
                    writer.Write("; index, ");
                }
                foreach (KeyValuePair<String, Bme688DataSetGroup> item in dataSetMap)
                {
                    if (count == 0)
                    {
                        writer.Write(item.Key + ", ");
                    }
                    categoryCount++;
                }
                if (count == 0)
                {
                    writer.WriteLine(" ;");
                }

                // 次に読み込むインデックス位置を取得する
                List<int> nextElement = new List<int>(categoryCount);
                for (int index = 0; index < categoryCount; index++)
                {
                    nextElement.Add(-1);
                }

                while (true)
                {
                    // 次に読み込むインデックス番号を決める
                    for (int index = 0; index < categoryCount; index++)
                    {
                        do
                        {
                            nextElement[index] = nextElement[index] + 1;
                        } while (dataSetMap.ElementAt(index).Value.getCollectedDataSet().ElementAt(nextElement[index]).lack_data != 0);
                    }

                    // それぞれｎデータを出力する
                    for (int dataIndex = 0; dataIndex < NUMBER_OF_INDEX; dataIndex++)
                    {
                        writer.Write(dataIndex + ", ");
                        for (int index = 0; index < categoryCount; index++)
                        {
                            Bme688DataSet collectedData = dataSetMap.ElementAt(index).Value.getCollectedDataSet().ElementAt(nextElement[index]);
                            Bme688Data data = collectedData.getBme688Data(dataIndex);
                            writer.Write(data.gas_registance_log + ", ");
                        }
                        writer.WriteLine(";"); // 改行
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(" index over ");
                // Debug.WriteLine(DateTime.Now + ex.StackTrace);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvDataOnlyGasRegistance() : " + e.Message + " ");
                Debug.WriteLine(DateTime.Now + e.StackTrace);
            }
        }

        public DataTable getGridDataSource()
        {
            try
            {
                // データグリッドに表示するカラムのヘッダーを設定する
                dataSource.Columns.Add("Category");
                dataSource.Columns.Add("dataCount", Type.GetType("System.Int32"));
                dataSource.Columns.Add("validCount", Type.GetType("System.Int32"));
                dataSource.Columns.Add("Temp. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Temp. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("Humi. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Humi. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("Pres. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("Pres. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR. - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR. - Min", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR.(log) - Max", Type.GetType("System.Double"));
                dataSource.Columns.Add("GasR.(log) - Min", Type.GetType("System.Double"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " getGridDataSource() : " + e.Message);

            }
            return (dataSource);
        }

        private void updateDataTable()
        {
            dataSource.Clear();
            try
            {
                Dictionary<String, Bme688DataSetGroup> datamap = dataSetMap;
                foreach (KeyValuePair<String, Bme688DataSetGroup> item in datamap)
                {
                    String category = item.Key;
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

                    dataSource.Rows.Add(category,
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
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
        }

        public void reset()
        {
            dataSource.Clear();
            dataSetMap.Clear();
        }
    }
}
