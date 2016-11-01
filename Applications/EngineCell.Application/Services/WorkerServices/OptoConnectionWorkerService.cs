using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using EngineCell.Application.Factories;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Services.WorkerServices
{
    public interface IOptoConnectionWorkerService
    {
        void DoWork();
        bool IsRunning { get; }
        bool IsCancelPending();
        void CancelWork();
    }

    public sealed class OptoConnectionWorkerService : BaseWorkerThreadService, IOptoConnectionWorkerService
    {
        private int _interval;
        private int _successCount;
        private const int MaximumWaitPeriod = 300000;
        private readonly string _successMessage = "Connected: " + ConfigurationManager.AppSettings["OptoIpAddress"] + ":" + ConfigurationManager.AppSettings["OptoMmpPort"];
        
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }
        private bool IsConnected { get; set; }
        private int ScratchPadValue { get; set; }

        public OptoConnectionWorkerService(IOptoMmpFactory optoMmpFactory, Dispatcher currentDispatcher, Label statusLabel)
        {

        }

        public OptoConnectionWorkerService(IApplicationSessionFactory applicationSessionFactory, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ApplicationSessionFactory = applicationSessionFactory;
            ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnected;
            WaitStopWatch = new Stopwatch();
            _interval = 500;
            _successCount = 0;
        }

        private void ToggleScratchPadConnectBit()
        {
            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Connecting:
                case StatusConstants.ConnectionStatus.Connected:
                    ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ConnectedToOpto.ToInt(), 1);
                    break;
                case StatusConstants.ConnectionStatus.Disconnecting:
                case StatusConstants.ConnectionStatus.Disconnected:
                    ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ConnectedToOpto.ToInt(), 0);
                    break;
            }
        }
        public override void DoWork()
        {
            try
            {
                ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Connecting;
                CancellationToken = new CancellationTokenSource();
                if (ApplicationSessionFactory.OptoMmpFactory.OpenConnection() != 0)
                {
                    ApplicationSessionFactory.LogEvent("Error connecting to Opto.", true);
                    ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnecting;
                    WorkCompleted();
                    return;
                }

                ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Connected;
                ApplicationSessionFactory.LogEvent(_successMessage, true);
                Dispatcher.Invoke(() => ApplicationSessionFactory.ApplicationViewModel.StatusLabel = _successMessage);
                ToggleScratchPadConnectBit();

                IsRunning = true;
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (!WaitStopWatch.IsRunning) WaitStopWatch.Start();
                    if (WaitStopWatch.ElapsedMilliseconds > _interval)
                    {
                        WaitStopWatch.Stop();
                        WaitStopWatch.Reset();
                        IsConnected = ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen;
                        ScratchPadValue = ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.StrategyLocationValue.ToInt());
                        ApplicationSessionFactory.ApplicationViewModel.StatusLabel = _successMessage;
                        if (!IsConnected || ScratchPadValue == 0)
                            Dispatcher.Invoke(CallbackAction(), DispatcherPriority.Normal);
                        AdjustInterval(IsConnected);
                    }
                    Thread.Sleep(500);
                }
                ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnecting;
                WorkCompleted();
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                IsRunning = false;
                if (ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                {
                    ToggleScratchPadConnectBit();
                    ApplicationSessionFactory.OptoMmpFactory.CloseConnection();
                }
            }
        }

        private Action CallbackAction()
        {
            return () =>
            {
                ApplicationSessionFactory.ApplicationViewModel.StatusLabel = "Connection lost or Opto not running";
                //TODO More Event Handling for lost Opto connection
            };
        }

        protected override void WorkCompleted()
        {
            IsRunning = false;
            ToggleScratchPadConnectBit();
            ApplicationSessionFactory.OptoMmpFactory.CloseConnection();
            CancellationToken = new CancellationTokenSource();
            ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnected;
            Dispatcher.Invoke(() =>
            {
                ApplicationSessionFactory.ApplicationViewModel.StatusLabel = "Disconnected";
                ApplicationSessionFactory.LogEvent("Disconnected", true);
            }, DispatcherPriority.Normal);
        }

        /// <summary>
        /// Increase the wait period before checkins if consistency successful
        /// </summary>
        /// <param name="isConnected"></param>
        private void AdjustInterval(bool isConnected)
        {
            if (isConnected)
                _successCount++;
            else
            {
                _successCount = 0;
                _interval = 500;
            }
            if (_successCount % 3 == 0 && _interval < MaximumWaitPeriod)
                _interval *= 2;
        }
    }
}
