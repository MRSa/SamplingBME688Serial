# Database on Docker

においデータをあとで活用するために、Web API経由で、WSL (Windows Subsystem for Linux)上で動くDockerコンテナのデータベース(PostgreSQL)に登録することができます。

![Database on Docker](https://github.com/MRSa/SamplingBME688Serial/blob/master/images/docker-database.png?raw=true)

## Web API一覧

### 登録されているカテゴリの一覧： sensor/list

データベースに登録されているカテゴリとデータカウントをJSON形式で応答します。

### 登録されているセンサデータの取得： sensor/get

データベースに登録されているデータを取得します。取得範囲を指定することができます。
指定可能なパラメータは以下です。

* category:  カテゴリ
* sensor_id: センサID
* limit:     最大取得件数(省略可)
* offset:    先頭位置(省略可)
* option:    オプション(省略可) : 指定した場合、すべてのセンサデータを取得する、未指定の場合は gas_index と gas_registance_log のみ登録する

### センサデータの登録： sensor/entry

POSTメソッドでセンサデータを登録します

* name
* sensor_id
* category
* data
  * datetime
  * gas_index
  * temperature
  * humidity
  * pressure
  * gas_registance
  * gas_registance_log
  * gas_registance_diff

-----

## 登録センサデータ サンプル

```json
{
  "result": {
    "category": "empty",
    "data": [
      {
        "category": "empty",
        "comment": null,
        "datetime": "Thu, 09 Mar 2023 23:58:55 GMT",
        "gas_registance": 10663196.0,
        "gas_registance_diff": 7.738,
        "gas_registance_log": 16.182308,
        "humidity": 29.94,
        "index": 0,
        "pressure": 101627.94,
        "sensor_id": 1,
        "serial": 3031,
        "temperature": 28.75
      }
    ],
    "sensor_id": "1"
  }
}
```

### カテゴリ一覧サンプル

```json
{
  "result": [
    {
      "category": "RoastedGreenTea",
      "count": 1520,
      "sensor_id": 1
    },
    {
      "category": "RoastedGreenTea",
      "count": 1510,
      "sensor_id": 2
    },
    {
      "category": "empty",
      "count": 1850,
      "sensor_id": 1
    },
    {
      "category": "empty",
      "count": 1850,
      "sensor_id": 2
    }
  ]
}
```

### 登録データサンプル

```json
{
    "name": "sensor_data",
    "sensor_id": 0,
    "category": "dummy00",
    "data" : {
        "datetime": "2023-01-01 00:00:00",
        "gas_index": 0,
        "temperature": 26.5,
        "humidity": 26.31,
        "pressure": 101019.08,
        "gas_registance": 6090706.5,
        "gas_registance_log": 15.622274,
        "gas_registance_diff": 11.457
    }
}
```
