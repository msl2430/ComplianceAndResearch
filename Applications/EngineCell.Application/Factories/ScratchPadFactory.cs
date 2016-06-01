using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Core.Constants;
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
        private const int MaxIntegerScratchPadElements = 10240;
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
            if (OptoMmp.Current.IsCommunicationOpen)
            {
                foreach (var bit in Enum.GetValues(typeof (ScratchPadConstants.BitIndexes)))
                {
                    try
                    {
                        var scratchPad = ScratchPadBits.FirstOrDefault(b => b.Index == (int) bit);
                        bool tempBit;
                        result = OptoMmp.Current.ScratchpadBitRead(out tempBit, scratchPad.Index);
                        if (result != 0)
                            throw new Exception("Error initializing bit " + bit + ".");
                        scratchPad.Value = tempBit;
                    }
                    catch (Exception ex)
                    {
                        //do nothing
                    }
                }
            }

            var optoScratchPadInts = new int[MaxIntegerScratchPadElements];
            try
            {
                if (OptoMmp.Current.IsCommunicationOpen)
                {
                    result = OptoMmp.Current.ScratchpadI32Read(optoScratchPadInts, 0, MaxIntegerScratchPadElements, 0);
                    if (result != 0)
                        //throw new Exception("Error getting scratchpad integers.");
                        foreach (var scratchPad in
                            from object intIndex in Enum.GetValues(typeof (ScratchPadConstants.IntegerIndexes))
                            select ScratchPadInts.FirstOrDefault(b => b.Index == (int) intIndex))
                        {
                            scratchPad.Value = optoScratchPadInts[scratchPad.Index];
                        }
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            var optoScratchPadStrs = new string[MaxStringScratchPadElements];
            try
            {
                if (OptoMmp.Current.IsCommunicationOpen)
                {
                    result = OptoMmp.Current.ScratchpadStringRead(optoScratchPadStrs, 0, MaxStringScratchPadElements, 0);
                    if (result != 0)
                        throw new Exception("Error getting scratchpad strings.");
                    foreach (var scratchPad in
                        from object strIndex in Enum.GetValues(typeof (ScratchPadConstants.StringIndexes))
                        select ScratchPadStrings.FirstOrDefault(b => b.Index == (int) strIndex))
                    {
                        scratchPad.Value = optoScratchPadStrs[scratchPad.Index];
                    }
                }
            }
            catch (Exception ex)
            {
                //do nothing
            }

            var optoScratchPadFloats = new float[MaxFloatScratchPadElements];
            try
            {
                if (!OptoMmp.Current.IsCommunicationOpen) return;
                result = OptoMmp.Current.ScratchpadFloatRead(optoScratchPadFloats, 0, MaxFloatScratchPadElements, 0);
                if (result != 0)
                    throw new Exception("Error getting scratchpad floats.");
                foreach (var scratchPad in
                    from object strIndex in Enum.GetValues(typeof (ScratchPadConstants.FloatIndexes))
                    select ScratchPadFloats.FirstOrDefault(b => b.Index == (int) strIndex))
                {
                    scratchPad.Value = Convert.ToDecimal(optoScratchPadFloats[scratchPad.Index]);
                }
            }
            catch (Exception ex)
            {
                //do nothing
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
                : new ScratchPadModel<int>(index, "undefined", 0);

            var attempts = 0;
            var result = -1;
            var optoScratchPadInt = new int[1];
            while (attempts < 5 && result != 0)
            {
                result = OptoMmp.Current.ScratchpadI32Read(optoScratchPadInt, 0, 1, index);
                attempts++;
            }

            if (result != 0)
                return sc;

            sc.Value = optoScratchPadInt[0];

            return sc;
        }

        public IScratchPadModel<string> GetScratchPadString(int index)
        {
            var sc = ScratchPadStrings.Any(b => b.Index == index)
                ? ScratchPadStrings.FirstOrDefault(x => x.Index == index)
                : new ScratchPadModel<string>(index, "undefined", "");

            var optoScratchPadString = new string[1];
            var result = OptoMmp.Current.ScratchpadStringRead(optoScratchPadString, 0, 1, index);
            if (result != 0)
                return sc;

            sc.Value = optoScratchPadString[0];

            return sc;
        }

        public IScratchPadModel<decimal> GetScratchPadFloat(int index)
        {
            var sc = ScratchPadFloats.Any(f => f.Index == index)
                ? ScratchPadFloats.FirstOrDefault(f => f.Index == index)
                : new ScratchPadModel<decimal>(index, "undefined", 0m);

            var optoScratchPadFloat = new float[1];
            var result = OptoMmp.Current.ScratchpadFloatRead(optoScratchPadFloat, 0, 1, index);
            if (result != 0)
                return sc;

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
                OptoMmp.Current.ScratchpadBitWrite(Convert.ToBoolean(value), index);

                var sc = ScratchPadBits.All(b => b.Index != index)
                    ? new ScratchPadModel<bool>(index, "undefined", false)
                    : ScratchPadBits.FirstOrDefault(b => b.Index == index);

                sc.Value = Convert.ToBoolean(value);
            }
            else if (typeof (T) == typeof (int))
            {
                OptoMmp.Current.ScratchpadI32Write(new[] {Convert.ToInt32(value)}, 0, 1, index);

                var sc = ScratchPadInts.All(b => b.Index != index)
                    ? new ScratchPadModel<int>(index, "undefined", 0)
                    : ScratchPadInts.FirstOrDefault(b => b.Index == index);

                sc.Value = Convert.ToInt32(value);
            }
            else if (typeof (T) == typeof (string))
            {
                OptoMmp.Current.ScratchpadStringWrite(new[] {Convert.ToString(value)}, 0, 1, index);

                var sc = ScratchPadStrings.All(b => b.Index != index)
                    ? new ScratchPadModel<string>(index, "undefined", "")
                    : ScratchPadStrings.FirstOrDefault(b => b.Index == index);

                sc.Value = Convert.ToString(value);
            }
            else if (typeof(T) == typeof(decimal))
            {
                OptoMmp.Current.ScratchpadFloatWrite(new[] { (float)Convert.ToDecimal(value) }, 0, 1, index);

                var sc = ScratchPadFloats.All(b => b.Index != index)
                    ? new ScratchPadModel<decimal>(index, "undefined", 0m)
                    : ScratchPadFloats.FirstOrDefault(b => b.Index == index);

                sc.Value = Convert.ToDecimal(value);
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
