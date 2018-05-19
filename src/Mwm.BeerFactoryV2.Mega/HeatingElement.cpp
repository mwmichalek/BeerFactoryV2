#include <CmdMessenger.h>
#include "Arduino.h"
#include "HeatingElement.h"
#include "Events.h"

///
/// Used to allow power to the element.  Percentage is controlled by the SSR (Kettle)
///
HeatingElement::HeatingElement() {

}

HeatingElement::HeatingElement(int relayPin, const int index, CmdMessenger* cmdMessenger) {
	pinMode(relayPin, OUTPUT);
	_cmdMessenger = cmdMessenger;
	_relayPin = relayPin;
	_index = index;
	//Serial.println("HeatingElement[" + String(_index) + "]: created.");
	//engage(false);
}

HeatingElement::HeatingElement(int relayPin, const String name, CmdMessenger* cmdMessenger) {
	pinMode(relayPin, OUTPUT);
	_cmdMessenger = cmdMessenger;
	_relayPin = relayPin;
	_name = name;
	//engage(false);
}

void HeatingElement::enable(bool isEnabled) {
	_isEnabled = isEnabled;
}

void HeatingElement::engage(bool isEngaged) {
	//TODO: This is still notifying when the value doesn't change.
	bool valueChanged = false;
	bool trueEngage = _isEnabled && isEngaged;

	if (_isEngaged != trueEngage) {
		_isEngaged = trueEngage;
		valueChanged = true;
	}

	// This doesn't do much but launch events when the parent SSR is engaged.
	if (valueChanged) {
		if (trueEngage) 
			postStatus(_relayPin, 1);
		
		else 	
			postStatus(_relayPin, 0);
	}
}

void HeatingElement::postStatus(int index, int onOrOff) {
	_cmdMessenger->sendCmdStart(Events::kHeaterChange);
	_cmdMessenger->sendCmdArg(index);
	_cmdMessenger->sendCmdArg(onOrOff);
	_cmdMessenger->sendCmdEnd();
}


