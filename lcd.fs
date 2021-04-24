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
#41  constant RK043FN48H_HSYNC           \ Horizontal synchronization
#13  constant RK043FN48H_HBP             \ Horizontal back porch
#32  constant RK043FN48H_HFP             \ Horizontal front porch
#10  constant RK043FN48H_VSYNC           \ Vertical synchronization
#2   constant RK043FN48H_VBP             \ Vertical back porch
#2   constant RK043FN48H_VFP             \ Vertical front porch

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
   RK043FN48H_VSYNC RK043FN48H_VBP + 1- LTDC_BPCR_AHBP bf!

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

\ ***************************************** old ******************************************
: u.8 ( n -- )                           \ unsigned output 8 digits
   0 <# # # # # # # # # #> type ;
: x.8 ( n -- )                           \ hex output 8 digits
   base @ hex swap u.8 base ! ;
: x.2 ( n -- )                           \ hex output 2 digits
   base @ hex swap 0 <# # # #> type base ! ;

\ ***** gpio definitions ****************
\ http://www.st.com/web/en/resource/technical/document/reference_manual/DM00124865.pdf#page=195&zoom=auto,67,755
$40020000 constant GPIO-BASE
: gpio ( n -- adr )
   $f and #10 lshift GPIO-BASE or 1-foldable ;
$00         constant GPIO_MODER
$04         constant GPIO_OTYPER
$08         constant GPIO_OSPEEDR
$0C         constant GPIO_PUPDR
$10         constant GPIO_IDR
$14         constant GPIO_ODR
$18         constant GPIO_BSRR
$1C         constant GPIO_LCKR
$20         constant GPIO_AFRL
$24         constant GPIO_AFRH

#0  GPIO    constant GPIOA
#1  GPIO    constant GPIOB
#2  GPIO    constant GPIOC
#3  GPIO    constant GPIOD
#4  GPIO    constant GPIOE
#5  GPIO    constant GPIOF
#6  GPIO    constant GPIOG
#7  GPIO    constant GPIOH
#8  GPIO    constant GPIOI
#9  GPIO    constant GPIOJ
#10 GPIO    constant GPIOK

: pin#  ( pin -- nr )                    \ get pin number from pin
   $f and 1-foldable ;
: port-base  ( pin -- adr )              \ get port base from pin
   $f bic 1-foldable ;
: port# ( pin -- n )                     \ return gpio port number A:0 .. K:10
   #10 rshift $f and 1-foldable ;
: mode-mask  ( pin -- m )
   #3 swap pin# 2* lshift 1-foldable ;
: mode-shift ( mode pin -- mode<< )      \ shift mode by pin number * 2 for gpio_moder
   pin# 2* lshift 2-foldable ;
: set-mask! ( v m a -- )
   tuck @ swap bic rot or swap ! ;
: bsrr-on  ( pin -- v )                  \ gpio_bsrr mask pin on
   pin# 1 swap lshift 1-foldable ;
: bsrr-off  ( pin -- v )                 \ gpio_bsrr mask pin off
   pin# #16 + 1 swap lshift 1-foldable ;
   

