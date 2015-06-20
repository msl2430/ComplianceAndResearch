using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Opto22.OptoMMP3;

namespace Opto22TestProject
{
    public class Program
    {
        //private const string IpAddress = "98.109.58.113";
        private const string IpAddress = "192.168.1.200";
        private const int Port = 2001;

        protected static IOptoMmpFactory OptoMmpFactory { get; set; }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Connecting to Opto Device at: " + IpAddress + ":" + Port);
                OptoMmpFactory = new OptoMmpFactory(IpAddress, Port);
                if (!OptoMmpFactory.IsConnected)
                {
                    Console.WriteLine("Error connecting");
                    Console.ReadLine();
                    return;
                }
                var pucResult = OptoMmpFactory.OptoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear);
                var pointService = new PointConfigurationService(OptoMmpFactory.OptoMmp);

                Console.WriteLine("Connected to Opto Device - " + pucResult);

                //var analogPoint20 = pointService.GetAnalogPointConfiguration(20);
                //var analogPoint21 = pointService.GetAnalogPointConfiguration(28);

                //Console.WriteLine();
                //analogPoint20.WriteConfigToConsole();
                //Console.WriteLine();
                //analogPoint21.WriteConfigToConsole();
                //Console.WriteLine();
                //float point20State, point21State, point25State;
                //OptoMmpFactory.OptoMmp.ReadAnalogState64(analogPoint20.PointNumber, out point20State);
                //OptoMmpFactory.OptoMmp.ReadAnalogState64(analogPoint21.PointNumber, out point21State);
                //Console.WriteLine("Point 20: " + point20State);
                //Console.WriteLine("Point 21: " + point21State);
                //Console.WriteLine();
                //OptoMmpFactory.OptoMmp.ScratchpadBitWrite(false, 0);

                bool scratchPad0;
                OptoMmpFactory.OptoMmp.ScratchpadBitRead(out scratchPad0, 0);
                Console.WriteLine("Scratchpad 0: " + scratchPad0);
                Console.WriteLine();

                var addr = new UInt32[1] { 0xf0d80000 };
                //Console.WriteLine("Addresss: " + OptoMmpFactory.OptoMmp.ReadCustomData(0, 1, addr, 0));
                Console.WriteLine("Number of strings: " + OptoMmpFactory.OptoMmp.ScratchPadStringNumberofElements());

                var str = new string[4];

                Console.WriteLine(OptoMmpFactory.OptoMmp.ScratchpadStringWrite(new string[] { "x", "e", "s", "t" }, 0, 4, 0));

                OptoMmpFactory.OptoMmp.ScratchpadStringRead(str, 0, 4, 0);
                Console.WriteLine(str[1]);

                //Console.WriteLine("Write State: " + OptoMmpFactory.OptoMmp.WriteAnalogState64(28, OptoMMP.AnalogWriteOptions.EngineeringUnits, 0f));
                //OptoMmpFactory.OptoMmp.ReadAnalogState64(28, out point21State);
                //Console.WriteLine("Point 28: " + point21State);
                //Console.WriteLine(OptoMMP.UInt32ToSingle(0xf0d82000));


                //var digitalPoint = pointService.GetAnalogPointConfiguration64(0);
                //Console.WriteLine(digitalPoint.ToString());
                //bool digitalState;
                //var stateResult = OptoMmpFactory.OptoMmp.ReadDigitalState64(digitalPoint.PointNumber, out digitalState);
                //Console.WriteLine();
                //Console.WriteLine("State: " + digitalState + " (" + stateResult + ")");

                //float digitalValue;
                //var valueResult = OptoMmpFactory.OptoMmp.ReadAnalogState64(digitalPoint.PointNumber, out digitalValue);
                //Console.WriteLine();
                //Console.WriteLine("State: " + valueResult + " (" + valueResult + ")");

