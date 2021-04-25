#require common.fs

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

$40020000 constant GPIOA
GPIOA $400 + constant GPIOB
GPIOB $400 + constant GPIOC
GPIOC $400 + constant GPIOD
GPIOD $400 + constant GPIOE
GPIOE $400 + constant GPIOF
GPIOF $400 + constant GPIOG
GPIOG $400 + constant GPIOH
GPIOH $400 + constant GPIOI
GPIOI $400 + constant GPIOJ
GPIOJ $400 + constant GPIOK
  
$0 constant MODER_OFFSET
$0 constant MODER_PIN0_OFFSET
#2 constant MODER_WIDTH
: moder! ( mode port pin -- )
  MODER_WIDTH * MODER_PIN0_OFFSET +
  MODER_WIDTH
  rot MODER_OFFSET +
  bf!
;

