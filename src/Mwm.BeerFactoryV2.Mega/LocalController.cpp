#include <CmdMessenger.h>
#include <LiquidCrystal_I2C.h>
#include "LocalController.h"
#include "Kettle.h"
#include "Events.h"

LocalController::LocalController() {
}

//enum {
//	kAcknowledge,
//	kError,
//	kPingRequest,
//	kPingResult,
//	kStatusRequest,
//	kStatusResult,
//	kKettleRequest,
//	kKettleResult,
//	kTempChange,
//	kHeaterChange,
//	kPumpChange
//};

LocalController::LocalController(Thermometer* thermometer1, Thermometer* thermometer2, Thermometer* thermometer3,
							     Kettle* hotLiquorTank, Kettle* boilKettle, CmdMessenger* cmdMessenger) {
	_thermometer1 = thermometer1;
	_thermometer2 = thermometer2;
	_thermometer3 = thermometer3;
	_hotLiquorTank = hotLiquorTank;
	_boilKettle = boilKettle;

	_cmdMessenger = cmdMessenger;

	_lcd.begin(20, 4);
	_lcd.setBacklight(HIGH);

	_lcd.setCursor(0, 0);            // go to the top left corner
	_lcd.print("BF v3.0");

	//TODO: (Michalek) HARDCODED!!!!
	_hotLiquorTank->setPercentage(10);
	_hotLiquorTank->enable(true);

	_boilKettle->setPercentage(90);
	_boilKettle->enable(true);
}

void LocalController::update() {

	_thermometer1->update();
	_thermometer2->update();
	_thermometer3->update();

	double newTemperature1 = _thermometer1->currentTemp();
	if (newTemperature1 != _temperature1) {
		_temperature1 = newTemperature1;
		postTemperature(1, _temperature1);
	}

	double newTemperature2 = _thermometer2->currentTemp();
	if (newTemperature2 != _temperature2) {
		_temperature2 = newTemperature2;
		postTemperature(2, _temperature2);
	}
	
	double newTemperature3 = _thermometer3->currentTemp();
	if (newTemperature3 != _temperature3) {
		_temperature3 = newTemperature3;
		postTemperature(3, _temperature3);
	}

	_hotLiquorTank->update();
	_boilKettle->update();

	handleCommand();

	displayStatus();
}

void LocalController::postTemperature(int tempNumber, double temperature) {
	if (temperature != 185) {
		_cmdMessenger->sendCmdStart(Events::kTempChange);
		_cmdMessenger->sendCmdArg(tempNumber);
		_cmdMessenger->sendCmdArg(temperature);
		_cmdMessenger->sendCmdEnd();
	}
}


void LocalController::postStatus() {
	_isConnected = true;
	postTemperature(1, _temperature1);
	postTemperature(2, _temperature2);
	postTemperature(3, _temperature3);
}

void LocalController::connectionStatus(bool isConnected) {
	if (isConnected)
		_isConnected = 1;
	else
		_isConnected = 0;
}

void LocalController::receivedCommand(String command) {

	int eqlIndex = command.indexOf("=");
	if (eqlIndex != -1) {
		_lastCmd = command.substring(0, eqlIndex - 1);
		_lastVal = command.substring(eqlIndex + 1, command.length() - 1);
	} else {
		_lastCmd = command;
		_lastVal = "";
	}
}

void LocalController::postMsg(String msg) {
	_lcd.setCursor(0, 3);
	_lcd.print(String(msg));
}


void LocalController::handleCommand() {
	//String command = "";

	//while (Serial.available()) {
	//	char inChar = (char)Serial.read();

	//	if (inChar != '\n') 
	//		command += inChar;
	//	else
	//		continue;
	//}

	//if (command == "STATE") {
	//	Serial.println("BF:STATE:T1:" + String(_temperature1) +
	//		"|T2:" + String(_temperature2) +
	//		"|T3:" + String(_temperature3) +
	//		"|HLT:" + String(_hotLiquorTank->currentPercentage()) +
	//		"|BK:" + String(_boilKettle->currentPercentage()));
	//} else if (command != "") {
	//	Serial.println("UNKNOWN_COMAND: " + command);
	//	//_lcd.setCursor(0, 3);
	//	//_lcd.print(command + "   ");
	//}
	////Serial.println("HEARD:" + reading);
}


void LocalController::displayStatus() {
	_lcd.setCursor(0, 0);
	_lcd.print("T1:" + String(_temperature1));
	_lcd.setCursor(0, 1);
	_lcd.print("T2:" + String(_temperature2));
	_lcd.setCursor(0, 2);
	_lcd.print("T3:" + String(_temperature3));
	
}