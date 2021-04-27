#require memmap.fs
#require bitfields.fs

: count-trailing-zeros ( mask -- u )
  dup negate and 1-
  clz negate #32 + 1-foldable
;
: masked@ ( mask addr -- u )		\ fetch value from masked bits at address
   @ over and swap
   count-trailing-zeros rshift
 ;
: masked! ( u mask addr -- )		\ store value at masked bits at address
  >R dup >R count-trailing-zeros lshift	\ shift value to proper position (u-shifted , R: mask addr )
  R@ and				\ mask out unrelated bits ( u-shifted-masked , R: mask addr )
  R> not R@ @ and			\ invert bitmask and maskout new bits in current value ( u-shifted-masked val-from-addr-masked, R: addr )
  or r> !				\ apply value and store back
;
: width-to-mask ( width -- mask ) #1 swap lshift #1 - 1-foldable ;
: offset-width-to-mask ( offset width -- mask ) width-to-mask swap lshift ;

: addr-offset-width-to-mask-addr ( addr addr-offset bit-offset width -- mask addr )
  width-to-mask swap lshift rot rot +
  4-foldable
 ;

\ TODO: update comment
\ Bitfields are represented by a bit offset, bit width, and register address
\ triplet. The bitfield words defined in bitfields.fs leave the
\ corresponding triplet on the stack. The following words perform an action
\ using a bitfield triplet from the stack.

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
  base @ >R hex bf. R> base !
;
: bfm ( addr-offset bit-offset width -- mask )
  offset-width-to-mask swap drop 3-foldable
;

: h.
  base @ >R hex . R> base !
;
