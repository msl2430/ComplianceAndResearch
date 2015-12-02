using System;
using Opto22.Core.Models;
using Opto22.OptoMMP3;

namespace Opto22.Core.Service
{
    public interface IScratchPadService
    {
        void WriteToScratchPad<T>(ScratchPadModel<T> obj);
    }

    public sealed class ScratchPadService : IScratchPadService
    {
        private OptoMMP OptoMmp { get; set; }

        public ScratchPadService(OptoMMP optoMmp)
        {
            OptoMmp = optoMmp;
        }

        public void WriteToScratchPad<T>(ScratchPadModel<T> obj)
        {
            if (typeof(T) == typeof(bool))
                WriteToScratchPad(Convert.ToBoolean(obj.Value), obj.Index);
            if (typeof(T) == typeof(int))
                WriteToScratchPad(Convert.ToInt32(obj.Value), obj.Index);
            if(typeof(T) == typeof(string))
                throw new Exception("No support for saving strings to scratch pad.");
        }

        private void WriteToScratchPad(bool objToWrite, int objIndex)
        {
            var result = OptoMmp.ScratchpadBitWrite(objToWrite, objIndex);
            CheckResultForError(result);
        }

        private void WriteToScratchPad(int objToWrite, int objIndex)
        {
            var result = OptoMmp.ScratchpadI32Write(new[] {objToWrite}, 0, 1, objIndex);
            CheckResultForError(result);
        }

        private void CheckResultForError(int result)
        {
            if (result != 0)
                throw new Exception("Error saving to scratch pad!");
        }
    }
}
