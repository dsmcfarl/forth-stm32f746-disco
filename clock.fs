#require common.fs

#16000000 constant HSI_CLK_HZ		\ high speed internal clock freq
#25000000 constant HSE_CLK_HZ		\ high speed external clock freq
HSE_CLK_HZ #1000000 / constant PLLM_VAL	\ division factor for main PLLs input clock
#432 constant PLLN_VAL			\ main PLL multiplication factor for VCO
%00 constant PLLP2_VAL			\ main PLL division factor for main system clock

: hsi-on ( -- ) RCC_CR HSION bfs! ;
: wait-hsi-rdy ( -- ) begin RCC_CR HSIRDY bf@ until ;
: set-sysclk-src-to-hsi ( -- ) RCC_CFGR SW0 bfc! RCC_CFGR SW1 bfc! ;

: hse-off ( -- ) RCC_CR HSEON bfc! ;
: hse-on ( -- ) RCC_CR HSEON bfs! ;
: bypass-hse-osc ( -- ) RCC_CR HSEBYP bfs! ;

: pll-off ( -- ) RCC_CR PLLON bfc! ;
: pll-on ( -- ) RCC_CR PLLON bfs! ;
: set-pll-src-to-hse ( -- ) RCC_PLLCFGR PLLSRC bfs! ;
: pllm! ( %bbbbbb -- ) RCC_PLLCFGR PLLM bf! ;
: plln! ( %bbbbbbbbb -- ) RCC_PLLCFGR PLLN bf! ;
: pllp! ( %bb -- ) RCC_PLLCFGR PLLP bf! ;
: wait-hse-rdy ( -- ) begin RCC_CR HSERDY bf@ until ;
: cfg-pll ( -- )
  pll-off
  set-pll-src-to-hse
  PLLM_VAL pllm! PLLN_VAL plln! PLLP2_VAL pllp!
  wait-hse-rdy
  pll-on
; 

%11 constant SCALE_1
: reg-mode! ( %bb -- ) PWR_CR1 VOS bf! ;
: enable-pwr-clock ( -- ) RCC_APB1ENR PWREN bfs! ;
: enable-over-drive ( -- ) PWR_CR1 ODEN bfs! ;
: wait-over-drive-rdy ( -- ) begin PWR_CSR1 ODRDY bf@ until ;
: enable-over-drive-switching ( -- ) PWR_CR1 ODSWEN bfs! ;
: wait-over-drive-switching-ready ( -- ) begin PWR_CSR1 ODSWRDY bf@ until ;
: cfg-reg ( -- )			\ For 216MHz need scale mode 1 and over-drive enabled (ref: RM0385 Rev 8 p78).
  SCALE_1 reg-mode!
  enable-pwr-clock
  enable-over-drive
  wait-over-drive-rdy
  enable-over-drive-switching
  wait-over-drive-switching-ready
;

: enable-flash-prefetch ( -- ) FLASH_ACR PRFTEN bfs! ;
: flash-latency-wait-states! ( u -- ) FLASH_ACR LATENCY bf! ;
: disable-art-cache ( -- ) FLASH_ACR ARTEN bfc! ;
: enable-art-cache  ( -- ) FLASH_ACR ARTEN bfs! ;
: reset-art-cache  ( -- ) FLASH_ACR ARTRST bfs! ;
: unreset-art-cache  ( -- ) FLASH_ACR ARTRST bfc! ;
: cfg-flash ( -- )
  enable-flash-prefetch
  #6 flash-latency-wait-states!
  disable-art-cache
  reset-art-cache
  unreset-art-cache
  enable-art-cache
;

%0000 constant HPRE/1			\ sysclk divided by 1
%101 constant PPRE1/4			\ sysclk divided by 4
%100 constant PPRE2/2			\ sysclk divided by 2
: ahb-pre-scalar! ( %bbbb -- ) RCC_CFGR HPRE bf! ;
: apb1-low-speed-pre-scalar! ( %bbb -- ) RCC_CFGR PPRE1 bf! ;
: apb2-high-speed-pre-scalar! ( %bbb -- ) RCC_CFGR PPRE2 bf! ;
: cfg-bus-pre-scalars ( -- )
  HPRE/1 ahb-pre-scalar!
  PPRE1/4 apb1-low-speed-pre-scalar!
  PPRE2/2 apb2-high-speed-pre-scalar!
;

: wait-pll-rdy ( -- ) begin RCC_CR PLLRDY bf@ until ;
: set-sysclk-src-to-pll ( -- ) RCC_CFGR SW0 bfc! RCC_CFGR SW1 bfs! ;

%10 constant HSI
: usart1-clk-src! ( %bb -- ) RCC_DKCFGR2 USART1SEL bf! ;
: baud-rate-for-16x-oversampling ( -- u ) #115200 HSI_CLK_HZ over 2/ + swap / ;
: cfg-usart1 ( -- )
  HSI usart1-clk-src!
  baud-rate-for-16x-oversampling USART1_BRR !
;

#192 constant PLLSAIN_VAL
#5 constant PLLSAIR_VAL
%01 constant PLLSAIDIVR\4_VAL		\ divide by 4
: enable-lcd-tft-controller-clock ( -- ) RCC_APB2ENR LTDCEN bfs! ;
: pllsai-off ( -- ) RCC_CR PLLSAION bfc! ;
: pllsai-on ( -- ) RCC_CR PLLSAION bfs! ;
: wait-pllsai-rdy ( -- ) begin RCC_CR PLLSAIRDY bf@ until ;
: pllsain! ( %bbbbbbbbb -- ) RCC_PLLSAICFGR PLLSAIN bf! ;
: pllsair! ( %bbb -- ) RCC_PLLSAICFGR PLLSAIR bf! ;
: pllsaidivr! ( %bb -- ) RCC_DKCFGR1 PLLSAIDIVR bf! ;
: cfg-pllsai ( -- )
  enable-lcd-tft-controller-clock
  pllsai-off
  \ 192 / 5 / 4 = 9.6 MHz
  PLLSAIN_VAL pllsain!
  PLLSAIR_VAL pllsair!
  PLLSAIDIVR\4_VAL pllsaidivr!
  pllsai-on
  wait-pllsai-rdy
;  

: cfg-sysclk  ( -- )
  hsi-on
  wait-hsi-rdy
  set-sysclk-src-to-hsi
  hse-off
  bypass-hse-osc
  hse-on
  cfg-pll
  cfg-reg
  cfg-flash
  cfg-bus-pre-scalars
  wait-pll-rdy
  set-sysclk-src-to-pll
  cfg-usart1
  cfg-pllsai
;
