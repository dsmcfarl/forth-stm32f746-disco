#require gpio.fs

\ RK043FN48H-CT672B datasheet defines back porch differently than ST. ST back
\ porch does not include HSYNC or VSYNC. All timings in clock cycles.
#480	constant RK043FN48H_WIDTH	\ width of active area
#272	constant RK043FN48H_HEIGHT	\ height of active area
#10	constant RK043FN48H_HSYNC	\ horiz sync
#33	constant RK043FN48H_HBP		\ horiz back porch
#8	constant RK043FN48H_HFP		\ horiz front porch
#10	constant RK043FN48H_VSYNC	\ vert sync
#2	constant RK043FN48H_VBP		\ vert back porch
#4	constant RK043FN48H_VFP		\ vert front porch
$000000	constant BLACK
: lcd-background-color! ( c -- ) LTDC BCCR_BC bf! ;
: cfg-lcd-display ( -- )
  \ NOTE: LTDCEN conflicts with LTDCEN from RCC_APB2ENR so this has GCR_ prefix
  LTDC GCR_LTDCEN bfs!
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
  BLACK lcd-background-color!
  LTDC GCR_LTDCEN bfs!								\ enable LTDCEN LCD-TFT controller
;

: cfg-lcd-gpio ( -- )
  \ mecrisp-stellaris automatically enables all gpio ports
  AF GPIOI MODER_MODER15 bf! HIGH GPIOI OSPEEDR_OSPEEDR15 bf! AF14 GPIOI AFRH_AFRH15 bf!	\ LCD_R0
  AF GPIOJ MODER_MODER0 bf! HIGH GPIOJ OSPEEDR_OSPEEDR0 bf! AF14 GPIOJ AFRL_AFRL0 bf!	\ LCD_R1
  AF GPIOJ MODER_MODER1 bf! HIGH GPIOJ OSPEEDR_OSPEEDR1 bf! AF14 GPIOJ AFRL_AFRL1 bf!	\ LCD_R2
  AF GPIOJ MODER_MODER2 bf! HIGH GPIOJ OSPEEDR_OSPEEDR2 bf! AF14 GPIOJ AFRL_AFRL2 bf!	\ LCD_R3
  AF GPIOJ MODER_MODER3 bf! HIGH GPIOJ OSPEEDR_OSPEEDR3 bf! AF14 GPIOJ AFRL_AFRL3 bf!	\ LCD_R4
  AF GPIOJ MODER_MODER4 bf! HIGH GPIOJ OSPEEDR_OSPEEDR4 bf! AF14 GPIOJ AFRL_AFRL4 bf!	\ LCD_R5
  AF GPIOJ MODER_MODER5 bf! HIGH GPIOJ OSPEEDR_OSPEEDR5 bf! AF14 GPIOJ AFRL_AFRL5 bf!	\ LCD_R6
  AF GPIOJ MODER_MODER6 bf! HIGH GPIOJ OSPEEDR_OSPEEDR6 bf! AF14 GPIOJ AFRL_AFRL6 bf!	\ LCD_R7
  AF GPIOJ MODER_MODER7 bf! HIGH GPIOJ OSPEEDR_OSPEEDR7 bf! AF14 GPIOJ AFRL_AFRL7 bf!	\ LCD_G0
  AF GPIOJ MODER_MODER8 bf! HIGH GPIOJ OSPEEDR_OSPEEDR8 bf! AF14 GPIOJ AFRH_AFRH8 bf!	\ LCD_G1
  AF GPIOJ MODER_MODER9 bf! HIGH GPIOJ OSPEEDR_OSPEEDR9 bf! AF14 GPIOJ AFRH_AFRH9 bf!	\ LCD_G2
  AF GPIOJ MODER_MODER10 bf! HIGH GPIOJ OSPEEDR_OSPEEDR10 bf! AF14 GPIOJ AFRH_AFRH10 bf!	\ LCD_G3
  AF GPIOJ MODER_MODER11 bf! HIGH GPIOJ OSPEEDR_OSPEEDR11 bf! AF14 GPIOJ AFRH_AFRH11 bf!	\ LCD_G4
  AF GPIOK MODER_MODER0 bf! HIGH GPIOK OSPEEDR_OSPEEDR0 bf! AF14 GPIOK AFRL_AFRL0 bf!	\ LCD_G5
  AF GPIOK MODER_MODER1 bf! HIGH GPIOK OSPEEDR_OSPEEDR1 bf! AF14 GPIOK AFRL_AFRL1 bf!	\ LCD_G6
  AF GPIOK MODER_MODER2 bf! HIGH GPIOK OSPEEDR_OSPEEDR2 bf! AF14 GPIOK AFRL_AFRL2 bf!	\ LCD_G7
  AF GPIOE MODER_MODER4 bf! HIGH GPIOE OSPEEDR_OSPEEDR4 bf! AF14 GPIOE AFRL_AFRL4 bf!	\ LCD_B0
  AF GPIOJ MODER_MODER13 bf! HIGH GPIOJ OSPEEDR_OSPEEDR13 bf! AF14 GPIOJ AFRH_AFRH13 bf!	\ LCD_B1
  AF GPIOJ MODER_MODER14 bf! HIGH GPIOJ OSPEEDR_OSPEEDR14 bf! AF14 GPIOJ AFRH_AFRH14 bf!	\ LCD_B2
  AF GPIOJ MODER_MODER15 bf! HIGH GPIOJ OSPEEDR_OSPEEDR15 bf! AF14 GPIOJ AFRH_AFRH15 bf!	\ LCD_B3
  AF GPIOG MODER_MODER12 bf! HIGH GPIOG OSPEEDR_OSPEEDR12 bf! AF14 GPIOG AFRH_AFRH12 bf!	\ LCD_B4
  AF GPIOK MODER_MODER4 bf! HIGH GPIOK OSPEEDR_OSPEEDR4 bf! AF14 GPIOK AFRL_AFRL4 bf!	\ LCD_B5
  AF GPIOK MODER_MODER5 bf! HIGH GPIOK OSPEEDR_OSPEEDR5 bf! AF14 GPIOK AFRL_AFRL5 bf!	\ LCD_B6
  AF GPIOK MODER_MODER6 bf! HIGH GPIOK OSPEEDR_OSPEEDR6 bf! AF14 GPIOK AFRL_AFRL6 bf!	\ LCD_B7
  AF GPIOK MODER_MODER7 bf! HIGH GPIOK OSPEEDR_OSPEEDR7 bf! AF14 GPIOK AFRL_AFRL7 bf!	\ LCD_DE
  AF GPIOI MODER_MODER14 bf! HIGH GPIOI OSPEEDR_OSPEEDR14 bf! AF14 GPIOI AFRH_AFRH14 bf!	\ LCD_CLK
  AF GPIOI MODER_MODER10 bf! HIGH GPIOI OSPEEDR_OSPEEDR10 bf! AF14 GPIOI AFRH_AFRH10 bf!	\ LCD_HSYNC
  AF GPIOI MODER_MODER9 bf! HIGH GPIOI OSPEEDR_OSPEEDR9 bf! AF14 GPIOI AFRH_AFRH9 bf!	\ LCD_VSYNC
  OUTPUT GPIOI MODER_MODER12 bf!								\ LCD_DISP
  OUTPUT GPIOK MODER_MODER3 bf!									\ LCD_BL
