compiletoflash

#require clock.fs
init-sysclk

#require debug.fs
init-mco1

#require systick.fs
init-systick

compiletoram

#require graphics.fs
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
