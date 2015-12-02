using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace EngineCell.Application.Services.WorkerServices
{
    public abstract class BaseWorkerThreadService
    {
        protected CancellationTokenSource CancellationToken;
        protected Stopwatch WaitStopWatch { get; set; }
        protected Dispatcher Dispatcher { get; set; }
        public bool IsRunning { get; internal set; }

        public abstract void DoWork();
        protected abstract void WorkCompleted();

        public bool IsCancelPending()
        {
            return CancellationToken.IsCancellationRequested;
        }

        public void CancelWork()
        {
            CancellationToken.Cancel();
        }
    }
}