\ ***** lcd definitions *****************
$40016800        constant LTDC              \ LTDC base
$08 LTDC +       constant LTDC_SSCR         \ LTDC Synchronization Size Configuration Register
$FFF #16 lshift  constant LTDC_SSCR_HSW     \ Horizontal Synchronization Width  ( pixel-1 )
$7FF             constant LTDC_SSCR_VSH     \ Vertical   Synchronization Height ( pixel-1 )
$0C LTDC +       constant LTDC_BPCR         \ Back Porch Configuration Register
$FFF #16 lshift  constant LTDC_BPCR_AHBP    \ HSYNC Width  + HBP - 1
$7FF             constant LTDC_BPCR_AVBP    \ VSYNC Height + VBP - 1
$10 LTDC +       constant LTDC_AWCR         \ Active Width Configuration Register
$FFF #16 lshift  constant LTDC_AWCR_AAW     \ HSYNC width  + HBP  + Active Width  - 1
$7FF             constant LTDC_AWCR_AAH     \ VSYNC Height + BVBP + Active Height - 1
$14 LTDC +       constant LTDC_TWCR         \ Total Width Configuration Register
$FFF #16 lshift  constant LTDC_TWCR_TOTALW  \ HSYNC Width + HBP  + Active Width  + HFP - 1
$7FF             constant LTDC_TWCR_TOTALH  \ VSYNC Height+ BVBP + Active Height + VFP - 1
$18 LTDC +       constant LTDC_GCR          \ Global Control Register
1  #31 lshift    constant LTDC_GCR_HSPOL    \ Horizontal Synchronization Polarity 0:active low 1: active high
1  #30 lshift    constant LTDC_GCR_VSPOL    \ Vertical Synchronization Polarity 0:active low 1:active high
1  #29 lshift    constant LTDC_GCR_DEPOL    \ Not Data Enable Polarity 0:active low 1:active high
1  #28 lshift    constant LTDC_GCR_PCPOL    \ Pixel Clock Polarity 0:nomal 1:inverted
1  #16 lshift    constant LTDC_GCR_DEN      \ dither enable
$7 #12 lshift    constant LTDC_GCR_DRW      \ Dither Red Width
$7  #8 lshift    constant LTDC_GCR_DGW      \ Dither Green Width
$7  #4 lshift    constant LTDC_GCR_DBW      \ Dither Blue Width
$1               constant LTDC_GCR_LTDCEN   \ LCD-TFT controller enable bit
$24 LTDC +       constant LTDC_SRCR         \ Shadow Reload Configuration Register
1 1 lshift       constant LTDC_SRCR_VBR     \ Vertical Blanking Reload
1                constant LTDC_SRCR_IMR     \ Immediate Reload
$2C LTDC +       constant LTDC_BCCR         \ Background Color Configuration Register RGB888
$FF #16 lshift   constant LTDC_BCCR_BCRED   \ Background Color Red
$FF  #8 lshift   constant LTDC_BCCR_BCGREEN \ Background Color Green
$FF              constant LTDC_BCCR_BCBLUE  \ Background Color Blue
$34 LTDC +       constant LTDC_IER          \ Interrupt Enable Register
#1 #3 lshift     constant LTDC_IER_RRIE     \ Register Reload interrupt enable
#1 #2 lshift     constant LTDC_IER_TERRIE   \ Transfer Error Interrupt Enable
#1 #1 lshift     constant LTDC_IER_FUIE     \ FIFO Underrun Interrupt Enable
#1               constant LTDC_IER_LIE      \ Line Interrupt Enable
$38 LTDC +       constant LTDC_ISR          \ Interrupt Status Register
#1 #3 lshift     constant LTDC_ISR_RRIF     \ Register Reload interrupt flag
#1 #2 lshift     constant LTDC_ISR_TERRIF   \ Transfer Error Interrupt flag
#1 #1 lshift     constant LTDC_ISR_FUIF     \ FIFO Underrun Interrupt flag
#1               constant LTDC_ISR_LIF      \ Line Interrupt flag
$3C LTDC +       constant LTDC_ICR          \ Interrupt Clear Register
#1 #3 lshift     constant LTDC_ICR_CRRIF    \ Register Reload interrupt flag
#1 #2 lshift     constant LTDC_ICR_CTERRIF  \ Transfer Error Interrupt flag
#1 #1 lshift     constant LTDC_ICR_CFUIF    \ FIFO Underrun Interrupt flag
#1               constant LTDC_ICR_CLIF     \ Line Interrupt flag
$40 LTDC +       constant LTDC_LIPCR        \ Line Interrupt Position Configuration Register
$7FF             constant LTDC_LIPCR_LIPOS  \ Line Interrupt Position
$44 LTDC +       constant LTDC_CPSR         \ Current Position Status Register
$FFFF #16 lshift constant LTDC_CPSR_CXPOS   \ Current X Position
$FFFF            constant LTDC_CPSR_CYPOS   \ Current Y Position
$48 LTDC +       constant LTDC_CDSR         \ Current Display Status Register
1 3 lshift       constant LTDC_CDSR_HSYNCS  \ Horizontal Synchronization display Status
1 2 lshift       constant LTDC_CDSR_VSYNCS  \ Vertical Synchronization display Status
1 1 lshift       constant LTDC_CDSR_HDES    \ Horizontal Data Enable display Status
1                constant LTDC_CDSR_VDES    \ Vertical Data Enable display Status
$84 LTDC +       constant LTDC_L1CR         \ Layer1 Control Register
1 4 lshift       constant LTDC_LxCR_CLUTEN  \ Color Look-Up Table Enable
1 2 lshift       constant LTDC_LxCR_COLKEN  \ Color Keying Enable
1                constant LTDC_LxCR_LEN     \ layer enable

$88 LTDC +       constant LTDC_L1WHPCR      \ Layer1 Window Horizontal Position Configuration Register
$8C LTDC +       constant LTDC_L1WVPCR      \ Layer1 Window Vertical Position Configuration Register
$90 LTDC +       constant LTDC_L1CKCR       \ Layer1 Color Keying Configuration Register
$94 LTDC +       constant LTDC_L1PFCR       \ Layer1 Pixel Format Configuration Register
$98 LTDC +       constant LTDC_L1CACR       \ Layer1 Constant Alpha Configuration Register
$9C LTDC +       constant LTDC_L1DCCR       \ Layer1 Default Color Configuration Register
$A0 LTDC +       constant LTDC_L1BFCR       \ Layer1 Blending Factors Configuration Register
$AC LTDC +       constant LTDC_L1CFBAR      \ Layer1 Color Frame Buffer Address Register
$B0 LTDC +       constant LTDC_L1CFBLR      \ Layer1 Color Frame Buffer Length Register
$B4 LTDC +       constant LTDC_L1CFBLNR     \ Layer1 ColorFrame Buffer Line Number Register
$C4 LTDC +       constant LTDC_L1CLUTWR     \ Layer1 CLUT Write Register

