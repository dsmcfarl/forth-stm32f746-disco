#216000 constant HCLK
$E000E010 constant SYST			\ SysTick (not defined in CMSIS-SVD)
SYST $0 + constant SYST_CSR		\ SysTick control and status register. R/W reset value = $00000000
SYST $4 + constant SYST_RVR		\ SysTick reload value register. R/W reset value = unknown
SYST $8 + constant SYST_CVR		\ SysTick current value register. R/W reset value = unknown  
SYST $C + constant SYST_CALIB		\ SysTick calibration value register. Read Only, reset value = $4000493E for the STM32F7

\ SYST_CSR Reset: 0x00000000
: CSR_COUNTFLAG ( -- addr-offset bit-offset width ) $0 #16 #1 ;		\ (read-only) Indicates whether the counter has counted to 0 since the last read of this register
: CSR_CLKSOURCE ( -- addr-offset bit-offset width ) $0 #2 #1 ;		\ (read-write) Indicates the SysTick clock source
: CSR_TICKINT ( -- addr-offset bit-offset width ) $0 #1 #1 ;		\ (read-write) Indicates whether counting to 0 causes the status of the SysTick exception to change to pending
: CSR_ENABLE ( -- addr-offset bit-offset width ) $0 #0 #1 ;		\ (read-write) Indicates the enabled status of the SysTick counter

\ SYST_RVR Reset: unknown
: RVR_RELOAD ( -- addr-offset bit-offset width ) $4 #0 #24 ;		\ (read-write) The value to load into the SYST_CVR when the counter reaches 0

\ SYST_CVR Reset: unknown
: CVR_CURRENT ( -- addr-offset bit-offset width ) $8 #0 #32 ;		\ (read-write) Current counter value

\ For STM32F7: The SysTick calibration value is fixed to 18750, which gives a
\ reference time base of 1 ms with the SysTick clock set to 18.75 MHz (HCLK/8,
\ with HCLK set to 150 MHz). Ref. RM0385 Rev 8 p290

\ The RCC feeds the external clock of the Cortex System Timer (SysTick) with
\ the AHB clock (HCLK) divided by 8.  The SysTick can work either with this clock
\ or with the Cortex clock (HCLK), configurable in the SysTick control and status
\ register. Rev. RM0385 Rev 8 p137

\ SYST_CALIB Reset: $4000493E
: CALIB_NOREF ( -- addr-offset bit-offset width ) $C #31 #1 ;		\ (read-only) Indicates whether the IMPLEMENTATIONDEFINED reference clock is implemented 
: CALIB_SKEW ( -- addr-offset bit-offset width ) $C #30 #1 ;		\ (read-only) Indicates whether the 10ms calibration value is exact 
: CALIB_TENMS ( -- addr-offset bit-offset width ) $C #0 #24 ;		\ (read-only) Optionally, holds a reload value to be used for 10ms (100Hz) timing 

0 0 2variable MS				\ can count to 64 bits or (2^64 - 1) miliseconds =  584,942,417 years

: systick-interrupt-enable ( -- ) SYST CSR_TICKINT bfs! ;
: systick-handler ( -- ) MS 2@ 1. d+ MS 2! ;
: systick-hclk-use ( -- ) SYST CSR_CLKSOURCE bfs! ;
: systick-rvr-set ( -- ) HCLK SYST RVR_RELOAD bf! ;
: systick-enable ( -- ) SYST CSR_ENABLE bfs! ;
: init-systick ( -- )
   systick-hclk-use
   systick-rvr-set
   systick-enable
  ['] systick-handler irq-systick !					\ This 'hooks' the systick-handler word (above) to the systick irq
  systick-interrupt-enable
;
: delay ( ms -- ) s>d MS 2@ d+ begin 2dup MS 2@ d= until 2drop ;

: init ( -- )
  init
  init-systick
  ." systick initialized" cr
;
