Imports System.Threading
Imports Opto22.OptoMMP3


Module Module1

	Dim sTestOptoMMPDevice As String = "10.192.54.46"

	Sub Main()

		Dim Mmp As OptoMMP = New OptoMMP()
		Dim i32Result As Int32

		' open the MMP Object
		Console.WriteLine("Opening Local OptoMMP Object")
		i32Result = Mmp.Open(sTestOptoMMPDevice, 2001, OptoMMP.Connection.TcpIp, 1000, True)
		If (i32Result <> 0) Then
			Console.WriteLine("Mmp.Open() failed i32Result is {0}", i32Result)
		End If

		' change the timeout
		Dim i32OldTimeoutMs As Int32 = Mmp.ReceiveTimeoutMs = 3000

		' set the cache freshness
		Mmp.ReadCacheFreshnessMs = 100

		' retrieve the current read cache fresheness
		Dim i32CacheFreshness As Int32 = Mmp.ReadCacheFreshnessMs

		' open the connection
		If (Mmp.IsCommunicationOpen) Then
			Console.WriteLine("Communication path appears open.")
		Else
			Console.WriteLine("Communication path appears closed.")
		End If

		' clear the power up flag
		Mmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear)

		' write the watchdog time
		i32Result = Mmp.WriteWatchdogTime(5000)

		Dim i32WatchdogTimeMs As Int32 = 0
		i32Result = Mmp.ReadWatchdogTime(i32WatchdogTimeMs)
		If (i32Result = 0) Then
			Console.WriteLine("Watchdog Time Set To {0} Milliseconds", i32WatchdogTimeMs)
		End If

		' date time functions
		' read date/time
		Dim sDateTime As String = ""
		i32Result = Mmp.ReadDateTime(sDateTime)
		If (i32Result < 0) Then
			Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString())
		Else
			Console.WriteLine("IO Date Time Is {0}", sDateTime)
		End If

		Console.WriteLine("Testing closing and reopening of object")
		Mmp.Close()
		i32Result = Mmp.Open(sTestOptoMMPDevice, 2001, OptoMMP.Connection.TcpIp, 1000, True)
		If (i32Result <> 0) Then
			Console.WriteLine("Mmp.Open() failed i32Result is {0}", i32Result)
		End If

		Dim sForceTime As String = "2009-12-08 12:01:01.00"
		Console.WriteLine("Forcing The IO Time to {0}", sForceTime)
		i32Result = Mmp.WriteDateTime(sForceTime)
		If (i32Result < 0) Then
			Console.WriteLine("WriteDateTime Returned Error {0}", i32Result.ToString())
		End If

		i32Result = Mmp.ReadDateTime(sDateTime)
		If (i32Result < 0) Then
			Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString())
		Else
			Console.WriteLine("IO Date Time Is {0}", sDateTime)
		End If

		Thread.Sleep(1000)

		Console.WriteLine("Setting Time to Local")
		i32Result = Mmp.WriteLocalDateTime()
		If (i32Result < 0) Then
			Console.WriteLine("WriteLocalDateTime Returned Error {0}", i32Result.ToString())
		End If

		i32Result = Mmp.ReadDateTime(sDateTime)
		If (i32Result < 0) Then
			Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString())
		Else
			Console.WriteLine("IO Date Time Is {0}", sDateTime)
		End If

		' note, enabling this test stops the analog and digital scanners of the PAC-R
		' this has adverse affects on testing digital, analog, and PID functions in this code
		' it is left here in the event someone needs an example for it
		' scanner flags
		' i32Result = Mmp.WriteScannerFlags(&H7)
		' If (i32Result < 0) Then
		'	Console.WriteLine("WriteScannerFlags Returned Error {0}", i32Result.ToString())
		' End If

		' Dim i32ScannerFlags As Int32
		' i32Result = Mmp.ReadScannerFlags(i32ScannerFlags)
		' If (i32Result < 0) Then
		' 	Console.WriteLine("ReadScannerFlags Returned Error {0}", i32Result.ToString())
		' Else
		' 	Console.WriteLine("ReadScannerFlags Reports {0}", i32ScannerFlags.ToString("x"))
		' End If

		' freshness tests
		Console.WriteLine("Default Freshness is {0}", Mmp.ReadCacheFreshnessMs().ToString())

		' disable read caching/freshness
		Mmp.ReadCacheFreshnessMs = 0

		Console.WriteLine("Disabled Freshness is {0}", Mmp.ReadCacheFreshnessMs().ToString())

		''''''''''''''''''''/
		' Raw Read/Write Tests
		''''''''''''''''''''/
		' ints
		Dim i32ary(1024) As Int32

		Dim i32Const As Int32 = 12345
		i32ary(10) = 12345
		i32Result = Mmp.WriteInts(&HFFFFF0D81000, 1, i32ary, 10)
		i32ary(10) = -1
		i32Result = Mmp.ReadInts(&HFFFFF0D81000, 1, i32ary, 10)
		If (i32ary(10) <> i32Const) Then
			Console.WriteLine("Raw Functions WriteInts and ReadInts failed. i32Result = {0}", i32Result)
		Else
			Console.WriteLine("Raw I32 Functions Test Passed")
		End If

		' singles
		Dim f32ary(1024) As Single
		Dim f32Const As Single = 3.1415
		f32ary(900) = f32Const

		i32Result = Mmp.WriteSingles(&HFFFFF0D82060, 1, f32ary, 900)
		f32ary(900) = 0.0
		i32Result = Mmp.ReadSingles(&HFFFFF0D82060, 1, f32ary, 900)
		If (f32ary(900) <> f32Const) Then
			Console.WriteLine("Raw Functions WriteSingles and ReadSingles failed. i32Result = {0}", i32Result)
		Else
			Console.WriteLine("Raw F32 Functions Test Passed")
		End If

		' longs
		Dim i64ary(512) As Long
		Dim i64Const As Long = &HF121235CAD2124
		i64ary(255) = i64Const

		i32Result = Mmp.WriteLongs(&HFFFFF0DE0140, 1, i64ary, 255)
		i64ary(255) = 0
		i32Result = Mmp.ReadLongs(&HFFFFF0DE0140, 1, i64ary, 255)
		If (i64ary(255) <> i64Const) Then
			Console.WriteLine("Raw Functions WriteLong and ReadLong failed. i32Result = {0}", i32Result)
		Else
			Console.WriteLine("Raw I64 Functions Test Passed")
		End If

		''''''''''''''''''''/
		' Configure Learning Center with some extra modules for demonstration
		' 64-point configuration
		' WriteDigitalPointConfiguration64(int, bool, eType, bool,bool)
		' WriteAnalogPointConfiguration4096(int,int)
		''''''''''''''''''''/
		' module 0 is a SNAP-IDC5 (digital input), not necessary for 4-channel digital inputs or high-density digital
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 0), False, OptoMMP.eDigitalFeature.None, False, False, "Mod0_Pt0")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (0,0) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 1), False, OptoMMP.eDigitalFeature.None, False, False, "Mod0_Pt1")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (0,1) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 2), False, OptoMMP.eDigitalFeature.None, False, False, "Mod0_Pt2")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (0,2) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 3), False, OptoMMP.eDigitalFeature.Counter, False, False, "Mod0_Pt3")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (0,3) returned code {0}", i32Result)
		End If

		' module 1 is a SNAP-ODC5 (digital output), required for 4-channel digital outputs
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 0), True, OptoMMP.eDigitalFeature.None, False, False, "Mod1_Pt0")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (1,0) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 1), True, OptoMMP.eDigitalFeature.None, False, False, "Mod1_Pt1")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (1,1) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 2), True, OptoMMP.eDigitalFeature.None, False, False, "Mod1_Pt2")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (1,2) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 3), True, OptoMMP.eDigitalFeature.None, False, False, "Mod1_Pt3")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (1,3) returned code {0}", i32Result)
		End If

		' module 5 is a SNAP-AITM-8 (high-density analog input), the "5" second parameter is a J-Type thermocouple
		' TC's do not require scaling, they are populated with zeroes for hi and lo scale.
		' this module is not provided with the learning center
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 0), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,0) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 1), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,1) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 2), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,2) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 3), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,3) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 4), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,4) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 5), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,5) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 6), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,6) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 7), 5)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (5,7) returned code {0}", i32Result)
		End If

		' configure a PID on the learning center
		Dim pid As OptoMMP.stPid

		' initialize the PID algorithm structure
		' configures the ICTD/Resistive heater probe on the Learning Center
		pid.eAlgorithm = OptoMMP.ePidAlgorithm.VelocityC
		pid.f32GainParameter = (-3.0)
		pid.f32IntegralParameter = 6.01
		pid.f32DerivativeParameter = 0.0
		pid.bInManualMode = True
		pid.f32PidScanTimeSeconds = 1.0
		pid.u32InputSourceOptoMmpAddress = &HF0600030UI	' ICTD input OPTOMMP address on Learning Center
		pid.u32OutputDestinationOptoMmpAddress = &HF0700024UI	' Second analog output on SNAP-AOV-27
		pid.f32InputLowRange = 0.0
		pid.f32InputHighRange = 120.0 ' just in case Fahrenheit is used
		pid.f32OutputLowerClamp = (0.0)
		pid.f32OutputUpperClamp = 10.0

		' this pushes the data of the entire pid structure to the I/O unit
		i32Result = Mmp.WritePid(0, pid, OptoMMP.ePidWriteOptions.InitializeAll)
		If (i32Result < 0) Then
			Console.WriteLine("Failed to Write Updated PID configuration at PID index 0")
		End If


		' switch pid to automatic mode
		pid.bInManualMode = False
		i32Result = Mmp.WritePid(0, pid, OptoMMP.ePidWriteOptions.PidAutoManualMode)
		If (i32Result < 0) Then
			Console.WriteLine("Failed to set PID to automatic mode.")
		End If

		' module 8 is a SNAP-AILC-2 (64-point model) this module is not provided by the learning center
		i32Result = Mmp.WriteAnalogLoadCellConfiguration64(OptoMMP.GetPointNumberFor64(8, 0), 500, 750)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (8,0 Load Cell AILC-2) returned code {0}", i32Result)
		End If

		Dim u32FastSettle As UInt32
		Dim u32FilterWeight As UInt32
		i32Result = Mmp.ReadAnalogLoadCellConfiguration64(OptoMMP.GetPointNumberFor64(8, 0), u32FastSettle, u32FilterWeight)
		If (u32FastSettle <> 500 Or u32FilterWeight <> 750) Then
			Console.WriteLine("Configure point read WriteAnalogLoadCellConfiguration64 (8,0 Load Cell AILC-2) mismatches")
		Else
			Console.WriteLine("Configure point 64 (8, 0 Load Cell AILC-2) succeeded")
		End If

		' module 8 is a SNAP-AILC-2 (4096-point model)  this module is not provided by the learning center
		i32Result = Mmp.WriteAnalogLoadCellConfiguration4096(OptoMMP.GetPointNumberFor4096(8, 0), 1000, 2000)
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (8,0 Load Cell -2) returned code {0}", i32Result)
		End If

		i32Result = Mmp.ReadAnalogLoadCellConfiguration4096(OptoMMP.GetPointNumberFor4096(8, 0), u32FastSettle, u32FilterWeight)
		If (u32FastSettle <> 1000 Or u32FilterWeight <> 2000) Then
			Console.WriteLine("Configure point read ReadAnalogLoadCellConfiguration4096 (8,0 Load Cell AILC-2) mismatches")
		Else
			Console.WriteLine("Configure point 4096 (8, 0 Load Cell AILC-2) succeeded")
		End If

		' module 15 is a SNAP-IDC5 (digital input), not necessary for 4-channel digital inputs or high-density digital
		' this module is not provided by the learning center
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 0), False, OptoMMP.eDigitalFeature.Counter, False, False, "Mod15_Pt3")
		If (i32Result < 0) Then '
			Console.WriteLine("Configure point (15,0) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 1), False, OptoMMP.eDigitalFeature.Counter, False, False, "Mod15_Pt3")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (15,1) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 2), False, OptoMMP.eDigitalFeature.Counter, False, False, "Mod15_Pt3")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (15,2) returned code {0}", i32Result)
		End If
		i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 3), False, OptoMMP.eDigitalFeature.Counter, False, False, "Mod15_Pt3")
		If (i32Result < 0) Then
			Console.WriteLine("Configure point (15,3) returned code {0}", i32Result)
		End If

		''''''''''''''''''''/
		' Analog Read Tests
		'
		' ReadAnalogState64
		' ReadAnalogStates64
		''''''''''''''''''''/
		Dim f32Ictd As Single = 0.0

		' set temp range to C
		Console.WriteLine("Setting Temp Range to C")
		i32Result = Mmp.WriteFOrCStatus(False)

		Dim bIsFahrenheit As Boolean
		i32Result = Mmp.ReadFOrCStatus(bIsFahrenheit)
		If (Not bIsFahrenheit) Then
			Console.WriteLine("Brain set to report temperature in C.")
		Else
			Console.WriteLine("Brain set to report temperature in F.")
		End If
		Thread.Sleep(3000)

		' by point number
		i32Result = Mmp.ReadAnalogState64(12, f32Ictd)

		If (i32Result < 0) Then
			Console.WriteLine("ReadAnalogState64 (Pointnumber) returned code {0}", i32Result)
		Else
			Console.WriteLine("Pointnumber ICTD Temperature is {0} C", f32Ictd.ToString())
		End If

		' by module and channel
		i32Result = Mmp.ReadAnalogState64(OptoMMP.GetPointNumberFor64(3, 0), f32Ictd)

		If (i32Result < 0) Then
			Console.WriteLine("ReadAnalogState64 (Module-Channel) returned code {0}", i32Result)
		Else
			Console.WriteLine("Module-Channel ICTD Temperature is {0} C", f32Ictd.ToString())
		End If

		' by array
		ReDim f32ary(64)
		i32Result = Mmp.ReadAnalogStates64(f32ary, 0)
		If (i32Result < 0) Then
			Console.WriteLine("ReadAnalogStates64 returned code {0}", i32Result)
		Else
			Console.WriteLine("Analog Bank ICTD Temperature is {0} C", f32ary(12).ToString())
		End If

		' by point number
		Dim f32Tc As Single = 0.0
		i32Result = Mmp.ReadAnalogEu512(160, f32Tc)
		If (i32Result < 0) Then
			Console.WriteLine("ReadAnalogEu512 returned code {0}", i32Result)
		Else
			Console.WriteLine("Analog Point J Thermocouple Temperature is {0} C", f32Tc.ToString())
		End If

		' set temp range to F
		Console.WriteLine("Setting Temp Range to F")
		Mmp.WriteFOrCStatus(True)
		Thread.Sleep(3000) ' allows brain to rescan an update the temp value

		' by bank
		Dim f32aryValues(512) As Single
		i32Result = Mmp.ReadAnalogEus512(f32aryValues, 0)
		If (i32Result < 0) Then
			Console.WriteLine("ReadAnalogEu512 returned code {0}", i32Result)
		Else
			Console.WriteLine("Analog Point J Thermocouple Temperature is {0} F", f32aryValues(160).ToString())
		End If

		Console.WriteLine("Analog 64-Point Max-Min Tests")
		Dim fMax As Single
		Dim fMin As Single
		i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), False, fMax, fMin)
		If (i32Result < 0) Then
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read) returned code {0}", i32Result)
		Else
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString())
		End If

		i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), True, fMax, fMin)
		If (i32Result < 0) Then
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read-clear) returned code {0}", i32Result)
		Else
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read-clear) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString())
		End If
		i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), False, fMax, fMin)
		If (i32Result < 0) Then
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read) returned code {0}", i32Result)
		Else
			Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString())
		End If

		''''''''''''''''''''/
		' Analog Write Tests
		'
		''''''''''''''''''''/
		Console.WriteLine("Analog Write Test, 64-Point Model")

		For i As Int32 = 0 To 9 Step 1
			i32Result = Mmp.WriteAnalogState64(OptoMMP.GetPointNumberFor64(2, 0), OptoMMP.AnalogWriteOptions.EngineeringUnits, i)
			If (i32Result < 0) Then
				Console.WriteLine("WriteAnalogState64 returned code {0}", i32Result)
			End If
			Thread.Sleep(500)
		Next

		Console.WriteLine("Analog Write Test, 512-Point Model")

		For i As Int32 = 10 To 0 Step -1
			i32Result = Mmp.WriteAnalogEu512(OptoMMP.GetPointNumberFor512(2, 0), i)
			If (i32Result < 0) Then
				Console.WriteLine("WriteAnalogState64 returned code {0}", i32Result)
			End If
			Thread.Sleep(500)
		Next

		Dim f32Array(74) As Single
		For i As Int32 = 0 To f32Array.Length - 1 Step 1
			f32Array(i) = 0.0
		Next

		Console.WriteLine("Analog Write Test, 64-Point Model, Bank")
		For i As Int32 = 0 To 9 Step 1
			f32Array(OptoMMP.GetPointNumberFor64(2, 0) + 10) = i
			i32Result = Mmp.WriteAnalogStates64(f32Array, True, 10)
			If (i32Result < 0) Then
				Console.WriteLine("WriteAnalogStates64 returned code {0}", i32Result)
			End If
			Thread.Sleep(500)
		Next

		''''''''''''''''''''/
		' Digital Read Tests
		'
		' ReadDigitalState64
		' ReadDigitalStates64
		''''''''''''''''''''/
		Console.WriteLine("Digital State and Latch Read Test")
		Dim u32Counts As UInt32

		' i32Result = Mmp.ReadOptionallyClearCounter64(3, true, u32Counts)
		' If (i32Result < 0) Then
		'        {
		'	Console.WriteLine("ReadOptionallyClearCounter64 (read-clear) returned code {0}", i32Result)
		' End If

		' digital state test
		For i As Int32 = 0 To 19 Step 1
			Dim bState3 As Boolean = False
			i32Result = Mmp.ReadDigitalState64(63, bState3)
			If (i32Result < 0) Then
				Console.WriteLine("ReadDigitalState64 (point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadDigitalState64 (point 63) {0}", bState3)
			End If

			Dim bOnLatch As Boolean = False
			Dim bOffLatch As Boolean = False
			i32Result = Mmp.ReadDigitalLatch64(63, bOnLatch, bOffLatch)
			If (i32Result < 0) Then
				Console.WriteLine("ReadDigitalLatch64 (latches for point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadDigitalLatch64 (on-latch) {0} (off-latch) {1}", bOnLatch, bOffLatch)
			End If

			i32Result = Mmp.ReadClearDigitalLatch64(63, True, bOnLatch)
			If (i32Result < 0) Then
				Console.WriteLine("ReadClearDigitalLatch64 (on-latch point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadClearDigitalLatch64 (on-latch point 63) {0}", bOnLatch)
			End If

			i32Result = Mmp.ReadClearDigitalLatch64(63, False, bOffLatch)
			If (i32Result < 0) Then
				Console.WriteLine("ReadClearDigitalLatch64 (off-latch point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadClearDigitalLatch64 (off-latch point 63) {0}", bOffLatch)
			End If

			i32Result = Mmp.ReadOptionallyClearCounter64(63, False, u32Counts)
			If (i32Result < 0) Then
				Console.WriteLine("ReadOptionallyClearCounter64 (read point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadOptionallyClearCounter64 (read point 63) {0}", u32Counts)
			End If

			i32Result = Mmp.ReadOptionallyClearCounter64(63, True, u32Counts)
			If (i32Result < 0) Then
				Console.WriteLine("ReadOptionallyClearCounter64 (read-clear point 63) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadOptionallyClearCounter64 (read-clear point 63) {0}", u32Counts)
			End If

			' so I can see the change
			Thread.Sleep(1000)
		Next

		Console.WriteLine("Digital Bank and Latch Read Test")
		Dim u32aryCounts(64) As UInt32

		i32Result = Mmp.ReadOptionallyClearCounters64(True, u32aryCounts, 0)
		If (i32Result < 0) Then
			Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) returned code {0}", i32Result)
		End If

		' digital bank test
		Dim i64OnMask As Long
		Dim i64OffMask As Long
		For i As Int32 = 0 To 19 Step 1
			Dim i64Mask As Long = 0
			i32Result = Mmp.ReadDigitalStates64(i64Mask)
			If (i32Result < 0) Then
				Console.WriteLine("ReadDigitalStates64 (bank) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadDigitalStates64 (bank) {0}", i64Mask.ToString("x"))
			End If

			i32Result = Mmp.ReadDigitalLatches64(i64OnMask, i64OffMask)
			If (i32Result < 0) Then
				Console.WriteLine("ReadDigitalLatches64 (bank) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadDigitalLatches64 (on-latches) {0} (off-latches) {1}", i64OnMask.ToString("x"), i64OffMask.ToString("x"))
			End If

			i32Result = Mmp.ReadClearDigitalLatches64(True, i64OnMask)
			If (i32Result < 0) Then
				Console.WriteLine("ReadClearDigitalLatches64 (bank) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadClearDigitalLatches64 (on-latches) {0}", i64OnMask.ToString("x"))
			End If

			i32Result = Mmp.ReadClearDigitalLatches64(False, i64OffMask)
			If (i32Result < 0) Then
				Console.WriteLine("ReadClearDigitalLatches64 (bank) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadClearDigitalLatches64 (off-latches) {0}", i64OffMask.ToString("x"))
			End If
			i32Result = Mmp.ReadOptionallyClearCounters64(False, u32aryCounts, 0)
			If (i32Result < 0) Then
				Console.WriteLine("ReadOptionallyClearCounters64 (read) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadOptionallyClearCounters64 (read) reports {0}", u32aryCounts(3))
			End If
			i32Result = Mmp.ReadOptionallyClearCounters64(True, u32aryCounts, 0)
			If (i32Result < 0) Then
				Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) returned code {0}", i32Result)
			Else
				Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) reports {0}", u32aryCounts(3))
			End If

			' so I can see the change
			Thread.Sleep(1000)
		Next

		''''''''''''''''''''/
		' Digital Write Tests
		''''''''''''''''''''/
		Console.WriteLine("Digital Point/Bank Write Test")
		' digital state test
		For i As Int32 = 0 To 2 Step 1
			i32Result = Mmp.WriteDigitalState64((i Mod 3) + 5, True)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalState64 (turn on point) returned code {0}", i32Result)
			End If
			Thread.Sleep(1000)
		Next

		For i As Int32 = 0 To 2 Step 1
			i32Result = Mmp.WriteDigitalState64((i Mod 3) + 5, False)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalState64 (turn off point) returned code {0}", i32Result)
			End If
			Thread.Sleep(1000)
		Next

		Console.WriteLine("Digital Bank Write Test")
		For i As Int32 = 0 To 17 Step 1
			Dim i64BitMask As Long

			i64BitMask = &H1 << ((i Mod 3) + 5)
			i32Result = Mmp.WriteDigitalStates64(i64BitMask)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalStates64 returned code {0}", i32Result)
			End If
			Thread.Sleep(1000)
		Next

		''''''''''''''''''''/
		' Digital MOMO Tests
		''''''''''''''''''''/
		Console.WriteLine("Digital 64-Point MOMO Bit Tests")
		i64OnMask = 0
		i64OffMask = Not 0

		' turn off all points
		i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask)
		If (i32Result < 0) Then
			Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result)
		End If

		' turn points on
		i64OffMask = 0
		For i As Int32 = 0 To 62 Step 1
			i64OnMask = &H1 << i
			i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result)
			End If
			Thread.Sleep(50)
		Next

		' turn points off
		i64OnMask = 0
		For i As Int32 = 0 To 62 Step 1
			i64OffMask = &H1 << i
			i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result)
			End If
			Thread.Sleep(50)
		Next

		Console.WriteLine("Digital 64-Point MOMO Bit Tests")
		Dim baryOn(74) As Boolean
		Dim baryOff(74) As Boolean
		For j As Int32 = 10 To 73
			baryOn(j) = False
			baryOff(j) = True
		Next

		' turn off the outputs
		i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10)
		If (i32Result < 0) Then
			Console.WriteLine("WriteDigitalStatesMoMo64(bool(),bool(),int) returned code {0}", i32Result)
		End If

		' turn points on
		For i As Int32 = 0 To 62 Step 1
			For j As Int32 = 10 To 73 Step 1
				baryOn(j) = False
				baryOff(j) = False
			Next

			baryOn(i + 10) = True
			i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalStatesMoMo64(bool(),bool(),int) returned code {0}", i32Result)
			End If
			Thread.Sleep(50)
		Next

		' turn points off
		For i As Int32 = 0 To 62 Step 1
			For j As Int32 = 10 To 73 Step 1
				baryOn(j) = False
				baryOff(j) = False
			Next
			baryOff(i + 10) = True

			i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10)
			If (i32Result < 0) Then
				Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result)
			End If
			Thread.Sleep(50)
		Next

		''''''''''''''''''''/
		' Caching Tests
		''''''''''''''''''''/
		ReDim i32ary(Mmp.ScratchPadI32NumberofElements() - 1)
		Console.WriteLine("Starting Integer Scratchpad Caching Tests")

		For i As Int32 = 0 To i32ary.Length - 1 Step 1
			i32ary(i) = i * 6
		Next
		i32Result = Mmp.ScratchpadI32Write(i32ary, 0, i32ary.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("Error writing {0}", i32Result)
		End If

		For k As Int32 = 0 To 9 Step 1
			Dim i64StartTicks As Long
			Dim i64StopTicks As Long

			' disable caching
			Mmp.ReadCacheFreshnessMs = 0

			' uncached section
			i64StartTicks = DateTime.UtcNow.Ticks
			For i As Int32 = 0 To i32ary.Length - 1 Step 1
				i32Result = Mmp.ScratchpadI32Read(i32ary, i, 1, i)
			Next
			i64StopTicks = DateTime.UtcNow.Ticks

			Console.WriteLine("Uncached took {0} milliseconds", (i64StopTicks - i64StartTicks) / 10000)

			' force the cache freshness
			Mmp.ReadCacheFreshnessMs = 50

			i64StartTicks = DateTime.UtcNow.Ticks
			For i As Int32 = 0 To i32ary.Length - 1 Step 1
				i32Result = Mmp.ScratchpadI32Read(i32ary, i, 1, i)
			Next
			i64StopTicks = DateTime.UtcNow.Ticks

			Console.WriteLine("Cached took {0} milliseconds", (i64StopTicks - i64StartTicks) / 10000)
		Next

		'''''''''''''''''''''
		' Float Scratchpad Array Tests
		' single element tests
		Console.WriteLine("(f32) Single Element Test Started")

		' disable read caching/freshness
		Mmp.ReadCacheFreshnessMs = 0

		For i As Int32 = 0 To Mmp.ScratchPadF32NumberofElements() - 1 Step 1
			Dim SingleArray(1) As Single
			SingleArray(0) = i + 1

			i32Result = Mmp.ScratchpadFloatWrite(SingleArray, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("(f32) Single Element Write Fault i = {0}, i32Result = {1}", i, i32Result)
			End If

			SingleArray(0) = -1.0
			i32Result = Mmp.ScratchpadFloatRead(SingleArray, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("(f32) Single Element Read Fault i = {0}, i32Result = {1}", i, i32Result)
			End If
			If ((i + 1) <> SingleArray(0)) Then
				Console.WriteLine("(f32) Fault At Index {0}", i)
			End If
		Next

		Console.WriteLine("(f32) Single Element Test Passed")


		' maximum block size test
		Console.WriteLine("Maximum (f32) Single Block Test")

		Dim BlockArray(Mmp.ScratchPadF32NumberofElements() - 1) As Single

		For i As Int32 = 0 To BlockArray.Length - 1 Step 1
			BlockArray(i) = BlockArray.Length - i
		Next

		i32Result = Mmp.ScratchpadFloatWrite(BlockArray, 0, BlockArray.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("(f32) Single Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To BlockArray.Length - 1 Step 1
			BlockArray(i) = 0.0
		Next
		i32Result = Mmp.ScratchpadFloatRead(BlockArray, 0, BlockArray.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("(f32) Single Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To BlockArray.Length - 1 Step 1
			If (BlockArray(i) <> BlockArray.Length - i) Then
				Console.WriteLine("(f32) Value Fault in Block Test, Index {0}", i)
			End If
		Next

		Console.WriteLine("Maximum (f32) Single Block Test Passed")


		' smaller block test
		Console.WriteLine("Smaller (f32) Single Block Test")
		ReDim BlockArray(113)

		For j As Int32 = 0 To Mmp.ScratchPadF32NumberofElements() - BlockArray.Length Step 1
			' initialize the values for the write try
			For i As Int32 = 0 To BlockArray.Length - 1 Step 1
				BlockArray(i) = (BlockArray.Length * i)
			Next

			' write the values
			i32Result = Mmp.ScratchpadFloatWrite(BlockArray, 0, BlockArray.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("(f32) Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' reset the values in the array for the readback test
			For i As Int32 = 0 To BlockArray.Length - 1
				BlockArray(i) = (0.0)
			Next

			' try and read back the values
			i32Result = Mmp.ScratchpadFloatRead(BlockArray, 0, BlockArray.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("(f32) Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' inspect the values
			For i As Int32 = 0 To BlockArray.Length - 1 Step 1
				If (BlockArray(i) <> (BlockArray.Length * i)) Then
					Console.WriteLine("(f32) Value Fault in Smaller Block Test, i = {0} j = {1}", i, j)
				End If
			Next
		Next
		Console.WriteLine("Smaller (f32) Single Block Test Passed")


		'''''''''''''''''''''
		' 32-bit integer Scratchpad Array Tests
		Console.WriteLine("32-bit Integer Element Test Started")
		Dim Int32Array(1) As Int32
		For i As Int32 = 0 To Mmp.ScratchPadI32NumberofElements() - 1
			Int32Array(0) = i + 1

			i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("i32 Element Write Fault i = {0}, i32Result = {1}", i, i32Result)
			End If

			Int32Array(0) = -1
			i32Result = Mmp.ScratchpadI32Read(Int32Array, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("i32 Element Read Fault i = {0}, i32Result = {1}", i, i32Result)
			End If
			If ((i + 1) <> Int32Array(0)) Then
				Console.WriteLine("Fault At Index {0}", i)
			End If
		Next
		Console.WriteLine("32-bit Integer Element Test Passed")

		' maximum block size test
		Console.WriteLine("Maximum 32-bit integer block test")
		ReDim Int32Array(Mmp.ScratchPadI32NumberofElements() - 1)

		For i As Int32 = 0 To Int32Array.Length - 1
			Int32Array(i) = Int32Array.Length - i
		Next

		i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, Int32Array.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("i32 Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To Int32Array.Length - 1
			Int32Array(i) = 0
		Next

		i32Result = Mmp.ScratchpadI32Read(Int32Array, 0, Int32Array.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("i32 Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To Int32Array.Length - 1 Step 1
			If (Int32Array(i) <> Int32Array.Length - i) Then
				Console.WriteLine("i32 Value Fault in Block Test, Index {0}", i)
			End If
		Next
		Console.WriteLine("Maximum 32-bit integer Block Test Passed")


		' smaller block test
		Console.WriteLine("Smaller i32 Block Test")
		ReDim Int32Array(113)

		For j As Int32 = 0 To (Mmp.ScratchPadI32NumberofElements() - Int32Array.Length) - 1 Step 1
			' initialize the values for the write try
			For i As Int32 = 0 To Int32Array.Length - 1 Step 1
				Int32Array(i) = Int32Array.Length * i
			Next

			' write the values
			i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, Int32Array.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("i32 Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' reset the values in the array for the readback test
			For i As Int32 = 0 To Int32Array.Length - 1
				Int32Array(i) = 0
			Next

			' try and read back the values
			i32Result = Mmp.ScratchpadI32Read(Int32Array, 0, Int32Array.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("i32 Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' inspect the values
			For i As Int32 = 0 To Int32Array.Length - 1 Step 1
				If (Int32Array(i) <> (Int32Array.Length * i)) Then
					Console.WriteLine("i32 Value Fault in Smaller Block Test, i = {0} j = {1}", i, j)
				End If
			Next
		Next
		Console.WriteLine("Smaller i32 Block Test Passed")


		'''''''''''''''''''''
		' 64-bit integer Scratchpad Array Tests
		Console.WriteLine("64-bit Integer Element Test Started")
		Dim Int64Array(1) As Long
		For i As Int32 = 0 To Mmp.ScratchPadI64NumberofElements() - 1 Step 1
			Int64Array(0) = i + 1

			i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("i64 Element Write Fault i = {0}, i32Result = {1}", i, i32Result)
			End If

			Int64Array(0) = -1
			i32Result = Mmp.ScratchpadI64Read(Int64Array, 0, 1, i)
			If (i32Result <> 0) Then
				Console.WriteLine("i64 Element Read Fault i = {0}, i32Result = {1}", i, i32Result)
			End If
			If ((i + 1) <> Int64Array(0)) Then
				Console.WriteLine("Fault At Index {0}", i)
			End If

		Next
		Console.WriteLine("64-bit Integer Element Test Passed")

		' maximum block size test
		Console.WriteLine("Maximum 64-bit integer block test")
		ReDim Int64Array(Mmp.ScratchPadI64NumberofElements() - 1)

		For i As Int32 = 0 To Int64Array.Length - 1 Step 1
			Int64Array(i) = Int64Array.Length - i
		Next

		i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, Int64Array.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("i64 Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To Int64Array.Length - 1 Step 1
			Int64Array(i) = 0
		Next

		i32Result = Mmp.ScratchpadI64Read(Int64Array, 0, Int64Array.Length, 0)
		If (i32Result <> 0) Then
			Console.WriteLine("i64 Block Write Fault i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To Int64Array.Length - 1 Step 1
			If (Int64Array(i) <> Int64Array.Length - i) Then
				Console.WriteLine("i64 Value Fault in Block Test, Index {0}", i)
			End If
		Next
		Console.WriteLine("Maximum 64-bit integer Block Test Passed")


		' smaller block test
		Console.WriteLine("Smaller i64 Block Test")
		ReDim Int64Array(27)

		For j As Int32 = 0 To (Mmp.ScratchPadI64NumberofElements() - Int64Array.Length) - 1 Step 1
			' initialize the values for the write try
			For i As Int32 = 0 To Int64Array.Length - 1 Step 1
				Int64Array(i) = Int64Array.Length * i
			Next

			' write the values
			i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, Int64Array.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("i64 Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' reset the values in the array for the readback test
			For i As Int32 = 0 To Int64Array.Length - 1 Step 1
				Int64Array(i) = 0
			Next

			' try and read back the values
			i32Result = Mmp.ScratchpadI64Read(Int64Array, 0, Int64Array.Length, j)
			If (i32Result <> 0) Then
				Console.WriteLine("i64 Block Write Fault j = {0} i32Result = {1}", j, i32Result)
			End If

			' inspect the values
			For i As Int32 = 0 To Int64Array.Length - 1 Step 1
				If (Int64Array(i) <> (Int64Array.Length * i)) Then
					Console.WriteLine("i64Value Fault in Smaller Block Test, i = {0} j = {1}", i, j)
				End If
			Next
		Next
		Console.WriteLine("Smaller i64 Block Test Passed")


		'''''''''''''''''''''
		' String Scratchpad Array Tests
		Console.WriteLine("Starting String Scratchpad Tests")

		' create a set of strings
		Dim strary(Mmp.ScratchPadStringNumberofElements() - 1) As String
		For i As Int32 = 0 To strary.Length - 1 Step 1
			Dim i64Value As Long = DateTime.UtcNow.Ticks
			strary(i) = DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x")
			Thread.Sleep(10)
		Next

		Dim straryRead(64) As String
		' write the strings
		i32Result = Mmp.ScratchpadStringWrite(strary, 0, strary.Length, 0)
		If (i32Result < 0) Then
			Console.WriteLine("ScratchpadStringWrite returned error i32Result = {0}", i32Result)
		End If
		i32Result = Mmp.ScratchpadStringRead(straryRead, 0, straryRead.Length - 1, 0)
		If (i32Result < 0) Then
			Console.WriteLine("ScratchpadStringRead returned error i32Result = {0}", i32Result)
		End If

		For i As Int32 = 0 To strary.Length - 1 Step 1
			If (strary(i) <> straryRead(i)) Then
				Console.WriteLine("String element {0} mismatches.", i)
			End If
		Next
		Console.WriteLine("All Finished")

		' close the object
		Mmp.Close()
	End Sub


	Sub HighDensityTests()

		Dim HDInMmp As OptoMMP = New OptoMMP()
		Dim HDOutMmp As OptoMMP = New OptoMMP()
		Dim i32Result As Int32

		' High-Density Tests
		Console.WriteLine("Starting High-Density Tests")

		i32Result = HDInMmp.Open("10.192.50.34", 2001, OptoMMP.Connection.UdpIp, 1000, True)
		If (i32Result <> 0) Then
			Console.WriteLine("HDInMmp.Open() failed i32Result is {0}", i32Result)
		End If

		i32Result = HDOutMmp.Open("10.192.50.35", 2001, OptoMMP.Connection.UdpIp, 1000, True)
		If (i32Result <> 0) Then
			Console.WriteLine("HDOutMmp.Open() failed i32Result is {0}", i32Result)
		End If

		Dim bWrStates(1024) As Boolean
		Dim bRdStates(1024) As Boolean
		Dim i32aryWrMasks(64) As Int32
		Dim i32aryRdMasks(64) As Int32
		Dim u32aryCounters(1024) As UInt32
		Dim baryOnLatches(2048) As Boolean
		Dim baryOffLatches(2048) As Boolean

		' initialize states
		For i As Int32 = 0 To bWrStates.Length - 1 Step 1
			bWrStates(i) = False
		Next
		For i As Int32 = 0 To bRdStates.Length - 1 Step 1
			bRdStates(i) = False
		Next
		For i As Int32 = 0 To i32aryWrMasks.Length - 1 Step 1
			i32aryWrMasks(i) = 0
		Next
		For i As Int32 = 0 To i32aryRdMasks.Length - 1 Step 1
			i32aryRdMasks(i) = 0
		Next


		' reset all outputs, then wait a moment to allow brain and signals to propagate
		i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 10)
		If (i32Result < 0) Then
			Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result)
		End If
		Thread.Sleep(50)

		' reset counters and latches
		i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(True, u32aryCounters, 100)
		If (i32Result < 0) Then
			Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned {0}.", i32Result)
		End If
		i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(True, baryOnLatches, baryOffLatches, 30)
		If (i32Result < 0) Then
			Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned {0}.", i32Result)
		End If

		' check all high-density states
		i32Result = HDOutMmp.ReadHighDensityDigitalStates(bRdStates, 20)
		If (i32Result < 0) Then
			Console.WriteLine("HDOutMmp.ReadHighDensityDigitalStates returned {0}.", i32Result)
		End If

		' check all states
		For i As Int32 = 0 To 511 Step 1
			If (bRdStates(i + 20) <> bWrStates(i + 10)) Then
				Console.WriteLine("(1) Input/Output States Do Not Match")
			End If
		Next

		' check counters and latches
		i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(False, u32aryCounters, 100)
		If (i32Result < 0) Then
			Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result)
		End If

		For i As Int32 = 0 To 511 Step 1
			If (u32aryCounters(i + 100) <> 0) Then
				Console.WriteLine("A counter did not clear, index {0} value {1}.", i, u32aryCounters(i + 100))
			End If
		Next


		i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(False, baryOnLatches, baryOffLatches, 30)
		If (i32Result < 0) Then
			Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result)
		End If


		' toggle one bit at a time, see if read back...
		For i As Int32 = 0 To 511 Step 1
			' set a state
			bWrStates(i + 45) = True

			' reset all outputs, then wait a moment to allow brain and signals to propagate
			i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 45)
			If (i32Result < 0) Then
				Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result)
			End If

			' reset the off state
			bWrStates(i + 45) = False

			Thread.Sleep(75)

			' read the states using boolean functions
			' read all the states, boolean version
			i32Result = HDInMmp.ReadHighDensityDigitalStates(bRdStates, 20)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadHighDensityDigitalStates returned {0}.", i32Result)
			End If

			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If (Not bRdStates(j + 20)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Enable.", i, j)
					End If
				Else
					If (bRdStates(j + 20)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j)
					End If
				End If
			Next

			' read all the states, integer version
			Dim u32aryBitStates(36) As UInt32
			i32Result = HDInMmp.ReadHighDensityDigitalStates(u32aryBitStates, 5)
			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If ((u32aryBitStates(j / 32 + 5) & (&H1 << (j Mod 32))) = 0) Then
						Console.WriteLine("UINT Bank Test, Index i:{0},j:{1}  Did Not Enable.", i, j)
					End If
				Else
					If ((u32aryBitStates(j / 32 + 5) & (&H1 << (j Mod 32))) <> 0) Then
						Console.WriteLine("UINT Bank Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j)
					End If
				End If
			Next

			' single HD digital state
			Dim bState As Boolean

			i32Result = HDInMmp.ReadHighDensityDigitalState(i, bState)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadHighDensityDigitalState returned {0}.", i32Result)
			End If

			If (Not bState) Then
				Console.WriteLine("Expected State to be True.")
			End If


			' single high-density digital latch test
			Dim bOnLatch As Boolean
			Dim bOffLatch As Boolean

			i32Result = HDInMmp.ReadHighDensityDigitalLatch(i / 32, i Mod 32, bOnLatch, bOffLatch)

			If (Not bOnLatch Or bOffLatch) Then
				Console.WriteLine("ReadHighDensityDigitalLatch returned bad value for latch i:{0}.", i)
			End If

			' read the counters
			' check counters and latches
			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(False, u32aryCounters, 100)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result)
			End If

			For j As Int32 = 0 To 511 Step 1
				If (i = j) Then
					If (u32aryCounters(j + 100) <> 1) Then
						Console.WriteLine("HDInMmmp.ReadOptionallyClearHighDensityDigitalCounters32 counter mismatch (one)")
					End If
				Else
					If (u32aryCounters(j + 100) <> 0) Then
						Console.WriteLine("HDInMmmp.ReadOptionallyClearHighDensityDigitalCounters32 counter mismatch (zero) j: {0}", j)
					End If
				End If

				' check counters and latches
				Dim u32Counter As UInt32
				i32Result = HDInMmp.ReadHighDensityDigitalCounter(i / 32, i Mod 32, u32Counter)
				If (i32Result < 0) Then
					Console.WriteLine("HDInMmp.ReadHighDensityDigitalCounter returned ", i32Result)
				End If

				If (u32Counter <> 1) Then
					Console.WriteLine("HDInMmmp.ReadHighDensityDigitalCounter counter mismatch (one)")
				End If
			Next

			' read the latches
			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(False, baryOnLatches, baryOffLatches, 30)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result)
			End If

			' test the values
			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If (Not baryOnLatches(j + 30)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Trigger.", i, j)
					End If
				Else
					If (baryOnLatches(j + 30)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j)
					End If
				End If

				If (j = i And baryOffLatches(j + 30)) Then
					Console.WriteLine("An HD Off-Latch Was Triggered i:{0} j:{1}", i, j)
				End If
			Next

			Dim u32aryOnLatches(68) As UInt32
			Dim u32aryOffLatches(68) As UInt32

			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(False, u32aryOnLatches, u32aryOffLatches, 2)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result)
			End If

			' test the values
			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If ((u32aryOnLatches(j / 32 + 2) And (&H1 << (j Mod 32))) = 0) Then
						Console.WriteLine("UINT Point Test, Index i:{0},j:{1}  Did Not Trigger.", i, j)
					End If
				Else
					If ((u32aryOnLatches(j / 32 + 2) And (&H1 << (j Mod 32))) <> 0) Then
						Console.WriteLine("UINT Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j)
					End If

					If (j = i) Then
						If ((u32aryOffLatches(j / 32 + 2) And (&H1 << (j Mod 32))) <> 0) Then
							Console.WriteLine("UINT An HD Off-Latch Was Triggered i:{0} j:{1}", i, j)
						End If
					End If
				End If
			Next

			' read and clear the latches
			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(True, baryOnLatches, baryOffLatches, 30)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result)
			End If

			' the latch should still be on... test the values
			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If (Not baryOnLatches(j + 30)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1}  Latch not on.", i, j)
					End If
				End If
			Next

			' test the clear function by re-reading the latches
			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(True, baryOnLatches, baryOffLatches, 30)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result)
			End If

			' test the values
			For j As Int32 = 0 To 511 Step 1
				If (j = i) Then
					If (baryOnLatches(j + 20)) Then
						Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Clear.", i, j)
					End If
				End If
			Next

			' reset all outputs, then wait a moment to allow brain and signals to propagate
			i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 45)
			If (i32Result < 0) Then
				Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result)
			End If
			Thread.Sleep(75)

			' single high-density digital latch test
			i32Result = HDInMmp.ReadHighDensityDigitalLatch(i / 32, i Mod 32, bOnLatch, bOffLatch)

			If (bOnLatch Or Not bOffLatch) Then
				Console.WriteLine("ReadHighDensityDigitalLatch returned bad value for off-latch (should be on) i:{0}.", i)
			End If

			' clear counters
			i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(True, u32aryCounters, 100)
			If (i32Result < 0) Then
				Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result)
			End If
		Next
	End Sub

End Module
