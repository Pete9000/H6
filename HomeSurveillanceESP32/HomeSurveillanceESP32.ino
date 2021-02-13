#include <WiFiManager.h> // Tool for selecting available networks.
#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <EEPROM.h>
#include <Ticker.h>

#define EEPROM_SIZE 2
#define SERVER_IP "192.168.0.106"
#define SERVER_PORT "49165"


const char* sslCert = \
                      "-----BEGIN CERTIFICATE-----\n" \
                      "MIIDDTCCAfWgAwIBAgIJAPhfaYN9vYnxMA0GCSqGSIb3DQEBCwUAMBQxEjAQBgNV\n" \
                      "BAMTCWxvY2FsaG9zdDAeFw0yMTAxMjEwOTE5NDZaFw0yMjAxMjEwOTE5NDZaMBQx\n" \
                      "EjAQBgNVBAMTCWxvY2FsaG9zdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoC\n" \
                      "ggEBAPkFOAf0QAfiT9PWQYQe+5YBwwglZWvVk7tGrbCmJZjyH7AtnJOtGcuJGXZd\n" \
                      "01F27XgxBd7KgQpH8T+Ue2kTiH04HcuV5tz3CWJMdQCVXcsIBWq1cGkuM+85duhQ\n" \
                      "GnUt1xSQYQGpUy6SzybNId79uCEAcwmCjdBuvjRDc1Cc/yqQISuGA76nNnzJhFOl\n" \
                      "G166o4+6qMEiOlU7Z6u4kSx0XCgGs0BRv8HdiSVZ1XmkTysOv8CLSienJKLV2bul\n" \
                      "6KL7AXyNbYJfskSJXy/0aQnFi5doq4s+ECYREvEjmZoO2VRwyH2ryDGZpdS6GaSu\n" \
                      "P6FoTw5Kb9cA0r4g5hmE/dmMNMkCAwEAAaNiMGAwDAYDVR0TAQH/BAIwADAOBgNV\n" \
                      "HQ8BAf8EBAMCBaAwFgYDVR0lAQH/BAwwCgYIKwYBBQUHAwEwFwYDVR0RAQH/BA0w\n" \
                      "C4IJbG9jYWxob3N0MA8GCisGAQQBgjdUAQEEAQIwDQYJKoZIhvcNAQELBQADggEB\n" \
                      "ABRj/AqgH1YijCxeaO3B7OHoFgb91zYdwgKzk4wDhXh0ITD1gOq6FigKjuYjnF//\n" \
                      "0hU+MTCm9fXyDaP6v72zVATkTZDyyhdrn+GJOMhJiJBbyCVZNlKtDO5aEZOwTVK9\n" \
                      "X1M95bq1P1G+N22Arq5jvOk2CL5kQkVI3Opj+JscJDZUNv0tX91xX4r6EDeKPWOa\n" \
                      "xK9zxYqBmYm4r1ApDKicZjym0ArwQSTLGF1d+qz4ocoOthSd4f/WB0oEf76ZC8xG\n" \
                      "irdY1lC4pPshF/yFWN2miCA/3GG4GIyJ+dQ3R1OCMSTnGI66zX0O4l6qbRZl4DRX\n" \
                      "CnL4BqXdQoPvv67sKkvI/P8=\n" \
                      "-----END CERTIFICATE-----\n";

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

String jwtToken;

void ICACHE_RAM_ATTR DetectMovement();

HTTPClient http;

Ticker timerInterrupt; //Timer interrupt

void setup() {
  WiFi.mode(WIFI_STA); // Setting WiFi to station mode

  Serial.begin(115200); //debugging with serial

  //http.useHTTP10(true); // Use http version 1.0 to use http.stream() with arduinojson

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

 
  //Serial.println(WiFi.macAddress());
  if ((WiFi.status() == WL_CONNECTED))
  {
    http.begin("https://" SERVER_IP ":" SERVER_PORT "/token", sslCert);
    int httpCode = http.GET();
    if (0 < httpCode)
    {
      if (HTTP_CODE_OK == httpCode)
      {
        jwtToken = http.getString();
        Serial.println(httpCode);
        Serial.println(jwtToken);
      }
    }
    else
    {
      Serial.print("Couldn't get JWT-token");
    }
    http.end();

    http.begin("https://" SERVER_IP ":" SERVER_PORT "/microcontroller", sslCert);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("Authorization", (String)"Bearer " += jwtToken);
    httpCode = http.GET();
    if (0 < httpCode)
    {
      if (HTTP_CODE_OK == httpCode)
      {
        String payload = http.getString();
        Serial.println(httpCode);
        Serial.println(payload);
      }
    }
    else
    {
      Serial.print("Couldn't get data");
    }
    http.end();
  }
  //LoadLastState();
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
