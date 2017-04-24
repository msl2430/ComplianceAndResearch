﻿using System;
using System.Collections.ObjectModel;
using EngineCell.Core.Constants;
using EngineCell.Core.Models;
using EngineCell.Models.Models;
using OxyPlot;

namespace EngineCell.Application.ViewModels.PointConfiguration
{
    public class PointDataModel : BaseModel, IDisposable
    {
        public int CellPointId { get; set; }
        public int CellId { get; set; }
        public string CellName { get; set; }
        public int PointId { get; set; }
        public string PointName { get; set; }
        public bool IsInput { get; set; }
        public bool IsAnalog { get; set; }
        public bool IsCustomValue { get; set; }
        public PointGroup PointGroupId { get; set; }
        public bool IsCustomValueSet { get; set; }

        private string _customName { get; set; }
        public string CustomName
        {
            get { return _customName; }
            set { _customName = value; OnPropertyChanged("CustomName"); }
        }

        private bool _isRecord { get; set; }
        public bool IsRecord {
            get { return _isRecord; }
            set { _isRecord = value; OnPropertyChanged("IsRecord"); }
        }

        private bool _isAverage { get; set; }
        public bool IsAverage
        {
            get { return _isAverage; }
            set { _isAverage = value; OnPropertyChanged("IsAverage"); }
        }

        private int? _averageSeconds { get; set; }
        public int? AverageSeconds
        {
            get { return _averageSeconds; }
            set { _averageSeconds = value; OnPropertyChanged("AverageSeconds"); }
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

        private decimal _averageData { get; set; }
        public decimal AverageData {
            get { return _averageData; }
            set { _averageData = value; OnPropertyChanged("AverageData"); }
        }

        private decimal? _customValue { get; set; }
        public decimal? CustomValue
        {
            get { return _customValue; }
            set { _customValue = value; OnPropertyChanged("CustomValue"); }
        }

        private ObservableCollection<decimal> _mostRecentData { get; set; } 
        public ObservableCollection<decimal> MostRecentData {
            get { return _mostRecentData; }
            set { _mostRecentData = value; OnPropertyChanged("MostRecentData"); }
        }

        private ObservableCollection<DataPoint> _dataPoints { get; set; } 
        public ObservableCollection<DataPoint> DataPoints { get { return _dataPoints; } set { _dataPoints = value; OnPropertyChanged("DataPoints"); } }

        private bool _isActive { get; set; }
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged("IsActive"); OnPropertyChanged("IsDisplayActive"); }
        }

        private bool _hasPhaseTrigger { get; set; }
        public bool HasPhaseTrigger
        {
            get {  return _hasPhaseTrigger;}
            set { _hasPhaseTrigger = value; OnPropertyChanged("HasPhaseTrigger"); OnPropertyChanged("IsDisplayActive"); }
        }

        public bool IsDigitalOn => !IsAnalog && Data > 0;
        public bool IsDisplayValue => IsAnalog || (!IsAnalog && IsInput);
        
        public bool IsDisplayActive => IsActive || HasPhaseTrigger;

        public PointDataModel()
        {
            HasPhaseTrigger = false;
            IsCustomValueSet = false;
        }

        public PointDataModel(CellPointModel obj)
        {
            if (obj == null)
                return;
            
            InstantiateFromDataObject(obj);
            HasPhaseTrigger = false;
            DataPoints = new ObservableCollection<DataPoint>();
            MostRecentData = new ObservableCollection<decimal>();
            IsCustomValueSet = false;
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
                IsRecord = IsRecord,
                IsAverage = IsAverage,
                AverageSeconds = AverageSeconds,
                IncludeInStripChart = IncludeInStripChart,
                StripChartScale = StripChartScale,
                IsCustomValue = IsCustomValue,
                CustomValue = !IsCustomValue || CustomValue == ScratchPadConstants.DefaultNullValue ? null : CustomValue,
                IsActive = IsActive
            };
        }

        public CellTestPointDataModel ToCellTestPointDataModel(int cellTestId, DateTime captureTime)
        {
            return new CellTestPointDataModel
            {
                CellTestId = cellTestId,
                CellPointId = CellPointId,
                Data = AverageData,
                CaptureTime = captureTime
            };
        }

        public void Dispose()
        {
            _dataPoints = null;
            _mostRecentData = null;
        }
    }
}
