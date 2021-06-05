\ TODO: allow setting a log level and use to decide what to show
: log.debug ( c-addr length -- ) cr ." DEBUG: " type ;
: log.debug-append ( c-addr length -- ) type ;
: log.info ( c-addr length -- ) cr ." INFO " type ;
: log.info-append ( c-addr length -- ) type ;
: log.warning ( c-addr length -- ) cr ." WARNING: " type ;
: log.warning-append ( c-addr length -- ) type ;
: u-to-string ( u -- c-addr length ) 0 <# #s #> ;	\ buffer for pictured strings is reused so must use immediately
