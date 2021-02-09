#include <WiFiManager.h> // Tool for selecting available networks.
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <EEPROM.h>
#include <Ticker.h>

#define EEPROM_SIZE 2
#define SERVER_IP "192.168.0.106"
#define SERVER_PORT "5000"

//IO Ports
const byte ledPin = 2;
const byte motionSensorPin = 27;

byte esp32ID;

const byte postDataTime = 5; // Interval to send data in seconds
const byte updateStateTime = 1; // sensorState refresh interval in seconds


unsigned long currentTime = millis(); //
unsigned long sensorTriggerTime = 0; // last motion detected
unsigned long refreshStateTime = 0; // last state

volatile bool motionTrigger = false;
volatile bool sensorState = false;
bool startMotionTimer = false;

void ICACHE_RAM_ATTR DetectMovement();

HTTPClient http;
Ticker timerInterrupt; //Timer interrupt

void setup() {
  WiFi.mode(WIFI_STA); // Setting WiFi to station mode

  Serial.begin(115200); //debugging with serial

  http.useHTTP10(true); // Use http version 1.0 to use http.stream() with arduinojson

  //WiFi manager settings
  WiFiManager wm; // Initialize WiFIManager
  bool res;
  res = wm.autoConnect("SetupAP", "pass1234");
  if (!res)
  {
    Serial.println("Connection failed");
    wm.resetSettings();
  }
  else
  {
    Serial.println("Connection success");
  }
  Serial.println(WiFi.macAddress());
  LoadLastState();
  pinMode(motionSensorPin, INPUT_PULLUP);
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  attachInterrupt(digitalPinToInterrupt(motionSensorPin), DetectMovement, RISING);
  timerInterrupt.attach_ms(updateStateTime * 1000, UpdateState);
  //detachInterrupt(digitalPinToInterrupt(motionSensor));
}

void loop()
{
  currentTime = millis();
  if (motionTrigger) // Motion detected
  {
    Serial.println("Motion detected");
    digitalWrite(ledPin, HIGH);
    sensorTriggerTime = millis();
    motionTrigger = false;
    startMotionTimer = true;
  }
  if (startMotionTimer && (currentTime - sensorTriggerTime > postDataTime * 1000)) //
  {
    Serial.println("Motion stopped");
    digitalWrite(ledPin, LOW);
    startMotionTimer = false;
  }
}
void IRAM_ATTR DetectMovement() {
  motionTrigger = true;
}

void LoadLastState()
{
  EEPROM.begin(EEPROM_SIZE);
  esp32ID = EEPROM.read(0); //read index 0

  // If ID is default value 255, get ID from API.
  while (255 == esp32ID)
  {
    if ((WiFi.status() == WL_CONNECTED))
    {
      http.begin("http://" SERVER_IP ":" SERVER_PORT "/Devices/?macaddress=" + (String)WiFi.macAddress());
      int httpCode = http.GET();
      if (0 < httpCode)
      {
        if (HTTP_CODE_OK == httpCode)
        {
          //StaticJsonDocument<128> json;
          DynamicJsonDocument json(2048);
          deserializeJson(json, http.getStream());
          esp32ID = json["espid"].as<byte>();
          EEPROM.write(0, esp32ID);
        }
      }
      else
      {
        Serial.print("http error");
        delay(300000); // 5 min
      }
      http.end();
    }
    else
    {
      Serial.print("WiFi error");
      delay(300000); // 5 min
    }
  }
  Serial.println(esp32ID);
  EEPROM.end();
}

byte GetESPID(String url)
{

}

void HttpPostData(String url)
{
  http.begin(url);
}

void UpdateState()
{

}
