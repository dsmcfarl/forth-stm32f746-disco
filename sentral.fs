\ Driver for EM7180 SENtral Motion Coprocessor. In particular, it is for the
\ MPU9250 based Ultimate Sensor Fusion Solution available from:
\ https://www.tindie.com/products/onehorse/ultimate-sensor-fusion-solution-mpu9250/

#require i2c.fs

$28 constant EM7180_ADDRESS
$00 constant EM7180_QX
$04 constant EM7180_QY
$08 constant EM7180_QZ
$0C constant EM7180_QW
$10 constant EM7180_Q_TIME
$12 constant EM7180_MX
$14 constant EM7180_MY
$16 constant EM7180_MZ
$18 constant EM7180_M_TIME
$1A constant EM7180_AX
$1C constant EM7180_AY
$1E constant EM7180_AZ
$20 constant EM7180_A_TIME
$22 constant EM7180_GX
$24 constant EM7180_GY
$26 constant EM7180_GZ
$28 constant EM7180_G_TIME
$2A constant EM7180_BARO
$2C constant EM7180_BARO_TIME
$2E constant EM7180_TEMP
$30 constant EM7180_TEMP_TIME
$32 constant EM7180_Q_RATE_DIVISOR
$33 constant EM7180_ENABLE_EVENTS
$34 constant EM7180_HOST_CONTROL
$35 constant EM7180_EVENT_STATUS
$36 constant EM7180_SENSOR_STATUS
$37 constant EM7180_SENTRAL_STATUS
$38 constant EM7180_ALGORITHM_STATUS
$39 constant EM7180_FEATURE_FLAGS
$3A constant EM7180_PARAM_ACKNOWLEDGE
$3B constant EM7180_SAVED_PARAM_BYTE_0
$3C constant EM7180_SAVED_PARAM_BYTE_1
$3D constant EM7180_SAVED_PARAM_BYTE_2
$3E constant EM7180_SAVED_PARAM_BYTE_3
$45 constant EM7180_ACTUAL_MAG_RATE
$46 constant EM7180_ACTUAL_ACCEL_RATE
$47 constant EM7180_ACTUAL_GYRO_RATE
$50 constant EM7180_ERROR_REGISTER
$54 constant EM7180_ALGORITHM_CONTROL
$55 constant EM7180_MAG_RATE
$56 constant EM7180_ACCEL_RATE
$57 constant EM7180_GYRO_RATE
$58 constant EM7180_BARO_RATE
$59 constant EM7180_TEMP_RATE
$60 constant EM7180_LOAD_PARAM_BYTE_0
$61 constant EM7180_LOAD_PARAM_BYTE_1
$62 constant EM7180_LOAD_PARAM_BYTE_2
$63 constant EM7180_LOAD_PARAM_BYTE_3
$64 constant EM7180_PARAM_REQUEST
$70 constant EM7180_ROM_VERSION_1
$71 constant EM7180_ROM_VERSION_2
$72 constant EM7180_RAM_VERSION_1
$73 constant EM7180_RAM_VERSION_2
$90 constant EM7180_PRODUCT_ID
$91 constant EM7180_REVISION_ID
$92 constant EM7180_RUN_STATUS
$94 constant EM7180_UPLOAD_ADDRESS
$96 constant EM7180_UPLOAD_DATA
$97 constant EM7180_CRC_HOST
$9B constant EM7180_RESET_REQUEST
$9E constant EM7180_PASS_THRU_STATUS
$A0 constant EM7180_PASS_THRU_CONTROL
$5B constant EM7180_ACC_LPF_BW		\ GP36
$5C constant EM7180_GYRO_LPF_BW		\ GP37
$5D constant EM7180_BARO_LPF_BW		\ GP38
$3F constant EM7180_GP8
$40 constant EM7180_GP9
$41 constant EM7180_GP10
$42 constant EM7180_GP11
$43 constant EM7180_GP12
$44 constant EM7180_GP13
$4B constant EM7180_GP20
$4C constant EM7180_GP21
$4D constant EM7180_GP22
$4E constant EM7180_GP23
$4F constant EM7180_GP24
$5B constant EM7180_GP36
$5C constant EM7180_GP37
$5D constant EM7180_GP38
$5E constant EM7180_GP39
$5F constant EM7180_GP40
$69 constant EM7180_GP50
$6A constant EM7180_GP51
$6B constant EM7180_GP52
$6C constant EM7180_GP53
$6D constant EM7180_GP54
$6E constant EM7180_GP55
$6F constant EM7180_GP56

