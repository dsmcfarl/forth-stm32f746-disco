compiletoflash
#require common.fs
#require clock.fs
#require systick.fs
#require debug.fs
cfg-sysclk
mco1-cfg
systick-cfg
' systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq
systick-interrupt-enable
compiletoram
#require graphics.fs
init-graphics
clear
: hello-world ( -- )
  s" hello world!" 10 40 drawstring
  s" ÄÖÜß" 10 60 drawstring
;

: demo-mecrisp ( -- )
  180 color!
  50 14 32 12 ellipse
  50 14 34 14 ellipse
  232 color!
  font-8x16 font!
  s" Mecrisp" 22 7 drawstring
  2 4 12 24 line
  4 4 14 24 line
  150 color!
  font-4x6 font!
  s" 123456789012345678901234567890123456789012345678901234567890" 0 20 drawstring
;
demo-mecrisp
