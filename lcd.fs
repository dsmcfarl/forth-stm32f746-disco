#require gpio.fs

\ RK043FN48H-CT672B datasheet defines back porch differently than ST. ST back
\ porch does not include HSYNC or VSYNC. All timings in pixel clock cycles.
#480	constant RK043FN48H_WIDTH	\ width of active area
#272	constant RK043FN48H_HEIGHT	\ height of active area
#10	constant RK043FN48H_HSYNC	\ horiz sync
#33	constant RK043FN48H_HBP		\ horiz back porch
#8	constant RK043FN48H_HFP		\ horiz front porch
#10	constant RK043FN48H_VSYNC	\ vert sync
#2	constant RK043FN48H_VBP		\ vert back porch
#4	constant RK043FN48H_VFP		\ vert front porch

: enable-lcd-controller-clock ( -- ) RCC APB2ENR_LTDCEN bfs! ;

#192	constant PLLSAIN_VAL
#5	constant PLLSAIR_VAL
%01	constant PLLSAIDIVR\4_VAL	\ divide by 4
: pllsai-off ( -- ) RCC CR_PLLSAION bfc! ;
: pllsai-on ( -- ) RCC CR_PLLSAION bfs! ;
: wait-pllsai-rdy ( -- ) begin RCC CR_PLLSAIRDY bf@ until ;
: pllsain! ( %bbbbbbbbb -- ) RCC PLLSAICFGR_PLLSAIN bf! ;
: pllsair! ( %bbb -- ) RCC PLLSAICFGR_PLLSAIR bf! ;
: pllsaidivr! ( %bb -- ) RCC DKCFGR1_PLLSAIDIVR bf! ;
: cfg-pixel-clock-9.6mhz ( -- )		\ pll-input frequency must be 1 MHz
  pllsai-off
  \ 192 / 5 / 4 = 9.6 MHz
  PLLSAIN_VAL pllsain!
  PLLSAIR_VAL pllsair!
  PLLSAIDIVR\4_VAL pllsaidivr!
  pllsai-on
  wait-pllsai-rdy
;  

: cfg-lcd-timings ( -- )
  RK043FN48H_HSYNC 1- LTDC SSCR_HSW bf!						\ horiz sync width
  RK043FN48H_VSYNC 1- LTDC SSCR_VSH bf!						\ vert sync width
  RK043FN48H_HSYNC RK043FN48H_HBP + 1- LTDC BPCR_AHBP bf!			\ accumulated horiz back porch
  RK043FN48H_VSYNC RK043FN48H_VBP + 1- LTDC BPCR_AVBP bf!			\ accumulated vert back porch
  RK043FN48H_WIDTH  RK043FN48H_HSYNC + RK043FN48H_HBP + 1- LTDC AWCR_AAW bf!	\ acumulated active width
  RK043FN48H_HEIGHT RK043FN48H_VSYNC + RK043FN48H_VBP + 1- LTDC AWCR_AAH bf!	\ acumulated active height
  RK043FN48H_WIDTH RK043FN48H_HSYNC +
  RK043FN48H_HBP + RK043FN48H_HFP + 1- LTDC TWCR_TOTALW bf!			\ total width
  RK043FN48H_HEIGHT RK043FN48H_VSYNC +
  RK043FN48H_VBP + RK043FN48H_VFP + 1- LTDC TWCR_TOTALH bf!			\ total height
;

: lcd-background-color! ( c -- ) LTDC BCCR_BC bf! ;

