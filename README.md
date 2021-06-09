# forth-stm32f746-disco
Example code in Forth for STM32F746 Discovery Kit.

This is a work in progress, mostly for my own experimentation and learning.
If using any of this elsewhere you will most likely have to customize to suit
your needs. It is a non-goal to make this a general purpose library or to make
gen-cmsis a general purpose tool.

## Prerequisites
* swdcom from https://github.com/Crest/swdcom with the following patch that
  fixes an issue where reset sometimes hangs the MCU.
  swdcom is only necessary for some of the Makefile optimization. The code
  itself does not depend on any swdcom features.

    diff --git a/swd2.c b/swd2.c
    index b9f435d..67580f5 100644
    --- a/swd2.c
    +++ b/swd2.c
    @@ -610,6 +610,7 @@ main(int argc, char *argv[])
                            if ( stlink_reset(handle) ) {
                                    die("Failed to reset target.");
                            }
    +                       usleep(1000);
                            if ( stlink_run(handle) ) {
                                    die("Failed to resume target.");
                            }

* mecrisp-stellaris from http://mecrisp.sourceforge.net/. I use the 2.5.8
  version for the stm32f746-ra with the following modifications:
  - terminal.s replaced by the version from swdcom
  - --defsym color=1 assembler flag added to the  Makefile for color output
  - the following patch to mecrisp-stelaris-stm32f746.s for swdcom support

<code><pre>
        diff --git a/mecrisp-stellaris-source/stm32f746-ra/mecrisp-stellaris-stm32f746.s b/mecrisp-stellaris-source/stm32f746-ra/mecrisp-stellaris-stm32f746.s
        index ba9e81c..0005166 100644
        --- a/mecrisp-stellaris-source/stm32f746-ra/mecrisp-stellaris-stm32f746.s
        +++ b/mecrisp-stellaris-source/stm32f746-ra/mecrisp-stellaris-stm32f746.s
        @@ -70,10 +70,10 @@ Reset: @ Einsprung zu Beginn
         @ -----------------------------------------------------------------------------
            @ Initialisierungen der Hardware, habe und brauche noch keinen Datenstack dafür
            @ Initialisations for Terminal hardware, without Datastack.
        -   bl uart_init
         
            @ Catch the pointers for Flash dictionary
            .include "../common/catchflashpointers.s"
        +   bl uart_init
         
            welcome " for STM32F746 by Matthias Koch"
</pre></code>

  - a \x07 BEL symbol added to common/datastackandmacros.s to beep at errors:

<code><pre>
        diff --git a/mecrisp-stellaris-source/common/datastackandmacros.s b/mecrisp-stellaris-source/common/datastackandmacros.s
        index 8259620..a1c6a8b 100644
        --- a/mecrisp-stellaris-source/common/datastackandmacros.s
        +++ b/mecrisp-stellaris-source/common/datastackandmacros.s
        @@ -382,7 +382,7 @@ psp .req r7
           bl dotgaensefuesschen
                 .byte 8f - 7f         @ Compute length of name field.
         .ifdef color
        -7:    .ascii "\x1B[31m\Meldung\x1B[0m\n"
        +7:    .ascii "\x1B[31m\Meldung\x1B[0m\x07\n"
         .else
         7:    .ascii "\Meldung\n"
         .endif
        @@ -399,7 +399,7 @@ psp .req r7
           bl dotgaensefuesschen
                 .byte 8f - 7f         @ Compute length of name field.
         .ifdef color
        -7:    .ascii "\x1B[31m\Meldung\x1B[0m\n"
        +7:    .ascii "\x1B[31m\Meldung\x1B[0m\x07\n"
         .else
         7:    .ascii "\Meldung\n"
         .endif
</pre></code>

* A STM32F746 Discovery Kit board.
* st-flash from https://github.com/stlink-org/stlink
* pip3 from https://pip.pypa.io/en/stable/installing/

## Development
Flash a prebuilt mecrisp-stellaris kernel to the target (make sure swd2 is not
running):
  
  make rom-swdcom

Start swdcom in another terminal:

  swd2

Clean working directory:

  make clean

Build:

  make

This builds the generated code, creates two files: upload-ram.fs and
upload-flash.fs, and uploads them to the target MCU. If any of the dependent
forth source files are modified and make is ran again, it will rebuild. If you
reset the target, anything uploaded to RAM is lost but the make build system
won't know it needs re-uploaded, so you must run "make ram" to force it to
rebuild/upload to RAM.

