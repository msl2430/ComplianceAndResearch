using System;
using System.Configuration;
using Opto22.Core.Service;
using Opto22.OptoMMP3;

namespace Robot.Application.Factories
{
    public interface IOptoMmpFactory
    {
        OptoMMP Current { get; }
    }

    public sealed class OptoMmpFactory : IOptoMmpFactory
    {
        private OptoMmpService OptoMmpService { get; set; }

        public OptoMMP Current
        {
            get
            {
                if (OptoMmpService != null)
                    return OptoMmpService.OptoMmp;

                OptoMmpService = new OptoMmpService(ConfigurationManager.AppSettings["OptoIpAddress"], Convert.ToInt32(ConfigurationManager.AppSettings["OptoMmpPort"]));
                return OptoMmpService.OptoMmp;
            }
        }
    }
}
