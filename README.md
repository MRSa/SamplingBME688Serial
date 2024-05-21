# SamplingBME688Serial : BME688でにおいを判別する（一式）

## リポジトリ

[https://github.com/MRSa/SamplingBME688Serial](https://github.com/MRSa/SamplingBME688Serial)

## 概要

BOSCH社のAIガスセンサ[BME688](https://www.bosch-sensortec.com/products/environmental-sensors/gas-sensors/bme688/) と [ATOM Lite](https://docs.m5stack.com/en/core/atom_lite)を最大２つWindows PCに接続し、C#で作成したWindowsアプリケーションで「におい」を学習し、学習結果を使って、どの「匂い」かを当てる（ことができるかもしれない）ようにしたシステムを提供します。

「匂い」の学習には、.NET用の機械学習フレームワーク([ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet))が提供する、[他クラス分類のトレーナー](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)を使用でき、各トレーナーによる判定の違いをお試しいただけるようになっています。

現在では、C#で作成したWindowsアプリケーション単独で匂いの学習と判定まで実施できるようになっていますが、それができるようになる前まではpythonのモデルを使用して匂いの判定をしていました。このリポジトリには、その名残のpythonスクリプトが登録してあります。

## 全体像

![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview00.png?raw=true)

- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルまたはデータベースへ出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
  - ML.NET を使用して、多クラス分類を行うようトレーニングしたモデルを作り、そのモデルを使って、匂いの判別をできるようにする
  - [ML.NET での機械学習のタスク](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)で紹介されている、多クラス分類のトレーナーが選べる
- Windows(C#)アプリから送られてきたJSONデータをPostgreSQLに格納するコンテナ群(docker-database/ 以下)

以前は、pythonスクリプトで解析を行っていました。（スクリプトの説明は以下です。）

- [以前のPythonスクリプト](Python_predict.md)

## BME688-Atom Lite

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/AtomLite.png?raw=true)

## Windows App

![WindowsApp](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview0.png?raw=true)

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
