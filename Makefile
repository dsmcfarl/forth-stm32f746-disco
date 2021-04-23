.PHONY: default clean

default: memmap.fs bitfields.fs

memmap.fs bitfields.fs: registers.txt gen-cmsis STM32F7x6.svd
	./gen-cmsis

clean:
	-rm -f bitfields.fs memmap.fs
