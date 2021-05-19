# forth-stm32f746-disco
Example code in Forth for STM32F746 Discovery Kit.

This is a work in progress, mostly for my own experimentation and learning.
If using any of this elsewhere you will most likely have to customize to suit
your needs. It is a non-goal to make this a general purpose library or to make
gen-cmsis a general purpose tool.

## Prerequisites

* e4thcom from https://wiki.forth-ev.de/doku.php/en:projects:e4thcom. Only
  required because I use e4thcom specific #require words to efficiently load
  code only when it hasn't already been loaded.
* mecrisp-stellaris from http://mecrisp.sourceforge.net/. I use the 2.5.7
  version and the stm32f746-ra build.
* A STM32F746 Discovery Kit board.
* A utility and programmer to flash the mecrisp Stellaris kernel on the board
* pip3 from https://pip.pypa.io/en/stable/installing/

## Development
Clean working directory:

  make clean

Build generated code: 

  make

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
