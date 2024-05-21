# SamplingBME688Serial : BME688でにおいを判別する（一式）

## リポジトリ

[https://github.com/MRSa/SamplingBME688Serial](https://github.com/MRSa/SamplingBME688Serial)

## 概要

BOSCH社のAIガスセンサ[BME688](https://www.bosch-sensortec.com/products/environmental-sensors/gas-sensors/bme688/) と [ATOM Lite](https://docs.m5stack.com/en/core/atom_lite)を最大２つWindows PCに接続し、C#で作成したWindowsアプリケーションで「におい」を学習し、学習結果を使って、どの「匂い」かを当てる（ことができるかもしれない）ようにしたシステムを提供します。

「匂い」の学習には、.NET用の機械学習フレームワーク([ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet))が提供する、[他クラス分類のトレーナー](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)を使用でき、各トレーナーによる判定の違いをお試しいただけるようになっています。

現在では、C#で作成したWindowsアプリケーション単独で匂いの学習と判定まで実施できるようになっていますが、それができるようになる前まではpythonのモデルを使用して匂いの判定をしていました。このリポジトリには、その名残のpythonスクリプトが登録してあります。

## 全体像

このリポジトリは

![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview00.png?raw=true)

- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルまたはデータベースへ出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
  - ML.NET を使用して、多クラス分類を行うようトレーニングしたモデルを作り、そのモデルを使って、匂いの判別をできるようにする
  - [ML.NET での機械学習のタスク](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)で紹介されている、多クラス分類のトレーナーが選べる
- Windows(C#)アプリから送られてきたJSONデータをPostgreSQLに格納するコンテナ群(docker-database/ 以下)

以前は、pythonスクリプトで匂いの学習と予測をを行っていました。

### 以前のPythonスクリプトについて

以前の pythonスクリプトについては、以下のページを参照してください。

- [以前のPythonスクリプト](Python_predict.md)

## BME688-Atom Lite

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/AtomLite.png?raw=true)

BME688 と Atom Lite についての詳細は [こちら](Atom_lite.md) を参照してください。

## Windows App

![WindowsApp](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview0.png?raw=true)

Windows Appの詳細は [こちら](Windows_app.md) を参照してください。

以上
