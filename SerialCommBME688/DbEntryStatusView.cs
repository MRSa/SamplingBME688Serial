using SerialCommBME688;
using System.Data;
using System.Diagnostics;
using System.Text.Json;

namespace SamplingBME688Serial
{
    internal class DbEntryStatusView : ILoadDataFromDatabase
    {
        private DataTable sensorDataList = new DataTable();
        private DbStatusDialog? dbStatusDialog = null;
        private Form parentForm;
        private SerialReceiver serialReceiver1;
        private SerialReceiver serialReceiver2;
        private List<LoadSensorDataInformation> categoryToLoad;
        private IDataImportCallback? importCallback = null;
        private string urlToGetData;

        public DbEntryStatusView(Form parent, ref SerialReceiver receiver1, ref SerialReceiver receiver2)
        {
            parentForm = parent;
            serialReceiver1 = receiver1;
            serialReceiver2 = receiver2;
        }

        public void LoadDataFromDatabase(ref List<LoadSensorDataInformation> categoryToLoad)
        {
            // ----- (データベースから)データを読み込む
            try
            {
                this.categoryToLoad = categoryToLoad;
                loadDataFromDatabaseImpl();
                //{
                //    Thread thread = new Thread(new ThreadStart(loadDataFromDatabaseImpl));
                //    thread.IsBackground = true;
                //    thread.Start();
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] LoadDataFromDatabase() " + ex.Message);
            }
        }