                //Console.WriteLine();
                //Console.WriteLine("Current Config: ");
                //var configResult = OptoMmpFactory.OptoMmp.WriteAnalogPointConfiguration64(digitalPoint.PointNumber, 256, 0f, 0f, 0f, 0f, 0f, false, 0f, "diAirPressureSe");
                //Console.WriteLine();

                //// //OptoMmpFactory.OptoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.StoreToFlash);

                // Console.WriteLine();
                // Console.WriteLine("configResult: " + configResult);
                // Console.WriteLine("New Config: ");
                // var analogPoint2 = pointService.GetAnalogPointConfiguration64(21);
                // analogPoint2.WriteConfigToConsole();

                // readStateResult = OptoMmpFactory.OptoMmp.ReadAnalogState64(analogPoint2.PointNumber, out analogValue);
                // Console.WriteLine("Value: " + analogValue + "(" + readStateResult + ")");

                //var configurePulseResult = OptoMmpFactory.OptoMmp.ConfigureContinuousPulsesDuration(false, 0f, 1f, 1f, OptoMMP.IoModel.IoModel64, 0);
                //Console.WriteLine("configurePulseResult : " + configurePulseResult);
                //var startPulseResult = OptoMmpFactory.OptoMmp.StartPulse(OptoMMP.IoModel.IoModel64, 0);
                //Console.WriteLine("startPulseResult : " + startPulseResult);
                //float period;
                //var readPulseResult = OptoMmpFactory.OptoMmp.ReadPulsePeriod(OptoMMP.IoModel.IoModel64, 0, out period);
                //Console.WriteLine("readPulseResult: " + readPulseResult + " _ " + period);
                //float point21Value;
                //float[] pointsValue = new float[64];
                //var readAnalogResult = OptoMmpFactory.OptoMmp.ReadAnalogState64(21, out point21Value);
                //Console.WriteLine("readAnalogResult: " + readAnalogResult + " _ " + point21Value);

                //WriteConfigToFile(pointService);

                //AnalogPointConfiguration analogPoint = pointService.GetAnalogPointConfiguration(21);
                //Console.WriteLine("Config Point 21");
                //var configResult = OptoMmpFactory.OptoMmp.WriteAnalogPointConfiguration4096(analogPoint.PointNumber, (int) analogPoint.PointType, analogPoint.HighScale, analogPoint.LowScale,
                //    analogPoint.Offset, analogPoint.Gain, analogPoint.FilterWeight, false, analogPoint.FilterWeight, "Input2Test", 0f, 0f);
                //Console.WriteLine(configResult);
                ////OptoMmpFactory.OptoMmp.WriteAnalogPointConfiguration64(analogPoint.PointNumber, (int) analogPoint.PointType, analogPoint.HighScale, analogPoint.LowScale, analogPoint.Offset, analogPoint.Gain, analogPoint.FilterWeight, false, analogPoint.FilterWeight,
                //    //analogPoint.PointName);
                ////OptoMmpFactory.OptoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.StoreToFlash);
                //var newConfig = pointService.GetAnalogPointConfiguration(21);
                //newConfig.WriteConfigToConsole();
                //Console.ReadLine();
                //var configPoint = pointService.GetAnalogPointConfiguration(25);
                //configPoint.WriteConfigToConsole();
                //Console.WriteLine("Change Name...");
                //var pointName = Console.ReadLine();
                //OptoMmpFactory.OptoMmp.WriteDigitalPointConfiguration64(configPoint.PointNumber, true, OptoMMP.eDigitalFeature.None, false, false, pointName);
                //Console.WriteLine("New Config");
                //var newConfigPoint = pointService.GetAnalogPointConfiguration(25);
                //newConfigPoint.WriteConfigToConsole();
                //Console.ReadLine();


                var device = new Station();
                OptoMmpFactory.OptoMmp.ReadBrainDiagnosticInformation(out device.structDiagInfo);

                Console.WriteLine("Diag Info: " + device.structDiagInfo.sFirmwareVersion);

