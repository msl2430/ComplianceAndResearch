using System;
using System.Threading;
using System.Windows.Threading;
using EngineCell.Application.Views.Clocks;
using EngineCell.Application.Views.TestDisplay;

namespace EngineCell.Application.Services.WorkerServices
{
    public class TestRunnerWorkerService : BaseWorkerThreadService
    {
        private TimeRemaining TimeRemaining { get; set; }
        private TestDisplay TestDisplay { get; set; }
        private Dispatcher Dispatcher { get; set; }

        public TestRunnerWorkerService(TimeRemaining timeRemaining, TestDisplay testDisplay, Dispatcher currentDispatcher)
        {
            TimeRemaining = timeRemaining;
            TestDisplay = testDisplay;
            Dispatcher = currentDispatcher;
        }

        public override void DoWork()
        {
            CancellationToken = new CancellationTokenSource();
            while (!CancellationToken.IsCancellationRequested && TimeRemaining.RemainingTime != TimeSpan.Zero)
            {
                if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                if (WaitStopWatch.ElapsedMilliseconds > 100)
                {
                    WaitStopWatch.Stop();
                    WaitStopWatch.Reset();
                }
            }
            Thread.Sleep(1100);
            WorkCompleted();
        }

        protected override void WorkCompleted()
        {
            TestDisplay.StopTest(false);
        }
    }
}
