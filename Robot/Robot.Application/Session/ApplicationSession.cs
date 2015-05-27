using System.Collections.Generic;
using Opto22.Core.Models;

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
    }
}
