using Opto22.OptoMMP3;

namespace Opto22TestProject
{
    public interface IOptoMmpFactory
    {
        OptoMMP OptoMmp { get; set; }
        bool IsConnected { get; set; }
    }

    public class OptoMmpFactory : IOptoMmpFactory
    {
        public OptoMMP OptoMmp { get; set; }
        public bool IsConnected { get; set; }

        public OptoMmpFactory() {}

        public OptoMmpFactory(string ipAddress, int port)
        {
            OptoMmp = new OptoMMP();
            var result = OptoMmp.Open(ipAddress, port, OptoMMP.Connection.TcpIp, 5000, false);
            IsConnected = result == 0;
        }
    }
}
