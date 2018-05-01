#include <FirmataParser.h>
#include <FirmataMarshaller.h>
#include <FirmataDefines.h>
#include <FirmataConstants.h>
#include <SoftwareSerial.h>
#include <DallasTemperature.h>
#include <SerialCommand.h>
#include <Firmata.h>
#include <Boards.h>
#include <PID_v1.h>
#include "RotaryEncoder.h"
#include <Button.h>
#include <TimerOne.h>
#include <ClickEncoder.h>
#include <LiquidCrystal_I2C.h>
#include <utility.h>
#include <unwind-cxx.h>
#include <system_configuration.h>
#include "Pump.h"
#include "RemoteController.h"
#include "LocalController.h"
#include "HeatingElement.h"
#include "Kettle.h"
#include "Thermometer.h"

//******************************************************************************
//
// Before you wipe way this machine, make sure you have libraries backed up!
//
//	C:\Users\mwmic\Documents\Arduino
//
// C:\Program Files (x86)\Arduino\libraries\NewliquidCrystal
//
//******************************************************************************

#define SSR_PIN_1 5
#define SSR_PIN_2 6

//#define HEATINGELEMENT_PIN_1 1
//#define HEATINGELEMENT_PIN_2 2
//#define HEATINGELEMENT_PIN_3 3
//#define HEATINGELEMENT_PIN_4 4



#define PUMP_PIN_1 10
#define PUMP_PIN_2 11
#define PUMP_PIN_3 12


#define HEATER_CYCLE_IN_MILLIS 5000
#define TEMP_READING_CYCLE_IN_MILLIS 1000
#define PING_CYCLE_IN_MILLIS 5000
#define CONFIG_CYCLE_IN_MILLIS 2000

// Data wire is plugged into pin 2 on the Arduino Blue Pin, Red Pin to 3.3V, Black Pin to GND, Resister between Red and Blue
#define ONE_WIRE_BUS 3   

OneWire oneWire1(ONE_WIRE_BUS);
DallasTemperature sensors(&oneWire1);

//DeviceAddress probe01 = { 0x28, 0xFF, 0x5E, 0xC0, 0x4E, 0x04, 0x00, 0xB8 };
//DeviceAddress probe02 = { 0x28, 0xFF, 0x14, 0xDB, 0x3C, 0x04, 0x00, 0xE0 };
//DeviceAddress probe03 = { 0x28, 0xFF, 0xAF, 0x3B, 0x4A, 0x04, 0x00, 0xA8 };

DeviceAddress probe04 = { 0x28, 0xFF, 0x43, 0x0E, 0x16, 0x15, 0x03, 0x90 };
DeviceAddress probe05 = { 0x28, 0xFF, 0xD2, 0x37, 0x16, 0x15, 0x03, 0x31 };
DeviceAddress probe06 = { 0x28, 0xFF, 0x4A, 0x3A, 0x16, 0x15, 0x03, 0x25 };
//DeviceAddress probe07 = { 0x28, 0xFF, 0xA1, 0x1E, 0x16, 0x15, 0x03, 0xF1 };

Thermometer thermometer1;
Thermometer thermometer2;
Thermometer thermometer3;

SerialCommand serialCommand;

//LiquidCrystal_I2C lcd(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

Kettle hotLiquorTank;
Kettle boilKettle;

//Pump hotWaterPump(PUMP_PIN_1);
//Pump wortPump(PUMP_PIN_2);

unsigned long LastTempUpdate, LastMessageReceived, LastPing, LastConfigRequest;

LocalController localController;

bool isConfigured = false;

void setup() {
	//Serial.begin(9600);
	//Serial.println("Initializing BrewMachine v3.0 ...");

	/*sensors.begin();

	thermometer1 = Thermometer(sensors, probe04, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer2 = Thermometer(sensors, probe05, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer3 = Thermometer(sensors, probe06, TEMP_READING_CYCLE_IN_MILLIS);

	hotLiquorTank = Kettle(SSR_PIN_1, "HLT", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_1, HEATINGELEMENT_PIN_2);
	boilKettle = Kettle(SSR_PIN_2, "BK", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_3, HEATINGELEMENT_PIN_4);

	localController = LocalController(&thermometer1, &thermometer2, &thermometer3, &hotLiquorTank, &boilKettle);

	pinMode(PUMP_PIN_1, OUTPUT);
	pinMode(PUMP_PIN_2, OUTPUT);
	pinMode(PUMP_PIN_3, OUTPUT);

	digitalWrite(PUMP_PIN_1, LOW);
	digitalWrite(PUMP_PIN_2, HIGH);
	digitalWrite(PUMP_PIN_3, LOW);*/

	//Serial1.begin(57600);
	// Firmata.begin(Serial1);

	Firmata.setFirmwareVersion(FIRMATA_FIRMWARE_MAJOR_VERSION, FIRMATA_FIRMWARE_MINOR_VERSION);
	Firmata.attach(STRING_DATA, receivedCommand);
	Firmata.begin(57600);

	while (!Serial) {
		; // wait for serial port to connect. Needed for ATmega32u4-based boards and Arduino 101
	}
	
}


