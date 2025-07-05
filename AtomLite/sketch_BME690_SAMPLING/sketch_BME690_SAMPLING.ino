// M5Atom で BME690 のガスセンサ・ヒーターを操作するサンプル
//   https://gist.githubusercontent.com/ksasao/5505e0e59a97cde799cf0ed2d2009b2d/raw/ec847139310ed5a3e51c163d95d8062d3e8f5d3d/M5BME688.ino
// 2022/1/6 @ksasao
// 2025/07/03  @MRSa  BME690向けに調整 (M5Unified化、Atom S3 Lite対応、bme688Library をBME690に変更し、リポジトリ内に取り込み、M5Unified化)
// 2025/07/05  @MRSa  温度プロファイルをシリアル経由で変更できるようにした (区切り文字：0x0a / New Line)
//
//    [コマンド]  ※ シリアル経由で受け付けるコマンド
//      CMD:RESTART  → Atom Liteを再起動
//      CMD:GETPROF  → 現在の設定取得
//        （応答例）{"pf":false,"name":"default","tempProf":[320,100,100,100,200,200,200,320,320,320],"holdProf":[5,2,10,30,5,5,5,5,5,5]}
//      CMD:EXTON    → 拡張モードON  (温度プロファイルも応答)
//      CMD:EXTOFF   → 拡張モードOFF (温度プロファイルは応答しない)
//      CMD:SETDEFAULT → 標準の温度プロファイルを設定し、再起動する
//      CMD:SETPROF {..(JSONフォーマット)..}  → 温度プロファイルを設定する
//        （コマンド例）
//            CMD:SETPROF {"name":"HP-xxx","tempProf":[200, 200, 320, 320, 320, 200, 200, 100, 100, 100],"holdProf":[3, 9, 3, 10, 10, 3, 10, 3, 10, 10]}
//            CMD:SETPROF {"name":"HP-yyy","tempProf":[200, 200, 320, 250, 280, 150, 200, 100, 250, 100],"holdProf":[5, 10, 5, 10, 10, 5, 10, 5, 10, 10]}


#include "ESP.h"
#include "M5Unified.h"
#include "src/bme690Library/bme690Library.h"
#include "Preferences.h"
#include <ArduinoJson.h>
#include <FastLED.h>
CRGB mainLED;

#define NEW_GAS_MEAS (BME69X_GASM_VALID_MSK | BME69X_HEAT_STAB_MSK | BME69X_NEW_DATA_MSK)
#define MEAS_DUR 100
#define WAIT_DUR 20

// Error LED Control
#define ERROR_DUR 1000
void errLeds(void);

// Grove (I2C SDA/SCL)
#if defined(ARDUINO_M5STACK_ATOM)
#define SDA_PIN 26
#define SCL_PIN 32
#else  // #if defined(ARDUINO_M5STACK_ATOMS3)
#define SDA_PIN 2
#define SCL_PIN 1
#endif

// I2C: Address
#define BME690_I2C_ADDR_1ST 0x76
#define BME690_I2C_ADDR_2ND 0x77

#include <math.h>
Bme69x bme;

bool expandedMode = false;
String profileName;
uint16_t temperatureProfile[10];
uint16_t maintainProfile[10];


