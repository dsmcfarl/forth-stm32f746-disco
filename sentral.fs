#require i2c.fs

: init-sentral ( -- ) init-i2c1 ;

$37 constant SENTRAL_STATUS
$28 constant SENTRAL_ADDR
$9B constant SENTRAL_RESET_REQUEST
6 buffer: BUF

: write-reg-to-sentral ( reg -- ) BUF c! BUF SENTRAL_ADDR 1 i2c1-write drop ;
: read-byte-from-sentral ( -- byte ) BUF SENTRAL_ADDR 1 i2c1-read drop BUF c@ ;
: read-byte-from-sentral-reg ( reg -- byte )
  write-reg-to-sentral
  read-byte-from-sentral
;

: write-byte-to-sentral-reg ( byte reg -- )
  BUF c!
  BUF #1 + c!
  BUF SENTRAL_ADDR 2 i2c1-write
  drop
;

: reset-sentral ( -- )
  $1 SENTRAL_RESET_REQUEST write-byte-to-sentral-reg
;
