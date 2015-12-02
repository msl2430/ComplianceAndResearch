using System.Collections.Generic;
using Robot.Models.Models;

namespace Robot.Application.Services.DataServices
{
    public interface IModelService
    {
        IList<CarModel> GetCarModelsByManufactureId(int manufacturerId);
        bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId);
        int AddModel(string name, int manufacturerId, int year, decimal pulseMultiplier, decimal pulsePerRev);
    }

    public sealed class ModelService : BaseDataService, IModelService
    {
        public IList<CarModel> GetCarModelsByManufactureId(int manufacturerId)
        {
            return ModelRepository.GetModelByManufacturerId(manufacturerId);
        }

        public bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId)
        {
            return ModelRepository.CheckUniqueModelNameByManufacturer(name, manufacturerId);
        }

        public int AddModel(string name, int manufacturerId, int year, decimal pulseMultiplier, decimal pulsePerRev)
        {
            return ModelRepository.AddModel(name, manufacturerId, year, pulseMultiplier, pulsePerRev);
        }
    }
}
