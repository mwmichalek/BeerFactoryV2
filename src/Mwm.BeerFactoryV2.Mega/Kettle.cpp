#include <CmdMessenger.h>
#include "Arduino.h"
#include "HeatingElement.h"
#include "Kettle.h"
#include "Events.h"

Kettle::Kettle() {
}

Kettle::Kettle(int ssrPin, String name, int cycleLengthInMillis, int heatingElementPin1, int heatingElementPin2, CmdMessenger* cmdMessenger) {
	pinMode(ssrPin, OUTPUT);
	_cmdMessenger = cmdMessenger;
	_name = name;
	_ssrPin = ssrPin;
	_cycleLengthInMillis = cycleLengthInMillis;

	_heatingElement1 = HeatingElement(heatingElementPin1, name + "1", cmdMessenger);
	_heatingElement2 = HeatingElement(heatingElementPin2, name + "2", cmdMessenger);

	setPercentage(0);
}

void Kettle::update() {
	unsigned long currentMillis = millis();

	if (_percentage > 0) {

		// Indicates the lag time between when an event should have happened and when it did.
		unsigned long millisOff;

		if (currentMillis >= _timeToToggle) {
			millisOff = currentMillis - _timeToToggle;

			if ((_millisOfOn > 0) && !_engaged) {
				// Turn on
				engage(true);
				_timeToToggle = currentMillis + _millisOfOn;

			}
			else {
				// Turn off
				engage(false);
				_timeToToggle = currentMillis + _millisOfOff;
			}
		} 
	} 
}

//bool Kettle::isEnabled() {
//	return _enabled;
//}

//void Kettle::enable(bool isEnabled) {
//	_enabled = isEnabled;
//
//	_heatingElement1.enable(_enabled);
//	_heatingElement2.enable(_enabled);
//
//	update();
//}

//bool Kettle::isEngaged() {
//	return _engaged;
//}

void Kettle::engage(bool isEngaged) {
	//TODO: Clean this up!
	bool valueChanged = false;
	//bool trueEngaged = _enabled && isEngaged;
	bool trueEngaged = isEngaged;

	if (_engaged != trueEngaged) {
		_engaged = trueEngaged;
		valueChanged = true;
	}

	// This doesn't do much but launch events when the parent SSR is engaged.
	if (valueChanged) {
		if (trueEngaged) {
			digitalWrite(_ssrPin, HIGH);
			postStatus(_ssrPin, true);
		} else {
			digitalWrite(_ssrPin, LOW);
			postStatus(_ssrPin, false);
		}

		_heatingElement1.engage(trueEngaged);
		_heatingElement2.engage(trueEngaged);
	}
}

void Kettle::disengage() {
	_timeToToggle = millis();
	_engaged = false;
	digitalWrite(_ssrPin, LOW);
}

int Kettle::currentPercentage() {
	return _percentage;
}

void Kettle::postStatus(int index, int onOrOff) {
	_cmdMessenger->sendCmdStart(Events::kSsrChange);
	_cmdMessenger->sendCmdArg(index);
	_cmdMessenger->sendCmdArg(onOrOff);
	_cmdMessenger->sendCmdEnd();
}


void Kettle::setPercentage(int percentage) {
	// TODO: Validate percentage
	_percentage = percentage;
	
	if (_percentage == 100) {
		_millisOfOn = _cycleLengthInMillis * 100000;
		_millisOfOff = 0;
	} else if (_percentage > 0) {
		double ratio = (double)_percentage / (double)100;
		_millisOfOn = _cycleLengthInMillis * ratio;
		_millisOfOff = _cycleLengthInMillis - _millisOfOn;
	} else {
		_millisOfOn = 0;
		_millisOfOff = _cycleLengthInMillis * 100000;
	}
	
	disengage();
	update();
}



