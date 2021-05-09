compiletoflash
#require clock.fs
#require debug.fs
#require systick.fs
#require graphics.fs
compiletoram

\ if we init-sysclk before #require graphics.fs, the font arrays do not get
\ initialized (all zeros) when graphics.fs is compiled to flash
init-sysclk
init-mco1
init-systick
init-graphics

: demo-graphics ( -- )
  red1 color!
  50 14 32 12 ellipse
  50 14 34 14 ellipse
  lightgreen color!
  8x16 font!
  s" Mecrisp" 22 7 drawstring
  yellow1 color!
  2 4 12 24 line
  4 4 14 24 line
  118 color!
  4x6 font!
  s" 123456789012345678901234567890123456789012345678901234567890" 0 80 drawstring
  8x16 font!
  gold3 color!
  s" hello world!" 10 40 drawstring
  orange1 color!
  s" ÄÖÜß" 10 60 drawstring

;
demo-graphics
: wait ( ms -- ) ms @ + begin dup ms @ <= until ;
#require i2c.fs
i2c1-init
$37 constant SENTRAL_STATUS
$28 constant SENTRAL_ADDR
$9B constant SENTRAL_RESET_REQUEST
6 buffer: BUF

: write-reg ( reg -- )
  BUF c!
  BUF SENTRAL_ADDR 1 i2c1-write
  h.
;
: write-reg-val ( val reg -- )
  BUF c!
  BUF #1 + c!
  BUF SENTRAL_ADDR 2 i2c1-write
  h.
;
: read-byte ( reg -- val )
  write-reg
  500 wait
  BUF SENTRAL_ADDR 1 i2c1-read
  h.
  BUF c@
;
: reset-request ( -- )
  $1 SENTRAL_RESET_REQUEST write-reg-val
;
sentral_status read-byte
h.
