#require common.fs

#16000000 constant HSI_CLK_HZ		\ high speed internal clock freq
#25000000 constant HSE_CLK_HZ		\ high speed external clock freq
HSE_CLK_HZ #1000000 / constant PLLM_VAL	\ division factor for main PLLs input clock
#432 constant PLLN_VAL			\ main PLL multiplication factor for VCO
%00 constant PLLP2_VAL			\ main PLL division factor for main system clock

: hsi-on ( -- ) RCC CR_HSION bfs! ;
: wait-hsi-rdy ( -- ) begin RCC CR_HSIRDY bf@ until ;
: set-sysclk-src-to-hsi ( -- ) RCC CFGR_SW0 bfc! RCC CFGR_SW1 bfc! ;

: hse-off ( -- ) RCC CR_HSEON bfc! ;
: hse-on ( -- ) RCC CR_HSEON bfs! ;
: bypass-hse-osc ( -- ) RCC CR_HSEBYP bfs! ;

: pll-off ( -- ) RCC CR_PLLON bfc! ;
: pll-on ( -- ) RCC CR_PLLON bfs! ;
: set-pll-src-to-hse ( -- ) RCC PLLCFGR_PLLSRC bfs! ;
: pllm! ( %bbbbbb -- ) RCC PLLCFGR_PLLM bf! ;
: plln! ( %bbbbbbbbb -- ) RCC PLLCFGR_PLLN bf! ;
: pllp! ( %bb -- ) RCC PLLCFGR_PLLP bf! ;
: wait-hse-rdy ( -- ) begin RCC CR_HSERDY bf@ until ;
: cfg-pll ( -- )
  pll-off
  set-pll-src-to-hse
  PLLM_VAL pllm! PLLN_VAL plln! PLLP2_VAL pllp!
  wait-hse-rdy
  pll-on
; 

%11 constant SCALE_1
: reg-mode! ( %bb -- ) PWR CR1_VOS bf! ;
: enable-pwr-clock ( -- ) RCC APB1ENR_PWREN bfs! ;
: enable-over-drive ( -- ) PWR CR1_ODEN bfs! ;
: wait-over-drive-rdy ( -- ) begin PWR CSR1_ODRDY bf@ until ;
: enable-over-drive-switching ( -- ) PWR CR1_ODSWEN bfs! ;
: wait-over-drive-switching-ready ( -- ) begin PWR CSR1_ODSWRDY bf@ until ;
: cfg-reg ( -- )			\ For 216MHz need scale mode 1 and over-drive enabled (ref: RM0385 Rev 8 p78).
  SCALE_1 reg-mode!
  enable-pwr-clock
  enable-over-drive
  wait-over-drive-rdy
  enable-over-drive-switching
  wait-over-drive-switching-ready
;

: enable-flash-prefetch ( -- ) FLASH ACR_PRFTEN bfs! ;
: flash-latency-wait-states! ( u -- ) FLASH ACR_LATENCY bf! ;
: disable-art-cache ( -- ) FLASH ACR_ARTEN bfc! ;
: enable-art-cache  ( -- ) FLASH ACR_ARTEN bfs! ;
: reset-art-cache  ( -- ) FLASH ACR_ARTRST bfs! ;
: unreset-art-cache  ( -- ) FLASH ACR_ARTRST bfc! ;
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
: ahb-pre-scalar! ( %bbbb -- ) RCC CFGR_HPRE bf! ;
: apb1-low-speed-pre-scalar! ( %bbb -- ) RCC CFGR_PPRE1 bf! ;
: apb2-high-speed-pre-scalar! ( %bbb -- ) RCC CFGR_PPRE2 bf! ;
: cfg-bus-pre-scalars ( -- )
  HPRE/1 ahb-pre-scalar!
  PPRE1/4 apb1-low-speed-pre-scalar!
  PPRE2/2 apb2-high-speed-pre-scalar!
;

: wait-pll-rdy ( -- ) begin RCC CR_PLLRDY bf@ until ;
: set-sysclk-src-to-pll ( -- ) RCC CFGR_SW0 bfc! RCC CFGR_SW1 bfs! ;

%10 constant HSI
: usart1-clk-src! ( %bb -- ) RCC DKCFGR2_USART1SEL bf! ;
: baud-rate-for-16x-oversampling ( -- u ) #115200 HSI_CLK_HZ over 2/ + swap / ;
: cfg-usart1 ( -- )
  HSI usart1-clk-src!
  baud-rate-for-16x-oversampling USART1_BRR !
;

#192 constant PLLSAIN_VAL
#5 constant PLLSAIR_VAL
%01 constant PLLSAIDIVR\4_VAL		\ divide by 4
: enable-lcd-tft-controller-clock ( -- ) RCC APB2ENR_LTDCEN bfs! ;
: pllsai-off ( -- ) RCC CR_PLLSAION bfc! ;
: pllsai-on ( -- ) RCC CR_PLLSAION bfs! ;
: wait-pllsai-rdy ( -- ) begin RCC CR_PLLSAIRDY bf@ until ;
: pllsain! ( %bbbbbbbbb -- ) RCC PLLSAICFGR_PLLSAIN bf! ;
: pllsair! ( %bbb -- ) RCC PLLSAICFGR_PLLSAIR bf! ;
: pllsaidivr! ( %bb -- ) RCC DKCFGR1_PLLSAIDIVR bf! ;
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
