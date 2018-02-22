#include <LiquidCrystal_I2C.h>
#include "LocalController.h"
#include "Kettle.h"
#include <Firmata.h>

LocalController::LocalController() {
}

LocalController::LocalController(Thermometer* thermometer1, Thermometer* thermometer2, Thermometer* thermometer3,
							     Kettle* hotLiquorTank, Kettle* boilKettle) {
	_thermometer1 = thermometer1;
	_thermometer2 = thermometer2;
	_thermometer3 = thermometer3;
	_hotLiquorTank = hotLiquorTank;
	_boilKettle = boilKettle;

	_lcd.begin(20, 4);
	_lcd.setBacklight(HIGH);

	_lcd.setCursor(0, 0);            // go to the top left corner
	_lcd.print("BF v3.0");

	//TODO: (Michalek) HARDCODED!!!!
	_hotLiquorTank->setPercentage(10);
	_hotLiquorTank->engage(true);

	_boilKettle->setPercentage(90);
	_boilKettle->engage(true);

}

void LocalController::update() {

	_thermometer1->update();
	_thermometer2->update();
	_thermometer3->update();

	
	double newTemperature1 = _thermometer1->currentTemp();
	if (newTemperature1 != _temperature1) {
		//Serial.println("BF:T1=" + String(newTemperature1));
		//Firmata.sendString("BF:T1=" + String(newTemperature1));
		Firmata.sendString("T1=T1");
		_temperature1 = newTemperature1;
	}

	double newTemperature2 = _thermometer2->currentTemp();
	if (newTemperature2 != _temperature2) {
		//Serial.println("BF:T2=" + String(newTemperature2));
		Firmata.sendString("T2=T2");
		_temperature2 = newTemperature2;
	}
	
	double newTemperature3 = _thermometer3->currentTemp();
	if (newTemperature3 != _temperature3) {
		//Serial.println("BF:T3=" + String(newTemperature3));
		Firmata.sendString("T3=T3");
		_temperature3 = newTemperature3;
	}

	_hotLiquorTank->update();
	_boilKettle->update();

	handleCommand();

	displayStatus();
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
	


	//Serial.println("{BF3:[T1:" + String(_temperature1) + "][T2:" + String(_temperature2) + "][T3:" + String(_temperature3) + "]}");
	
	//_lcd.setCursor(0, 3);
	//_lcd.print(" BK:%" + String(_boilKettle->currentPercentage()));

	//Serial.println("temp1: " + String(_temperature1) + " temp2: " + String(_temperature2) + " temp3: " + String(_temperature3));
	//Serial.println("hotLiquorTank: " + String(_hotLiquorTank->currentPercentage()) + " " + String(_hotLiquorTank->isEngaged()));
	//Serial.println("boilKettle: " + String(_boilKettle->currentPercentage()) + " " + String(_boilKettle->isEngaged()));
}