$1  constant SENTRAL_STATUS_EEPROM_DETECTED
$2  constant SENTRAL_STATUS_EE_UPLOAD_DONE
$4  constant SENTRAL_STATUS_EE_UPLOAD_ERROR
$8  constant SENTRAL_STATUS_IDLE
$10 constant SENTRAL_STATUS_NO_EEPROM

\ TODO: check all i2c1-* call returns instead of dropping
2 buffer: _BUF
: _write-reg-to-sentral ( reg -- ) _BUF c! _BUF EM7180_ADDRESS 1 i2c1-write drop ;
: read-bytes-from-sentral-reg-to-buf ( numbytes buf-addr reg -- )
  _write-reg-to-sentral
  EM7180_ADDRESS rot i2c1-read drop
;

: read-byte-from-sentral-reg ( reg -- byte )
  1 _BUF rot read-bytes-from-sentral-reg-to-buf
  _BUF c@
;

: write-byte-to-sentral-reg ( byte reg -- )
  _BUF c!
  _BUF #1 + c!
  _BUF EM7180_ADDRESS #2 i2c1-write
  drop
;

: _status@ ( -- status ) EM7180_SENTRAL_STATUS read-byte-from-sentral-reg ;
SENTRAL_STATUS_EEPROM_DETECTED SENTRAL_STATUS_EE_UPLOAD_DONE or SENTRAL_STATUS_IDLE or constant _READY
: _reset ( -- ) $1 EM7180_RESET_REQUEST write-byte-to-sentral-reg ;
: _to-string ( ud|d -- c-addr length ) 0 <# #s #> ;	\ buffer for pictured strings is reused so must use immediately
: _wait-ready ( -- )
  _status@
  dup s" init-sentral: status " log.debug _to-string log.debug-append
  _READY <> if
    begin
      s" init-sentral: not ready, resetting: " log.warning
      _reset
      #1000 delay
      _status@
      dup s" init-sentral: status " log.debug _to-string log.debug-append
      _READY =
    until
  then
;

: _enable-pass-thru-state ( -- ) $1 EM7180_PASS_THRU_CONTROL write-byte-to-sentral-reg ;
: _to-flag ( val -- flag ) if true exit then false ;
: _in-pass-thru-state? ( -- flag ) EM7180_PASS_THRU_STATUS read-byte-from-sentral-reg _to-flag ;
: _wait-until-in-pass-thru-state ( -- )
  _in-pass-thru-state? not if begin
    s" _wait-until-in-pass-thru-state: not in pass thru state, looping" log.warning
    #5 delay
    _in-pass-thru-state?
  until then
;
: _enter-pass-thru-state ( -- )
  _enable-pass-thru-state
  _wait-until-in-pass-thru-state
;

: _disable-pass-thru-state ( -- ) 0 EM7180_PASS_THRU_CONTROL write-byte-to-sentral-reg ;
: _wait-until-not-in-pass-thru-state ( -- )
  _in-pass-thru-state? if begin
    s" _wait-until-not-pass-thru-state: in pass thru state, looping" log.warning
    #5 delay
    _in-pass-thru-state?
  until then
;
: _exit-pass-thru-state ( -- )
  _disable-pass-thru-state
  5 delay
  _wait-until-not-in-pass-thru-state
;

