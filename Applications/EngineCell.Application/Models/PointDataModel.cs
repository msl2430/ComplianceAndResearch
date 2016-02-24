using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineCell.Models.Models;

namespace EngineCell.Application.Models
{
    public class PointDataModel : CellPointModel
    {
        public PointDataModel() { }

        public PointDataModel(CellPointModel obj)
        {
            if (obj == null)
                return;
            
            InstantiateFromDataObject(obj);
        }
    }
}
