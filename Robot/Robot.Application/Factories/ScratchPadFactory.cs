using System;
using System.Collections.Generic;
using System.Linq;
using Opto22.Core.Constants;
using Opto22.Core.Models;
using Robot.Core.Extensions;

namespace Robot.Application.Factories
{
    public interface IScratchPadFactory
    {
        IScratchPadModel<bool> GetScratchPadBit(int index);
        IScratchPadModel<int> GetScratchPadInt(int index);
        IScratchPadModel<string> GetScratchPadString(int index);
        IScratchPadModel<decimal> GetScratchPadFloat(int index);

        void SetScratchPadValue<T>(int index, T value);
        void SetScratchPadValue<T>(IScratchPadModel<T> scratchPad);
        void SetScratchPadTspValues(List<List<IScratchPadModel<decimal>>> tspValues, List<decimal> speedPoints, List<decimal> accelerationPoints, IList<ScratchPadModel<decimal>> roadTestPoints);
    }

    public sealed class ScratchPadFactory : IScratchPadFactory
    {
        private const int MaxIntegerScratchPadElements = 512;
        private const int MaxFloatScratchPadElements = 10240;
        private const int MaxStringScratchPadElements = 64;
        private IOptoMmpFactory OptoMmp { get; set; }
        private IList<ScratchPadModel<bool>> ScratchPadBits { get; set; }
        private IList<ScratchPadModel<int>> ScratchPadInts { get; set; }
        private IList<ScratchPadModel<string>> ScratchPadStrings { get; set; }
        private IList<ScratchPadModel<decimal>> ScratchPadFloats { get; set; }

        public ScratchPadFactory(IOptoMmpFactory optoMmp)
        {
            OptoMmp = optoMmp;
            ScratchPadBits = ((int[]) Enum.GetValues(typeof (ScratchPadConstants.BitIndexes))).ToList()
                .Select(x => new ScratchPadModel<bool>(x, Enum.GetName(typeof (ScratchPadConstants.BitIndexes), x), false)).ToList();
            ScratchPadInts = ((int[]) Enum.GetValues(typeof (ScratchPadConstants.IntegerIndexes))).ToList()
                .Select(x => new ScratchPadModel<int>(x, Enum.GetName(typeof (ScratchPadConstants.IntegerIndexes), x), 0)).ToList();
            ScratchPadStrings = ((int[]) Enum.GetValues(typeof (ScratchPadConstants.StringIndexes))).ToList()
                .Select(x => new ScratchPadModel<string>(x, Enum.GetName(typeof (ScratchPadConstants.StringIndexes), x), "")).ToList();
            ScratchPadFloats = ((int[])Enum.GetValues(typeof(ScratchPadConstants.FloatIndexes))).ToList()
                .Select(x => new ScratchPadModel<decimal>(x, Enum.GetName(typeof(ScratchPadConstants.FloatIndexes), x), 0m)).ToList();

            InitializeFromOpto();
        }

        private void InitializeFromOpto()
        {
            int result;
            foreach (var bit in Enum.GetValues(typeof (ScratchPadConstants.BitIndexes)))
            {
                var scratchPad = ScratchPadBits.FirstOrDefault(b => b.Index == (int) bit);
                bool tempBit;
                result = OptoMmp.Current.ScratchpadBitRead(out tempBit, scratchPad.Index);
                if (result != 0)
                    throw new Exception("Error initializing bit " + bit + ".");
                scratchPad.Value = tempBit;
            }

            var optoScratchPadInts = new int[MaxIntegerScratchPadElements];
            result = OptoMmp.Current.ScratchpadI32Read(optoScratchPadInts, 0, MaxIntegerScratchPadElements, 0);
            if (result != 0)
                throw new Exception("Error getting scratchpad integers.");
            foreach (var scratchPad in
                from object intIndex in Enum.GetValues(typeof (ScratchPadConstants.IntegerIndexes))
                select ScratchPadInts.FirstOrDefault(b => b.Index == (int) intIndex))
            {
                scratchPad.Value = optoScratchPadInts[scratchPad.Index];
            }

            var optoScratchPadStrs = new string[MaxStringScratchPadElements];
            result = OptoMmp.Current.ScratchpadStringRead(optoScratchPadStrs, 0, MaxStringScratchPadElements, 0);
            if (result != 0)
                throw new Exception("Error getting scratchpad strings.");
            foreach (var scratchPad in
                from object strIndex in Enum.GetValues(typeof (ScratchPadConstants.StringIndexes))
                select ScratchPadStrings.FirstOrDefault(b => b.Index == (int) strIndex))
            {
                scratchPad.Value = optoScratchPadStrs[scratchPad.Index];
            }

            var optoScratchPadFloats = new float[MaxFloatScratchPadElements];
            result = OptoMmp.Current.ScratchpadFloatRead(optoScratchPadFloats, 0, MaxFloatScratchPadElements, 0);
            if (result != 0)
                throw new Exception("Error getting scratchpad floats.");
            foreach (var scratchPad in
                    from object strIndex in Enum.GetValues(typeof(ScratchPadConstants.FloatIndexes))
                    select ScratchPadFloats.FirstOrDefault(b => b.Index == (int)strIndex))
            {
                scratchPad.Value = Convert.ToDecimal(optoScratchPadFloats[scratchPad.Index]);
            }
        }

