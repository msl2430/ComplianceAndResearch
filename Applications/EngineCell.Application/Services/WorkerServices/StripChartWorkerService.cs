using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using Opto22.Core.Models;
using OxyPlot;
using OxyPlot.Axes;

namespace EngineCell.Application.Services.WorkerServices
{
    public class StripChartWorkerService : BaseWorkerThreadService
    {
        private StripChartViewModel StripChartViewModel { get; set; }
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }
        private DateTime CaptureTime { get; set; }
        private IList<IScratchPadModel<decimal>> ScratchPadValues { get; set; }
        private IScratchPadModel<decimal> ScratchPadValue { get; set; }

        public StripChartWorkerService(StripChartViewModel stripChartViewModel, Dispatcher currentDispatcher)
        {
            StripChartViewModel = stripChartViewModel;
            Dispatcher = currentDispatcher;
        }

        public override void DoWork()
        {
            try
            {
                CancellationToken = new CancellationTokenSource();
                ApplicationSessionFactory = StripChartViewModel.ApplicationSessionFactory;
                ScratchPadValues = new List<IScratchPadModel<decimal>>();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.IsHardwareSafety.ToInt()) == 1)
                    {
                        CancellationToken.Cancel();
                        continue;
                    }

                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds <= 10) continue;

                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();

                    //Check if we're connected to and collecting data from Opto before proceeding
                    if (ApplicationSessionFactory.OptoMmpFactory == null || !ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen ||
                        ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt()) != 1)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    CaptureTime = DateTime.Now;
                    ScratchPadValues = ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatRange(1000, 1073);
                    foreach (var cellPoint in ApplicationSessionFactory.CellPoints)
                    {
                        ScratchPadValue = ScratchPadValues.FirstOrDefault(sc => sc.Index == ((ScratchPadConstants.FloatIndexes) Enum.Parse(typeof (ScratchPadConstants.FloatIndexes), cellPoint.PointName, true)).ToInt());
                        if (ScratchPadValue == null)
                            continue;
                        cellPoint.DataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(CaptureTime), Convert.ToDouble(Math.Truncate(ScratchPadValue.Value * 10000m) / 10000m * (cellPoint.StripChartScale ?? 1m))));
                        ScratchPadValue = null;
                    }

                    foreach (var maxDataPoints in StripChartViewModel.ApplicationSessionFactory.CellPoints.Where(cp => cp.DataPoints.Count > 500))
                    {
                        maxDataPoints.DataPoints.RemoveAt(0);
                    }

                    if (!StripChartViewModel.IsPlay)
                        continue;

                    StripChartViewModel.UpdateSeries();
                    Thread.Sleep(150); //threshold for the series to update and the panning to be possible
                    ResetAndPanGraph(CaptureTime);
                }
            }
            catch (TaskCanceledException ex)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void WorkCompleted()
        {
            
        }

        private Axis TempAxis { get; set; }
        private ScreenPoint TempScreenPoint { get; set; }

        private void ResetAndPanGraph(DateTime timePoint)
        {
            try
            {
                TempAxis = StripChartViewModel.PlotModel.Axes.First(a => a.Key == "Time");

                var screenPos = TempAxis.Transform(DateTimeAxis.ToDouble(timePoint));
                TempScreenPoint = new ScreenPoint(screenPos, 0);
                TempAxis.Pan(TempScreenPoint, TempAxis.ScreenMax);
                //using (var file = new StreamWriter(@"D:\test.txt", true))
                //{
                //    file.WriteLine("{0} >> ScreenPos: {1} Point: {2} Max: {3}", DateTime.Now.ToShortTimeString(), screenPos, point, axis.ScreenMax);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
