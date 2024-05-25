using SamplingBME688Serial;
using System.Diagnostics;

namespace SerialCommBME688
{
    class SerialReceiverForAnalysis
    {
        private const int NUMBER_OF_INDEX = 10;
        private const int NUMBER_OF_ITEMS = 13;
        private int sensorId;
        private bool isTagetDataLog = false;
        private TextBox? aOutputArea = null;
        private Thread? readThread = null;  // new Thread(ReadSerial);
        private System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
        private bool _continue = true;
        private double totalPressureValue = 0.0f;
        private double totalTemperatureValue = 0.0f;
        private double totalHumidityValue = 0.0f;
        private IReceivedSmellDataForAnalysis notify;
        private SmellOrData receivedData = new SmellOrData();
        private int waitIndexNumber = 0;

        public SerialReceiverForAnalysis(int sensorId, IReceivedSmellDataForAnalysis notify)
        {
            receivedData.sensorId = sensorId;
            this.sensorId = sensorId;
            this.notify = notify;
        }

        public bool startReceive(string comPort, bool isTagetDataLog, TextBox aOutputArea)
        {
            try
            {
                this.aOutputArea = aOutputArea;
                this.isTagetDataLog = isTagetDataLog;

                /*****/
                mySerialPort.BaudRate = 115200;
                mySerialPort.Parity = System.IO.Ports.Parity.None;
                mySerialPort.DataBits = 8;
                mySerialPort.StopBits = System.IO.Ports.StopBits.One;
                mySerialPort.Handshake = System.IO.Ports.Handshake.None;
                mySerialPort.PortName = comPort;
                mySerialPort.Open();
                /*****/

                _continue = true;
                readThread = new Thread(ReadSerial);
                readThread.Start();
                return (true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                if (aOutputArea != null)
                {
                    string message = "=== Open ERROR (" + comPort + ") [" + sensorId + "] === : " + DateTime.Now + "\r\n";
                    message = message + " " + e.Message + "\r\n" + e.StackTrace;
                    aOutputArea.Text = message;
                }
            }
            return (false);
        }

        public bool stopReceive()
        {
            _continue = false;
            appendText("Done.\r\n");
            return (true);
        }

        public void ReadSerial()
        {
            Debug.WriteLine("  ----- START ReadSerial(" + mySerialPort.PortName + ") ----- : " + DateTime.Now);
            while (_continue)
            {
                try
                {
                    //string message = mySerialPort.ReadLine();
                    parseReceivedData(mySerialPort.ReadLine());

                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " ReadSerial(" + waitIndexNumber + ") : " + e.Message);
                }
            }
            try
            {
                mySerialPort.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " ReadSerial() : Close " + e.Message);
            }
            Debug.WriteLine("  ----- FINISH ReadSerial(" + mySerialPort.PortName + ") ----- : " + DateTime.Now);
        }

        private void parseReceivedData(string data)
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

                if (gas_index == waitIndexNumber)
                {
                    // "待ち"番号を受信した
                    switch (gas_index)
                    {
                        case 0:
                            waitIndexNumber = 1;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;

                            receivedData.sequence0Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 1:
                            waitIndexNumber = 2;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence1Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 2:
                            waitIndexNumber = 3;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence2Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 3:
                            waitIndexNumber = 4;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence3Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 4:
                            waitIndexNumber = 5;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence4Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 5:
                            waitIndexNumber = 6;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence5Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 6:
                            waitIndexNumber = 7;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence6Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 7:
                            waitIndexNumber = 8;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence7Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 8:
                            waitIndexNumber = 9;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence8Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);
                            break;
                        case 9:
                            // データが全部そろった！
                            waitIndexNumber = 0;
                            totalHumidityValue += humidity;
                            totalPressureValue += pressure;
                            totalTemperatureValue += temperature;
                            receivedData.sequence9Value = (float)((isTagetDataLog) ? gas_registance_log : gas_registance);

                            receivedData.averagePressureValue = (float) (totalPressureValue / 10.0f);
                            receivedData.averageTemperatureValue = (float) (totalTemperatureValue / 10.0f);
                            receivedData.averageHumidityValue = (float) (totalHumidityValue / 10.0f);

                            // 受信したデータを報告する
                            appendText("\r\nGOT IT!\r\n");
                            notify.receivedSmellDataForAnalysis(receivedData);

                            totalPressureValue = 0.0f;
                            totalTemperatureValue = 0.0f;
                            totalHumidityValue = 0.0f;
                            break;

                        default:
                            // 想定外の値... 最初のindex番号(0)を待つ
                            waitIndexNumber = 0;
                            totalPressureValue = 0.0f;
                            totalTemperatureValue = 0.0f;
                            totalHumidityValue = 0.0f;
                            break;
                    }
                }
                else
                {
                    // データが途中抜けた...? 最初のindex番号(0)を待つ
                    waitIndexNumber = 0;
                }
                // 待っている数値を出力する
                appendText("" + waitIndexNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " parseReceivedData() : " + ex.Message);
            }
        }

        private void appendText(string itemData)
        {
            try
            {
                if ((aOutputArea != null) && (itemData.Length > 0))
                {
                    if (aOutputArea.InvokeRequired)
                    {
                        Action safeWrite = delegate { appendText(itemData); };
                        aOutputArea.Invoke(safeWrite);
                    }
                    else
                    {
                        aOutputArea.AppendText(itemData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " appendText() : " + e.Message + "  --- " + itemData);
            }
        }
    }
}
