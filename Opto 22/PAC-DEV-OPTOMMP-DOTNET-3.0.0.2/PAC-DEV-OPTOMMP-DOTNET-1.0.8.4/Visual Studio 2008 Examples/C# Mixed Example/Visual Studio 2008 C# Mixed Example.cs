using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OptoMMP2;


namespace OptoMMP2dev
{
	class Program
	{


		static void TestStreamingFunctions()
		{
			OptoMMP Mmp = new OptoMMP();

			int i32Result = Mmp.Open("10.192.54.72", 2001, OptoMMP.Connection.UdpIp, 1000, true);

			bool bIsEnabled;
			int i32IntervalMs;
			int i32UdpPortDestination;
			string[] saryIpAddresses = null;
			long i64OptoMMPAddress;
			int i32LengthOfStream;

			i32Result = Mmp.ReadBrainStreamingConfiguration(out bIsEnabled,
				out i32IntervalMs,
				out i32UdpPortDestination,
				out saryIpAddresses,
				out i64OptoMMPAddress,
				out i32LengthOfStream);

			Console.WriteLine("ReadBrainStreamingConfiguration returned {0}", i32Result);

			saryIpAddresses[1] = "10.192.0.33";

			i32Result = Mmp.WriteBrainStreamingConfiguration(
					i32IntervalMs,
					i32UdpPortDestination,
					saryIpAddresses,
					i64OptoMMPAddress,
					i32LengthOfStream);

			Console.WriteLine("WriteBrainStreamingConfiguration returned {0}", i32Result);

			i32Result = Mmp.WriteStrategyStreaming("udp:10.192.0.7:5223", (float)0.1, 0x3, true);
			Console.WriteLine("WriteStrategyStreaming returned {0}", i32Result);
			string sCommHandle;
			float fInterval;
			int i32Bits;
			i32Result = Mmp.ReadStrategyStreaming(out sCommHandle, out fInterval, out i32Bits);
			Console.WriteLine("ReadStrategyStreaming returned {0}", i32Result);

		}

		static void Main(string[] args)
		{
			OptoMMP Mmp = new OptoMMP();
			OptoMMP HDInMmp = new OptoMMP();
			OptoMMP HDOutMmp = new OptoMMP();
			int i32Result;

			//            TestStreamingFunctions();

			// open the MMP objects
			{
				// open the MMP Object
				Console.WriteLine("Opening Local OptoMMP Object");
				i32Result = Mmp.Open("10.192.54.72", 2001, OptoMMP.Connection.UdpIp, 1000, true);
				//                i32Result = Mmp.Open("10.192.54.46", 2001, OptoMMP.Connection.UdpIp, 1000, true);
				//int i32Result = Mmp.Open("10.192.54.46", 2001, OptoMMP.Connection.TcpIp, 1000, true);
				//int i32Result = Mmp.Open("brycer1", 2001, OptoMMP.Connection.TcpIp, 1000, true);
				if (i32Result != 0)
				{
					Console.WriteLine("Mmp.Open() failed i32Result is {0}", i32Result);
				}
			}
			goto skiphdd;

			// change the timeout
			int i32OldTimeoutMs = Mmp.ChangeTimeout(3000);

			// set the cache freshness
			Mmp.SetCacheFreshness(100);

			// retrieve the current read cache fresheness
			int i32CacheFreshness = Mmp.GetCacheFreshness();

			// open the connection
			if (Mmp.IsOpen())
			{
				Console.WriteLine("Communication path appears open.");
			}
			else
			{
				Console.WriteLine("Communication path appears closed.");
			}

			// clear the power up flag
			Mmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear);

			// write the watchdog time
			i32Result = Mmp.WriteWatchdogTime(5000);

			int i32WatchdogTimeMs;
			i32Result = Mmp.ReadWatchdogTime(out i32WatchdogTimeMs);
			if (i32Result == 0)
			{
				Console.WriteLine("Watchdog Time Set To {0} Milliseconds", i32WatchdogTimeMs);
			}

			// close the object
			//            Mmp.Close();


			// High-Density Tests
			/*
			 * ReadHighDensityDigitalStates(bool[], int)
				ReadHighDensityDigitalStates(int[], int)
				ReadHighDensityDigitalState(int(point#), out bool)
				ReadOptionallyClearHighDensityDigitalLatches(bool, bool[], bool[], int)
				ReadOptionallyClearHighDensityDigitalLatches(bool, int[], int[], int)
				ReadOptionallyClearHighDensityDigitalLatches(int, int, bool, bool[], bool[], int); should I implement this?
				ReadOptionallyClearHighDensityDigitalLatches(int, int, bool, int[], int[], int)
				ReadHighDensityDigitalLatch(int, int, bool, bool)
				ReadOptionallyClearHighDensityDigitalCounters32(bool bClear, ref UInt32[] u32aryCounters, int i32StartIndex)
				ReadHighDensityDigitalCounter(int i32Module, int i32Channel, out UInt32 u32Counter)
				WriteHighDensityDigitalStates(int i32StartModule, int i32EndModule, ref bool[] baryStates, int i32StartIndex)
				WriteHighDensityDigitalStates(int i32StartModule, int i32EndModule, ref int[] i32aryStates, int i32StartIndex)
			 */
			{
				Console.WriteLine("Starting High-Density Tests");

				i32Result = HDInMmp.Open("10.192.50.34", 2001, OptoMMP.Connection.UdpIp, 1000, true);
				if (i32Result != 0)
				{
					Console.WriteLine("HDInMmp.Open() failed i32Result is {0}", i32Result);
				}

				i32Result = HDOutMmp.Open("10.192.50.35", 2001, OptoMMP.Connection.UdpIp, 1000, true);
				if (i32Result != 0)
				{
					Console.WriteLine("HDOutMmp.Open() failed i32Result is {0}", i32Result);
				}

				bool[] bWrStates = new bool[1024];
				bool[] bRdStates = new bool[1024];
				int[] i32aryWrMasks = new int[64];
				int[] i32aryRdMasks = new int[64];
				uint[] u32aryCounters = new uint[1024];
				bool[] baryOnLatches = new bool[2048];
				bool[] baryOffLatches = new bool[2048];

				// initialize states
				for (int i = 0; i < bWrStates.Length; i++) bWrStates[i] = false;
				for (int i = 0; i < bRdStates.Length; i++) bRdStates[i] = false;
				for (int i = 0; i < i32aryWrMasks.Length; i++) i32aryWrMasks[i] = 0;
				for (int i = 0; i < i32aryRdMasks.Length; i++) i32aryRdMasks[i] = 0;

				// reset all outputs, then wait a moment to allow brain and signals to propagate
				i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 10);
				if (i32Result < 0)
				{
					Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result);
				}
				Thread.Sleep(50);

