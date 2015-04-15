using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using Opto22.OptoMMP3;
using Opto22.StreamsHelper;

namespace StreamingExample
{
	/// <summary>
	/// A class to demonstrate an approach to implementing a I/O unit data streaming session.
	/// You're welcome to create your implementation. Streaming information is documented in
	/// form 1465.
	/// </summary>
	class StreamsExample
	{
		#region Default Streaming Demo
		/// This is a message handler call-back example. See the "TODO" section in the middle of the method.
		/// 
		/// Each time a message arrives, this method is called-back by a background thread.
		/// 
		/// Since this function is called within the scope of a background thread, this function should not block,
		/// nor take an amount of time longer than the rate of incoming UDP stream messages. If this method
		/// is too slow, the incoming UDP queue will fill up and will drop incoming messages.
		public void MyDefaultStreamMessageHandler(StreamMessage NewMessage)
		{
			// items needed to buffer the messsage
			EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
			Byte[] byary = null;

			// get the message object components, receives the source address information, bytes received and the length of data
			NewMessage.Get(out endpoint, out byary);

			// process the message received
			Int16 i16SequenceId;
			Single[] fary = null;
			UInt32[] u32aryCounters;
			UInt64 u64DiscreteStates;
			UInt64 u64OnDigitalLatches;
			UInt64 u64OffDigitalLatches;
			UInt64 u64CounterStates;

			// for the index of digital states and channel numbers, use this equation:
			// index = (module number [0 to 15] * 4 + channel number [0 to 3]).
			// Or, instead use static method OptoMMP.GetModuleAndChannelFromPoint64().

			// process the message
			Boolean bState = OptoMMPStream.ProcessMessageasBrainStream(NewMessage,
				out endpoint, out i16SequenceId,
				out fary, out u32aryCounters,
				out u64DiscreteStates, out u64OnDigitalLatches,
				out u64OffDigitalLatches, out u64CounterStates);

			#region Add code to process the received data
			// print some output and an indication of the result.
			if (bState)
			{
				/*
				 * *** TODO ***
				 * Add your code here, the data is available. The following WriteLine()s are intended as an example. This is an example
				 * when using the "standard" I/O data stream.
				 */
				Console.WriteLine("Source [IP Address:Udp Port] {0} MMP Sequence Id {1}", endpoint.ToString(), i16SequenceId.ToString());
				String sOutput = "u64DigitalStates = 0x" + u64DiscreteStates.ToString("X16") + "\n" +
					"u64OnDigitalLatches = 0x" + u64OnDigitalLatches.ToString("X16") + "\n" +
					"u64OffDigitalLatches = 0x" + u64OffDigitalLatches.ToString("X16") + "\n";
				Console.WriteLine(sOutput);
			}	
			else
			{
				// with UDP this either means a misconfigured Opto MMP device or some other data that
				// happens to arrive on the UDP port (this is possible with UDP and when connected to a large
				// largely unknown network).
				Console.WriteLine("False Return");
			}
			#endregion Add code to process the received data
		}

