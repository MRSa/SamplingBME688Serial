# Windows アプリケーション for BME688

## 目次

- [Windows アプリケーション for BME688](#windows-アプリケーション-for-bme688)
  - [目次](#目次)
  - [概要](#概要)
  - [ビルド](#ビルド)
    - [使用NuGetパッケージ](#使用nugetパッケージ)
  - [サンプリングモードと予測モード](#サンプリングモードと予測モード)
  - [収集の開始](#収集の開始)
    - [データベースへの登録](#データベースへの登録)
  - [収集の終了](#収集の終了)
    - [Collected Data欄の表示](#collected-data欄の表示)
  - [CSVファイルへのエクスポート](#csvファイルへのエクスポート)
    - [エクスポートオプション](#エクスポートオプション)
    - [CSVファイルのフォーマット](#csvファイルのフォーマット)
  - [CSVファイルからインポート](#csvファイルからインポート)
  - [データベースからのデータロード](#データベースからのデータロード)
  - [収集データのリセット](#収集データのリセット)
  - [グラフの表示](#グラフの表示)
  - [モデルの作成](#モデルの作成)
    - [モデル作成時に使用するセンサ](#モデル作成時に使用するセンサ)
    - [利用可能なアルゴリズム](#利用可能なアルゴリズム)
    - [トレーニングデータについて](#トレーニングデータについて)
      - [トレーニングに使用するデータの区間](#トレーニングに使用するデータの区間)
      - [トレーニングデータの増幅](#トレーニングデータの増幅)
      - [対数データでのトレーニング](#対数データでのトレーニング)
      - [圧力、温度、湿度込みでのトレーニング](#圧力温度湿度込みでのトレーニング)
    - [トレーニングの開始](#トレーニングの開始)
    - [トレーニング結果の評価](#トレーニング結果の評価)
    - [トレーニング結果の保存](#トレーニング結果の保存)
  - [モデルの読み込み](#モデルの読み込み)
  - [予測の実行](#予測の実行)
    - [予測結果の保存](#予測結果の保存)
      - [予測結果のCSVファイルフォーマット](#予測結果のcsvファイルフォーマット)
  - [予測モードからサンプリングモードへの切り替え](#予測モードからサンプリングモードへの切り替え)

## 概要

Atom Liteから送られてきたBME688の匂いデータを蓄積し、機械学習を行い、学習したデータを使ってどの匂いか判定することができるアプリです。

以下の３ステップで匂いを判別します。

1. 匂いを記憶させる (区別したい匂いの種類分、すべて記憶させます)
2. 匂いを判別できるよう、学習させる
3. 学習したモデルを使って、匂いを判別する

![WindowsApp](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview0.png?raw=true)

## ビルド

Visual Studio 2022 で .NET 6.0をターゲットフレームワークと設定しています。ソリューションファイルを読み込んでビルドしてください。

### 使用NuGetパッケージ

本アプリケーションは、以下のNuGetパッケージを利用しています。

- [Microsoft.ML](https://www.nuget.org/packages/Microsoft.ML/)
- [Microsoft.ML.LightGbm](https://www.nuget.org/packages/Microsoft.ML.LightGbm/)
- [System.IO.Ports](https://www.nuget.org/packages/System.IO.Ports/)

## サンプリングモードと予測モード

アプリケーションは、おおまかに、センサからデータを収集・蓄積する「サンプリングモード」と、センサからのデータを使って、匂いが何かをあてる「予測モード」があります。アプリケーション起動直後は、サンプリングモードです。

[アプリケーションのモード](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/app-mode.png?raw=true)

## 収集の開始

データのカテゴリラベル(Data Category欄)を入力し、Sensor1 / Sensor2 の シリアルポート名を指定して「Connect」ボタンを押すと、収集を開始します。
データカテゴリラベルを設定しない場合は、自動的に「empty」という名前をつけます。
収集中は、"Sampling Status" 欄に状況を表示します。

![収集の開始](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/main-screen00.png?raw=true)

シリアルポートの番号が有効でない時にConnectボタンを押した場合は、エラーが表示されて収集ができません。

![エラー発生の表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/main-error.png?raw=true)

収集中は、ストップボタンが有効となり、"Sampling Status"欄に収集状況を表示します。

![収集中表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/sampling.png?raw=true)

### データベースへの登録

データの収集時、Databaseエリアの先頭にあるチェックボックスをONにすることで、アプリ内にデータが到着したタイミングで、[Web API経由でデータベース](../docker-database/README.md)にデータを保管し、あとからアプリへ読み込むことができます。保管先データベースの詳細については [docker-database](../docker-database/README.md) を参照してください。

![データベースへ登録](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/entry-database.png?raw=true)

なお、後からデータを読み出し、利用するためには、**Entry All Data のチェックはONのままにしておく必要がありますので、ご注意ください。**

## 収集の終了

Sensor1 / Sensor2 の Stop ボタンを押すと、収集を終了します。収集結果は、「Collected Data」欄に表示します。「Clear」ボタンを押すと、収集状況を表示していた欄をクリアします。

![収集の終了](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/sampling_result.png?raw=true)

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

## CSVファイルへのエクスポート

「Export CSV」ボタンを押すと、収集した結果をエクスポートすることができます。あとで収集したデータを読み込みたいという場合もありますので、収集後にエクスポートしておくことをお勧めします。

![CSVエクスポート](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/export-csv.png?raw=true)

### エクスポートオプション

以前のプロトタイプで使用するために作成したオプションです。 **現在は、これらのエクスポートオプションを使用する必要はありません。**

- Only Gas R.(Log) : ガス抵抗値の対数だけをCSVファイルに出力します。後述するモデル学習で使用する形式です。チェックなしの場合は、収集全データを出力します。
- Combine sensor : チェックを入れると、センサ１とセンサ２の同時収集を行った場合、そのデータを結合（ステップ数を０－９を０－１９にする）してCSVファイルに出力する形式です。チェックを入れた場合は、後述のモデル学習や判定でオプション(-i 20)を付与する必要があります。
- Duplicate : 標準は１です。収集データをCSVファイルに指定した回数出力します。データ増殖に利用できます。
- From (%) : エクスポートするデータの最初の位置を割合(パーセント)で指定します。初期値は 0 です。
- To (%) : エクスポートするデータの最後の位置を割合(パーセント)で指定します。初期値は 0 です。

### CSVファイルのフォーマット

エクスポートするCSVファイルは、1行に以下のデータをカンマ区切りで記録しています。１行目はデータの説明を記載しています。インポート可能なCSVファイルも、この形式です。
具体的な出力内容は、[出力したサンプルCSVファイル](https://raw.githubusercontent.com/MRSa/SamplingBME688Serial/master/sample_data/coffee-smells.csv) を参照してください。

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

## CSVファイルからインポート

「Import CSV」ボタンを押すと、あらかじめエクスポートしていたCSVファイルを読み込むことができます。**本ツールのデフォルト設定でCSVエクスポートしたファイル以外は読み込みがきませんので、ご注意ください。**

ボタンを押すと、「作りが悪いのでアプリケーションが固まりますけど、ちょっと待ってください」といったワーニングメッセージを表示しますので、「OK」を押してください。続けてファイル選択ダイアログが表示されるので、読み出したいCSVファイルを選択し、「開く」ボタンを押してください。CSVファイルのインポートが始まりますので、しばらくお待ちください。

![CSVファイルからインポート・ワーニングダイアログ](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/import-warning.png?raw=true)

インポートが終了すると、読み込みが終了したことをダイアログでお知らせします。

![CSVファイルからインポート・成功ダイアログ](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/import-success.png?raw=true)

## データベースからのデータロード

「Load data」ボタン、または、「Status」ボタンを押すと、データベースに接続し、格納されているデータの情報を表示します。

![データベースの状態](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/database-status.png?raw=true)

「Load Data」ボタンを押して表示されたダイアログでは、読み込みたいデータにチェックを入れて、「Load Data」ボタンを押すことで、データを読み込むことができます。また、ダイアログの下部に表示されている「From」で、データの読み出し開始の位置を、「Count」で読み出しデータ数の指定ができます。

なお、ここでのcountは、データのインデックスデータ１つが１件ですので、センサデータとしては10個で１組のデータとして取り扱いますのでご注意ください。
（メイン画面のdataCount, validCountは、ここで指定した count のおよそ 1/10 になります。）

![データベースからの読み出し](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/load-data.png?raw=true)

## 収集データのリセット

アプリ右下の「Reset」ボタンで、収集データすべてをクリアします。
（アプリ右上の「Clear」ボタンでは、「sampling Status」画面表示をクリアするだけなので、ご注意ください。）

![収集データのリセット](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/data-reset.png?raw=true)

## グラフの表示

収集したデータ(ガス抵抗値)のグラフを表から選択した行のデータについて表示します。

![行の選択](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/select-row.png?raw=true)

行を選択していない場合は、「行を選択してください」というダイアログを表示します。

![行選択エラー](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/warning-row.png?raw=true)

グラフは、収集した１つのシーケンスのグラフを最大３つまで重ね合わせて表示できます。
デフォルトでは、最初と最後と中間の３点のグラフを表示しています。

時系列の変位を確認する、収集したカテゴリそれぞれの差異を確認することができます。

![グラフ表示](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/graph1.png?raw=true)

グラフは、Y軸の高さを拡大するモード（Zoomモード）、対数値を表示するモード(Logモード)を持っています。

![ZoomとLog](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/graph2.png?raw=true)

左下に強調表示しているグラフの名前を表示しています。グラフ名の左右にあるボタンを押すことで、強調表示するグラフを変更することができます。

![強調表示するグラフの変更](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/Graph_selection.gif?raw=true)

グラフのスライダーを動かすことで、表示するグラフのタイミングが変更できます。スライダーを 0% → 99% まで動かすと、データの経時変化を確認することができます。

![グラフの経時変化確認](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/Graph_move.gif?raw=true)

## モデルの作成

「Create Model」ボタンを押すと、モデルを作成するためのダイアログを表示します。

![モデル作成](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/create-model.png?raw=true)

### モデル作成時に使用するセンサ

対象とするサンプルデータの種類により、いくつか選択が可能です。

- 1 and 2 : センサ1 と センサ2 の両方を利用するモードです
- 1 Only : センサ1 だけ利用するモードです
- 2 Only : センサ2 だけ利用するモードです
- 1 or 2 : センサ1 か センサ2 か、どちらかのデータを使うモードです

### 利用可能なアルゴリズム

利用可能なアルゴリズムの種類は、[ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet)が提供する、[多クラス分類のトレーナー](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)が選択可能となっています。

![選択可能なアルゴリズム](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/model_types.png?raw=true)

選択可能なアルゴリズムは以下です。

- LightGbm
- LbfgsMaximumEntropy
- SdcaMaximumEntropy
- SdcaNonCalibrated
- Navie Bayes
- Pairwise Coupling
- OneVersusAll
- K-Means

なお、PairwiseCoupling と OneVersusAll を選択した場合は、同時に使用する二項分類のアルゴリズムを以下から選択する必要があります。

- LightGbm
- Gam
- FastTree
- FastForest
- AveragedPerceptron
- LbfgsLogisticRegression
- SymbolicSgdLogisticRegression

### トレーニングデータについて

#### トレーニングに使用するデータの区間

トレーニングするデータの区間を割合(0%～100%)で指定できます。カテゴリによって、サンプルサイズのばらつきが発生するのを防ぐために、最小のサンプルデータ数をベースに区間を指定します。

![データの区間](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/data-range.png?raw=true)

#### トレーニングデータの増幅

「Duplicate」で、トレーニングデータを増やすことができます。

- x1 : 1倍、通常です
- x5 : データを5倍にします        (同じデータを5回使います)
- x10 : データを10倍にします      (同じデータを10回使います)
- x20 : データを20倍にします      (同じデータを20回使います)
- x30 : データを30倍にします      (同じデータを30回使います)
- x50 : データを50倍にします      (同じデータを50回使います)
- x100 : データを100倍にします    (同じデータを100回使います)
- x1000 : データを1000倍にします  (同じデータを1000回使います)

#### 対数データでのトレーニング

「Log」のチェックを ON にすると、ガス抵抗値を対数値で学習することができます。

#### 圧力、温度、湿度込みでのトレーニング

「w./P T H」のチェックを ON にすると、ガス抵抗値に加えて、圧力、温度、湿度（の平均値）を使用してトレーニングを実行します。
（デフォルトでチェックは ON にしています。）

### トレーニングの開始

「Create Model」を押すと、トレーニングを開始するかどうか、確認を求めるダイアログを表示します。OKを押すと、トレーニングを開始します。トレーニングが終了するまで、しばらくお待ちください。
トレーニングが終了すると、そのことを示すダイアログを表示します。

![トレーニングの開始](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/create_model.png?raw=true)

### トレーニング結果の評価

トレーニングが終了すると、「Message」欄にトレーニング結果の評価が表示されます。モデルの精度の目安にお使いください。

- MicroAccuracy : マイクロ平均精度 (1に近いほどよい)
- MacroAccuracy : マクロ平均精度 (1に近いほどよい)
- LogLoss : 分類子の平均対数損失 (0に近いほどよい)
- LogLossReduction : 分類子の対数損失の減少 (1に近いほどよい)

![トレーニング結果](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/model-result.png?raw=true)

### トレーニング結果の保存

トレーニングが終了すると、「Save Model」ボタンが表示されます。このボタンを押すと、トレーニングしたモデルをファイル（zip形式）で保存することができます。

モデルの保存が終わると、保存したモデルファイルのファイル名を表示します。

![モデルの保存](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/save-model.png?raw=true)

## モデルの読み込み

「Load Model」ボタンを押すと、保存していたモデルファイル（zip形式）を読み出すことができます。このとき、**モデル作成時に使用するセンサの設定は、モデルを保存したときの設定と合わせてください。**
適切な予測ができませんのでご注意ください。

モデルの読み込みが終わると、読み込んだモデルファイルのファイル名を表示します。

![モデルの読み込み](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/load-model.png?raw=true)

## 予測の実行

モデルの作成、あるいはモデルの読み込みを行ったあと、モデル作成ダイアログを閉じると、アプリケーションはサンプリングモードから予測モードに切り替わり、「Prediction」が有効になります。

![予測モード](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/prediction_mode.png?raw=true)

"Prediction" の中にある、Analyze チェックボックスをONにすると、予測を開始します。

![予測の実行](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/prediction.png?raw=true)

Analyze チェックボックスを外すと、予測の実行を終了します。

### 予測結果の保存

予測の実行を終了させたとき、予測を１回以上実行していたときには、予測結果の実行確認ダイアログが表示されます。

![予測結果の保存確認](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/result_save.png?raw=true)

予測結果を保存したい場合は、「はい(Yes)」を選んでください。名前を付けて保存ダイアログが表示され、ファイル名を決めると、
予測結果をCSV形式で保存することができます。

#### 予測結果のCSVファイルフォーマット

予測結果のCSVファイルは、1行に以下のデータをカンマ区切りで記録しています。１行目はデータの説明を記載しています。

- count
  - 予測実行した回数
- result
  - 予測した結果
- time
  - 予測を実行した時間

## 予測モードからサンプリングモードへの切り替え

予測モードから、サンプリングモードに戻す場合は、右上の「Clear」ボタンか、右下の「Reset」ボタンを押してください。
「Clear」ボタンは、モデルを作り直す場合、「Reset」ボタンは、センサのデータを最初から収集しなおす場合にご使用ください。

![画面遷移について](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/screen-transition.png?raw=true)

以上