## gen-cmsis
gen-cmsis is a python3 script inspired by Terry Porter's svd2forth. It parses
the STM32F7x6.svd file and generates memmap.fs and bitfields.fs.

These files provide constants for register addresses and words for each bitfield
within those registers. gen-cmsis only generates code for registers listed in
registers.txt. The bitfield words are very simple and just put a triplet of (1)
register address offset from peripheral base address, (2) bitfield bit offset,
and (3) bit width on the stack.  Words defined in common.fs are used to
manipulate the bitfields based on these triplets.

I think this strategy should be efficent in use as long as the words that
manipulate the bitfield triplets and convert them to masks and addresses, etc.
as required are defined as foldable (maybe inline??). I'm new to all of this so
this may prove to be wrong.

The goal of gen-cmsis is to be simple and make the generated code as
non-opinionated as possible about how it is to be used. The generated code
should just provide the raw data about the registers and bitfields, then the
code in common.fs can be tailored as desired to actually manipulate the
bitfields.

To add more generated registers, just add them to registers.txt and run make
again.

gen-cmsis uses the cmsis-svd python package from
https://github.com/posborne/cmsis-svd under the hood. It will automatically
install it if not installed already. pip3 must be installed for the automatic
installation to work.

## Example Usage

<code><pre>
  \ print current value of the RCC_PLLCFGR_PLLN bitfield:
  RCC PLLCFGR_PLLN bf. 192  ok.
  \ set RCC_PLLCFGR_PLLN bitfield to 216:
   #216 RCC PLLCFGR_PLLN bf!  ok.
  \ fetch the current value:
  RCC PLLCFGR_PLLN bf@  ok.
  \ set all bits:
  RCC PLLCFGR_PLLN bfs!  ok.
  \ clear all bits:
  RCC PLLCFGR_PLLN bfc!  ok.
  \ shift value into bitfield position and mask
  #216 PLLCFGR_PLLN bf<<
</pre></code>

There are other variations available. See common.fs for details.

## Style Rules
I don't follow this strictly as it is still evolving since I do not have too
much experience with Forth. I do think some kind of consistent style is critical
to a project of non-trivial complexity since Forth is naturally so unstructured
and flexible:

* Constants, variables, and buffers must be captialized, underscore separated;
  all other definitions must be lower case, dash separated. 
* Word names should be chosen for readability. They may contain multiple english
  words separated by "-". The order of the english words should be the order
  they would normally be spoken. Avoid prefixes to group sets of words.
  e.g., prefer "write-byte-to-i2c1" instead of "i2c1-write-byte".
* Prefix words that are only used in the local file with an underscore, I refer
  to these as "local words".
* Local words do not have to be globally unique as the most recently defined
  version of the word at the time it is used in a definition will be used even
  if it is redefined later.
* Local words can be shorter since their use should be obvious from local
  context.
* Prefer longer words to comments. i.e., if making a word longer will eliminate
  the need for a comment explaining the meaning of the word, then make it
  longer.
* Prefer defining short descriptive local words to adding comments at the end
  of each line in a definition.
* All words that fetch a value must be suffixed with a "@".
* All word that store a value must be suffixed with a "!".
* Use "$", "%", and "#" to prefix all literal numbers except 0 to avoid amiguity
  about base.
* Dinstinct functionality should be split into separate files.
* Each file must have a comment at the top explaining the purpose of the file.
* Source file namess must be suffixed with ".fs".
* Use appropriate #require directives (e4thcom specific) at the top of any file
  using words from other files.
* When defining multiple constants, variables, etc. in a row, vertically align
  the defining words (e.g., constant).
* Vertically align inline comments whenever practical. Prefer starting the
  comments at column 41.
* Use tabs to line up inline comments because it makes them less sensitive to
  minor adjustments.
* Try to keep line lengths less than 80 columns, but don't sacrifice readability
  just to keep it below 80; always keep them less than 120 columns.
* Define local words directly above global words that use them with no blank
  lines between definitions.
* Put one blank line or EOF after each global definition and each "major" local
  definition.
* Short definitions (especially local words) should be on one line unless
  splitting into multiple lines increases readability.
* The first line of multi-line definitions must only have ":" followed by the
  word name and a stack comment and it must not be indented.
* The last line of a multi-line definition must only contain ";" and it must not
  be indented.
* All lines except the first and last of a multi-line definition must be
  indented with two spaces.
* TODO: stack comments
