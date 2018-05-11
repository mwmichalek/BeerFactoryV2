#include <CmdMessenger.h>
#include <OneWire.h>
#include <DallasTemperature.h>
//#include <Boards.h>
//#include <PID_v1.h>
//#include "RotaryEncoder.h"
//#include <TimerOne.h>
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

#define HEATINGELEMENT_PIN_1 1
#define HEATINGELEMENT_PIN_2 2
#define HEATINGELEMENT_PIN_3 3
#define HEATINGELEMENT_PIN_4 4



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

//LiquidCrystal_I2C lcd(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

Kettle hotLiquorTank;
Kettle boilKettle;

//Pump hotWaterPump(PUMP_PIN_1);
//Pump wortPump(PUMP_PIN_2);

unsigned long LastTempUpdate, LastMessageReceived, LastPing, LastConfigRequest;

LocalController localController;

bool isConfigured = false;

CmdMessenger cmdMessenger = CmdMessenger(Serial);

// This is the list of recognized commands. These can be commands that can either be sent or received. 
// In order to receive, attach a callback function to these events
enum {
	// Commands
	kAcknowledge, // Command to acknowledge that cmd was received
	kError, // Command to report errors
	//kFloatAddition, // Command to request add two floats
	//kFloatAdditionResult, // Command to report addition result
	kEcho,
	kEchoResult,
	kTempChange
};


void setup() {
	Serial.begin(57600);
		//9600
		//57600

	// Adds newline to every command
	cmdMessenger.printLfCr();

	// Attach my application's user-defined callback methods
	attachCommandCallbacks();

	// Send the status to the PC that says the Arduino has booted
	cmdMessenger.sendCmd(kAcknowledge, "Arduino has started!");



	sensors.begin();

	thermometer1 = Thermometer(sensors, probe04, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer2 = Thermometer(sensors, probe05, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer3 = Thermometer(sensors, probe06, TEMP_READING_CYCLE_IN_MILLIS);

	hotLiquorTank = Kettle(SSR_PIN_1, "HLT", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_1, HEATINGELEMENT_PIN_2);
	boilKettle = Kettle(SSR_PIN_2, "BK", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_3, HEATINGELEMENT_PIN_4);

	localController = LocalController(&thermometer1, &thermometer2, &thermometer3, &hotLiquorTank, &boilKettle);

	pinMode(PUMP_PIN_1, OUTPUT);
	pinMode(PUMP_PIN_2, OUTPUT);
	pinMode(PUMP_PIN_3, OUTPUT);

	digitalWrite(PUMP_PIN_1, HIGH);
	digitalWrite(PUMP_PIN_2, LOW);
	digitalWrite(PUMP_PIN_3, HIGH);
	
}


void loop() {
	localController.update();
	cmdMessenger.feedinSerialData();
}

void attachCommandCallbacks() {
	// Attach callback methods
	cmdMessenger.attach(OnUnknownCommand);
	cmdMessenger.attach(kEcho, OnEcho);
}

// ------------------  C A L L B A C K S -----------------------

// Called when a received command has no attached function
void OnUnknownCommand() {
	cmdMessenger.sendCmd(kError, "Command without attached callback");
}

// Callback function that responds that Arduino is ready (has booted up)
void OnArduinoReady() {
	cmdMessenger.sendCmd(kAcknowledge, "Arduino ready");
}

void OnEcho() {
	String s = cmdMessenger.readStringArg();

	localController.postMsg(s);

	cmdMessenger.sendCmdStart(kEchoResult);
	cmdMessenger.sendCmdArg(s);
	cmdMessenger.sendCmdEnd();
}

