using System;
using Opto22.OptoMMP3;

namespace Opto22.Core.Models
{
    public enum DeviceScanState
    {
        Configuring = 1,
        Scanning,
        Fault
    }

    public class Opto22StationModel
    {
        #region Informational
        public DeviceScanState eState = DeviceScanState.Configuring;
        public Int32 i32FaultDecrementCounter = 0;

        // statistics
        public UInt64 u64CommAttempts = 0;
        public UInt64 u64CommFaults = 0;

        public Int64 i64LastScanTimeMs;
        public String sScanState = "";
        #endregion Informational

        #region Device Configuration
        // diagnostic info
        public OptoMMP.structBrainDiagnosticInformation structDiagInfo = new OptoMMP.structBrainDiagnosticInformation();

        // module inventory
        public UInt32[] u32aryModuleId = new UInt32[16];
        #endregion Device Configuration

        #region IO States
        // values read from station
        public Boolean[] baryDiscreteStates64 = new Boolean[64];
        public Single[] f32aryAnalogValues = new Single[64];
        #endregion IO States

        #region IO Writes
        // to write a discrete output
        public Boolean bPushDiscreteStates = false;
        public Boolean[] baryNewDiscreteOnStates64 = new Boolean[64];
        public Boolean[] baryNewDiscreteOffStates64 = new Boolean[64];

        // initiate a pulse output
        public Boolean bPushPulse = false;
        public Int32 i32PulseIndex = 0;
        public Single f32StartDelaySeconds;
        public Single f32OnTimeSeconds;
        public Single f32OffTimeSeconds;
        public UInt32 u32PulseQuantity;

        // change an analog output value
        public Boolean bPushAnalogOutput = false;
        public Int32 i32AnalogIndexToWrite = 0;
        public Single f32AnalogOutput = (Single)0.0;

        // clear any prior configuration and reset device
        public Boolean bDeviceResetToDefaults = false;
        #endregion IO Writes

        /// <summary>
        /// Constructor
        /// </summary>
        public Opto22StationModel()
        {
        }
    }
}
