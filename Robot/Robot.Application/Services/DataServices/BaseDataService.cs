using Robot.Models.Repositories;

namespace Robot.Application.Services.DataServices
{
    public class BaseDataService
    {
        private IManufacturerRepository _manufacturerRepository { get; set; }

        public IManufacturerRepository ManufacturerRepository
        {
            get { return _manufacturerRepository ?? (_manufacturerRepository = new ManufacturerRepository()); }
        }

        private ICountryRepository _countryRepository { get; set; }

        public ICountryRepository CountryRepository
        {
            get { return _countryRepository ?? (_countryRepository = new CountryRepository()); }
        }

        private IModelRepostiory _modelRepostiory { get; set; }

        public IModelRepostiory ModelRepostiory
        {
            get { return _modelRepostiory ?? (_modelRepostiory = new ModelRepostiory()); }
        }

        private ISetPointRepository _setPointRepository { get; set; }
        public ISetPointRepository SetPointRepository
            {
                get
                {
                    return _setPointRepository ?? (_setPointRepository = new SetPointRepository());
                }
            }

        private IRoadTestRepository _roadTestRepository { get; set; }
        public IRoadTestRepository RoadTestRepository
            {
                get
                {
                    return _roadTestRepository ?? (_roadTestRepository = new RoadTestRepository());
                }
            }
        }
}
