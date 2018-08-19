//#include <SoftI2CMaster.h>
#include <SI2CIO.h>
#include <LiquidCrystal_SR3W.h>
#include <LiquidCrystal_SR2W.h>
#include <LiquidCrystal_SR1W.h>
#include <LiquidCrystal_SR.h>
#include <LiquidCrystal_SI2C.h>
#include <LiquidCrystal_I2C_ByVac.h>
#include <LiquidCrystal.h>
#include <LCD.h>
#include <I2CIO.h>
#include <FastIO.h>
#include "Events.h"
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

Kettle hotLiquorTank;
Kettle boilKettle;

//Pump hotWaterPump(PUMP_PIN_1);
//Pump wortPump(PUMP_PIN_2);

unsigned long LastTempUpdate, LastMessageReceived, LastPing, LastConfigRequest;

LocalController localController;

bool isConfigured = false;

CmdMessenger cmdMessenger = CmdMessenger(Serial);

void setup() {
	Serial.begin(57600);

	sensors.begin();

	thermometer1 = Thermometer(sensors, probe04, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer2 = Thermometer(sensors, probe05, TEMP_READING_CYCLE_IN_MILLIS);
	thermometer3 = Thermometer(sensors, probe06, TEMP_READING_CYCLE_IN_MILLIS);

	hotLiquorTank = Kettle(SSR_PIN_1, "HLT", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_1, HEATINGELEMENT_PIN_2, &cmdMessenger);
	boilKettle = Kettle(SSR_PIN_2, "BK", HEATER_CYCLE_IN_MILLIS, HEATINGELEMENT_PIN_3, HEATINGELEMENT_PIN_4, &cmdMessenger);

	localController = LocalController(&thermometer1, &thermometer2, &thermometer3, &hotLiquorTank, &boilKettle, &cmdMessenger);

	pinMode(PUMP_PIN_1, OUTPUT);
	pinMode(PUMP_PIN_2, OUTPUT);
	pinMode(PUMP_PIN_3, OUTPUT);

	digitalWrite(PUMP_PIN_1, HIGH);
	digitalWrite(PUMP_PIN_2, LOW);
	digitalWrite(PUMP_PIN_3, HIGH);

	configureCmdMessenger();
}


void loop() {
	localController.update();
	cmdMessenger.feedinSerialData();
}

void configureCmdMessenger() {
	cmdMessenger.printLfCr();
	cmdMessenger.attach(onUnknownCommand);
	cmdMessenger.attach(Events::kStatusRequest, onStatusRequest);
	cmdMessenger.attach(Events::kPingRequest, onPing);
	cmdMessenger.attach(Events::kKettleRequest, onKettleRequest);
	onArduinoReady();
}

// ------------------  C A L L B A C K S -----------------------

// Called when a received command has no attached function
void onUnknownCommand() {
	cmdMessenger.sendCmd(Events::kError, "Command without attached callback");
}

void onStatusRequest() {
	localController.postStatus();
}

// Callback function that responds that Arduino is ready (has booted up)
void onArduinoReady() {
	cmdMessenger.sendCmd(Events::kAcknowledge, "Arduino ready");
}

void onPing() {
	cmdMessenger.sendCmdStart(Events::kPingResult);
	cmdMessenger.sendCmdEnd();
}

void onKettleRequest() {
	int index = cmdMessenger.readInt16Arg();
	double percentage = cmdMessenger.readDoubleArg();

}

