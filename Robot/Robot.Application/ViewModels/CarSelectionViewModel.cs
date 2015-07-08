using System.Collections.Generic;
using Robot.Application.Factories;
using Robot.Core.Constants;
using Robot.Models.Models;

namespace Robot.Application.ViewModels
{
    public class CarSelectionViewModel : BaseViewModel
    {
        private IList<ManufacturerModel> _manufacturers { get; set; }
        public IList<ManufacturerModel> Manufacturers
        {
            get { return _manufacturers; }
            set
            {
                _manufacturers = value;
                OnPropertyChanged("Manufacturers");
            }
        }

        private IList<CarModel> _carModels { get; set; }
        public IList<CarModel> CarModels
        {
            get { return _carModels; }
            set
            {
                _carModels = value;
                OnPropertyChanged("CarModels");
            }
        }

        private IList<int> _availableYears { get; set; }
        public IList<int> AvailableYears
        {
            get { return _availableYears;}
            set { _availableYears = value; OnPropertyChanged("AvailableYears"); }
        }

        private bool _isOptoConnected { get; set; }
        public bool IsOptoConnected
        {
            get { return _isOptoConnected; }
            set { _isOptoConnected = value; OnPropertyChanged("IsOptoConnected"); }
        }
        //public bool IsOptoConnected { get { return ApplicationSessionFactory.ApplicationViewModel.OptoConnectionStatus == StatusConstants.ConnectionStatus.Connected; } }

        public CarSelectionViewModel(IApplicationSessionFactory applicationSessionFactory) 
        {
            Manufacturers = new List<ManufacturerModel>();
            CarModels = new List<CarModel>();
            AvailableYears = new List<int>();
            ApplicationSessionFactory = applicationSessionFactory;
        }
    }
}