: cfg-lcd-gpio ( -- )
  AF GPIOI MODER_MODER15 bf! HIGH GPIOI OSPEEDR_OSPEEDR15 bf! AF14 GPIOI AFRH_AFRH15 bf!	\ LCD_R0
  AF GPIOJ MODER_MODER0 bf! HIGH GPIOJ OSPEEDR_OSPEEDR0 bf! AF14 GPIOJ AFRL_AFRL0 bf!		\ LCD_R1
  AF GPIOJ MODER_MODER1 bf! HIGH GPIOJ OSPEEDR_OSPEEDR1 bf! AF14 GPIOJ AFRL_AFRL1 bf!		\ LCD_R2
  AF GPIOJ MODER_MODER2 bf! HIGH GPIOJ OSPEEDR_OSPEEDR2 bf! AF14 GPIOJ AFRL_AFRL2 bf!		\ LCD_R3
  AF GPIOJ MODER_MODER3 bf! HIGH GPIOJ OSPEEDR_OSPEEDR3 bf! AF14 GPIOJ AFRL_AFRL3 bf!		\ LCD_R4
  AF GPIOJ MODER_MODER4 bf! HIGH GPIOJ OSPEEDR_OSPEEDR4 bf! AF14 GPIOJ AFRL_AFRL4 bf!		\ LCD_R5
  AF GPIOJ MODER_MODER5 bf! HIGH GPIOJ OSPEEDR_OSPEEDR5 bf! AF14 GPIOJ AFRL_AFRL5 bf!		\ LCD_R6
  AF GPIOJ MODER_MODER6 bf! HIGH GPIOJ OSPEEDR_OSPEEDR6 bf! AF14 GPIOJ AFRL_AFRL6 bf!		\ LCD_R7
  AF GPIOJ MODER_MODER7 bf! HIGH GPIOJ OSPEEDR_OSPEEDR7 bf! AF14 GPIOJ AFRL_AFRL7 bf!		\ LCD_G0
  AF GPIOJ MODER_MODER8 bf! HIGH GPIOJ OSPEEDR_OSPEEDR8 bf! AF14 GPIOJ AFRH_AFRH8 bf!		\ LCD_G1
  AF GPIOJ MODER_MODER9 bf! HIGH GPIOJ OSPEEDR_OSPEEDR9 bf! AF14 GPIOJ AFRH_AFRH9 bf!		\ LCD_G2
  AF GPIOJ MODER_MODER10 bf! HIGH GPIOJ OSPEEDR_OSPEEDR10 bf! AF14 GPIOJ AFRH_AFRH10 bf!	\ LCD_G3
  AF GPIOJ MODER_MODER11 bf! HIGH GPIOJ OSPEEDR_OSPEEDR11 bf! AF14 GPIOJ AFRH_AFRH11 bf!	\ LCD_G4
  AF GPIOK MODER_MODER0 bf! HIGH GPIOK OSPEEDR_OSPEEDR0 bf! AF14 GPIOK AFRL_AFRL0 bf!		\ LCD_G5
  AF GPIOK MODER_MODER1 bf! HIGH GPIOK OSPEEDR_OSPEEDR1 bf! AF14 GPIOK AFRL_AFRL1 bf!		\ LCD_G6
  AF GPIOK MODER_MODER2 bf! HIGH GPIOK OSPEEDR_OSPEEDR2 bf! AF14 GPIOK AFRL_AFRL2 bf!		\ LCD_G7
  AF GPIOE MODER_MODER4 bf! HIGH GPIOE OSPEEDR_OSPEEDR4 bf! AF14 GPIOE AFRL_AFRL4 bf!		\ LCD_B0
  AF GPIOJ MODER_MODER13 bf! HIGH GPIOJ OSPEEDR_OSPEEDR13 bf! AF14 GPIOJ AFRH_AFRH13 bf!	\ LCD_B1
  AF GPIOJ MODER_MODER14 bf! HIGH GPIOJ OSPEEDR_OSPEEDR14 bf! AF14 GPIOJ AFRH_AFRH14 bf!	\ LCD_B2
  AF GPIOJ MODER_MODER15 bf! HIGH GPIOJ OSPEEDR_OSPEEDR15 bf! AF14 GPIOJ AFRH_AFRH15 bf!	\ LCD_B3
  AF GPIOG MODER_MODER12 bf! HIGH GPIOG OSPEEDR_OSPEEDR12 bf! AF14 GPIOG AFRH_AFRH12 bf!	\ LCD_B4
  AF GPIOK MODER_MODER4 bf! HIGH GPIOK OSPEEDR_OSPEEDR4 bf! AF14 GPIOK AFRL_AFRL4 bf!		\ LCD_B5
  AF GPIOK MODER_MODER5 bf! HIGH GPIOK OSPEEDR_OSPEEDR5 bf! AF14 GPIOK AFRL_AFRL5 bf!		\ LCD_B6
  AF GPIOK MODER_MODER6 bf! HIGH GPIOK OSPEEDR_OSPEEDR6 bf! AF14 GPIOK AFRL_AFRL6 bf!		\ LCD_B7
  AF GPIOK MODER_MODER7 bf! HIGH GPIOK OSPEEDR_OSPEEDR7 bf! AF14 GPIOK AFRL_AFRL7 bf!		\ LCD_DE
  AF GPIOI MODER_MODER14 bf! HIGH GPIOI OSPEEDR_OSPEEDR14 bf! AF14 GPIOI AFRH_AFRH14 bf!	\ LCD_CLK
  AF GPIOI MODER_MODER10 bf! HIGH GPIOI OSPEEDR_OSPEEDR10 bf! AF14 GPIOI AFRH_AFRH10 bf!	\ LCD_HSYNC
  AF GPIOI MODER_MODER9 bf! HIGH GPIOI OSPEEDR_OSPEEDR9 bf! AF14 GPIOI AFRH_AFRH9 bf!		\ LCD_VSYNC
  OUTPUT GPIOI MODER_MODER12 bf!								\ LCD_DISP
  OUTPUT GPIOK MODER_MODER3 bf!									\ LCD_BL
;

: lcd-backlight-on  ( -- ) GPIOK BSRR_BS3 bfs! ;

: lcd-disp-on  ( -- ) GPIOI BSRR_BS12 bfs! ;

