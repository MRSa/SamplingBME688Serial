using SamplingBME688Serial;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SerialCommBME688
{
    interface IMessageCallback
    {
        void messageCallback(String message);
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

        public void setOutputArea(TextBox aOutputArea)
        {
            outputArea = aOutputArea;
        }

        public void parseReceivedData(String categoryName, String sendUrl, bool isSingleEntry, String data)
        {
            //  データを受信したとき...
            try
            {
                // 受信データをカンマで切り出し
                String[] dataValues = data.Split(',');
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
                String itemData = dataHolder.entryData(categoryName,
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

        private void appendText(String itemData)
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
                String itemData = dataHolder.finishReceivedData();
                appendText(itemData);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " finishReceivedData() : " + e.Message);
            }
        }

        public void exportCsvData(Stream myStream, bool isWriteHeader)
        {
            try
            {
                appendText("----- exportCsvData(" + isWriteHeader + ") : START...\r\n");
                dataHolder.exportCsvData(myStream, isWriteHeader);
                appendText("----- exportCsvData(" + isWriteHeader + ") : FINISHED.\r\n");
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " exportCsvData(" + isWriteHeader + ") : " + e.Message);
            }
        }

        public void exportCsvDataOnlyGasRegistance(StreamWriter writer, List<String> categoryList, int validCount, int numOfDuplicate)
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

        public void messageCallback(String message)
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

        public Dictionary<String, List<List<double>>> getGasRegLogDataSet()
        {
            return (dataHolder.getGasRegLogDataSet());
        }

        public List<String> getCollectedCategoryList()
        {
            return (dataHolder.getCollectedCategoryList());

        }

        public int getDataIndexCount()
        {
            return (dataHolder.getDataIndexCount());
        }
    }
}
