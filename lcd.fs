#require gpio.fs

: lcd-gpio-init ( -- )
  \ LCD_R0 thru 7
  AF GPIOI_MODER_MODER15 bf! HIGH_SPD GPIOI_OSPEEDR_OSPEEDR15 bf! AF14 GPIOI_AFRH_AFRH15 bf!
  AF GPIOJ_MODER_MODER0 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR0 bf! AF14 GPIOJ_AFRL_AFRL0 bf!
  AF GPIOJ_MODER_MODER1 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR1 bf! AF14 GPIOJ_AFRL_AFRL1 bf!
  AF GPIOJ_MODER_MODER2 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR2 bf! AF14 GPIOJ_AFRL_AFRL2 bf!
  AF GPIOJ_MODER_MODER3 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR3 bf! AF14 GPIOJ_AFRL_AFRL3 bf!
  AF GPIOJ_MODER_MODER4 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR4 bf! AF14 GPIOJ_AFRL_AFRL4 bf!
  AF GPIOJ_MODER_MODER5 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR5 bf! AF14 GPIOJ_AFRL_AFRL5 bf!
  AF GPIOJ_MODER_MODER6 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR6 bf! AF14 GPIOJ_AFRL_AFRL6 bf!

  \ LCD_G0 thru 7
  AF GPIOJ_MODER_MODER7 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR7 bf! AF14 GPIOJ_AFRL_AFRL7 bf!
  AF GPIOJ_MODER_MODER8 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR8 bf! AF14 GPIOJ_AFRH_AFRH8 bf!
  AF GPIOJ_MODER_MODER9 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR9 bf! AF14 GPIOJ_AFRH_AFRH9 bf!
  AF GPIOJ_MODER_MODER10 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR10 bf! AF14 GPIOJ_AFRH_AFRH10 bf!
  AF GPIOJ_MODER_MODER11 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR11 bf! AF14 GPIOJ_AFRH_AFRH11 bf!
  AF GPIOK_MODER_MODER0 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR0 bf! AF14 GPIOK_AFRL_AFRL0 bf!
  AF GPIOK_MODER_MODER1 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR1 bf! AF14 GPIOK_AFRL_AFRL1 bf!
  AF GPIOK_MODER_MODER2 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR2 bf! AF14 GPIOK_AFRL_AFRL2 bf!

  \ LCD_B0 thru 7
  AF GPIOE_MODER_MODER4 bf! HIGH_SPD GPIOE_OSPEEDR_OSPEEDR4 bf! AF14 GPIOE_AFRL_AFRL4 bf!
  AF GPIOJ_MODER_MODER13 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR13 bf! AF14 GPIOJ_AFRH_AFRH13 bf!
  AF GPIOJ_MODER_MODER14 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR14 bf! AF14 GPIOJ_AFRH_AFRH14 bf!
  AF GPIOJ_MODER_MODER15 bf! HIGH_SPD GPIOJ_OSPEEDR_OSPEEDR15 bf! AF14 GPIOJ_AFRH_AFRH15 bf!
  AF GPIOG_MODER_MODER12 bf! HIGH_SPD GPIOG_OSPEEDR_OSPEEDR12 bf! AF14 GPIOG_AFRH_AFRH12 bf!
  AF GPIOK_MODER_MODER4 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR4 bf! AF14 GPIOK_AFRL_AFRL4 bf!
  AF GPIOK_MODER_MODER5 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR5 bf! AF14 GPIOK_AFRL_AFRL5 bf!
  AF GPIOK_MODER_MODER6 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR6 bf! AF14 GPIOK_AFRL_AFRL6 bf!

  AF GPIOK_MODER_MODER7 bf! HIGH_SPD GPIOK_OSPEEDR_OSPEEDR7 bf! AF14 GPIOK_AFRL_AFRL7 bf!	\ LCD_DE
  AF GPIOI_MODER_MODER14 bf! HIGH_SPD GPIOI_OSPEEDR_OSPEEDR14 bf! AF14 GPIOI_AFRH_AFRH14 bf!	\ LCD_CLK
  AF GPIOI_MODER_MODER10 bf! HIGH_SPD GPIOI_OSPEEDR_OSPEEDR10 bf! AF14 GPIOI_AFRH_AFRH10 bf!	\ LCD_HSYNC
  AF GPIOI_MODER_MODER9 bf! HIGH_SPD GPIOI_OSPEEDR_OSPEEDR9 bf! AF14 GPIOI_AFRH_AFRH9 bf!	\ LCD_VSYNC
  OUTPUT GPIOI_MODER_MODER12 bf!
;

: lcd-backlight-init  ( -- )             \ initialize lcd backlight port
  enable-gpiok-clock
  OUTPUT GPIOK_MODER_MODER3 bf!		\ LCD_BL
;