void loop() {
	//localController.update();

	while (Firmata.available())
		Firmata.processInput();

	/*if (Firmata.available()) {
		if (!isConfigured) {
			localController.postStatus();
			isConfigured = true;
		}

		Firmata.processInput();
	} else {
		isConfigured = false;
		localController.connectionStatus(false);
	}*/
}

void receivedCommand(char *msg) {
	String msgString = String(msg);
	Firmata.sendString(msg);
	//localController.receivedCommand(msgString);
}











//#include <SerialCommand.h>      // Steven Cogswell ArduinoSerialCommand library from https://github.com/scogswell/ArduinoSerialCommand
//
//#define DBGMSG(A)    if (dbg){ Serial.print("DBG: "); Serial.println(A);}
//SerialCommand     serialCommand;
//boolean           dbg = true;
//
////
//// Initialize the serial command table, I/O pins, and the DHT sensor
////
//void setup() {
//	Serial.begin(9600);
//	serialCommand.addCommand("ledon", SetLedOn);
//	serialCommand.addCommand("ledoff", SetLedOff);
//	serialCommand.addCommand("echo", Echo);
//	serialCommand.addDefaultHandler(UnrecognizedCommand);
//}
//
////
//// Read and respond to commands recieved over the serial port
////
//void loop() {
//	serialCommand.readSerial();
//}
//
//void Echo() {
//	char *arg = serialCommand.next();
//
//	if (arg != NULL) {
//		Serial.println("Echo " + String(arg));
//	}
//	else {
//		DBGMSG(F("led not specified"));
//	}
//}
//
////
//// Turn on the LED connected to the specified port
////
//void SetLedOn() {
//	char *arg = serialCommand.next();
//
//	if (arg != NULL) {
//		DBGMSG(F("SetLedOn"));
//	}
//	else {
//		DBGMSG(F("led not specified"));
//	}
//}
//
////
//// Turn off the led connected to the specified port
////
//void SetLedOff() {
//	char *arg = serialCommand.next();
//
//	if (arg != NULL) {
//
//		DBGMSG(F("SetLedOff"));
//
//	}
//	else {
//		DBGMSG(F("led not specified"));
//	}
//}
//
////
//// An unrecognized command was recieved
////
//void UnrecognizedCommand() {
//	DBGMSG(F("Unrecognized command"));
//	DBGMSG(F(" ledon 3  - turn on led connected to digital I/O 3"));
//	DBGMSG(F(" ledon 4  - turn on led connected to digital I/O 4"));
//	DBGMSG(F(" ledon 5  - turn on led connected to digital I/O 5"));
//	DBGMSG(F(" ledon 6  - turn on led connected to digital I/O 6"));
//	DBGMSG(F(" ledoff 3 - turn off led connected to digital I/O 3"));
//	DBGMSG(F(" ledoff 4 - turn off led connected to digital I/O 4"));
//	DBGMSG(F(" ledoff 5 - turn off led connected to digital I/O 5"));
//	DBGMSG(F(" ledoff 6 - turn off led connected to digital I/O 6"));
//	DBGMSG(F(" temp     - read temperature"));
//	DBGMSG(F(" hum      - read humidity"));
//	DBGMSG(F(" debug on - turn on debug messages"));
//	DBGMSG(F(" debug off- turn off debug messages"));
//}
//
//
//
//
//















































//	isConfigured = true;
//
//	//LastMessageReceived = millis();
//
//	String msgString = String(msg);
//	int heater1Index = msgString.indexOf(':');
//	int heater2Index = msgString.indexOf(':', heater1Index + 1);
//	int pump1Index = msgString.indexOf(':', heater2Index + 1);
//	int pump2Index = msgString.indexOf(':', pump1Index + 1);
//
//	int heater1value = msgString.substring(0, heater1Index).toInt();
//	int heater2value = msgString.substring(heater1Index + 1, heater2Index).toInt();
//	int pump1value = msgString.substring(heater2Index + 1, pump1Index).toInt();
//	int pump2value = msgString.substring(pump1Index + 1).toInt();
//	String timestamp = msgString.substring(pump2Index + 1);
//
//	//heater1.setPercentage(heater1value);
//	//heater2.setPercentage(heater2value);
//}

//void sendSetting(String settingName, double settingValue) {
//	String settingStr = settingName + "=" + String(settingValue, 2);
//	Firmata.sendString(settingStr.c_str());
//}


//
//void test() {
//	Serial.println("Test complete");
//}
//
//void ping() {
//	Serial.println("Ping!");
//}
//
//
//void unrecognizedCommand() {
//	Serial.println("UnrecognizedCommand");
//}
