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
gen-cmsis is a python3 script that parses the STM32F746.svd file and generates
memmap.fs and bitfields.fs. The goal of it is to be simple and make the
generated code as non-opinionated as possible about how it is to be used. The
generated code should just provide the raw data about the registers and
bitfields, then the code in common.fs can be tailored as desired without
regenerating any code.

I think it should still be efficent in use as long as the words that manipulate
the bitfield triplets are and convert them to masks and addresses as required
are defined as foldable (maybe inline??). I'm new to all of this so this may
prove to be wrong.

These files provide constants for register addresses
and words for each bitfield within those registers. It only generates code for
registers listed in registers.txt. The bitfield words are very simple and just
put a triplet of: bitfield bit offset, bit width, and register address; on the
stack.  Words defined in common.fs are used then to to manipulate the bitfields
based on these triplets.

To add more generated registers, just add them to registers.txt and run make
again.

gen-cmsis uses the cmsis-svd python package from
https://github.com/posborne/cmsis-svd under the hood. It will automatically
install it if not installed already. pip3 must be installed for the automatic
installation to work.

## Example Usage

  \ show current value of the RCC_PLLCFGR_PLLN bitfield:
  RCC_PLLCFGR_PLLN bf. 192  ok.
  \ set RCC_PLLCFGR_PLLN bitfield to 216:
   #216 RCC_PLLCFGR_PLLN bf!  ok.
  \ show the updated value:
  RCC_PLLCFGR_PLLN bf. 216  ok.

There are other variations available. See common.fs for details.
