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
                var connectionResult = optoMmp.Open(@"127.0.0.1", 22001, OptoMMP.Connection.TcpIp, 5000, false);
                //Console.WriteLine("Power up: " + optoMmp.WriteStatusCommand(OptoMMP.StatusWriteCommand.PowerUpClear));
                
                Console.WriteLine("Connection OptoMMP: " + connectionResult + " - " + optoMmp.IsCommunicationOpen);
                string dateTime = "";
                var result = optoMmp.ReadDateTime(out dateTime);
                Console.WriteLine(result);
                Console.WriteLine(dateTime);

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



                //var device = new Station();
                //optoMmp.ReadBrainDiagnosticInformation(out device.structDiagInfo);
                
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
