#include <WiFi.h>
#include <WiFiManager.h> // Tool for selecting available networks.
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <Ticker.h>
#include <NTPClient.h>
#include <WiFiUdp.h>

// Server
#define BASE_URL "https://192.168.1.108:5000/"
#define GET_JWT_URL "api/authenticate/login/"
#define DEVICE_ID_URL "api/device/getidfrom?mac="
#define POST_URL "api/telemetry/"
#define MYSTATE_URL "api/iounit/"

const char* ntpServer = "pool.ntp.org"; // DateTime Server.
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

const char* username = "deviceuser";
const char* password = "test123!";
//IO Ports
const byte ledPin = 2;
const byte motionSensorPin = 27;
byte id = 0;
byte sensorId = 1;

const byte motionTriggerInterval = 30; // Interval beetween motion triggers in seconds
const byte updateStateTime = 60; // State refresh interval in seconds

unsigned long currentTime = millis(); //
unsigned long sensorTriggerTime = 0; // last motion detected

String jwtToken;

bool isrActive = true;
void ICACHE_RAM_ATTR DetectMovement();
volatile bool motionTrigger = false;
bool motionAlreadyTriggered = false;
bool newData = false;

Ticker timerInterrupt;
volatile bool updateState = false;
bool sensorState = true;
Ticker timerInterrupt2;//Timer interrupt
volatile bool loggedIn = false;

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, ntpServer); //, 3600, 60000);

void setup() {
  WiFi.mode(WIFI_STA); // Setting WiFi to station mode
  Serial.begin(115200); //debugging with serial

  //WiFi manager settings
  WiFiManager wm; // Initialize WiFIManager
  bool res;
  res = wm.autoConnect("SetupAP", "pass1234");
  if (!res)
  {
    //Serial.println("Connection failed");
    wm.resetSettings();
  }
  else
  {
    //Serial.println("Connection success");
  }
  while (id == 0)
  {
    GetId();
  }
  Serial.print(F("Id is: "));
  Serial.println(id);
  pinMode(motionSensorPin, INPUT_PULLUP);
  pinMode(ledPin, OUTPUT);
  digitalWrite(ledPin, LOW);
  attachInterrupt(digitalPinToInterrupt(motionSensorPin), DetectMovement, RISING);
  timerInterrupt.attach(updateStateTime , UpdateState);
}

void loop()
{
  currentTime = millis();
  if (motionTrigger && !motionAlreadyTriggered) // Motion detected
  {
    Serial.println(F("Motion detected"));
    digitalWrite(ledPin, HIGH);
    sensorTriggerTime = millis();
    motionTrigger = false;
    motionAlreadyTriggered = true;
    newData = true;

  }
  if (motionAlreadyTriggered && currentTime - sensorTriggerTime > motionTriggerInterval * 1000) //
  {
    digitalWrite(ledPin, LOW);
    Serial.println(F("Ready for new motion"));
    motionAlreadyTriggered = false;
  }

  if (WiFi.status() == WL_CONNECTED)
  {
    if (newData)
      PostSensorData();

    if (updateState)
    {
      GetSensorState();
      if (sensorState && !isrActive)
      {
        attachInterrupt(digitalPinToInterrupt(motionSensorPin), DetectMovement, RISING);
        isrActive = !isrActive;
        Serial.println("Activated sensor");
      }
      else if (!sensorState && isrActive)
      {
        detachInterrupt(motionSensorPin);
        isrActive = !isrActive;
        Serial.println("Deactivated sensor");
      }
    }
  }
  else
  {
    Serial.println(F("WiFi Error. Reconnecting"));
    WiFi.reconnect();
    delay(1000);
  }
}

///////////////////Interrupts//////////////////
void IRAM_ATTR DetectMovement()
{
  motionTrigger = true;
}

void UpdateState()
{
  updateState = true;
}

void RefreshToken()
{
  loggedIn = false;
  timerInterrupt2.detach();
}

////////////////////Interrupts END //////////////////


