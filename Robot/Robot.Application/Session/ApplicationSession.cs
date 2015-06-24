using System.Collections.Generic;
using Opto22.Core.Models;
using Robot.Application.Factories;
using Robot.Application.Services;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Session
{
    public static class ApplicationSession
    {
        private static IOptoMmpFactory _optoMmpFactory { get; set; }
        public static IOptoMmpFactory OptoMmpFactory { get { return _optoMmpFactory ?? (_optoMmpFactory = new OptoMmpFactory()); } }

        private static IScratchPadFactory _scratchPadFactory { get; set; }
        public static IScratchPadFactory ScratchPadFactory { get { return _scratchPadFactory ?? (_scratchPadFactory = new ScratchPadFactory(OptoMmpFactory)); } }

        private static ILoggingService _loggingService { get; set; }
        public static ILoggingService LoggingService { get { return _loggingService ?? (_loggingService = new LoggingService(ControlWindowViewModel)); } }

        public static ControlWindowViewModel _controlWindowViewModel { get; set; }
        public static ControlWindowViewModel ControlWindowViewModel { get {return _controlWindowViewModel ?? (_controlWindowViewModel = new ControlWindowViewModel());} }

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
