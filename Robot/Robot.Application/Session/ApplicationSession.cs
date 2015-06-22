using System.Collections.Generic;
using Opto22.Core.Models;
using Robot.Application.Factories;

namespace Robot.Application.Session
{
    public static class ApplicationSession
    {
        public static bool IsConnectedToOpto { get; set; }

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

        public static IList<AnalogPointModel> AnalogPoints { get; set; }

        public static IScratchPadFactory ScratchPadFactory { get; set; }
    }
}
