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
%0 constant PUSH_PULL
%1 constant OPEN_DRAIN

MODER_MODER0
  constant MODER_WIDTH
  constant MODER_PIN0_OFFSET
  constant MODER_OFFSET
: moder! ( mode port pin -- )
  MODER_WIDTH *
  MODER_PIN0_OFFSET +
  MODER_WIDTH
  MODER_OFFSET -rot
  bf!
;
OSPEEDR_OSPEEDR0
  constant OSPEEDR_WIDTH
  constant OSPEEDR_PIN0_OFFSET
  constant OSPEEDR_OFFSET
: ospeedr! ( speed port pin -- )
  OSPEEDR_WIDTH *
  OSPEEDR_PIN0_OFFSET +
  OSPEEDR_WIDTH
  OSPEEDR_OFFSET -rot
  bf!
;
OTYPER_OT0
  constant OTYPER_WIDTH
  constant OTYPER_PIN0_OFFSET
  constant OTYPER_OFFSET
: otyper! ( speed port pin -- )
  OTYPER_WIDTH *
  OTYPER_PIN0_OFFSET +
  OTYPER_WIDTH
  OTYPER_OFFSET -rot
  bf!
;
AFRL_AFRL0
  constant AFRL_WIDTH
  constant AFRL_PIN0_OFFSET
  constant AFRL_OFFSET
: afrl! ( af port pin -- )
  AFRL_WIDTH *
  AFRL_PIN0_OFFSET +
  AFRL_WIDTH
  AFRL_OFFSET -rot
  bf!
;
AFRH_AFRH8
  constant AFRH_WIDTH
  constant AFRH_PIN8_OFFSET
  constant AFRH_OFFSET
: afrh! ( af port pin -- )
  #8 -
  AFRH_WIDTH *
  AFRH_PIN8_OFFSET +
  AFRH_WIDTH
  AFRH_OFFSET -rot
  bf!
;
: afr! ( af port pin -- )
  dup 8 < if afrl! else afrh! then
;
: bs! ( port pin -- )
  >R BSRR_BS0 swap R> + swap bfs!
;
: br! ( port pin -- )
  >R BSRR_BR0 swap R> + swap bfs!
;
