/* Name: main.c
 * Project: hid-data, example how to use HID for data transfer
 * Author: Christian Starkjohann
 * Creation Date: 2008-04-11
 * Tabsize: 4
 * Copyright: (c) 2008 by OBJECTIVE DEVELOPMENT Software GmbH
 * License: GNU GPL v2 (see License.txt), GNU GPL v3 or proprietary (CommercialLicense.txt)
 * This Revision: $Id: main.c 692 2008-11-07 15:07:40Z cs $
 */

/*
This example should run on most AVRs with only little changes. No special
hardware resources except INT0 are used. You may have to change usbconfig.h for
different I/O pins for USB. Please note that USB D+ must be the INT0 pin, or
at least be connected to INT0 as well.
*/

#include <avr/io.h>
#include <avr/wdt.h>
#include <avr/interrupt.h>  /* for sei() */
#include <util/delay.h>     /* for _delay_ms() */
#include <avr/eeprom.h>

#include <avr/pgmspace.h>   /* required by usbdrv.h */
#include "usbdrv.h"
#include "oddebug.h"        /* This is also an example for using debug macros */

#define PHASE_A		(PINB & 1<<PB1)
#define PHASE_B		(PINB & 1<<PB2)
#define	XTAL		12e6			// 12MHz

/* ------------------------------------------------------------------------- */
/* ----------------------------- USB interface ----------------------------- */
/* ------------------------------------------------------------------------- */

PROGMEM char usbHidReportDescriptor[22] = {    /* USB report descriptor */
    0x06, 0x00, 0xff,              // USAGE_PAGE (Generic Desktop)
    0x09, 0x01,                    // USAGE (Vendor Usage 1)
    0xa1, 0x01,                    // COLLECTION (Application)
    0x15, 0x00,                    //   LOGICAL_MINIMUM (0)
    0x26, 0xff, 0x00,              //   LOGICAL_MAXIMUM (255)
    0x75, 0x08,                    //   REPORT_SIZE (8)
    0x95, 0x08,                    //   REPORT_COUNT (8)
    0x09, 0x00,                    //   USAGE (Undefined)
    0xb2, 0x02, 0x01,              //   FEATURE (Data,Var,Abs,Buf)
    0xc0                           // END_COLLECTION
};
/* Since we define only one feature report, we don't use report-IDs (which
 * would be the first byte of the report). The entire report consists of 16
 * opaque data bytes.
 */

static int8_t currentState[8];

void freqChange(uchar i, uchar c, uchar min, uchar max)
{
	uchar curr = currentState[i];
	curr += c;
	if(curr > max)
	{
		currentState[i] = max;
	}
	else if(curr < min)
	{
		currentState[i] = min;
	}
	else
	{
		currentState[i] = curr;
	}
}

void dataRead(uchar *data)
{
	for(int i = 0; i < 8; i++)
	{
		data[i] = currentState[i];
	}
}

void dataWrite(uchar *data)
{
for(int i = 0; i < 8; i++)
	{
		currentState[i] = data[i];
	}
}

/* ------------------------------------------------------------------------- */

/* usbFunctionRead() is called when the host requests a chunk of data from
 * the device. For more information see the documentation in usbdrv/usbdrv.h.
 */
uchar   usbFunctionRead(uchar *data, uchar len)
{
	PORTB ^= 0x01;
	dataRead(data);
   	return len;
}

/* usbFunctionWrite() is called when the host sends a chunk of data to the
 * device. For more information see the documentation in usbdrv/usbdrv.h.
 */
uchar   usbFunctionWrite(uchar *data, uchar len)
{
	dataWrite(data);
	return 1;
}

/* ------------------------------------------------------------------------- */

