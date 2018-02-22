// 
// 
// 
#include "Arduino.h"
#include "HeatingElement.h"

///
/// Used to allow power to the element.  Percentage is controlled by the SSR (Kettle)
///
HeatingElement::HeatingElement() {

}

HeatingElement::HeatingElement(int relayPin, const int index) {
	pinMode(relayPin, OUTPUT);
	_relayPin = relayPin;
	_index = index;
	//Serial.println("HeatingElement[" + String(_index) + "]: created.");
	//engage(false);
}

HeatingElement::HeatingElement(int relayPin, const String name) {
	pinMode(relayPin, OUTPUT);
	_relayPin = relayPin;
	_name = name;
	//engage(false);
}

void HeatingElement::engage(bool isEngaged) {
	_isEngaged = isEngaged;
	//Serial.println("HeatingElement[" + _name + "]: engaged: " + String(isEngaged));
	//Serial.println("HeatingElement[" + String(_index) + "]: engaged: " + String(isEngaged));
	if (isEngaged)
		digitalWrite(_relayPin, HIGH);
	else
		digitalWrite(_relayPin, LOW);
}


