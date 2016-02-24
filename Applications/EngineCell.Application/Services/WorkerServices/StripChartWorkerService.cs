using System;
using System.Linq;
using System.Threading;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using OxyPlot;
using OxyPlot.Axes;

namespace EngineCell.Application.Services.WorkerServices
{
    public class StripChartWorkerService : BaseWorkerThreadService
    {
        private StripChartViewModel StripChartViewModel { get; set; }

        public StripChartWorkerService(StripChartViewModel stripChartViewModel)
        {
            StripChartViewModel = stripChartViewModel;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        var appSession = StripChartViewModel.ApplicationSessionFactory;

                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (appSession.OptoMmpFactory == null || !appSession.OptoMmpFactory.Current.IsCommunicationOpen || appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt()).Value != 1)
                            continue;

                        var timePoint = DateTime.Now;

                        foreach (var series in StripChartViewModel.Series)
                        {
                            var point = appSession.CellPoints.FirstOrDefault(p => p.PointName == series.SeriesName);
                            if (point == null)
                                continue;
                            series.DataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), Convert.ToDouble(point.Data)));
                            Thread.Sleep(10);
                        }

                        if (!StripChartViewModel.IsPlay)
                            continue;
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
