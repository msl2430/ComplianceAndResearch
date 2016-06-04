using System;
using System.Collections.ObjectModel;
using EngineCell.Application.ViewModels.AlarmConfiguration;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.Models;
using OxyPlot;

namespace EngineCell.Application.ViewModels.PointConfiguration
{
    public class PointDataModel : BaseModel
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
            set { _data = value; OnPropertyChanged("Data"); OnPropertyChanged("IsDigitalOn"); }
        }

        public bool IsDigitalOn => !IsAnalog && Data > 0;

        private ObservableCollection<DataPoint> _dataPoints { get; set; } 
        public ObservableCollection<DataPoint> DataPoints { get { return _dataPoints; } set { _dataPoints = value; OnPropertyChanged("DataPoints"); } }

        private AlarmSetting _alarm { get; set; }
        public AlarmSetting Alarm {
            get { return _alarm; }
            set { _alarm = value; OnPropertyChanged("Alarm"); }
        }

        public PointDataModel()
        {
            Alarm = new AlarmSetting();
        }

        public PointDataModel(CellPointModel obj)
        {
            if (obj == null)
                return;
            
            InstantiateFromDataObject(obj);
            DataPoints = new ObservableCollection<DataPoint>();
            Alarm = new AlarmSetting(obj.Alarm);
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

        public CellTestPointDataModel ToCellTestPointDataModel(int cellTestId)
        {
            return new CellTestPointDataModel
            {
                CellTestId = cellTestId,
                CellPointId = CellPointId,
                Data = Data,
                CaptureTime = DateTime.Now
            };
        }
    }
}
