using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opto22TestProject
{
    public class Constants
    {
        public enum ModuleType
        {
            Error = -1,
            Empty, 
            SnapAimaI = 34,
            SnapAivI = 35,
            SnapAov27 = 167,
            SnapIdc16 = 229
        }

        public enum PointType
        {
            SnapAivIPoint = 12,
            SnapAimaIPoint = 64,
            SnapAovPoint = 167,
            DigitalInput = 256,
            DigitalOutput = 384,
        }
    }
}
