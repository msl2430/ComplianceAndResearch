using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opto22.Core.Models;
using Robot.Application.Session;

namespace Robot.Application.Extensions
{
    public static class ScratchPadExtensions
    {
        public static void Update<T>(this ScratchPadModel<T> obj)
        {
            ApplicationSession.ScratchPadFactory.SetScratchPadValue(obj);
        }

        public static void Update<T>(this IScratchPadModel<T> obj)
        {
            ApplicationSession.ScratchPadFactory.SetScratchPadValue(obj);
        }
    }
}
