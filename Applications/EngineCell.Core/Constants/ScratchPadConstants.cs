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
            
                 
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum FloatIndexes
        {
            //Output Point Values
            Ao10BipolarValue = 300,
            Ao10Value,
            Ao5BipolarValue,
            Ao5Value,
            Ao4_20_0Value,
            Ao4_20_1Value,
            Ao4_20_2Value,
            Ao4_20_3Value,
            Dyno0Value,
            Dyno1Value,
            Dyno2Value,
            Dyno3Value,
            Dyno4Value,
            Dyno5Value,
            Dyno6Value,
            Dyno7Value,
            Engine0Value,
            Engine1Value,
            Engine2Value,
            Engine3Value,
            Engine4Value,
            Engine5Value,
            Engine6Value,
            Engine7Value,
            Fuel0Value,
            Fuel1Value,
            Fuel2Value,
            Fuel3Value,
            Fuel4Value,
            Fuel5Value,
            Fuel6Value,
            Fuel7Value,
            Misc0Value,
            Misc1Value,
            Misc2Value,
            Misc3Value,
            Misc4Value,
            Misc5Value,
            Misc6Value,
            Misc7Value,
            Freq0Value,
            Freq1Value,
            Freq2Value,
            Freq3Value,

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
            Ao10Bipolar,
            Ao10,
            Ao5Bipolar,
            Ao5,
            Ao4_20_0,
            Ao4_20_1,
            Ao4_20_2,
            Ao4_20_3,
            Dyno0,
            Dyno1,
            Dyno2,
            Dyno3,
            Dyno4,
            Dyno5,
            Dyno6,
            Dyno7,
            Engine0,
            Engine1,
            Engine2,
            Engine3,
            Engine4,
            Engine5,
            Engine6,
            Engine7,
            Fuel0,
            Fuel1,
            Fuel2,
            Fuel3,
            Fuel4,
            Fuel5,
            Fuel6,
            Fuel7,
            Misc0,
            Misc1,
            Misc2,
            Misc3,
            Misc4,
            Misc5,
            Misc6,
            Misc7,
            Freq0,
            Freq1,
            Freq2,
            Freq3,
            DynoIn0,
            DynoIn1,
            DynoIn2,
            DynoIn3,
            DynoIn4,
            DynoIn5,
            DynoIn6,
            DynoIn7,
            EngineIn0,
            EngineIn1,
            EngineIn2,
            EngineIn3,
            EngineIn4,
            EngineIn5,
            EngineIn6,
            EngineIn7,

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
