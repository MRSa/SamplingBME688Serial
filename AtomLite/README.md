# BME688-Atom Lite

[BOSCHのガスセンサである、BME688](https://www.bosch-sensortec.com/products/environmental-sensors/gas-sensors/bme688/)と[M5 Atom Lite](https://docs.m5stack.com/en/core/atom_lite)をI2C(Groveコネクタ)で接続し、
それをWindows PCにUSB シリアル接続(COMポート接続)を行い、センサーのデータをWindowsのアプリケーションに送るためのスケッチです。

BME688のI2Cアドレス(0x76 or 0x77)は自動で判定しますので、Atom LiteとBME688はワイヤ１本で接続するだけで利用可能です。スケッチ等の変更は必要ありません。

Atom LiteへBME688の2台同時接続はできません。BME688を2台使用したい場合には、Atom Liteも2台用意し、それぞれWindows PCに接続してください。

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/AtomLite.png?raw=true)

## 動作確認

以下の環境で動作確認を行っています。

* [M5 Atom Lite](https://docs.m5stack.com/en/core/atom_lite)
* [PIMORONI BME688 4-in-1 Air Quality Breakout (Gas, Temperature, Pressure, Humidity)](https://shop.pimoroni.com/products/bme688-breakout)
* [Adafruit BME688 - Temperature, Humidity, Pressure and Gas Sensor - STEMMA QT](https://www.adafruit.com/product/5046)
* [ENV Pro Unit with Temperature, Humidity, Pressure and Gas Sensor (BME688)](https://shop.m5stack.com/products/env-pro-unit-with-temperature-humidity-pressure-and-gas-sensor-bme688)

## 使用方法

Atom Lite と、センサ(BME688)を接続し、さらにUSBケーブルでWindowsパソコンに接続するだけです。
うまく接続ができていると、自動的にセンサデータをPCに送ります。

通常に動作をしている場合は、Atom LiteのLEDが青色に点灯し、PCにデータを送るときだけピンク色になります。

センサとの接続がうまくできていない場合は、緑色の点滅になりますので、Atom LiteとBME688との配線のチェックをお願いします。

## PCへ送信するデータ

本スケッチは、以下のデータをカンマ区切りでWindows PCに送信しています。定義や意味については、 [https://github.com/boschsensortec/BME68x_SensorAPI/blob/master/bme68x_defs.h](https://github.com/boschsensortec/BME68x_SensorAPI/blob/master/bme68x_defs.h) を参照してください。

* ガスインデックス番号(gas_index) : int (0-9)
* 測定インデックス番号(meas_index) : int
* PowerONからの時間(ミリ秒) : int
* 収集ステータス(status) : 整数
* ガス待ち点(gas_wait) : int
* 温度(temperature) : float
* 湿度(humidity) : float
* 圧力(pressure) : float
* ガス抵抗値(gas_registance) : float
* gas_registanceの対数 : float
* gas_registanceの変動値 : float
* データの末尾 : セミコロン

## その他

ヒータプロファイルの変更は、スケッチ内で可能となっています。
スケッチ内のコメント「ヒーターの温度(℃)の１サイクル分の温度変化。」で設定温度、「ヒーターの温度を保持する時間の割合。」で保持時間が変更可能です。
必要な場合は、この部分の変更をお試しください。

--------

## 使用ライブラリ

本スケッチは、以下のライブラリを使用しています。

* [Bosch BME68x Library](https://github.com/BoschSensortec/Bosch-BME68x-Library)
  * [https://github.com/BoschSensortec/Bosch-BME68x-Library](https://github.com/BoschSensortec/Bosch-BME68x-Library)

本スケッチは、 [https://gist.githubusercontent.com/ksasao/5505e0e59a97cde799cf0ed2d2009b2d/raw/ec847139310ed5a3e51c163d95d8062d3e8f5d3d/M5BME688.ino](https://gist.githubusercontent.com/ksasao/5505e0e59a97cde799cf0ed2d2009b2d/raw/ec847139310ed5a3e51c163d95d8062d3e8f5d3d/M5BME688.ino) を参考にさせていただきました。というか、I2CのAddressを自動で判定するようにした以外は、ほぼそのまま使わせていただいています。ありがとうございました。

--------

## 参考： スケッチをコンパイルし、Atom Liteにインストールする方法

ここでは、[Arduino IDEを使った方法](https://docs.m5stack.com/en/arduino/arduino_ide)を記載します。

### Arduino IDEの準備

1. [Arduino IDE](https://www.arduino.cc/en/software) をインストール
2. [Arduino Board Management](https://docs.m5stack.com/en/arduino/arduino_board)の記載に沿って、File>**環境設定**にある、追加のボードマネージャ欄のURLに、[https://static-cdn.m5stack.com/resource/arduino/package_m5stack_index.json](https://static-cdn.m5stack.com/resource/arduino/package_m5stack_index.json) を追加
3. （念のため）Arduino IDEを一度終了し、立ち上げなおす
4. Tools > Boards Manager... を選択して、M5のボードライブラリをインストール
5. [https://github.com/BoschSensortec/Bosch-BME68x-Library](https://github.com/BoschSensortec/Bosch-BME68x-Library) から、ZIP ファイルをダウンロード(Code > Download ZIP から ZIPファイルとしてダウンロードする)
6. ダウンロードした ZIPファイルを　Sketch > Include Library > Add .ZIP Library... のメニューからライブラリをインストールする
7. Atom Lite を USB経由で接続し、シリアルポートの番号を把握する

#### Arduino IDE スクリーンショット集

追加のボードマネージャ

![Arduino Board Management設定](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino01.png?raw=true)

ボードマネージャのインストール

![Arduino Board Manager](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino02.png?raw=true)

### スケッチのコンパイルと転送

1. File>Open... で、[スケッチ](https://raw.githubusercontent.com/MRSa/SamplingBME688Serial/master/AtomLite/sketch_BME688_SAMPLING.ino)を読み込む
2. ボードの設定を行う（接続したシリアルポート番号と、ボード名（M5Atom）を設定する）
3. Sketch>Verify/Compile で、スケッチをコンパイルする
4. コンパイルでエラーが出なかった場合は、Sketch>Uploadを選択し、Atom Liteにプログラムを送る （コンパイルが実行されてから、エラーが発生してないときにスケッチが転送される）
5. コンパイラエラーが出ておらず、スケッチが転送されると、LEDが緑色で0.5秒間隔で点滅する

#### スクリーンショット集

GitHub上のBosch BME68x ライブラリ(ダウンロード)

![Bosch BME68x Library(GitHub)](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino10.png?raw=true)

ライブラリの設定

![Libraryの設定](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino11.png?raw=true)

接続したAtom Liteのport設定

![Atom Lite port設定](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino12.png?raw=true)

スケッチのコンパイルと転送

![スケッチのコンパイルと転送](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino13.png?raw=true)

コンパイルと転送が成功した例

![スケッチのコンパイルと転送（成功）](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/arduino14.png?raw=true)

以上
