// LocalController.h

#ifndef _LOCALCONTROLLER_h
#define _LOCALCONTROLLER_h
#if defined(ARDUINO) && ARDUINO >= 100
#include "arduino.h"
#else
#include "WProgram.h"
#endif

#include <LiquidCrystal_I2C.h>
#include "Thermometer.h"
#include "Kettle.h"

#define DISPLAY_REFRESH_MILLIS 1000

class LocalController {

public:
	LocalController();
	LocalController(Thermometer* thermometer1, Thermometer* thermometer2, Thermometer* thermometer3,
		            Kettle* hotLiquorTank, Kettle* boilKettle);
	void update();
	void handleCommand();
	void displayStatus();
	void receivedCommand(String command);
	void postStatus();
	void postMsg(String msg);
	void connectionStatus(bool isConnected);
	

private:
	Thermometer* _thermometer1;
	Thermometer* _thermometer2;
	Thermometer* _thermometer3;
	Kettle* _hotLiquorTank;
	Kettle* _boilKettle;
	double _temperature1;
	double _temperature2;
	double _temperature3;
	int _nextDisplayUpdate;
	String _lastCmd;
	String _lastVal;
	int _isConnected;
	LiquidCrystal_I2C _lcd = LiquidCrystal_I2C(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

	void postTemperature(int tempNumber, double temperature);

	//String inputString = "";         // a string to hold incoming data
	//boolean stringComplete = false;  // whether the string is complete
};

#endif

