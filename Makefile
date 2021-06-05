.PHONY: default clean

default: memmap.fs bitfields.fs colormap.fs

memmap.fs bitfields.fs: registers.txt gen-cmsis STM32F7x6.svd
	./gen-cmsis

colormap.fs: x11-256-colors.json gen-colors
	./gen-colors

clean:
	-rm -f bitfields.fs memmap.fs colormap.fs

flash-swd:
	st-flash erase
	st-flash write rom/mecrisp-stellaris-stm32f746-swd.bin 0x08000000
flash:
	st-flash erase
	st-flash write rom/mecrisp-stellaris-stm32f746.bin 0x08000000
