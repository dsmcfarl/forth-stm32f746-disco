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

: lcd-background-color! ( rgb -- ) $ffffff and LTDC_BCCR ! ;

: af14-fast! ( port pin -- ) 2dup AF -rot moder! 2dup HIGH -rot ospeedr! AF14 -rot afr! ;
: cfg-lcd-gpio ( -- )
  GPIOI #15 af14-fast!			\ LCD_R0
  GPIOJ #0 af14-fast!			\ LCD_R1
  GPIOJ #1 af14-fast!			\ LCD_R2
  GPIOJ #2 af14-fast!			\ LCD_R3
  GPIOJ #3 af14-fast!			\ LCD_R4
  GPIOJ #4 af14-fast!			\ LCD_R5
  GPIOJ #5 af14-fast!			\ LCD_R6
  GPIOJ #6 af14-fast!			\ LCD_R7
  GPIOJ #7 af14-fast!			\ LCD_G0
  GPIOJ #8 af14-fast!			\ LCD_G1
  GPIOJ #9 af14-fast!			\ LCD_G2
  GPIOJ #10 af14-fast!			\ LCD_G3
  GPIOJ #11 af14-fast!			\ LCD_G4
  GPIOK #0 af14-fast!			\ LCD_G5
  GPIOK #1 af14-fast!			\ LCD_G6
  GPIOK #2 af14-fast!			\ LCD_G7
  GPIOE #4 af14-fast!			\ LCD_B0
  GPIOJ #13 af14-fast!			\ LCD_B1
  GPIOJ #14 af14-fast!			\ LCD_B2
  GPIOJ #15 af14-fast!			\ LCD_B3
  GPIOG #12 af14-fast!			\ LCD_B4
  GPIOK #4 af14-fast!			\ LCD_B5
  GPIOK #5 af14-fast!			\ LCD_B6
  GPIOK #6 af14-fast!			\ LCD_B7
  GPIOK #7 af14-fast!			\ LCD_DE
  GPIOI #14 af14-fast!			\ LCD_CLK
  GPIOI #10 af14-fast!			\ LCD_HSYNC
  GPIOI #9 af14-fast!			\ LCD_VSYNC
  OUTPUT GPIOI #12 moder!		\ LCD_DISP
  OUTPUT GPIOK #3 moder!		\ LCD_BL
;

: lcd-backlight-on  ( -- ) GPIOK #3 bs! ;

: lcd-disp-on  ( -- ) GPIOI #12 bs! ;

RK043FN48H_HSYNC RK043FN48H_HBP +       constant L1_H_START
L1_H_START       RK043FN48H_WIDTH + 1-  constant L1_H_END
RK043FN48H_VSYNC RK043FN48H_VBP +       constant L1_V_START
L1_V_START       RK043FN48H_HEIGHT + 1- constant L1_V_END
%101					constant L8_FMT     \ 8 bit per pixel frame buffer format
RK043FN48H_WIDTH RK043FN48H_HEIGHT * dup BUFFER: LCD_FB1_BUFFER constant LCD_FB1_SIZE#
LCD_FB1_BUFFER				variable LCD_FB1              \ frame buffer 1 pointer
LCD_FB1_SIZE#				variable LCD_FB1_SIZE         \ frame buffer 1 size
: lcd-layer1-h-pos! ( start end -- ) L1WHPCR_WHSPPOS bf<< swap L1WHPCR_WHSTPOS bf<< or LTDC_L1WHPCR ! ;
: lcd-layer1-v-pos! ( start end -- ) L1WVPCR_WVSPPOS bf<< swap L1WVPCR_WVSTPOS bf<< or LTDC_L1WVPCR ! ;
: lcd-layer1-pixel-format! ( %bbb -- ) LTDC L1PFCR_PF bf! ;
: lcd-layer1-fb-adr! ( a -- ) LTDC L1CFBAR_CFBADD bf! ;
: lcd-layer1-fb-line-length! ( len -- ) dup L1CFBLR_CFBP bf<< swap $3 + L1CFBLR_CFBLL bf<< or LTDC_L1CFBLR ! ;
: lcd-layer1-num-lines! ( lines -- ) LTDC L1CFBLNR_CFBLNBR bf! ;
\ ref: https://en.wikipedia.org/wiki/List_of_software_palettes#8-8-4_levels_RGB
\ These map an index to an 8 bit color, cylcling thru the colors
\ to ensure every combination is covered over 256 values of i.
: red-884 ( i -- c ) $e0 and #5 rshift #255 * #3 + #7 / ;
: green-884 ( i -- c ) $1C and #2 rshift #255 * #3 + #7 / ;
: blue-884 ( i -- c ) $3 and #255 * 1 + 3 / ;
: add-lcd-layer1-color-map-entry ( r g b i -- ) L1CLUTWR_CLUTADD bf<< swap L1CLUTWR_BLUE bf<< or swap L1CLUTWR_GREEN bf<< or swap L1CLUTWR_RED bf<< or LTDC_L1CLUTWR ! ;
: create-lcd-layer1-color-map-8-8-4 ( -- )
  256 0 do
    i red-884
    i green-884
    i blue-884
    i
    add-lcd-layer1-color-map-entry
  loop
;
$0 constant BLACK
: lcd-layer1-default-color! ( argb -- ) LTDC_L1DCCR ! ;
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
\ shadowed. Reading from the register shows the active value. 
\ You have to do update-lcd-shadow-registers for changes to take
\ effect. If writing to more than one bitfield in separate ops you have to update
\ the shadow registers in between for shadowed registers. In this case, prefer to
\ write the whole register at once.
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
: lcd-backlight-off  ( -- ) GPIOK #3 br! ;
: lcd-disp-off  ( -- ) GPIOI #12 br! ;
: lcd-layer1-off  ( -- ) $0 LTDC_L1CR ! ;

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

: clear ( -- )                           \ fill with 0
  lcd-layer1-fb-adr@ dup RK043FN48H_HEIGHT RK043FN48H_WIDTH * + swap do 0 i ! #4 +loop
;
0 variable LAYER1_COLOR
: color! ( c -- ) $ff and LAYER1_COLOR ! ;
: y-limit ( y -- y ) 0 max RK043FN48H_HEIGHT 1- min ;    
: x-limit ( y -- y ) 0 max RK043FN48H_WIDTH 1- min ;    
: putpixel ( x y -- )                       \ draw pixel with current color
  LAYER1_COLOR @ swap y-limit RK043FN48H_WIDTH *
  rot x-limit + lcd-layer1-fb-adr@ + c!
;
