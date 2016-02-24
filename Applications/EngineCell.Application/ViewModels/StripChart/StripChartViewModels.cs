using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Application.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.ViewModels.StripChart
{
    public class StripChartViewModel : BaseViewModel
    {
        public PlotModel PlotModel { get; set; }

        private IList<ChartSeries> _series { get; set; }

        public IList<ChartSeries> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }

        private bool _isPlay { get; set; }
        public bool IsPlay
        {
            get { return _isPlay; }
            set { _isPlay = value; OnPropertyChanged("IsPlay"); }
        }

        public StripChartViewModel(IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
            PlotModel = new PlotModel();
            Series = new List<ChartSeries>();
            InitializeSeries();
            IsPlay = false;
            InitializePlotModel();
        }

        public StripChartViewModel(string name, IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
            PlotModel = new PlotModel() { Title = name };
            Series = new List<ChartSeries>();
            InitializeSeries();
            IsPlay = false;
            InitializePlotModel();
        }

        private void InitializeSeries()
        {
            foreach (var point in ApplicationSessionFactory.CellPoints)
            {
                Series.Add(new ChartSeries(point));
            }
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
            foreach (var series in Series)
            {
                if (series == null || !series.IsVisible || !series.DataPoints.Any())
                    continue;
                var newSeries = new LineSeries
                {
                    Title = series.SeriesName,
                    IsVisible = series.IsVisible
                };
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
        public int PointId { get; set; }
        public string SeriesName { get; set; }
        public string CustomName { get; set; }
        public bool IsVisible { get; set; }
        public IList<DataPoint> DataPoints { get; set; }

        public ChartSeries()
        {
            IsVisible = true;
            DataPoints = new List<DataPoint>();
        }

        public ChartSeries(PointDataModel point)
        {
            PointId = point.CellPointId;
            SeriesName = point.PointName;
            CustomName = point.CustomName;
            IsVisible = true;
            DataPoints = new List<DataPoint>();
        }
    }
}
