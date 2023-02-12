using SerialCommBME688;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
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

        public async void receivedData(
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

                // POSTメソッドで送信する
                HttpClient client = new HttpClient();
                String messageToSend = JsonSerializer.Serialize(singleData);
                StringContent message = new StringContent(messageToSend, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(sendUrl, message);

                // 応答コードで結果を判断する
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // 登録成功を通知する
                    notifyCallback.dataEntryNotify(true, "");
                }
                else
                {
                    // 登録失敗を通知する
                    notifyCallback.dataEntryNotify(false, "DB ENTRY NG: " + " (" + gas_index + ") : " + response.StatusCode);
                }

                // デバッグ用メッセージ
                Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson(" + sendUrl + ") -----");
                Debug.WriteLine(DateTime.Now + "       " + messageToSend + " ");
                Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson  END -----");
            }
            catch (Exception e)
            {
                // 例外を通知する
                Debug.WriteLine(DateTime.Now + " receivedData(" + sendUrl + " " + gas_index + ") : " + e.Message);
                notifyCallback.dataEntryNotify(false, "EXCEPTION: " + sendUrl + " (" + gas_index + ") : " + e.Message);
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
