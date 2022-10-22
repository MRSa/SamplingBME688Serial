#
#   BME688の匂いのデータをモデルから判定する
#      - 引数１ :  モデルファイル名
#      - 引数２ :  チェックするCSVファイル名
#      - 引数３ :  分類ラベルの書かれたファイル名（オプション）
#          ※ 引数１、引数３は、 create_smell_model.py で生成したファイルを指定する
#
import pandas as pd
import numpy as np
import keras
import sys

# -------------   実行時パラメータ
nofIndex = 10       # 
maxRange = 20.0     # 

#  ------  引数でCSVファイルを指定する
category_filename = ""
if __name__ == '__main__':
    args = sys.argv
    if 3 <= len(args):
        h5_filename = args[1]
        csv_filename = args[2]
        if 4 <= len(args):
            category_filename = args[3]
    else:
        print('Usage: {0} model_file check_file [category_file]'.format(args[0]))
        quit()

#  ---------- 引数に指定されていた
print(" Model file: {0}  Check target: {1}  Category define: {2}".format(h5_filename, csv_filename, category_filename))

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
