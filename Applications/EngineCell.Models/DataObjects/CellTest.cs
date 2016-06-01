﻿using System;

namespace EngineCell.Models.DataObjects
{
    //dbo.CellTest
    public class CellTest
    {
        public virtual int CellTestId { get; set; }
        public virtual int CellId { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
    }
}
