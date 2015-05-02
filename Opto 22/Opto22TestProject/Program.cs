using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                var temp = new Int32[1];
                for (var i = 0; i < 17; i++)
                {
                    var mmpAddress = 0xfffff0c00000 + i*0x100;
                    var result = optoMmp.ReadInts(mmpAddress, 1, temp, 0);
                    if(result < 0)
                        Console.WriteLine("Error reading ints");
                    else
                        Console.WriteLine("Module " + i + " found: " + temp[0]);
                }

                Console.WriteLine("Current Operation: " + Enum.GetName(typeof(DeviceScanState), device.eState));

                device.eState = DeviceScanState.Scanning;

                Console.WriteLine("Switched Operation to " + Enum.GetName(typeof(DeviceScanState), device.eState));

                Console.WriteLine("Current Operation: " + Enum.GetName(typeof(DeviceScanState), device.eState));
                for(var i = 0; i < device.baryDiscreteStates64.Length; i++)
                {
                    //Console.WriteLine("Discreate State 64 " + i + ": " + device.baryDiscreteStates64[i]);
                }
                var test = optoMmp.ReadAnalogStates64(device.f32aryAnalogValues, 0);
                Console.WriteLine("Analog States: " + test);
                var singleResult = optoMmp.ReadSingles(0xfffff0500000, 6*4, device.f32aryAnalogValues, 0);
                Console.WriteLine("Read single result: " + singleResult);
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
