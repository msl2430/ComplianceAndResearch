using System;
using System.Configuration;
using Opto22.OptoMMP3;

namespace EngineCell.Application.Factories
{
    public interface IOptoMmpFactory
    {
        OptoMMP Current { get; }
        void CloseConnection();
        int OpenConnection();
    }

    public sealed class OptoMmpFactory : IOptoMmpFactory
    {
        private OptoMMP OptoMmp { get; set; }

        public OptoMMP Current
        {
            get
            {
                if (OptoMmp != null)
                    return OptoMmp;

                OptoMmp = new OptoMMP();
                return OptoMmp;
            }
        }

        public int OpenConnection()
        {
            return Current.Open(ConfigurationManager.AppSettings["OptoIpAddress"], Convert.ToInt32(ConfigurationManager.AppSettings["OptoMmpPort"]), OptoMMP.Connection.TcpIp, 5000, false);
        }

        public void CloseConnection()
        {
            if (Current.IsCommunicationOpen)
                Current.Close();
            OptoMmp = null;
        }
    }
}
