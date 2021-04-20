#require common.fs
#require clock.fs
#require systick.fs
#require debug.fs
sys-clk-cfg
mco1-cfg
systick-cfg
' systick-handler irq-systick !    \ This 'hooks' the systick-handler word (above) to the systick irq
systick-int-enable
