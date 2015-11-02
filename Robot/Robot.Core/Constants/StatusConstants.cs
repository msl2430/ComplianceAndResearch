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

        public enum SqlStatus
        {
            Active = 1,
            Inactive,
            Deleted
        }

        public enum ProgressStatus
        {
            InActive,
            Active,
            Finished
        }

        public enum PhaseStatus
        {
            InActive,
            PreCheckActive,
            LoadDataPointActive,
            Running,
            Shutdown,
            Completed,
        }
    }
}
