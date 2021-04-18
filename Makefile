.PHONY: default clean

default: memmap.fs bitfields.fs

memmap.fs bitfields.fs:
	./gen-cmsis

clean:
	-rm -f bitfields.fs memmap.fs
