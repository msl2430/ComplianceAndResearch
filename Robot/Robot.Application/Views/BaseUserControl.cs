using System.Windows.Controls;
using Robot.Application.Services.DataServices;

namespace Robot.Application.Views
{
    public class BaseUserControl : UserControl
    {
        private IManufacturerService _manufacturerService { get; set; }
        public IManufacturerService ManufacturerService { get { return _manufacturerService ?? (_manufacturerService = new ManufacturerService()); } }

        private ICountryService _countryService { get; set; }
        public ICountryService CountryService { get { return _countryService ?? (_countryService = new CountryService()); } }

        private IModelService _modelService { get; set; }
        public IModelService ModelService { get { return _modelService ?? (_modelService = new ModelService()); } }
    }
}
