#
#   BME688の匂いのデータをモデルにする
#      - 引数１ : 収集した匂いのデータのCSVファイル
#
#      - 出力１ : モデルファイル(HDF5形式, xxx.h5)
#      - 出力２ : モデルのカテゴリファイル (CSV形式, xxx_category.txt)
#          ※ xxx は、引数１で指定したファイルの名前
#
import pandas as pd
import numpy as np
import sys
import os
from keras.models import Sequential
from keras.layers import Dense, LSTM
from sklearn.preprocessing import MinMaxScaler
from sklearn.metrics import mean_squared_error

# -------------   作成用パラメータ
nofIndex = 10       # 
maxRange = 20.0     # 
n_input  = 1        # 
n_hidden = nofIndex # 10
n_epochs = 250      # 
n_batch  = 32       # 

#  ------  引数でCSVファイルを指定する
if __name__ == '__main__':
    args = sys.argv
    if 2 <= len(args):
        csv_filename = args[1]
    else:
        print('Arguments are too short, please specify a CSV file name.')
        quit()

h5_filename = os.path.splitext(os.path.basename(csv_filename))[0] + ".h5"
category_filename = os.path.splitext(os.path.basename(csv_filename))[0] + "_category.txt"

print(' Create model: {0} -> {1} ({2})'.format(csv_filename, h5_filename, category_filename))

# -------------  CSVデータファイルからガス抵抗値を読み込む
smell_data = pd.read_csv(csv_filename) 

# ------------- 読み込んだ試料数（カテゴリの数）を取得する
number_of_categories = len(smell_data.columns) - 2
data_interval = 1.0 / (number_of_categories - 1)

# ------------- トレーニングモデルの教師データを作る
dataX = []
dataY = []

#  -------------------- 繰り返し回数 (カテゴリ×サンプルデータ数 を 行列に)
data_count = len(smell_data) / nofIndex
dataYvalue = 0.0 - data_interval
for category in range(number_of_categories):
    dataYvalue = dataYvalue + data_interval
    for count in range(int(data_count)):
        dataXX = []
        for index in range(nofIndex):
            dataXX.append(smell_data.iat[count * nofIndex + index, (category + 1)]  / maxRange)
        dataX.append(dataXX)
        dataY.append(dataYvalue)

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
    for category in range(number_of_categories):
        print("{0},{1:.5f},;".format(smell_data.columns[category + 1], result_number), file = output_file)
        result_number = result_number + data_interval
