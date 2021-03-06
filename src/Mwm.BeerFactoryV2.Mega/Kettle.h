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
	Kettle(int ssrPin, String name, int cycleLengthInMillis, int heatingElementPin1, int heatingElementPin2, CmdMessenger* _cmdMessenger);
	

	void update();
	bool isEnabled();
	bool isEngaged();
	void enable(bool isEnabled);
	void engage(bool isEngaged);
	int currentPercentage();
	void setPercentage(int percentage);
	void postStatus(int index, int onOrOff);
	
private:
	int _ssrPin;
	String _name;
	HeatingElement _heatingElement1;
	HeatingElement _heatingElement2;
	CmdMessenger* _cmdMessenger;
	int _cycleLengthInMillis;
	int _percentage;
	int _millisOfOn;
	int _millisOfOff;
	unsigned long _timeToOn;
	unsigned long _timeToOff;
	bool _enabled;
	bool _engaged;
};

#endif

