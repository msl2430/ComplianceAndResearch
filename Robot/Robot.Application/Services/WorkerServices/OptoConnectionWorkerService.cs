using System;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Opto22.Core.Constants;
using Opto22.Core.Models;
using Robot.Application.Extensions;
using Robot.Application.Factories;
using Robot.Core.Constants;
using Robot.Core.Extensions;

namespace Robot.Application.Services.WorkerServices
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
        
        private IScratchPadModel<bool> ConnectedScratchPad { get; set; }
        private IApplicationSessionFactory ApplicationSessionFactory { get; set; }

        
        public OptoConnectionWorkerService(IOptoMmpFactory optoMmpFactory, Dispatcher currentDispatcher, Label statusLabel)
        {
            
        }

        public OptoConnectionWorkerService(IApplicationSessionFactory applicationSessionFactory, Dispatcher currentDispatcher)
        {
            Dispatcher = currentDispatcher;
            ApplicationSessionFactory = applicationSessionFactory;
            ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnected;
            _interval = 500;
            _successCount = 0;
        }

        private void ToggleScratchPadConnectBit()
        {
            if(ConnectedScratchPad == null)
                ConnectedScratchPad = ApplicationSessionFactory.ScratchPadFactory.GetScratchPadBit(ScratchPadConstants.BitIndexes.ConnectedToOpto.ToInt());

            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Connected:
                    ConnectedScratchPad.Value = true;
                    ConnectedScratchPad.Update();
                    break;
                case StatusConstants.ConnectionStatus.Disconnected:
                    ConnectedScratchPad.Value = false;
                    ConnectedScratchPad.Update();
                    break;
            }
        }
        public override void DoWork()
        {
            try
            {
                ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Connecting;
                CancellationToken = new CancellationTokenSource();
                if(ApplicationSessionFactory.OptoMmpFactory.OpenConnection() != 0)
                    throw new Exception("Error connecting to Opto.");

                ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Connected;
                ApplicationSessionFactory.LogEvent(_successMessage, true);
                Dispatcher.Invoke(() => ApplicationSessionFactory.ControlWindowViewModel.StatusLabel = _successMessage);
                ToggleScratchPadConnectBit();

                IsRunning = true;
                while (!CancellationToken.IsCancellationRequested)
                {
                    var isConnected = ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen;
                    if(!isConnected)
                        Dispatcher.Invoke(CallbackAction(), DispatcherPriority.Normal);

                    AdjustInterval(isConnected);
                    Thread.Sleep(_interval);
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
                ApplicationSessionFactory.ControlWindowViewModel.StatusLabel = "Connection lost";
                //TODO More Event Handling for lost Opto connection
            };
        }

        protected override void WorkCompleted()
        {
            IsRunning = false;
            ApplicationSessionFactory.OptoMmpFactory.CloseConnection();
            CancellationToken = new CancellationTokenSource();
            ApplicationSessionFactory.OptoConnectionStatus = StatusConstants.ConnectionStatus.Disconnected;
            Dispatcher.Invoke(() =>
            {
                ApplicationSessionFactory.ControlWindowViewModel.StatusLabel = "Disconnected";
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
