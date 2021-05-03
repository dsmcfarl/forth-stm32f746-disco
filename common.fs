#require memmap.fs
#require bitfields.fs

\ helpers
: width-to-mask ( width -- mask ) #1 swap lshift #1 - 1-foldable 1-foldable ;
: offset-width-to-mask ( offset width -- mask ) width-to-mask swap lshift 2-foldable ;

\ Bitfields manipulation: bitfields are represented by a triplet of:
\   * register address offset from peripheral address (defined in memmap.fs)
\   * bit offset
\   * bit width
\ The bitfield words defined in bitfields.fs leave the
\ corresponding triplet on the stack. The following words perform an action
\ using a peripheral address and a bitfield triplet from the stack.
: bfs! ( addr addr-offset bit-offset width -- )		\ set all bitfield bits
  offset-width-to-mask -rot + bis!
;
: bfc! ( addr addr-offset bit-offset width -- )		\ clear all bitfield bits
  offset-width-to-mask -rot + bic!
;
: bf! ( u addr addr-offset bit-offset width -- )	\ store value in bitfield
  over >R		\ save a copy of bit-offset
  offset-width-to-mask	\ get a mask ( u addr addr-offset mask, R: bit-offset )
  -rot + rot		\ calc address and move u to top ( mask addr u, R: bit-offset )
  R> lshift		\ shift u to proper position ( mask addr u-shifted )
  rot dup not >R	\ ( addr u-shifted mask, R: mask-inverted )	
  and			\ ( addr u-shifted-masked, R: mask-inverted )
  swap dup		\ ( u-shifted-masked addr addr, R: mask-inverted )
  @ R> and		\ ( u-shifted-masked addr val-from-addr-masked )
  rot or swap !
;
: bf@ ( addr addr-offset bit-offset width -- u )	\ fetch value from bitfield
  over >R		\ save a copy of bit-offset
  offset-width-to-mask
  -rot +		\ move mask out of way and calc address ( mask addr, R: bit-offset )
  @ and R> rshift	\ fetch value and mask it then rshift by bit-offset
;
: bf. ( addr addr-offset bit-offset width -- u )	\ print value from bitfield
  bf@ .
;
: bfh. ( addr addr-offset bit-offset width -- )		\ print value from bitfield in hex
  base @ >R hex bf. R> base !
;
: bfb. ( addr addr-offset bit-offset width -- )		\ print value from bitfield in binary
  base @ >R binary bf. R> base !
;
: bfm ( addr-offset bit-offset width -- mask )
  offset-width-to-mask swap drop 3-foldable
;
: bf<< ( u addr-offset bit-offset width -- )		\ shift value into bitfield position and mask
  rot drop		\ do not need addr-offset
  over swap		\ save a copy of bit-offset
  offset-width-to-mask
  -rot
  lshift and
;

\ convenience words for printing in different formats without having the side
\ effect of changing the base for future words
: h. base @ >R hex . R> base ! ;	\ print in hex
: b. base @ >R binary . R> base ! ;	\ print in binary
: hu. base @ >R hex u. R> base ! ;	\ print in hex, unsigned
