using System;
using System.Diagnostics;
using System.Threading;
using Opto22.OptoMMP3;

namespace SdkSnapPacScannerDemo
{
	class SnapPacSbScannerDemo
	{
		/// <summary>
		/// scan states
		/// </summary>
		public enum DeviceScanState
		{
			Configuring = 1,
			Scanning,
			Fault
		}

		/// <summary>
		/// contains information for a single station/device
		/// </summary>
		public class Station
		{
			#region Informational
			public DeviceScanState eState = DeviceScanState.Configuring;
			public Int32 i32FaultDecrementCounter = 0;

			// statistics
			public UInt64 u64CommAttempts = 0;
			public UInt64 u64CommFaults = 0;

			public Int64 i64LastScanTimeMs;
			public String sScanState = "";
			#endregion Informational

			#region Device Configuration
			// diagnostic info
			public OptoMMP.structBrainDiagnosticInformation structDiagInfo = new OptoMMP.structBrainDiagnosticInformation();

			// module inventory
			public UInt32[] u32aryModuleId = new UInt32[16];
			#endregion Device Configuration

			#region IO States
			// values read from station
			public Boolean[] baryDiscreteStates64 = new Boolean[64];
			public Single[] f32aryAnalogValues = new Single[64];
			#endregion IO States

			#region IO Writes
			// to write a discrete output
			public Boolean bPushDiscreteStates = false;
			public Boolean[] baryNewDiscreteOnStates64 = new Boolean[64];
			public Boolean[] baryNewDiscreteOffStates64 = new Boolean[64];

			// initiate a pulse output
			public Boolean bPushPulse = false;
			public Int32 i32PulseIndex = 0;
			public Single f32StartDelaySeconds;
			public Single f32OnTimeSeconds;
			public Single f32OffTimeSeconds;
			public UInt32 u32PulseQuantity;

			// change an analog output value
			public Boolean bPushAnalogOutput = false;
			public Int32 i32AnalogIndexToWrite = 0;
			public Single f32AnalogOutput = (Single)0.0;

			// clear any prior configuration and reset device
			public Boolean bDeviceResetToDefaults = false;
			#endregion IO Writes

			/// <summary>
			/// Constructor
			/// </summary>
			public Station()
			{
			}
		}

		/// <summary>
		/// maximum stations on a string
		/// </summary>
		public const Int32 i32cMaximumSerialDevices = 256;
		/// <summary>
		/// array of station address, implicitly determines the number of devices to scan
		/// </summary>
		internal Byte[] byaryStationAddresses = null;
		/// <summary>
		/// copy of the com port used to communicate
		/// </summary>
		internal String sPort = "";
		/// <summary>
		/// baud rate setting on the com port
		/// </summary>
		internal Int32 i32BaudRate = 230400;
		/// <summary>
		/// time consume scanning all devices
		/// </summary>
		public Int64 i64TotalScanTimeMs = 0;

		/// <summary>
		/// PAC SB Scanner Constructor
		/// </summary>
		public SnapPacSbScannerDemo(String sPort, Int32 i32BaudRate, Byte[] byaryStationAddresses)
		{
			// duplicate an internal copy of the stations
			this.byaryStationAddresses = (Byte[])byaryStationAddresses.Clone();

			// initialize the data array
			oaryDevices = new Station[byaryStationAddresses.Length];
			for (Int32 i = 0; i < oaryDevices.Length; i++)
			{
				oaryDevices[i] = new Station();
			}

			this.i32BaudRate = i32BaudRate;
			this.sPort = sPort;
		}

		/// <summary>
		/// While true, the scanner thread continue to run.
		/// </summary>
		internal Boolean bRunScannerThread = false;
		/// <summary>
		/// Echo flag that the scanner has completed.
		/// </summary>
		internal Boolean bScannerThreadFinished = false;
		/// <summary>
		/// The scanner thread object.
		/// </summary>
		internal Thread threadScanner = null;

		/// <summary>
		/// Start the scanner thread.
		/// </summary>
		public void StartThread()
		{
			if (threadScanner != null)
			{
				return;
			}

			bRunScannerThread = true;
			bScannerThreadFinished = false;
			threadScanner = new Thread(new ThreadStart(ScanThread));
			threadScanner.Name = "SNAP-PAC Scanner Thread";
			threadScanner.Start();
		}

		/// <summary>
		/// Terminate the scanner thread.
		/// </summary>
		public void StopThread()
		{
			if (threadScanner == null)
			{
				return;
			}

			bRunScannerThread = false;

			do
			{
				Thread.Sleep(100);
			} while (bScannerThreadFinished == false);

			Thread.Sleep(1);
			bScannerThreadFinished = false;
			threadScanner = null;
		}

		/// <summary>
		/// Array of devices to scan.
		/// </summary>
		public Station[] oaryDevices = null;