bool parseProfileFromJson(String &jsonString)
{
  DynamicJsonDocument profileJson(1024);
  
  DeserializationError error = deserializeJson(profileJson, jsonString);
  switch (error.code())
  {
    case DeserializationError::Ok:
      // ---- デシリアライズ成功、次の処理に。
      break;

    default:
      Serial.print(" RECEIVED PROFILE DATA ERROR: ");
      Serial.println(jsonString.c_str());
      Serial.println("");
      return (false);
  }

  const char* profName = profileJson["name"] | "Unknown";
  JsonArray tempProf = profileJson["tempProf"];
  JsonArray holdProf = profileJson["holdProf"];
  if ((tempProf.size() != 10)||(holdProf.size() != 10))
  {
    Serial.print(" Specified array size is wrong. : ");
    Serial.println(jsonString.c_str());
    Serial.println("");
    return (false);
  }
  Preferences preferences;
  preferences.begin("profile");
    profileName = String(profName);

    temperatureProfile[0] = tempProf[0].as<uint16_t>();
    temperatureProfile[1] = tempProf[1].as<uint16_t>();
    temperatureProfile[2] = tempProf[2].as<uint16_t>();
    temperatureProfile[3] = tempProf[3].as<uint16_t>();
    temperatureProfile[4] = tempProf[4].as<uint16_t>();
    temperatureProfile[5] = tempProf[5].as<uint16_t>();
    temperatureProfile[6] = tempProf[6].as<uint16_t>();
    temperatureProfile[7] = tempProf[7].as<uint16_t>();
    temperatureProfile[8] = tempProf[8].as<uint16_t>();
    temperatureProfile[9] = tempProf[9].as<uint16_t>();

    maintainProfile[0] = holdProf[0].as<uint16_t>();
    maintainProfile[1] = holdProf[1].as<uint16_t>();
    maintainProfile[2] = holdProf[2].as<uint16_t>();
    maintainProfile[3] = holdProf[3].as<uint16_t>();
    maintainProfile[4] = holdProf[4].as<uint16_t>();
    maintainProfile[5] = holdProf[5].as<uint16_t>();
    maintainProfile[6] = holdProf[6].as<uint16_t>();
    maintainProfile[7] = holdProf[7].as<uint16_t>();
    maintainProfile[8] = holdProf[8].as<uint16_t>();
    maintainProfile[9] = holdProf[9].as<uint16_t>();

    preferences.putString("profileName", profileName);
    preferences.putUShort("tp0", temperatureProfile[0]);
    preferences.putUShort("tp1", temperatureProfile[1]);
    preferences.putUShort("tp2", temperatureProfile[2]);
    preferences.putUShort("tp3", temperatureProfile[3]);
    preferences.putUShort("tp4", temperatureProfile[4]);
    preferences.putUShort("tp5", temperatureProfile[5]);
    preferences.putUShort("tp6", temperatureProfile[6]);
    preferences.putUShort("tp7", temperatureProfile[7]);
    preferences.putUShort("tp8", temperatureProfile[8]);
    preferences.putUShort("tp9", temperatureProfile[9]);

    preferences.putUShort("mp0", maintainProfile[0]);
    preferences.putUShort("mp1", maintainProfile[1]);
    preferences.putUShort("mp2", maintainProfile[2]);
    preferences.putUShort("mp3", maintainProfile[3]);
    preferences.putUShort("mp4", maintainProfile[4]);
    preferences.putUShort("mp5", maintainProfile[5]);
    preferences.putUShort("mp6", maintainProfile[6]);
    preferences.putUShort("mp7", maintainProfile[7]);
    preferences.putUShort("mp8", maintainProfile[8]);
    preferences.putUShort("mp9", maintainProfile[9]);

    preferences.putBool("isEntry", true);
  preferences.end();

  Serial.print(" ACCEPTED NEW PROFILE : ");
  Serial.println(profileName.c_str());
  Serial.println(" - - -  RESTART - - - ");
  Serial.println("");
  delay(WAIT_DUR);
  return (true);
}

bool checkReceivedString(String &str)
{
  if (str.indexOf("CMD:RESTART") == 0)
  {
    Serial.println(" - - - RESTART - - - ");
    delay(WAIT_DUR);
    return (true);
  } else if (str.indexOf("CMD:GETPROF") == 0)
  {
    // ----- 現在のプロファイルを応答する
    DynamicJsonDocument profileJson(1024);
    profileJson["pf"] = expandedMode;
    profileJson["name"] = profileName;
    JsonArray tempProf = profileJson.createNestedArray("tempProf");
    JsonArray holdProf = profileJson.createNestedArray("holdProf");
    for (int index = 0; index < 10; index++)
    {
      tempProf.add(temperatureProfile[index]);
      holdProf.add(maintainProfile[index]);
    }
    String outputJson;
    serializeJson(profileJson, outputJson);
    Serial.println(outputJson);
    Serial.println("");
  }
  else if ((str.indexOf("CMD:SETPROF") == 0)&&(str.indexOf("{") > 0)&&(str.indexOf("}") > 0))
  {
    // ----- 温度プロファイルを受信、更新する
    int index = str.indexOf("{");
    String jsonString = str.substring(index);
    return (parseProfileFromJson(jsonString));
  }
  else if (str.indexOf("CMD:SETDEFAULT") == 0)
  {
    // ----- デフォルトのプロファイルに戻す
    Serial.println(" - - - SET DEFAULT PROFILE - - - ");
    applyDefaultProfile();

    Serial.println(" - - - - - - RESTART - - - - - - ");
    delay(WAIT_DUR);
    return (true);
  }
 else if (str.indexOf("CMD:EXTON") == 0)
  {
    // ----- 拡張モード ON
    Preferences preferences;
    preferences.begin("profile");
    preferences.putBool("ex", true);
    preferences.end();
    expandedMode = true;

    Serial.println(" - - - EXT MODE : ON - - - ");
  }
  else if (str.indexOf("CMD:EXTOFF") == 0)
  {
    // ----- 拡張モード OFF
    Preferences preferences;
    preferences.begin("profile");
    preferences.putBool("ex", false);
    preferences.end();
    expandedMode = false;

    Serial.println(" - - - EXT MODE : OFF - - - ");
  }

  return (false);
}