$50 constant _M24512DFM_DATA_ADDRESS
0 variable _ACCEL_X_MAX
0 variable _ACCEL_X_MIN
0 variable _ACCEL_Y_MAX
0 variable _ACCEL_Y_MIN
0 variable _ACCEL_Z_MAX
0 variable _ACCEL_Z_MIN
12 buffer: _EEPROM_BUF
140 buffer: _WARM_START_DATA
: _signed-h@ ( buf-addr -- n) h@ dup %1000000000000000 and if $ffff0000 or then ;
$80 constant _EEPROM_CAL_MEM_ADDR_MSB
$8c constant _EEPROM_CAL_MEM_ADDR_LSB
$80 constant _EEPROM_WS_MEM_ADDR_MSB
$00 constant _EEPROM_WS_MEM_ADDR_LSB
$80 constant _EEPROM_WS_VALID_MEM_ADDR_MSB
$98 constant _EEPROM_WS_VALID_MEM_ADDR_LSB
: _read-accel-cal-from-eeprom ( -- )
  _EEPROM_CAL_MEM_ADDR_MSB _EEPROM_BUF c!
  _EEPROM_CAL_MEM_ADDR_LSB _EEPROM_BUF 1 + c!
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write drop
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 12 i2c1-read drop
;
: _get-accel-cal ( -- )
  _read-accel-cal-from-eeprom
  _EEPROM_BUF _signed-h@ _ACCEL_X_MAX !
  _EEPROM_BUF 2 + _signed-h@ _ACCEL_Y_MAX !
  _EEPROM_BUF 4 + _signed-h@ _ACCEL_Z_MAX !
  _EEPROM_BUF 6 + _signed-h@ _ACCEL_X_MIN !
  _EEPROM_BUF 8 + _signed-h@ _ACCEL_Y_MIN !
  _EEPROM_BUF 10 + _signed-h@ _ACCEL_Z_MIN !
;

: _min-accel-cal-valid? ( n -- flag )
  dup -2240 >= swap -1800 <= and
;
: _max-accel-cal-valid? ( n -- flag )
  dup 2240 <= swap 1800 >= and
;
: _cal-accel-valid? ( -- flag )
  _ACCEL_X_MIN @ _min-accel-cal-valid?
  _ACCEL_X_MAX @ _max-accel-cal-valid? and
  _ACCEL_Y_MIN @ _min-accel-cal-valid? and
  _ACCEL_Y_MAX @ _max-accel-cal-valid? and
  _ACCEL_Z_MIN @ _min-accel-cal-valid? and
  _ACCEL_Z_MAX @ _max-accel-cal-valid? and
;

: _get-warm-start-data ( -- )
  _EEPROM_WS_MEM_ADDR_MSB _EEPROM_BUF c!
  _EEPROM_WS_MEM_ADDR_LSB _EEPROM_BUF 1 + c!
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write drop
  _WARM_START_DATA _M24512DFM_DATA_ADDRESS 140 i2c1-read drop
;
$aa constant _WS_VALID_MAGIC_BYTE
: _ws-valid? ( -- flag )
  _EEPROM_WS_VALID_MEM_ADDR_MSB _EEPROM_BUF c!
  _EEPROM_WS_VALID_MEM_ADDR_LSB _EEPROM_BUF 1 + c!
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write drop
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 1 i2c1-read drop
  _EEPROM_BUF c@ _WS_VALID_MAGIC_BYTE =
;

: print-accel-cal ( -- )
  cr
  _ACCEL_X_MIN @ . ." x min" cr
  _ACCEL_X_MAX @ . ." x max" cr
  _ACCEL_Y_MIN @ . ." x min" cr
  _ACCEL_Y_MAX @ . ." x max" cr
  _ACCEL_Z_MIN @ . ." x min" cr
  _ACCEL_Z_MAX @ . ." x max" cr
;

: _set-idle-state ( -- ) 0 EM7180_HOST_CONTROL write-byte-to-sentral-reg ;
\ enable-run state forces initialization and reads Accel Cal data into static variables
: _enable-run-state ( -- ) $1 EM7180_HOST_CONTROL write-byte-to-sentral-reg ;

: _max-min-to-scale-cal ( max min -- cal ) - 4096000000 swap u/mod swap drop 1000000 - ;
: _max-min-to-offset-cal ( max min -- cal ) + 1000000 * 4096 u/mod swap drop ;
: _cal-to-cal-bytes ( cal -- byte0 byte1 ) dup $ff and swap $ff00 and 8 rshift ;
: _write-accel-scale-cal ( byte0reg byte1reg max min -- )
  _max-min-to-scale-cal
  _cal-to-cal-bytes ( byte0reg byte1reg byte0 byte1 )
  rot write-byte-to-sentral-reg
  swap write-byte-to-sentral-reg
