using System.Collections.Generic;
using Robot.Models.Models;

namespace Robot.Application.Services.DataServices
{
    public interface IManufacturerService
    {
        IList<ManufacturerModel> GetAllManufactures();
        bool CheckUniqueManufacturerName(string name);
        void AddManufacturer(string name, int countryId, string modelName);
    }

    public sealed class ManufacturerService : BaseDataService, IManufacturerService
    {
        public IList<ManufacturerModel> GetAllManufactures()
        {
            return ManufacturerRepository.GetManufacturerList();
        }

        public bool CheckUniqueManufacturerName(string name)
        {
            return ManufacturerRepository.CheckUniqueManufacturerName(name);
        }

        public void AddManufacturer(string name, int countryId, string modelName)
        {
            ManufacturerRepository.AddManufacturer(name, countryId, modelName);
        }
    }
}
