# SamplingBME688Serial : BME688でにおいを判別する（一式）

## リポジトリ
https://github.com/MRSa/SamplingBME688Serial

## 全体像
![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/Overview2.png?raw=true)

- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルまたはデータベースへ出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
- CSV形式のファイルから、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model.py)
- 学習したH5形式のモデルファイルを使って、CSV形式のファイルに記録していたデータの種別を判定する (python/check_smell.py)
- Windows(C#)アプリから送られてきたJSONデータをPostgreSQLに格納するコンテナ群(docker-database/ 以下)
- PostgreSQLに格納されているデータをJSONデータ形式で取り出し、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model_from_url.py)

## BME688-Atom Lite

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/AtomLite.png?raw=true)

## Windows App

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/bme688Win.png?raw=true)

### ビルド
Visual Studio 2022 で .NET 6.0をターゲットフレームワークと設定しています。ソリューションファイルを読み込んでビルドしてください。

### 収集の開始
データのカテゴリラベル(Data Category欄)を入力し、Sensor1 / Sensor2 の シリアルポート名を指定して「Connect」ボタンを押すと、収集を開始します。
データカテゴリラベルを設定しない場合は、自動的に「empty」という名前をつけます。
収集中は、「Sampling Status」欄に状況を表示します。
### 収集の終了
Sensor1 / Sensor2 の Stop ボタンを押すと、収集を終了します。
収集結果は、「Collected Data」欄に表示します。

### CSVファイルへのエクスポート
「Export CSV」ボタンを押すと、収集した結果をエクスポートします。

#### エクスポートオプション
- Only Gas R.(Log) : 標準でチェックが入っています。ガス抵抗値の対数だけをCSVファイルに出力します。後述するモデル学習で使用する形式です。チェックなしの場合は、収集全データを出力します。
- Combine sensor : チェックを入れると、センサ１とセンサ２の同時収集を行った場合、そのデータを結合（ステップ数を０－９を０－１９にする）してCSVファイルに出力する形式です。チェックを入れた場合は、後述のモデル学習や判定でオプション(-i 20)を付与する必要があります。
- Duplicate : 標準は１です。収集データをCSVファイルに指定した回数出力します。データ増殖に利用できます。

### Collected Data欄の表示
- Category : 収集時に指定したカテゴリ名を表示します。
- sensorId : 収集したセンサ番号 (Sensor1: 1, Sensor2: 2)を表示します。
- dataCount : 収集したデータ数を表示します。
- validCount : 収集したデータ数のうち、有効な数（データが0-9の10個分そろっている数）を表示します。
- Temp. Max/Min : センサで収集した温度の最大値/最小値を表示します。
- Humi. Max/Min : センサで収集した湿度の最大値/最小値を表示します。
- Pres. Max/Min : センサで収集した圧力の最大値/最小値を表示します。
- GasR. Max/Min : センサで収集したガス抵抗値の最大値/最小値を表示します。
- GasR.(log) Max/Min : センサで収集したガス抵抗値の対数値の最大値/最小値を表示します。


### 収集データのリセット
アプリ右下の「Reset」ボタンで、収集データすべてをクリアします。
アプリ右上の「Clear」ボタンでは、「sampling Status」画面表示をクリアします。


### CSVファイル： モデル作成用
Windowsアプリで作成した、CSVファイルです。データのカテゴリラベルを入力して収集・ストップを繰り返すと、そのカテゴリラベル分のガス抵抗値の対数をステップ数毎に出力します。
データ数は、有効なデータ数(validCount)の最小の数で全てのデータカテゴリのデータを揃えます。

![モデル作成用CSVファイル](https://github.com/MRSa/SamplingBME688Serial/blob/master/python/sample_data/for_smell_model.csv)

### CSVファイル： 判定用
フォーマットはモデル作成用と同じですが、データを１回収集して、ストップしたデータを想定しています。

![判定用CSVファイル](https://github.com/MRSa/SamplingBME688Serial/blob/master/python/sample_data/for_test1.csv)

### 収集全データ
CSV形式で、収集した有効なデータを全て出力します。
なお、有効なデータとは、ステップ0-9がすべて収集できているデータです。


## Python Script

### create_smell_model.py
CSVファイルを読み込み、LSTMで学習させたモデルをH5形式のファイルとしてカレントディレクトリに出力します。

#### 使い方
```
create_smell_model.py [-h] [-e EPOCH] [-i INDEX] csvFile
```
- csvFile : モデル作成用のCSVファイルを指定します
- -h : 引数のヘルプを表示します
- -e 数字 : エポック数 (繰り返し学習回数)を指定します。標準は 500 です。
- -i 数字 : 収集したステップ数を指定します。標準は 10 です。 combine で出力したCSVファイルを使用するときには 20 を指定してください。

#### 出力
学習が終了すると、指定したCSVファイルに対応するモデルファイルと、カテゴリ名ファイルをカレントディレクトリに出力します。
例えば、CSVファイル名が aaa.csv だった場合は、モデルファイル aaa.h5 と、カテゴリ名ファイル aaa_category.txt という２ファイルを出力します。

#### 実行イメージ
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

### check_smell.py
モデルファイルと判定したいCSVファイルを読み込み、判定結果を表示します。

#### 使い方
```
check_smell.py [-h] [-c CATEGORY] [-i INDEX] modelFile csvFile
```
- modelFile : create_smell_mode.py で作成した モデルファイル名を指定します
- csvFile : 判定用のCSVファイルを指定します
- -h : 引数のヘルプを表示します
- -c ファイル名 : create_smell_mode.py で作成した カテゴリ名ファイルを指定します
- -i 数字 : 収集したステップ数を指定します。標準は 10 です。 combine で出力したCSVファイルを使用するときには 20 を指定してください。

#### 出力
判定用のCSVファイルに記録されているデータから、判定値（数値）と、カテゴリ名ファイルを指定していた場合はそのデータカテゴリを画面表示します。

#### 実行イメージ
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
