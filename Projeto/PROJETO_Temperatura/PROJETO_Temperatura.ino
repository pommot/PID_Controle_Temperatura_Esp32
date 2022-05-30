#include <OneWire.h> //controle de dados do sensor
#include <DallasTemperature.h> //biblioteca do sensor
#include "BluetoothSerial.h"

const int TIP = 15;
const int freq = 25000;
const int ledChannel = 0;
const int resolution = 8;
double kp = 0, ki = 0 , kd = 0;
float setpoint = 35;

BluetoothSerial SerialBT;
// define pino GPIO23 que será conectado ao DS18B20
const int oneWireBus = 23;
// Objeto que tratará da troca de dados com o sensor DS18B20
OneWire oneWire(oneWireBus);
DallasTemperature sensors(&oneWire);

void setup() {
  ledcSetup(ledChannel, freq, resolution);
  ledcAttachPin(TIP, ledChannel);
  Serial.begin(9600);

  SerialBT.begin("ESP32test");
  // inicia o sensor DS18B20
  sensors.begin();

}

void loop()
{

  //Coleae informações de temperatura
  sensors.requestTemperatures();
  double ultimatemp;
  long lastProcess = 0;
  double controle = 0, PID = 0;
  double modCTRL;
  float temperatureC = sensors.getTempCByIndex(0);
  ultimatemp = temperatureC;
  double erro = 0;
  char valorRx;
  valorRx = (char)SerialBT.read();
  if (SerialBT.available()) {
    if (valorRx == 'A') {
      kp = kp + 10;
    }
    if (valorRx == 'B') {
      kp = kp - 10;
    }
    if (valorRx == 'C') {
      ki = ki + 0.1;
    }
    if (valorRx == 'D') {
      ki = ki - 0.1;
    }

    if (valorRx == 'E') {
      kd = kd + 1;
    }
    if (valorRx == 'F') {
      kd = kd - 1;
    }
    if (valorRx == 'G') {
      setpoint  = setpoint + 1;
    }
    if (valorRx == 'H') {
      setpoint  = setpoint - 1;
    }
  }
  double P = 0, I = 0, D = 0;
  float deltaTime = millis();
  erro = setpoint - temperatureC;
  P = erro * kp;
  I = (erro * ki) * (deltaTime / 1000.0);
  D = (ultimatemp - temperatureC) * kd * (deltaTime / 1000.0);
  ultimatemp = temperatureC;
  PID = -(P + I + D);
  if (PID < 0)PID = 0;
  if (PID > 255)PID = 255;
  ledcWrite(ledChannel, PID);

  SerialBT.println(ultimatemp);
  SerialBT.println(erro);
  SerialBT.println(setpoint);
  SerialBT.println(PID);
  SerialBT.println(kp);
  SerialBT.println(ki);
  SerialBT.println(kd);
}
