namespace EngineCell.Core.Constants
{
    public class ScratchPadConstants
    {
        public const decimal DefaultNullValue = -32769;

        public enum BitIndexes
        {
            
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum IntegerIndexes
        {
            ConnectedToOpto,
            StrategyLocationValue,

            StartTest = 10,
            TestRunning = 11,

            StartDataCollection = 999,

            //Test Schedule Widget
            StartTestScheduleWidget = 3000,
            IsUsingDynoPid,
            IsUsingThrottle,

            //Custom Chart Widgets
            StartCustomChart1 = 3100,
            IsCustomChart1Running,
            StartCustomChart2,
            IsCustomChart2Running,
            StartCustomChart3,
            IsCustomChart3Running,
            StartCustomChart4,
            IsCustomChart4Running,
            StartCustomChart5,
            IsCustomChart5Running,

            //Dyno PID Ramp Widget
            DynoPidWidgetMode = 3399,
            StartDynoPidWidget = 3400,
            DynoPidWidgetRunning,

            //Starter Widget
            StartStarterWidget = 3402,
            StarterWidgetRunning,
            StarterAttempt,
            StarterCrankOn,
            StarterEngineOn,

            //Throttle PID Ramp Widget
            StartThrottlePidWidget = 3407,
            ThrottlePidWidgetRunning,
            ThrottlePidMode,

            //Pid Config
            PidConfigStatus = 500,
            PidConfigType,
            InputType,
            OutputType,
            SetPointType,

            //Ventilation Control 1
            VentCtrl1Status = 600,
            VentCtrl1InsideType,
            VentCtrl1OutsideType,
            VentCtrl1OutputType,     
            
            //DynoPid
            DynoPidStatus = 610,
            DynoPidMode,
            DynoPidMeasurement,

            ThrottleLoadSafety = 9000,
            ThrottleSpeedSafety,
            DynoLoadSafety,
            ExhaustSafety,
            EngineRpmSafety,
            ThrottleSpeedPressureSafety,
            StarterCrankSafety,
            IsHardwareSafety = 9999,
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum FloatIndexes
        {
            //Output Point Values
            Ao10_0Value = 300,
            Ao10_1Value,
            Ao5_0Value,
            Ao5_1Value,
            Ao4_20_0Value,
            Ao4_20_1Value,
            Ao4_20_2Value,
            Ao4_20_3Value,
            DigitalOut0Value,
            DigitalOut1Value,
            DigitalOut2Value,
            DigitalOut3Value,
            DigitalOut4Value,
            DigitalOut5Value,
            DigitalOut6Value,
            DigitalOut7Value,
            DigitalOut8Value,
            DigitalOut9Value,
            DigitalOut10Value,
            DigitalOut11Value,
            DigitalOut12Value,
            DigitalOut13Value,
            DigitalOut14Value,
            DigitalOut15Value,
            DigitalOut16Value,
            DigitalOut17Value,
            DigitalOut18Value,
            DigitalOut19Value,
            DigitalOut20Value,
            DigitalOut21Value,
            DigitalOut22Value,
            DigitalOut23Value,
            DigitalOut24Value,
            DigitalOut25Value,
            DigitalOut26Value,
            DigitalOut27Value,
            DigitalOut28Value,
            DigitalOut29Value,
            DigitalOut30Value,
            DigitalOut31Value,
            Freq0Value,
            Freq1Value,
            Lc_BandStopValue,
            Lc_BandPassValue,

            //Pid Config
            InputValue = 500,
            InputLowRange,
            InputHighRange,
            OutputValue,
            OutputLowerClamp,
            OutputUpperClamp,
            OutputMinChange,
            OutputMaxChange,
            SetPointValue,
            Gain,
            TuneI,
            TuneD,
            FeedFwdInitial,
            FeedFwdGain,

            //Ventilation Control 1
            VentCtrl1Gain = 604,
            VentCtrl1Setpoint,
            VentCtrl1Inside,
            VentCtrl1Outside,
            VentCtrl1Output,

            //DynoPid
            DynoPidSetpoint = 610,

            //Points,
            ThermoCouple0 = 1000,
            ThermoCouple1,
            ThermoCouple2,
            ThermoCouple3,
            ThermoCouple4,
            ThermoCouple5,
            ThermoCouple6,
            ThermoCouple7,
            ThermoCouple8,
            ThermoCouple9,
            ThermoCouple10,
            ThermoCouple11,
            ThermoCouple12,
            ThermoCouple13,
            ThermoCouple14,
            ThermoCouple15,
            ThermoCouple16,
            ThermoCouple17,
            ThermoCouple18,
            ThermoCouple19,
            Volt0,
            Volt1,
            Volt2,
            Volt3,
            Volt4,
            Volt5,
            MilliAmp0,
            MilliAmp1,
            Hart0,
            Hart1,
            Ao10_0,
            Ao10_1,
            Ao5_0,
            Ao5_1,
            Ao4_20_0,
            Ao4_20_1,
            Ao4_20_2,
            Ao4_20_3,
            DigitalOut0,
            DigitalOut1,
            DigitalOut2,
            DigitalOut3,
            DigitalOut4,
            DigitalOut5,
            DigitalOut6,
            DigitalOut7,
            DigitalOut8,
            DigitalOut9,
            DigitalOut10,
            DigitalOut11,
            DigitalOut12,
            DigitalOut13,
            DigitalOut14,
            DigitalOut15,
            DigitalOut16,
            DigitalOut17,
            DigitalOut18,
            DigitalOut19,
            DigitalOut20,
            DigitalOut21,
            DigitalOut22,
            DigitalOut23,
            DigitalOut24,
            DigitalOut25,
            DigitalOut26,
            DigitalOut27,
            DigitalOut28,
            DigitalOut29,
            DigitalOut30,
            DigitalOut31,
            Freq0,
            Freq1,
            LC_BandStop,
            LC_BandPass,
            DigitalIn0,
            DigitalIn1,
            DigitalIn2,
            DigitalIn3,
            DigitalIn4,
            DigitalIn5,
            DigitalIn6,
            DigitalIn7,
            DigitalIn8,
            DigitalIn9,
            DigitalIn10,
            DigitalIn11,
            DigitalIn12,
            DigitalIn13,
            DigitalIn14,
            DigitalIn15,

            //PID Values
            DynoSetpointValue = 2000,
            ThrottleSetpointValue,

            //Test Schedule Setpoints
            TimeInStage = 3000,
            DynoSetpoint = 3120,
            ThrottleSetpoint = 3240,

            //Dyno PID Ramp
            DynoPidRampSetpoint = 3400,
            DynoPidRampTime,
            DynoPidRampCurrentSetpoint,
            DynoPidRampCurrentTime,

            //Throttle PID Ramp
            ThrottlePidRampSetpoint = 3404,
            ThrottlePidRampTime,
            ThrottlePidRampCurrentSetpoint,
            ThrottlePidRampCurrentTime,

            //Starter 
            StarterCrankTime = 3403,
            StarterStartRpm,
            StarterRetryWaitTime,
            StarterRetryCount,
            StarterCutoffTime,
            StarterEngineRpm,
        }

        public enum StringIndexes
        {
        }

        public enum StrategyLocationValues
        {
            Off,
            AppRunningCheck,
            WaitingForTestStart,
        }
    }
}