        public IScratchPadModel<bool> GetScratchPadBit(int index)
        {
            var sc = ScratchPadBits.Any(b => b.Index == index)
                ? ScratchPadBits.FirstOrDefault(x => x.Index == index)
                : null;

            if (sc == null)
                throw new Exception("Bad scratchpad Bit index.");

            bool tempBit;
            var result = OptoMmp.Current.ScratchpadBitRead(out tempBit, index);
            if (result != 0)
                throw new Exception("Error getting scratchpad bit.");

            sc.Value = tempBit;

            return sc;
        }

        public IScratchPadModel<int> GetScratchPadInt(int index)
        {
            var sc = ScratchPadInts.Any(b => b.Index == index)
                ? ScratchPadInts.FirstOrDefault(x => x.Index == index)
                : null;

            if (sc == null)
                throw new Exception("Bad scratchpad Int index.");

            var optoScratchPadInt = new int[1];
            var result = OptoMmp.Current.ScratchpadI32Read(optoScratchPadInt, 0, 1, index);
            if (result != 0)
                throw new Exception("Error getting scratchpad integers.");

            sc.Value = optoScratchPadInt[0];

            return sc;
        }

        public IScratchPadModel<string> GetScratchPadString(int index)
        {
            var sc = ScratchPadStrings.Any(b => b.Index == index)
                ? ScratchPadStrings.FirstOrDefault(x => x.Index == index)
                : null;

            if (sc == null)
                throw new Exception("Bad scratchpad String index.");

            var optoScratchPadString = new string[1];
            var result = OptoMmp.Current.ScratchpadStringRead(optoScratchPadString, 0, 1, index);
            if (result != 0)
                throw new Exception("Error getting scratchpad string.");

            sc.Value = optoScratchPadString[0];

            return sc;
        }

        public IScratchPadModel<decimal> GetScratchPadFloat(int index)
        {
            var sc = ScratchPadFloats.Any(f => f.Index == index)
                ? ScratchPadFloats.FirstOrDefault(f => f.Index == index)
                : null;

            if (sc == null)
                throw new Exception("Bad scratchpad Float index.");

            var optoScratchPadFloat = new float[1];
            var result = OptoMmp.Current.ScratchpadFloatRead(optoScratchPadFloat, 0, 1, index);
            if (result != 0)
                throw new Exception("Error getting scratchpad float.");

            if (float.IsNaN(optoScratchPadFloat[0]))
                optoScratchPadFloat[0] = 0f;
            sc.Value = Convert.ToDecimal(optoScratchPadFloat[0]);

            return sc;
        }

        public void SetScratchPadValue<T>(IScratchPadModel<T> scratchPad)
        {
            SetScratchPadValue(scratchPad.Index, scratchPad.Value);
        }

