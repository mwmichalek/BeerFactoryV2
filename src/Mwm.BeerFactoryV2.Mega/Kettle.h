// Kettle.h

#ifndef _KETTLE_h
#define _KETTLE_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
	#include "HeatingElement.h"
#else
	#include "WProgram.h"
#endif

class Kettle {

public:
	Kettle();
	Kettle(int ssrPin, String name, int cycleLengthInMillis, int heatingElementPin1, int heatingElementPin2);

	void update();
	bool isEngaged();
	void engage(bool isEngaged);
	int currentPercentage();
	void setPercentage(int percentage);
	
private:
	int _ssrPin;
	String _name;
	HeatingElement _heatingElement1;
	HeatingElement _heatingElement2;
	int _cycleLengthInMillis;
	int _percentage;
	int _millisOfOn;
	int _millisOfOff;
	unsigned long _timeToOn;
	unsigned long _timeToOff;
	bool _engaged;
};

#endif

