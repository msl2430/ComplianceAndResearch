using System;
using System.Collections.Generic;
using System.Linq;
using Opto22.Core.Constants;
using Opto22.Core.Models;

namespace EngineCell.Application.Factories
{
    public interface IScratchPadFactory
    {
        IScratchPadModel<bool> GetScratchPadBit(int index);
        IScratchPadModel<int> GetScratchPadInt(int index);
        IScratchPadModel<string> GetScratchPadString(int index);
        IScratchPadModel<decimal> GetScratchPadFloat(int index);

        void SetScratchPadValue<T>(int index, T value);
        void SetScratchPadValue<T>(IScratchPadModel<T> scratchPad);
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
                //throw new Exception("Error getting scratchpad integers.");
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
                ScratchPadBits.Add(new ScratchPadModel<bool>(index, "undefined", false));
            //throw new Exception("Bad scratchpad Bit index."); TODO: Do we want to keep this?

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
                ScratchPadInts.Add(new ScratchPadModel<int>(index, "undefined", 0));
            //throw new Exception("Bad scratchpad Int index.");TODO: Do we want to keep this?

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
            {
                sc = new ScratchPadModel<decimal>(index, "undefined", 0);
                ScratchPadFloats.Add(sc);
            }
            //throw new Exception("Bad scratchpad Float index.");

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
                    ScratchPadBits.Add(new ScratchPadModel<bool>(index, "undefined", false));
                //throw new Exception("Bad scratchpad bit index.");

                var result = OptoMmp.Current.ScratchpadBitWrite(Convert.ToBoolean(value), index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad bit.");

                ScratchPadBits.FirstOrDefault(b => b.Index == index).Value = Convert.ToBoolean(value);
            }
            else if (typeof (T) == typeof (int))
            {
                if (ScratchPadInts.All(b => b.Index != index))
                    ScratchPadInts.Add(new ScratchPadModel<int>(index, "undefined", 0));
                //throw new Exception("Bad scratchpad int index.");

                var result = OptoMmp.Current.ScratchpadI32Write(new[] {Convert.ToInt32(value)}, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad int.");

                ScratchPadInts.FirstOrDefault(b => b.Index == index).Value = Convert.ToInt32(value);
            }
            else if (typeof (T) == typeof (string))
            {
                if (ScratchPadStrings.All(b => b.Index != index))
                    ScratchPadStrings.Add(new ScratchPadModel<string>(index, "undefined", ""));
                //throw new Exception("Bad scratchpad string index.");

                var result = OptoMmp.Current.ScratchpadStringWrite(new[] {Convert.ToString(value)}, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad string.");

                ScratchPadStrings.FirstOrDefault(b => b.Index == index).Value = Convert.ToString(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                if (ScratchPadFloats.All(b => b.Index != index))
                    ScratchPadFloats.Add(new ScratchPadModel<decimal>(index, "undefined", 0));
                //throw new Exception("Bad scratchpad float index.");

                var result = OptoMmp.Current.ScratchpadFloatWrite(new[] { (float)Convert.ToDecimal(value) }, 0, 1, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad float.");

                ScratchPadFloats.FirstOrDefault(b => b.Index == index).Value = Convert.ToDecimal(value);
            }
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

    }
}