		/// <summary>
		/// Scanner Thread
		/// </summary>
		protected void ScanThread()
		{
			// a single OptoMMP object for the entire serial multidrop COM port
			OptoMMP Mmp = new OptoMMP();

			// open the serial COM port
			Int32 i32OpenResult = Mmp.OpenSerialBinary(sPort, i32BaudRate);
			Int32 i32Result = 0;
			Stopwatch swatch = new Stopwatch();
			Stopwatch totalswatch = new Stopwatch();
			Int32 i32DeviceIndex = 0;

			// set an initial timeout
			Mmp.ReceiveTimeoutMs = Mmp.MinimumRecommendedTimeoutMs;

			// to measure the elapsed time of the scanner
			totalswatch.Reset();
			totalswatch.Start();

			while (bRunScannerThread == true)
			{
				// test communication
				if (Mmp.IsCommunicationOpen == false)
				{
					Thread.Sleep(1000);
					i32Result = Mmp.OpenSerialBinary(sPort, i32BaudRate);
				}

				// set the address
				Mmp.SerialDeviceIndex = byaryStationAddresses[i32DeviceIndex];
				oaryDevices[i32DeviceIndex].sScanState = oaryDevices[i32DeviceIndex].eState.ToString();

				// perform actions based on current operation
				switch (oaryDevices[i32DeviceIndex].eState)
				{
					case DeviceScanState.Configuring:
						// clear the power-up cleared flag
						oaryDevices[i32DeviceIndex].u64CommAttempts++;
						i32Result = Mmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear);
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}

						// diagnostic info
						oaryDevices[i32DeviceIndex].u64CommAttempts++;
						i32Result = Mmp.ReadBrainDiagnosticInformation(out oaryDevices[i32DeviceIndex].structDiagInfo);
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}
						
						// enable SNAP-PAC-SB serial anomaly indication may be read with ReadPACSBCommunicationBlinkcodeFlag()
						i32Result = Mmp.WritePacSBCommunicationBlinkCodeFlag(true);
						if (i32Result < 0)
						{
							break;
						}

						// analog module inventory
						Int32[] i32aryTemp = new Int32[1];
						for (Int32 i = 0; i < oaryDevices[i32DeviceIndex].u32aryModuleId.Length; i++)
						{
							Int64 i64MmpAddress = 0xfffff0c00000 + i * 0x100;
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.ReadInts(i64MmpAddress, 1, i32aryTemp, 0);
							if (i32Result < 0) break;
							oaryDevices[i32DeviceIndex].u32aryModuleId[i] = (UInt32)i32aryTemp[0];
						}
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}


						// analog point/watchdog configuration
						for (Int32 i32Module = 4; i32Module <= 5; i32Module++)
						{
							for (Int32 i32Channel = 0; i32Channel < 2; i32Channel++)
							{
								oaryDevices[i32DeviceIndex].u64CommAttempts++;
								i32Result = Mmp.WriteAnalogPointConfiguration64(i32Module * 4 + i32Channel,
									0xa3, (Single)20.0, (Single)(4.0), (Single)0.0, (Single)0.0, (Single)0.0, true, (Single)4.0, "Point " + i32DeviceIndex.ToString());

								/*
								 * When writing analog point configuration, 
								 */
								if (i32Result < 0)
								{
									oaryDevices[i32DeviceIndex].u64CommFaults++;
								}
							}
						}