$84 LTDC + $80 + constant LTDC_L2CR         \ Layer2 Control Register
$88 LTDC + $80 + constant LTDC_L2WHPCR      \ Layer2 Window Horizontal Position Configuration Register
$8C LTDC + $80 + constant LTDC_L2WVPCR      \ Layer2 Window Vertical Position Configuration Register
$90 LTDC + $80 + constant LTDC_L2CKCR       \ Layer2 Color Keying Configuration Register
$94 LTDC + $80 + constant LTDC_L2PFCR       \ Layer2 Pixel Format Configuration Register
$98 LTDC + $80 + constant LTDC_L2CACR       \ Layer2 Constant Alpha Configuration Register
$9C LTDC + $80 + constant LTDC_L2DCCR       \ Layer2 Default Color Configuration Register
$A0 LTDC + $80 + constant LTDC_L2BFCR       \ Layer2 Blending Factors Configuration Register
$AC LTDC + $80 + constant LTDC_L2CFBAR      \ Layer2 Color Frame Buffer Address Register
$B0 LTDC + $80 + constant LTDC_L2CFBLR      \ Layer2 Color Frame Buffer Length Register
$B4 LTDC + $80 + constant LTDC_L2CFBLNR     \ Layer2 ColorFrame Buffer Line Number Register
$C4 LTDC + $80 + constant LTDC_L2CLUTWR     \ Layer2 CLUT Write Register

\ ***** lcd constants *******************
#0 constant LCD-PF-ARGB8888              \ pixel format argb
#1 constant LCD-PF-RGB888                \ pixel format rgb
#2 constant LCD-PF-RGB565                \ pixel format 16 bit
#3 constant LCD-PF-ARGB1555              \ pixel format 16 bit alpha
#4 constant LCD-PF-ARGB4444              \ pixel format 4 bit/color + 4 bit alpha
#5 constant LCD-PF-L8                    \ pixel format luminance 8 bit
#6 constant LCD-PF-AL44                  \ pixel format 4 bit alpha 4 bit luminance
#7 constant LCD-PF-AL88                  \ pixel format 8 bit alpha 8 bit luminance

\ ***** lcd gpio ports ******************
#4  GPIOE + constant PE4

#12 GPIOG + constant PG12

#7  GPIOH + constant PH7
#8  GPIOH + constant PH8

#0  GPIOJ + constant PJ0
#1  GPIOJ + constant PJ1
#2  GPIOJ + constant PJ2
#3  GPIOJ + constant PJ3
#4  GPIOJ + constant PJ4
#5  GPIOJ + constant PJ5
#6  GPIOJ + constant PJ6
#7  GPIOJ + constant PJ7
#8  GPIOJ + constant PJ8
#9  GPIOJ + constant PJ9
#10 GPIOJ + constant PJ10
#11 GPIOJ + constant PJ11
#13 GPIOJ + constant PJ13
#14 GPIOJ + constant PJ14
#15 GPIOJ + constant PJ15

#9   GPIOI + constant PI9
#10  GPIOI + constant PI10
#12  GPIOI + constant PI12
#13  GPIOI + constant PI13
#14  GPIOI + constant PI14
#15  GPIOI + constant PI15

#0  GPIOK + constant PK0
#1  GPIOK + constant PK1
#2  GPIOK + constant PK2
#3  GPIOK + constant PK3
#4  GPIOK + constant PK4
#5  GPIOK + constant PK5
#6  GPIOK + constant PK6
#7  GPIOK + constant PK7

\ ***** lcd io ports ********************
PI15 constant LCD_R0                     \ GPIO-AF14
PJ0  constant LCD_R1                     \ GPIO-AF14
PJ1  constant LCD_R2                     \ GPIO-AF14
PJ2  constant LCD_R3                     \ GPIO-AF14
PJ3  constant LCD_R4                     \ GPIO-AF14
PJ4  constant LCD_R5                     \ GPIO-AF14
PJ5  constant LCD_R6                     \ GPIO-AF14
PJ6  constant LCD_R7                     \ GPIO-AF14

PJ7  constant LCD_G0                     \ GPIO-AF14
PJ8  constant LCD_G1                     \ GPIO-AF14
PJ9  constant LCD_G2                     \ GPIO-AF14
PJ10 constant LCD_G3                     \ GPIO-AF14
PJ11 constant LCD_G4                     \ GPIO-AF14
PK0  constant LCD_G5                     \ GPIO-AF14
PK1  constant LCD_G6                     \ GPIO-AF14
PK2  constant LCD_G7                     \ GPIO-AF14

