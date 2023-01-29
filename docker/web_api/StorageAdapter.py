import datetime
import os
import sys
import SensorData
from sqlalchemy import create_engine, text
from sqlalchemy.ext.declarative import declarative_base


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

        # -------- センサ情報クラス
        self.sensorData = SensorData.SensorData()
        #self.sensorData = SensorData()

    def entry(self, jsonData):
        try:
            self.sensorData.parseJson(jsonData)

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
