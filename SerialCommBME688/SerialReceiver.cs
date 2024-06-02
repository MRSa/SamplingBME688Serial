using SamplingBME688Serial;
using System.Diagnostics;

namespace SerialCommBME688
{
    class SerialReceiver
    {
        private int sensorId;
        private Thread? readThread = null;  // new Thread(ReadSerial);
        private System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
        private SerialDataParser dataParser;
        private bool _continue = true;
        private string dataCategory = "";
        private string sendUrl = "";
        private bool isDbEntrySingle = false;

        public SerialReceiver(int sensorId, IDataReceiveNotify notify)
        {
            this.sensorId = sensorId;
            this.dataParser = new SerialDataParser(sensorId, notify);
        }

        public IDataHolder getDataHolder()
        {
            return (dataParser.getDataHolder());
        }

        public bool startReceive(string comPort, string aDataCategory, string aUrl, bool isDbEntrySingle, TextBox aOutputArea)
        {
            try
            {
                dataParser.setOutputArea(aOutputArea);

                //aOutputArea.Text = "";
                this.isDbEntrySingle = isDbEntrySingle;

                /*****/
                mySerialPort.BaudRate = 115200;
                mySerialPort.Parity = System.IO.Ports.Parity.None;
                mySerialPort.DataBits = 8;
                mySerialPort.StopBits = System.IO.Ports.StopBits.One;
                mySerialPort.Handshake = System.IO.Ports.Handshake.None;
                mySerialPort.PortName = comPort;
                mySerialPort.Open();
                /*****/

                if (aDataCategory.Length > 0)
                {
                    dataCategory = aDataCategory;
                }
                else
                {
                    dataCategory = "empty";
                }
                if (aUrl.Length > 0)
                {
                    sendUrl = aUrl;
                }
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
                    string message = "=== Open ERROR (" + comPort + ") === : " + DateTime.Now + "\r\n";
                    message = message + " " + e.Message + "\r\n" + e.StackTrace;

                    aOutputArea.Text = message;
                }
            }
            return (false);
        }

        public bool stopReceive()
        {
            _continue = false;
            dataParser.finishReceivedData();
            return (true);
        }

        public void ReadSerial()
        {
            Debug.WriteLine("  ----- START ReadSerial() ----- : " + DateTime.Now);
            while (_continue)
            {
                try
                {
                    string message = mySerialPort.ReadLine();
                    dataParser.parseReceivedData(dataCategory, sendUrl, isDbEntrySingle, message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " ReadSerial(" + dataCategory + " " + sendUrl + ", " + isDbEntrySingle + ") : " + e.Message);
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
            Debug.WriteLine("  ----- FINISH ReadSerial() ----- : " + DateTime.Now);
        }

        public void importSingleData(string categoryName, int gas_index, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
        {
            dataParser.importSingleData(categoryName, gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);

        }

        public void startExportAllDataToCsv(Stream myStream, bool isWriteHeader, int startPosition, int endPosition)
        {
            try
            {
                // このままだと固まるはずなので、本当はここでコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();
                dataParser.exportCsvData(myStream, isWriteHeader, startPosition, endPosition);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportAllDataToCsv(" + isWriteHeader + ") : " + e.Message);
            }
        }

/*
        public int getReceivedCount()
        {
            return (0);
        }
*/

        public void startExportCsvOnlyGasRegistance(StreamWriter writer, List<string> categoryList, int validCount, int numOfDuplicate)
        {
            try
            {
                // このままだと固まるはずなので、本当はここでコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();

                // 収集した対数データをCSVファイルに出力する
                dataParser.exportCsvDataOnlyGasRegistance(writer, categoryList, validCount, numOfDuplicate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportCsvOnlyGasRegistance() : " + e.Message);
            }
        }


        public void startExportCsvCombine(Stream myStream, int validCount, int numOfDuplicate)
        {
            try
            {
                 // このままだと固まるはずなので、本当はここでコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();

                // 収集した対数データをCSVファイルに出力する
                int duplicateCount = (numOfDuplicate <= 0) ? 1 : numOfDuplicate;
                for (int count = 0; count < duplicateCount; count++)
                {
                    // GasRegistance の 対数データのみ出力する
                    //dataParser.exportCsvDataOnlyGasRegistance(myStream, validCount, count);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportCsv() : " + e.Message);
            }
        }

        public bool isDataReceived()
        {
            return (dataParser.isDataReceived());
        }

        public void reset()
        {
            // データをリセットする
            dataParser.reset();
        }

        public Dictionary<string, List<List<double>>> getGasRegLogDataSet()
        {
            return (dataParser.getGasRegLogDataSet());
        }

        public Dictionary<string, List<List<GraphDataValue>>> getGasRegDataSet()
        {
            return (dataParser.getGasRegDataSet());
        }


        public List<string> getCollectedCategoryList()
        {
            return (dataParser.getCollectedCategoryList());

        }
        public int getDataIndexCount()
        {
            return (dataParser.getDataIndexCount());
        }

    }
}
