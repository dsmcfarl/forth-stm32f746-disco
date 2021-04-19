\ pin allocations
\ PA8 => TIM11_INPUT => MCO1 => PLL/5

#require common.fs
#require gpio.fs

\ configure PLL/5 clock output to pin PA8 via MCO1 (useful for testing clock setup)
: mco1-cfg ( -- )
  RCC_APB2ENR_TIM11EN bfs!		\ enable TIM11
  TIM11_OR_TI1_RMP bfs!			\ remap MCO1 to TIM11 input
  RCC_CFGR_MCO1 bfs!			\ send PLL clock to MCO1
  \ RCC_CFGR_MCO1 bfc!			\ send HSI clock to MCO1
  \ %10 RCC_CFGR_MCO1 bf!			\ send HSE clock to MCO1
  RCC_CFGR_MCO1PRE bfs!			\ divide MCO1 by 5
  enable-gpioa-clock
  %10 GPIOA_MODER_MODER8 bf!		\ set PA8 to alternate function mode
  GPIOA_GPIOB_OSPEEDR_OSPEEDR8 bfs!	\ set GPIOA8 to very high speed
;
