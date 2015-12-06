EESchema Schematic File Version 2
LIBS:power
LIBS:device
LIBS:transistors
LIBS:conn
LIBS:linear
LIBS:regul
LIBS:74xx
LIBS:cmos4000
LIBS:adc-dac
LIBS:memory
LIBS:xilinx
LIBS:microcontrollers
LIBS:dsp
LIBS:microchip
LIBS:analog_switches
LIBS:motorola
LIBS:texas
LIBS:intel
LIBS:audio
LIBS:interface
LIBS:digital-audio
LIBS:philips
LIBS:display
LIBS:cypress
LIBS:siliconi
LIBS:opto
LIBS:atmel
LIBS:contrib
LIBS:valves
LIBS:wriley-custom
LIBS:usbSimRadio-cache
EELAYER 25 0
EELAYER END
$Descr USLetter 11000 8500
encoding utf-8
Sheet 1 1
Title "usbSimRadio"
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L HS-3461B D?
U 1 1 56639E81
P 1800 1750
F 0 "D?" H 1550 2200 60  0000 C CNN
F 1 "HS-3461B" V 1800 1750 60  0000 C CNN
F 2 "" H 1250 1400 60  0000 C CNN
F 3 "" H 1250 1400 60  0000 C CNN
	1    1800 1750
	1    0    0    -1  
$EndComp
$Comp
L 74LS164 U?
U 1 1 5663A2BE
P 2950 4550
F 0 "U?" H 2650 4950 60  0000 C CNN
F 1 "74LS164" V 2950 4450 60  0000 C CNN
F 2 "" H 2950 4550 60  0000 C CNN
F 3 "" H 2950 4550 60  0000 C CNN
	1    2950 4550
	1    0    0    -1  
$EndComp
$Comp
L CONN_01X02 P?
U 1 1 5663A447
P 1150 7100
F 0 "P?" H 1150 7250 50  0000 C CNN
F 1 "CONN_01X02" V 1250 7100 50  0000 C CNN
F 2 "" H 1150 7100 60  0000 C CNN
F 3 "" H 1150 7100 60  0000 C CNN
	1    1150 7100
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663A9FF
P 950 7050
F 0 "#PWR?" H 950 6900 50  0001 C CNN
F 1 "+5V" H 950 7190 50  0000 C CNN
F 2 "" H 950 7050 60  0000 C CNN
F 3 "" H 950 7050 60  0000 C CNN
	1    950  7050
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR?
U 1 1 5663AA19
P 950 7150
F 0 "#PWR?" H 950 6900 50  0001 C CNN
F 1 "GND" H 950 7000 50  0000 C CNN
F 2 "" H 950 7150 60  0000 C CNN
F 3 "" H 950 7150 60  0000 C CNN
	1    950  7150
	1    0    0    -1  
$EndComp
$Comp
L CONN_01X02 P?
U 1 1 5663AA33
P 2000 7100
F 0 "P?" H 2000 7250 50  0000 C CNN
F 1 "CONN_01X02" V 2100 7100 50  0000 C CNN
F 2 "" H 2000 7100 60  0000 C CNN
F 3 "" H 2000 7100 60  0000 C CNN
	1    2000 7100
	1    0    0    -1  
$EndComp
Text GLabel 1800 7050 0    60   Input ~ 0
DDAT
Text GLabel 1800 7150 0    60   Input ~ 0
DCLK
$Comp
L +5V #PWR?
U 1 1 5663AD9E
P 2950 4000
F 0 "#PWR?" H 2950 3850 50  0001 C CNN
F 1 "+5V" H 2950 4140 50  0000 C CNN
F 2 "" H 2950 4000 60  0000 C CNN
F 3 "" H 2950 4000 60  0000 C CNN
	1    2950 4000
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR?
U 1 1 5663ADBA
P 2950 5300
F 0 "#PWR?" H 2950 5050 50  0001 C CNN
F 1 "GND" H 2950 5150 50  0000 C CNN
F 2 "" H 2950 5300 60  0000 C CNN
F 3 "" H 2950 5300 60  0000 C CNN
	1    2950 5300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663ADD6
P 2400 4000
F 0 "#PWR?" H 2400 3850 50  0001 C CNN
F 1 "+5V" H 2400 4140 50  0000 C CNN
F 2 "" H 2400 4000 60  0000 C CNN
F 3 "" H 2400 4000 60  0000 C CNN
	1    2400 4000
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 5663ADF2
P 2400 4150
F 0 "R?" V 2480 4150 50  0000 C CNN
F 1 "10K" V 2400 4150 50  0000 C CNN
F 2 "" V 2330 4150 30  0000 C CNN
F 3 "" H 2400 4150 30  0000 C CNN
	1    2400 4150
	1    0    0    -1  
