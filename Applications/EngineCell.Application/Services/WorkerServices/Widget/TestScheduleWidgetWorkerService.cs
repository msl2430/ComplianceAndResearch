using System;
using System.Threading;
using System.Threading.Tasks;
using EngineCell.Application.ViewModels.Widget;

namespace EngineCell.Application.Services.WorkerServices.Widget
{
    public sealed class TestScheduleWidgetWorkerService : BaseWorkerThreadService
    {
        private TestScheduleDisplayViewModel ViewModel { get; set; }

        public TestScheduleWidgetWorkerService(TestScheduleDisplayViewModel viewModel)
        {
            ViewModel = viewModel;
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

                        //Check if we're connected to and collecting data from Opto before proceeding
                        if (ViewModel.ApplicationSessionFactory.OptoMmpFactory == null || !ViewModel.ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                ViewModel.ApplicationSessionFactory.LogEvent("Erorr in Test Schedule Widget: " + ex.Message, true);
                throw ex;
            }
        }

        protected override void WorkCompleted()
        {
        }
    }
}
