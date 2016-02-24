using System.Collections.Generic;
using EngineCell.Models.Models;

namespace EngineCell.Application.Services.DataServices
{
    public interface ICellPointService
    {
        IList<CellPointModel> GetCellPointModels(int cellId);
    }

    public class CellPointService : BaseDataService, ICellPointService
    {
        public IList<CellPointModel> GetCellPointModels(int cellId)
        {
            return CellPointRepository.GetCellPointsByCellId(cellId);
        } 
    }
}
