
\ ********************** PER File **********************

?EXISTS DL.STAT SWAP DROP .IF 1 DL.STAT .THEN
_END NEW $$$.RUN FORGET $$$.RUN
GetPersistentTop  **Robot.Strategy**
PersistentMAKECHECK

\ ********************** CRN File **********************

_END NEW $$$.RUN
: FILENAME  ." Robot.Strategy " ;
1 0 $VAR *_HSV_SEMA 
1024 0 $VAR *_HSV_TEMP 
200 0 $VAR *_HSV_INIT_IO 
0 IVAR ^_HNV_INIT_IO 
0 TASK  &_INIT_IO
0 TASK &Powerup
0 IVAR ^PowerOn
128 0 $VAR *Result 
128 0 $VAR *TestString 

nullMMPBOARD $FFFFFFFE $FFFFFFFE 0 32768 1.000000 0.010000 0.000000 2001 $7F000001 0 11 BOARD.MMP %ScratchPad
: 0_0
5 JUMP ;
: 0_13

1 LINE.NUM
  %ScratchPad 
  0  
  *TestString   IO.SP.$READ
  ^PowerOn @! 
3 JUMP ;
: 0_15

1 LINE.NUM
  " Paul is a dummy" 
 
  *Result   $CAT
3 JUMP ;
: 0_18

1 LINE.NUM
  " Mike is a dummy" 
 
  *Result   $CAT
2 JUMP ;
: 0_22
1 JUMP ;
: 0_1
TRUE

1 LINE.NUM
  *TestString
 
  " x" 
   $=
LAND
IF -4 ELSE -3 THEN JUMP ;
T: T0
DUMMY
0_0
0_13
0_15
0_18
0_22
0_1
T;
&Powerup ' T0 SETTASK
CREATE T.ARRAY
&Powerup ,
 0 ,
CREATE V.ARRAY
^PowerOn ,
*Result ,
*TestString ,
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
%ScratchPad ,
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
$0000000000000000.. %ScratchPad ENABLES!
" %ScratchPad  (1/1)" *_HSV_INIT_IO $MOVE 0 ^_HNV_INIT_IO @!
%ScratchPad ENABLE
 " Initializing variables" *_HSV_INIT_IO $MOVE
0 ^PowerOn @!
" "
 *Result $MOVE
" Test123"
 *TestString $MOVE
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
: DATESTAMP ." 06/26/15 " ;
: TIMESTAMP ." 09:36:27 " ;
: CRCSTAMP  ." CB03F31D5234EF3A4418450363D96AC2 " ;
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
