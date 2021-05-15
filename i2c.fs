\ I2C1 driver for master send and receive, 7 bit addressing, 400KHz.

#require log.fs

\ Wait while the word test with address 'test results in a true, or until
\ timeout ends.  Returns false if 'test execute returns false before timeout,
\ otherwise true.
$ffff constant TIMEOUT
: true-until-timeout? ( 'test -- flag )
  TIMEOUT
  begin swap dup execute while
    swap 1- dup 0= if
      2drop true exit			\ timed out
    then
  repeat
  2drop false				\ 'test became false before timeout 
; 

: init-i2c-gpio-pin  ( port pin -- )
  2dup OPEN_DRAIN -rot otyper!
  2dup AF         -rot moder!
  2dup MED        -rot ospeedr!
       AF4        -rot afr!
;

%10 constant I2C1_HSI
: enable-i2c1-clock ( -- ) RCC APB1ENR_I2C1EN bfs! ;
: set-i2c1-clock-hsi ( -- ) I2C1_HSI RCC DCKCFGR2_I2C1SEL bf! ;
: reset-i2c1 ( -- ) RCC APB1RSTR_I2C1RST bfs! RCC APB1RSTR_I2C1RST bfc! ;
: disable-i2c1 ( -- ) I2C1 CR1_PE bfc! ;
: config-i2c1-timing ( -- )
  \ Ref: RM0385 p 990 (table 182) for example timing
  $9 TIMINGR_SCLL bf<<				\ SCL low period: tSCLL = (SCLL+1) x tPRESC
  $3 TIMINGR_SCLH bf<< or			\ SCL high period: tSCLH = (SCLH+1) x tPRESC
  $1 TIMINGR_SDADEL bf<< or			\ SDA delay: tSDADEL= SDADEL x tPRESC
  $3 TIMINGR_SCLDEL bf<< or			\ SCL delay: tSCLDEL = (SCLDEL+1) x tPRESC
  $0 TIMINGR_PRESC bf<< or I2C1_TIMINGR !	\ tPRESC = (PRESC+1) x tI2CCLK
;
: enable-i2c1 ( -- ) I2C1 CR1_PE bfs! ;
: i2c1-init ( -- )
  GPIOB 8 init-i2c-gpio-pin
  GPIOB 9 init-i2c-gpio-pin
  enable-i2c1-clock
  set-i2c1-clock-hsi
  reset-i2c1
  disable-i2c1
  config-i2c1-timing
  enable-i2c1
;

: i2c1-busy? ( -- flag ) I2C1 ISR_BUSY bf@ ;
: i2c1-rxdr-empty? ( -- flag ) I2C1 ISR_RXNE bf@ 0= ;
: i2c1-txdr-not-empty? ( -- flag ) I2C1 ISR_TXIS bf@ 0= ;
: i2c1-stop-flag-not-set? ( -- flag) I2C1 ISR_STOPF bf@ 0= ;

: timed-out-waiting-for-i2c1? ( -- flag ) ['] i2c1-busy? true-until-timeout? ;
: prepare-read ( slave-addr nbytes -- nbytes )
  swap over
  CR2_NBYTES bf<<
  swap #1 lshift		\ in 7-bit mode, slave addr starts at bit1 not 0
  CR2_SADD bf<< or
  $1 CR2_AUTOEND bf<< or	\ auto end mode	
  $1 CR2_RD_WRN bf<< or		\ master read request
  $1 CR2_START bf<< or		\ Start transfer
  I2C1_CR2 !
;
: timed-out-waiting-for-i2c1-rxdr-not-empty? ( -- ) ['] i2c1-rxdr-empty? true-until-timeout? ;
#0 constant STATUS_OK
#1 constant STATUS_BUSY
#2 constant STATUS_RXDR_EMPTY
: save-rxdr-to-addr ( addr -- ) I2C1_RXDR c@ swap c! ;
: i2c1-read ( buf-addr slave-addr nbytes -- status )
  timed-out-waiting-for-i2c1? if
    s" i2c1-read: bus is busy timeout" log.debug
    2drop drop STATUS_BUSY exit
  then
  prepare-read
  0 do
    timed-out-waiting-for-i2c1-rxdr-not-empty? if
      s" i2c1-read: timed out waiting for RXDR not empty" log.debug
      drop STATUS_RXDR_EMPTY unloop exit
    then
    dup i + 
    save-rxdr-to-addr
  loop
  drop STATUS_OK
;

: i2c1-write ( buf-addr slave-addr nbytes -- status )

  \ If nbytes = 0, nothing to do
  dup 0= if 
    2drop drop 0 exit
  then
  
  ['] i2c1-busy? true-until-timeout? if
    2drop drop 
    cr cr ." DEBUG: i2c1-write: bus is busy timeout" cr
    1 exit
  then
  
  \ Prepare for transfer
  swap over
  CR2_NBYTES bf<<
  swap #1 lshift		\ in 7-bit mode, slave addr starts at bit1 not 0
  CR2_SADD bf<< or
  $1 CR2_AUTOEND bf<< or	\ auto end mode	
  $1 CR2_START bf<< or		\ Start transfer
  I2C1_CR2 !
  I2C1 ISR_TXIS bfs!
  0 do  \ nbyte consumed, only buf-addr remains
    \ Wait until tx buffer is empty
    ['] i2c1-txdr-not-empty? true-until-timeout? if
      drop 
      cr cr ." DEBUG: i2c1-write: tx buffer not empty timeout" cr
      2 unloop exit
    then
    \ Send next byte of data
    dup i + c@ I2C1_TXDR c!
    \ Stop transfer if data byte is not acknowledged by slave
    %1 4 lshift I2C1_ISR @ and if
      cr cr ." DEBUG: i2c1-write: tx stopped early, slave NACK" cr
      leave
    then
  loop
  drop    \ Drop buf-addr

  ['] i2c1-stop-flag-not-set? true-until-timeout? if
    cr cr ." DEBUG: i2c1-write: no stop generated timeout" cr
    3 exit
  then
  %1 5 lshift I2C1_ICR bis!   \ Clear stop flag
  %1 4 lshift I2C1_ISR @ and if   \ NACK received from slave?
    cr cr ." DEBUG: i2c1-write: NACK at end of transfer" cr
    4 exit
  then

  0   \ No error exit status
;
