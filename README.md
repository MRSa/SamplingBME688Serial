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

## Python Script

### create_smell_model.py

### check_smell.py