		/// <summary>
		/// Represents the entry point of this example (like a Main()) for the default streams demo.
		/// </summary>
		/// <param name="args"></param>
		public void DefaultStreamsDemo()
		{
			#region Connect to Opto MMP Device
			String sMyPcIpAddress = "172.22.254.201";
			String sMyMmpDeviceIpAddress = "172.22.50.0";
			Int32 i32MyMmpDeviceUdpPort = 2001;

			// instantiate an OptoMMP object
			OptoMMP Mmp = new OptoMMP();

			// open a UDP connection and issue a power-up-clear to the device
			Int32 i32Result = Mmp.Open(sMyMmpDeviceIpAddress, i32MyMmpDeviceUdpPort, OptoMMP.Connection.UdpIp, 1000, true);
			if (i32Result < 0)
			{
				Console.WriteLine("Connection to {0} failed.", sMyMmpDeviceIpAddress);
				goto NoConfigurationExit;
			}
			#endregion Connect to Opto MMP Device

			#region Configure Stream on OptoMMP Device
			Int32 i32IntervalMs = 500;
			Int32 i32MyPcUdpPortReceivePort = 5001;
			String[] saryIpAddresses = {sMyPcIpAddress};
			UInt32 u32OptoMMPAddress = 0;
			Int32 i32LengthOfStream = 0;

			i32Result = Mmp.WriteBrainStreamingConfiguration(i32IntervalMs, i32MyPcUdpPortReceivePort, saryIpAddresses, u32OptoMMPAddress, i32LengthOfStream);
			if (i32Result < 0)
			{
				Console.WriteLine("Configuring stream on device failed");
				goto NoConfigurationExit;
			}

			Boolean bIsEnabled;
			i32IntervalMs = 0;
			Int32 i32UdpPortDestination = 0;
			u32OptoMMPAddress = 0;
			i32LengthOfStream = 0;
			i32Result = Mmp.ReadBrainStreamingConfiguration(out bIsEnabled, out i32IntervalMs, out i32UdpPortDestination,
													out saryIpAddresses, out u32OptoMMPAddress, out i32LengthOfStream);
			if (i32Result < 0)
			{
				Console.WriteLine("Reading Stream Status Failed");
				goto NoConfigurationExit;
			}

			String sOutput = "Streaming Enabled = " + bIsEnabled.ToString() + "\n" +
				"Interval = " + i32IntervalMs.ToString() + " milliseconds\n" +
				"UDP Port on PC = " + i32UdpPortDestination.ToString() + "\n";
			for(Int32 i = 0; i < saryIpAddresses.Length; i++)
			{
				sOutput = sOutput + "saryIpAddresses[0] = " + saryIpAddresses[i] + "\n";
			}
			sOutput = sOutput + "OptoMMP Address = 0x" + u32OptoMMPAddress.ToString("X8") + " \n" + 
			"Length of Stream = " + i32LengthOfStream.ToString() + "\n";
			Console.WriteLine(sOutput);

			#endregion Configure Stream on OptoMMP Device

			#region Initiating Stream
			// example for the OptoMMP stream
			OptoMMPStream MMPStream = new OptoMMPStream();

			// this configures the MMPStream object to handle a default stream message handler
			MMPStream.Open(i32MyPcUdpPortReceivePort, new OptoMMPStream.ProcessStream(MyDefaultStreamMessageHandler));

			Console.WriteLine("Starting Stream Handling Thread");
			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);
			#endregion Initiating Stream

			#region Receiving Stream Messages
			// a loop to run the background thread for about 15 seconds
			Int32 i32Counter = 0;
			while (i32Counter < 15)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}
			#endregion Receiving Stream Messages

			#region Stopping Stream
			// stop the stream
			Mmp.EnableBrainStreaming(false);
			MMPStream.Stop();
			#endregion Stopping Stream

