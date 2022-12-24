# SamplingBME688Serial : BME688でにおいを判別する（一式）

## リポジトリ
https://github.com/MRSa/SamplingBME688Serial


![Overview images](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/Overview.png?raw=true)


## リポジトリ概要
- Atom LiteにGrove(I2C)経由で接続したBME688から、シリアルポートにデータを出力するスケッチ (AtomLite/sketch_BME688_SAMPLING.ino)
- シリアルポート経由でつないだAtom Liteから、BME688のデータを受信してCSVファイルに出力するWindows(C#)アプリ (SerialCommBME688/ 以下)
- CSV形式のファイルから、LSTMで学習しH5形式のモデルファイルを作成する pythonスクリプト (python/create_smell_model.py)
- 学習したH５形式のモデルファイルを使って、CSV形式のファイルに記録していたデータの種別を判定する (python/check_smell.py)

