// M5Atom で BME688 のガスセンサ・ヒーターを操作するサンプル
//   https://gist.githubusercontent.com/ksasao/5505e0e59a97cde799cf0ed2d2009b2d/raw/ec847139310ed5a3e51c163d95d8062d3e8f5d3d/M5BME688.ino
// 2022/1/6 @ksasao  (自分環境用に微調整...I2CのIDどちらでもいけるようにした: MRSa)
// 
// 利用デバイス:
// デバイスは、下記などで入手してください
// BME688搭載 4種空気質センサモジュール（ガス/温度/気圧/湿度）
// https://www.switch-science.com/catalog/7383/
// 
// ライブラリの追加:
// https://github.com/BoschSensortec/Bosch-BME68x-Library
// で、Code > Download ZIP から ZIPファイルとしてダウンロードし、
// Arduino IDE の スケッチ > ライブラリをインクルード > .ZIP形式のライブラリをインストール
// からZIPファイルを登録してください。
//
// 参考:
//   https://twitter.com/ksasao/status/1479108937861709825
#include "M5Atom.h"
#include "bme68xLibrary.h"

#define NEW_GAS_MEAS (BME68X_GASM_VALID_MSK | BME68X_HEAT_STAB_MSK | BME68X_NEW_DATA_MSK)
#define MEAS_DUR 100
#define WAIT_DUR 20

//  Error LED Control
#define ERROR_DUR 1000
void errLeds(void);

// Atomic proto
//#define SDA_PIN 25
//#define SCL_PIN 21

// Grove
#define SDA_PIN 26
#define SCL_PIN 32

//  I2C: Address
#define BME688_I2C_ADDR_1ST 0x76
#define BME688_I2C_ADDR_2ND 0x77

#include <math.h>
Bme68x bme;

/**
 * @brief Initializes the sensor and hardware settings
 */
void setup(void)
{
  M5.begin(true, false, true);
  delay(50);

  //  LED OFF
  M5.dis.drawpix(0, 0x000000);


  Serial.begin(115200);
  Wire.begin(SDA_PIN, SCL_PIN);

  while (!Serial)
  {
    delay(10);
  }

  /* initializes the sensor based on I2C library */
  bme.begin(BME688_I2C_ADDR_2ND, Wire);

  if((bme.checkStatus())&&(bme.checkStatus() == BME68X_ERROR))
  {
     Serial.println(" TRY : ADDR_1ST");
     bme.begin(BME688_I2C_ADDR_1ST, Wire);
  }

  if(bme.checkStatus())
  {
    if (bme.checkStatus() == BME68X_ERROR)
    {
      Serial.println("Sensor error:" + bme.statusString());
      errLeds();
      return;
    }
    else if (bme.checkStatus() == BME68X_WARNING)
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
  uint16_t sharedHeatrDur = MEAS_DUR - (bme.getMeasDur(BME68X_PARALLEL_MODE) / 1000);

  bme.setHeaterProf(tempProf, mulProf, sharedHeatrDur, 10);
  bme.setOpMode(BME68X_PARALLEL_MODE);
}

float last = 0;

void loop(void)
{
  // 構造体の定義は https://raw.githubusercontent.com/BoschSensortec/Bosch-BME68x-Library/master/src/bme68x/bme68x_defs.h 参照
  bme68xData data;   // struct bme68x_data
  uint8_t nFieldsLeft = 0;
  
  M5.dis.drawpix(0, 0x0000f0);
  delay(WAIT_DUR);

  if (bme.fetchData())
  {
    do
    {
      nFieldsLeft = bme.getData(data);
      if (data.status == NEW_GAS_MEAS)
      {
        if(data.gas_index == 9){
          M5.dis.drawpix(0, 0x00f000);
        }else{
          M5.dis.drawpix(0, 0xf060f0);
        }
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
        M5.dis.drawpix(0, 0x0000f0);
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
        M5.dis.drawpix(0, 0x00ff00);
        delay(ERROR_DUR);

        //  LED OFF
        M5.dis.drawpix(0, 0x000000);
        delay(ERROR_DUR);
    }
}
