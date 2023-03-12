#
#   BME688で収集した匂いのデータをモデルにする
#      - 引数 : 生成する匂いデータのモデルファイル名
#      - オプション１(--url)       :  センサデータを取得するURL (初期値： http://localhost:3010/)
#      - オプション２(--epoch)     :  エポック数
#      - オプション３(--duplicate) :  データ増殖回数
#      - オプション４(--offset)    :  データ取得のオフセット
#      - オプション５(--count)     :  取得するデータ数
#      - オプション６(--sensor)    :  モデル生成に利用するセンサID
#      - オプション７(--labels)    :  モデル生成に利用するデータのカテゴリ
#      - オプション８(--range)     :  モデル生成に利用するデータのレンジ（データ正規化で利用）
#      - オプション９(--entry)     :  生成したモデルをデータベースに格納する（拡張予定、現在機能なし）
#
#      - 出力１ : モデルファイル(HDF5形式, xxx.h5)
#      - 出力２ : モデルのカテゴリファイル (CSV形式, xxx_category.txt)
#          ※ xxx は、引数１で指定したファイルの名前
#
import pandas as pd
import numpy as np
import sys
import os
import argparse
import requests, json
from keras.models import Sequential
from keras.layers import Dense, LSTM
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics import mean_squared_error

#  ------  デフォルトパラメータ群
nofIndex = 10         #
n_epochs = 500        #
n_input  = 1          # 
n_hidden = nofIndex   #
n_batch  = 32         # 

data_duplicate = 1
data_offset = 0
data_count = -1
sensorId = -1
maxRange = 20.0
url_to_get = "http://localhost:3010/"
categories_opt = lambda x:list(map(str, x.split(',')))
data_labels = []
db_entry = False

#  ------  コマンドライン引数の解析...
parser = argparse.ArgumentParser(description='Create Smell Model')
parser.add_argument('modelFile', help='Model file name to create')                         # 生成するモデルファイル名
parser.add_argument("-u", "--url", type=str, help='URL to get sensor data')                # データを取得するURL
parser.add_argument("-e", "--epoch", type=int, help="number of epochs")                    # エポック数
parser.add_argument("-d", "--duplicate", type=int, help="number of data duplicate times")  # データ増殖回数
parser.add_argument("-o", "--offset", type=int, help="data start offset")                  # データ取得オフセット
parser.add_argument("-c", "--count", type=int, help="data count to get")                   # モデル作成に使用するデータ数
parser.add_argument("-s", "--sensor", type=int, help="sensor Id")                          # モデル作成に使用するセンサID
parser.add_argument("-l", "--labels", type=categories_opt, help="data labels to get")      # モデル作成に使用するデータカテゴリ
parser.add_argument("-r", "--range", type=float, help="max data range")                    # データのレンジ (正規化で使用する)
#parser.add_argument("--entry", action='store_true', help="Store model in database")        # 作成したモデルをデータベースに格納する

args =  parser.parse_args()
model_to_create = args.modelFile
if args.url:
    url_to_get = args.url
if args.epoch:
    n_epochs = args.epoch
if args.duplicate:
    data_duplicate = args.duplicate
if args.offset:
    data_offset = args.offset
if args.sensor:
    sensorId = args.sensor
if args.count:
    data_count = args.count
if args.range:
    maxRange = args.range
if args.labels:
    data_labels = args.labels

# 出力するファイル名を生成
h5_filename = os.path.splitext(os.path.basename(model_to_create))[0] + ".h5"
category_filename = os.path.splitext(os.path.basename(model_to_create))[0] + "_category.txt"

# データを取得するURLを生成
url_list = url_to_get + "sensor/list"
url_getdata = url_to_get + "sensor/get"

#  ----- 設定したパラメータ値を表示する
print("----------")
print("URL to get : {0}".format(url_to_get))
print("Model file name to create : {0}".format(model_to_create))
print("number of epochs : {0}".format(n_epochs))
print("number of data duplicate times : {0}".format(data_duplicate))
print("data start offset : {0}".format(data_offset))
print("data count : {0}".format(data_count))
print("sensorId : {0}".format(sensorId))
print("range : {0}".format(maxRange))
print("data labels: {0} {1}".format(len(data_labels), data_labels))
#if args.entry:
#    print("Store model in database : Yes")
#    db_entry = True
print("----------")
print(" ")
print("- - - - - - - - - -")
print('Create model: {0} -> {1} ({2}) [index:{3}]'.format(url_to_get, h5_filename, category_filename, nofIndex))