PE4  constant LCD_B0                     \ GPIO-AF14
PJ13 constant LCD_B1                     \ GPIO-AF14
PJ14 constant LCD_B2                     \ GPIO-AF14
PJ15 constant LCD_B3                     \ GPIO-AF14
PG12 constant LCD_B4                     \ GPIO-AF9
PK4  constant LCD_B5                     \ GPIO-AF14
PK5  constant LCD_B6                     \ GPIO-AF14
PK6  constant LCD_B7                     \ GPIO-AF14

PI14 constant LCD_CLK                    \ GPIO-AF14
PK7  constant LCD_DE                     \ GPIO-AF14
PI10 constant LCD_HSYNC                  \ GPIO-AF14
PI9  constant LCD_VSYNC                  \ GPIO-AF14
PI12 constant LCD_DISP
PI13 constant LCD_INT                    \ touch interrupt
PH7  constant LCD_SCL                    \ I2C3_SCL GPIO-AF4 touch i2c
PH8  constant LCD_SDA                    \ I2C3_SCL GPIO-AF4 touch i2c
PK3  constant LCD_BL                     \ lcd back light port

\ ***** lcd layer functions *************
0   constant layer1
$80 constant layer2
$10 constant LTDC_LxCR_CLUTEN                \ Color Look-Up Table Enable
 $2 constant LTDC_LxCR_COLKEN                \ Color Keying Enable
 $1 constant LTDC_LxCR_LEN                   \ layer enable
: layer-base ( l -- offset )                 \ layer base address
   0<> $80 and LTDC + 1-foldable ;
: layer-base ( l -- offset )                 \ layer base address
   LTDC + 1-foldable ;
: lcd-layer-const-alpha! ( alpha layer -- )  \ set layer constant alpha
   layer-base $98 + ! ;
: lcd-layer-default-color! ( c layer -- )    \ set layer default color ( argb8888 )
   layer-base $9C + ! ;
: lcd-layer-blend-cfg! ( bf1 bf2 layer -- )  \ set layer blending function
   layer-base $a0 + -rot swap 8 lshift or swap ! ;
: lcd-layer-num-lines! ( lines layer -- )    \ set layer number of lines to buffer
   layer-base $b4 + ! ;
: lcd-layer-color-map ( c i l -- )           \ set layer color at map index
   layer-base $c4 +
   -rot $ff and #24 lshift                   \ shift index to pos [31..24]
   swap $ffffff and or                       \ cleanup color
   swap ! ;

: lcd-layer-colormap-gray-scale ( layer -- ) \ grayscale colormap quick n dirty
   >R
   #256 0 do
     i dup dup #8 lshift or #8 lshift or
     i r@ lcd-layer-color-map
   loop rdrop ;
: red-884 ( i -- c )                         \ calc red component for 8-8-4 palette
   $e0 and #5 rshift
   #255 * #3 + #7 /
   #16 lshift ;
: green-884 ( i -- c )                       \ green component for 8-8-4 palette
   $1C and #2 rshift
   #255 * #3 + #7 /
   #8 lshift ;
: blue-884 ( i -- c )                        \ blue component for 8-8-4 palette
   $3 and
   #255 * 1 + 3 / ;
   
: lcd-layer-color-map-8-8-4 ( layer -- )     \ colormap 8 level red 8 green 4 blue
   >R
  256 0 do
    i dup red-884                            \ red
    over  green-884 or                       \ green
    over  blue-884 or                        \ blue
    swap R@ lcd-layer-color-map
    lcd-reg-update
  loop rdrop ;
   
: fb-init-0-ff ( layer -- )              \ fill frame buffer with values 0..255
   lcd-reg-update
   lcd-layer1-fb-adr@
   MAX_WIDTH MAX_HEIGHT * 0 do dup i + i swap c! loop drop ;

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
   MAX_HEIGHT layer1 lcd-layer-num-lines!
   layer1 fb-init-0-ff
   layer1 lcd-layer-color-map-8-8-4
   lcd-layer1-on
   0 layer1 lcd-layer-default-color!
   lcd-reg-update
   ;
: lcd-init  ( -- )                       \ pll-input frequency must be 1 MHz
   lcd-backlight-init
   lcd-display-init lcd-reg-update lcd-gpio-init lcd-disp-on
   lcd-layer1-init lcd-reg-update lcd-backlight-on ;
: >token ( a -- a )                      \ retrieve token name address for cfa
   1- dup c@ 0= +                        \ skip the padding zero 
   #256 1 do 1- dup c@ i = if leave then loop ; \ backtrack to start of counted string
