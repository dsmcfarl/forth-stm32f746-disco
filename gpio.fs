#require common.fs

%00 constant INPUT
%01 constant OUTPUT
%10 constant AF
%11 constant ANALOG

: enable-gpiok-clock ( -- )
  RCC_AHB1ENR_GPIOKEN bfs!
;

: enable-gpioa-clock ( -- )
  RCC_AHB1ENR_GPIOAEN bfs!
;
: set-gpiok3-output-mode
  %01 GPIOK_MODER_MODER3 bf!
;
