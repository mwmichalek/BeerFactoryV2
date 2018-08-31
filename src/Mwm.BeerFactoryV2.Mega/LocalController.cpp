#include <CmdMessenger.h>
#include <LiquidCrystal_I2C.h>
#include "LocalController.h"
#include "Kettle.h"
#include "Events.h"

LocalController::LocalController() {
}

LocalController::LocalController(Thermometer* thermometers[], 
	Kettle* hotLiquorTank, Kettle* boilKettle, CmdMessenger* cmdMessenger) {
	_thermometer1 = thermometers[0];
	_thermometer2 = thermometers[1];
	_thermometer3 = thermometers[2];
	_thermometer4 = thermometers[3];
	_thermometer5 = thermometers[4];
	_thermometer6 = thermometers[5];
	_thermometer7 = thermometers[6];
	_thermometer8 = thermometers[7];
	_thermometer9 = thermometers[8];

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


//LocalController::LocalController(Thermometer* thermometer1, Thermometer* thermometer2, Thermometer* thermometer3,
//							     Kettle* hotLiquorTank, Kettle* boilKettle, CmdMessenger* cmdMessenger) {
//	_thermometer1 = thermometer1;
//	_thermometer2 = thermometer2;
//	_thermometer3 = thermometer3;
//	_hotLiquorTank = hotLiquorTank;
//	_boilKettle = boilKettle;
//
//	_cmdMessenger = cmdMessenger;
//
//	_lcd.begin(20, 4);
//	_lcd.setBacklight(HIGH);
//
//	_lcd.setCursor(0, 0);            // go to the top left corner
//	_lcd.print("BF v3.0");
//
//	//TODO: (Michalek) HARDCODED!!!!
//	_hotLiquorTank->setPercentage(10);
//	_hotLiquorTank->enable(true);
//
//	_boilKettle->setPercentage(90);
//	_boilKettle->enable(true);
//}



void LocalController::update() {

	_thermometer1->update();
	_thermometer2->update();
	_thermometer3->update();
	_thermometer4->update();
	_thermometer5->update();
	_thermometer6->update();
	_thermometer7->update();
	_thermometer8->update();
	_thermometer9->update();
	   
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

	double newTemperature4 = _thermometer4->currentTemp();
	if (newTemperature4 != _temperature4) {
		_temperature4 = newTemperature4;
		postTemperature(4, _temperature4);
	}

	double newTemperature5 = _thermometer5->currentTemp();
	if (newTemperature5 != _temperature5) {
		_temperature5 = newTemperature5;
		postTemperature(5, _temperature5);
	}

	double newTemperature6 = _thermometer6->currentTemp();
	if (newTemperature6 != _temperature6) {
		_temperature6 = newTemperature6;
		postTemperature(6, _temperature6);
	}

	double newTemperature7 = _thermometer7->currentTemp();
	if (newTemperature7 != _temperature7) {
		_temperature7 = newTemperature7;
		postTemperature(7, _temperature7);
	}

	double newTemperature8 = _thermometer8->currentTemp();
	if (newTemperature8 != _temperature8) {
		_temperature8 = newTemperature8;
		postTemperature(8, _temperature8);
	}

	double newTemperature9 = _thermometer9->currentTemp();
	if (newTemperature9 != _temperature9) {
		_temperature9 = newTemperature9;
		postTemperature(9, _temperature9);
	}

	_hotLiquorTank->update();
	_boilKettle->update();

	//handleCommand();

	displayStatus();
}

void LocalController::handleKettleRequest() {
	int index = _cmdMessenger->readInt16Arg();
	double percentage = _cmdMessenger->readDoubleArg();
	if (index == 1) {
		_hotLiquorTank->setPercentage(percentage);
		_hotLiquorTank->enable(true);
		postMsg("HLT:" + String(percentage));
	} else if (index == 2) {
		_boilKettle->setPercentage(percentage);
		_boilKettle->enable(true);
		postMsg("BK:" + String(percentage));
	}
}

void LocalController::postTemperature(int index, double temperature) {
	if (temperature != 185) {
		_cmdMessenger->sendCmdStart(Events::kTempChange);
		_cmdMessenger->sendCmdArg(index);
		_cmdMessenger->sendCmdArg(temperature);
		_cmdMessenger->sendCmdEnd();
	}
}

void LocalController::postKettle(int index, double percentage) {
	_cmdMessenger->sendCmdStart(Events::kKettleResult);
	_cmdMessenger->sendCmdArg(index);
	_cmdMessenger->sendCmdArg(percentage);
	_cmdMessenger->sendCmdEnd();
}



void LocalController::postStatus() {
	_isConnected = true;
	postTemperature(1, _temperature1);
	postTemperature(2, _temperature2);
	postTemperature(3, _temperature3);
	postTemperature(4, _temperature4);
	postTemperature(5, _temperature5);
	postTemperature(6, _temperature6);
	postTemperature(7, _temperature7);
	postTemperature(8, _temperature8);
	postTemperature(9, _temperature9);

	//postKettle(1, _hotLiquorTank->currentPercentage);
	//postKettle(2, _boilKettle->currentPercentage);
}

void LocalController::connectionStatus(bool isConnected) {
	if (isConnected)
		_isConnected = 1;
	else
		_isConnected = 0;
}

void LocalController::postMsg(String msg) {
	_lcd.setCursor(0, 3);
	_lcd.print(String(msg));
}

void LocalController::displayStatus() {
	//_lcd.setCursor(0, 0);
	//_lcd.print(String(_temperature1) + "  ");
	//_lcd.setCursor(0, 1);
	//_lcd.print(String(_temperature2) + "  ");
	//_lcd.setCursor(0, 2);
	//_lcd.print(String(_temperature3) + "  ");
	//_lcd.setCursor(0, 3);
	//_lcd.print(String(_temperature4) + "  ");

	_lcd.setCursor(0, 0);
	_lcd.print(String(_temperature1) + " " + String(_temperature2) + " " + String(_temperature3));
	_lcd.setCursor(0, 1);
	_lcd.print(String(_temperature4) + " " + String(_temperature5) + " " + String(_temperature6));
	_lcd.setCursor(0, 2);
	_lcd.print(String(_temperature7) + " " + String(_temperature8) + " " + String(_temperature9));
	
}

//readInt16Argvoid LocalController::handleCommand() {
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
//}

//void LocalController::receivedCommand(String command) {
//
//	int eqlIndex = command.indexOf("=");
//	if (eqlIndex != -1) {
//		_lastCmd = command.substring(0, eqlIndex - 1);
//		_lastVal = command.substring(eqlIndex + 1, command.length() - 1);
//	} else {
//		_lastCmd = command;
//		_lastVal = "";
//	}
//}