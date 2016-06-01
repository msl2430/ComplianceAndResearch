using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using OxyPlot;
using OxyPlot.Axes;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IPidConfigStripChartWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public class PidConfigStripChartWorkerService : BaseWorkerThreadService, IPidConfigStripChartWorkerService
    {
        private PidConfigStripChartViewModel PidConfigStripChartViewModel { get; set; }

        private const int MaxDataPointSize = 1000;

        public PidConfigStripChartWorkerService(PidConfigStripChartViewModel stripChartViewModel)
        {
            PidConfigStripChartViewModel = stripChartViewModel;
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

                    var appSession = PidConfigStripChartViewModel.ApplicationSessionFactory;

                    //Check if we're connected to and collecting data from Opto before proceeding
                    if (appSession.OptoMmpFactory == null || !appSession.OptoMmpFactory.Current.IsCommunicationOpen)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    var timePoint = DateTime.Now;

                    if (PidConfigStripChartViewModel.InputDataPoints.IsNullOrEmpty())
                        PidConfigStripChartViewModel.InputDataPoints = new List<DataPoint>();
                    if (PidConfigStripChartViewModel.OutputDataPoints.IsNullOrEmpty())
                        PidConfigStripChartViewModel.OutputDataPoints = new List<DataPoint>();
                    if (PidConfigStripChartViewModel.SetPointDataPoints.IsNullOrEmpty())
                        PidConfigStripChartViewModel.SetPointDataPoints = new List<DataPoint>();

                    //Make a cap for how many data points
                    if (PidConfigStripChartViewModel.InputDataPoints.Count() > MaxDataPointSize)
                        PidConfigStripChartViewModel.InputDataPoints.Remove(PidConfigStripChartViewModel.InputDataPoints.First());
                    if (PidConfigStripChartViewModel.OutputDataPoints.Count() > MaxDataPointSize)
                        PidConfigStripChartViewModel.OutputDataPoints.Remove(PidConfigStripChartViewModel.OutputDataPoints.First());
                    if (PidConfigStripChartViewModel.SetPointDataPoints.Count() > MaxDataPointSize)
                        PidConfigStripChartViewModel.SetPointDataPoints.Remove(PidConfigStripChartViewModel.SetPointDataPoints.First());

                    PidConfigStripChartViewModel.InputDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), Convert.ToDouble(appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.InputValue.ToInt()).Value)));
                    PidConfigStripChartViewModel.OutputDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), Convert.ToDouble(appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputValue.ToInt()).Value)));
                    PidConfigStripChartViewModel.SetPointDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), Convert.ToDouble(appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.SetPointValue.ToInt()).Value)));

                    //PidConfigStripChartViewModel.InputDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), (new Random(DateTime.Now.Millisecond)).Next(-5, 5)));
                    //PidConfigStripChartViewModel.OutputDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), (new Random(DateTime.Now.Millisecond)).Next(-15, 15)));
                    //PidConfigStripChartViewModel.SetPointDataPoints.Add(new DataPoint(DateTimeAxis.ToDouble(timePoint), (new Random(DateTime.Now.Millisecond)).Next(-25, 25)));

                    if (!PidConfigStripChartViewModel.IsPlay)
                        continue;

                    PidConfigStripChartViewModel.CreateSeries();
                    Thread.Sleep(50); //threshold for the series to update and the panning to be possible
                    ResetAndPanGraph(DateTime.Now);
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
            var axis = PidConfigStripChartViewModel.PlotModel.Axes.First(a => a.Key == "Time");

            var screenPos = axis.Transform(DateTimeAxis.ToDouble(timePoint));
            var point = new ScreenPoint(screenPos, 0);
            axis.Pan(point, axis.ScreenMax);
        }
    }
}
