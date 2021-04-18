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
: offset-width-addr-to-mask-addr ( offset width addr -- mask addr )
  >R width-to-mask swap lshift R>
  3-foldable
 ;

\ Bitfields are represented by a bit offset, bit width, and register address
\ triplet. The bitfield words defined in bitfields.fs leave the
\ corresponding triplet on the stack. The following words perform an action
\ using a bitfield triplet from the stack.

: bfs! ( offset width addr -- )		\ set all bitfield bits
  offset-width-addr-to-mask-addr bis!
;
: bfc! ( offset width addr -- )		\ clear all bitfield bits
  offset-width-addr-to-mask-addr bic!
;
: bf! ( u offset width addr -- )	\ store value in bitfield
  offset-width-addr-to-mask-addr masked!
;
: bf@ ( offset width addr -- u )	\ fetch value from bitfield
  offset-width-addr-to-mask-addr masked@
;
: bf. ( offset width addr -- u )	\ print value from bitfield
  bf@ .
;
: bfh. ( offset width addr -- )		\ print value from bitfield in hex
  base @ >R hex bf. R> base !
;
: bfb. ( offset width addr -- )		\ print value from bitfield in binary
  base @ >R hex bf. R> base !
;

\ TODO: sandbox, remove or move elsewhere
: hse-off RCC_CR_HSEON bfc! ;
: hse-on RCC_CR_HSEON bfs! ;
: reg. ( addr -- ) @ . ;				\ print value stored at register address
: regh. ( addr -- ) base @ >R hex reg. R> base ! ;	\ print value stored at register address in hex
: plln! ( %bbbbbbbbb -- ) RCC_PLLCFGR_PLLN bf! ;
