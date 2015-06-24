using Opto22.OptoMMP3;

namespace Opto22.Core.Service
{
    public interface IOptoMmpService
    {
        OptoMMP OptoMmp { get; set; }
    }

    public class OptoMmpService : IOptoMmpService
    {
        public OptoMMP OptoMmp { get; set; }
    }
}
