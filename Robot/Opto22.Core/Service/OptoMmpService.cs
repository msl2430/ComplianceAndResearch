using Opto22.OptoMMP3;

namespace Opto22.Core.Service
{
    public interface IOptoMmpService
    {
        OptoMMP OptoMmp { get; set; }
        bool IsConnected { get; set; }
    }

    public class OptoMmpService : IOptoMmpService
    {
        public OptoMMP OptoMmp { get; set; }
        public bool IsConnected { get; set; }

        public OptoMmpService() {}

        public OptoMmpService(string ipAddress, int port)
        {
            OptoMmp = new OptoMMP();
            var result = OptoMmp.Open(ipAddress, port, OptoMMP.Connection.TcpIp, 5000, false);
            IsConnected = result == 0;
        }
    }
}
