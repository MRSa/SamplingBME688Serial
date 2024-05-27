
# Windows アプリケーション

Atom Liteから送られてきたBME688の匂いデータを蓄積し、機械学習を行い、学習したデータを使ってどの匂いか判定することができるアプリです。

以下の３ステップで匂いを判別します。

1. 匂いを記憶させる (区別したい匂いの種類分、すべて記憶させます)
2. 匂いを判別できるよう、学習させる
3. 学習したモデルを使って、匂いを判別する

![WindowsApp](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview0.png?raw=true)

## ビルド

Visual Studio 2022 で .NET 6.0をターゲットフレームワークと設定しています。ソリューションファイルを読み込んでビルドしてください。

## 収集の開始

データのカテゴリラベル(Data Category欄)を入力し、Sensor1 / Sensor2 の シリアルポート名を指定して「Connect」ボタンを押すと、収集を開始します。
データカテゴリラベルを設定しない場合は、自動的に「empty」という名前をつけます。
収集中は、「Sampling Status」欄に状況を表示します。

![収集の開始](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/main-screen00.png?raw=true)

シリアルポートの番号が有効でない時にConnectボタンを押した場合、エラーが表示され、収集を中止します。

![エラー発生の表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/main-error.png?raw=true)

収集中は、ストップボタンが有効となり、収集状況を表示します。

![収集中表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/sampling.png?raw=true)

## 収集の終了

Sensor1 / Sensor2 の Stop ボタンを押すと、収集を終了します。収集結果は、「Collected Data」欄に表示します。「Clear」ボタンを押すと、収集状況を表示していた欄をクリアします。

![収集の終了](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/sampling_result.png?raw=true)

## CSVファイルへのエクスポート

「Export CSV」ボタンを押すと、収集した結果をエクスポートします。

### エクスポートオプション

以前のプロトタイプで使用するためのオプションです。現状、このエクスポートオプションを使用する必要はありません。

- Only Gas R.(Log) : ガス抵抗値の対数だけをCSVファイルに出力します。後述するモデル学習で使用する形式です。チェックなしの場合は、収集全データを出力します。
- Combine sensor : チェックを入れると、センサ１とセンサ２の同時収集を行った場合、そのデータを結合（ステップ数を０－９を０－１９にする）してCSVファイルに出力する形式です。チェックを入れた場合は、後述のモデル学習や判定でオプション(-i 20)を付与する必要があります。
- Duplicate : 標準は１です。収集データをCSVファイルに指定した回数出力します。データ増殖に利用できます。

### CSVファイルのフォーマット

CSVファイルは、1行に以下のデータをカンマ区切りで記録しています。（１行目はデータの説明を記載しています。）

- sensorId
  - センサID (1 or 2)
- category
  - データのカテゴリ名
- index
  - 収集データのインデックス番号(0-9)
- temperature
  - 温度
- humidity
  - 湿度
- pressure
  - 圧力
- gas_registance
  - ガス抵抗値
- gas_registance_log
  - ガス抵抗値の対数
- gas_registance_diff
  - ガス抵抗値の前回との差分値

## Collected Data欄の表示

- Category : 収集時に指定したカテゴリ名を表示します。
- sensorId : 収集したセンサ番号 (Sensor1: 1, Sensor2: 2)を表示します。
- dataCount : 収集したデータ数を表示します。
- validCount : 収集したデータ数のうち、有効な数（データが0-9の10個分そろっている数）を表示します。
- Temp. Max/Min : センサで収集した温度の最大値/最小値を表示します。
- Humi. Max/Min : センサで収集した湿度の最大値/最小値を表示します。
- Pres. Max/Min : センサで収集した圧力の最大値/最小値を表示します。
- GasR. Max/Min : センサで収集したガス抵抗値の最大値/最小値を表示します。
- GasR.(log) Max/Min : センサで収集したガス抵抗値の対数値の最大値/最小値を表示します。

## CSVファイルからインポート

「Import CSV」ボタンを押すと、あらかじめ保存していたCSVファイルを読み込みます。

## 収集データのリセット

アプリ右下の「Reset」ボタンで、収集データすべてをクリアします。
アプリ右上の「Clear」ボタンでは、「sampling Status」画面表示をクリアします。

## グラフの表示

収集したデータ(ガス抵抗値)のグラフを表から選択した行のデータについて表示します。
グラフは、収集した１つのシーケンスのグラフを最大３つまで重ね合わせて表示できます。
デフォルトでは、最初と最後と中間の３点のグラフを表示しています。

時系列の変位を確認する、収集したカテゴリそれぞれの差異を確認することができます。

![グラフ表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/graph1.png?raw=true)

グラフは、Y軸の高さを拡大するモード（Zoomモード）、対数値を表示するモード(Logモード)を持っています。

![ZoomとLog](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/graph2.png?raw=true)

## モデルの作成

「Create Model」ボタンを押すと、モデルを作成するためのダイアログを表示します。

![モデル作成](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/create_model.png?raw=true)

作成できるモデルの種類は、[ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet)が提供する、[多クラス分類のトレーナー](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)が選択可能となっています。
なお、PairwiseCoupling と OneVersusAll を選択した場合は、同時に使用する二項分類のモデルを選択する必要があります。

![作成可能なモデル](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/model_types.png?raw=true)

対象とするサンプルデータの種類により、いくつか選択が可能です。

- 1 and 2 : センサ1 と センサ2 の両方を利用するモードです
- 1 Only : センサ1 だけ利用するモードです
- 2 Only : センサ2 だけ利用するモードです
- 1 or 2 : センサ1 か センサ2 か、どちらかのデータを使うモードです

## 予測の実施

モデル作成ダイアログを閉じ、"Prediction" の中にある、Analyze チェックボックスをONにすると、予測を開始します。

## サンプルデータ

収集したサンプルのデータ（CSVファイル、6種類の試料をセンサ２個で収集したデータ、コーヒー：５、ほうじ茶：１）です。

![6種類の試料](https://github.com/MRSa/SamplingBME688Serial/blob/master/sample_data/COFFEE-SMELLS.JPG?raw=true)

[CSVファイル](https://raw.githubusercontent.com/MRSa/SamplingBME688Serial/master/sample_data/coffee-smells.csv)

- イタリアンロースト (カルディ)
- 炭焼珈琲 (カルディ)
- マイルドカルディ (カルディ)
- ダークロースト (カルディ)
- TOKYOロースト (スターバックス)
- ほうじ茶 (伊藤園)

以上
