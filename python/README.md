# Python Script

## 全体像

![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/python_src.png?raw=true)

- CSV形式のファイルから、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model.py)
- 学習したH5形式のモデルファイルを使って、CSV形式のファイルに記録していたデータの種別を判定する (python/check_smell.py)
- PostgreSQLに格納されているデータをJSONデータ形式で取り出し、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model_from_url.py)

## create_smell_model.py

CSVファイルを読み込み、LSTMで学習させたモデルをH5形式のファイルとしてカレントディレクトリに出力します。

### 使い方

```
create_smell_model.py [-h] [-e EPOCH] [-i INDEX] csvFile
```

- csvFile : モデル作成用のCSVファイルを指定します
- -h : 引数のヘルプを表示します
- -e 数字 : エポック数 (繰り返し学習回数)を指定します。標準は 500 です。
- -i 数字 : 収集したステップ数を指定します。標準は 10 です。 combine で出力したCSVファイルを使用するときには 20 を指定してください。

### 出力

学習が終了すると、指定したCSVファイルに対応するモデルファイルと、カテゴリ名ファイルをカレントディレクトリに出力します。
例えば、CSVファイル名が aaa.csv だった場合は、モデルファイル aaa.h5 と、カテゴリ名ファイル aaa_category.txt という２ファイルを出力します。

### 実行イメージ

```
$ python3 create_smell_model.py -e 10 for_smell_model.csv

number of epochs : 10
 Create model: ../for_smell_model.csv -> for_smell_model.h5 (for_smell_model_category.txt) [index:10]

Epoch 1/10
99/99 [==============================] - 2s 6ms/step - loss: 0.1654
Epoch 2/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1412
Epoch 3/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1410
Epoch 4/10
99/99 [==============================] - 1s 7ms/step - loss: 0.1407
Epoch 5/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1404
Epoch 6/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1393
Epoch 7/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1392
Epoch 8/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1390
Epoch 9/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1383
Epoch 10/10
99/99 [==============================] - 1s 6ms/step - loss: 0.1374
Model: "sequential"
_________________________________________________________________
 Layer (type)                Output Shape              Param #
=================================================================
 lstm (LSTM)                 (None, 10)                480

 dense (Dense)               (None, 1)                 11

=================================================================
Total params: 491
Trainable params: 491
Non-trainable params: 0
_________________________________________________________________

```

## create_smell_model_from_url.py

データベースからセンサデータを読み込み、LSTMで学習させたモデルをH5形式のファイルとしてカレントディレクトリに出力します。

### 使い方

```
python create_smell_model_from_url.py [-h] [--url URL] [--epoch EPOCH] [--duplicate DUPLICATE] [--offset OFFSET] [--count COUNT] [--sensor SENSOR] [--labels LABELS] modelFile 
```

- modelFile : 作成するモデルのファイル名を指定します
- -h           　　　　　　     : 引数のヘルプを表示します
- --url  URL    　　　　　　   : センサデータを取得するURLを指定します。 未指定時は http://localhost:3010/ です
- --epoch 数字    　　　　　　 : エポック数 (繰り返し学習回数)を指定します。標準は 500 です。
- --duplicate 数字 　　　　　　: データを増殖する回数を指定します。未指定時は 1 です。
- --offset 数字  　　　　　　  : センサデータを取得するとき、何番目のデータから取得するかを指定します。
- --count  数字               : センサデータを取得する数を指定します。
- --sensor 数字              : 取得するセンサのID(1または2)を指定します。 未指定時は全てのセンサデータを取得します。
- --labels カンマ区切り文字列 : 取得するセンサのデータカテゴリを指定します。登録されているモデルデータの一部を指定して学習する場合に指定してください。

### 出力

学習が終了すると、指定したCSVファイルに対応するモデルファイルと、カテゴリ名ファイルをカレントディレクトリに出力します。
例えば、CSVファイル名が aaa.csv だった場合は、モデルファイル aaa.h5 と、カテゴリ名ファイル aaa_category.txt という２ファイルを出力します。

## check_smell.py

モデルファイルと判定したいCSVファイルを読み込み、判定結果を表示します。

### 使い方

```
check_smell.py [-h] [-c CATEGORY] [-i INDEX] modelFile csvFile
```

- modelFile : create_smell_mode.py で作成した モデルファイル名を指定します
- csvFile : 判定用のCSVファイルを指定します
- -h : 引数のヘルプを表示します
- -c ファイル名 : create_smell_mode.py で作成した カテゴリ名ファイルを指定します
- -i 数字 : 収集したステップ数を指定します。標準は 10 です。 combine で出力したCSVファイルを使用するときには 20 を指定してください。

### 出力

判定用のCSVファイルに記録されているデータから、判定値（数値）と、カテゴリ名ファイルを指定していた場合はそのデータカテゴリを画面表示します。

### 実行イメージ

```
$ python3 check_smell.py for_smell_model.h5 -c for_smell_model_category.txt for_test2.csv

 - - - - - - CHECK RESULT - - - - - -
0.862 :  紅茶
0.831 :  ほうじ茶
0.814 :  ほうじ茶
0.801 :  ほうじ茶
0.793 :  ほうじ茶
0.788 :  ほうじ茶
0.786 :  ほうじ茶
0.785 :  ほうじ茶
0.779 :  ほうじ茶
0.775 :  ほうじ茶
0.777 :  ほうじ茶
0.775 :  ほうじ茶
 - - - - - - - - - - - -  - - - - - -

```
