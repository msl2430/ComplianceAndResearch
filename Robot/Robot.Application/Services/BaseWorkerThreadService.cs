using System.Threading;
using System.Windows.Threading;

namespace Robot.Application.Services
{
    public abstract class BaseWorkerThreadService
    {
        protected CancellationTokenSource CancellationToken;
        
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
