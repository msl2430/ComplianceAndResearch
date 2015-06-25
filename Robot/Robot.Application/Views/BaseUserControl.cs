using System.Windows.Controls;
using Robot.Application.Factories;
using Robot.Application.Services.DataServices;

namespace Robot.Application.Views
{
    public class BaseUserControl : UserControl
    {
        public IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        private IManufacturerService _manufacturerService { get; set; }
        public IManufacturerService ManufacturerService { get { return _manufacturerService ?? (_manufacturerService = new ManufacturerService()); } }

        private ICountryService _countryService { get; set; }
        public ICountryService CountryService { get { return _countryService ?? (_countryService = new CountryService()); } }

        private IModelService _modelService { get; set; }
        public IModelService ModelService { get { return _modelService ?? (_modelService = new ModelService()); } }
    }
}
