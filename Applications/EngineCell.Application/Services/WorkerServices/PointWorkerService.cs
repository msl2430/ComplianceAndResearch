using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.TestDisplay;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IPointWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public class PointWorkerService : BaseWorkerThreadService, IPointWorkerService
    {
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }
        private TestDisplayViewModel TestDisplayViewModel { get; set; }

        public PointWorkerService(TestDisplayViewModel viewModel, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            TestDisplayViewModel = viewModel;
            ApplicationSessionFactory = viewModel.ApplicationSessionFactory;
        }

        public override void DoWork()
        {
            try
            {
                var appSession = ApplicationSessionFactory;
                appSession.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt(), 1);
                CancellationToken = new CancellationTokenSource();
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > 10)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (appSession.OptoMmpFactory != null && appSession.OptoMmpFactory.Current.IsCommunicationOpen && appSession.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.StartDataCollection.ToInt()).Value == 1)
                        {
                            foreach (var cellPoint in appSession.CellPoints)
                            {
                                var point = (ScratchPadConstants.FloatIndexes)Enum.Parse(typeof(ScratchPadConstants.FloatIndexes), cellPoint.PointName, true);
                                cellPoint.Data = Math.Truncate(appSession.ScratchPadFactory.GetScratchPadFloat(point.ToInt()).Value * 10000m) / 10000m;
                                if (!cellPoint.IsAverage || cellPoint.AverageSeconds == null)
                                    continue;

                                cellPoint.MostRecentData.Add(cellPoint.Data);
                                if(cellPoint.MostRecentData.Count() > cellPoint.AverageSeconds * 2) //2 because we're capturing 2 data points per second
                                    cellPoint.MostRecentData.RemoveAt(0);
                                
                                cellPoint.AverageData = Math.Truncate(cellPoint.MostRecentData.Average() * 10000m) / 10000m; 
                            }
                            if (appSession.CurrentCellTest != null)
                                CellPointRepository.CreateCellPointData(appSession.CellPoints.Where(cp => cp.IsRecord).SelectDistinct(cp => cp.ToCellTestPointDataModel(appSession.CurrentCellTest.CellTestId)).ToList());

                            TestDisplayViewModel.UpdateVisibleCellPoints();

                            TestDisplayViewModel.VentControl1Display.Inside = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Inside.ToInt()).Value;
                            TestDisplayViewModel.VentControl1Display.Outside = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Outside.ToInt()).Value;
                            TestDisplayViewModel.VentControl1Display.Output = appSession.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.VentCtrl1Output.ToInt()).Value;
                        }
                    }
                    Thread.Sleep(500);
                }
                WorkCompleted();
            }
            catch (TaskCanceledException ex)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => ApplicationSessionFactory.LogEvent("Error in point data collection: " + ex.Message, true));
            }
        }

        protected override void WorkCompleted()
        {
            Dispatcher.Invoke(() => ApplicationSessionFactory.LogEvent("Point data collection stopped.", true));
        }
    }
}
