using System.Collections.ObjectModel;
using EngineCell.Application.ViewModels;
using EngineCell.Core.Constants;
using EngineCell.Models.Models;
using OxyPlot;

namespace EngineCell.Application.Models
{
    public class PointDataModel : BaseViewModel
    {
        public int CellPointId { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; }
        public int PointId { get; set; }
        public string PointName { get; set; }
        public bool IsInput { get; set; }
        public bool IsAnalog { get; set; }
        public PointGroup PointGroupId { get; set; }

        private string _customName { get; set; }
        public string CustomName
        {
            get { return _customName; }
            set { _customName = value; OnPropertyChanged("CustomName"); }
        }

        private bool _includeInStripChart { get; set; }
        public bool IncludeInStripChart {
            get { return _includeInStripChart; }
            set { _includeInStripChart = value; OnPropertyChanged("IncludeInStripChart"); }
        }

        private decimal? _stripChartScale { get; set; }
        public decimal? StripChartScale
        {
            get { return _stripChartScale; }
            set { _stripChartScale = value; OnPropertyChanged("StripChartScale"); }
        }

        private decimal _data { get; set; }
        public decimal Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged("Data"); }
        }

        private ObservableCollection<DataPoint> _dataPoints { get; set; } 
        public ObservableCollection<DataPoint> DataPoints { get { return _dataPoints; } set { _dataPoints = value; OnPropertyChanged("DataPoints"); } }

        public PointDataModel() { }

        public PointDataModel(CellPointModel obj)
        {
            if (obj == null)
                return;
            
            InstantiateFromDataObject(obj);
            DataPoints = new ObservableCollection<DataPoint>();
        }

        public CellPointModel ToCellPointModel()
        {
            return new CellPointModel
            {
                CellPointId = CellPointId,
                CellId = CellId,
                CellName = CellName,
                PointId = PointId,
                PointName = PointName,
                IsInput = IsInput,
                IsAnalog = IsAnalog,
                PointGroupId = PointGroupId,
                CustomName = CustomName,
                IncludeInStripChart = IncludeInStripChart,
                StripChartScale = StripChartScale,
            };
        }
    }
}