			#region Starting Another Stream
			Console.WriteLine("Second Starting Thread");
			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);
			#endregion Starting Another Stream

			#region Receiving Stream Messages
			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 15)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}
			#endregion Receiving Stream Messages

			#region Stopping Receiving and Closing
			// stop the stream
			Mmp.EnableBrainStreaming(false);
			MMPStream.Stop();
			MMPStream.Close();
			#endregion Stopping Receiving and Closing

			#region Reopen The Object And Test Reestablishment of Stream
			MMPStream.Open(i32MyPcUdpPortReceivePort, new OptoMMPStream.ProcessStream(MyDefaultStreamMessageHandler));

			Console.WriteLine("Third Restarting Thread");

			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);

			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 10)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}

			MMPStream.Stop();

			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 5)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}

			// stop the stream
			Mmp.EnableBrainStreaming(false);
			#endregion Reopen The Object And Test Reestablishment of Stream

		NoConfigurationExit: ;
		}
		#endregion Default Streaming Demo

		#region Custom Data Streaming Demo
		/// This is a message handler using a custom data area example. See the "TODO" section in the middle of the method.
		/// 
		/// Each time a message arrives, this method is called-back by a background thread.
		/// 
		/// Since this function is called within the scope of a background thread, this function should not block,
		/// nor consume an amount of time longer than the rate of incoming UDP stream messages. If this method
		/// is too slow, the incoming UDP queue will fill up and will drop incoming messages.
		public void MyCustomDataStreamMessageHandler(StreamMessage NewMessage)
		{
			// items needed to buffer the messsage
			EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
			Byte[] byary = null;

			// get the message object components, receives the source address information, bytes received and the length of data
			NewMessage.Get(out endpoint, out byary);


			#region Add your specific code here...
			{
				/*
				 * *** TODO ***
				 * Add your code here to update the specific expected i32ExpectedU32Ints and process the values.
				 */
				// process the message received
				Int16 i16SequenceId;
				UInt32 u32ExpectedMmpAddress;
				Int32 i32ExpectedU32Ints = 4;
				UInt32[] u32aryValues;

				// process the message
				Boolean bState = OptoMMPStream.ProcessMessageasBrainStreamUint32Array(NewMessage,
					out endpoint, out i16SequenceId,
					out u32ExpectedMmpAddress, i32ExpectedU32Ints, out u32aryValues);

				// print some output and an indication of the result.
				if (bState)
				{
					/*
					 * *** TODO ***
					 * Add your code here, the data is available. The following WriteLines
					 * are intended as an example to display the custom configured data.
					 * This is an example when using the "standard" I/O data stream.
					 */
					UInt32 u32Lower32DiscreteStates = u32aryValues[0];
					Single f32FloatScratchpadElement0 = BitConverter.ToSingle(BitConverter.GetBytes(u32aryValues[1]), 0);
					Single f32IctdTempInput = BitConverter.ToSingle(BitConverter.GetBytes(u32aryValues[2]), 0);
					UInt32 u32HighDensityOutput = u32aryValues[3];

					Console.WriteLine("Source [IP Address:Udp Port] {0} MMP Sequence Id {1} OptoMMP Address 0x{2}",
						endpoint.ToString(), i16SequenceId.ToString(), u32ExpectedMmpAddress.ToString("x8"));
					String sOutput = "u32Lower32DiscreteStates = 0x" + u32Lower32DiscreteStates.ToString("X8") + "\n" +
						"f32FloatScratchpadElement0 = " + f32FloatScratchpadElement0.ToString() + "\n" +
						"f32IctdTempInput = " + f32IctdTempInput.ToString() + "\n" +
						"u32HighDensityOutput = 0x" + u32HighDensityOutput.ToString("X8") + "\n";
					Console.WriteLine(sOutput);
				}
				else
				{
					// with UDP this either means a misconfigured Opto MMP device or some other data that
					// happens to arrive on the UDP port (this is possible with UDP and when connected to a large
					// largely unknown network).
					Console.WriteLine("False Return");
				}

			}
			#endregion Add your specific code here...
		}

		/// <summary>
		/// Represents the entry point of this example (like a Main()) for the custom data streams demo.
		/// </summary>
		/// <param name="args"></param>
		public void CustomDataStreamsDemo()
		{
			#region Connect to Opto MMP Device
			String sMyPcIpAddress = "172.22.254.201";
			String sMyMmpDeviceIpAddress = "172.22.50.0";
			Int32 i32MyMmpDeviceUdpPort = 2001;

			// instantiate an OptoMMP object
			OptoMMP Mmp = new OptoMMP();

			// open a UDP connection and issue a power-up-clear to the device
			Int32 i32Result = Mmp.Open(sMyMmpDeviceIpAddress, i32MyMmpDeviceUdpPort, OptoMMP.Connection.UdpIp, 1000, true);
			if (i32Result < 0)
			{
				Console.WriteLine("Connection to {0} failed.", sMyMmpDeviceIpAddress);
				goto NoConfigurationExit;
			}
			#endregion Connect to Opto MMP Device

			#region Configure Stream on OptoMMP Device
			Int32 i32IntervalMs = 500;
			Int32 i32MyPcUdpPortReceivePort = 5001;
			String[] saryIpAddresses = { sMyPcIpAddress };
			UInt32 u32OptoMMPAddress = 0;
			Int32 i32LengthOfStream = 0;
			UInt32[] u32aryOptoMmpCustomAddresses = new UInt32[4];

			// these addresses may be retrieved from PAC Manager or from form 1465.
			u32aryOptoMmpCustomAddresses[0] = 0xf0400004; // the upper 4 bytes are the lower 32 points (due to big endian byte ordering)
			u32aryOptoMmpCustomAddresses[1] = 0xf0d82000; // 32-bit float scratchpad element 0
			u32aryOptoMmpCustomAddresses[2] = 0xf0262000; // ictd temperature probe at module 2, channel 0
			u32aryOptoMmpCustomAddresses[3] = 0xf1808384; // 32-bits of high density output module in position 14

			// write these addresses to the device
			i32Result = Mmp.WriteCustomConfiguration(0, 4, u32aryOptoMmpCustomAddresses, 0);

			i32Result = Mmp.WriteBrainStreamingConfiguration(i32IntervalMs, i32MyPcUdpPortReceivePort, saryIpAddresses, 0xf0d60000, 16);
			if (i32Result < 0)
			{
				Console.WriteLine("Configuring stream on device failed");
				goto NoConfigurationExit;
			}

			Boolean bIsEnabled;
			i32IntervalMs = 0;
			Int32 i32UdpPortDestination = 0;
			u32OptoMMPAddress = 0;
			i32LengthOfStream = 0;
			i32Result = Mmp.ReadBrainStreamingConfiguration(out bIsEnabled, out i32IntervalMs, out i32UdpPortDestination,
													out saryIpAddresses, out u32OptoMMPAddress, out i32LengthOfStream);
			if (i32Result < 0)
			{
				Console.WriteLine("Reading Stream Status Failed");
				goto NoConfigurationExit;
			}

			String sOutput = "Streaming Enabled = " + bIsEnabled.ToString() + "\n" +
				"Interval = " + i32IntervalMs.ToString() + " milliseconds\n" +
				"UDP Port on PC = " + i32UdpPortDestination.ToString() + "\n";
			for (Int32 i = 0; i < saryIpAddresses.Length; i++)
			{
				sOutput = sOutput + "saryIpAddresses[" + i +"] = " + saryIpAddresses[i] + "\n";
			}
			sOutput = sOutput + "OptoMMP Address = 0x" + u32OptoMMPAddress.ToString("X8") + " \n" +
			"Length of Stream = " + i32LengthOfStream.ToString() + "\n";
			Console.WriteLine(sOutput);

			#endregion Configure Stream on OptoMMP Device

			#region Initiating Stream
			// example for the OptoMMP stream
			OptoMMPStream MMPStream = new OptoMMPStream();

			// this configures the MMPStream object to handle a default stream message handler
			MMPStream.Open(i32MyPcUdpPortReceivePort, new OptoMMPStream.ProcessStream(MyCustomDataStreamMessageHandler));

			Console.WriteLine("Starting Stream Handling Thread");
			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);
			#endregion Initiating Stream

			#region Receiving Stream Messages
			// a loop to run the background thread for about 15 seconds
			Int32 i32Counter = 0;
			while (i32Counter < 15)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}
			#endregion Receiving Stream Messages

			#region Stopping Stream
			// stop the stream
			Mmp.EnableBrainStreaming(false);
			MMPStream.Stop();
			#endregion Stopping Stream

			#region Starting Another Stream
			Console.WriteLine("Second Starting Thread");
			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);
			#endregion Starting Another Stream

			#region Receiving Stream Messages
			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 15)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}
			#endregion Receiving Stream Messages

			#region Stopping Receiving and Closing
			// stop the stream
			Mmp.EnableBrainStreaming(false);
			MMPStream.Stop();
			MMPStream.Close();
			#endregion Stopping Receiving and Closing

			#region Reopen The Object And Test Reestablishment of Stream
			MMPStream.Open(i32MyPcUdpPortReceivePort, new OptoMMPStream.ProcessStream(MyCustomDataStreamMessageHandler));

			Console.WriteLine("Third Restarting Thread");

			// this starts the background thread and initiates the application to being receiving messages
			MMPStream.Start();
			Mmp.EnableBrainStreaming(true);

			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 10)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}

			MMPStream.Stop();

			// a loop to run the background thread for about 15 seconds
			i32Counter = 0;
			while (i32Counter < 5)
			{
				Thread.Sleep(1000);
				i32Counter++;
			}

			// stop the stream
			Mmp.EnableBrainStreaming(false);
			#endregion Reopen The Object And Test Reestablishment of Stream

		NoConfigurationExit: ;
		}
		#endregion Custom Data Streaming Demo
	}
}
