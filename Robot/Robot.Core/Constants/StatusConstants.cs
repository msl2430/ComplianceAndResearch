using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Core.Constants
{
    public static class StatusConstants
    {
        public enum ConnectionStatus
        {
            Disconnected,
            Disconnecting,
            Connecting,
            Connected
        }
    }
}
