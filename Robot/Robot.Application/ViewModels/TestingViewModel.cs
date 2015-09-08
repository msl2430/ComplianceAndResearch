using System.Collections.Generic;
using Robot.Application.Factories;
using Robot.Models.Models;

namespace Robot.Application.ViewModels
{
    public class TestingViewModel : BaseViewModel
    {
        public int ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        public string CarDisplayName { get { return string.Format("{0} {1} {2}", ModelYear, ManufacturerName, ModelName); } }

        private IList<RoadTestChartModel> _roadTestCharts { get; set; }
        public IList<RoadTestChartModel> RoadTestCharts
        {
            get { return _roadTestCharts;}
            set {
                _roadTestCharts = value;
                OnPropertyChanged("RoadTestCharts");
            }
        }

        public TestingViewModel(IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
        }
    }
}
