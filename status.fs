#require log.fs

#0 constant STATUS_OK
#1 constant STATUS_BUSY
#2 constant STATUS_RXDR_EMPTY
#3 constant STATUS_TXDR_NOT_EMPTY
#4 constant STATUS_NO_STOP
#5 constant STATUS_NACK

\ check status and log warning if not STATUS_OK.
: check-warn ( status c-addr length -- )
  rot dup STATUS_OK <> if -rot log.warning s" : " log.warning-append u-to-string log.warning-append exit then drop drop drop
;

