: init-ram ( -- )
  init-i2c1
  ." i2c initialized" cr
  init-sentral
  ." sentral initialized" cr
;
init-ram
