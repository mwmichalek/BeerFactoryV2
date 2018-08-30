#include <Wire.h>
#include <LCD.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd = LiquidCrystal_I2C(0x27, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);

void setup()
{
	lcd.begin(20, 4);

	// Switch on the backlight
	//lcd.setBacklightPin(BACKLIGHT_PIN, POSITIVE);
	lcd.setBacklight(HIGH);
	lcd.home();                   // go home

	lcd.print("SainSmart I2C test");
	lcd.setCursor(0, 1);        // go to the next line
	lcd.print("F Malpartida library");
	lcd.setCursor(0, 2);        // go to the next line
	lcd.print("Test By Edward Comer");
	lcd.setCursor(0, 3);        // go to the next line
	lcd.print("Iteration No: ");
}

void loop()
{

}