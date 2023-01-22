# ========== web-service.py : BME688のセンサデータをDB (PostgreSQL)に取り込む
import os
import datetime
import json
from flask import Flask, Response, request, jsonify
from flask_cors import CORS
import psycopg2
#import pandas as pd
#import numpy as np
#import tflite_runtime.interpreter as tflite


#  --------------- flask with CORS
app = Flask(__name__)
app.config["JSON_AS_ASCII"] = False
CORS(app)

#  --------------- トップページ
@app.route("/")
def base_page():
    return ("<hr><br><p>Hello!</p><hr><br>")

#  --------------- データ登録
@app.route('/api/entry', methods=['POST'])
def entry_data():
    if request.headers['Content-Type'] != 'application/json':
        print(request.headers['Content-Type'])
        return jsonify(res='error'), 400
    #
    print(request.json)
    return jsonify(res='ok'), 200

#  --------------- データ情報
@app.route("/info/")
def information():
    response = "<p>Information : (host -> {0}[{1}], user -> {2})</p>".format(os.getenv("SENSOR_DB_ACCESS_HOST"), os.getenv("SENSOR_DB_NAME"), os.getenv("SENSOR_DB_USER"))
    rep = Response(response)
    rep.headers['Access-Control-Allow-Origin'] = '*'
    return (rep)

# ----- メイン部分 -----
if __name__ == "__main__":
    app.run(host="0.0.0.0", port=os.getenv("SENSOR_SERVICE_PORT"), debug=True)


# ------------  デバッグについて (POST) -----
#   +++++ Chromeで 空白ページを 開いて、開発者ツールのコンソールで以下を実行する +++++
# var xhr = new XMLHttpRequest();
# xhr.open('POST', 'http://localhost:3010/api/entry');
# xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
# xhr.send(JSON.stringify({
#     data: 'XYZ'
# }));