;

\ Some LTDC registers use shadow registers so if writing to more than one bitfield
\ in separate ops so you have to do lcd-reg-update in between.
: lcd-reg-update ( -- ) LTDC SRCR_IMR bfs! ;

: lcd-backlight-on  ( -- ) GPIOK BSRR_BS3 bfs! ;
: lcd-disp-on  ( -- ) GPIOI BSRR_BS12 bfs! ;
: lcd-layer1-on  ( -- ) L1CR_CLUTEN bfm L1CR_LEN bfm or LTDC_L1CR bis! ;
: lcd-layer1-off  ( -- ) $0 LTDC_L1CR ! ;
: lcd-layer1-h-pos! ( start end -- ) LTDC L1WHPCR_WHSPPOS bf! lcd-reg-update LTDC L1WHPCR_WHSTPOS bf! ;
: lcd-layer1-v-pos! ( start end -- ) LTDC L1WVPCR_WVSPPOS bf! lcd-reg-update LTDC L1WVPCR_WVSTPOS bf! ;
: lcd-layer1-key-color! ( r g b -- )    \ set layer color keying color
  LTDC L1CKCR_CKBLUE bf!
  LTDC L1CKCR_CKGREEN bf!
  LTDC L1CKCR_CKRED bf!
;
%101 constant L8_FMT
: lcd-layer1-pixel-format! ( %bbb -- ) LTDC L1PFCR_PF bf! ;

\ for readability:
RK043FN48H_WIDTH  constant MAX_WIDTH
RK043FN48H_HEIGHT constant MAX_HEIGHT

