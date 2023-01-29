import os
import sys
import json
from sqlalchemy.schema import Column
from sqlalchemy.types import Integer, SmallInteger, String, Float, DateTime
from sqlalchemy.ext.declarative import declarative_base
from alchemy_base import Base
class SensorData(Base):
    __tablename__ = os.getenv("SENSOR_DB_TABLE")

    serial = Column(Integer, primary_key=True)
    datetime = Column('datetime', DateTime)
    sensor_id = Column('sensor_id', SmallInteger)
    category = Column('category', String(128))
    gas_index = Column('gas_index', SmallInteger)
    temperature = Column('temperature', Float(5, False, 3))
    humidity = Column('humidity', Float(5, False, 3))
    pressure = Column('pressure', Float(5, False, 3))
    gas_registance = Column('gas_registance', Float(5, False, 3))
    gas_registance_log = Column('gas_registance_log', Float(5, False, 3))
    gas_registance_diff = Column('gas_registance_diff', Float(5, False, 3))
    comment = Column(String(256))

    def parseJson(self, jsonObject):
        category = ''
        sensor_id = 0
        try:
            # ----- JSONオブジェクトを解析する
            category = jsonObject.get('category')
            sensor_id = jsonObject.get('sensor_id')
            if (jsonObject.get('name')).lower() == "sensor_data_array":
                # ----- センサデータをまとめて登録する場合
                for sensorData in jsonObject["data_array"]:
                    datetime = sensorData["datetime"]
                    gas_index = sensorData["gas_index"]
                    print(" Data: {0} {1}".format(datetime, gas_index), file=sys.stderr)
            
            else:
                # ----- センサデータを１件登録する場合
                sensorData = jsonObject.get('data')
                datetime = sensorData["datetime"]
                gas_index = sensorData["gas_index"]
                print(" Data: {0} {1}".format(datetime, gas_index), file=sys.stderr)
            
            print(" Category:{0} id:{1}".format(category, sensor_id), file=sys.stderr)
            return

        except  Exception as e:
            print(" === Received Exception : {0} {1} ".format(e.args, jsonObject), file=sys.stderr)
            return    

#
#  -------------------------------------------------------
#    CREATE TABLE IF NOT EXISTS $SENSOR_DB_TABLE  (
#        serial SERIAL PRIMARY KEY,
#        datetime TIMESTAMP NOT NULL,
#        sensor_id SMALLINT NOT NULL,
#        category VARCHAR(128) NOT NULL,
#        gas_index SMALLINT NOT NULL,
#        temperature REAL NOT NULL,
#        humidity REAL NOT NULL,
#        pressure REAL NOT NULL,
#        gas_registance REAL NOT NULL,
#        gas_registance_log REAL NOT NULL,
#        gas_registance_log REAL NOT NULL,
#        comment VARCHAR(256)
#    );
#  -------------------------------------------------------
#    INSERT INTO bme688sensor VALUES (
#      0,
#      '2023-01-27 10:00:01',
#      1,
#      'Category1',
#      0,
#      26.5,
#      26.31,
#      101019.08,
#      6090706.5,
#      15.622274,
#      11.457
# );
#  -------------------------------------------------------
#
