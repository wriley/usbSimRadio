#define REPORT_SIZE		16
#define ENCODERPORT             PORTB
#define ENCODERDDR              DDRB
#define ENCODERPIN              PINB
#define BUTTONPORT              PORTA
#define BUTTONDDR               DDRA
#define BUTTONPIN               PINA
#define PHASE_A_1		        (ENCODERPIN & 1<<0)
#define PHASE_B_1		        (ENCODERPIN & 1<<1)
#define PHASE_A_2		        (ENCODERPIN & 1<<2)
#define PHASE_B_2		        (ENCODERPIN & 1<<3)
#define PHASE_A_3		        (ENCODERPIN & 1<<4)
#define PHASE_B_3		        (ENCODERPIN & 1<<5)
#define PHASE_A_4		        (ENCODERPIN & 1<<6)
#define PHASE_B_4		        (ENCODERPIN & 1<<7)
#define BUTTON1                 (BUTTONPIN & 1<<0)
#define BUTTON2                 (BUTTONPIN & 1<<1)
#define BUTTON3                 (BUTTONPIN & 1<<2)
#define BUTTON4                 (BUTTONPIN & 1<<3)
#define BUTTON5                 (BUTTONPIN & 1<<4)
#define BUTTON6                 (BUTTONPIN & 1<<5)
#define BUTTON7                 (BUTTONPIN & 1<<6)
#define BUTTON8                 (BUTTONPIN & 1<<7)
#define OFFSET_FREQ_ACTIVE_1    2
#define OFFSET_FREQ_STANDBY_1   4
#define OFFSET_FREQ_ACTIVE_2    6
#define OFFSET_FREQ_STANDBY_2   8
#define OFFSET_BUTTONS          10
#define BUTTON_DEBOUNCE         20
#define BUTTON_SWAP_1           0
#define BUTTON_SWAP_2           1
#define BUTTON_INCR_1           2
#define BUTTON_INCR_2           3

typedef struct {
    int report_size;
    void (*init)(void);
    void (*update)(void);
    char (*changed)(void);
    void (*buildReport)(unsigned char *buf);
} Radio

Radio *getRadio(void);
void setRadio(unsigned char *setBuffer);