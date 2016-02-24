using System;
using System.Threading;
using System.Threading.Tasks;
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

        public PointWorkerService(IApplicationSessionFactory appSession)
        {
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
                                var point = (ScratchPadConstants.IntegerIndexes)Enum.Parse(typeof (ScratchPadConstants.IntegerIndexes), cellPoint.PointName, true);
                                cellPoint.Data = appSession.ScratchPadFactory.GetScratchPadFloat(point.ToInt()).Value;
                            }
                        }
                    }
                    Thread.Sleep(1000);
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
    }
}
