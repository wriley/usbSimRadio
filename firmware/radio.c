#include <avr/io.h>
#include <avr/interrupt.h>
#include <util/delay.h>
#include <avr/pgmspace.h>
#include <string.h>
#include "radio.h"

static void radioInit(void);
static void radioUpdate(void);
static char radioChanged(void);
static void radioBuildReport(unsigned char *reportBuffer);

// report matching the most recent bytes from the radio
static unsigned char last_read_radio_bytes[REPORT_SIZE];
// the most recently reported bytes
static unsigned char last_reported_radio_bytes[REPORT_SIZE];

volatile int8_t enc_delta1, enc_delta2, enc_delta3, enc_delta4;
static int8_t last1, last2, last3, last4;
static unsigned char buttonLast = 0xff;
static buttonCounter = 0;

static void radioInit( void )
{
    unsigned char sreg;
    sreg = SREG;
    cli();
    
    int8_t new1, new2, new3, new4;
    ENCODERDDR = 0x00;
    ENCODERPORT |= 0xff;
    BUTTONDDR = 0x00;
    BUTTONPORT |= 0xff;
 
    new1 = 0;
    if( PHASE_A_1 )
        new1 = 3;
    if( PHASE_B_1 )
        new1 ^= 1;
    last1 = new1;
    enc_delta1 = 0;
    
    new2 = 0;
    if( PHASE_A_2 )
        new2 = 3;
    if( PHASE_B_2 )
        new2 ^= 1;
    last2 = new2;
    enc_delta2 = 0;
    
    new3 = 0;
    if( PHASE_A_3 )
        new3 = 3;
    if( PHASE_B_3 )
        new3 ^= 1;
    last3 = new3;
    enc_delta3 = 0;
    
    new4 = 0;
    if( PHASE_A_4 )
        new4 = 3;
    if( PHASE_B_4 )
        new4 ^= 1;
    last4 = new4;
    enc_delta4 = 0;
    
    radioUpdate();
    
    SREG = sreg;
    
    TCCR0 = 0<<CS02^1<<CS01^1<<CS00;		// CTC, XTAL / 64
    OCR1B = (uint8_t)(F_CPU / 64.0 * 1e-3 - 0.5);	// 1ms
    TIMSK |= 1<<TOIE0;
    
    TCCR2 = (1<<WGM21)|(1<<CS22)|(1<<CS21)|(1<<CS20);
	OCR2 = 196; // for 60 hz
}
 
 
ISR( SIG_OVERFLOW0 )				// 1ms for manual movement
{
    int8_t new1, new2, new3, new4, diff1, diff2, diff3, diff4;
    unsigned char buttons;
    
    new1 = 0;
    if( PHASE_A_1 )
        new1 = 3;
    if( PHASE_B_1 )
        new1 ^= 1;					        // convert gray to binary
    diff1 = last1 - new1;				    // difference last - new
    if( diff1 & 1 ){				        // bit 0 = value (1)
        last1 = new1;					    // store new as next last
        enc_delta1 += (diff1 & 2) - 1;		// bit 1 = direction (+/-)
    }

    new2 = 0;
    if( PHASE_A_2 )
        new2 = 3;
    if( PHASE_B_2 )
        new2 ^= 1;					        // convert gray to binary
    diff2 = last2 - new2;				    // difference last - new
    if( diff2 & 1 ){				        // bit 0 = value (1)
        last2 = new2;					    // store new as next last
        enc_delta2 += (diff2 & 2) - 1;	   	// bit 1 = direction (+/-)
    }

    new3 = 0;
    if( PHASE_A_3 )
        new3 = 3;
    if( PHASE_B_3 )
        new3 ^= 1;					        // convert gray to binary
    diff3 = last3 - new3;				    // difference last - new
    if( diff3 & 1 ){				        // bit 0 = value (1)
        last3 = new3;					    // store new as next last
        enc_delta3 += (diff3 & 2) - 1;		// bit 1 = direction (+/-)
    }

    new4 = 0;
    if( PHASE_A_4 )
        new4 = 3;
    if( PHASE_B_4 )
        new4 ^= 1;					        // convert gray to binary
    diff4 = last4 - new4;				    // difference last - new
    if( diff4 & 1 ){				        // bit 0 = value (1)
        last4 = new4;					    // store new as next last
        enc_delta4 += (diff4 & 2) - 1;		// bit 1 = direction (+/-)
    }
    
    if(last_read_radio_bytes[OFFSET_BUTTONS] & BUTTON_SWAP_1) { swapFrequency(1); }
    if(last_read_radio_bytes[OFFSET_BUTTONS] & BUTTON_SWAP_2) { swapFrequency(2); }

    if(buttonCounter == 0) { buttons = buttonsLast = BUTTONPIN; }
    buttonCounter++;
    if(buttonCounter > BUTTON_DEBOUNCE)
    {
        buttonCounter = 0;
        buttons = BUTTONPIN;
        if(memcmp(buttons, buttonsLast, sizeof(buttons)) != 0)
        {
            last_read_radio_bytes[OFFSET_BUTTON] = buttons;
        }
    }
}