\ setup a frame buffer   
\ TODO: why the variabls? how does BUFFER: work?
MAX_WIDTH MAX_HEIGHT * dup BUFFER: lcd-fb1-buffer constant lcd-fb1-size#
lcd-fb1-buffer variable lcd-fb1              \ frame buffer 1 pointer
lcd-fb1-size#  variable lcd-fb1-size         \ frame buffer 1 size
: lcd-layer1-fb-adr!  ( a -- ) LTDC L1CFBAR_CFBADD bf! ;
: lcd-layer1-fb-adr@  ( a -- ) LTDC L1CFBAR_CFBADD bf@ ;
: lcd-layer1-fb-line-length! ( len -- ) dup LTDC L1CFBLR_CFBP bf! lcd-reg-update $3 + LTDC L1CFBLR_CFBLL bf! ;
: lcd-layer1-num-lines! ( lines -- ) LTDC L1CFBLNR_CFBLNBR bf! ;

: fb-init-0-ff ( -- )              \ fill frame buffer with values 0..255
  lcd-reg-update
  lcd-layer1-fb-adr@
  MAX_HEIGHT 0 do
	  MAX_WIDTH 0 do
		dup
		j MAX_WIDTH *
		i +
		+
		j MAX_WIDTH *
		i +
		swap c!
	  loop
  loop
  drop
;
: lcd-layer1-color-map ( c i -- )           \ set layer color at map index
  \ must set whole register at once
  #24 lshift or
  LTDC_L1CLUTWR !
;

: rgb>color ( r g b -- c )               \ calc color c from r-g-b components
  $ff and swap $ff and 8 lshift or swap $ff and #16 lshift or ;
: lcd-layer-colormap-gray-scale ( layer -- ) \ grayscale colormap quick n dirty
   #256 0 do
     i i i rgb>color i lcd-layer1-color-map
   loop
;

\ ref: https://en.wikipedia.org/wiki/List_of_software_palettes#8-8-4_levels_RGB
\ These map an index to an 8 bit color, cylcling thru the colors
\ to ensure every combination is covered over 256 values of i.
: red-884 ( i -- c )                         \ calc red component for 8-8-4 palette
   $e0 and #5 rshift
   #255 * #3 + #7 /
;
: green-884 ( i -- c )                       \ green component for 8-8-4 palette
   $1C and #2 rshift
   #255 * #3 + #7 /
;
: blue-884 ( i -- c )                        \ blue component for 8-8-4 palette
   $3 and
   #255 * 1 + 3 /
;
   
: lcd-layer1-color-map-8-8-4 ( -- )     \ colormap 8 level red 8 green 4 blue
  256 0 do
    i red-884
    i green-884
    i blue-884
    rgb>color
    i
    lcd-layer1-color-map
    lcd-reg-update
  loop
;
: lcd-layer1-default-color! ( c -- )    \ set layer default color ( argb8888 )
  LTDC_L1DCCR !
;
   
\ layer 1 view port constants
RK043FN48H_HSYNC RK043FN48H_HBP +       constant L1-h-start
L1-h-start       RK043FN48H_WIDTH + 1-  constant L1-h-end
RK043FN48H_VSYNC RK043FN48H_VBP +       constant L1-v-start
L1-v-start       RK043FN48H_HEIGHT + 1- constant L1-v-end


: lcd-layer1-init ( -- )
   lcd-layer1-off
   lcd-reg-update
   10 0 do LTDC_L1CR @ h. loop
   L1-h-start L1-h-end lcd-layer1-h-pos!
   L1-v-start L1-v-end lcd-layer1-v-pos!
   0 0 0 lcd-layer1-key-color!         \ key color black no used here
   L8_FMT lcd-layer1-pixel-format!     \ 8 bit per pixel frame buffer format
   lcd-fb1 @ lcd-layer1-fb-adr!    \ set frame buffer address
   MAX_WIDTH lcd-layer1-fb-line-length!
   MAX_HEIGHT lcd-layer1-num-lines!
   fb-init-0-ff
   lcd-layer1-color-map-8-8-4
   lcd-layer1-on
   lcd-reg-update
   cr
   10 0 do LTDC_L1CR @ h. loop
   $0 lcd-layer1-default-color!
   lcd-reg-update
   ;
: lcd-init  ( -- )                       \ pll-input frequency must be 1 MHz
   cfg-lcd-display lcd-reg-update cfg-lcd-gpio lcd-disp-on
   lcd-layer1-init lcd-reg-update lcd-backlight-on ;
