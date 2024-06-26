﻿using SamplingBME688Serial;
using System.Diagnostics;

namespace SerialCommBME688
{
    interface IMessageCallback
    {
        void messageCallback(string message);
    }
    internal class SerialDataParser : IMessageCallback
    {
        private const int NUMBER_OF_ITEMS = 13;
        private TextBox? outputArea = null;
        private Bme688DataHolder dataHolder;

        public SerialDataParser(int sensorId, IDataReceiveNotify notify)
        {
            dataHolder = new Bme688DataHolder(this, notify, sensorId);
        }

        public IDataHolder getDataHolder()
        {
            return (dataHolder);
        }

        public void setOutputArea(TextBox aOutputArea)
        {
            outputArea = aOutputArea;
        }

        public void parseReceivedData(string categoryName, string sendUrl, bool isSingleEntry, string data)
        {
            //  データを受信したとき...
            try
            {
                // 受信データをカンマで切り出し
                string[] dataValues = data.Split(',');
                if (dataValues.Length != NUMBER_OF_ITEMS)
                {
                    // データの長さが期待したものではなかった。何もせず抜ける。
                    return;
                }

                // 切り出したデータに意味をつける
                int gas_index, meas_index, data_status, gas_wait;
                Int64 serial_number;
                double temperature, humidity, pressure;
                double gas_registance, gas_registance_log, gas_registance_diff;

                int.TryParse(dataValues[1], out gas_index);                // gas_index
                int.TryParse(dataValues[2], out meas_index);               // meas_index
                Int64.TryParse(dataValues[3], out serial_number);          // serial number
                int.TryParse(dataValues[4], out data_status);              // data_status
                int.TryParse(dataValues[5], out gas_wait);                 // gas_wait
                double.TryParse(dataValues[6], out temperature);           // Temperature
                double.TryParse(dataValues[7], out humidity);              // Humidity
                double.TryParse(dataValues[8], out pressure);              // Pressure
                double.TryParse(dataValues[9], out gas_registance);        // Temperature
                double.TryParse(dataValues[10], out gas_registance_log);   // gas_registance_log
                double.TryParse(dataValues[11], out gas_registance_diff);  // gas_registance_diff

                // データを格納する。
                string itemData = dataHolder.entryData(categoryName,
                                                       sendUrl,
                                                       isSingleEntry,
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

                // 応答時、値が入っていたら表示する
                appendText(itemData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " parseReceivedData() : " + ex.Message);
            }
        }

        public void importSingleData(string categoryName, int gas_index, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            try
            {
                // データを格納する。
                string itemData = dataHolder.entryData(categoryName,
                                                       "",
                                                       false,
                                                       gas_index,
                                                       0,
                                                       0,
                                                       0,
                                                       0,
                                                       temperature,
                                                       humidity,
                                                       pressure,
                                                       gas_registance,
                                                       gas_registance_log,
                                                       gas_registance_diff);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " importData() : " + ex.Message);
            }
        }


        private void appendText(string itemData)
        {
            try
            {
                if ((outputArea != null) && (itemData.Length > 0))
                {
                    if (outputArea.InvokeRequired)
                    {
                        Action safeWrite = delegate { appendText(itemData); };
                        outputArea.Invoke(safeWrite);
                    }
                    else
                    {
                        outputArea.AppendText(itemData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " appendText() : " + e.Message + "  --- " + itemData);
            }
        }

        public void finishReceivedData()
        {
            try
            {
                // 保持クラスに終了を通知する。値が入っていたら表示する。
                string itemData = dataHolder.finishReceivedData();
                appendText(itemData);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " finishReceivedData() : " + e.Message);
            }
        }

        public void exportCsvData(Stream myStream, bool isWriteHeader, int startPosition, int endPosition)
        {
            try
            {
                appendText("----- exportCsvData(" + isWriteHeader + ") : START..." + DateTime.Now + "\r\n");
                dataHolder.exportCsvData(myStream, isWriteHeader, startPosition, endPosition);
                appendText("----- exportCsvData(" + isWriteHeader + ") : FINISHED." + DateTime.Now + "\r\n");
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvData(" + isWriteHeader + ") : " + e.Message);
            }
        }

        public void exportCsvDataOnlyGasRegistance(StreamWriter writer, List<string> categoryList, int validCount, int numOfDuplicate)
        {
            try
            {
                dataHolder.exportCsvDataOnlyGasRegistance(writer, categoryList, validCount, numOfDuplicate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvDataOnlyGasRegistance() : " + e.Message);
            }
        }

        public void messageCallback(string message)
        {
            try
            {
                appendText(message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " messageCallback() : " + e.Message + " " + message);
            }
        }

        public bool isDataReceived()
        {
            return (dataHolder.isDataReceived());
        }

        public void reset()
        {
            // データをリセットする
            dataHolder.reset();
        }

        public Dictionary<string, List<List<double>>> getGasRegLogDataSet()
        {
            return (dataHolder.getGasRegLogDataSet());
        }

        public Dictionary<string, List<List<GraphDataValue>>> getGasRegDataSet()
        {
            return (dataHolder.getGasRegDataSet());
        }

        public List<string> getCollectedCategoryList()
        {
            return (dataHolder.getCollectedCategoryList());

        }

        public int getDataIndexCount()
        {
            return (dataHolder.getDataIndexCount());
        }
    }
}
