using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Core.Constants
{
    public static class ControlConstants
    {
        public static int TextBlockMaxCharacter = 10000;

        public enum ChangeTracking
        {
            Pristine,
            Dirty
        }
    }
}