void applyDefaultProfile()
{
  Preferences preferences;
  preferences.begin("profile");
    profileName = String("default");

    temperatureProfile[0] = 320;
    temperatureProfile[1] = 100;
    temperatureProfile[2] = 100;
    temperatureProfile[3] = 100;
    temperatureProfile[4] = 200;
    temperatureProfile[5] = 200;
    temperatureProfile[6] = 200;
    temperatureProfile[7] = 320;
    temperatureProfile[8] = 320;
    temperatureProfile[9] = 320;

    maintainProfile[0] = 5;
    maintainProfile[1] = 2;
    maintainProfile[2] = 10;
    maintainProfile[3] = 30;
    maintainProfile[4] = 5;
    maintainProfile[5] = 5;
    maintainProfile[6] = 5;
    maintainProfile[7] = 5;
    maintainProfile[8] = 5;
    maintainProfile[9] = 5;

    preferences.putString("profileName", profileName);
    preferences.putUShort("tp0", temperatureProfile[0]);
    preferences.putUShort("tp1", temperatureProfile[1]);
    preferences.putUShort("tp2", temperatureProfile[2]);
    preferences.putUShort("tp3", temperatureProfile[3]);
    preferences.putUShort("tp4", temperatureProfile[4]);
    preferences.putUShort("tp5", temperatureProfile[5]);
    preferences.putUShort("tp6", temperatureProfile[6]);
    preferences.putUShort("tp7", temperatureProfile[7]);
    preferences.putUShort("tp8", temperatureProfile[8]);
    preferences.putUShort("tp9", temperatureProfile[9]);

    preferences.putUShort("mp0", maintainProfile[0]);
    preferences.putUShort("mp1", maintainProfile[1]);
    preferences.putUShort("mp2", maintainProfile[2]);
    preferences.putUShort("mp3", maintainProfile[3]);
    preferences.putUShort("mp4", maintainProfile[4]);
    preferences.putUShort("mp5", maintainProfile[5]);
    preferences.putUShort("mp6", maintainProfile[6]);
    preferences.putUShort("mp7", maintainProfile[7]);
    preferences.putUShort("mp8", maintainProfile[8]);
    preferences.putUShort("mp9", maintainProfile[9]);

    preferences.putBool("isEntry", true);
  preferences.end();
}


/**
 * @brief Initializes the sensor and hardware settings
 */