        public void SetScratchPadValue<T>(int index, T value)
        {
            if (typeof (T) == typeof (bool))
            {
                if (ScratchPadBits.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad bit index.");

                var result = OptoMmp.Current.ScratchpadBitWrite(Convert.ToBoolean(value), index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad bit.");

                ScratchPadBits.FirstOrDefault(b => b.Index == index).Value = Convert.ToBoolean(value);
            }
            else if (typeof (T) == typeof (int))
            {
                if (ScratchPadInts.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad int index.");

                var result = OptoMmp.Current.ScratchpadI32Write(new[] {Convert.ToInt32(value)}, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad int.");

                ScratchPadInts.FirstOrDefault(b => b.Index == index).Value = Convert.ToInt32(value);
            }
            else if (typeof (T) == typeof (string))
            {
                if (ScratchPadStrings.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad string index.");

                var result = OptoMmp.Current.ScratchpadStringWrite(new[] {Convert.ToString(value)}, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad string.");

                ScratchPadStrings.FirstOrDefault(b => b.Index == index).Value = Convert.ToString(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                if (ScratchPadFloats.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad float index.");

                var result = OptoMmp.Current.ScratchpadFloatWrite(new[] { (float)Convert.ToDecimal(value) }, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad float.");

                ScratchPadFloats.FirstOrDefault(b => b.Index == index).Value = Convert.ToDecimal(value);
            }
        }

        public void SetScratchPadTspValues(List<List<IScratchPadModel<decimal>>> tspValues, List<decimal> speedPoints, List<decimal> accelerationPoints, IList<ScratchPadModel<decimal>> roadTestPoints)
        {
            OptoMmp.Current.ScratchpadFloatWrite(new[] {0f}, 0, MaxFloatScratchPadElements, 0);
            var result = 0;
            var tspArray = Enumerable.Repeat(-1f, 100).ToArray();
            for (var i = 0; i < 1 /*tspValues.Count*/; i++)
            {
                for (var tsp = 0; tsp < tspArray.Length; tsp++)
                {
                    if (tspValues[i].Count() <= tsp)
                        break;
                    tspArray[tsp] = (float) tspValues[i].ElementAt(tsp).Value;
                }
                result = OptoMmp.Current.ScratchpadFloatWrite(tspArray, 0, tspArray.Length, ScratchPadConstants.FloatIndexes.TspGear1Start.ToInt());
                if (result != 0)
                    throw new Exception("Error saving TSP Chart scratchpad float.");
            }

            var speedPointArray = Enumerable.Repeat(-1f, 20).ToArray();
            for (var sp = 0; sp < speedPointArray.Length; sp++)
            {
                if (speedPoints.Count() <= sp)
                    break;
                speedPointArray[sp] = speedPoints.Select(x => (float) x).OrderBy(x => x).ElementAt(sp);
            }
            result = OptoMmp.Current.ScratchpadFloatWrite(speedPointArray, 0, speedPointArray.Length, ScratchPadConstants.FloatIndexes.SpeedPoints.ToInt());
            if (result != 0)
                throw new Exception("Error saving Speed points scratchpad float.");


            var accelerationPointArray = Enumerable.Repeat(-1f, 20).ToArray();
            for (var ac = 0; ac < accelerationPointArray.Length; ac++)
            {
                if (accelerationPoints.Count() <= ac)
                    break;
                accelerationPointArray[ac] = accelerationPoints.Select(x => (float) x).OrderBy(x => x).ElementAt(ac);
            }
            result = OptoMmp.Current.ScratchpadFloatWrite(accelerationPointArray, 0, accelerationPointArray.Length, ScratchPadConstants.FloatIndexes.AcclerationPoints.ToInt());
            if (result != 0)
                throw new Exception("Error saving Acceleration points scratchpad float.");

            var roadTestPointArray = Enumerable.Repeat(-1f, 3600).ToArray();
            for (var rt = 0; rt < roadTestPointArray.Length; rt++)
            {
                if (roadTestPoints.Count() <= rt)
                    break;
                roadTestPointArray[rt] = (float)roadTestPoints[rt].Value;
            }
            result = OptoMmp.Current.ScratchpadFloatWrite(roadTestPointArray, 0, roadTestPointArray.Length, ScratchPadConstants.FloatIndexes.RoadTestSpeedPerSecond.ToInt());
            if(result != 0)
                throw new Exception("Error saving Road Test points scratchpad float;");
        }
    }

    public sealed class MockScratchPadFactory : IScratchPadFactory
    {
        public IScratchPadModel<bool> GetScratchPadBit(int index)
        {
            throw new NotImplementedException();
        }

        public IScratchPadModel<int> GetScratchPadInt(int index)
        {
            throw new NotImplementedException();
        }

        public IScratchPadModel<string> GetScratchPadString(int index)
        {
            throw new NotImplementedException();
        }

        public IScratchPadModel<decimal> GetScratchPadFloat(int index)
        {
            throw new NotImplementedException();
        }

        public void SetScratchPadValue<T>(int index, T value)
        {
            
        }

        public void SetScratchPadValue<T>(IScratchPadModel<T> scratchPad)
        {
            
        }

        public void SetScratchPadTspValues(List<List<IScratchPadModel<decimal>>> tspValues, List<decimal> speedPoints, List<decimal> accelerationPoints, IList<ScratchPadModel<decimal>> roadTestPoints)
        {
            throw new NotImplementedException();
        }
    }
}