				// reset counters and latches
				i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(true, ref u32aryCounters, 100);
				if (i32Result < 0)
				{
					Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned {0}.", i32Result);
				}
				i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(true, ref baryOnLatches, ref baryOffLatches, 30);
				if (i32Result < 0)
				{
					Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned {0}.", i32Result);
				}

				// check all high-density states
				i32Result = HDOutMmp.ReadHighDensityDigitalStates(ref bRdStates, 20);
				if (i32Result < 0)
				{
					Console.WriteLine("HDOutMmp.ReadHighDensityDigitalStates returned {0}.", i32Result);
				}

				// check all states
				for (int i = 0; i < 512; i++)
				{
					if (bRdStates[i + 20] != bWrStates[i + 10])
					{
						Console.WriteLine("(1) Input/Output States Do Not Match");
					}
				}

				// check counters and latches
				i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(false, ref u32aryCounters, 100);
				if (i32Result < 0)
				{
					Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result);
				}

				for (int i = 0; i < 512; i++)
				{
					if (u32aryCounters[i + 100] != 0)
					{
						Console.WriteLine("A counter did not clear, index {0} value {1}.", i, u32aryCounters[i + 100]);
					}
				}

				i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(false, ref baryOnLatches, ref baryOffLatches, 30);
				if (i32Result < 0)
				{
					Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result);
				}


				{
					// toggle one bit at a time, see if read back...
					for (int i = 0; i < 512; i++)
					{
						// set a state
						bWrStates[i + 45] = true;

						// reset all outputs, then wait a moment to allow brain and signals to propagate
						i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 45);
						if (i32Result < 0)
						{
							Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result);
						}

						// reset the off state
						bWrStates[i + 45] = false;

						Thread.Sleep(75);

						{ // read the states using boolean functions
							// read all the states, boolean version
							i32Result = HDInMmp.ReadHighDensityDigitalStates(ref bRdStates, 20);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadHighDensityDigitalStates returned {0}.", i32Result);
							}

							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if (!bRdStates[j + 20])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Enable.", i, j);
									}
								}
								else
								{
									if (bRdStates[j + 20])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j);
									}
								}
							}
						}

						{   // read all the states, integer version
							uint[] u32aryBitStates = new uint[36];
							i32Result = HDInMmp.ReadHighDensityDigitalStates(ref u32aryBitStates, 5);
							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if ((u32aryBitStates[j / 32 + 5] & (0x01 << (j % 32))) == 0)
									{
										Console.WriteLine("UINT Bank Test, Index i:{0},j:{1}  Did Not Enable.", i, j);
									}
								}
								else
								{
									if ((u32aryBitStates[j / 32 + 5] & (0x01 << (j % 32))) != 0)
									{
										Console.WriteLine("UINT Bank Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j);
									}
								}
							}
						}

						{ // single HD digital state
							bool bState;

							i32Result = HDInMmp.ReadHighDensityDigitalState(i, out bState);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadHighDensityDigitalState returned {0}.", i32Result);
							}

							if (!bState)
							{
								Console.WriteLine("Expected State to be True.");
							}

						}


						{ // single high-density digital latch test
							bool bOnLatch, bOffLatch;

							i32Result = HDInMmp.ReadHighDensityDigitalLatch(i / 32, i % 32, out bOnLatch, out bOffLatch);

							if (!bOnLatch || bOffLatch)
							{
								Console.WriteLine("ReadHighDensityDigitalLatch returned bad value for latch i:{0}.", i);
							}
						}

						{ // read the counters
							// check counters and latches
							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(false, ref u32aryCounters, 100);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result);
							}

							for (int j = 0; j < 512; j++)
							{
								if (i == j)
								{
									if (u32aryCounters[j + 100] != 1)
									{
										Console.WriteLine("HDInMmmp.ReadOptionallyClearHighDensityDigitalCounters32 counter mismatch (one)");
									}
								}
								else
								{
									if (u32aryCounters[j + 100] != 0)
									{
										Console.WriteLine("HDInMmmp.ReadOptionallyClearHighDensityDigitalCounters32 counter mismatch (zero) j: {0}", j);
									}
								}
							}

							// check counters and latches
							uint u32Counter;
							i32Result = HDInMmp.ReadHighDensityDigitalCounter(i / 32, i % 32, out u32Counter);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadHighDensityDigitalCounter returned ", i32Result);
							}

							if (u32Counter != 1)
							{
								Console.WriteLine("HDInMmmp.ReadHighDensityDigitalCounter counter mismatch (one)");
							}
						}

						{ // read the latches
							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(false, ref baryOnLatches, ref baryOffLatches, 30);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result);
							}

							// test the values
							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if (!baryOnLatches[j + 30])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Trigger.", i, j);
									}
								}
								else
								{
									if (baryOnLatches[j + 30])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j);
									}
								}

								if (j == i && baryOffLatches[j + 30])
								{
									Console.WriteLine("An HD Off-Latch Was Triggered i:{0} j:{1}", i, j);
								}
							}

							uint[] u32aryOnLatches = new uint[68];
							uint[] u32aryOffLatches = new uint[68];

							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(false, ref u32aryOnLatches, ref u32aryOffLatches, 2);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result);
							}

							// test the values
							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if ((u32aryOnLatches[j / 32 + 2] & (0x01 << (j % 32))) == 0)
									{
										Console.WriteLine("UINT Point Test, Index i:{0},j:{1}  Did Not Trigger.", i, j);
									}
								}
								else
								{
									if ((u32aryOnLatches[j / 32 + 2] & (0x01 << (j % 32))) != 0)
									{
										Console.WriteLine("UINT Point Test, Index i:{0},j:{1} Enable When Should Be Disabled.", i, j);
									}
								}

								if (j == i)
								{
									if ((u32aryOffLatches[j / 32 + 2] & (0x01 << (j % 32))) != 0)
									{
										Console.WriteLine("UINT An HD Off-Latch Was Triggered i:{0} j:{1}", i, j);
									}
								}
							}


							// read and clear the latches
							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(true, ref baryOnLatches, ref baryOffLatches, 30);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result);
							}

							// the latch should still be on... test the values
							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if (!baryOnLatches[j + 30])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1}  Latch not on.", i, j);
									}
								}
							}

							// test the clear function by re-reading the latches
							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalLatches(true, ref baryOnLatches, ref baryOffLatches, 30);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalLatches returned ", i32Result);
							}

							// test the values
							for (int j = 0; j < 512; j++)
							{
								if (j == i)
								{
									if (baryOnLatches[j + 20])
									{
										Console.WriteLine("Point Test, Index i:{0},j:{1}  Did Not Clear.", i, j);
									}
								}
							}
						}

						// reset all outputs, then wait a moment to allow brain and signals to propagate
						i32Result = HDOutMmp.WriteHighDensityDigitalStates(0, 15, bWrStates, 45);
						if (i32Result < 0)
						{
							Console.WriteLine("HDOutMmp.WriteHighDensityDigitalStates returned {0}.", i32Result);
						}

						Thread.Sleep(75);

						{ // single high-density digital latch test
							bool bOnLatch, bOffLatch;

							i32Result = HDInMmp.ReadHighDensityDigitalLatch(i / 32, i % 32, out bOnLatch, out bOffLatch);

							if (bOnLatch || !bOffLatch)
							{
								Console.WriteLine("ReadHighDensityDigitalLatch returned bad value for off-latch (should be on) i:{0}.", i);
							}
						}

						{
							// clear counters
							i32Result = HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32(true, ref u32aryCounters, 100);
							if (i32Result < 0)
							{
								Console.WriteLine("HDInMmp.ReadOptionallyClearHighDensityDigitalCounters32 returned ", i32Result);
							}
						}
					}
				}
			}


			// use to skip the rest of the test
			goto end;
		skiphdd: ;
			// date time functions
			{
				// read date/time
				string sDateTime;
				i32Result = Mmp.ReadDateTime(out sDateTime);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString());
				}
				else
				{
					Console.WriteLine("IO Date Time Is {0}", sDateTime);
				}


				Console.WriteLine("Testing closing and reopening of object");
				Mmp.Close();
				i32Result = Mmp.Open("10.192.54.72", 2001, OptoMMP.Connection.TcpIp, 1000, true);
				if (i32Result != 0)
				{
					Console.WriteLine("Mmp.Open() failed i32Result is {0}", i32Result);
				}


				string sForceTime = "2009-12-08 12:01:01.00";
				Console.WriteLine("Forcing The IO Time to {0}", sForceTime);
				i32Result = Mmp.WriteDateTime(sForceTime);
				if (i32Result < 0)
				{
					Console.WriteLine("WriteDateTime Returned Error {0}", i32Result.ToString());
				}

				i32Result = Mmp.ReadDateTime(out sDateTime);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString());
				}
				else
				{
					Console.WriteLine("IO Date Time Is {0}", sDateTime);
				}

				Thread.Sleep(1000);

				Console.WriteLine("Setting Time to Local");
				i32Result = Mmp.WriteLocalDateTime();
				if (i32Result < 0)
				{
					Console.WriteLine("WriteLocalDateTime Returned Error {0}", i32Result.ToString());
				}

				i32Result = Mmp.ReadDateTime(out sDateTime);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadDateTime Returned Error {0}", i32Result.ToString());
				}
				else
				{
					Console.WriteLine("IO Date Time Is {0}", sDateTime);
				}
			}

			{
				// scanner flags
				i32Result = Mmp.WriteScannerFlags(0x0007);
				if (i32Result < 0)
				{
					Console.WriteLine("WriteScannerFlags Returned Error {0}", i32Result.ToString());
				}

				int i32ScannerFlags;
				i32Result = Mmp.ReadScannerFlags(out i32ScannerFlags);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadScannerFlags Returned Error {0}", i32Result.ToString());
				}
				else
				{
					Console.WriteLine("ReadScannerFlags Reports {0}", i32ScannerFlags.ToString("x"));
				}
			}

			{
				// freshness tests
				Console.WriteLine("Default Freshness is {0}", Mmp.GetCacheFreshness().ToString());

				// disable read caching/freshness
				Mmp.SetCacheFreshness(0);

				Console.WriteLine("Disabled Freshness is {0}", Mmp.GetCacheFreshness().ToString());
			}

			/////////////////////////////////////////
			// Raw Read/Write Tests
			/////////////////////////////////////////
			{
				// ints
				int[] i32ary = new int[1024];
				int i32Const = 12345;
				i32ary[10] = 12345;
				i32Result = Mmp.WriteInts(0xfffff0d81000, 1, i32ary, 10);
				i32ary[10] = -1;
				i32Result = Mmp.ReadInts(0xfffff0d81000, 1, ref i32ary, 10);
				if (i32ary[10] != i32Const)
				{
					Console.WriteLine("Raw Functions WriteInts and ReadInts failed. i32Result = {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Raw I32 Functions Test Passed");
				}
				i32ary = null;

				// singles
				Single[] f32ary = new Single[1024];
				Single f32Const = (Single)3.1415;
				f32ary[900] = f32Const;

				i32Result = Mmp.WriteSingles(0xfffff0d82060, 1, f32ary, 900);
				f32ary[900] = (Single)0.0;
				i32Result = Mmp.ReadSingles(0xfffff0d82060, 1, ref f32ary, 900);
				if (f32ary[900] != f32Const)
				{
					Console.WriteLine("Raw Functions WriteSingles and ReadSingles failed. i32Result = {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Raw F32 Functions Test Passed");
				}
				f32ary = null;

				// longs
				long[] i64ary = new long[512];
				long i64Const = 0xF121235CAD2124;
				i64ary[255] = i64Const;

				i32Result = Mmp.WriteLongs(0xfffff0de0140, 1, i64ary, 255);
				i64ary[255] = 0;
				i32Result = Mmp.ReadLongs(0xfffff0de0140, 1, ref i64ary, 255);
				if (i64ary[255] != i64Const)
				{
					Console.WriteLine("Raw Functions WriteLong and ReadLong failed. i32Result = {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Raw I64 Functions Test Passed");
				}
				i64ary = null;
			}

			/////////////////////////////////////////
			// Configure Learning Center
			// 64-point configuration
			// WriteDigitalPointConfiguration64(int, bool, eType, bool,bool);
			// WriteAnalogPointConfiguration4096(int,int)
			/////////////////////////////////////////
			{
				// module 0 is a SNAP-IDC5 (digital input), not necessary for 4-channel digital inputs or high-density digital
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 0), false, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (0,0) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 1), false, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (0,1) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 2), false, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (0,2) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(0, 3), false, OptoMMP.eDigitalFeature.Counter, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (0,3) returned code {0}", i32Result);

				// module 1 is a SNAP-ODC5 (digital output), required for 4-channel digital outputs
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 0), true, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (1,0) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 1), true, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (1,1) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 2), true, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (1,2) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(1, 3), true, OptoMMP.eDigitalFeature.None, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (1,3) returned code {0}", i32Result);

				// module 5 is a SNAP-AITM-8 (high-density analog input), the 5 second parameter is a J-Type thermocouple
				// TC's do not require scaling, they are populated with zeroes for hi and lo scale.
				// this is not standard with the learning center
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 0), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,0) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 1), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,1) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 2), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,2) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 3), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,3) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 4), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,4) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 5), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,5) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 6), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,6) returned code {0}", i32Result);
				i32Result = Mmp.WriteAnalogPointConfiguration4096(OptoMMP.GetPointNumberFor4096(5, 7), 5);
				if (i32Result < 0) Console.WriteLine("Configure point (5,7) returned code {0}", i32Result);

				// module 8 is a SNAP-AILC-2 (64-point model)
				{
					i32Result = Mmp.WriteAnalogLoadCellConfiguration64(OptoMMP.GetPointNumberFor64(8, 0), 500, 750);
					if (i32Result < 0) Console.WriteLine("Configure point (8,0 Load Cell AILC-2) returned code {0}", i32Result);

					uint u32FastSettle;
					uint u32FilterWeight;
					i32Result = Mmp.ReadAnalogLoadCellConfiguration64(OptoMMP.GetPointNumberFor64(8, 0), out u32FastSettle, out u32FilterWeight);
					if (u32FastSettle != 500 || u32FilterWeight != 750) Console.WriteLine("Configure point read WriteAnalogLoadCellConfiguration64 (8,0 Load Cell AILC-2) mismatches");
					else Console.WriteLine("Configure point 64 (8, 0 Load Cell AILC-2) succeeded");
				}

				// module 8 is a SNAP-AILC-2 (4096-point model)
				{
					i32Result = Mmp.WriteAnalogLoadCellConfiguration4096(OptoMMP.GetPointNumberFor4096(8, 0), 1000, 2000);
					if (i32Result < 0) Console.WriteLine("Configure point (8,0 Load Cell -2) returned code {0}", i32Result);

					uint u32FastSettle;
					uint u32FilterWeight;
					i32Result = Mmp.ReadAnalogLoadCellConfiguration4096(OptoMMP.GetPointNumberFor4096(8, 0), out u32FastSettle, out u32FilterWeight);
					if (u32FastSettle != 1000 || u32FilterWeight != 2000) Console.WriteLine("Configure point read ReadAnalogLoadCellConfiguration4096 (8,0 Load Cell AILC-2) mismatches");
					else Console.WriteLine("Configure point 4096 (8, 0 Load Cell AILC-2) succeeded");
				}

				// module 15 is a SNAP-IDC5 (digital input), not necessary for 4-channel digital inputs or high-density digital
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 3), false, OptoMMP.eDigitalFeature.Counter, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (15,0) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 3), false, OptoMMP.eDigitalFeature.Counter, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (15,1) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 3), false, OptoMMP.eDigitalFeature.Counter, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (15,2) returned code {0}", i32Result);
				i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(15, 3), false, OptoMMP.eDigitalFeature.Counter, false, false);
				if (i32Result < 0) Console.WriteLine("Configure point (15,3) returned code {0}", i32Result);

			}

			/////////////////////////////////////////
			// Analog Read Tests
			//
			// ReadAnalogState64
			// ReadAnalogStates64
			/////////////////////////////////////////
			{
				Single f32Ictd = (Single)0.0;

				// set temp range to C
				Console.WriteLine("Setting Temp Range to C");
				i32Result = Mmp.WriteFOrCStatus(false);

				bool bIsFahrenheit;
				i32Result = Mmp.ReadFOrCStatus(out bIsFahrenheit);
				if (!bIsFahrenheit)
				{
					Console.WriteLine("Brain set to report temperature in C.");
				}
				else
				{
					Console.WriteLine("Brain set to report temperature in F.");
				}

				Thread.Sleep(3000);

				// by point number
				i32Result = Mmp.ReadAnalogState64(12, ref f32Ictd);

				if (i32Result < 0)
				{
					Console.WriteLine("ReadAnalogState64 (Pointnumber) returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Pointnumber ICTD Temperature is {0} C", f32Ictd.ToString());
				}

				// by module and channel
				i32Result = Mmp.ReadAnalogState64(OptoMMP.GetPointNumberFor64(3, 0), ref f32Ictd);

				if (i32Result < 0)
				{
					Console.WriteLine("ReadAnalogState64 (Module-Channel) returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Module-Channel ICTD Temperature is {0} C", f32Ictd.ToString());
				}

				// by array
				Single[] f32ary = new Single[64];
				i32Result = Mmp.ReadAnalogStates64(ref f32ary, 0);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadAnalogStates64 returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Analog Bank ICTD Temperature is {0} C", f32ary[12].ToString());
				}

				// by point number
				Single f32Tc = (Single)0.0;
				i32Result = Mmp.ReadAnalogEu512(160, ref f32Tc);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadAnalogEu512 returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Analog Point J Thermocouple Temperature is {0} C", f32Tc.ToString());
				}

				// set temp range to F
				Console.WriteLine("Setting Temp Range to F");
				Mmp.WriteFOrCStatus(true);
				Thread.Sleep(3000); // allows brain to rescan an update the temp value

				// by bank
				Single[] f32aryValues = new Single[512];
				i32Result = Mmp.ReadAnalogEus512(ref f32aryValues, 0);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadAnalogEu512 returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("Analog Point J Thermocouple Temperature is {0} F", f32aryValues[160].ToString());
				}
			}

			{
				Console.WriteLine("Analog 64-Point Max-Min Tests");
				Single fMax, fMin;
				i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), false, out fMax, out fMin);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read) returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString());
				}

				i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), true, out fMax, out fMin);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read-clear) returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read-clear) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString());
				}
				i32Result = Mmp.ReadOptionallyClearAnalogMaxMin64(OptoMMP.GetPointNumberFor64(3, 0), false, out fMax, out fMin);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 (read) returned code {0}", i32Result);
				}
				else
				{
					Console.WriteLine("ReadOptionallyClearAnalogMaxMin64 Point (read) 3,0 Max {0} Min {1}", fMax.ToString(), fMin.ToString());
				}

			}

			/////////////////////////////////////////
			// Analog Write Tests
			//
			/////////////////////////////////////////
			{
				Console.WriteLine("Analog Write Test, 64-Point Model");

				for (int i = 0; i < 10; i++)
				{
					i32Result = Mmp.WriteAnalogState64(OptoMMP.GetPointNumberFor64(2, 0), OptoMMP.AnalogWriteOptions.EngineeringUnits, (float)i);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteAnalogState64 returned code {0}", i32Result);
					}
					Thread.Sleep(500);
				}

				Console.WriteLine("Analog Write Test, 512-Point Model");

				for (int i = 10; i >= 0; i--)
				{
					i32Result = Mmp.WriteAnalogEu512(OptoMMP.GetPointNumberFor512(2, 0), (float)i);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteAnalogState64 returned code {0}", i32Result);
					}
					Thread.Sleep(500);
				}

				Single[] f32Array = new Single[74];
				for (int i = 0; i < f32Array.Length; i++) f32Array[i] = (Single)0.0;

				Console.WriteLine("Analog Write Test, 64-Point Model, Bank");
				for (int i = 0; i < 10; i++)
				{
					f32Array[OptoMMP.GetPointNumberFor64(2, 0) + 10] = (Single)i;
					i32Result = Mmp.WriteAnalogStates64(f32Array, true, 10);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteAnalogStates64 returned code {0}", i32Result);
					}
					Thread.Sleep(500);
				}
			}

			/////////////////////////////////////////
			// Digital Read Tests
			//
			// ReadDigitalState64
			// ReadDigitalStates64
			/////////////////////////////////////////
			{
				Console.WriteLine("Digital State and Latch Read Test");
				uint u32Counts;
				/*
                i32Result = Mmp.ReadOptionallyClearCounter64(3, true, out u32Counts);
                if (i32Result < 0)
                {
                    Console.WriteLine("ReadOptionallyClearCounter64 (read-clear) returned code {0}", i32Result);
                }
				 */

				// digital state test
				for (int i = 0; i < 20; i++)
				{
					bool bState3 = false;
					i32Result = Mmp.ReadDigitalState64(63, ref bState3);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadDigitalState64 (point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadDigitalState64 (point 63) {0}", bState3 ? "on" : "off");
					}

					bool bOnLatch = false;
					bool bOffLatch = false;
					i32Result = Mmp.ReadDigitalLatch64(63, out bOnLatch, out bOffLatch);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadDigitalLatch64 (latches for point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadDigitalLatch64 (on-latch) {0} (off-latch) {1}", bOnLatch ? "on" : "off", bOffLatch ? "on" : "off");
					}

					i32Result = Mmp.ReadClearDigitalLatch64(63, true, out bOnLatch);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadClearDigitalLatch64 (on-latch point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadClearDigitalLatch64 (on-latch point 63) {0}", bOnLatch ? "on" : "off");
					}

					i32Result = Mmp.ReadClearDigitalLatch64(63, false, out bOffLatch);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadClearDigitalLatch64 (off-latch point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadClearDigitalLatch64 (off-latch point 63) {0}", bOffLatch ? "on" : "off");
					}

					i32Result = Mmp.ReadOptionallyClearCounter64(63, false, out u32Counts);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadOptionallyClearCounter64 (read point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadOptionallyClearCounter64 (read point 63) {0}", u32Counts);
					}

					i32Result = Mmp.ReadOptionallyClearCounter64(63, true, out u32Counts);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadOptionallyClearCounter64 (read-clear point 63) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadOptionallyClearCounter64 (read-clear point 63) {0}", u32Counts);
					}

					// so I can see the change
					Thread.Sleep(1000);
				}

				Console.WriteLine("Digital Bank and Latch Read Test");
				uint[] u32aryCounts = new uint[64];

				i32Result = Mmp.ReadOptionallyClearCounters64(true, ref u32aryCounts, 0);
				if (i32Result < 0)
				{
					Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) returned code {0}", i32Result);
				}

				// digital bank test
				for (int i = 0; i < 20; i++)
				{
					long i64Mask = 0;
					i32Result = Mmp.ReadDigitalStates64(ref i64Mask);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadDigitalStates64 (bank) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadDigitalStates64 (bank) {0}", i64Mask.ToString("x"));
					}

					long i64OnMask, i64OffMask;
					i32Result = Mmp.ReadDigitalLatches64(out i64OnMask, out i64OffMask);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadDigitalLatches64 (bank) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadDigitalLatches64 (on-latches) {0} (off-latches) {1}", i64OnMask.ToString("x"), i64OffMask.ToString("x"));
					}

					i32Result = Mmp.ReadClearDigitalLatches64(true, out i64OnMask);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadClearDigitalLatches64 (bank) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadClearDigitalLatches64 (on-latches) {0}", i64OnMask.ToString("x"));
					}

					i32Result = Mmp.ReadClearDigitalLatches64(false, out i64OffMask);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadClearDigitalLatches64 (bank) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadClearDigitalLatches64 (off-latches) {0}", i64OffMask.ToString("x"));
					}
					i32Result = Mmp.ReadOptionallyClearCounters64(false, ref u32aryCounts, 0);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadOptionallyClearCounters64 (read) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadOptionallyClearCounters64 (read) reports {0}", u32aryCounts[3]);
					}
					i32Result = Mmp.ReadOptionallyClearCounters64(true, ref u32aryCounts, 0);
					if (i32Result < 0)
					{
						Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) returned code {0}", i32Result);
					}
					else
					{
						Console.WriteLine("ReadOptionallyClearCounters64 (read-clear) reports {0}", u32aryCounts[3]);
					}

					// so I can see the change
					Thread.Sleep(1000);
				}
			}

			/////////////////////////////////////////
			// Digital Write Tests
			/////////////////////////////////////////
			{
				Console.WriteLine("Digital Point/Bank Write Test");
				// digital state test
				for (int i = 0; i < 3; i++)
				{
					i32Result = Mmp.WriteDigitalState64((i % 3) + 5, true);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalState64 (turn on point) returned code {0}", i32Result);
					}
					Thread.Sleep(1000);
				}
				for (int i = 0; i < 3; i++)
				{
					i32Result = Mmp.WriteDigitalState64((i % 3) + 5, false);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalState64 (turn off point) returned code {0}", i32Result);
					}
					Thread.Sleep(1000);
				}

				Console.WriteLine("Digital Bank Write Test");
				for (int i = 0; i < 18; i++)
				{
					long i64BitMask;

					i64BitMask = 0x01 << ((i % 3) + 5);
					i32Result = Mmp.WriteDigitalStates64(i64BitMask);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalStates64 returned code {0}", i32Result);
					}
					Thread.Sleep(1000);
				}
			}

			/////////////////////////////////////////
			// Digital MOMO Tests
			/////////////////////////////////////////
			{
				Console.WriteLine("Digital 64-Point MOMO Bit Tests");
				long i64OnMask = 0;
				long i64OffMask = ~0;

				// turn off all points
				i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask);
				if (i32Result < 0)
				{
					Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result);
				}

				// turn points on
				i64OffMask = 0;
				for (int i = 0; i < 63; i++)
				{
					i64OnMask = 0x01 << i;
					i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result);
					}
					Thread.Sleep(50);
				}

				// turn points off
				i64OnMask = 0;
				for (int i = 0; i < 63; i++)
				{
					i64OffMask = 0x01 << i;
					i32Result = Mmp.WriteDigitalStatesMoMo64(i64OnMask, i64OffMask);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result);
					}
					Thread.Sleep(50);
				}
			}
			{
				Console.WriteLine("Digital 64-Point MOMO Bit Tests");
				bool[] baryOn = new bool[74];
				bool[] baryOff = new bool[74];

				for (int j = 10; j < 74; j++)
				{
					baryOn[j] = false;
					baryOff[j] = true;
				}

				// turn off the outputs
				i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10);
				if (i32Result < 0)
				{
					Console.WriteLine("WriteDigitalStatesMoMo64(bool[],bool[],int) returned code {0}", i32Result);
				}

				// turn points on
				for (int i = 0; i < 63; i++)
				{
					for (int j = 10; j < 74; j++)
					{
						baryOn[j] = false;
						baryOff[j] = false;
					}

					baryOn[i + 10] = true;
					i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalStatesMoMo64(bool[],bool[],int) returned code {0}", i32Result);
					}
					Thread.Sleep(50);
				}

				// turn points off
				for (int i = 0; i < 63; i++)
				{
					for (int j = 10; j < 74; j++)
					{
						baryOn[j] = false;
						baryOff[j] = false;
					}
					baryOff[i + 10] = true;
					i32Result = Mmp.WriteDigitalStatesMoMo64(baryOn, baryOff, 10);
					if (i32Result < 0)
					{
						Console.WriteLine("WriteDigitalStatesMoMo64(bool,bool) returned code {0}", i32Result);
					}
					Thread.Sleep(50);
				}

			}

			/////////////////////////////////////////
			// Caching Tests
			/////////////////////////////////////////
			{
				int[] i32ary = new int[Mmp.ScratchPadI32NumberofElements()];
				Console.WriteLine("Starting Integer Scratchpad Caching Tests");

				for (int i = 0; i < i32ary.Length; i++)
				{
					i32ary[i] = i * 6;
				}
				i32Result = Mmp.ScratchpadI32Write(i32ary, 0, i32ary.Length, 0);
				if (i32Result != 0)
				{
					Console.WriteLine("Error writing {0}", i32Result);
				}

				for (int k = 0; k < 10; k++)
				{
					long i64StartTicks, i64StopTicks;

					// disable caching
					Mmp.SetCacheFreshness(0);

					// uncached section
					i64StartTicks = DateTime.UtcNow.Ticks;
					for (int i = 0; i < i32ary.Length; i++)
						i32Result = Mmp.ScratchpadI32Read(ref i32ary, i, 1, i);
					i64StopTicks = DateTime.UtcNow.Ticks;

					Console.WriteLine("Uncached took {0} milliseconds", (i64StopTicks - i64StartTicks) / 10000);

					// force the cache freshness
					Mmp.SetCacheFreshness(50);

					i64StartTicks = DateTime.UtcNow.Ticks;
					for (int i = 0; i < i32ary.Length; i++)
						i32Result = Mmp.ScratchpadI32Read(ref i32ary, i, 1, i);
					i64StopTicks = DateTime.UtcNow.Ticks;

					Console.WriteLine("Cached took {0} milliseconds", (i64StopTicks - i64StartTicks) / 10000);
				}
			}

			//////////////////////////////////////////
			// Float Scratchpad Array Tests
			// single element tests
			{
				Console.WriteLine("(f32) Single Element Test Started");

				// disable read caching/freshness
				Mmp.SetCacheFreshness(0);

				for (int i = 0; i < Mmp.ScratchPadF32NumberofElements(); i++)
				{
					Single[] SingleArray = new Single[1];
					SingleArray[0] = (Single)(i + 1);

					i32Result = Mmp.ScratchpadFloatWrite(SingleArray, 0, 1, i);
					if (i32Result != 0)
					{
						Console.WriteLine("(f32) Single Element Write Fault i = {0}, i32Result = {1}", i, i32Result);

					}
					SingleArray[0] = (Single)(-1.0);
					i32Result = Mmp.ScratchpadFloatRead(ref SingleArray, 0, 1, i);
					if (i32Result != 0)
					{
						Console.WriteLine("(f32) Single Element Read Fault i = {0}, i32Result = {1}", i, i32Result);
					}
					if ((i + 1) != (int)SingleArray[0])
					{
						Console.WriteLine("(f32) Fault At Index {0}", i);
					}
				}
				Console.WriteLine("(f32) Single Element Test Passed");


				// maximum block size test
				Console.WriteLine("Maximum (f32) Single Block Test");
				{
					Single[] BlockArray = new Single[Mmp.ScratchPadF32NumberofElements()];

					for (int i = 0; i < BlockArray.Length; i++) BlockArray[i] = (Single)(BlockArray.Length - i);

					i32Result = Mmp.ScratchpadFloatWrite(BlockArray, 0, BlockArray.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("(f32) Single Block Write Fault i32Result = {0}", i32Result);

					}
					for (int i = 0; i < BlockArray.Length; i++) BlockArray[i] = (Single)(0.0);
					i32Result = Mmp.ScratchpadFloatRead(ref BlockArray, 0, BlockArray.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("(f32) Single Block Write Fault i32Result = {0}", i32Result);
					}

					for (int i = 0; i < BlockArray.Length; i++)
					{
						if (BlockArray[i] != (Single)(BlockArray.Length - i))
						{
							Console.WriteLine("(f32) Value Fault in Block Test, Index {0}", i);
						}
					}
				}
				Console.WriteLine("Maximum (f32) Single Block Test Passed");


				// smaller block test
				Console.WriteLine("Smaller (f32) Single Block Test");
				{
					Single[] BlockArray = new Single[113];

					for (int j = 0; j < (Mmp.ScratchPadF32NumberofElements() - BlockArray.Length); j++)
					{
						// initialize the values for the write try
						for (int i = 0; i < BlockArray.Length; i++) BlockArray[i] = (Single)(BlockArray.Length * i);

						// write the values
						i32Result = Mmp.ScratchpadFloatWrite(BlockArray, 0, BlockArray.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("(f32) Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// reset the values in the array for the readback test
						for (int i = 0; i < BlockArray.Length; i++) BlockArray[i] = (Single)(0.0);

						// try and read back the values
						i32Result = Mmp.ScratchpadFloatRead(ref BlockArray, 0, BlockArray.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("(f32) Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// inspect the values
						for (int i = 0; i < BlockArray.Length; i++)
						{
							if (BlockArray[i] != (Single)(BlockArray.Length * i))
							{
								Console.WriteLine("(f32) Value Fault in Smaller Block Test, i = {0} j = {1}", i, j);
							}
						}
					}
				}
				Console.WriteLine("Smaller (f32) Single Block Test Passed");
			}

			//////////////////////////////////////////
			// 32-bit integer Scratchpad Array Tests
			{
				Console.WriteLine("32-bit Integer Element Test Started");
				for (int i = 0; i < Mmp.ScratchPadI32NumberofElements(); i++)
				{
					int[] Int32Array = new int[1];
					Int32Array[0] = i + 1;

					i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, 1, i);
					if (i32Result != 0)
					{
						Console.WriteLine("i32 Element Write Fault i = {0}, i32Result = {1}", i, i32Result);

					}
					Int32Array[0] = -1;
					i32Result = Mmp.ScratchpadI32Read(ref Int32Array, 0, 1, i);
					if (i32Result != 0)
					{
						Console.WriteLine("i32 Element Read Fault i = {0}, i32Result = {1}", i, i32Result);
					}
					if ((i + 1) != (int)Int32Array[0])
					{
						Console.WriteLine("Fault At Index {0}", i);
					}
				}
				Console.WriteLine("32-bit Integer Element Test Passed");

				// maximum block size test
				Console.WriteLine("Maximum 32-bit integer block test");
				{
					int[] Int32Array = new int[Mmp.ScratchPadI32NumberofElements()];

					for (int i = 0; i < Int32Array.Length; i++) Int32Array[i] = Int32Array.Length - i;

					i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, Int32Array.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("i32 Block Write Fault i32Result = {0}", i32Result);

					}
					for (int i = 0; i < Int32Array.Length; i++) Int32Array[i] = 0;
					i32Result = Mmp.ScratchpadI32Read(ref Int32Array, 0, Int32Array.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("i32 Block Write Fault i32Result = {0}", i32Result);
					}

					for (int i = 0; i < Int32Array.Length; i++)
					{
						if (Int32Array[i] != Int32Array.Length - i)
						{
							Console.WriteLine("i32 Value Fault in Block Test, Index {0}", i);
						}
					}
				}
				Console.WriteLine("Maximum 32-bit integer Block Test Passed");


				// smaller block test
				Console.WriteLine("Smaller i32 Block Test");
				{
					int[] Int32Array = new int[113];

					for (int j = 0; j < (Mmp.ScratchPadI32NumberofElements() - Int32Array.Length); j++)
					{
						// initialize the values for the write try
						for (int i = 0; i < Int32Array.Length; i++) Int32Array[i] = Int32Array.Length * i;

						// write the values
						i32Result = Mmp.ScratchpadI32Write(Int32Array, 0, Int32Array.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("i32 Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// reset the values in the array for the readback test
						for (int i = 0; i < Int32Array.Length; i++) Int32Array[i] = 0;

						// try and read back the values
						i32Result = Mmp.ScratchpadI32Read(ref Int32Array, 0, Int32Array.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("i32 Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// inspect the values
						for (int i = 0; i < Int32Array.Length; i++)
						{
							if (Int32Array[i] != (Single)(Int32Array.Length * i))
							{
								Console.WriteLine("i32 Value Fault in Smaller Block Test, i = {0} j = {1}", i, j);
							}
						}
					}
				}
				Console.WriteLine("Smaller i32 Block Test Passed");
			}

			//////////////////////////////////////////
			// 64-bit integer Scratchpad Array Tests
			{
				Console.WriteLine("64-bit Integer Element Test Started");
				for (int i = 0; i < Mmp.ScratchPadI64NumberofElements(); i++)
				{
					long[] Int64Array = new long[1];
					Int64Array[0] = i + 1;

					i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, 1, (int)i);
					if (i32Result != 0)
					{
						Console.WriteLine("i64 Element Write Fault i = {0}, i32Result = {1}", i, i32Result);

					}
					Int64Array[0] = -1;
					i32Result = Mmp.ScratchpadI64Read(ref Int64Array, 0, 1, (int)i);
					if (i32Result != 0)
					{
						Console.WriteLine("i64 Element Read Fault i = {0}, i32Result = {1}", i, i32Result);
					}
					if ((i + 1) != (int)Int64Array[0])
					{
						Console.WriteLine("Fault At Index {0}", i);
					}
				}
				Console.WriteLine("64-bit Integer Element Test Passed");

				// maximum block size test
				Console.WriteLine("Maximum 64-bit integer block test");
				{
					long[] Int64Array = new long[Mmp.ScratchPadI64NumberofElements()];

					for (int i = 0; i < Int64Array.Length; i++) Int64Array[i] = Int64Array.Length - i;

					i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, Int64Array.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("i64 Block Write Fault i32Result = {0}", i32Result);

					}
					for (int i = 0; i < Int64Array.Length; i++) Int64Array[i] = 0;
					i32Result = Mmp.ScratchpadI64Read(ref Int64Array, 0, Int64Array.Length, 0);
					if (i32Result != 0)
					{
						Console.WriteLine("i64 Block Write Fault i32Result = {0}", i32Result);
					}

					for (int i = 0; i < Int64Array.Length; i++)
					{
						if (Int64Array[i] != Int64Array.Length - i)
						{
							Console.WriteLine("i64 Value Fault in Block Test, Index {0}", i);
						}
					}
				}
				Console.WriteLine("Maximum 64-bit integer Block Test Passed");


				// smaller block test
				Console.WriteLine("Smaller i64 Block Test");
				{
					long[] Int64Array = new long[27];

					for (int j = 0; j < (Mmp.ScratchPadI64NumberofElements() - Int64Array.Length); j++)
					{
						// initialize the values for the write try
						for (int i = 0; i < Int64Array.Length; i++) Int64Array[i] = Int64Array.Length * i;

						// write the values
						i32Result = Mmp.ScratchpadI64Write(Int64Array, 0, Int64Array.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("i64 Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// reset the values in the array for the readback test
						for (int i = 0; i < Int64Array.Length; i++) Int64Array[i] = 0;

						// try and read back the values
						i32Result = Mmp.ScratchpadI64Read(ref Int64Array, 0, Int64Array.Length, j);
						if (i32Result != 0)
						{
							Console.WriteLine("i64 Block Write Fault j = {0} i32Result = {1}", j, i32Result);
						}

						// inspect the values
						for (int i = 0; i < Int64Array.Length; i++)
						{
							if (Int64Array[i] != (Single)(Int64Array.Length * i))
							{
								Console.WriteLine("i64Value Fault in Smaller Block Test, i = {0} j = {1}", i, j);
							}
						}
					}
				}
				Console.WriteLine("Smaller i64 Block Test Passed");
			}

			//////////////////////////////////////////
			// String Scratchpad Array Tests
			{
				Console.WriteLine("Starting String Scratchpad Tests");

				// create a set of strings
				String[] strary = new string[Mmp.ScratchPadStringNumberofElements()];
				for (int i = 0; i < strary.Length; i++)
				{
					long i64Value = DateTime.UtcNow.Ticks;
					strary[i] = DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x") + DateTime.UtcNow.Ticks.ToString("x");
					Thread.Sleep(10);
				}

				String[] straryRead = new string[64];
				// write the strings
				i32Result = Mmp.ScratchpadStringWrite(strary, 0, strary.Length, 0);
				if (i32Result < 0)
				{
					Console.WriteLine("ScratchpadStringWrite returned error i32Result = {1}", i32Result);
				}
				i32Result = Mmp.ScratchpadStringRead(ref straryRead, 0, straryRead.Length, 0);
				if (i32Result < 0)
				{
					Console.WriteLine("ScratchpadStringRead returned error i32Result = {1}", i32Result);
				}

				for (int i = 0; i < strary.Length; i++)
				{
					if (strary[i] != straryRead[i])
					{
						Console.WriteLine("String element {0} mismatches.", i);
					}
				}
			}

		end: ;


			Console.WriteLine("All Done");
			// close the object
			Mmp.Close();
		}



	}
}