$EndComp
Text GLabel 2400 4400 0    60   Input ~ 0
DDAT
Text GLabel 2400 4800 0    60   Input ~ 0
DCLK
$Comp
L R R?
U 1 1 5663AEEF
P 2150 5000
F 0 "R?" V 2230 5000 50  0000 C CNN
F 1 "10K" V 2150 5000 50  0000 C CNN
F 2 "" V 2080 5000 30  0000 C CNN
F 3 "" H 2150 5000 30  0000 C CNN
	1    2150 5000
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663AF76
P 2000 5000
F 0 "#PWR?" H 2000 4850 50  0001 C CNN
F 1 "+5V" H 2000 5140 50  0000 C CNN
F 2 "" H 2000 5000 60  0000 C CNN
F 3 "" H 2000 5000 60  0000 C CNN
	1    2000 5000
	1    0    0    -1  
$EndComp
Wire Wire Line
	2300 5000 2400 5000
Text GLabel 3450 4300 2    60   Input ~ 0
D1DIG1
Text GLabel 3450 4400 2    60   Input ~ 0
D1DIG2
Text GLabel 3450 4500 2    60   Input ~ 0
D1DIG3
Text GLabel 3450 4600 2    60   Input ~ 0
D1DIG4
Text GLabel 3450 4700 2    60   Input ~ 0
D2DIG1
Text GLabel 3450 4800 2    60   Input ~ 0
D2DIG2
Text GLabel 3450 4900 2    60   Input ~ 0
D2DIG3
Text GLabel 3450 5000 2    60   Input ~ 0
D2DIG4
$Comp
L 74LS164 U?
U 1 1 5663B25C
P 5050 4550
F 0 "U?" H 4750 4950 60  0000 C CNN
F 1 "74LS164" V 5050 4450 60  0000 C CNN
F 2 "" H 5050 4550 60  0000 C CNN
F 3 "" H 5050 4550 60  0000 C CNN
	1    5050 4550
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B262
P 5050 4000
F 0 "#PWR?" H 5050 3850 50  0001 C CNN
F 1 "+5V" H 5050 4140 50  0000 C CNN
F 2 "" H 5050 4000 60  0000 C CNN
F 3 "" H 5050 4000 60  0000 C CNN
	1    5050 4000
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR?
U 1 1 5663B268
P 5050 5300
F 0 "#PWR?" H 5050 5050 50  0001 C CNN
F 1 "GND" H 5050 5150 50  0000 C CNN
F 2 "" H 5050 5300 60  0000 C CNN
F 3 "" H 5050 5300 60  0000 C CNN
	1    5050 5300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B26E
P 4500 4000
F 0 "#PWR?" H 4500 3850 50  0001 C CNN
F 1 "+5V" H 4500 4140 50  0000 C CNN
F 2 "" H 4500 4000 60  0000 C CNN
F 3 "" H 4500 4000 60  0000 C CNN
	1    4500 4000
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 5663B274
P 4500 4150
F 0 "R?" V 4580 4150 50  0000 C CNN
F 1 "10K" V 4500 4150 50  0000 C CNN
F 2 "" V 4430 4150 30  0000 C CNN
F 3 "" H 4500 4150 30  0000 C CNN
	1    4500 4150
	1    0    0    -1  
$EndComp
Text GLabel 4500 4400 0    60   Input ~ 0
D2DIG4
Text GLabel 4500 4800 0    60   Input ~ 0
DCLK
$Comp
L R R?
U 1 1 5663B27C
P 4250 5000
F 0 "R?" V 4330 5000 50  0000 C CNN
F 1 "10K" V 4250 5000 50  0000 C CNN
F 2 "" V 4180 5000 30  0000 C CNN
F 3 "" H 4250 5000 30  0000 C CNN
	1    4250 5000
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B282
P 4100 5000
F 0 "#PWR?" H 4100 4850 50  0001 C CNN
F 1 "+5V" H 4100 5140 50  0000 C CNN
F 2 "" H 4100 5000 60  0000 C CNN
F 3 "" H 4100 5000 60  0000 C CNN
	1    4100 5000
	1    0    0    -1  
$EndComp
Wire Wire Line
	4400 5000 4500 5000
