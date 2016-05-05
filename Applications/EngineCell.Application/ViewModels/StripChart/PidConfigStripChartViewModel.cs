using System;
using System.Collections.Generic;
using System.Linq;
using EngineCell.Application.Factories;
using EngineCell.Core.Extensions;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.ViewModels.StripChart
{
    public class PidConfigStripChartViewModel: BaseStripChartViewModel
    {
        public IList<DataPoint> InputDataPoints { get; set; }

        public IList<DataPoint> OutputDataPoints { get; set; } 

        public IList<DataPoint> SetPointDataPoints { get; set; } 

        public PidConfigStripChartViewModel(IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
            PlotModel = new PlotModel();
            IsPlay = true;
            InitializePlotModel();
        }

        public PidConfigStripChartViewModel(string name, IApplicationSessionFactory applicationSessionFactory)
        {
            ApplicationSessionFactory = applicationSessionFactory;
            PlotModel = new PlotModel() { Title = name };
            IsPlay = true;
            InitializePlotModel();
        }

        protected override void InitializePlotModel()
        {
            PlotModel.LegendTitle = "Legend";
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.BottomCenter;
            PlotModel.Background = OxyColor.FromRgb(37,37,37);
            PlotModel.PlotAreaBackground = OxyColor.FromRgb(204,204,204);
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
        }

        public override void CreateSeries()
        {
            try
            {
                PlotModel.Series.Clear();

                var inputSeries = new LineSeries { Title = "Input", IsVisible = true };
                inputSeries.Points.AddRange(InputDataPoints);
                PlotModel.Series.Add(inputSeries);
                var outputSeries = new LineSeries { Title = "Output", IsVisible = true };
                outputSeries.Points.AddRange(OutputDataPoints);
                PlotModel.Series.Add(outputSeries);
                var setpointSeries = new LineSeries { Title = "Set Point", IsVisible = true };
                setpointSeries.Points.AddRange(SetPointDataPoints);
                PlotModel.Series.Add(setpointSeries);

                PlotModel.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                throw new Exception("CreateSeries(PidConfig): " + ex.Message, ex.InnerException);
            }
        }

        public override void UpdateSeries()
        {
            throw new NotImplementedException();
        }
    }
}