						// aiv modules 0-3, default ±10VDC operation, configuration not required
						// aoa-23-isrc 4,5, default 0-20mA operation, configuration not required
						// snap-iac5ma 6,7, default 4-channel discrete inputs, configuration not required
						// snap-oac5ma 8-12, must be configured as discrete outputs
						for (Int32 i32Module = 8; i32Module <= 12; i32Module++)
						{
							for (Int32 i32Channel = 0; i32Channel < 4; i32Channel++)
							{
								oaryDevices[i32DeviceIndex].u64CommAttempts++;
								i32Result = Mmp.WriteDigitalPointConfiguration64(OptoMMP.GetPointNumberFor64(i32Module, i32Channel),
									true, OptoMMP.eDigitalFeature.None, true, false, "Point " + i32DeviceIndex.ToString());
								if (i32Result < 0) break;
							}
							if (i32Result < 0) break;
						}
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}

						// watchdog, 10 seconds
						oaryDevices[i32DeviceIndex].u64CommAttempts++;
						i32Result = Mmp.WriteWatchdogTime(10000);
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}

						// advance the scan state
						oaryDevices[i32DeviceIndex].eState = DeviceScanState.Scanning;

						break;

					case DeviceScanState.Scanning:
						swatch.Reset();
						swatch.Start();

						// read discretes
						oaryDevices[i32DeviceIndex].u64CommAttempts++;
						i32Result = Mmp.ReadDigitalStates64(oaryDevices[i32DeviceIndex].baryDiscreteStates64, 0);
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}

						// read analogs, we could use this but it requests data from points not installed and that causes excessive overhead
						// i32Result = Mmp.ReadAnalogStates64(oaryDevices[i32DeviceIndex].f32aryAnalogValues, 0);
						oaryDevices[i32DeviceIndex].u64CommAttempts++;
						i32Result = Mmp.ReadSingles(0xfffff0600000, 6 * 4, oaryDevices[i32DeviceIndex].f32aryAnalogValues, 0);
						if (i32Result < 0)
						{
							// back off a few iterations and move to error state
							oaryDevices[i32DeviceIndex].u64CommFaults++;
							oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
							break;
						}

						// pulse
						if (oaryDevices[i32DeviceIndex].bPushPulse == true)
						{
							oaryDevices[i32DeviceIndex].bPushPulse = false;
							// configure the pulse
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.ConfigureNPulsesTime(false, oaryDevices[i32DeviceIndex].f32StartDelaySeconds,
								oaryDevices[i32DeviceIndex].f32OnTimeSeconds,
								oaryDevices[i32DeviceIndex].f32OffTimeSeconds,
								oaryDevices[i32DeviceIndex].u32PulseQuantity,
								OptoMMP.IoModel.IoModel64, oaryDevices[i32DeviceIndex].i32PulseIndex);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].u64CommFaults++;
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								break;
							}

							// start the pulse
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.StartPulse(OptoMMP.IoModel.IoModel64, oaryDevices[i32DeviceIndex].i32PulseIndex);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].u64CommFaults++;
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								break;
							}
						}

						if (oaryDevices[i32DeviceIndex].bPushDiscreteStates == true)
						{
							oaryDevices[i32DeviceIndex].bPushDiscreteStates = false;
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.WriteDigitalStatesMoMo64(oaryDevices[i32DeviceIndex].baryNewDiscreteOnStates64, oaryDevices[i32DeviceIndex].baryNewDiscreteOffStates64, 0);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].u64CommFaults++;
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								break;
							}
						}

						// check for analog write requests
						if (oaryDevices[i32DeviceIndex].bPushAnalogOutput)
						{
							oaryDevices[i32DeviceIndex].bPushAnalogOutput = false;
							// this packs a direct contiguous write to the I/O unit without writing all the other points
							Single[] f32aryOutputData = new Single[1];
							f32aryOutputData[0] = oaryDevices[i32DeviceIndex].f32AnalogOutput;

							// our station object is configured so index 0 and 1 are the two channels at module slot 4
							// and index 2 and 3 are at module slot 5
							Int32 i32ModuleIndex = oaryDevices[i32DeviceIndex].i32AnalogIndexToWrite / 2 + 4;
							Int32 i32ModulePointIndex = oaryDevices[i32DeviceIndex].i32AnalogIndexToWrite % 2;
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.WriteAnalogState64(4 * i32ModuleIndex + i32ModulePointIndex,
								OptoMMP.AnalogWriteOptions.EngineeringUnits, oaryDevices[i32DeviceIndex].f32AnalogOutput);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].u64CommFaults++;
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								break;
							}
						}

						// device clear and reset
						if (oaryDevices[i32DeviceIndex].bDeviceResetToDefaults)
						{
							// clear any stored I/O unit configuration in flash
							oaryDevices[i32DeviceIndex].bDeviceResetToDefaults = false;
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.EraseFlash);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].u64CommFaults++;
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								break;
							}

							// this resets the device
							oaryDevices[i32DeviceIndex].u64CommAttempts++;
							i32Result = Mmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.ResetHardware);
							if (i32Result < 0)
							{
								// back off a few iterations and move to error state
								oaryDevices[i32DeviceIndex].i32FaultDecrementCounter = 10;
								oaryDevices[i32DeviceIndex].eState = DeviceScanState.Fault;
								oaryDevices[i32DeviceIndex].u64CommFaults++;
							}
						}

						// update the last scan time consumed
						oaryDevices[i32DeviceIndex].i64LastScanTimeMs = swatch.ElapsedMilliseconds;
						break;


					case DeviceScanState.Fault:
						oaryDevices[i32DeviceIndex].i32FaultDecrementCounter--;
						if (oaryDevices[i32DeviceIndex].i32FaultDecrementCounter <= 0)
						{
							oaryDevices[i32DeviceIndex].eState = DeviceScanState.Configuring;
						}
						break;
				}

				// finished, increment the device index and check for overflow
				i32DeviceIndex++;
				if (i32DeviceIndex >= byaryStationAddresses.Length)
				{
					// record the total scan time
					i64TotalScanTimeMs = totalswatch.ElapsedMilliseconds;

					i32DeviceIndex = 0;

					totalswatch.Reset();
					totalswatch.Start();
				}
			}
			Mmp.Close();
			bScannerThreadFinished = true;
		}
	}
}