void GetJWT()
{
  //Check for WiFi connection
  if (WiFi.status() == WL_CONNECTED)
  {
    //Create JSON document and Serialize for request body
    DynamicJsonDocument doc(128);
    doc["username"] = username;
    doc["password"] = password;
    String jsonstr;
    serializeJson(doc, jsonstr);
    //Start HTTPclient
    HTTPClient https;
    https.useHTTP10(true);
    // Send request and get http statuscode
    https.begin(BASE_URL GET_JWT_URL, sslCert);
    https.addHeader(F("Content-Type"), F("application/json"));
    int httpCode = https.POST(jsonstr);
    Serial.println(httpCode);
    //Check for acceptable http statuscode
    if (0 < httpCode)
    {
      if (HTTP_CODE_OK == httpCode)
      {
        //Create JsonDocument and Deserialize response body
        DynamicJsonDocument json(768);
        deserializeJson(json, https.getStream());
        jwtToken = json[F("token")].as<char*>();
        byte tokenExpiration = json[F("expiration")].as<byte>();
        Serial.println(F("GetJWT success"));
        //Add timer for expiration of JWT token
        timerInterrupt2.attach((tokenExpiration - 1) * 60, RefreshToken);
        loggedIn = true;
      }
      else
      {
        Serial.print(F("Error. HTTP Statuscode = "));
        Serial.println(httpCode);
      }
    }
    else
      Serial.println(F("GetJWT Connection error"));
    https.end();
  }
  else
    Serial.println(F("GetJWT WiFi Error."));
}

void GetId()
{
  if (WiFi.status() == WL_CONNECTED)
  {
    if (!loggedIn)
    {
      GetJWT();
      if (!loggedIn)
        return;
    }
    HTTPClient https;
    https.useHTTP10(true);
    https.begin(BASE_URL DEVICE_ID_URL + (String)WiFi.macAddress(), sslCert);
    https.addHeader(F("Authorization"), "Bearer " + jwtToken);
    int httpCode = https.GET();
    Serial.println(httpCode);
    if (0 < httpCode)
    {
      if (HTTP_CODE_OK == httpCode)
      {
        DynamicJsonDocument json(192);
        deserializeJson(json, https.getStream());
        id = json[F("deviceId")].as<byte>();
        Serial.println(F("GetId Success"));
      }
      else
      {
        Serial.print(F("Error. HTTP Statuscode = "));
        Serial.println(httpCode);
      }
    }
    else
    {
      Serial.println(F("GetId Connection error"));
    }
    https.end();
  }
  else
    Serial.println(F("GetId WiFi Error."));
}
void PostSensorData()
{
  if (WiFi.status() == WL_CONNECTED)
  {
    if (!loggedIn)
    {
      GetJWT();
      if (!loggedIn)
        return;
    }
    // GET TIME STAMP FIRST
    timeClient.begin();
    while (!timeClient.update()) {
      timeClient.forceUpdate();
    }
    String formattedDate = timeClient.getFormattedDate();
    timeClient.end();
    
    DynamicJsonDocument telemetry(128);
    telemetry[F("activityTimeStamp")] = formattedDate;
    telemetry[F("ioUnitId")] = sensorId;
    // Serialize JSON document
    String jsonstr;
    serializeJson(telemetry, jsonstr);
    HTTPClient https;
    https.useHTTP10(true);
    // Send request
    https.begin(BASE_URL POST_URL, sslCert);
    https.addHeader(F("Content-Type"), F("application/json"));
    https.addHeader(F("Authorization"), "Bearer " + jwtToken);
    int httpCode = https.POST(jsonstr);
    Serial.println(httpCode);
    if (0 < httpCode)
    {
      if (HTTP_CODE_CREATED == httpCode)
      {
        Serial.println(F("Post telemetry success"));
        newData = false;
      }
      else
      {
        Serial.print(F("Error. HTTP Statuscode = "));
        Serial.println(httpCode);
      }
    }
    else
    {
      Serial.println(F("PostSensorData Connection error"));
    }
    https.end();
  }
  else
    Serial.println(F("PostSensorData WiFi Error."));
}
void GetSensorState()
{
  if (WiFi.status() == WL_CONNECTED)
  {
    if (!loggedIn)
    {
      GetJWT();
      if (!loggedIn)
        return;
    }
    HTTPClient https;
    https.useHTTP10(true);
    Serial.println(BASE_URL MYSTATE_URL + (String)sensorId);
    https.begin(BASE_URL MYSTATE_URL + (String)sensorId, sslCert);
    https.addHeader(F("Authorization"), "Bearer " + jwtToken);
    int httpCode = https.GET();
    Serial.println(httpCode);
    if (0 < httpCode)
    {
      if (HTTP_CODE_OK == httpCode)
      {
        DynamicJsonDocument json(192);
        deserializeJson(json, https.getStream());
        sensorState = json[F("enabled")].as<bool>();
        Serial.print(F("Sensor is "));
        Serial.println(sensorState);
        updateState = false;
      }
      else
      {
        Serial.print(F("Error. HTTP Statuscode = "));
        Serial.println(httpCode);
      }
    }
    else
    {
      Serial.println(F("GetSensorState Connection error"));
    }
    https.end();
  }
  else
    Serial.println(F("GetSensorState WiFi Error."));
}
