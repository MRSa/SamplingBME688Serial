using SamplingBME688Serial;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialCommBME688
{
    internal class SerialReceiver
    {
        private int sensorId;
        private Thread? readThread = null;  // new Thread(ReadSerial);
        private System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
        private SerialDataParser dataParser;
        private bool _continue = true;
        private String dataCategory = "";

        public SerialReceiver(int sensorId, IDataReceiveNotify notify)
        {
            this.sensorId = sensorId;
            this.dataParser = new SerialDataParser(sensorId, notify);
        }


        public bool startReceive(String comPort, String aDataCategory, TextBox aOutputArea)
        {
            try
            {
                dataParser.setOutputArea(aOutputArea);
                //aOutputArea.Text = "";

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
                    String message = "=== Open ERROR (" + comPort + ") === " + "\r\n";
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
            Debug.WriteLine("  ----- START ReadSerial() -----");
            while (_continue)
            {
                try
                {
                    string message = mySerialPort.ReadLine();
                    dataParser.parseReceivedData(dataCategory, message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " ReadSerial() : " + e.Message);
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
            Debug.WriteLine("  ----- FINISH ReadSerial() -----");
        }


        public void startExportAllDataToCsv(Stream myStream, bool isWriteHeader)
        {
            try
            {
                // このままだと固まるはずなので、本当はここでコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();
                dataParser.exportCsvData(myStream, isWriteHeader);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportAllDataToCsv(" + isWriteHeader + ") : " + e.Message);
            }
        }

        public int getReceivedCount()
        {
            return (0);
        }

        public void startExportCsv(Stream myStream, int numOfDuplicate, bool isWriteHeader)
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
                    bool writeHeader = (count == 0) ? isWriteHeader : false;
                    dataParser.exportCsvDataOnlyGasRegistance(myStream, count, writeHeader);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportCsv() : " + e.Message);
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

        //public DataTable getGridDataSource()
        //{
        //    return (dataParser.getGridDataSource());
        //}

        public bool isDataReceived()
        {
            return (dataParser.isDataReceived());
        }

        public void reset()
        {
            // データをリセットする
            dataParser.reset();
        }
    }
}
