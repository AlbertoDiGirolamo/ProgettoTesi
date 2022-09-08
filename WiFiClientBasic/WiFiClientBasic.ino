/*
 *  This sketch sends a message to a TCP server
 *
 */

#include <WiFi.h>
#include <WiFiMulti.h>

WiFiMulti WiFiMulti;
// IP    http://192.168.1.208:8080
void setup()
{
    Serial.begin(115200);
    delay(10);

    // We start by connecting to a WiFi network
    WiFiMulti.addAP("TIM-32246399", "AMvljDSrPRCd4buAb0JVd2Di");

    Serial.println();
    Serial.println();
    Serial.print("Waiting for WiFi... ");

    while(WiFiMulti.run() != WL_CONNECTED) {
        Serial.print(".");
        delay(500);
    }

    Serial.println("");
    Serial.println("WiFi connected");
    Serial.println("IP address: ");
    Serial.println(WiFi.localIP());

    delay(500);
}

int pr=65;
int modpr = 1;
int modrr = 1;
float temp = 36.5;
int rr = 28;
int SPO2 = 99;
void loop()
{
//    const uint16_t port = 80;
//    const char * host = "192.168.1.1"; // ip or dns
    const uint16_t port = 8080;
    const char * host = "192.168.1.80"; // ip or dns

    Serial.print("Connecting to ");
    Serial.println(host);

    // Use WiFiClient class to create TCP connections
    WiFiClient client;

    if (!client.connect(host, port)) {
        Serial.println("Connection failed.");
        Serial.println("Waiting 5 seconds before retrying...");
        delay(5000);
        return;
    }

    // This will send a request to the server
    //uncomment this line to send an arbitrary string to the server
    //client.print("Send this data to the server");
    //uncomment this line to send a basic document request to the server
    if (pr >68 or pr < 62){
      modpr = -modpr;
      temp = temp +0.3;
      
    }
    if (rr > 31 or rr < 26){
      modrr = -modrr;
      temp = temp -0.2;
    }
    rr = rr + modrr;
    pr = pr + modpr;
    
    client.print("PR: "+ String(pr));
    client.print("|TEMP: "+String(temp));
    client.print("|SPO2: "+String(SPO2));
    client.print("|RR: "+String(rr));

  int maxloops = 0;

  //wait for the server's reply to become available
  while (!client.available() && maxloops < 1000)
  {
    maxloops++;
    delay(1); //delay 1 msec
  }
  if (client.available() > 0)
  {
    //read back one line from the server
    String line = client.readStringUntil('\r');
    Serial.println(line);
  }
  else
  {
    Serial.println("client.available() timed out ");
  }

    Serial.println("Closing connection.");
    client.stop();

    Serial.println("Waiting 5 seconds before restarting...");
    delay(1000);
}
