\ Microcontroller Clock Output (MCO)

\ pin allocations
\ PA8 => TIM11_INPUT => MCO1 => PLL/5

\ #require common.fs
\ #require gpio.fs

: _init-tim11 ( -- )
  RCC APB2ENR_TIM11EN bfs!		\ enable TIM11
  TIM11 OR_TI1_RMP bfs!			\ remap MCO1 to TIM11 input
;
: _init-pa8 ( -- )
  enable-gpioa-clock
  AF GPIOA #8 moder!
  VERY_HIGH GPIOA #8 ospeedr!
;
\ configure clock output to pin PA8 via MCO1. sends HSI by default. (useful for testing clock setup)
: init-mco1 ( -- )
  _init-tim11
  _init-pa8
;

%00 constant _HSI
%10 constant _HSE
%11 constant _PLL
%000 constant _NO_PRE
%111 constant _OVER_5
: _send-to-mco1 ( clock pre -- ) swap RCC CFGR_MCO1 bf! RCC CFGR_MCO1PRE bf! ;
: send-hsi-to-mco1 ( -- )
  _HSI _NO_PRE _send-to-mco1
;

: send-pll/5-to-mco1 ( -- )
  _PLL _OVER_5 _send-to-mco1
;

: send-hse-to-mco1 ( -- )
  _HSE _NO_PRE _send-to-mco1
;

: init ( -- )
  init
  init-mco1
  send-pll/5-to-mco1
  ." mco initialized" cr
;