#  ----- 登録されているセンサデータを取得
data_list = (json.loads(requests.get(url_list).text)).get("result")
is_first = True
number_of_categories = 0
minimum_data_count = 0
for data in data_list:
    category = data.get("category")
    sensor_id = data.get("sensor_id")
    count = data.get("count")
    if sensorId <= 0 or sensor_id == sensorId:
        if len(data_labels) == 0 or category in data_labels:
            # ====- モデルを作成するカテゴリの数 および 最小データ数を決定する
            if is_first == True:
                minimum_data_count = count
                is_first = False
            if minimum_data_count > count:
                minimum_data_count = count
            number_of_categories = number_of_categories + 1
            #print("category: {0} sensor Id: {1}  count: {2}".format(category, sensor_id, count))

#  データオフセット分、最小件数を引いておく
minimum_data_count = minimum_data_count - data_offset

if data_count > 0 and data_count < minimum_data_count:
    # オプションでデータ取得の上限値が指定されていたら、その値に更新する
    minimum_data_count = data_count

# --- モデルデータのサマリを表示
print("Data Count: {0} (Offset: {1}) Duplicate: {2}  Category: {3} ".format(minimum_data_count, data_offset, data_duplicate, number_of_categories))
print("- - - - - - - - - -")
# ------------- 読み込んだ試料数（カテゴリの数）を取得する
data_interval = 1.0 / (number_of_categories - 1)


# -------------  データベースからガス抵抗値を読み込む
category_array = []
sensor_id_array = []
sensor_data_array = []

for data in data_list:
    category = data.get("category")
    sensor_id = data.get("sensor_id")
    if sensorId <= 0 or sensor_id == sensorId:
        if len(data_labels) == 0 or category in data_labels:
           # -------------  センサIDが未指定、指定されていたセンサIDだった場合、実データを取得する
            url_to_get = url_getdata + "?category=" + category + "&sensor_id=" + str(sensor_id) + "&limit=" + str(minimum_data_count) + "&offset=" + str(data_offset)
            collected_list = (json.loads(requests.get(url_to_get).text)).get("result")
            value_list = collected_list.get("data")
            isStartIndex = True
            value_array = []
            for sensor_value in value_list:
                index = sensor_value.get("index")
                value = sensor_value.get("value")
                if isStartIndex == False and index == 0:
                    isStartIndex = True
                if isStartIndex == True:
                    value_array.append(value)
            key = str(sensor_id) + "-" + category

            arrayIndexCount = divmod(len(value_array), nofIndex)
            category_array.append(category)
            sensor_id_array.append(sensor_id)
            data_value_array = []
            for i in range(data_duplicate):
                data_value_array = data_value_array + value_array[0:(arrayIndexCount[0] + 1) * nofIndex]   # arrayのサイズをindexの倍数に調整する
            sensor_data_array.append(data_value_array)
            print("Sensor Id:{0} Count:{1} Category: {2}".format(sensor_id, len(data_value_array), category))
print("--------------------")
print(" ")
            
# ------------- トレーニングモデルの教師データを作る
dataX = []
dataY = []

# ------------- データをモデル生成用に加工
dataYvalue = 0.0 - data_interval
for list_data_array in sensor_data_array:
    dataYvalue = dataYvalue + data_interval
    dataXX = []
    indexCount = 0
    for sensor_data_value in list_data_array:
        dataXX.append(sensor_data_value / maxRange)
        indexCount = indexCount + 1
        if indexCount == nofIndex:
            dataX.append(dataXX)
            dataY.append(dataYvalue)
            #print("count: {0}  sensor_data_value: {1}".format(indexCount, dataXX))
            dataXX = []          
            indexCount = 0

#  ---------------------- データを整形する
trainX = []
length = 0
for i in range(len(dataX)):
    value = dataX[i]
    if length == 0:
        length = len(value)
    if length != len(value):
        value = value[0:length]
    trainX.append(value)

trainX = np.array(trainX)
trainX.reshape(trainX.shape[0], 1, trainX.shape[1])
dataY = np.array(dataY)

# ----- LSTMネットワーク(モデル)を構築する
model = Sequential()
model.add(LSTM(n_hidden, batch_input_shape=(None, length, n_input), return_sequences=False))
model.add(Dense(n_input))
model.compile(loss='mean_squared_error', optimizer='adam')
model.fit(trainX, dataY, epochs=n_epochs, batch_size=n_batch)

# ----- モデルの情報を表示する
model.summary()

# ----- できたモデルをファイルに保存する
model.save(h5_filename)

# ----- モデルのデータ情報についての値を出力する
result_number = 0.0
with open(category_filename,"w") as output_file:
    print("label,value,;", file = output_file)
    for category in category_array:
        print("{0},{1:.5f},;".format(category, result_number), file = output_file)
        result_number = result_number + data_interval
