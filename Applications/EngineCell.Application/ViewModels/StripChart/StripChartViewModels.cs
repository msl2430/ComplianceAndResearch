using System;
using System.Linq;
using EngineCell.Application.Factories;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.ViewModels.StripChart
{
    public class StripChartViewModel : BaseViewModel
    {
        public PlotModel PlotModel { get; set; }

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
            IsPlay = false;
            InitializePlotModel();
        }

        public StripChartViewModel(string name, IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
            PlotModel = new PlotModel() { Title = name };
            IsPlay = false;
            InitializePlotModel();
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
                Minimum = -2,
                Maximum = 2,
            };
            PlotModel.Axes.Add(dateAxis);
            PlotModel.Axes.Add(valueAxis);
        }

        public void CreateSeries() 
        {
            try
            {
                PlotModel.Series.Clear();
                foreach (var point in ApplicationSessionFactory.CellPoints)
                {
                    if (point == null || !point.IncludeInStripChart || !point.DataPoints.Any())
                        continue;

                    var newSeries = new LineSeries
                    {
                        Title = point.CustomName,
                        IsVisible = point.IncludeInStripChart
                    };
                    newSeries.Points.AddRange(point.DataPoints);
                    PlotModel.Series.Add(newSeries);
                }
                PlotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateSeries: " + ex.Message, ex.InnerException);
            }
        }

        public void UpdateSeries()
        {
            if (!PlotModel.Series.Any())
            {
                CreateSeries();
                return;
            }

            foreach (var point in ApplicationSessionFactory.CellPoints)
            {
                if (point == null || !point.IncludeInStripChart || !point.DataPoints.Any())
                    continue;

                var series = (LineSeries)PlotModel.Series.FirstOrDefault(s => s.Title == point.CustomName);
                if (series == null)
                    continue;

                series.IsVisible = point.IncludeInStripChart;
                series.Points.AddRange(point.DataPoints.Except(series.Points));
            }
            PlotModel.InvalidatePlot(true);
        }
    }
}
