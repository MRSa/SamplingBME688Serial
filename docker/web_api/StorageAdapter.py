import datetime
import os
import sys
import SensorData
from sqlalchemy import create_engine, text
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

class StorageAdapter:
    def  __init__(self):
        # ------- 環境変数からDBへアクセスするための情報を取得する
        self.host = os.getenv("SENSOR_DB_ACCESS_HOST")
        self.dbname = os.getenv("SENSOR_DB_NAME")
        self.dbuser = os.getenv("SENSOR_DB_USER")
        self.dbpass = os.getenv("SENSOR_DB_PASS")
        self.dbtable = os.getenv("SENSOR_DB_TABLE")
        self.dboption = os.getenv("SENSOR_DB_OPTION")
        self.db_url = "postgresql+psycopg2://{0}:{1}@{2}/{3}".format(self.dbuser, self.dbpass, self.host, self.dbname)
        self.engine = create_engine(self.db_url)
        self.Session = sessionmaker(bind=self.engine)

    def entry(self, jsonData):
        try:
            session = self.Session()
            self.entryFromJson(jsonData, session)
            session.close()

        except  Exception as e:
            print(" --- Received Exception : {0} {1} ".format(e.args, self.db_url), file=sys.stderr)
            return False

        if self.dboption == "debug":
            # ----- デバッグモードの時は、受信データを標準エラー出力に吐き出す
            print(" ", file=sys.stderr)
            print(" ----- DB ENTRY ----- ", file=sys.stderr)
            print(" parameter : {0}, {1}, {2}, {3} ".format(self.host, self.dbname, self.dbuser, self.dbtable, file=sys.stderr))
            print(" - - - - -", file=sys.stderr)
            print(jsonData, file=sys.stderr)
            print(" - - - - -", file=sys.stderr)
            return True
        return True

    def testDump(self, jsonData):
        try:
            # 接続する
            with self.engine.connect() as con:
                # Select文を実行する
                query = "SELECT * FROM {0};".format(self.dbtable)
                rows = con.execute(text(query))

                # 受信結果を表示する
                print("-=-=-=-=-=-=-=-=-=-=", file=sys.stderr)
                for row in rows:
                    print(row, file=sys.stderr)
                print("-=-=-=-=-=-=-=-=-=-=", file=sys.stderr)
        except  Exception as e:
            print(" --- Received Exception : {0} {1} ".format(e.args, self.db_url), file=sys.stderr)
            return False

        if self.dboption == "debug":
            # ----- デバッグモードの時は、受信データを標準エラー出力に吐き出す
            print(" ", file=sys.stderr)
            print(" ----- DB ENTRY ----- ", file=sys.stderr)
            print(" parameter : {0}, {1}, {2}, {3} ".format(self.host, self.dbname, self.dbuser, self.dbtable, file=sys.stderr))
            print(" - - - - -", file=sys.stderr)
            print(jsonData, file=sys.stderr)
            print(" - - - - -", file=sys.stderr)
            return True
        return True

    def entryFromJson(self, jsonObject, session):
        category = ''
        sensor_id = 0
        try:
            # ----- JSONオブジェクトを解析する
            category = jsonObject.get('category')
            sensor_id = jsonObject.get('sensor_id')
            if (jsonObject.get('name')).lower() == "sensor_data_array":
                # ----- センサデータをまとめて登録する場合
                for sensorData in jsonObject["data_array"]:
                    data = SensorData.SensorData()
                    data.category = category
                    data.sensor_id = sensor_id
                    data.datetime = sensorData["datetime"]
                    data.gas_index = sensorData["gas_index"]
                    data.temperature = sensorData["temperature"]
                    data.humidity = sensorData["humidity"]
                    data.pressure = sensorData["pressure"]
                    data.gas_registance = sensorData["gas_registance"]
                    data.gas_registance_log = sensorData["gas_registance_log"]
                    data.gas_registance_diff = sensorData["gas_registance_diff"]
                    session.add(data)
            else:
                # ----- センサデータを１件登録する場合
                data = SensorData.SensorData()
                sensorData = jsonObject["data"]
                data.category = category
                data.sensor_id = sensor_id
                data.datetime = sensorData["datetime"]
                data.gas_index = sensorData["gas_index"]
                data.temperature = sensorData["temperature"]
                data.humidity = sensorData["humidity"]
                data.pressure = sensorData["pressure"]
                data.gas_registance = sensorData["gas_registance"]
                data.gas_registance_log = sensorData["gas_registance_log"]
                data.gas_registance_diff = sensorData["gas_registance_diff"]
                session.add(data)

            session.commit()
            print(" Category:{0} id:{1}".format(category, sensor_id), file=sys.stderr)
            return

        except  Exception as e:
            print(" === Received Exception : {0} {1} ".format(e.args, jsonObject), file=sys.stderr)
            return    

# --------------------------------------------------------------
accessor = StorageAdapter()

def entryValue(jsonData):
    return (accessor.entry(jsonData))

if __name__ == '__main__':
    accessor = StorageAdapter()
    try:
        print(" ")

    except KeyboardInterrupt:
        pass

'''
<!-- 登録テスト用データ例：ChromeでURLに about:blank を開いて、開発者ツールのコンソールで以下を実行する  -->
var xhr = new XMLHttpRequest();
xhr.open('POST', 'http://localhost:3010/sensor/entry');
xhr.setRequestHeader('Content-Type', 'application/json');
xhr.send(JSON.stringify(
{
    "name": "sensor_data",
    "sensor_id": 0,
    "category": "dummy00",
    "data" : {
        "datetime": "2023-01-01 00:00:00",
        "gas_index": 0,
        "temperature": 26.5,
        "humidity": 26.31,
        "pressure": 101019.08,
        "gas_registance": 6090706.5,
        "gas_registance_log": 15.622274,
        "gas_registance_diff": 11.457
    }
}));
<!--  データ登録例 ここまで  -->
'''
