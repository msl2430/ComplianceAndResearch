using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.ViewModels.StripChart
{
    public class StripChartViewModel : BaseViewModel
    {
        public IList<ChartSeries> ChartSeries { get; set; } 
        public PlotModel PlotModel { get; set; }

        private ChartSeries _coolantSeries { get; set; }
        public ChartSeries CoolantSeries
        {
            get { return _coolantSeries; }
            set
            {
                _coolantSeries = value;
                OnPropertyChanged("CoolantSeries");
            }
        }

        private ChartSeries _oilSeries { get; set; }
        public ChartSeries OilSeries
        {
            get { return _oilSeries; }
            set
            {
                _oilSeries = value;
                OnPropertyChanged("OilSeries");
            }
        }

        private ChartSeries _throttle1Series { get; set; }
        public ChartSeries Throttle1Series
        {
            get { return _throttle1Series; }
            set
            {
                _throttle1Series = value;
                OnPropertyChanged("Throttle1Series");
            }
        }

        private ChartSeries _throttle2Series { get; set; }
        public ChartSeries Throttle2Series
        {
            get { return _throttle2Series; }
            set
            {
                _throttle2Series = value;
                OnPropertyChanged("Throttle2Series");
            }
        }

        private ChartSeries _dyno1Series { get; set; }
        public ChartSeries Dyno1Series
        {
            get { return _dyno1Series; }
            set
            {
                _dyno1Series = value;
                OnPropertyChanged("Dyno1Series");
            }
        }

        private ChartSeries _dyno2Series { get; set; }
        public ChartSeries Dyno2Series
        {
            get { return _dyno2Series; }
            set
            {
                _dyno2Series = value;
                OnPropertyChanged("Dyno2Series");
            }
        }

        private ChartSeries _intercooler { get; set; }
        public ChartSeries Intercooler
        {
            get { return _intercooler; }
            set
            {
                _intercooler = value;
                OnPropertyChanged("Intercooler");
            }
        }

        private bool _isPlay { get; set; }
        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; OnPropertyChanged("IsPlay"); }
        }

        public StripChartViewModel()
        {
            PlotModel = new PlotModel();
            InitializeSeries();
            IsPlay = false;
            InitializePlotModel();
        }

        public StripChartViewModel(string name)
        {
            PlotModel = new PlotModel() { Title = name };
            InitializeSeries();
            IsPlay = false;
            InitializePlotModel();
        }

        private void InitializeSeries()
        {
            Dyno1Series = new ChartSeries("Dyno 1");
            Dyno2Series = new ChartSeries("Dyno 2");
            Throttle1Series = new ChartSeries("Thorttle 1");
            Throttle2Series = new ChartSeries("Throttle 2");
            CoolantSeries = new ChartSeries("Coolant");
            OilSeries = new ChartSeries("Oil");
            Intercooler = new ChartSeries("Intercooler");
        }

        private void InitializePlotModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            var dateAxis = new DateTimeAxis
            {
                Key = "Time",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Minimum = DateTimeAxis.ToDouble(DateTime.Now),
                Maximum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(30)),
                IntervalType = DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss"
            };
            var valueAxis = new LinearAxis()
            {
                Key = "Value",
                Position = AxisPosition.Left,
                StartPosition = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Output",
                Minimum = -50,
                Maximum = 50,
            };
            PlotModel.Axes.Add(dateAxis);
            PlotModel.Axes.Add(valueAxis);
        }

        public void UpdateSeries() 
        {
            PlotModel.Series.Clear();

            foreach (var propInfo in GetType().GetProperties().Where(p => p.PropertyType == typeof (ChartSeries)))
            {
                var series = propInfo.GetValue(this) as ChartSeries;
                if (series == null || !series.IsVisible || !series.DataPoints.Any())
                    continue;
                var newSeries = new LineSeries() {Title = series.SeriesName};
                newSeries.IsVisible = series.IsVisible;
                newSeries.Points.AddRange(series.DataPoints);
                PlotModel.Series.Add(newSeries);
            }
            PlotModel.InvalidatePlot(true);
        }

        public void UpdatePlot()
        {
            PlotModel.InvalidatePlot(false);
        }
    }

    public class ChartSeries
    {
        public string SeriesName { get; set; }
        public bool IsVisible { get; set; }
        public IList<DataPoint> DataPoints { get; set; }

        public ChartSeries()
        {
            IsVisible = true;
            DataPoints = new List<DataPoint>();
        }

        public ChartSeries(string name)
        {
            SeriesName = name;
            IsVisible = true;
            DataPoints = new List<DataPoint>();
        }
    }
}
