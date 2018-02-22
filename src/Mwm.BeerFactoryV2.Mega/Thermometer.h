// Thermometer.h


#ifndef _THERMOMETER_h
#define _THERMOMETER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#include <DallasTemperature.h>
#include <OneWire.h>

class Thermometer {

public:
	Thermometer();
	Thermometer(const DallasTemperature &sensor, const DeviceAddress &probe, int cycleLengthInMillis);
	void update();
	double currentTemp();

private:
	int _cycleLengthInMillis;
	double _temperature;
	unsigned long _timeToUpdate;
	uint8_t *_probe;
	DallasTemperature _sensor;
};

#endif

