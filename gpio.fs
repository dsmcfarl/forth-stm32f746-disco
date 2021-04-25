#require common.fs

\ mecrisp-stellaris automatically enables all gpio ports

%00 constant INPUT
%01 constant OUTPUT
%10 constant AF
%11 constant ANALOG

#0 constant AF0
#1 constant AF1
#2 constant AF2
#3 constant AF3
#4 constant AF4
#5 constant AF5
#6 constant AF6
#7 constant AF7
#8 constant AF8
#9 constant AF9
#10 constant AF10
#11 constant AF11
#12 constant AF12
#13 constant AF13
#14 constant AF14
#15 constant AF15
%00 constant LOW
%01 constant MED
%10 constant HIGH
%11 constant VERY_HIGH

MODER_MODER0
  constant MODER_WIDTH
  constant MODER_PIN0_OFFSET
  constant MODER_OFFSET
: moder! ( mode port pin -- )
  MODER_WIDTH * MODER_PIN0_OFFSET +
  MODER_WIDTH
  rot MODER_OFFSET +
  bf!
;
