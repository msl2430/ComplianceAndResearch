using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views
{
    public class BaseUserControl : UserControl
    {
        public ICellPointRepository _cellPointRepository { get; set; }
        public ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());
    }
}
