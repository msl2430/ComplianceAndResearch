using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels;

namespace EngineCell.Application.Session
{
    public static class ApplicationSession
    {
        private static IOptoMmpFactory _optoMmpFactory { get; set; }
        public static IOptoMmpFactory OptoMmpFactory { get { return _optoMmpFactory ?? (_optoMmpFactory = new OptoMmpFactory()); } }

        private static IScratchPadFactory _scratchPadFactory { get; set; }
        public static IScratchPadFactory ScratchPadFactory { get { return _scratchPadFactory ?? (_scratchPadFactory = new ScratchPadFactory(OptoMmpFactory)); } }

        public static ApplicationViewModel ApplicationViewModel { get; set; }
        public static bool IsConnectedToOpto { get; set; }        
    }
}
