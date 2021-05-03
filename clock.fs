#require clock-sysclk.fs

: init-sysclk  ( -- )
  hsi-on
  wait-hsi-rdy
  set-sysclk-src-to-hsi
  hse-off
  bypass-hse-osc
  hse-on
  cfg-pll
  cfg-reg
  cfg-flash
  cfg-bus-pre-scalars
  wait-pll-rdy
  set-sysclk-src-to-pll
  cfg-usart1
;
