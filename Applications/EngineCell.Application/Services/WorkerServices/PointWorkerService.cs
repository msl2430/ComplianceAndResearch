using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using EngineCell.Application.Factories;
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

        public PointWorkerService(IApplicationSessionFactory appSession, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ApplicationSessionFactory = appSession;
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
                                cellPoint.Data = appSession.ScratchPadFactory.GetScratchPadFloat(point.ToInt()).Value;                                
                            }
                            if (appSession.CurrentCellTest != null)
                                CellPointRepository.CreateCellPointData(appSession.CellPoints.SelectDistinct(cp => cp.ToCellTestPointDataModel(appSession.CurrentCellTest.CellTestId)).ToList());
                        }
                    }
                    Thread.Sleep(1000);
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
