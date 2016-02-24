using EngineCell.Models.Repositories;

namespace EngineCell.Application.Services.DataServices
{
    public class BaseDataService
    {
        private ICellPointRepository _cellPointRepository { get; set; }

        public ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());
    }
}
