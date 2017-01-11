using System;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Extensions;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.ViewModels.StripChart
{
    public sealed class StripChartViewModel : BaseStripChartViewModel
    {
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
            PlotModel = new PlotModel() {Title = name};
            IsPlay = false;
            InitializePlotModel();
        }

        protected override void InitializePlotModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.Background = OxyColor.FromRgb(37, 37, 37);
            PlotModel.PlotAreaBackground = OxyColor.FromRgb(204, 204, 204);
            PlotModel.PlotAreaBorderColor = OxyColor.FromRgb(204, 204, 204);
            PlotModel.TextColor = OxyColor.FromRgb(204, 204, 204);
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
                //Minimum = -2,
                //Maximum = 2,
            };
            PlotModel.Axes.Add(dateAxis);
            PlotModel.Axes.Add(valueAxis);
            dateAxis = null;
            valueAxis = null;
        }

        public override void CreateSeries() 
        {
            try
            {
                PlotModel.Series.Clear();
                if (ApplicationSessionFactory.CurrentCell == null || ApplicationSessionFactory.CurrentCellTest == null || ApplicationSessionFactory.CurrentCellTest.Phases.IsNullOrEmpty())
                    return; 
                foreach (var point in ApplicationSessionFactory.CellPoints.Where(cp => cp != null && cp.IncludeInStripChart && cp.DataPoints.Any()))
                {
                    var newSeries = new LineSeries
                    {
                        Title = point.CustomName,
                        IsVisible = point.IncludeInStripChart,
                    };
                    newSeries.Points.AddRange(point.DataPoints);
                    PlotModel.Series.Add(newSeries);
                    newSeries = null;
                }
                
                PlotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                throw new Exception("UpdateSeries: " + ex.Message, ex.InnerException);
            }
        }

        public override void UpdateSeries()
        {
            if (!PlotModel.Series.Any())
            {
                CreateSeries();
                return;
            }

            foreach (var point in ApplicationSessionFactory.CellPoints.Where(cp => cp != null && cp.IncludeInStripChart && cp.DataPoints.Any()))
            {
                var series = (LineSeries)PlotModel.Series.FirstOrDefault(s => s.Title == point.CustomName);

                series?.Points.AddRange(point.DataPoints.Except(series.Points));
                series = null;
            }
            PlotModel.InvalidatePlot(true);
        }

        public void CellSelectionChange()
        {
            UpdateSeries();
        }
    }
}
