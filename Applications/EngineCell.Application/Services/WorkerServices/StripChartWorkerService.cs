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
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    var appSession = StripChartViewModel.ApplicationSessionFactory;

                    //Check if we're connected to and collecting data from Opto before proceeding
                    if (appSession.OptoMmpFactory == null || !appSession.OptoMmpFactory.Current.IsCommunicationOpen ||
                        appSession.ScratchPadFactory.GetScratchPadInt(
                            ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt()).Value != 1)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    var timePoint = DateTime.Now;

                    foreach (var cellPoint in appSession.CellPoints)
                    {
                        var point = appSession.CellPoints.FirstOrDefault(p => p.PointName == cellPoint.PointName);
                        if (point == null)
                            continue;

                        var pointEnum = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                        cellPoint.Data = Math.Truncate(appSession.ScratchPadFactory.GetScratchPadFloat(pointEnum.ToInt()).Value * 10000m) / 10000m;
                        cellPoint.DataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), Convert.ToDouble(cellPoint.Data * (point.StripChartScale ?? 1m))));
                        while (cellPoint.DataPoints.Count > 350)
                        {
                            cellPoint.DataPoints.RemoveAt(0);
                        }
                    }

                    if (!StripChartViewModel.IsPlay)
                        continue;
                        
                    StripChartViewModel.CreateSeries();
                    Thread.Sleep(50); //threshold for the series to update and the panning to be possible
                    ResetAndPanGraph(timePoint);
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