usbMsgLen_t usbFunctionSetup(uchar data[8])
{
usbRequest_t    *rq = (void *)data;
    if((rq->bmRequestType & USBRQ_TYPE_MASK) == USBRQ_TYPE_CLASS){    /* HID class request */
        if(rq->bRequest == USBRQ_HID_GET_REPORT){  /* wValue: ReportType (highbyte), ReportID (lowbyte) */
            /* since we have only one report type, we can ignore the report-ID */
            return USB_NO_MSG;  /* use usbFunctionRead() to obtain data */
        }else if(rq->bRequest == USBRQ_HID_SET_REPORT){
            /* since we have only one report type, we can ignore the report-ID */
            return USB_NO_MSG;  /* use usbFunctionWrite() to receive data from host */
        }
    }else{
        /* ignore vendor type requests, we don't use any */
    }
    return 0;
}

/* ------------------------------------------------------------------------- */

volatile int8_t enc_delta;			// -128 ... 127
static int8_t last;
 
 
void encode_init( void )
{
  int8_t new;
 
  new = 0;
  if( PHASE_A )
    new = 3;
  if( PHASE_B )
    new ^= 1;					// convert gray to binary
  last = new;					// power on state
  enc_delta = 0;
  TCCR0 = 0<<CS02^1<<CS01^1<<CS00;		// CTC, XTAL / 64
  OCR1B = (uint8_t)(XTAL / 64.0 * 1e-3 - 0.5);	// 1ms
  TIMSK |= 1<<TOIE0;
}
 
 
ISR( SIG_OVERFLOW0 )				// 1ms for manual movement
{
  int8_t new, diff;
 
  new = 0;
  if( PHASE_A )
    new = 3;
  if( PHASE_B )
    new ^= 1;					// convert gray to binary
  diff = last - new;				// difference last - new
  if( diff & 1 ){				// bit 0 = value (1)
    last = new;					// store new as next last
    enc_delta += (diff & 2) - 1;		// bit 1 = direction (+/-)
  }
}
 
 
int8_t encode_read1( void )			// read single step encoders
{
  int8_t val;
 
  cli();
  val = enc_delta;
  enc_delta = 0;
  sei();
  return val;					// counts since last call
}
 
 
int8_t encode_read2( void )			// read two step encoders
{
  int8_t val;
 
  cli();
  val = enc_delta;
  enc_delta = val & 1;
  sei();
  return val >> 1;
}
 
 
int8_t encode_read4( void )			// read four step encoders
{
  int8_t val;
 
  cli();
  val = enc_delta;
  enc_delta = val & 3;
  sei();
  return val >> 2;
}

int main(void)
{
uchar   i;

    wdt_enable(WDTO_1S);
    /* Even if you don't use the watchdog, turn it off here. On newer devices,
     * the status of the watchdog (on/off, period) is PRESERVED OVER RESET!
     */
    DBG1(0x00, 0, 0);       /* debug output: main starts */
    /* RESET status: all port bits are inputs without pull-up.
     * That's the way we need D+ and D-. Therefore we don't need any
     * additional hardware initialization.
     */
    odDebugInit();
    usbInit();
	encode_init();
    usbDeviceDisconnect();  /* enforce re-enumeration, do this while interrupts are disabled! */
    i = 0;
    while(--i){             /* fake USB disconnect for > 250 ms */
        wdt_reset();
        _delay_ms(1);
    }
    usbDeviceConnect();
    sei();
    DBG1(0x01, 0, 0);       /* debug output: main loop starts */

	currentState[0] = 1;
	currentState[1] = 120;
	currentState[2] = 35;
	currentState[3] = 129;
	currentState[4] = 60;
	currentState[5] = 0x0f;
	currentState[6] = 0xf0;
	currentState[7] = 0;

	DDRB = 0x00;
	DDRB ^= 0x01;
	PORTB = 0xff;

    for(;;){                /* main event loop */
        DBG1(0x02, 0, 0);   /* debug output: main loop iterates */
        wdt_reset();
        usbPoll();
		freqChange(1, encode_read2(), 118, 136);
    }
    return 0;
}

/* ------------------------------------------------------------------------- */