void setup(void)
{
  auto cfg = M5.config();
  cfg.serial_baudrate = 115200;
  cfg.pmic_button = false;
  cfg.output_power = true;
  cfg.clear_display = true;
  cfg.led_brightness = 96;
  M5.begin(cfg);

#if defined(ARDUINO_M5STACK_ATOM)
  // ----- RGB LED OFF : for M5 Atom Lite
  pinMode(27, OUTPUT);
  FastLED.addLeds<NEOPIXEL, 27>(&mainLED, 1);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
  // ----- RGB LED OFF : for M5 Atom S3 Lite
  FastLED.addLeds<NEOPIXEL, 35>(&mainLED, 1);
#endif

  delay(WAIT_DUR);

  //  LED OFF
  FastLED.setBrightness(10);
  mainLED = CRGB::Black;
#if defined(ARDUINO_M5STACK_ATOM)
  neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
  FastLED.show();
#endif

  Wire.begin(SDA_PIN, SCL_PIN);

  /* initializes the sensor based on I2C library */
  bme.begin(BME690_I2C_ADDR_2ND, Wire);

  if((bme.checkStatus())&&(bme.checkStatus() == BME69X_ERROR))
  {
     Serial.println(" TRY : ADDR_1ST");
     bme.begin(BME690_I2C_ADDR_1ST, Wire);
  }

  if(bme.checkStatus())
  {
    if (bme.checkStatus() == BME69X_ERROR)
    {
      Serial.println("Sensor error:" + bme.statusString());
      errLeds();
      return;
    }
    else if (bme.checkStatus() == BME69X_WARNING)
    {
      Serial.println("Sensor Warning:" + bme.statusString());
    }
  }
  
  /* Set the default configuration for temperature, pressure and humidity */
  bme.setTPH();

  // Preferenceから温度設定情報を読み込む。（読み込めない場合はデフォルト値を設定）

  Preferences preferences;
  preferences.begin("profile");
  bool isProfileEntry = preferences.getBool("isEntry");
  if (!isProfileEntry)
  {
    profileName = String("default");

    temperatureProfile[0] = 320;
    temperatureProfile[1] = 100;
    temperatureProfile[2] = 100;
    temperatureProfile[3] = 100;
    temperatureProfile[4] = 200;
    temperatureProfile[5] = 200;
    temperatureProfile[6] = 200;
    temperatureProfile[7] = 320;
    temperatureProfile[8] = 320;
    temperatureProfile[9] = 320;

    maintainProfile[0] = 5;
    maintainProfile[1] = 2;
    maintainProfile[2] = 10;
    maintainProfile[3] = 30;
    maintainProfile[4] = 5;
    maintainProfile[5] = 5;
    maintainProfile[6] = 5;
    maintainProfile[7] = 5;
    maintainProfile[8] = 5;
    maintainProfile[9] = 5;

    preferences.putString("profileName", profileName);
    preferences.putUShort("tp0", temperatureProfile[0]);
    preferences.putUShort("tp1", temperatureProfile[1]);
    preferences.putUShort("tp2", temperatureProfile[2]);
    preferences.putUShort("tp3", temperatureProfile[3]);
    preferences.putUShort("tp4", temperatureProfile[4]);
    preferences.putUShort("tp5", temperatureProfile[5]);
    preferences.putUShort("tp6", temperatureProfile[6]);
    preferences.putUShort("tp7", temperatureProfile[7]);
    preferences.putUShort("tp8", temperatureProfile[8]);
    preferences.putUShort("tp9", temperatureProfile[9]);

    preferences.putUShort("mp0", maintainProfile[0]);
    preferences.putUShort("mp1", maintainProfile[1]);
    preferences.putUShort("mp2", maintainProfile[2]);
    preferences.putUShort("mp3", maintainProfile[3]);
    preferences.putUShort("mp4", maintainProfile[4]);
    preferences.putUShort("mp5", maintainProfile[5]);
    preferences.putUShort("mp6", maintainProfile[6]);
    preferences.putUShort("mp7", maintainProfile[7]);
    preferences.putUShort("mp8", maintainProfile[8]);
    preferences.putUShort("mp9", maintainProfile[9]);

    preferences.putBool("ex", false);
    preferences.putBool("isEntry", true);
  }
  else
  {
    profileName = preferences.getString("profileName");
    expandedMode = preferences.getBool("ex", false);

    temperatureProfile[0] = preferences.getUShort("tp0", 320);
    temperatureProfile[1] = preferences.getUShort("tp1", 100);
    temperatureProfile[2] = preferences.getUShort("tp2", 100);
    temperatureProfile[3] = preferences.getUShort("tp3", 100);
    temperatureProfile[4] = preferences.getUShort("tp4", 200);
    temperatureProfile[5] = preferences.getUShort("tp5", 200);
    temperatureProfile[6] = preferences.getUShort("tp6", 200);
    temperatureProfile[7] = preferences.getUShort("tp7", 320);
    temperatureProfile[8] = preferences.getUShort("tp8", 320);
    temperatureProfile[9] = preferences.getUShort("tp9", 320);

    maintainProfile[0] = preferences.getUShort("mp0", 5);
    maintainProfile[1] = preferences.getUShort("mp1", 2);
    maintainProfile[2] = preferences.getUShort("mp2", 10);
    maintainProfile[3] = preferences.getUShort("mp3", 30);
    maintainProfile[4] = preferences.getUShort("mp4", 5);
    maintainProfile[5] = preferences.getUShort("mp5", 5);
    maintainProfile[6] = preferences.getUShort("mp6", 5);
    maintainProfile[7] = preferences.getUShort("mp7", 5);
    maintainProfile[8] = preferences.getUShort("mp8", 5);
    maintainProfile[9] = preferences.getUShort("mp9", 5);
  }
  preferences.end();

  /* ヒーターの温度(℃)の１サイクル分の温度変化。 200-400℃程度を指定。配列の長さは最大10。*/
  // uint16_t tempProf[10] = { 320, 100, 100, 100, 200, 200, 200, 320, 320, 320 }; // HP-354 (Standard Heater profile)

  /* ヒーターの温度を保持する時間の割合。数値×MEAS_DUR(ms)保持される。保持時間は1～4032ms。指定温度に達するまで20-30ms程度が必要。 */
  // uint16_t mulProf[10] = { 5, 2, 10, 30, 5, 5, 5, 5, 5, 5 };                    // HP-354 (Standard Heater profile)

  /* 各測定(温度,湿度,気圧,抵抗値)の繰り返し間隔(MEAS_DUR)から測定にかかる正味時間を引いたものをsharedHeatrDurに設定 */
  uint16_t sharedHeatrDur = MEAS_DUR - (bme.getMeasDur(BME69X_PARALLEL_MODE) / 1000);

  // bme.setHeaterProf(tempProf, mulProf, sharedHeatrDur, 10);
  bme.setHeaterProf(temperatureProfile, maintainProfile, sharedHeatrDur, 10);
  bme.setOpMode(BME69X_PARALLEL_MODE);

  // ---------- 開始メッセージを出力
  Serial.println("");
  Serial.println(" - - - - - ");
  Serial.print(" BME690 '");
  Serial.print(profileName.c_str());
  Serial.println("' START!");
  Serial.println(" - - - - - ");
}

