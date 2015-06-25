using System.Collections.Generic;
using Robot.Models.Models;

namespace Robot.Application.Services.DataServices
{
    public interface IModelService
    {
        IList<CarModel> GetCarModelsByManufactureId(int manufacturerId);
        bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId);
        void AddModel(string name, int manufacturerId);
    }

    public sealed class ModelService : BaseDataService, IModelService
    {
        public IList<CarModel> GetCarModelsByManufactureId(int manufacturerId)
        {
            return ModelRepostiory.GetModelByManufacturerId(manufacturerId);
        }

        public bool CheckUniqueModelNameByManufacturer(string name, int manufacturerId)
        {
            return ModelRepostiory.CheckUniqueModelNameByManufacturer(name, manufacturerId);
        }

        public void AddModel(string name, int manufacturerId)
        {
            ModelRepostiory.AddModel(name, manufacturerId);
        }
    }
}
