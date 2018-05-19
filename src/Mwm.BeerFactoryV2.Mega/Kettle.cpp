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


	_timeToOn = 0;
	_timeToOff = 0;

	enable(false);
	setPercentage(0);
}

void Kettle::update() {
	unsigned long currentMillis = millis();

	if (_percentage > 0) {

		// Indicates the lag time between when an event should have happened and when it did.
		unsigned long millisOff;

		if (currentMillis >= _timeToOff) {
			millisOff = currentMillis - _timeToOff;

			_timeToOn = currentMillis + _millisOfOff;
			_timeToOff = _timeToOn + _millisOfOn;

			engage(true);
	
		} else if (currentMillis >= _timeToOn) {
			millisOff = currentMillis - _timeToOn;

			_timeToOff = currentMillis + _millisOfOn;
			_timeToOn = _timeToOff + _millisOfOff;

			engage(false);
		}
	} else {
		digitalWrite(_ssrPin, LOW);
		_timeToOn = currentMillis;
		_timeToOff = currentMillis;
	}
}

bool Kettle::isEnabled() {
	return _enabled;
}

void Kettle::enable(bool isEnabled) {
	_enabled = isEnabled;

	_heatingElement1.enable(_enabled);
	_heatingElement2.enable(_enabled);

	update();
}

bool Kettle::isEngaged() {
	return _engaged;
}

void Kettle::engage(bool isEngaged) {
	//TODO: Clean this up!

	//bool valueChanged = false;
	//bool trueEngage = _enabled && isEngaged;

	//if (_isEngaged != trueEngage) {
	//	_isEngaged = trueEngage;
	//	valueChanged = true;
	//}
	//

	if (isEngaged && (isEngaged != _engaged)) {
		digitalWrite(_ssrPin, LOW);
		postStatus(_ssrPin, true);
	} else if (isEngaged != _engaged) {

		digitalWrite(_ssrPin, HIGH);
		postStatus(_ssrPin, false);
	}

	_heatingElement1.engage(isEngaged);
	_heatingElement2.engage(isEngaged);

	_engaged = isEngaged;

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
	
	if (_percentage > 0) {
		double ratio = (double)_percentage / (double)100;
		_millisOfOn = _cycleLengthInMillis * ratio;
		_millisOfOff = _cycleLengthInMillis - _millisOfOn;
	} else {
		_millisOfOn = 0;
		_millisOfOff = _cycleLengthInMillis;
	}
	update();
}



