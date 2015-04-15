using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SdkSnapPacScannerDemo;
using Opto22.OptoMMP3;


namespace IO_Demo
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		SnapPacSbScannerDemo scanner = null;

		/// <summary>
		/// Addresses (stations) to scan. If you had more than one station, add addresses to this array.
		/// </summary>
		Byte[] byarySerialStations = new Byte[] { 4 };

		/// <summary>
		/// Scanning flag
		/// </summary>
		Boolean bScanning = false;

		private void Form1_Load(object sender, EventArgs e)
		{
			// discrete combobox
			comboBoxDiscreteOutput.SelectedIndex = 0;

			// station id combobox
			for (Int32 i = 0; i < byarySerialStations.Length; i++)
			{
				comboBoxStationId.Items.Add((Object)byarySerialStations[i].ToString());
			}
			comboBoxStationId.SelectedIndex = 0;
			
			// analog output combobox
			comboAnalogOutput.SelectedIndex = 0;

			// baud rate combobox
			comboBaudRate.SelectedIndex = 0;

			// serial ports combobox
			String[] saryPorts = System.IO.Ports.SerialPort.GetPortNames();
			if (saryPorts.Length == 0)
			{
				comboComPort.Items.Add((Object)"No Serial Ports Available");
			}
			else
			{
				for (Int32 i = saryPorts.Length - 1; i >= 0; i--)
				{
					comboComPort.Items.Add((Object)saryPorts[i]);
				}
			}
			comboComPort.SelectedIndex = 0;

			// pulse combo box
			comboPulse.SelectedIndex = 0;
		}

		private String AnalogToText64(Int32 i32SerialAddress, Int32 i32ModuleNumber, Int32 i32PointNumber)
		{
			Int32 i32AnalogIndex64 = OptoMMP.GetPointNumberFor64(i32ModuleNumber, i32PointNumber);
			Single f32Value = scanner.oaryDevices[i32SerialAddress].f32aryAnalogValues[i32AnalogIndex64];
			if (Single.IsNaN(f32Value))
			{
				// implies module not scanning, failed, module not reporting data
				return "NAN";
			}
			else
			{
				return f32Value.ToString("N2");
			}
		}

		private void timerGuiUpdate_Tick(object sender, EventArgs e)
		{
			Int32 i32CurrentSerialAddress = comboBoxStationId.SelectedIndex;

			// analog status
			textBox00_00.Text = AnalogToText64(i32CurrentSerialAddress, 0, 0);
			textBox00_01.Text = AnalogToText64(i32CurrentSerialAddress, 0, 1);
			textBox00_02.Text = AnalogToText64(i32CurrentSerialAddress, 0, 2);
			textBox00_03.Text = AnalogToText64(i32CurrentSerialAddress, 0, 3);

			textBox01_00.Text = AnalogToText64(i32CurrentSerialAddress, 1, 0);
			textBox01_01.Text = AnalogToText64(i32CurrentSerialAddress, 1, 1);
			textBox01_02.Text = AnalogToText64(i32CurrentSerialAddress, 1, 2);
			textBox01_03.Text = AnalogToText64(i32CurrentSerialAddress, 1, 3);

			textBox02_00.Text = AnalogToText64(i32CurrentSerialAddress, 2, 0);
			textBox02_01.Text = AnalogToText64(i32CurrentSerialAddress, 2, 1);
			textBox02_02.Text = AnalogToText64(i32CurrentSerialAddress, 2, 2);
			textBox02_03.Text = AnalogToText64(i32CurrentSerialAddress, 2, 3);

			textBox03_00.Text = AnalogToText64(i32CurrentSerialAddress, 3, 0);
			textBox03_01.Text = AnalogToText64(i32CurrentSerialAddress, 3, 1);
			textBox03_02.Text = AnalogToText64(i32CurrentSerialAddress, 3, 2);
			textBox03_03.Text = AnalogToText64(i32CurrentSerialAddress, 3, 3);

			textBox04_00.Text = AnalogToText64(i32CurrentSerialAddress, 4, 0);
			textBox04_01.Text = AnalogToText64(i32CurrentSerialAddress, 4, 1);

			textBox05_00.Text = AnalogToText64(i32CurrentSerialAddress, 5, 0);
			textBox05_01.Text = AnalogToText64(i32CurrentSerialAddress, 5, 1);

			// discrete status, input module at position 6
			checkBox06_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(6, 0)];
			checkBox06_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(6, 1)];
			checkBox06_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(6, 2)];
			checkBox06_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(6, 3)];

			// discrete status, input module at position 7
			checkBox07_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(7, 0)];
			checkBox07_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(7, 1)];
			checkBox07_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(7, 2)];
			checkBox07_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(7, 3)];

			// discrete status, output module at position 8
			checkBox08_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(8, 0)];
			checkBox08_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(8, 1)];
			checkBox08_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(8, 2)];
			checkBox08_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(8, 3)];

			// discrete status, output module at position 9
			checkBox09_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(9, 0)];
			checkBox09_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(9, 1)];
			checkBox09_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(9, 2)];
			checkBox09_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(9, 3)];

			// discrete status, output module at position 10
			checkBox10_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(10, 0)];
			checkBox10_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(10, 1)];
			checkBox10_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(10, 2)];
			checkBox10_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(10, 3)];

			// discrete status, output module at position 11
			checkBox11_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(11, 0)];
			checkBox11_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(11, 1)];
			checkBox11_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(11, 2)];
			checkBox11_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(11, 3)];

			// discrete status, output module at position 12
			checkBox12_00.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(12, 0)];
			checkBox12_01.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(12, 1)];
			checkBox12_02.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(12, 2)];
			checkBox12_03.Checked = scanner.oaryDevices[i32CurrentSerialAddress].baryDiscreteStates64[OptoMMP.GetPointNumberFor64(12, 3)];

			textBoxStationStatus.Text = scanner.oaryDevices[i32CurrentSerialAddress].sScanState;
			textStationScanTime.Text = scanner.oaryDevices[i32CurrentSerialAddress].i64LastScanTimeMs.ToString();
			textTotalScanTime.Text = scanner.i64TotalScanTimeMs.ToString();

			textDevicePartNumber.Text = scanner.oaryDevices[i32CurrentSerialAddress].structDiagInfo.sDevice;
			textFirmwareRev.Text = scanner.oaryDevices[i32CurrentSerialAddress].structDiagInfo.sFirmwareVersion + " " +
				scanner.oaryDevices[i32CurrentSerialAddress].structDiagInfo.sFirmwareDate + " " +
				scanner.oaryDevices[i32CurrentSerialAddress].structDiagInfo.sFirmwareTime;
			textDeviceStartDateTime.Text = scanner.oaryDevices[i32CurrentSerialAddress].structDiagInfo.sEstimatedRestartTime;
			textCommAttempts.Text = scanner.oaryDevices[i32CurrentSerialAddress].u64CommAttempts.ToString();
			textCommFailures.Text = scanner.oaryDevices[i32CurrentSerialAddress].u64CommFaults.ToString();

			textModType0.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[0].ToString("X02");
			textModType1.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[1].ToString("X02");
			textModType2.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[2].ToString("X02");
			textModType3.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[3].ToString("X02");
			textModType4.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[4].ToString("X02");
			textModType5.Text = "0x" + scanner.oaryDevices[i32CurrentSerialAddress].u32aryModuleId[5].ToString("X02");
		}

		private void buttonWriteDiscrete_Click(object sender, EventArgs e)
		{
			// process user input for push a discrete
			// offset index is that the first output is module 8 and 4 channels per module
			Int32 i32Index = comboBoxDiscreteOutput.SelectedIndex + 8 * 4;
			Int32 i32StationIndex = comboBoxStationId.SelectedIndex;
			scanner.oaryDevices[i32StationIndex].baryNewDiscreteOnStates64[i32Index] = checkBoxTurnDiscreteOn.Checked;
			scanner.oaryDevices[i32StationIndex].baryNewDiscreteOffStates64[i32Index] = checkBoxTurnDiscreteOff.Checked;

			// signal the scanner to push a discrete update
			scanner.oaryDevices[i32StationIndex].bPushDiscreteStates = true;
		}

		private void buttonWriteAnalogValue_Click(object sender, EventArgs e)
		{
			Int32 i32StationIndex = comboBoxStationId.SelectedIndex;
			Int32 i32AnalogIndex = comboAnalogOutput.SelectedIndex;

			scanner.oaryDevices[i32StationIndex].i32AnalogIndexToWrite = i32AnalogIndex;
			scanner.oaryDevices[i32StationIndex].f32AnalogOutput = Single.Parse(textBoxAnalogWrite0.Text);
			scanner.oaryDevices[i32StationIndex].bPushAnalogOutput = true;
		}

		private void buttonScan_Click(object sender, EventArgs e)
		{
			if (bScanning == false)
			{
				scanner = new SnapPacSbScannerDemo(
					(String)comboComPort.SelectedItem, 
					Int32.Parse((String)comboBaudRate.SelectedItem), 
					byarySerialStations);
				scanner.StartThread();
				timerGuiUpdate.Start();
				buttonScan.Text = "Stop";
				bScanning = true;
			}
			else
			{
				timerGuiUpdate.Stop();
				scanner.StopThread();
				scanner = null;
				buttonScan.Text = "Start";
				bScanning = false;
			}
		}

		private void buttonPulse_Click(object sender, EventArgs e)
		{
			Int32 i32StationIndex = comboBoxStationId.SelectedIndex;
			// 32 is the first discrete output point in the I/O module arrangement
			Int32 i32PointIndex = comboPulse.SelectedIndex + 32;
			scanner.oaryDevices[i32StationIndex].i32PulseIndex = i32PointIndex;
			scanner.oaryDevices[i32StationIndex].f32StartDelaySeconds = Single.Parse(textPulseDelay.Text);
			scanner.oaryDevices[i32StationIndex].f32OnTimeSeconds = Single.Parse(textPulseOn.Text);
			scanner.oaryDevices[i32StationIndex].f32OffTimeSeconds = Single.Parse(textPulseOff.Text);
			scanner.oaryDevices[i32StationIndex].u32PulseQuantity = UInt32.Parse(textPulseQuantity.Text);
			scanner.oaryDevices[i32StationIndex].bPushPulse = true;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (bScanning == true)
			{
				timerGuiUpdate.Stop();
				scanner.StopThread();
				scanner = null;
				buttonScan.Text = "Start";
				bScanning = false;
			}
		}

		private void buttonReset_Click(object sender, EventArgs e)
		{
			scanner.oaryDevices[comboBoxStationId.SelectedIndex].bDeviceResetToDefaults = true;
		}
	}
}
