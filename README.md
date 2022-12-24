# SamplingBME688Serial : BME688でにおいを判別する（一式）

## リポジトリ
https://github.com/MRSa/SamplingBME688Serial

## 全体像
![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/Overview.png?raw=true)

- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルに出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
- CSV形式のファイルから、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model.py)
- 学習したH5形式のモデルファイルを使って、CSV形式のファイルに記録していたデータの種別を判定する (python/check_smell.py)


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
- Combine sensor : チェックを入れると、センサ１とセンサ２の同時収集を行った場合、そのデータを結合（０－９を０－１９にする）してCSVファイルに出力する形式です。チェックを入れた場合は、後述のモデル学習や判定でオプション(-i 20)を付与する必要があります。
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

## Python Script

### create_smell_model.py
CSVファイルを読み込み、LSTMで学習させたモデルをH5形式のファイルとしてカレントディレクトリに出力します。
### check_smell.py
モデルファイルと判定したいCSVファイルを読み込み、判定結果を表示します。

