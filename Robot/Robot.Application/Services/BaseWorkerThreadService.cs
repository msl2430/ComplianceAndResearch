using System.Threading;
using System.Windows.Threading;

namespace Robot.Application.Services
{
    public abstract class BaseWorkerThreadService
    {
        protected readonly CancellationTokenSource CancellationToken = new CancellationTokenSource();
        
        protected Dispatcher Dispatcher { get; set; }
        public bool IsRunning { get; internal set; }

        public abstract void DoWork();
        protected abstract void WorkCompleted();

        public void CancelWork()
        {
            CancellationToken.Cancel();
        }
    }
}
