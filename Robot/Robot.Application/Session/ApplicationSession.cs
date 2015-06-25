using System.Collections.Generic;
using Opto22.Core.Models;
using Robot.Application.Factories;
using Robot.Application.ViewModels;

namespace Robot.Application.Session
{
    public static class ApplicationSession
    {
        private static IOptoMmpFactory _optoMmpFactory { get; set; }
        public static IOptoMmpFactory OptoMmpFactory { get { return _optoMmpFactory ?? (_optoMmpFactory = new OptoMmpFactory()); } }

        private static IScratchPadFactory _scratchPadFactory { get; set; }
        public static IScratchPadFactory ScratchPadFactory { get { return _scratchPadFactory ?? (_scratchPadFactory = new ScratchPadFactory(OptoMmpFactory)); } }

        public static ControlWindowViewModel ControlWindowViewModel { get; set; }
        public static bool IsConnectedToOpto { get; set; }

        /*------TODO REMOVE------*/
        public static IList<AnalogPointModel> AnalogPoints { get; set; }
        public static bool WaitForConnection()
        {
            var counter = 0;
            while (!IsConnectedToOpto)
            {
                counter++;
                if (counter > 60)
                    return false;
            }
            return true;
        }
    }
}
