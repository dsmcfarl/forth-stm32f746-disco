\ Driver for EM7180 SENtral Motion Coprocessor. In particular, it is for the
\ MPU9250 based Ultimate Sensor Fusion Solution available from:
\ https://www.tindie.com/products/onehorse/ultimate-sensor-fusion-solution-mpu9250/

#require i2c.fs

$37 constant SENTRAL_STATUS
$28 constant SENTRAL_ADDR
$9B constant SENTRAL_RESET_REQUEST
$1  constant SENTRAL_STATUS_EEPROM_DETECTED
$2  constant SENTRAL_STATUS_EE_UPLOAD_DONE
$4  constant SENTRAL_STATUS_EE_UPLOAD_ERROR
$8  constant SENTRAL_STATUS_IDLE
$10 constant SENTRAL_STATUS_NO_EEPROM

2 buffer: _BUF
: _write-reg-to-sentral ( reg -- ) _BUF c! _BUF SENTRAL_ADDR 1 i2c1-write drop ;
: _read-byte-from-sentral ( -- byte ) _BUF SENTRAL_ADDR 1 i2c1-read drop _BUF c@ ;
: read-byte-from-sentral-reg ( reg -- byte )
  _write-reg-to-sentral
  _read-byte-from-sentral
;

: write-byte-to-sentral-reg ( byte reg -- )
  _BUF c!
  _BUF #1 + c!
  _BUF SENTRAL_ADDR 2 i2c1-write
  drop
;

: _status@ ( -- status ) SENTRAL_STATUS read-byte-from-sentral-reg ;
SENTRAL_STATUS_EEPROM_DETECTED SENTRAL_STATUS_EE_UPLOAD_DONE or SENTRAL_STATUS_IDLE or constant _READY
: _reset ( -- ) $1 SENTRAL_RESET_REQUEST write-byte-to-sentral-reg ;
: _to-string ( ud|d -- c-addr length ) 0 <# #s #> ;	\ the buffer for pictured strings is reused so must use immediately
: _wait-ready ( -- )
  _status@ dup _READY <> if begin
    s" init-sentral: not ready, resetting: " log.warning _to-string log.warning-append
    _reset
    500 delay
    _status@ _READY <>
  until then drop
;
: init-sentral ( -- )
  init-i2c1
  _wait-ready
;