RK043FN48H_HSYNC RK043FN48H_HBP +       constant L1_H_START
L1_H_START       RK043FN48H_WIDTH + 1-  constant L1_H_END
RK043FN48H_VSYNC RK043FN48H_VBP +       constant L1_V_START
L1_V_START       RK043FN48H_HEIGHT + 1- constant L1_V_END
$000000					constant BLACK
%101					constant L8_FMT     \ 8 bit per pixel frame buffer format
RK043FN48H_WIDTH RK043FN48H_HEIGHT * dup BUFFER: LCD_FB1_BUFFER constant LCD_FB1_SIZE#
LCD_FB1_BUFFER				variable LCD_FB1              \ frame buffer 1 pointer
LCD_FB1_SIZE#				variable LCD_FB1_SIZE         \ frame buffer 1 size
: lcd-layer1-h-pos! ( start end -- ) #16 lshift or LTDC_L1WHPCR ! ;
: lcd-layer1-v-pos! ( start end -- ) #16 lshift or LTDC_L1WVPCR ! ;
: lcd-layer1-pixel-format! ( %bbb -- ) LTDC L1PFCR_PF bf! ;
: lcd-layer1-fb-adr! ( a -- ) LTDC L1CFBAR_CFBADD bf! ;
: lcd-layer1-fb-line-length! ( len -- ) dup #16 lshift swap $3 + or LTDC_L1CFBLR ! ;
: lcd-layer1-num-lines! ( lines -- ) LTDC L1CFBLNR_CFBLNBR bf! ;
\ ref: https://en.wikipedia.org/wiki/List_of_software_palettes#8-8-4_levels_RGB
\ These map an index to an 8 bit color, cylcling thru the colors
\ to ensure every combination is covered over 256 values of i.
: red-884 ( i -- c ) $e0 and #5 rshift #255 * #3 + #7 / ;
: green-884 ( i -- c ) $1C and #2 rshift #255 * #3 + #7 / ;
: blue-884 ( i -- c ) $3 and #255 * 1 + 3 / ;
: add-lcd-layer1-color-map-entry ( c i -- ) #24 lshift or LTDC_L1CLUTWR ! ;
: rgb>color ( r g b -- c ) $ff and swap $ff and 8 lshift or swap $ff and #16 lshift or ;
: create-lcd-layer1-color-map-8-8-4 ( -- )
  256 0 do
    i red-884
    i green-884
    i blue-884
    rgb>color
    i
    add-lcd-layer1-color-map-entry
  loop
;
: lcd-layer1-default-color! ( c -- ) LTDC_L1DCCR ! ;
: cfg-lcd-layer1 ( -- )
  L1_H_START L1_H_END lcd-layer1-h-pos!
  L1_V_START L1_V_END lcd-layer1-v-pos!
  L8_FMT lcd-layer1-pixel-format!
  LCD_FB1 @ lcd-layer1-fb-adr!
  RK043FN48H_WIDTH lcd-layer1-fb-line-length!
  RK043FN48H_HEIGHT lcd-layer1-num-lines!
  create-lcd-layer1-color-map-8-8-4
  BLACK lcd-layer1-default-color!
  \ config blending factors if needed
;

: lcd-layer1-on  ( -- ) L1CR_CLUTEN bfm L1CR_LEN bfm or LTDC_L1CR bis! ;

\ All of the layer1 and layer2 registers except the LTDC_LxCLUTWR register are
\ shadowed. If writing to more than one bitfield in separate ops you have to
\ update the shadow registers in between for shadowed registers. In this case,
\ prefer to write the whole register at once.
: update-lcd-shadow-registers ( -- ) LTDC SRCR_IMR bfs! ;

: enable-lcd-controller ( -- ) LTDC GCR_LTDCEN bfs! ;

: lcd-init  ( -- )
  cfg-lcd-gpio
  lcd-disp-on
  lcd-backlight-on
  enable-lcd-controller-clock
  cfg-pixel-clock-9.6mhz
  cfg-lcd-timings
  \ config sync signals and polarities in LTDC_GCR if needed
  BLACK lcd-background-color!
  \ config interrupts if needed
  cfg-lcd-layer1
  lcd-layer1-on
  \ enable dithering and color keying if needed
  update-lcd-shadow-registers
  enable-lcd-controller
;

: lcd-layer1-fb-adr@  ( a -- ) LTDC L1CFBAR_CFBADD bf@ ;
: show-test-pattern ( -- )
  lcd-layer1-fb-adr@
  RK043FN48H_HEIGHT 0 do
	  RK043FN48H_WIDTH 0 do
		dup
		j RK043FN48H_WIDTH *
		i +
		+
		j RK043FN48H_WIDTH *
		i +
		swap c!
	  loop
  loop
  drop
;

\ : lcd-layer1-off  ( -- ) $0 LTDC_L1CR ! ;
\ : lcd-layer1-key-color! ( r g b -- )    \ set layer color keying color
\   LTDC L1CKCR_CKBLUE bf!
\   LTDC L1CKCR_CKGREEN bf!
\   LTDC L1CKCR_CKRED bf!
\ ;
\ : lcd-layer-colormap-gray-scale ( layer -- ) \ grayscale colormap quick n dirty
\   #256 0 do
\     i i i rgb>color i add-lcd-layer1-color-map-entry
\   loop
\ ;
