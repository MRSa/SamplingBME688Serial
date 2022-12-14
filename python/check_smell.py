#
#   BME688の匂いのデータをモデルから判定する
#      - 引数１ :  モデルファイル名
#      - 引数２ :  チェックするCSVファイル名
#      - オプション１(-c) :  カテゴリファイル名
#      - オプション２(-i) :  インデックスの数
#          ※ 引数１、オプション１は、 create_smell_model.py で生成したファイルを指定する
#
import pandas as pd
import numpy as np
import keras
import sys
import argparse

# -------------   実行時パラメータ
nofIndex = 10       # 
maxRange = 20.0     # 
category_filename = ""

#  ------  コマンドライン引数の解析
parser = argparse.ArgumentParser(description='Check Smell')
parser.add_argument('modelFile', help='model file path')
parser.add_argument('csvFile', help='CSV file path')
parser.add_argument("-c", "--category", help="category file")
parser.add_argument("-i", "--index", type=int, help="number of indexes")
args =  parser.parse_args()
h5_filename = args.modelFile
csv_filename = args.csvFile
if args.category:
    print("category file name : {0}".format(args.category))
    category_filename = args.category
if args.index:
    print("number of indexes : {0}".format(args.index))
    nofIndex = args.index

print(" Model file: {0}  Check target: {1}  Category define: {2}  [index: {3}]".format(h5_filename, csv_filename, category_filename, nofIndex))


#  ---------- モデル(HDF5形式)の読み出し
model = keras.models.load_model(h5_filename)
#model.summary()
#print(" ")

#  ---------- CSVデータの読み出し
target_data = pd.read_csv(csv_filename) 
#target_data.info()
#print(" ")

#  ---------- カテゴリ情報の読み出し
if len(category_filename) > 0:
    category_data = pd.read_csv(category_filename) 
    #category_data.info()
    #print(" ")

print(" ")
# ------------- チェックするデータを作る
dataX = []
data_count = len(target_data) / nofIndex
for count in range(int(data_count)):
        dataXX = []
        for index in range(nofIndex):
            dataXX.append(target_data.iat[count * nofIndex + index, 1]  / maxRange)
        dataX.append(dataXX)

# ------------- チェックの実行
checkX = np.array(dataX)
checkX.reshape(checkX.shape[0], 1, checkX.shape[1])
checkResult = model.predict(checkX)
print(" ")

# ------------- チェック結果の表示
print(" - - - - - - CHECK RESULT - - - - - -")
for count in range(len(checkResult)):
    distance = 2.0
    label = ""
    value = checkResult[count][0]
    if len(category_filename) > 0:
        # ラベルとの比較
        for category in range(len(category_data)):
            diff = abs(category_data["value"][category] - value)
            if distance > diff:
                distance = diff
                label = category_data["label"][category]

    # ------------ 結果を表示する(ラベルファイルを指定していたらそれも表示) ------------     
    print("{0:.3f} : {1}".format(value, label))

print(" - - - - - - - - - - - -  - - - - - -")
