using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using EngineCell.Application.ViewModels.StripChart;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace EngineCell.Application.Services.WorkerServices
{
    public class StripChartWorkerService : BaseWorkerThreadService
    {
        public StripChartViewModel StripChartViewModel { get; set; }

        public StripChartWorkerService(StripChartViewModel stripChartViewModel)
        {
            StripChartViewModel = stripChartViewModel;
            WaitStopWatch = new Stopwatch();
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10 && StripChartViewModel.IsPlay)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        var timePoint = DateTime.Now;

                        foreach (
                            var series in
                                StripChartViewModel.GetType()
                                    .GetProperties()
                                    .Where(p => p.PropertyType == typeof (ChartSeries))
                                    .Select(propInfo => propInfo.GetValue(StripChartViewModel) as ChartSeries)
                                    .Where(series => series != null && series.IsVisible))
                        {
                            series.DataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint),
                                new Random(DateTime.Now.Millisecond).Next(maxValue: 50, minValue: -50)));
                            Thread.Sleep(10);
                        }

                        ResetAndPanGraph(timePoint);
                        StripChartViewModel.UpdateSeries();
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void WorkCompleted()
        {
            
        }

        private void ResetAndPanGraph(DateTime timePoint)
        {
            var axis = StripChartViewModel.PlotModel.Axes.First(a => a.Key == "Time");

            var screenPos = axis.Transform(DateTimeAxis.ToDouble(timePoint));
            var point = new ScreenPoint(screenPos, 0);
            axis.Pan(point, axis.ScreenMax);
        }
    }
}
