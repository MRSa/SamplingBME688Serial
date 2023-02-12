# ========== web-service.py : BME688のセンサデータをDB (PostgreSQL)に取り込む (受付部分)
import os
import sys
from flask import Flask, Response, request, jsonify
from flask_cors import CORS
import StorageAdapter

#  --------------- flask with CORS
app = Flask(__name__)
app.config["JSON_AS_ASCII"] = False
CORS(app)

#  --------------- DBアクセス部分を準備
adapter = StorageAdapter.StorageAdapter()

#  --------------- トップページ
@app.route("/")
def base_page():
    return ("<hr><br><p>Hello!</p><hr><br>")

#  --------------- データ登録
@app.route('/sensor/entry', methods=['POST'])
def entry_data():
    # --- 要求ヘッダの内容確認
    if ((request.headers['Content-Type']).casefold()).find('application/json'.casefold()):
    #if 'application/json'.casefold() in ((request.headers['Content-Type']).casefold()):
        print(request.headers['Content-Type'], file=sys.stderr)
        return jsonify(res='error'), 400

    # ----- DB登録(本処理)の呼び出し
    result = adapter.entry(request.json)

    # ----- DBアクセス部分の応答（true or false）によって、応答コードを変える
    if result == False:
        #  --- return 406: NOT ACCEPTABLE
        return jsonify(res='error'), 406
    # --- return 200: OK
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
    option_debug = False
    if os.getenv("SENSOR_DB_OPTION") == "debug":
        option_debug = True

    app.run(host="0.0.0.0", port=os.getenv("SENSOR_SERVICE_PORT"), debug=option_debug)

# ------------  デバッグについて (POST) -----
#   +++++ Chromeで 空白ページを 開いて、開発者ツールのコンソールで以下を実行する +++++
# var xhr = new XMLHttpRequest();
# xhr.open('POST', 'http://localhost:3010/sensor/entry');
# xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
# xhr.send(JSON.stringify({
#     data: 'XYZ'
# }));