: ctype.n ( width a -- )                 \ output counted string with fixed width
   dup ctype c@ - spaces ;
: const. ( a -- )                        \ dump register constant
   cr dup >token #15 swap ctype.n
   execute dup x.8 space @ x.8 ;
: 'reg. ( -- ) ( n:constant )            \ dump next word as register constant 
   postpone ['] postpone const. immediate ; 
: lcd. ( -- )                            \ dump lcd registers
  'reg. LTDC_SSCR
  'reg. LTDC_BPCR
  'reg. LTDC_AWCR
  'reg. LTDC_TWCR
  'reg. LTDC_GCR
  'reg. LTDC_SRCR
  'reg. LTDC_BCCR
  'reg. LTDC_IER
  'reg. LTDC_ISR
  'reg. LTDC_ICR
  'reg. LTDC_LIPCR
  'reg. LTDC_CPSR
  'reg. LTDC_CDSR cr ;
: lcd-l1.  ( -- )                         \ dump lcd layer1 registers
  'reg. LTDC_L1CR
  'reg. LTDC_L1WHPCR
  'reg. LTDC_L1WVPCR
  'reg. LTDC_L1CKCR
  'reg. LTDC_L1PFCR
  'reg. LTDC_L1CACR
  'reg. LTDC_L1DCCR
  'reg. LTDC_L1BFCR
  'reg. LTDC_L1CFBAR
  'reg. LTDC_L1CFBLR
  'reg. LTDC_L1CFBLNR
  'reg. LTDC_L1CLUTWR cr ;
: lcd-l2.  ( -- )  
  'reg. LTDC_L2CR
  'reg. LTDC_L2WHPCR
  'reg. LTDC_L2WVPCR
  'reg. LTDC_L2CKCR
  'reg. LTDC_L2PFCR
  'reg. LTDC_L2CACR
  'reg. LTDC_L2DCCR
  'reg. LTDC_L2BFCR
  'reg. LTDC_L2CFBAR
  'reg. LTDC_L2CFBLR
  'reg. LTDC_L2CFBLNR
  'reg. LTDC_L2CLUTWR cr ;
: gpiox. ( base -- base )                \ output string "gpio[a..x]_"
  dup ." GPIO" port# [char] A + emit [char] _ emit ; 
: gpio. ( pin -- )                       \ dump gpio port settings for pin
  dup cr ." PIN " $f and . cr
  port-base
  cr dup gpiox. ." MODER   " GPIO_MODER + @ x.8
  cr dup gpiox. ." OTYPER  " GPIO_OTYPER + @ x.8
  cr dup gpiox. ." OSPEEDR " GPIO_OSPEEDR + @ x.8
  cr dup gpiox. ." PUPDR   " GPIO_PUPDR + @ x.8
  cr dup gpiox. ." IDR     " GPIO_IDR + @ x.8
  cr dup gpiox. ." ODR     " GPIO_ODR + @ x.8
  cr dup gpiox. ." BSRR    " GPIO_BSRR + @ x.8
  cr dup gpiox. ." LCKR    " GPIO_LCKR + @ x.8
  cr dup gpiox. ." AFRL    " GPIO_AFRL + @ x.8
  cr     gpiox. ." AFRH    " GPIO_AFRH + @ x.8 cr ;

: vfade ( -- )                           \ vertical fade test
  lcd-fb1 @
  MAX_HEIGHT 0  do  MAX_WIDTH 0  do  j over c! 1+  loop  loop  drop ;
: clear ( -- )                           \ fill with 0
   lcd-fb1 @ dup MAX_HEIGHT MAX_WIDTH * + swap do 0 i ! #4 +loop ;
: fill ( c -- )                          \ fill sceen with color
   dup #8 lshift or dup #16 lshift or
   lcd-fb1 @ dup MAX_HEIGHT MAX_WIDTH * + swap do dup i ! #4 +loop drop ;
0 variable l1-x                          \ graphics x-pos
0 variable l1-y                          \ graphics y-pos
0 variable l1-c                          \ color
0 variable l1-fg                         \ forground color
0 variable l1-c4                         \ cccc
0 variable l1-bg                         \ background color
0 variable l1-bg4                        \ bcbcbcbc 4xbackground color
: y-limit ( y -- y )                     \ limit y
   0 max MAX_HEIGHT 1- min ;    
: x-limit ( y -- y )                     \ limit y
   0 max MAX_WIDTH 1- min ;    
: draw-pixel ( -- )                      \ draw pixel with current color
   l1-c @ l1-y @ y-limit MAX_WIDTH *
   l1-x @ x-limit + lcd-fb1 @ + c! ;
: l1-x++ ( -- )
   l1-x @ dup MAX_WIDTH 1- < - l1-x ! ;
: l1-x-- ( -- )
   l1-x @ dup 1 >= + l1-x ! ;
: l1-y++ ( -- )
   l1-y @ dup MAX_HEIGHT 1- < - l1-y ! ;
: l1-y-- ( -- )
   l1-y @ dup 1 >= + l1-y ! ;
: hline  ( l -- )                        \ draw horizontal line
   dup 0<
   if negate 0 do draw-pixel l1-x-- loop
   else      0 do draw-pixel l1-x++ loop then ;
: vline  ( h -- )                        \ draw vertical line
   dup 0<
   if negate 0 ?do draw-pixel l1-y-- loop
   else      0 ?do draw-pixel l1-y++ loop then ;
: rect ( w h -- )                        \ draw a rectangle
  over hline
  dup vline
  swap negate hline
  negate vline ;

0 variable x-alt
0 variable x-neu
0 variable x-sum
0 variable y-alt
0 variable y-neu
0 variable y-sum

0 variable dy
0 variable dx
0 variable xinc
0 variable yinc

: line-x>=y-test  ( -- )                 \ line drawing with rounding test
  0 x-alt !
  dx @ x-neu !
  0 y-sum !
  0 yinc !
  dx @ 1+ 0 do
   draw-pixel
   l1-x++
   y-sum @ dy @ + y-sum !
   y-sum @ x-neu @ >= if dx @ x-alt +! dx @ x-neu +! 1 yinc ! then
   y-sum @ x-alt @ - x-neu @ y-sum @ - >= if yinc @ l1-y +! 0 yinc ! then
  loop ;

: line-x>=y  ( -- )                      \ line for dx >= dy
  0 x-alt !
  dx @ x-neu !
  0 y-sum !
  dx @ 0 do
   draw-pixel
   xinc @ l1-x +!
   dy @ y-sum +!
   y-sum @ x-neu @ >= if dx @ x-alt +! dx @ x-neu +! yinc @ l1-y +! then
  loop draw-pixel ;
: line-y>=x  ( -- )                      \ line for dy >= dx
  0 y-alt !
  dy @ y-neu !
  0 x-sum !
  dy @ 0 do
   draw-pixel
   yinc @ l1-y +!
   dx @ x-sum +!
   x-sum @ y-neu @ >= if dy @ y-alt +! dy @ y-neu +! xinc @ l1-x +! then
  loop draw-pixel ;
: line  ( dx dy -- )                     \ line relative dx, dy
   1 xinc !
   1 yinc !
   dup 0< if -1 yinc ! negate then swap
   dup 0< if -1 xinc ! negate then swap
   2dup dy ! dx !
   >= if line-x>=y else
         line-y>=x then ;
: color! ( c -- )
   $ff and l1-c ! ;
: move-to ( x y -- )                     \ move graphics cursor to
   y-limit l1-y !
   x-limit l1-x ! ;
: move-by ( dx dy -- )                   \ move graphics cursor relative
   l1-y @ + y-limit l1-y !
   l1-x @ + x-limit l1-x ! ;
: nics-home ( -- )                       \ draw saint nicolaus home
  0 fill 0 0 move-to
   100    0 line 
  -100  100 line
   100    0 line
   -50   50 line
   -50  -50 line
     0 -100 line
   100  100 line
     0 -100 line ;
: hline-cx  ( l -- )                     \ hline constant x
  l1-x @ swap hline l1-x ! ;
: fill-rect ( w h -- )                   \ fill a rectangle
   l1-y @ tuck + swap
   do i l1-y ! dup hline-cx loop
   drop ;
: fill-rect-bg ( w h -- )
  l1-c @ -rot l1-bg @ l1-c ! fill-rect l1-c ! ; 
0 variable vx
0 variable vy
0 variable fx
0 variable fy

: vsync? ( -- f )                        \ in vsync state ?
   $4 LTDC_CDSR bit@ ;
: wait-vsync ( -- )                      \ wait for vertical sync
   begin vsync? not until
   begin vsync?     until ;   
: <0> ( n -- n )                         \ sign <0:-1, 0:0, >0:1
   dup 0< swap 0 > negate or ;
: mod-step ( a d -- m d )                \ modulo step (a<0): a+d, -1 (a>=d):a-d, 1 else a, 0
   abs 2dup >=
   if - 1
   else over 0< if + -1 else drop 0 then
   then ;
\ : s vx @ abs vy @ abs > if vy @ fy +!
0 variable idx                           \ current color cycling palette index 
: idx++ ( -- )
  idx @ 1+ $ff and idx ! ;
: rgb>color ( r g b -- c )               \ calc color c from r-g-b components
  $ff and swap $ff and 8 lshift or swap $ff and #16 lshift or ;
: r-g-b-r ( -- )                         \ red green blue palette for color cycling, start at idx
  #256 0 do #255 i - i 0 rgb>color idx @ layer1 lcd-layer-color-map idx++ 3 +loop
  #256 0 do 0 #255 i - i rgb>color idx @ layer1 lcd-layer-color-map idx++ 3 +loop
  #256 0 do i 0 #255 i - rgb>color idx @ layer1 lcd-layer-color-map idx++ 3 +loop ;

: delay 0 do loop ;
: palette-demo ( -- )                    \ color cycle palette
   begin wait-vsync r-g-b-r key? until ;
: palette-demo1 ( didx -- )              \ color cycle palette
   begin wait-vsync r-g-b-r dup idx +! key? until drop ;
: circle-test ( div -- )
  MAX_HEIGHT 0
  do
    MAX_WIDTH  0
    do
      i l1-x ! j l1-y ! i MAX_WIDTH 2/ - dup * j MAX_HEIGHT 2/ - dup * + over / color! draw-pixel
    loop
  loop drop ;
: circle-demo ( -- )
  begin
  150 1 do i circle-test 
    wait-vsync r-g-b-r
    key? if leave then
  loop
  150 1 do 150 i - circle-test 
    wait-vsync r-g-b-r
    key? if leave then
  loop
  key? until ;
: n->bbbb ( n -- n )                     \ make byte mask from nibble $c->$FF00FF00
  dup  $8 and #21 lshift
  over $4 and #14 lshift or
  over $2 and  #7 lshift or
  swap  1 and            or
  dup 2* or dup 2 lshift or ;
(create) nibble-mask-tab
   #0 n->bbbb ,  #1 n->bbbb ,  #2 n->bbbb ,  #3 n->bbbb ,
   #4 n->bbbb ,  #5 n->bbbb ,  #6 n->bbbb ,  #7 n->bbbb ,
   #8 n->bbbb ,  #9 n->bbbb , #10 n->bbbb , #11 n->bbbb ,
  #12 n->bbbb , #13 n->bbbb , #14 n->bbbb , #15 n->bbbb , smudge
' nibble-mask-tab constant nibble-mask-tab-adr   
: pixel-4-mask ( n -- n )                \ cache table
   2 lshift nibble-mask-tab-adr + @ 1-foldable ; 
0 variable pixel-adr
: b->bbbb ( -- )                         \ expand a byte to 4 byte 0xAB -> 0xABAB_ABAB
   dup #8 lshift or dup #16 lshift or ;
: pixel-line-4 ( n -- )                  \ draw 4 2-color pixel 1-forground 0 background
   $f and pixel-4-mask dup negate l1-bg @ b->bbbb and
   swap l1-c @ b->bbbb and or pixel-adr @ ! ;
: pixel-line-2 ( n -- )                  \ draw 2 2-color pixel 1-forground 0 background
   $3 and pixel-4-mask dup negate l1-bg @ b->bbbb and
   swap l1-c @ b->bbbb and or pixel-adr @ 2 + h! ;
: pixel-line-6-y++ ( n -- )              \ draw 6 2-color pixel in a line starting from pixel-adr
   dup pixel-line-4                      \ update pixel adr to next line
   4 rshift pixel-line-2
   pixel-adr @ MAX_WIDTH + pixel-adr ! ; \ next line
: draw-letter-6x8 ( a -- )
   2@ dup pixel-line-6-y++
   dup #6 rshift pixel-line-6-y++
   dup #12 rshift pixel-line-6-y++
   dup #18 rshift pixel-line-6-y++
   dup #24 rshift pixel-line-6-y++
   #30 rshift swap 2 lshift or
   dup pixel-line-6-y++
   dup #6 rshift pixel-line-6-y++
      #12 rshift pixel-line-6-y++ ;
MAX_HEIGHT 8 2 + / constant grid-space   \ grid space for 6x8 raster
grid-space 1 -     constant grid-fill    \ fill width of grid
MAX_WIDTH  grid-space 6 * - 2/ constant grid-h-start
MAX_HEIGHT grid-space 8 * - 2/ constant grid-v-start
grid-space 8 * constant grid-v-length
grid-space 6 * constant grid-h-length
: draw-raster-h-grid ( -- )              \ draw vertical lines of raster - horizontal grid
   grid-h-start grid-space 6 * 1+ +
   grid-h-start
   do
     grid-v-start l1-y ! i l1-x ! grid-v-length vline
   grid-space +loop ;
: draw-raster-v-grid ( -- )              \ draw horizontal lines of raster - vertical grid
   grid-v-start grid-space 8 * 1+ + 
   grid-v-start
   do
     i l1-y ! grid-h-start l1-x ! grid-h-length hline
   grid-space +loop ;
   
: draw-raster-6x8 ( -- )                 \ draw a 6x8 raster
   draw-raster-h-grid
   draw-raster-v-grid ;
0 variable raster-pixel-x
0 variable raster-pixel-y
: pixel-coord ( -- )                     \ update-pixel-coordinates
   raster-pixel-x @ grid-space * grid-h-start + 1+ l1-x !
   raster-pixel-y @ grid-space * grid-v-start + 1+ l1-y !
   raster-pixel-x @ 1+ dup 5 >
   if 1 raster-pixel-y +! drop 0 then 
   raster-pixel-x ! ;
: draw-raster-6x8-fill ( d -- d )        \ draw lsb a square and shift down
   pixel-coord
   over 1 and 0<>
   if   grid-fill dup fill-rect
   else grid-fill dup fill-rect-bg then
   dshr ;
: draw-raster-6x8-line ( d -- d )        \ draw a 6 pixel line
   draw-raster-6x8-fill draw-raster-6x8-fill
   draw-raster-6x8-fill draw-raster-6x8-fill
   draw-raster-6x8-fill draw-raster-6x8-fill ;
: draw-raster-6x8-letter ( d -- )        \ draw a character bitmap on raster
   0 raster-pixel-x !
   0 raster-pixel-y !
   \ 2@
   draw-raster-6x8-line draw-raster-6x8-line
   draw-raster-6x8-line draw-raster-6x8-line
   draw-raster-6x8-line draw-raster-6x8-line
   draw-raster-6x8-line draw-raster-6x8-line 2drop ;
: test-pixel-coord-line ( n -- )
   . raster-pixel-x @ . raster-pixel-y @ . l1-x @ . l1-y @ .
   pixel-coord ."  | "
   raster-pixel-x @ . raster-pixel-y @ . l1-x @ . l1-y @ .
   cr ;
: trp ( -- )                             \ test pixel-coord
   cr
   0 raster-pixel-x ! 0 raster-pixel-y ! 49 0
   do i test-pixel-coord-line loop ; 
: circle-palette-test ( -- )             \ animated color cycling circles
   lcd-init 50 circle-test -1 palette-demo1 ;
\ circle-palette-test

: bit-reverse-5..0 ( w - w ) \ reverse b0-b5
   $3f and
   dup  $01 and #5 lshift
   over $02 and #3 lshift or
   over $04 and shl or
   over $08 and shr or
   over $10 and #3 rshift or
   swap $20 and #5 rshift or ;
: 5dlshift ( d -- d )                    \ shift double left by 5 bits
  #64 0 ud* ;
: bit-5..0-rev-append ( w d -- d )       \ reverse bits 5..0 and append them to double word on stack
  5dlshift rot bit-reverse-5..0  rot or swap ;
: genchar ( l1 l2 l3 l4 l5 l6 l7 l8 -- d ) \ generate character bitmap line by line
   bit-reverse-5..0 0                    \ line 8 
   bit-5..0-rev-append                   \ line 7
   bit-5..0-rev-append                   \ line 6
   bit-5..0-rev-append                   \ line 5
   bit-5..0-rev-append                   \ line 4
   bit-5..0-rev-append                   \ line 3
   bit-5..0-rev-append                   \ line 2
   bit-5..0-rev-append ;                 \ line 1

\ some colors in 8-8-4 palette
\ red b7..5 green b4..2 blue b1..0
              7 2 lshift    constant green
   7 5 lshift 7 2 lshift or constant yellow
   7 5 lshift 4 2 lshift or constant orange
   7 5 lshift               constant red
                          3 constant blue
                          0 constant black
                          
: test-genchar                           \ test genchar
   lcd-init                                  \ init display
   layer1 lcd-layer-color-map-8-8-4      \ colormap rgb 8-8-4
   black fill                            \ clear screen
   232 color! draw-raster-6x8            \ orange raster
   green color!
   %001000   \ b00,b01,b02,b03,b04,b05
   %010100   \ b06,b07,b08,b09,b10,b11
   %100010   \ b12,b13,b14,b15,b16,b17
   %111110   \ b18,b19,b20,b21,b22,b23
   %100010   \ b24,b25,b26,b27,b28,b29
   %100010   \ b30,b31,b32,b33,b34,b35
   %000000   \ b36,b37,b38,b39,b40,b41
   %000000   \ b36,b37,b38,b39,b40,b41
   genchar
   draw-raster-6x8-letter ;
: raster-color-up ( -- )                 \ next color
  l1-c @ 1+ $ff and dup . cr color! draw-raster-6x8 ;
: raster-color-down ( -- )               \ previous color
  l1-c @ 1- $ff and dup . cr color! draw-raster-6x8 ;
: raster-color-sel ( -- )                \ change raster color
   l1-c @ . cr draw-raster-6x8
   begin key case [char] w of raster-color-up   0 endof \ next color
                  [char] s of raster-color-down 0 endof \ previous color
                  [char] q of                   1 endof
                  1
             endcase
   until ;
