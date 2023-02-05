import os
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

#  -------------------------------------------------------
#   　センサデータの型定義
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
#        gas_registance_diff REAL NOT NULL,
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