\ ***** LCD Timings *********************
#480 constant RK043FN48H_WIDTH
#272 constant RK043FN48H_HEIGHT
#10  constant RK043FN48H_HSYNC           \ Horizontal synchronization
#33  constant RK043FN48H_HBP             \ Horizontal back porch
#8  constant RK043FN48H_HFP             \ Horizontal front porch
#10  constant RK043FN48H_VSYNC           \ Vertical synchronization
#2   constant RK043FN48H_VBP             \ Vertical back porch
#4   constant RK043FN48H_VFP             \ Vertical front porch

RK043FN48H_WIDTH  constant MAX_WIDTH     \ maximum width
RK043FN48H_HEIGHT constant MAX_HEIGHT    \ maximum height

: lcd-back-color! ( r g b -- )           \ lcd background color
 $ff and swap $ff and #8 lshift or
 swap $ff and #16 lshift LTDC_BCCR_BC bf!
;
: lcd-display-init ( -- )                \ set display configuration
  LTDC_GCR_LTDCEN bfs!
   RK043FN48H_HSYNC 1- LTDC_SSCR_HSW bf!
   RK043FN48H_VSYNC 1- LTDC_SSCR_VSH bf!

   RK043FN48H_HSYNC RK043FN48H_HBP + 1- LTDC_BPCR_AHBP bf!
   RK043FN48H_VSYNC RK043FN48H_VBP + 1- LTDC_BPCR_AVBP bf!

   RK043FN48H_WIDTH  RK043FN48H_HSYNC + RK043FN48H_HBP + 1- LTDC_AWCR_AAV bf!
   RK043FN48H_HEIGHT RK043FN48H_VSYNC + RK043FN48H_VBP + 1- LTDC_AWCR_AAH bf!

   RK043FN48H_WIDTH RK043FN48H_HSYNC +
   RK043FN48H_HBP + RK043FN48H_HFP + 1- LTDC_TWCR_TOTALW bf!
   RK043FN48H_HEIGHT RK043FN48H_VSYNC +
   RK043FN48H_VBP + RK043FN48H_VFP + 1- LTDC_TWCR_TOTALH bf!

   0 0 0 lcd-back-color!                 \ black back ground
   LTDC_GCR_LTDCEN bfs!                          \ LTDCEN LCD-TFT controller enable
;
\ LTDC registers use shadow registers so if writing to more than one bitfield
\ in separate ops, you have to do lcd-gre-update in between
: lcd-reg-update ( -- ) LTDC_SRCR_IMR bfs! ;
: lcd-backlight-on  ( -- ) GPIOK_BSRR_BS3 bfs! ;
: lcd-disp-on  ( -- ) GPIOI_BSRR_BS12 bfs! ;
: lcd-layer1-on  ( -- ) LTDC_L1CR_CLUTEN bfs! lcd-reg-update LTDC_L1CR_LEN bfs! ;
: lcd-layer1-off  ( -- ) $0 LTDC_L1CR ! ;
: lcd-layer1-h-pos! ( start end -- ) LTDC_L1WHPCR_WHSPPOS bf! lcd-reg-update LTDC_L1WHPCR_WHSTPOS bf! ;
: lcd-layer1-v-pos! ( start end -- ) LTDC_L1WVPCR_WVSPPOS bf! lcd-reg-update LTDC_L1WVPCR_WVSTPOS bf! ;
: lcd-layer1-key-color! ( r g b -- )    \ set layer color keying color
  LTDC_L1CKCR_CKBLUE bf!
  LTDC_L1CKCR_CKGREEN bf!
  LTDC_L1CKCR_CKRED bf!
;
%101 constant L8_FMT
: lcd-layer1-pixel-format! ( %bbb -- ) LTDC_L1PFCR_PF bf! ;

\ setup a frame buffer   
\ TODO: why the variabls? how does BUFFER: work?
MAX_WIDTH MAX_HEIGHT * dup BUFFER: lcd-fb1-buffer constant lcd-fb1-size#
lcd-fb1-buffer variable lcd-fb1              \ frame buffer 1 pointer
lcd-fb1-size#  variable lcd-fb1-size         \ frame buffer 1 size
: lcd-layer1-fb-adr!  ( a -- ) LTDC_L1CFBAR_CFBADD bf! ;
: lcd-layer1-fb-adr@  ( a -- ) LTDC_L1CFBAR_CFBADD bf@ ;
: lcd-layer1-fb-line-length! ( len -- ) dup LTDC_L1CFBLR_CFBP bf! lcd-reg-update $3 + LTDC_L1CFBLR_CFBLL bf! ;
: lcd-layer1-num-lines! ( lines -- ) LTDC_L1CFBLNR_CFBLNBR bf! ;

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
   $0 lcd-layer1-default-color!
   lcd-reg-update
   ;
: lcd-init  ( -- )                       \ pll-input frequency must be 1 MHz
   lcd-backlight-init
   lcd-display-init lcd-reg-update lcd-gpio-init lcd-disp-on
   lcd-layer1-init lcd-reg-update lcd-backlight-on ;
