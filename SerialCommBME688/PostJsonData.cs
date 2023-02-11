using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SamplingBME688Serial
{
    internal class PostJsonData
    {
        private int maxIndexNumber;
        private IDataEntryNotify notifyCallback;

        public PostJsonData(int maxIndex, IDataEntryNotify notify)
        {
            this.maxIndexNumber = maxIndex;
            this.notifyCallback = notify;
        }

        public void receivedData(
            String sendUrl,
            String category,
            int sensorId,
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
            double gas_registance_diff)
        {
            try
            {
                // 受信したデータを　JSON形式に置き換える...
                SensorDataDetail sensorDetail = new SensorDataDetail(gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                SensorDataSingle singleData = new SensorDataSingle("sensor_data", sensorId, category, sensorDetail);

                // 送り先と送るメッセージを記憶する
                SendDataImplement sendData = new SendDataImplement(sendUrl, JsonSerializer.Serialize(singleData), this.notifyCallback);

                // スレッドを起こしてメッセージを送る
                Thread postThread = new Thread(new ThreadStart(sendData.postJson));
                postThread.Start();
            }
            catch (Exception e)
            {
                Debug.WriteLine(DateTime.Now + " receivedData(" + sendUrl + " " + gas_index + ") : " + e.Message);
            }
        }

        class SendDataImplement
        {
            // POST送信の実処理
            private String urlToSend;
            private String messageToSend;
            private IDataEntryNotify notifyCallback;

            public SendDataImplement(String url, String message, IDataEntryNotify callback)
            {
                this.urlToSend = url;
                this.messageToSend = message;
                this.notifyCallback = callback;
            }

            public void postJson()
            {
                //  データ登録本処理 (URLをたたく)
                try
                {
                    Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson(" + urlToSend + ") -----");
                    Debug.WriteLine(DateTime.Now + "       " + messageToSend + " ");
                    Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson  END -----");

                    notifyCallback.dataEntryNotify(true, messageToSend);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " Exception(" + " " + ") : " + e.Message);
                }
            }

        }

        class SensorDataSingle
        {
            public String name { get; }
            public int sensor_id { get; }
            public String category { get; }

            [JsonPropertyName("data")]
            public SensorDataDetail detail { get; }

            public SensorDataSingle(string name, int sensor_id, string category, SensorDataDetail detail)
            {
                this.name = name;
                this.sensor_id = sensor_id;
                this.category = category;
                this.detail = detail;
            }
        }
        class SensorDataDetail
        {
            public DateTime datetime { get; }
            public int gas_index { get; }
            public double temperature { get; }
            public double humidity { get; }
            public double pressure { get; }
            public double gas_registance { get; }
            public double gas_registance_log { get; }
            public double gas_registance_diff { get; }

            public SensorDataDetail(int gas_index, double temperature, double humidity, double pressure, double gas_registance, double gas_registance_log, double gas_registance_diff)
            {
                this.datetime = DateTime.Now;
                this.gas_index = gas_index;
                this.temperature = temperature;
                this.humidity = humidity;
                this.pressure = pressure;
                this.gas_registance = gas_registance;
                this.gas_registance_log = gas_registance_log;
                this.gas_registance_diff = gas_registance_diff;
            }
        }

    }
}
