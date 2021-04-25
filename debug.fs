\ pin allocations
\ PA8 => TIM11_INPUT => MCO1 => PLL/5

#require common.fs
#require gpio.fs

\ configure PLL/5 clock output to pin PA8 via MCO1 (useful for testing clock setup)
: mco1-cfg ( -- )
  RCC APB2ENR_TIM11EN bfs!		\ enable TIM11
  TIM11 OR_TI1_RMP bfs!			\ remap MCO1 to TIM11 input
  RCC CFGR_MCO1 bfs!			\ send PLL clock to MCO1
  \ RCC CFGR_MCO1 bfc!			\ send HSI clock to MCO1
  \ %10 RCC CFGR_MCO1 bf!		\ send HSE clock to MCO1
  RCC CFGR_MCO1PRE bfs!			\ divide MCO1 by 5
  RCC AHB1ENR_GPIOAEN bfs!
  AF GPIOK MODER_MODER3 bf!
  GPIOA OSPEEDR_OSPEEDR8 bfs!		\ set GPIOA8 to very high speed
;