        private void loadDataFromDatabaseImpl()
        {
            // ----- (データベースから)データを読み込む本処理 (スレッドで実行)
            try
            {
                bool importResult = false;
                int numberOfCategories = 0;

                // ------------------------------------------------------------------------------------
                foreach (LoadSensorDataInformation info in categoryToLoad)
                {
                    string categoryName = info.dataCategory;
                    int sensorId = info.sensorId;
                    int start = info.startFrom;
                    int count = info.dataCount;
                    int dataToGet = 10000;  // 1回の取得でどれくらいのデータをとってくるかの指定

                    for (int loopCount = start; loopCount < count; loopCount += dataToGet)
                    {
                        string getUrl = urlToGetData + "?category=" + categoryName + "&sensor_id=" + sensorId + "&option=true&offset=" + loopCount + "&limit=" + dataToGet;
                        if (sensorId == 1)
                        {
                            getSensorDataBody(getUrl, serialReceiver1);
                        }
                        else
                        {
                            getSensorDataBody(getUrl, serialReceiver2);
                        }
                        // Debug.WriteLine(DateTime.Now + " [GET DATA] " + categoryName + " (START: " + loopCount + " COUNT: " + dataToGet + ")");
                    }
                    importResult = true;
                    numberOfCategories++;
                }
                // ------------------------------------------------------------------------------------

                // ----- インポート終了を通知する。
                importCallback?.dataImportFinished(importResult, "Data import finished. (" + numberOfCategories + " categories.)");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " [ERROR] loadDataFromDatabaseImpl() " + ex.Message);
            }
        }

        public void showDbEntryStatus(string listUrl)
        {
            // データベースの登録情報を確認する
            databaseOperation(listUrl, false);
        }

        public void getDataFromDatabase(string listUrl, string getUrl, in IDataImportCallback importCallback)
        {
            // ----- データを取得する箇所のURL
            urlToGetData = getUrl;

            // ----- インポート結果を報告するところ
            this.importCallback = importCallback;

            // データベースからデータを取得する
            databaseOperation(listUrl, true);
        }

        private void databaseOperation(string listUrl, bool isLoadMode)
        {
            try
            {
                // センサ登録情報を取得する
                getSensorDataList(listUrl, isLoadMode);

                // ダイアログを表示する、重複してダイアログの表示はしないようにする
                if ((dbStatusDialog == null) || (dbStatusDialog.IsDisposed))
                {
                    dbStatusDialog = new DbStatusDialog(sensorDataList, this);
                }
                dbStatusDialog.setLoadDataMode(isLoadMode, listUrl);
                dbStatusDialog.Owner = parentForm;
                if (!dbStatusDialog.Visible)
                {
                    //dbStatusDialog.Show();
                    dbStatusDialog.ShowDialog();  // モーダルダイアログ
                }
                else
                {
                    dbStatusDialog.Activate();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " showDbEntryStatus() " + ex.Message);
            }
        }

        private async void getSensorDataBody(string getUrl, SerialReceiver? serialReceiver)
        {
            //  センサデータをデータベースからインポートする実処理
            // ----- Debug.WriteLine(DateTime.Now + " getSensorDataBody() : " + getUrl);
            if (serialReceiver == null)
            {
                Debug.WriteLine(DateTime.Now + " getSensorDataBody() SerialReceiver is null...");
                return;
            }
            int count = 0;
            try
            {
                var result = await new HttpClient().GetAsync(getUrl, HttpCompletionOption.ResponseHeadersRead);
                string response = await result.Content.ReadAsStringAsync();

                Debug.WriteLine(DateTime.Now + " [RECV] getSensorDataBody() ");
                try
                {
                    var dataList = JsonSerializer.Deserialize<SensorDataResult>(response);
                    if ((dataList != null) && (dataList.result != null) && (dataList.result.data != null))
                    {
                        foreach (SensorData data in dataList.result.data)
                        {
                            // データが登録されている分だけデータを入れる
                            bool isSuccess = importReceivedData(data, serialReceiver);
                            if (isSuccess)
                            {
                                count++;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " ------ getSensorDataBody() " + e.Message);
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " getSensorDataBody() " + ex.Message);
            }

            // ----- 登録終了
            serialReceiver.stopReceive();
            Debug.WriteLine(DateTime.Now + " getSensorDataBody() data entry: " + count);
        }

        private bool importReceivedData(SensorData data, SerialReceiver serialReceiver)
        {
            // データベースから読み込んだデータを記憶する実処理
            bool ret;
            try
            {
                string categoryName = (data.category == null) ? "empty" : data.category;
                int sensorId = data.sensor_id;
                int gas_index = data.index;
                double temperature = data.temperature;
                double humidity = data.humidity;
                double pressure = data.pressure;
                double gas_registance = data.gas_registance;
                double gas_registance_log = data.gas_registance_log;
                double gas_registance_diff = data.gas_registance_diff;
                serialReceiver.importSingleData(categoryName, gas_index, temperature, humidity, pressure, gas_registance, gas_registance_log, gas_registance_diff);
                ret = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " importReceivedData() " + ex.Message);
                ret = false;
            }
            return ret;
        }

        private async void getSensorDataList(string listUrl, bool isLoadMode)
        {
            //  センサの登録情報を取得して、表示用に加工する（DataTableに入れる）
            try
            {
                var result = await new HttpClient().GetAsync(listUrl, HttpCompletionOption.ResponseHeadersRead);
                string response = await result.Content.ReadAsStringAsync();

                // 応答に合わせてデータを設定する
                sensorDataList = new DataTable();
                sensorDataList.Clear();
                if (isLoadMode)
                {
                    sensorDataList.Columns.Add(new DataColumn("Load", typeof(bool)));
                }
                sensorDataList.Columns.Add("category");
                sensorDataList.Columns.Add("sensor_id");
                sensorDataList.Columns.Add("count");

                try
                {
                    // Debug.WriteLine(DateTime.Now + " getSensorDataList() : " + response);
                    var dataList = JsonSerializer.Deserialize<SensorDataListResult>(response);
                    if ((dataList != null) && (dataList.result != null))
                    {
                        foreach (SensorDataSummary data in dataList.result)
                        {
                            // データが登録されている分だけ GridViewに入れる
                            if (isLoadMode)
                            {
                                sensorDataList.Rows.Add(true, data.category, data.sensor_id, data.count);
                                //sensorDataList.Rows.Add(data.category, data.sensor_id, data.count);
                            }
                            else
                            {
                                sensorDataList.Rows.Add(data.category, data.sensor_id, data.count);
                            }
                        }
                    }
                    if (dbStatusDialog != null)
                    {
                        dbStatusDialog.setDataTable(sensorDataList);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(DateTime.Now + " getSensorDataList() " + e.Message);
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DateTime.Now + " getSensorDataList() " + ex.Message);
            }
        }

        private class SensorDataListResult
        {
            public SensorDataSummary[]? result { get; set; }
        }

        private class SensorDataSummary
        {
            public string? category { get; set; }
            public int sensor_id { get; set; }
            public int count { get; set; }
        }

        private class SensorDataResult
        {
            public SensorDataList? result { get; set; }
        }

        private class SensorDataList
        {
            public string? category { get; set; }
            public string? sensor_id { get; set; }
            public SensorData[]? data { get; set; }
        }

        private class SensorData
        {
            public string? category { get; set; }
            public string? comment { get; set; }
            public string? datetime { get; set; }
            public double gas_registance { get; set; }
            public double gas_registance_diff { get; set; }
            public double gas_registance_log { get; set; }
            public double humidity { get; set; }
            public int index { get; set; }
            public double pressure { get; set; }
            public int sensor_id { get; set; }
            public int serial { get; set; }
            public double temperature { get; set; }
        }
    }
}