float last = 0;

void loop(void)
{
  // 構造体の定義は src/bme690Library/bme69x/bme69x_defs.h 参照
  bme69xData data;   // struct bme69x_data
  uint8_t nFieldsLeft = 0;

  mainLED = CRGB::Blue;
#if defined(ARDUINO_M5STACK_ATOM)
  neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
  FastLED.show();
#endif
  delay(MEAS_DUR);

//  ------------------------------
  if (Serial.available() > 0)
  {
    // --- コード 0x0a まで読み出す
    String readString = Serial.readStringUntil(0x0a);
    if (checkReceivedString(readString))
    {
      // ----- センサのソフトリセットをして再起動。
      bme.softReset();
      ESP.restart();
      return;
    }
  }
//  ------------------------------

  if (bme.fetchData())
  {
    do
    {
      M5.update();

      nFieldsLeft = bme.getData(data);
      if (data.status == NEW_GAS_MEAS)
      {
        if(data.gas_index == 9){
          mainLED = CRGB::Magenta;
        }else{
          mainLED = CRGB::Blue;
        }

#if defined(ARDUINO_M5STACK_ATOM)
        neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
        FastLED.show();
#endif

        // ちょっと出力データを追加。
        Serial.print(",");
        Serial.print(String(data.gas_index)+",");
        Serial.print(String(data.meas_index) + ",");
        Serial.print(String(millis()) + ",");
        Serial.print(String(data.status) + ",");
        Serial.print(String(data.gas_wait) + ",");
        Serial.print(String(data.temperature) + ","); // 周囲の温度湿度も結構影響があります
        Serial.print(String(data.humidity) + ",");
        Serial.print(String(data.pressure) + ",");
        Serial.print(String(data.gas_resistance) + ",");
        float current = log(data.gas_resistance); // 値の変動が大きいので対数をとるといい感じです
        Serial.print(String(current,6)+",");      // 精度をちょっと拡大
        Serial.print(String(current-last+10,3));  // ガスの脱着は温度変化に敏感なので差分もつかうと良いです
        Serial.print(",;");
        if ((expandedMode)&&(data.gas_index >= 0)&&(data.gas_index <= 9))
        {
          Serial.print(",pf,");
          Serial.print(profileName.c_str());
          Serial.print(",");
          Serial.print(temperatureProfile[data.gas_index]);
          Serial.print(",");
          Serial.print(maintainProfile[data.gas_index]);
          Serial.print(",;");
        }
        Serial.println();
        last = current;

        delay(WAIT_DUR);
        mainLED = CRGB::Black;
#if defined(ARDUINO_M5STACK_ATOM)
        neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
        FastLED.show();
#endif
      }
    } while (nFieldsLeft);
  }
}

void errLeds(void)
{
    while(1)
    {
        //  LED ON
        mainLED = CRGB::Red;
#if defined(ARDUINO_M5STACK_ATOM)
        neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
        FastLED.show();
#endif
        delay(ERROR_DUR);

        //  LED OFF
        mainLED = CRGB::Black;
#if defined(ARDUINO_M5STACK_ATOM)
        neopixelWrite(27, mainLED.red, mainLED.green, mainLED.blue);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
        FastLED.show();
#endif
        delay(ERROR_DUR);
    }
}
