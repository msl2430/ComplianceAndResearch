
\ ********************** PER File **********************

?EXISTS DL.STAT SWAP DROP .IF 1 DL.STAT .THEN
_END NEW $$$.RUN FORGET $$$.RUN
GetPersistentTop  **JohnDeereGenset.otg**
PersistentMAKECHECK

\ ********************** CRN File **********************

_END NEW $$$.RUN
: FILENAME  ." JohnDeereGenset.otg " ;
1 0 $VAR *_HSV_SEMA 
1024 0 $VAR *_HSV_TEMP 
200 0 $VAR *_HSV_INIT_IO 
0 IVAR ^_HNV_INIT_IO 
0 TASK  &_INIT_IO
0 TASK &Powerup

: 0_0
0 JUMP ;
T: T0
DUMMY
0_0
T;
&Powerup ' T0 SETTASK
CREATE T.ARRAY
&Powerup ,
 0 ,
CREATE V.ARRAY
*_HSV_SEMA ,
*_HSV_TEMP ,
 0 ,
CREATE TI.ARRAY
 0 ,
CREATE PTR.ARRAY
 0 ,
CREATE TA.ARRAY 
 0 ,
CREATE PTRTABLE.ARRAY 
 0 ,
CREATE B.ARRAY
 0 ,
CREATE P.ARRAY
 0 ,
CREATE PID.ARRAY
 0 ,
CREATE E/R.ARRAY
 0 ,
CREATE E/RGROUP.ARRAY
 0 ,
: CONFIG_PORTS
;
: W_INIT_IO
CONFIG_PORTS
 " Initializing variables" *_HSV_INIT_IO $MOVE
 " " *_HSV_INIT_IO $MOVE
;
T: T_INIT_IO
W_INIT_IO
&Powerup START.T DROP
T;
&_INIT_IO ' T_INIT_IO  SETTASK
   : _RUN
   CLEARERRORS
   &_INIT_IO START.T DROP
   ;
: DATESTAMP ." 06/03/15 " ;
: TIMESTAMP ." 12:14:49 " ;
: CRCSTAMP  ." 32A66908D74F5BE4222545F252814BE6 " ;
MAKECHECK
CLEAR.BREAKS

\ ********************** INC File **********************

\ ""DOWNLOAD_COMPRESSION_OFF
MAKECHECK 
0 0 0 BP! 
0 0 1 BP! 
0 0 2 BP! 
0 0 3 BP! 
0 0 4 BP! 
0 0 5 BP! 
0 0 6 BP! 
0 0 7 BP! 
0 0 8 BP! 
0 0 9 BP! 
0 0 10 BP! 
0 0 11 BP! 
0 0 12 BP! 
0 0 13 BP! 
0 0 14 BP! 
0 0 15 BP! 
0 I!AUTORUN 
?EXISTS DL.STAT SWAP DROP .IF 0 DL.STAT .THEN
