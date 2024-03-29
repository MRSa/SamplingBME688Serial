﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    internal class DbEntryStatusView
    {
        private DataTable sensorDataList = new DataTable();
        private DbStatusDialog? dbStatusDialog = null;
        private System.Windows.Forms.Form parentForm;

        public DbEntryStatusView(System.Windows.Forms.Form parent)
        {
            this.parentForm = parent;
        }

        public void showDbEntryStatus(String listUrl)
        {
            try
            {
                // センサ登録情報を取得する
                getSensorDataList(listUrl);

                // ダイアログを表示する、重複してダイアログの表示はしないようにする
                if ((dbStatusDialog == null) || (dbStatusDialog.IsDisposed))
                {
                    dbStatusDialog = new DbStatusDialog(sensorDataList);
                }
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

        private async void getSensorDataList(String listUrl)
        {
            //  センサの登録情報を取得して、表示用に加工する（DataTableに入れる）
            try
            {
                var result = await new HttpClient().GetAsync(listUrl, HttpCompletionOption.ResponseHeadersRead);
                String response = await result.Content.ReadAsStringAsync();

                // 応答に合わせてデータを設定する
                sensorDataList = new DataTable();
                sensorDataList.Clear();
                sensorDataList.Columns.Add("category");
                sensorDataList.Columns.Add("sensor_id");
                sensorDataList.Columns.Add("count");

                try
                {
                    // Debug.WriteLine(DateTime.Now + " getSensorDataList() : " + response);
                    var dataList = JsonSerializer.Deserialize<SensorDataResult>(response);
                    if ((dataList != null) && (dataList.result != null))
                    {
                        foreach (SensorData data in dataList.result)
                        {
                            // データが登録されている分だけ GridViewに入れる
                            sensorDataList.Rows.Add(data.category, data.sensor_id, data.count);
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

        private class SensorDataResult
        {
            public SensorData[]? result { get; set; }
        }

        private class SensorData
        {
            public String? category { get; set; }
            public int sensor_id { get; set; }
            public int count { get; set; }
        }

    }
}