;
: _write-accel-offset-cal ( byte0reg byte1reg max min -- )
  _max-min-to-offset-cal
  _cal-to-cal-bytes ( byte0reg byte1reg byte0 byte1 )
  rot write-byte-to-sentral-reg
  swap write-byte-to-sentral-reg
;
: _write-accel-cal-x ( -- )
  EM7180_GP36 EM7180_GP37 _ACCEL_X_MAX @ _ACCEL_X_MIN @ _write-accel-scale-cal
  EM7180_GP51 EM7180_GP52 _ACCEL_X_MAX @ _ACCEL_X_MIN @ _write-accel-offset-cal
;
: _write-accel-cal-y ( -- )
  EM7180_GP38 EM7180_GP39 _ACCEL_Y_MAX @ _ACCEL_Y_MIN @ _write-accel-scale-cal
  EM7180_GP53 EM7180_GP54 _ACCEL_Y_MAX @ _ACCEL_Y_MIN @ _write-accel-offset-cal
;
: _write-accel-cal-z ( -- )
  EM7180_GP40 EM7180_GP50 _ACCEL_Z_MAX @ _ACCEL_Z_MIN @ _write-accel-scale-cal
  EM7180_GP55 EM7180_GP56 _ACCEL_Z_MAX @ _ACCEL_Z_MIN @ _write-accel-offset-cal
;

: print-accel-cal-regs ( -- )
  cr
  EM7180_GP36 read-byte-from-sentral-reg hex. ." x scale0" cr
  EM7180_GP37 read-byte-from-sentral-reg hex. ." x scale1" cr
  EM7180_GP38 read-byte-from-sentral-reg hex. ." y scale0" cr
  EM7180_GP39 read-byte-from-sentral-reg hex. ." y scale1" cr
  EM7180_GP40 read-byte-from-sentral-reg hex. ." z scale0" cr
  EM7180_GP50 read-byte-from-sentral-reg hex. ." z scale1" cr
  EM7180_GP51 read-byte-from-sentral-reg hex. ." x offset0" cr
  EM7180_GP52 read-byte-from-sentral-reg hex. ." x offset1" cr
  EM7180_GP53 read-byte-from-sentral-reg hex. ." y offset0" cr
  EM7180_GP54 read-byte-from-sentral-reg hex. ." y offset1" cr
  EM7180_GP55 read-byte-from-sentral-reg hex. ." z offset0" cr
  EM7180_GP56 read-byte-from-sentral-reg hex. ." z offset1" cr
;

: _set-alg-ctl-param-xfer ( -- ) $80 EM7180_ALGORITHM_CONTROL write-byte-to-sentral-reg ;
: _set-alg-ctl-standby ( -- ) 0 EM7180_ALGORITHM_CONTROL write-byte-to-sentral-reg ;
: _param-req-ack? ( param-req -- flag ) EM7180_PARAM_ACKNOWLEDGE read-byte-from-sentral-reg = ;
: _paramnum-to-param-load-req ( paramnum -- param-req ) $80 or ;
: _wait-param-req-ack ( param-req -- ) begin dup _param-req-ack? until drop ;
: _load-param-bytes ( paramnum byte3 byte2 byte1 byte0 -- )
  EM7180_LOAD_PARAM_BYTE_0 write-byte-to-sentral-reg
  EM7180_LOAD_PARAM_BYTE_1 write-byte-to-sentral-reg
  EM7180_LOAD_PARAM_BYTE_2 write-byte-to-sentral-reg
  EM7180_LOAD_PARAM_BYTE_3 write-byte-to-sentral-reg
  _paramnum-to-param-load-req
  s" _load-param-bytes: show stack" log.debug
  h.s
  dup EM7180_PARAM_REQUEST write-byte-to-sentral-reg
  s" _load-param-bytes: sent request" log.debug
  h.s
  _wait-param-req-ack
  s" _load-param-bytes: done" log.debug
;
: _load-param-word-from-warm-start-data ( paramnum offset -- )
  _WARM_START_DATA + ( parmnum buf-addr )
  dup #3 + c@
  swap dup #2 + c@
  swap dup #1 + c@
  swap          c@ ( paramnum byte3 byte2 byte1 byte0 )
  s" _load-param-word-from-warm-start-data: show stack" log.debug
  h.s
  _load-param-bytes
  h.s
