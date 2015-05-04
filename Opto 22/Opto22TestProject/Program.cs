using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Opto22.Ip4;
using Opto22.OptoMMP3;

namespace Opto22TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const int pointNumber = 12;
                var optoMmp = new OptoMMP();
                var connectionResult = optoMmp.Open(@"173.70.96.171", 2001, OptoMMP.Connection.TcpIp, 5000, false);
                //var connectionResult = optoMmp.Open(@"127.0.0.1", 22001, OptoMMP.Connection.TcpIp, 5000, false);
                //Console.WriteLine("Power up: " + optoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear));
                
                Console.WriteLine("Connection OptoMMP: " + connectionResult + " - " + optoMmp.IsCommunicationOpen);

                if (optoMmp.IsCommunicationOpen)
                {
                    var result = optoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear);
                    Console.WriteLine("PUC: " + result);
                }

                var device = new Station();
                optoMmp.ReadBrainDiagnosticInformation(out device.structDiagInfo);

                Console.WriteLine("Diag Info: " + device.structDiagInfo.sDevice);

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
                int modType, pointType;
                float highScale, lowScale, offset, gain, filterWeight, watchdogValue;
                bool enableWatchdog;
                string name;
                //var writeResult = optoMmp.WriteDigitalPointConfiguration64(10, false, OptoMMP.eDigitalFeature.None, false, false, "doLiftUp");
                //Console.WriteLine("Write Result: " + writeResult);
                
                //var testpointResult = optoMmp.ReadAnalogPointConfiguration64(10, out modType, out pointType, out highScale, out lowScale, out offset, out gain, out filterWeight,
                //        out enableWatchdog, out watchdogValue, out name);
                //Console.WriteLine("Point " + 10 + ": \nModType: " + Enum.GetName(typeof(Constants.ModuleType), modType) + "\nPointType: " +
                //                          Enum.GetName(typeof(Constants.PointType), pointType) + "\nHighScale: " + highScale + "\nLowScale: " + lowScale + "\nOffset: " +
                //                          offset + "\nGain: :" + gain + "\nFilterWeight: " + filterWeight + "\nEnableWatchdog: " + enableWatchdog + "\nName: " + name);
                //optoMmp.WriteAnalogPointConfiguration64(10, 384, 0, 0, 0, 0, 0, false, 0, "doLiftUpTest");

                Console.ReadLine();
                Console.Clear();
                for (var i = 0; i < 64; i++)
                {

                    var readResult = optoMmp.ReadAnalogPointConfiguration64(i, out modType, out pointType, out highScale, out lowScale, out offset, out gain, out filterWeight,
                        out enableWatchdog, out watchdogValue, out name);
                    if (readResult >= 0)
                    {
                        Console.WriteLine("Point " + i + ": \nModType: " + Enum.GetName(typeof(Constants.ModuleType), modType) + "\nPointType: " +
                                          Enum.GetName(typeof(Constants.PointType), pointType) + "\nHighScale: " + highScale + "\nLowScale: " + lowScale + "\nOffset: " +
                                          offset + "\nGain: :" + gain + "\nFilterWeight: " + filterWeight + "\nEnableWatchdog: " + enableWatchdog + "\nName: " + name);
                    }
                    else
                    {
                        Console.WriteLine("Error reading point " + i + ": " + readResult);
                        Console.ReadLine();
                        break;
                    }
                    if (readResult == 0 && (modType > 0 || !string.IsNullOrEmpty(name)))
                        Console.ReadLine();
                    Console.Clear();
                }


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



                
                
                optoMmp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}
