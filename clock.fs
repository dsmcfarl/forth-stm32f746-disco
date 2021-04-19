#require common.fs

#16000000 constant HSI_CLK_HZ		\ high speed internal clock freq
#25000000 constant HSE_CLK_HZ		\ high speed external clock freq
HSE_CLK_HZ #1000000 / constant PLLM	\ division factor for main PLLs input clock
#432 constant PLLN			\ main PLL multiplication factor for VCO
%00 constant PLLP2			\ main PLL division factor for main system clock

: hsi-on ( -- ) RCC_CR_HSION bfs! ;
: hsi-wait-rdy ( -- ) begin RCC_CR_HSIRDY bf@ until ;
: sys-clk-hsi-switch ( -- ) RCC_CFGR_SW0 bfc! RCC_CFGR_SW1 bfc! ;
: sys-clk-hsi-set ( -- )
  hsi-on
  hsi-wait-rdy
  sys-clk-hsi-switch
;

: hse-off ( -- ) RCC_CR_HSEON bfc! ;
: hse-on ( -- ) RCC_CR_HSEON bfs! ;
: hse-byp-on ( -- ) RCC_CR_HSEBYP bfs! ;
: hse-ext-osc-set ( -- )
  hse-off
  hse-byp-on
  hse-on
;

: pll-off ( -- ) RCC_CR_PLLON bfc! ;
: pll-on ( -- ) RCC_CR_PLLON bfs! ;
: pll-src-hse-set ( -- ) RCC_PLLCFGR_PLLSRC bfs! ;
: pllm! ( %bbbbbb -- ) RCC_PLLCFGR_PLLM bf! ;
: plln! ( %bbbbbbbbb -- ) RCC_PLLCFGR_PLLN bf! ;
: pllp! ( %bb -- ) RCC_PLLCFGR_PLLP bf! ;
: hse-wait-rdy ( -- ) begin RCC_CR_HSERDY bf@ until ;
: pll-cfg ( -- )
  pll-off
  pll-src-hse-set
  PLLM pllm! PLLN plln! PLLP2 pllp!
  hse-wait-rdy
  pll-on
; 

: regulator-scale-mode-1-set ( -- ) %11 PWR_CR1_VOS bf! ;
: pwr-clock-enable ( -- ) RCC_APB1ENR_PWREN bfs! ;
: over-drive-enable ( -- ) PWR_CR1_ODEN bfs! ;
: over-drive-wait-rdy ( -- ) begin PWR_CSR1_ODRDY bf@ until ;
: over-drive-switching-enable ( -- ) PWR_CR1_ODSWEN bfs! ;
: over-drive-switching-wait-ready ( -- ) begin PWR_CSR1_ODSWRDY bf@ until ;
: regulator-cfg ( -- )			\ For 216MHz need scale mode 1 and over-drive enabled (ref: RM0385 Rev 8 p78).
  regulator-scale-mode-1-set
  pwr-clock-enable
  over-drive-enable
  over-drive-wait-rdy
  over-drive-switching-enable
  over-drive-switching-wait-ready
;

: flash-prefetch-enable ( -- ) FLASH_ACR_PRFTEN bfs! ;
: flash-latency-wait-states! ( u -- ) FLASH_ACR_LATENCY bf! ;
: art-cache-disable ( -- ) FLASH_ACR_ARTEN bfc! ;
: art-cache-enable ( -- ) FLASH_ACR_ARTEN bfs! ;
: art-cache-reset ( -- ) FLASH_ACR_ARTRST bfs! ;
: art-cache-unreset ( -- ) FLASH_ACR_ARTRST bfc! ;
: flash-cfg ( -- )
  flash-prefetch-enable
  #6 flash-latency-wait-states!
  art-cache-disable
  art-cache-reset
  art-cache-unreset
  art-cache-enable
;

\ cfgure bus pre-scalars
: bus-cfg ( -- )
  RCC_CFGR_HPRE bfc!			\ set AHB clock to system clock
  $5 RCC_CFGR_PPRE1 bf!		\ set APB1 low speed clock to system clock/4
  $4 RCC_CFGR_PPRE2 bf!		\ set APB2 high speed clock to system clock/2
;

\ cfgure system clock to use PLL clock
: sys-clk-pll ( -- )
  begin RCC_CR_PLLRDY bf@ until		\ wait until PLL is ready
  RCC_CFGR_SW0 bfc!			\ switch to PLL clock
  RCC_CFGR_SW1 bfs!
;

: usart1-cfg ( -- )
  #2 RCC_DKCFGR2_USART1SEL bf!
  #115200 HSI_CLK_HZ over 2/ + swap /	\ calculate baudrate for 16 times oversampling
  USART1_BRR !
;

\ cfgure PLLSAI (used by LCD-TFT controller) (assumes PLLM set to 1MHz)
: pllsai-cfg ( -- )
  RCC_APB2ENR_LTDCEN bfs!		\ enable the LCD-TFT controller clock
  RCC_CR_PLLSAION bfc!			\ turn off PLLSAI
  #192 RCC_PLLSAICFGR_PLLSAIN bf! \ PLLSAI mult factor to set PLLSAIN to 192 MHz
  #5 RCC_PLLSAICFGR_PLLSAIR bf!	\ PLLSAI div factor to set PLLSAIR to 38.4 MHz 
  %01 RCC_DKCFGR1_PLLSAIDIVR bf!	\ div factor to set LTDC clock to 9.6 MHz
  RCC_CR_PLLSAION bfs!			\ turn on PLLSAI
  begin RCC_CR_PLLSAIRDY bf@ until		\ wait for PLLSAI to be ready
;  

\ setup system clock
: sys-clk-cfg  ( -- )
  sys-clk-hsi-set
  hse-ext-osc-set 
  pll-cfg
  regulator-cfg
  flash-cfg
  bus-cfg
  sys-clk-pll
  usart1-cfg
  pllsai-cfg
;