;
: _end-param-xfer ( -- ) 0 EM7180_PARAM_REQUEST write-byte-to-sentral-reg ;
: _load-warm-start ( -- )
  _set-alg-ctl-param-xfer \ TODO: this is supposed to be done only after the first param transfer request
  s" _load-warm-start: starting loop" log.debug
  36 0 do
    s" _load-warm-start: loop" log.debug
    i #1 + i #4 * _load-param-word-from-warm-start-data
  loop
  s" _load-warm-start: loop finished" log.debug
  _end-param-xfer
  s" _load-warm-start: setting alg ctl to standby" log.debug
  _set-alg-ctl-standby
  s" _load-warm-start: done" log.debug
;

$3 constant _MPU9250_GYRO_LPF_41HZ
$3 constant _MPU9250_ACC_LPF_41HZ
: _set-sensor-lpf-bandwidth ( -- )
  _MPU9250_ACC_LPF_41HZ EM7180_ACC_LPF_BW write-byte-to-sentral-reg
  _MPU9250_GYRO_LPF_41HZ EM7180_GYRO_LPF_BW write-byte-to-sentral-reg
;

$64 constant _ACC_ODR_1000HZ
$64 constant _GYRO_ODR_1000HZ
$64 constant _MAG_ODR_100HZ
$9 constant _Q_RATE_DIVISOR_10
$19 constant _BARO_ODR_25HZ
: _set-sensor-odr ( -- )
  _ACC_ODR_1000HZ EM7180_ACCEL_RATE write-byte-to-sentral-reg
  _GYRO_ODR_1000HZ EM7180_GYRO_RATE write-byte-to-sentral-reg
  _MAG_ODR_100HZ EM7180_MAG_RATE write-byte-to-sentral-reg
  _Q_RATE_DIVISOR_10 EM7180_Q_RATE_DIVISOR write-byte-to-sentral-reg
  \ ODR + 10000000b to activate the eventStatus bit for the barometer...
  _BARO_ODR_25HZ $80 or EM7180_BARO_RATE write-byte-to-sentral-reg
;

: _enable-int ( -- )
  \ gyros updated (0x20), Sentral error (0x02) or Sentral reset (0x01)
  $23 EM7180_ENABLE_EVENTS write-byte-to-sentral-reg
;

: init-sentral ( -- )
  init-i2c1
  _wait-ready
  _enter-pass-thru-state
  _get-accel-cal
  print-accel-cal
  _cal-accel-valid? not if s" init-sentral: accel calibration invalid; exiting" log.warning exit then
  _get-warm-start-data
  _ws-valid? not if s" init-sentral: warm start data invalid; exiting" log.warning exit then
  _exit-pass-thru-state
  _set-idle-state
  _write-accel-cal-x
  _write-accel-cal-y
  _write-accel-cal-z
  _enable-run-state
  print-accel-cal-regs
  _load-warm-start
  s" init-sentral: warm start data loaded" log.debug
  \ _set-sensor-lpf-bandwidth
  \ _set-sensor-odr
  \ _set-alg-ctl-standby
  \ _enable-int
  \ _set-alg-ctl-standby
;
: init-sentral-2 ( -- )
;

: get_quat ( -- )
  EM7180_QX
;

\  // Perform final Sentral alogorithm parameter modifications
\  EM7180::EM7180_set_integer_param (0x49, 0x00); // Disable "Stillness" mode
\  EM7180::EM7180_set_integer_param (0x48, 0x01); // Set Gbias_mode to 1
\  EM7180::EM7180_set_mag_acc_FS (MAG_SCALE, ACC_SCALE); // Set magnetometer/accelerometer full-scale ranges
\  EM7180::EM7180_set_gyro_FS (GYRO_SCALE); // Set gyroscope full-scale range
\  EM7180::EM7180_set_float_param (0x3B, 0.0f); // Param 59 Mag Transient Protect off (0.0)
\
\  // Choose interrupt events: Gyros updated (0x20), Sentral error (0x02) or Sentral reset (0x01)
\  I2C->writeByte(EM7180_ADDRESS, EM7180_EnableEvents, 0x23);
\
\  // Read event status register
\  eventStatus[SensorNum] = I2C->readByte(EM7180_ADDRESS, EM7180_EventStatus);
