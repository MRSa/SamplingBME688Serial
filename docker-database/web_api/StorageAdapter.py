import datetime
import os
import sys
import SensorData
from sqlalchemy import create_engine, text, distinct
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.ext.orderinglist import ordering_list
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

    def dataList(self):
        response = ""
        try:
            session = self.Session()
            response = self.listOfDataCount(session)
            session.close()

        except Exception as e:
            print(" -=- Received Exception : {0} {1} ".format(e.args, self.db_url), file=sys.stderr)
        
        return response

    def listOfDataCount(self, session):
        data = ''
        category_result = []
        sensor_id_result = []
        entry_count = []
        try:
            # ---------------
            categories = session.query(distinct(SensorData.SensorData.category)).all()
            for category in categories:
                #print("{0}  {1}  {2}".format(category, category[0], count), file=sys.stderr)
                try:
                    #print(" --- {0} {1} --- ".format(category[0], category), file=sys.stderr)
                    category_result.append(category[0])
                except Exception as ex:
                    print(" XXX Received Exception : {0} {1} ".format(ex.args, category), file=sys.stderr)
            # ---------------
            sensors = session.query(distinct(SensorData.SensorData.sensor_id)).all()
            for sensor in sensors:
                try:
                    #print(" --- {0} {1} --- ".format(sensor[0], sensor), file=sys.stderr)
                    sensor_id_result.append(sensor[0])
                except Exception as ex:
                    print(" xXx Received Exception : {0} {1} ".format(ex.args, category), file=sys.stderr)
            # ---------------
            for category in category_result:
                for sensor_id in sensor_id_result:
                    try:
                        dataCount = session.query(SensorData.SensorData).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).count()
                        print("  {0}({1}) : {2} ".format(category, sensor_id, dataCount), file=sys.stderr)
                        entry_count.append(dict(category = category, sensor_id = sensor_id, count = dataCount))
                    except Exception as ex:
                        print(" XxX Received Exception : {0} {1} ".format(ex.args, category), file=sys.stderr)

            data = entry_count # 'OK' # categories
            #print(category_result, file=sys.stderr)
        except Exception as e:
            print(" xxx Received Exception : {0} {1} ".format(e.args, " "), file=sys.stderr)

        return data

    def getSensorData(self, category, sensor_id, limit, offset, option):
        response = ''
        try:
            session = self.Session()
            if len(option) > 0:
                # ----- DBからデータをとってきて応答する（オプション指定あり）
                response = self.getDataFromDatabaseWithOption(session, category, sensor_id, limit, offset, option)
            else:
                # ----- DBからデータをとってきて応答する（オプション指定なし）
                response = self.getDataFromDatabase(session, category, sensor_id, limit, offset)
            session.close()
        except Exception as e:
            print(" === Received Exception : {0} {1} ".format(e.args, " "), file=sys.stderr)
        return response


    def getDataFromDatabase(self, session, category, sensor_id, limit, offset):
        data = []
        try:
            print(" Category: {0}, sensor id:{1} limit: {2}, (offset:{3})".format(category, sensor_id, limit, offset), file=sys.stderr)
            #session.query(SensorData.SensorData).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).count()
            if len(limit) > 0 or len(offset) > 0:
                if len(limit) <= 0:
                    # offset だけ指定された
                    results = session.query(SensorData.SensorData.gas_index, SensorData.SensorData.gas_registance_log).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).offset(offset).all()
                elif len(offset) <= 0:
                    # limit だけ指定された
                    results = session.query(SensorData.SensorData.gas_index, SensorData.SensorData.gas_registance_log).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).limit(limit).all()
                else:
                    # limit と offsetの両方が指定された
                    results = session.query(SensorData.SensorData.gas_index, SensorData.SensorData.gas_registance_log).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).limit(limit).offset(offset).all()
            else:
                # limit と offsetの両方とも指定がなかった
                results = session.query(SensorData.SensorData.gas_index, SensorData.SensorData.gas_registance_log).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).all()

            for result in results:
                item = dict(index = result[0], value = result[1])
                data.append(item)
            #print(data, file=sys.stderr)
        except Exception as e:
            print(" ___ Received Exception : {0} {1} ".format(e.args, " "), file=sys.stderr)

        return dict(category = category, sensor_id = sensor_id, data = data)

    def getDataFromDatabaseWithOption(self, session, category, sensor_id, limit, offset, option):
        data = []
        try:
            print(" Category: {0}, sensor id:{1} limit: {2}, (offset:{3}, option:{4})".format(category, sensor_id, limit, offset, option), file=sys.stderr)
            #session.query(SensorData.SensorData).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).count()
            if len(limit) > 0 or len(offset) > 0:
                if len(limit) <= 0:
                    # offset だけ指定された
                    results = session.query(
                        SensorData.SensorData.serial,
                        SensorData.SensorData.datetime,
                        SensorData.SensorData.sensor_id,
                        SensorData.SensorData.category,
                        SensorData.SensorData.gas_index,
                        SensorData.SensorData.temperature,
                        SensorData.SensorData.humidity,
                        SensorData.SensorData.pressure,
                        SensorData.SensorData.gas_registance,
                        SensorData.SensorData.gas_registance_log,
                        SensorData.SensorData.gas_registance_diff,
                        SensorData.SensorData.comment
                        ).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).offset(offset).all()
                elif len(offset) <= 0:
                    # limit だけ指定された
                    results = session.query(
                        SensorData.SensorData.serial,
                        SensorData.SensorData.datetime,
                        SensorData.SensorData.sensor_id,
                        SensorData.SensorData.category,
                        SensorData.SensorData.gas_index,
                        SensorData.SensorData.temperature,
                        SensorData.SensorData.humidity,
                        SensorData.SensorData.pressure,
                        SensorData.SensorData.gas_registance,
                        SensorData.SensorData.gas_registance_log,
                        SensorData.SensorData.gas_registance_diff,
                        SensorData.SensorData.comment
                        ).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).limit(limit).all()
                else:
                    # limit と offsetの両方が指定された
                    results = session.query(
                        SensorData.SensorData.serial,
                        SensorData.SensorData.datetime,
                        SensorData.SensorData.sensor_id,
                        SensorData.SensorData.category,
                        SensorData.SensorData.gas_index,
                        SensorData.SensorData.temperature,
                        SensorData.SensorData.humidity,
                        SensorData.SensorData.pressure,
                        SensorData.SensorData.gas_registance,
                        SensorData.SensorData.gas_registance_log,
                        SensorData.SensorData.gas_registance_diff,
                        SensorData.SensorData.comment
                        ).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).limit(limit).offset(offset).all()
            else:
                # limit と offsetの両方とも指定がなかった
                results = session.query(
                        SensorData.SensorData.serial,
                        SensorData.SensorData.datetime,
                        SensorData.SensorData.sensor_id,
                        SensorData.SensorData.category,
                        SensorData.SensorData.gas_index,
                        SensorData.SensorData.temperature,
                        SensorData.SensorData.humidity,
                        SensorData.SensorData.pressure,
                        SensorData.SensorData.gas_registance,
                        SensorData.SensorData.gas_registance_log,
                        SensorData.SensorData.gas_registance_diff,
                        SensorData.SensorData.comment
                    ).filter(SensorData.SensorData.category == category, SensorData.SensorData.sensor_id == sensor_id).order_by(SensorData.SensorData.serial).all()

            for result in results:
                item = dict(
                    serial = result[0],
                    datetime = result[1],
                    sensor_id = result[2],
                    category = result[3],
                    index = result[4],
                    temperature = result[5],
                    humidity = result[6],
                    pressure = result[7],
                    gas_registance = result[8],
                    gas_registance_log = result[9],
                    gas_registance_diff = result[10],
                    comment = result[11])
                data.append(item)
            #print(data, file=sys.stderr)
        except Exception as e:
            print(" ___ Received Exception : {0} {1} ".format(e.args, " "), file=sys.stderr)

        return dict(category = category, sensor_id = sensor_id, data = data)

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
