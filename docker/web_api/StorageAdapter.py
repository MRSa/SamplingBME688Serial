import datetime
import os
import sys
import json

class StorageAdapter:
    def  __init__(self):
        self.temperature = 0.0
        self.humidity = 0.0
        self.scanDateTime = datetime.datetime.now()

        # ------- 環境変数からDBへアクセスするための情報を取得する
        self.host = os.getenv("SENSOR_DB_ACCESS_HOST")
        self.dbname = os.getenv("SENSOR_DB_NAME")
        self.dbuser = os.getenv("SENSOR_DB_USER")
        self.dbpass = os.getenv("SENSOR_DB_PASS")
        self.dbtable = os.getenv("SENSOR_DB_TABLE")
        self.dboption = os.getenv("SENSOR_DB_OPTION")

    def entry(self, jsonData):
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
