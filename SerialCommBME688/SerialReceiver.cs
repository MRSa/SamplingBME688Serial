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
        private static System.IO.Ports.SerialPort mySerialPort = new System.IO.Ports.SerialPort(new System.ComponentModel.Container());
        private static SerialDataParser dataParser = new SerialDataParser();
        private DataTable dataSource = new DataTable("dataSummary");
        private Thread? readThread = null;  // new Thread(ReadSerial);
        private static bool _continue = true;
        private static String dataCategory = "";

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
            updateDataTable();

            return (true);
        }

        public static void ReadSerial()
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


        public void startExportCsv(Stream myStream)
        {
            try
            {
                // 収集データをCSVファイルに出力する
                dataParser.exportCsvData(myStream);

                // 固まるので、本当はコンテキストを切りたい...
                //Thread writeThread = new Thread(exportCsvData);
                //writeThread.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " startExportCsv() : " + e.Message);
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
                Dictionary<String, Bme688DataSetGroup> datamap = dataParser.getDataMap();
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
                                        gas_registance_min);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " updateDataTable() : " + e.Message);
            }
        }

    }
}