                OptoMmpFactory.OptoMmp.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);
                //
            }
            Console.ReadLine();
        }

        private static void ReadWritePointConfig(PointConfigurationService pointService)
        {
            var analogPoint = pointService.GetAnalogPointConfiguration64(21);

            Console.WriteLine();
            Console.WriteLine("Current Config: ");
            analogPoint.WriteConfigToConsole();
            var configResult = OptoMmpFactory.OptoMmp.WriteAnalogPointConfiguration64(analogPoint.PointNumber, 12, 10f, -10f, 0f, 0f, 0f, false, 0f, "Input2");
            Console.WriteLine();

            float analogValue;

            for (var i = 0; i < 10; i++)
            {
                var readStateResult = OptoMmpFactory.OptoMmp.ReadAnalogState64(analogPoint.PointNumber, out analogValue);
                Console.WriteLine(DateTime.Now + ">> Value: " + analogValue + "(" + readStateResult + ")");
                Thread.Sleep(1000);
            }
        }

        private static void WriteConfigToFile(PointConfigurationService pointService)
        {
            var points = pointService.GetAllPoints();
            Console.WriteLine("Added " + points.Count + " points");
            if (File.Exists(@"C:\AnalogPoints.txt"))
                File.Delete(@"C:\AnalogPoints.txt");
            using (var file = new StreamWriter(@"C:\AnalogPoints.txt", true))
            {
                file.Write(points.ToJson());
            }
            //Console.WriteLine("DigitalPoint Config");
            //var digitalPoints = pointService.GetAllDigitalPoints();
            //Console.WriteLine("Added " + digitalPoints.Count + " digital points");
            //if (File.Exists(@"C:\DigitalPoints.txt"))
            //    File.Delete(@"C:\DigitalPoints.txt");
            //using (var file = new StreamWriter(@"C:\DigitalPoints.txt", true))
            //{
            //    file.Write(digitalPoints.ToJson());
            //}
            //Console.WriteLine("Wrote to file");
        }

        /***Point Config Detail***/
        //Console.ReadLine();
        //Console.Clear();
        //for (var i = 0; i < 64; i++)
        //{
        //    var point = pointService.GetAnalogPointConfiguration(i);
        //    if ((point.ModuleType != Constants.ModuleType.Error && point.ModuleType != Constants.ModuleType.Empty) || !string.IsNullOrEmpty(point.PointName))
        //    {
        //        Console.WriteLine("Point " + i + ": \nModType: " + Enum.GetName(typeof(Constants.ModuleType), point.ModuleType) + "\nPointType: " +
        //                          Enum.GetName(typeof(Constants.PointType), point.PointType) + "\nHighScale: " + point.HighScale + "\nLowScale: " + point.LowScale +
        //                          "\nOffset: " +
        //                          point.Offset + "\nGain: :" + point.Gain + "\nFilterWeight: " + point.FilterWeight + "\nEnableWatchdog: " + point.EnableWatchdog +
        //                          "\nName: " + point.PointName);
        //        Console.ReadLine();
        //    }
        //    Console.Clear();
        //}
        //Console.WriteLine("Done");
        /***Point Config Detail***/






        //var temp = new Int32[1];
        //for (var i = 5; i < 6; i++)
        //{
        //    var mmpAddress = 0xfffff0c00000 + i * 0x100;
        //    Console.WriteLine("Module Address: " + mmpAddress);
        //    var result = optoMmp.ReadInts(mmpAddress, 1, temp, 0);
        //    if (result < 0)
        //        Console.WriteLine("Error reading ints");
        //    else
        //        Console.WriteLine("Module " + i + " found: " + temp[0]);

        //    var singleResult = optoMmp.ReadSingles(mmpAddress, 6 * 4, device.f32aryAnalogValues, 0);
        //    Console.WriteLine("Single Result: " + singleResult);
        //    if (singleResult == 0)
        //    {
        //        for (var j = 0; j < device.f32aryAnalogValues.Length; j++)
        //        {
        //            Console.WriteLine("Analog State " + j + ": " + device.f32aryAnalogValues[j]);
        //        }
        //    }
        //    Console.ReadLine();
        //    Console.Clear();
        //}
        //var writeResult = optoMmp.WriteDigitalPointConfiguration64(10, false, OptoMMP.eDigitalFeature.None, false, false, "doLiftUp");
        //Console.WriteLine("Write Result: " + writeResult);

        //var testpointResult = optoMmp.ReadAnalogPointConfiguration64(10, out modType, out pointType, out highScale, out lowScale, out offset, out gain, out filterWeight,
        //        out enableWatchdog, out watchdogValue, out name);
        //Console.WriteLine("Point " + 10 + ": \nModType: " + Enum.GetName(typeof(Constants.ModuleType), modType) + "\nPointType: " +
        //                          Enum.GetName(typeof(Constants.PointType), pointType) + "\nHighScale: " + highScale + "\nLowScale: " + lowScale + "\nOffset: " +
        //                          offset + "\nGain: :" + gain + "\nFilterWeight: " + filterWeight + "\nEnableWatchdog: " + enableWatchdog + "\nName: " + name);
        //optoMmp.WriteAnalogPointConfiguration64(10, 384, 0, 0, 0, 0, 0, false, 0, "doLiftUpTest");






        //string dateTime = "";

        //var dtResult = optoMmp.ReadDateTime(out dateTime);
        //Console.WriteLine("DateTime Result: " + dtResult + " - " + dateTime);

        //Console.WriteLine("Current Operation: " + Enum.GetName(typeof(DeviceScanState), device.eState));

        //device.eState = DeviceScanState.Scanning;

        //Console.WriteLine("Switched Operation to " + Enum.GetName(typeof(DeviceScanState), device.eState));

        //Console.WriteLine("Current Operation: " + Enum.GetName(typeof(DeviceScanState), device.eState));


        //for(var i = 0; i < device.baryDiscreteStates64.Length; i++)
        //{
        //    //Console.WriteLine("Discreate State 64 " + i + ": " + device.baryDiscreteStates64[i]);
        //}
        //var digitalStateResult = optoMmp.ReadDigitalStates64(device.baryDiscreteStates64, 0);
        //var analogStateResult = optoMmp.ReadAnalogStates64(device.f32aryAnalogValues, 0);
        //if (digitalStateResult == 0)
        //{
        //    for (var i = 0; i < device.baryDiscreteStates64.Length; i++)
        //    {
        //        Console.WriteLine("Digital State " + i + ": " + device.baryDiscreteStates64[i]);
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Digital State Result: " + digitalStateResult);
        //}
        //if (analogStateResult == 0)
        //{
        //    for (var i = 0; i < device.f32aryAnalogValues.Length; i++)
        //    {
        //        Console.WriteLine("Analog State " + i + ": " + device.f32aryAnalogValues[i]);
        //    }
        //}
        //else
        //{
        //    Console.WriteLine("Analog State Result: " + analogStateResult);
        //}
        ////var test = optoMmp.ReadAnalogStates64(device.f32aryAnalogValues, 0);
        ////Console.WriteLine("Analog States: " + test);
        //
        //Console.WriteLine("Read single result: " + singleResult);
        //string dateTime = "";

        //var result = optoMmp.ReadDateTime(out dateTime);
        //Console.WriteLine(result);
        //Console.WriteLine(dateTime);

        //if (optoMmp.IsCommunicationOpen)
        //{
        //    optoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear);
        //}

        //var pointNames = "";
        //optoMmp.ReadPointName4096(pointNumber, out pointNames);
        //Console.WriteLine("Point Names: " + pointNames);
        //var blah = optoMmp.ReadPointName4096(pointNumber, out pointNames);
        //Console.WriteLine("Point Names: " + pointNames);

        //optoMmp.ReadPointName64(pointNumber, out pointNames);
        //Console.WriteLine("Point Names: " + pointNames);
    }
}
