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

        private SensorDataDetail[] detailArray;
        private int detailEntryCount;
        private int sensorId;
        private int entryIndex;

        public PostJsonData(int sensorId, int maxIndex, IDataEntryNotify notify)
        {
            this.sensorId = sensorId;
            this.maxIndexNumber = maxIndex;
            this.notifyCallback = notify;
            this.detailArray = new SensorDataDetail[maxIndex];
            this.detailEntryCount = 0;
            this.entryIndex = 0;
        }

        public void receivedData(
            String sendUrl,
            bool isSingleEntry,
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
            // データを受信したら毎回DBに格納するか、まとめて格納するかの分岐
            if (isSingleEntry)
            {
                // 受信したら毎回データベースに登録する場合...
                receivedDataSingle(sendUrl, category, sensorId, gas_index, meas_index, serial_number, data_status, gas_wait, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
            }
            else
            {
                // データをまとめて登録する場合...
                receivedDataBatch(sendUrl, category, sensorId, gas_index, meas_index, serial_number, data_status, gas_wait, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
            }
        }
        
        private async void receivedDataSingle(
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
                SensorDataSingle singleData = new SensorDataSingle(sensorId, category, sensorDetail);

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
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson(" + sendUrl + ") -----");
                //Debug.WriteLine(DateTime.Now + "       " + messageToSend + " ");
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson  END -----");
            }
            catch (Exception e)
            {
                // 例外を通知する
                Debug.WriteLine(DateTime.Now + " receivedDataSingle(" + sendUrl + " " + gas_index + ") : " + e.Message);
                notifyCallback.dataEntryNotify(false, "EXCEPTION: " + sendUrl + " (" + gas_index + ") : " + e.Message);
            }
        }

        private void receivedDataBatch(
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
                if ((sensorId != this.sensorId)|| (gas_index < 0) || (gas_index > this.maxIndexNumber))
                {

                    notifyCallback.dataEntryNotify(false, " Wrong id: [" + sensorId + "] (" + gas_index + ") ");
                    return;
                }

                // 初回データ
                if (gas_index == 0)
                {
                    if (entryIndex == this.maxIndexNumber)
                    {
                        // データが溜まっていたら、データベースにデータを登録する
                        SensorDataBatch data = new SensorDataBatch(sensorId, category);
                        for (int index = 0; index < entryIndex; index++)
                        {
                            data.AddDetail(this.detailArray[index]);
                        }

                        // データベースにデータを格納する
                        sendSensorDataBatch(sendUrl, data);
                        notifyCallback.dataEntryNotify(true, "DB ENTRY : [" + sensorId + "] (" + entryIndex + ") ");
                    }
                    else
                    {
                        // データが欠損していた場合はDBに登録しない
                        notifyCallback.dataEntryNotify(false, " W: Lack data : [" + sensorId + "] (" + entryIndex + ") ");
                    }

                    // ここでarrayにデータを格納する
                    this.detailArray[0] = new SensorDataDetail(gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                    entryIndex = 1;
                }
                else
                {
                    if (entryIndex == gas_index)
                    {
                        // ここでarrayにデータを格納する
                        this.detailArray[entryIndex] = new SensorDataDetail(gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                        entryIndex++;
                    }
                    else
                    {
                        // "待ち" 
                        Debug.WriteLine(DateTime.Now + " W: Wrong expected index: [" + sensorId + "] (" + gas_index + ") wait: " + entryIndex);
                    }
                }
                // デバッグ用メッセージ
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::receivedDataBatch(" + sendUrl + ") -----");
                //Debug.WriteLine(DateTime.Now + "       " + messageToSend + " ");
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::receivedDataBatch  END -----");
            }
            catch (Exception e)
            {
                // 例外を通知する
                Debug.WriteLine(DateTime.Now + " receievedDataBatch(" + sendUrl + " " + gas_index + ") : " + e.Message);
                notifyCallback.dataEntryNotify(false, "EXCEPTION: " + sendUrl + " (" + gas_index + ") : " + e.Message);
            }
        }

        private async void sendSensorDataBatch(String sendUrl, SensorDataBatch dataToSend)
        {
            try
            {
                // POSTメソッドで送信する
                HttpClient client = new HttpClient();
                String messageToSend = JsonSerializer.Serialize(dataToSend);
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
                    notifyCallback.dataEntryNotify(false, "DB ENTRY NG: " + response.StatusCode);
                }

                // デバッグ用メッセージ
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson(" + sendUrl + ") -----");
                //Debug.WriteLine(DateTime.Now + "       " + messageToSend + " ");
                //Debug.WriteLine(DateTime.Now + " ----- PostJsonData::postJson  END -----");
            }
            catch (Exception e)
            {
                // 例外を通知する
                Debug.WriteLine(DateTime.Now + " sendSensorDataBatch(" + sendUrl + " : " + e.Message);
                notifyCallback.dataEntryNotify(false, "EXCEPTION: " + sendUrl + " : " + e.Message);
            }
        }

        class SensorDataSingle
        {
            public String name { get; }
            public int sensor_id { get; }
            public String category { get; }

            [JsonPropertyName("data")]
            public SensorDataDetail detail { get; }

            public SensorDataSingle(int sensor_id, string category, SensorDataDetail detail)
            {
                this.name = "sensor_data";
                this.sensor_id = sensor_id;
                this.category = category;
                this.detail = detail;
            }
        }
        class SensorDataBatch
        {
            public String name { get; }
            public int sensor_id { get; }
            public String category { get; }

            [JsonPropertyName("data_array")]
            public List<SensorDataDetail> detailArray { get; }

            public SensorDataBatch(int sensor_id, string category)
            {
                this.name = "sensor_data_array";
                this.sensor_id = sensor_id;
                this.category = category;
                this.detailArray = new List<SensorDataDetail>();
            }

            public void AddDetail(SensorDataDetail detail)
            {
                detailArray.Add(detail);
            }
            public void ClearDetail()
            {
                detailArray.Clear();
            }
            public int DetailCount()
            {
                return (detailArray.Count);
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
