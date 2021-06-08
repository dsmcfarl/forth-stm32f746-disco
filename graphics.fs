\ graphics.fs
\ Words for drawing geometry and text to an LCD.

\ #require graphics-rk043fn48h.fs
\ #require graphics-text.fs
\ #require graphics-geometry.fs

: line ( x0 y0 x1 y1 -- )
  line-y1 ! line-x1 !
  over line-x1 @ -   dup 0< if 1 else -1 then line-sx !   abs        line-dx !
  dup  line-y1 @ -   dup 0< if 1 else -1 then line-sy !   abs negate line-dy !
  line-dx @ line-dy @ + line-err !
  begin
    2dup putpixel
    2dup line-x1 @ line-y1 @ d<>
  while
    line-err @ 2* >r
    r@ line-dy @ > if line-dy @ line-err +! swap line-sx @ + swap then
    r> line-dx @ < if line-dx @ line-err +!      line-sy @ +      then
  repeat
  2drop
;

: ellipse ( xm ym a b -- )
  0 ellipse-dx ! dup ellipse-dy !
  dup ellipse-b ! dup * ellipse-b^2 !
  dup ellipse-a ! dup * ellipse-a^2 !
  ellipse-ym ! ellipse-xm !
  ellipse-b^2 @ ellipse-b @ 2* 1- ellipse-a^2 @ * - ellipse-err !
  begin
    ellipse-step
    ellipse-dy @ 0<
  until
  ellipse-dx @
  begin
    1+
    dup ellipse-a @ <
  while
    0 over        ellipse-putpixel
    0 over negate ellipse-putpixel
  repeat
  drop
;

: circle ( xm ym r -- ) dup ellipse ;

: font! ( font -- )
  dup font !
  4x6 =
  if
    font-char-bytes-4x6 font-char-bytes !
    ['] drawcharacterbitmap-4x6 drawcharacterbitmap-addr !
  else
    font-char-bytes-8x16 font-char-bytes !
    ['] drawcharacterbitmap-8x16 drawcharacterbitmap-addr !
  then
;

: drawstring ( addr u x y -- )
  font-y ! font-x !
  begin
    dup 0<>
  while
    get-first-char
    drawcharacter
    cut-first-char
  repeat
  2drop
;

: clear ( -- ) clear-layer1 ;

: color! ( c -- ) $ff and LAYER1_COLOR ! ;

: demo-graphics ( -- )
  red1 color!
  50 14 32 12 ellipse
  50 14 34 14 ellipse
  lightgreen color!
  8x16 font!
  s" Mecrisp" 22 7 drawstring
  yellow1 color!
  2 4 12 24 line
  4 4 14 24 line
  118 color!
  4x6 font!
  s" 123456789012345678901234567890123456789012345678901234567890" 0 80 drawstring
  8x16 font!
  gold3 color!
  s" hello world!" 10 40 drawstring
  orange1 color!
  s" ÄÖÜß" 10 60 drawstring

;

: init-graphics ( -- ) init-rk043fn48h 8x16 font! 245 color! ;

: init ( -- )
  init
  init-graphics
  ." graphics initialized" cr
;