static void swapFrequency(char i)
{
    unsigned char major, minor;
    if(i == 1){
        major = last_read_radio_bytes[OFFSET_FREQ_STANDBY_1];
        minor = last_read_radio_bytes[OFFSET_FREQ_STANDBY_1 + 1];
        last_read_radio_bytes[OFFSET_FREQ_STANDBY_1] = last_read_radio_bytes[OFFSET_FREQ_ACTIVE_1];
        last_read_radio_bytes[OFFSET_FREQ_STANDBY_1 + 1] = last_read_radio_bytes[OFFSET_FREQ_ACTIVE_1 + 1];
        last_read_radio_bytes[OFFSET_FREQ_ACTIVE_1] = major;
        last_read_radio_bytes[OFFSET_FREQ_ACTIVE_1 + 1] = minor;
    }
    if(i == 2){
        major = last_read_radio_bytes[OFFSET_FREQ_STANDBY_2];
        minor = last_read_radio_bytes[OFFSET_FREQ_STANDBY_2 + 1];
        last_read_radio_bytes[OFFSET_FREQ_STANDBY_2] = last_read_radio_bytes[OFFSET_FREQ_ACTIVE_2];
        last_read_radio_bytes[OFFSET_FREQ_STANDBY_2 + 1] = last_read_radio_bytes[OFFSET_FREQ_ACTIVE_2 + 1];
        last_read_radio_bytes[OFFSET_FREQ_ACTIVE_2] = major;
        last_read_radio_bytes[OFFSET_FREQ_ACTIVE_2 + 1] = minor;
    }
}

static void updateFrequency(char i, int8_t v, int8_t min, int8_t max)
{
    int8_t curr = last_read_radio_bytes[i];
    curr += v;
    if(curr > max)
    {
        last_read_radio_bytes[i] = max;
    }
    else if(curr < min)
    {
        last_read_radio_bytes[i] = min;
    }
    else
    {
        last_read_radio_bytes[i] = curr;
    }
}

static void setFrequency(unsigned char i, unsigned char v)
{
    cli();
    last_read_radio_bytes[i] = v;
    last_reported_radio_bytes[i] = v;
    sei();
}

static void setButtons(unsigned char v)
{
    cli();
    last_read_radio_bytes[OFFSET_BUTTONS] = v;
    last_reported_radio_bytes[OFFSET_BUTTONS] = v;
    sei();
}
 
static void radioUpdate( void )
{
    int8_t val1, val2, val3, val4;
 
    cli();
    val1 = enc_delta1;
    enc_delta1 = val1 & 1;
    val2 = enc_delta2;
    enc_delta2 = val2 & 1;
    val3 = enc_delta3;
    enc_delta3 = val3 & 1;
    val4 = enc_delta4;
    enc_delta4 = val4 & 1;
    sei();
    
    if(val1 != 0) { updateFrequency(OFFSET_FREQ_STANDBY_1, val1 >> 1, 118, 136); }
    if(val2 != 0) { updateFrequency(OFFSET_FREQ_STANDBY_1 + 1, val2 >> 1, 0, 99); }
    if(val3 != 0) { updateFrequency(OFFSET_FREQ_STANDBY_2, val3 >> 1, 118, 136); }
    if(val4 != 0) { updateFrequency(OFFSET_FREQ_STANDBY_2 + 1, val4 >> 1, 0, 99); }
}

static char radioChanged(void)
{
    static int first = 1;
    if(first) [ first = 0; return 1 }
    
    return memcmp(last_read_radio_bytes, last_reported_radio_bytes, REPORT_SIZE);
}

static void radioBuildReport(unsigned char *reportBuffer)
{
    if(reportBuffer != NULL)
    {
        memcpy(reportBuffer, last_read_radio_bytes, REPORT_SIZE);
    }
    memcpy(last_reported_radio_bytes, last_read_radio_bytes, REPORT_SIZE);
}

Radio radio = {
    report_size:    REPORT_SIZE,
    init:           radioInit,
    update:         radioUpdate,
    changed:        radioChanged,
    buildReport:    readioBuildReport
};

Radio *getRadio(void)
{
    return &radio;
}

static void setRadio(unsigned char *setBuffer)
{
    setFrequency(OFFSET_FREQ_ACTIVE_1, setBuffer[OFFSET_FREQ_ACTIVE_1]);
    setFrequency(OFFSET_FREQ_ACTIVE_1 + 1, setBuffer[OFFSET_FREQ_ACTIVE_1 + 1]);
    setFrequency(OFFSET_FREQ_ACTIVE_2, setBuffer[OFFSET_FREQ_ACTIVE_2]);
    setFrequency(OFFSET_FREQ_ACTIVE_2 + 1, setBuffer[OFFSET_FREQ_ACTIVE_2 + 1]);
    setFrequency(OFFSET_FREQ_STANDBY_1, setBuffer[OFFSET_FREQ_STANDBY_1]);
    setFrequency(OFFSET_FREQ_STANDBY_1 + 1, setBuffer[OFFSET_FREQ_STANDBY_1 + 1]);
    setFrequency(OFFSET_FREQ_STANDBY_2, setBuffer[OFFSET_FREQ_STANDBY_2]);
    setFrequency(OFFSET_FREQ_STANDBY_2 + 1, setBuffer[OFFSET_FREQ_STANDBY_2 + 1]);
    setButtons(setBuffer[OFFSET_BUTTONS]);
}