# SamplingBME688Serial : BME688でにおいを判別する（一式）

-----

- [SamplingBME688Serial : BME688でにおいを判別する（一式）](#samplingbme688serial--bme688でにおいを判別する一式)
  - [リポジトリ](#リポジトリ)
  - [概要](#概要)
  - [全体像](#全体像)
    - [以前のPythonスクリプトについて](#以前のpythonスクリプトについて)
  - [BME688-Atom Lite](#bme688-atom-lite)
  - [Windows アプリケーション](#windows-アプリケーション)
  - [Database on Docker](#database-on-docker)
  - [サンプルデータ](#サンプルデータ)

-----

## リポジトリ

[https://github.com/MRSa/SamplingBME688Serial](https://github.com/MRSa/SamplingBME688Serial)

## 概要

BOSCH社のAIガスセンサ[BME688](https://www.bosch-sensortec.com/products/environmental-sensors/gas-sensors/bme688/) と [ATOM Lite](https://docs.m5stack.com/en/core/atom_lite)を最大２つWindows PCに接続し、C#で作成したWindowsアプリケーションで「におい」を学習し、学習結果を使って、どの「匂い」かを当てる（ことができるかもしれない）ようにしたシステムを提供します。

「匂い」の学習には、.NET用の機械学習フレームワーク([ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet))が提供する、[多クラス分類のトレーナー](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)を使用でき、各トレーナーによる判定の違いをお試しいただけるようになっています。

現在では、C#で作成したWindowsアプリケーション単独で匂いの学習と判定まで実施できるようになっていますが、それができるようになる前まではpythonのモデルを使用して匂いの判定をしていました。このリポジトリには、その名残のpythonスクリプトが登録してあります。

## 全体像

このリポジトリには、以下が格納されています。

![Overview](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview00.png?raw=true)

- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルまたはデータベースへ出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
  - ML.NET を使用して、多クラス分類を行うようトレーニングしたモデルを作り、そのモデルを使って、匂いの判別をできるようにする
  - 匂いの判別には、[ML.NET での機械学習のタスク](https://learn.microsoft.com/ja-jp/dotnet/machine-learning/resources/tasks#multiclass-classification)で紹介されている、多クラス分類のトレーナーが選べる
- Windows(C#)アプリから送られてきたJSONデータをPostgreSQLに格納するコンテナ群(docker-database/ 以下)

上記のほか、pythonスクリプトで匂いの学習と予測を行うスクリプトがあります。（現在は Windows(C#)アプリ自体に機械学習を行う機能を搭載したため、利用していません。）

### 以前のPythonスクリプトについて

以前の pythonスクリプトについては、以下のページを参照してください。

- [以前のPythonスクリプト](python/README.md)

## BME688-Atom Lite

BME688とAtom LiteをI2Cで接続し、それをWindows PCにつないで、センサーのデータをWindows Appに送ります。

![Atom Lite](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/AtomLite.png?raw=true)

BME688 と Atom Lite についての詳細は [こちら](AtomLite/README.md) を参照してください。

## Windows アプリケーション

Atom Liteから送られてきたBME688の匂いデータについて、機械学習を行い、学習したデータを使ってどの匂いか判定させるアプリです。

![WindowsApp](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/overview0.png?raw=true)

以下の３ステップで匂いを判別します。

1. 匂いを記憶させる (区別したい匂いの種類分、すべて記憶させます)
2. 匂いを判別できるよう、学習させる
3. 学習したモデルを使って、匂いを判別する

Windows アプリケーションの詳細は [こちら](SerialCommBME688/README.md) を参照してください。

## Database on Docker

においデータをあとで活用するために、WSL (Windows Subsystem for Linux)上で動くDockerのデータベース(PostgreSQL)に登録することができます。
ただし、まだ登録したデータを読み込むことはできません。。

Docker databaseの詳細は [こちら](docker-database/README.md) を参照してください。

## サンプルデータ

BME688で収集したデータを置いています。

詳細は [こちら](sample_data/README.md) を参照してください。

以上
<!----


---->