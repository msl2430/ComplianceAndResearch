using System;
using System.IO;
using Opto22.OptoMMP3;

namespace Opto22TestProject
{
    public class Program
    {
        private const string IpAddress = "173.70.96.171";
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

                Console.WriteLine("Connected to Opto Device");

                var pointService = new PointConfigurationService(OptoMmpFactory.OptoMmp);

                var points = pointService.GetAllPoints();
                Console.WriteLine("Added " + points.Count + " points");
                if(File.Exists(@"C:\Points.txt")) 
                    File.Delete(@"C:\Points.txt");
                using (var file = new StreamWriter(@"C:\Points.txt", true))
                {
                    foreach (var pt in points)
                    {
                        file.WriteLine("Point " + pt.PointNumber + ": \nModType: " + Enum.GetName(typeof(Constants.ModuleType), pt.ModuleType) + "\nPointType: " +
                              Enum.GetName(typeof(Constants.PointType), pt.PointType) + "\nHighScale: " + pt.HighScale + "\nLowScale: " + pt.LowScale +
                              "\nOffset: " + pt.Offset + "\nGain: :" + pt.Gain + "\nFilterWeight: " + pt.FilterWeight + "\nEnableWatchdog: " + pt.EnableWatchdog +
                              "\nName: " + pt.PointName + "\n");
                    }
                }
                var configPoint = pointService.GetAnalogPointConfiguration(25);
                configPoint.WriteConfigToConsole();
                Console.WriteLine("Change Name...");
                Console.ReadLine();
                OptoMmpFactory.OptoMmp.WriteDigitalPointConfiguration64(configPoint.PointNumber, true, OptoMMP.eDigitalFeature.None, false, false, "TestPoint25");
                Console.WriteLine("New Config");
                var newConfigPoint = pointService.GetAnalogPointConfiguration(25);
                newConfigPoint.WriteConfigToConsole();
                Console.ReadLine();


                var device = new Station();
                OptoMmpFactory.OptoMmp.ReadBrainDiagnosticInformation(out device.structDiagInfo);

                Console.WriteLine("Diag Info: " + device.structDiagInfo.sDevice);

                OptoMmpFactory.OptoMmp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
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
