\ Wait while the word test with address 'test results in a true, or until
\ timeout ends.  Returns status = 0 if exits before timeout, returns
\ status = 1 if timeout occurs. 
: timed ( 'test -- status )
    $FFFFF                             \ Timeout value
    begin swap dup execute while
      swap 1- dup 0= if
        2drop 1 exit                  \ Timed out
      then
    repeat
    2drop 0                           \ 'test became false before timeout 
; 

: af4-med-open-drain! ( port pin -- )
  2dup OPEN_DRAIN -rot otyper!
  2dup AF         -rot moder!
  2dup MED        -rot ospeedr!
       AF4        -rot afr!
;

\ -------------------------------------------------------------------------
\  I2C1 driver for master send and receive, 7 bit addressing.
\  I2C speed is determined by value of variable i2c1-Fast (0 = 100 KHz,
\  1 = 400 KHz).  Timing values are from Table 76 of RM0360 for I2CCLK =
\  SYSCLK = 48 MHz.
\ -------------------------------------------------------------------------

%10 constant I2C1_HSI
: i2c1-init ( -- )
  GPIOB 8 af4-med-open-drain!
  GPIOB 9 af4-med-open-drain!
  RCC APB1ENR_I2C1EN bfs!		\ Enable peripheral clock
  I2C1_HSI RCC DCKCFGR2_I2C1SEL bf!	\ I2C1 clock source is HSI
  RCC APB1RSTR_I2C1RST bfs!		\ reset I2C1 
  RCC APB1RSTR_I2C1RST bfc!
  I2C1 CR1_PE bfc!			\ Disable I2C1
  \ Ref: RM0385 p 990 (table 182) for example timing
  \ TODO: define constants for these values
  $9 TIMINGR_SCLL bf<<
  $3 TIMINGR_SCLH bf<< or
  $1 TIMINGR_SDADEL bf<< or
  $3 TIMINGR_SCLDEL bf<< or
  $0 TIMINGR_PRESC bf<< or I2C1_TIMINGR !
  I2C1 CR1_PE bfs!			\ Enable I2C1
;

\ These words are used for timed waits
\ Return true if I2C1 busy, false if not busy
: i2c1-busy? ( -- flag ) I2C1 ISR_BUSY bf@ ;

\ Return true if the I2C1 rx buffer is empty
: i2c1-nrxne-new? ( -- flag ) I2C1 ISR_RXNE bf@ 0= ;
: i2c1-nrxne? ( -- flag )  %1 2 lshift I2C1_ISR bit@ not ;

: i2c1-ntxis? ( -- flag ) I2C1 ISR_TXIS bf@ 0= ;

\ Return true if stop flag not set
: i2c1-nstopf? ( -- flag) I2C1 ISR_STOPF bf@ 0= ;

\ I2C1 read/write nbytes bytes into/from buffer with address buf-addr
\ to slave with (left-shifted) address slave-addr
: i2c1-read ( buf-addr slave-addr nbytes -- exit-status )

  \ If nbytes = 0, nothing to do
  dup 0= if
    2drop drop 0 exit
  then
  
  ['] i2c1-busy? timed if
    2drop drop 
    cr cr ." DEBUG: i2c1-read: bus is busy timeout" cr
    1 exit
  then
  \ Prepare for transfer
  $0 I2C1_CR2 !                 \ Clear register
  swap over 16 lshift swap 1 lshift or        \ Slave address (consumed) and num bytes
  %1 25 lshift or               \ Auto end mode
  %1 10 lshift or               \ Master read
  %1 13 lshift or I2C1_CR2 !    \ Start transfer

  0 do  \ nbyte consumed, only buf-addr remains
    \ Wait until rx buffer has data
    ['] i2c1-nrxne? timed if
      drop 
      cr cr ." DEBUG: i2c1-read: rx buffer empty timeout" cr
      2 unloop exit
    then
    \ Receive next byte of data
    dup i + I2C1_RXDR c@ swap c!
  loop
  drop    \ Drop buf-addr
  
  0   \ No error exit status
;

: i2c1-write ( buf-addr slave-addr nbytes -- exit-status )

  \ If nbytes = 0, nothing to do
  dup 0= if 
    2drop drop 0 exit
  then
  
  ['] i2c1-busy? timed if
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
    ['] i2c1-ntxis? timed if
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

  ['] i2c1-nstopf? timed if
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
