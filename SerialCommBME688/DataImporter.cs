using Microsoft.VisualBasic.FileIO;
using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    internal class DataImporter
    {
        private String fileName;
        private SerialReceiver receiver1;
        private SerialReceiver receiver2;
        private IDataImportCallback callback;
        private Stream myStream;

        public DataImporter(SerialReceiver receiver1, SerialReceiver receiver2, IDataImportCallback callback, OpenFileDialog fileDialog)
        {
            this.receiver1 = receiver1;
            this.receiver2 = receiver2;
            this.callback = callback;
            this.myStream = Stream.Synchronized(fileDialog.OpenFile());
            this.fileName = fileDialog.FileName;
        }


        public void importDataFromCsv()
        {
            try
            {
                int readLines = 0;
                using (TextFieldParser csvParser = new TextFieldParser(this.myStream))
                {
                    csvParser.SetDelimiters(",");
                    while (!csvParser.EndOfData)
                    {
                        try
                        {
                            String[] values = csvParser.ReadFields();
                            if ((readLines > 0) && (values != null))
                            {
                                int sensorId = parseIntFromString(values[0]);
                                String categoryName = values[1];
                                int gas_index = parseIntFromString(values[2]);
                                double temperature = parseDoubleFromString(values[3]);
                                double humidity = parseDoubleFromString(values[4]);
                                double pressure = parseDoubleFromString(values[5]);
                                double gas_registance = parseDoubleFromString(values[6]);
                                double gas_registance_log = parseDoubleFromString(values[7]);
                                double gas_registance_diff = parseDoubleFromString(values[8]);

                                if (sensorId == 1)
                                {
                                    receiver1.importSingleData(categoryName, gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                                }
                                else
                                {
                                    receiver2.importSingleData(categoryName, gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);

                                }
                            }
                        }
                        catch (Exception ee) { }
                        callback.dataImportProgress(readLines, -1);
                        readLines++;
                    }
                }

                Debug.WriteLine(DateTime.Now + " ----- importDataFromCsv(): Read " + readLines + " lines. -----");

                // 受信終了を呼び出す
                receiver1.stopReceive();
                receiver2.stopReceive();
                callback.dataImportFinished(true, "Import success : " + fileName + "\r\n Read " + readLines + " lines.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " importDataFromCsv(): Read ::" + ex.Message);
                callback.dataImportFinished(false, "Import error : " + ex.Message);
            }
        }

        private double parseDoubleFromString(String data)
        {
            double value = 0.0d;
            try
            {
                value = Double.Parse(data);
            }
            catch (Exception e)
            {
                // parse失敗
            }
            return (value);
        }

        private int parseIntFromString(String data)
        {
            int value = 0;
            try
            {
                value = int.Parse(data);
            }
            catch (Exception e)
            {
                // parse失敗
            }
            return (value);
        }
    }
}
