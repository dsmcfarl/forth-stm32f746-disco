SOURCES=memmap.fs \
	bitfields.fs \
	common.fs \
	sysclk.fs \
	systick.fs \
	gpio.fs \
	colormap.fs \
	colors.fs \
	graphics-rk043fn48h.fs \
	graphics-text.fs \
	graphics-geometry.fs \
	graphics.fs \
	mco.fs

RCAS=./rcas
UPLOAD_DELAY=15

ram:
	./reset
	cat $(SOURCES) | $(RCAS) > upload.fs
	printf "init\n" >> upload.fs
	./upload
	sleep $(UPLOAD_DELAY)

flash:
	./eraseflash
	printf "compiletoflash\n" > upload.fs
	cat $(SOURCES) | $(RCAS) >> upload.fs
	printf "compiletoram\n" >> upload.fs
	./upload
	sleep $(UPLOAD_DELAY)
	./reset

memmap.fs bitfields.fs: registers.txt gen-cmsis STM32F7x6.svd
	./gen-cmsis

colormap.fs: x11-256-colors.json gen-colors
	./gen-colors

clean:
	-rm -f bitfields.fs memmap.fs colormap.fs upload.fs

rom-swd:
	st-flash erase
	st-flash write rom/mecrisp-stellaris-stm32f746-swd.bin 0x08000000
rom:
	st-flash erase
	st-flash write rom/mecrisp-stellaris-stm32f746.bin 0x08000000

.PHONY: default clean flash rom rom-swd ram
