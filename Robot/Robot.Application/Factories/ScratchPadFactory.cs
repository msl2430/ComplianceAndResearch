using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Opto22.Core.Constants;
using Opto22.Core.Models;

namespace Robot.Application.Factories
{
    public interface IScratchPadFactory
    {
        ScratchPadModel<bool> GetScratchPadBit(int index);
        ScratchPadModel<int> GetScratchPadInt(int index);
        ScratchPadModel<string> GetScratchPadString(int index);
        void SetScratchPadValue<T>(int index, T value);
        void SetScratchPadValue<T>(ScratchPadModel<T> scratchPad);
    }

    public sealed class ScratchPadFactory : IScratchPadFactory
    {
        private const int MaxIntegerScratchPadElements = 256;
        private const int MaxStringScratchPadElements = 64;
        private IOptoMmpFactory OptoMmp { get; set; }
        private IList<ScratchPadModel<bool>> ScratchPadBits { get; set; }
        private IList<ScratchPadModel<int>> ScratchPadInts { get; set; }
        private IList<ScratchPadModel<string>> ScratchPadStrings { get; set; }

        public ScratchPadFactory(IOptoMmpFactory optoMmp)
        {
            OptoMmp = optoMmp;
            ScratchPadBits = ((int[]) Enum.GetValues(typeof (ScratchPadConstants.BitIndexes))).ToList()
                    .Select(x => new ScratchPadModel<bool>(x, Enum.GetName(typeof (ScratchPadConstants.BitIndexes), x), false)).ToList();
            ScratchPadInts = ((int[])Enum.GetValues(typeof(ScratchPadConstants.IntegerIndexes))).ToList()
                    .Select(x => new ScratchPadModel<int>(x, Enum.GetName(typeof(ScratchPadConstants.IntegerIndexes), x), 0)).ToList();
            ScratchPadStrings = ((int[])Enum.GetValues(typeof(ScratchPadConstants.StringIndexes))).ToList()
                    .Select(x => new ScratchPadModel<string>(x, Enum.GetName(typeof(ScratchPadConstants.StringIndexes), x), "")).ToList();
            InitializeFromOpto();
        }

        private void InitializeFromOpto()
        {
            int result;
            foreach (var bit in Enum.GetValues(typeof (ScratchPadConstants.BitIndexes)))
            {
                var scratchPad = ScratchPadBits.FirstOrDefault(b => b.Index == (int)bit);
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
                    from object intIndex in Enum.GetValues(typeof(ScratchPadConstants.IntegerIndexes))
                    select ScratchPadInts.FirstOrDefault(b => b.Index == (int)intIndex))
            {
                scratchPad.Value = optoScratchPadInts[scratchPad.Index];
            }

            var optoScratchPadStrs = new string[MaxStringScratchPadElements];
            result = OptoMmp.Current.ScratchpadStringRead(optoScratchPadStrs, 0, 64, 0);
            if (result != 0)
                throw new Exception("Error getting scratchpad strings.");
            foreach (var scratchPad in
                    from object strIndex in Enum.GetValues(typeof(ScratchPadConstants.StringIndexes))
                    select ScratchPadStrings.FirstOrDefault(b => b.Index == (int)strIndex))
            {
                scratchPad.Value = optoScratchPadStrs[scratchPad.Index];
            }
        }

        public ScratchPadModel<bool> GetScratchPadBit(int index)
        {
            return ScratchPadBits.Any(b => b.Index == index)
                ? ScratchPadBits.FirstOrDefault(x => x.Index == index)
                : null;
        }

        public ScratchPadModel<int> GetScratchPadInt(int index)
        {
            return ScratchPadInts.Any(b => b.Index == index)
                ? ScratchPadInts.FirstOrDefault(x => x.Index == index)
                : null;
        }

        public ScratchPadModel<string> GetScratchPadString(int index)
        {
            return ScratchPadStrings.Any(b => b.Index == index)
                ? ScratchPadStrings.FirstOrDefault(x => x.Index == index)
                : null;
        }

        public void SetScratchPadValue<T>(ScratchPadModel<T> scratchPad)
        {
            SetScratchPadValue(scratchPad.Index, scratchPad.Value);
        }

        public void SetScratchPadValue<T>(int index, T value)
        {
            if (typeof(T) == typeof(bool))
            {
                if (ScratchPadBits.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad bit index.");

                var result = OptoMmp.Current.ScratchpadBitWrite(Convert.ToBoolean(value), index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad bit.");

                ScratchPadBits.FirstOrDefault(b => b.Index == index).Value = Convert.ToBoolean(value);
            }
            else if (typeof(T) == typeof(int))
            {
                if (ScratchPadInts.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad bit index.");

                var result = OptoMmp.Current.ScratchpadI32Write(new[] { Convert.ToInt32(value) }, 0, 0, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad bit.");

                ScratchPadInts.FirstOrDefault(b => b.Index == index).Value = Convert.ToInt32(value);
            }
            else if (typeof(T) == typeof(string))
            {
                if (ScratchPadStrings.All(b => b.Index != index))
                    throw new Exception("Bad scratchpad bit index.");

                var result = OptoMmp.Current.ScratchpadStringWrite(new[] { Convert.ToString(value) }, 0, 0, index);
                if (result != 0)
                    throw new Exception("Error saving scratchpad bit.");

                ScratchPadStrings.FirstOrDefault(b => b.Index == index).Value = Convert.ToString(value);
            }
        }
    }
}
