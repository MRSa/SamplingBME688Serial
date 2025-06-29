// M5Atom で BME690 のガスセンサ・ヒーターを操作するサンプル
//   https://gist.githubusercontent.com/ksasao/5505e0e59a97cde799cf0ed2d2009b2d/raw/ec847139310ed5a3e51c163d95d8062d3e8f5d3d/M5BME688.ino
// 2022/1/6 @ksasao
//
// 2025/06/29  @MRSa  BME690向けに調整 (M5Unified化、Atom S3 Lite対応、bme688Library をBME690に変更し、リポジトリ内に取り込み、M5Unified化)
// 

#include "M5Unified.h"
#include "src/bme690Library/bme690Library.h"

#include <FastLED.h>
CRGB mainLED;

#define NEW_GAS_MEAS (BME69X_GASM_VALID_MSK | BME69X_HEAT_STAB_MSK | BME69X_NEW_DATA_MSK)
#define MEAS_DUR 100
#define WAIT_DUR 20

//  Error LED Control
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

//  I2C: Address
#define BME690_I2C_ADDR_1ST 0x76
#define BME690_I2C_ADDR_2ND 0x77

#include <math.h>
Bme69x bme;

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
    FastLED.addLeds<NEOPIXEL, 27>(&mainLED, 1);
#else // #if defined(ARDUINO_M5STACK_ATOMS3)
  // ----- RGB LED OFF : for M5 Atom S3 Lite
  FastLED.addLeds<NEOPIXEL, 35>(&mainLED, 1);
#endif

  delay(50);

  //  LED OFF
  FastLED.setBrightness(10);
  mainLED = CRGB::Black;
  FastLED.show();

  //Serial.begin(115200);
  Wire.begin(SDA_PIN, SCL_PIN);

  ////while (!Serial)
  //{
  //  delay(10);
  //}

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

  /* ヒーターの温度(℃)の１サイクル分の温度変化。 200-400℃程度を指定。配列の長さは最大10。*/
  uint16_t tempProf[10] = { 320, 100, 100, 100, 200, 200, 200, 320, 320, 320 }; // HP-354 (Standard Heater profile)
  //uint16_t tempProf[10] = { 200, 200, 320, 320, 320, 200, 200, 100, 100, 100 };
  //uint16_t tempProf[10] = { 200, 200, 320, 250, 280, 150, 200, 100, 250, 100 };

  /* ヒーターの温度を保持する時間の割合。数値×MEAS_DUR(ms)保持される。保持時間は1～4032ms。指定温度に達するまで20-30ms程度が必要。 */
  uint16_t mulProf[10] = { 5, 2, 10, 30, 5, 5, 5, 5, 5, 5 };                    // HP-354 (Standard Heater profile)
  //uint16_t mulProf[10] = { 3, 9, 3, 10, 10, 3, 10, 3, 10, 10 };
  //uint16_t mulProf[10] = { 5, 10, 5, 10, 10, 5, 10, 5, 10, 10 };

  /* 各測定(温度,湿度,気圧,抵抗値)の繰り返し間隔(MEAS_DUR)から測定にかかる正味時間を引いたものをsharedHeatrDurに設定 */
  uint16_t sharedHeatrDur = MEAS_DUR - (bme.getMeasDur(BME69X_PARALLEL_MODE) / 1000);

  bme.setHeaterProf(tempProf, mulProf, sharedHeatrDur, 10);
  bme.setOpMode(BME69X_PARALLEL_MODE);
}

float last = 0;

void loop(void)
{
  // 構造体の定義は src/bme690Library/bme69x/bme69x_defs.h 参照
  bme69xData data;   // struct bme69x_data
  uint8_t nFieldsLeft = 0;

  mainLED = CRGB::Blue;
  FastLED.show();
  delay(WAIT_DUR);

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
        FastLED.show();

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
        Serial.println();
        last = current;

        delay(WAIT_DUR);
        mainLED = CRGB::Black;
        FastLED.show();
        delay(WAIT_DUR);
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
        FastLED.show();
        delay(ERROR_DUR);

        //  LED OFF
        mainLED = CRGB::Black;
        FastLED.show();
        delay(ERROR_DUR);
    }
}
