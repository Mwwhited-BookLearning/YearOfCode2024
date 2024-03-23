/********************************* Shenzhen Aerospace Electronics Co., Ltd *******************************
 * Experiment Name: Dynamic Display Experiment
 * Experiment Description: Display numbers sequentially from 0 to 100, repeating indefinitely; when the count reaches 100, reset to 0. Display is done on 4 units with separate display controls, and the units are displayed simultaneously.
 * Experiment Platform: Aerospace 51 microcontroller development board V1.1
 * Connection Method: A -> P00, B -> P01, C -> P02, D -> P03, E -> P04, F -> P05, G -> P06, DP -> P07
 *                     PUT1 -> P24, PUT2 -> P25, PUT3 -> P26, PUT4 -> P27
 * Note: The connections are for common anode display. If using common cathode display, connections and logic need to be adjusted accordingly. Additionally, the time of each display is about 1 ms, and the refresh rate is fast, suitable for high-speed scanning. Simultaneous displays are achievable.
 * Author: Aerospace Electronics Product Development Department    QQ: 1909197536
 * Store: http://shop120013844.taobao.com/
 ****************************************************************************************/

#include <reg52.h>
#include <intrins.h>

#define FOSC 11059200L // Crystal oscillator setting, default is 11.0592M Hz
// #define FOSC 12000000L // Crystal oscillator setting, using 12M Hz
// #define FOSC 24000000L // Crystal oscillator setting, using 24M Hz

#define TIME_MS 50 // Set timing time in ms, for 11.0592M, the minimum is 60ms

// IO interface definition
#define LED_PORT P0
sbit wela_1 = P2 ^ 4;
sbit wela_2 = P2 ^ 5;
sbit wela_3 = P2 ^ 6;
sbit wela_4 = P2 ^ 7;

// Global variables
unsigned char count, temp;

// LED display pattern 0-F
unsigned code table[] = {0Xc0, 0xf9, 0xa4, 0xb0, 0x99, 0x92, 0x82, 0xf8, 0x80, 0x90, 0x88, 0x83, 0xc6, 0xa1, 0x86, 0x8e};

/*******************************************************************************
 * Function Name: Delayms
 * Function Description: Implement ms-level delay
 * Input: ms
 * Output: None
 *******************************************************************************/
void Delayms(unsigned int ms)
{
	unsigned int i, j;
	for (i = 0; i < ms; i++)
#if FOSC == 11059200L
		for (j = 0; j < 114; j++)
			;
#elif FOSC == 12000000L
		for (j = 0; j < 123; j++)
			;
#elif FOSC == 24000000L
		for (j = 0; j < 249; j++)
			;
#else
		for (j = 0; j < 114; j++)
			;
#endif
}

/*******************************************************************************
 * Function Name: Timer0Init
 * Function Description: Timer 0 initialization
 * Input: None
 * Output: None
 *******************************************************************************/
void Timer0Init()
{
	TMOD = 0x01; // Set timer 0 mode to 1
	TH0 = (65536 - FOSC / 12 / 1000 * TIME_MS) / 256;
	TL0 = (65536 - FOSC / 12 / 1000 * TIME_MS) % 256;
	ET0 = 1; // Enable timer 0 interrupt
	TR0 = 1; // Start timer 0
	EA = 1;	 // Enable interrupts
}

/*******************************************************************************
 * Function Name: LEDdisplay
 * Function Description: Display the number at the given position
 * Input: num - number to be displayed
 * Output: None
 *******************************************************************************/
void LEDdisplay(unsigned int num)
{
	unsigned char qian, bai, shi, ge;
	qian = num / 1000;
	bai = num % 1000 / 100;
	shi = num % 100 / 10;
	ge = num % 10;

	wela_1 = 1; // Turn off all display units
	wela_2 = 1;
	wela_3 = 1;
	wela_4 = 1;

	wela_4 = 0; // Display thousands place
	LED_PORT = table[qian];
	Delayms(1);
	LED_PORT = 0xff;
	wela_4 = 1;

	wela_3 = 0; // Display hundreds place
	LED_PORT = table[bai];
	Delayms(1);
	LED_PORT = 0xff;
	wela_3 = 1;

	wela_2 = 0; // Display tens place
	LED_PORT = table[shi];
	Delayms(1);
	LED_PORT = 0xff;
	wela_2 = 1;

	wela_1 = 0; // Display ones place
	LED_PORT = table[ge];
	Delayms(1);
	LED_PORT = 0xff;
}

/*******************************************************************************
 * Function Name: main
 * Function Description: Main function
 * Input: None
 * Output: None
 *******************************************************************************/
void main