#require graphics-rk043fn48h.fs
#require graphics-text.fs
#require graphics-geometry.fs

: line ( x0 y0 x1 y1 -- )		\ Bresenham line

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

: ellipse ( xm ym a b -- )		\ Bresenham ellipse

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
  font-4x6 =
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

: init-graphics ( -- ) init-rk043fn48h ;
