using System;
using System.Collections.Generic;
using EngineCell.Models.Models;

namespace EngineCell.Application.ViewModels.Welcome
{
    [Serializable]
    public class TestHistoryViewModel : BaseViewModel
    {
        public IList<CellTestModel> Tests { get; set; }

        public CellTestModel TestToCopy { get; set; }
    }
}
