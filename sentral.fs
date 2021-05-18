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

2 buffer: _BUF
: _write-reg-to-sentral ( reg -- ) _BUF c! _BUF EM7180_ADDRESS 1 i2c1-write drop ;
: _read-byte-from-sentral ( -- byte ) _BUF EM7180_ADDRESS 1 i2c1-read drop _BUF c@ ;
: read-byte-from-sentral-reg ( reg -- byte )
  _write-reg-to-sentral
  _read-byte-from-sentral
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
  _status@ dup _READY <> if begin
    s" init-sentral: not ready, resetting: " log.warning _to-string log.warning-append
    _reset
    #500 delay
    _status@ _READY <>
  until then drop
;

: _enable-pass-thru-state ( -- ) $1 EM7180_PASS_THRU_CONTROL write-byte-to-sentral-reg ;
: _to-flag ( val -- flag ) if true exit then false ;
: _in-pass-thru-state? ( -- flag ) EM7180_PASS_THRU_STATUS read-byte-from-sentral-reg _to-flag ;
: _wait-until-in-pass-thru-state ( -- )
  _in-pass-thru-state? not if begin
    s" _wait-pass-thru-state: not in pass thru state, looping" log.warning
    #5 delay
    _in-pass-thru-state?
  until then
;
: _enter-pass-thru-state ( -- )
  _enable-pass-thru-state
  _wait-until-in-pass-thru-state
;

\  // Sentral I2C EEPROM
\   #define M24512DFM_IDPAGE_ADDRESS              0x58
\   uint8_t data[12];
\   uint8_t axis;

\   I2C->M24512DFMreadBytes(M24512DFM_DATA_ADDRESS, 0x80, 0x8c, 12, data);                                                                         // Page 257
\   for (axis = 0; axis < 3; axis++)
\   {
\     global_conf.accZero_max[SensorNum][axis] = ((int16_t)(data[(2*axis + 1)]<<8) | data[2*axis]);
\     global_conf.accZero_min[SensorNum][axis] = ((int16_t)(data[(2*axis + 7)]<<8) | data[(2*axis + 6)]);
\   }



$50 constant _M24512DFM_DATA_ADDRESS
0 variable _ACCEL_X_MAX
0 variable _ACCEL_X_MIN
0 variable _ACCEL_Y_MAX
0 variable _ACCEL_Y_MIN
0 variable _ACCEL_Z_MAX
0 variable _ACCEL_Z_MIN
12 buffer: _EEPROM_BUF
140 buffer: _WARM_START_DATA
1 variable _WS_VALID
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
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 12 i2c1-read
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
: _get-warm-start-data ( -- )
  _EEPROM_WS_MEM_ADDR_MSB _EEPROM_BUF c!
  _EEPROM_WS_MEM_ADDR_LSB _EEPROM_BUF 1 + c!
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write
  _WARM_START_DATA _M24512DFM_DATA_ADDRESS 140 i2c1-read
;
: _get-warm-start-valid-byte ( -- )
  _EEPROM_WS_VALID_MEM_ADDR_MSB _EEPROM_BUF c!
  _EEPROM_WS_VALID_MEM_ADDR_LSB _EEPROM_BUF 1 + c!
  _EEPROM_BUF _M24512DFM_DATA_ADDRESS 2 i2c1-write
  _WS_VALID _M24512DFM_DATA_ADDRESS 1 i2c1-read
;

: init-sentral ( -- )
  init-i2c1
  _wait-ready
  _enter-pass-thru-state
  _get-accel-cal
  _get-warm-start-data
  _get-warm-start-valid-byte
;
