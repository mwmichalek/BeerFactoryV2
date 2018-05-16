#include <CmdMessenger.h>
#include "Arduino.h"
#include "HeatingElement.h"
#include "Kettle.h"

Kettle::Kettle() {
}

Kettle::Kettle(int ssrPin, String name, int cycleLengthInMillis, int heatingElementPin1, int heatingElementPin2, CmdMessenger* cmdMessenger) {
	pinMode(ssrPin, OUTPUT);
	_cmdMessenger = cmdMessenger;
	_name = name;
	_ssrPin = ssrPin;
	_cycleLengthInMillis = cycleLengthInMillis;

	_heatingElement1 = HeatingElement(heatingElementPin1, name + "1");
	_heatingElement2 = HeatingElement(heatingElementPin2, name + "2");


	_timeToOn = 0;
	_timeToOff = 0;

	engage(false);
	setPercentage(0);
}

void Kettle::update() {
	//Serial.println("Kettle[" + _name + "]: update " + _percentage);
	unsigned long currentMillis = millis();

	if (_percentage > 0) {
		

		// Indicates the lag time between when an event should have happened and when it did.
		unsigned long millisOff;

		if (currentMillis >= _timeToOff) {
			millisOff = currentMillis - _timeToOff;

			_timeToOn = currentMillis + _millisOfOff;
			_timeToOff = _timeToOn + _millisOfOn;
			//Serial.println(_name + ": Off Lagtime: " + millisOff);
			digitalWrite(_ssrPin, LOW);

			/*_cmdMessenger->sendCmdStart(kTempChange);
			_cmdMessenger->sendCmdArg(tempNumber);
			_cmdMessenger->sendCmdArg(temperature);
			_cmdMessenger->sendCmdEnd();
*/

		} else if (currentMillis >= _timeToOn) {
			millisOff = currentMillis - _timeToOn;

			_timeToOff = currentMillis + _millisOfOn;
			_timeToOn = _timeToOff + _millisOfOff;
			//Serial.println(_name + ": On Lagtime :" + millisOff);
			digitalWrite(_ssrPin, HIGH);
		}
	} else {
		digitalWrite(_ssrPin, LOW);
		_timeToOn = currentMillis;
		_timeToOff = currentMillis;
	}
}

bool Kettle::isEngaged() {
	return _engaged;
}

void Kettle::engage(bool isEngaged) {
	_engaged = isEngaged;

	_heatingElement1.engage(isEngaged);
	_heatingElement2.engage(isEngaged);

	//Serial.println("Kettle[" + _name + "]: engaged: " + String(_engaged));
	update();
}

int Kettle::currentPercentage() {
	return _percentage;
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
	//Serial.println("Kettle[" + _name + "]: setPercentage: " + String(_percentage));
	update();
}



