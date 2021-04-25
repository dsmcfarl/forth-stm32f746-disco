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
  >R dup >R count-trailing-zeros lshift	\ shift value to proper position
  R@ and				\ mask out unrelated bits
  R> not R@ @ and			\ invert bitmask and maskout new bits in current value
  or r> !				\ apply value and store back
;
: width-to-mask ( width -- mask )	\ covert a bitfield width to a mask
  #1 swap lshift #1 - 1-foldable
;
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
  addr-offset-width-to-mask-addr bis!
;
: bfc! ( addr addr-offset bit-offset width -- )		\ clear all bitfield bits
  addr-offset-width-to-mask-addr bic!
;
: bf! ( u addr addr-offset bit-offset width -- )	\ store value in bitfield
  addr-offset-width-to-mask-addr masked!
;
: bf@ ( addr addr-offset bit-offset width -- u )	\ fetch value from bitfield
  addr-offset-width-to-mask-addr masked@
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
  width-to-mask swap lshift swap drop
  3-foldable
 ;

: h.
  base @ >R hex . R> base !
;
