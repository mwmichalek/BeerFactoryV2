#include "Thermometer.h"
#include <DallasTemperature.h>
#include <OneWire.h>

Thermometer::Thermometer() {}

Thermometer::Thermometer(const DallasTemperature &sensor, const DeviceAddress &probe, int cycleLengthInMillis) {
	_cycleLengthInMillis = cycleLengthInMillis;
	_timeToUpdate = 0;
	_temperature = 0;
	_probe = probe;
	_sensor = sensor;
}

void Thermometer::update() {
	unsigned long currentMillis = millis();

	if (currentMillis > _timeToUpdate) {
		_timeToUpdate = currentMillis + _cycleLengthInMillis;
		_sensor.requestTemperaturesByAddress(_probe);
		_temperature = _sensor.getTempF(_probe);
	}
}

double Thermometer::currentTemp() {
	return _temperature;
}

