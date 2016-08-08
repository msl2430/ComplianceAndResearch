﻿namespace EngineCell.Core.Constants
{
    public class ScratchPadConstants
    {
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

            StartDataCollection = 100,

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
        }

        /// <summary>
        /// Max 10,240
        /// </summary>
        public enum FloatIndexes
        {
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