Text GLabel 5550 4300 2    60   Input ~ 0
D3DIG1
Text GLabel 5550 4400 2    60   Input ~ 0
D3DIG2
Text GLabel 5550 4500 2    60   Input ~ 0
D3DIG3
Text GLabel 5550 4600 2    60   Input ~ 0
D3DIG4
Text GLabel 5550 4700 2    60   Input ~ 0
D4DIG1
Text GLabel 5550 4800 2    60   Input ~ 0
D4DIG2
Text GLabel 5550 4900 2    60   Input ~ 0
D4DIG3
Text GLabel 5550 5000 2    60   Input ~ 0
D4DIG4
$Comp
L 74LS164 U?
U 1 1 5663B636
P 7150 4550
F 0 "U?" H 6850 4950 60  0000 C CNN
F 1 "74LS164" V 7150 4450 60  0000 C CNN
F 2 "" H 7150 4550 60  0000 C CNN
F 3 "" H 7150 4550 60  0000 C CNN
	1    7150 4550
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B63C
P 7150 4000
F 0 "#PWR?" H 7150 3850 50  0001 C CNN
F 1 "+5V" H 7150 4140 50  0000 C CNN
F 2 "" H 7150 4000 60  0000 C CNN
F 3 "" H 7150 4000 60  0000 C CNN
	1    7150 4000
	1    0    0    -1  
$EndComp
$Comp
L GND #PWR?
U 1 1 5663B642
P 7150 5300
F 0 "#PWR?" H 7150 5050 50  0001 C CNN
F 1 "GND" H 7150 5150 50  0000 C CNN
F 2 "" H 7150 5300 60  0000 C CNN
F 3 "" H 7150 5300 60  0000 C CNN
	1    7150 5300
	1    0    0    -1  
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B648
P 6600 4000
F 0 "#PWR?" H 6600 3850 50  0001 C CNN
F 1 "+5V" H 6600 4140 50  0000 C CNN
F 2 "" H 6600 4000 60  0000 C CNN
F 3 "" H 6600 4000 60  0000 C CNN
	1    6600 4000
	1    0    0    -1  
$EndComp
$Comp
L R R?
U 1 1 5663B64E
P 6600 4150
F 0 "R?" V 6680 4150 50  0000 C CNN
F 1 "10K" V 6600 4150 50  0000 C CNN
F 2 "" V 6530 4150 30  0000 C CNN
F 3 "" H 6600 4150 30  0000 C CNN
	1    6600 4150
	1    0    0    -1  
$EndComp
Text GLabel 6600 4400 0    60   Input ~ 0
D4DIG4
Text GLabel 6600 4800 0    60   Input ~ 0
DCLK
$Comp
L R R?
U 1 1 5663B656
P 6350 5000
F 0 "R?" V 6430 5000 50  0000 C CNN
F 1 "10K" V 6350 5000 50  0000 C CNN
F 2 "" V 6280 5000 30  0000 C CNN
F 3 "" H 6350 5000 30  0000 C CNN
	1    6350 5000
	0    1    1    0   
$EndComp
$Comp
L +5V #PWR?
U 1 1 5663B65C
P 6200 5000
F 0 "#PWR?" H 6200 4850 50  0001 C CNN
F 1 "+5V" H 6200 5140 50  0000 C CNN
F 2 "" H 6200 5000 60  0000 C CNN
F 3 "" H 6200 5000 60  0000 C CNN
	1    6200 5000
	1    0    0    -1  
$EndComp
Wire Wire Line
	6500 5000 6600 5000
Text GLabel 7650 4300 2    60   Input ~ 0
D3DIG1
Text GLabel 7650 4400 2    60   Input ~ 0
D3DIG2
Text GLabel 7650 4500 2    60   Input ~ 0
D3DIG3
Text GLabel 7650 4600 2    60   Input ~ 0
D3DIG4
Text GLabel 7650 4700 2    60   Input ~ 0
D4DIG1
Text GLabel 7650 4800 2    60   Input ~ 0
D4DIG2
Text GLabel 7650 4900 2    60   Input ~ 0
D4DIG3
Text GLabel 7650 5000 2    60   Input ~ 0
D4DIG4
Text GLabel 1300 1450 0    60   Input ~ 0
D1DIG1
Text GLabel 1300 1550 0    60   Input ~ 0
D1DIG2
Text GLabel 1300 1650 0    60   Input ~ 0
D1DIG3
Text GLabel 1300 1750 0    60   Input ~ 0
D1DIG4
Text GLabel 2200 1450 2    60   Input ~ 0
SEGA
Text GLabel 2200 1550 2    60   Input ~ 0
SEGB
Text GLabel 2200 1650 2    60   Input ~ 0
SEGC
Text GLabel 2200 1750 2    60   Input ~ 0
SEGD
Text GLabel 2200 1850 2    60   Input ~ 0
SEGE
Text GLabel 2200 1950 2    60   Input ~ 0
SEGF
Text GLabel 2200 2050 2    60   Input ~ 0
SEGG
Text GLabel 1300 2050 0    60   Input ~ 0
SEGP
$EndSCHEMATC
