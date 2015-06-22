using System;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Opto22.Core.Constants;
using Opto22.Core.Models;
using Robot.Application.Extensions;
using Robot.Application.Factories;
using Robot.Application.Session;
using Robot.Core.Extensions;

namespace Robot.Application.Services
{
    public sealed class OptoConnectionWorkerService : BaseWorkerThreadService
    {
        private IOptoMmpFactory OptoMmpFactory { get; set; }
        private Label StatusLabel { get; set; }
        private int _interval;
        private int _successCount;
        private const int MaximumWaitPeriod = 300000;
        private ScratchPadModel<bool> ConnectedScratchPad { get; set; }

        public OptoConnectionWorkerService(IOptoMmpFactory optoMmpFactory, Dispatcher currentDispatcher, Label statusLabel)
        {
            OptoMmpFactory = optoMmpFactory;
            Dispatcher = currentDispatcher;
            StatusLabel = statusLabel;
            ConnectedScratchPad = ApplicationSession.ScratchPadFactory.GetScratchPadBit(ScratchPadConstants.BitIndexes.ConnectedToOpto.ToInt());
            ApplicationSession.IsConnectedToOpto = false;
            _interval = 500;
            _successCount = 0;
        }

        public override void DoWork()
        {
            try
            {
                ConnectedScratchPad.Value = true;
                ConnectedScratchPad.Update();
                IsRunning = true;
                while (!CancellationToken.IsCancellationRequested)
                {
                    var isConnected = OptoMmpFactory.Current.IsCommunicationOpen;
                    Dispatcher.Invoke(CallbackAction(isConnected), DispatcherPriority.Normal);
                    AdjustInterval(isConnected);
                    Thread.Sleep(_interval);
                }
                WorkCompleted();
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                IsRunning = false;
                if (OptoMmpFactory.Current.IsCommunicationOpen)
                {
                    ConnectedScratchPad.Value = false;
                    ConnectedScratchPad.Update();
                    OptoMmpFactory.Current.Close();
                }
            }
        }

        private Action CallbackAction(bool isConnected)
        {
            return () =>
            {
                StatusLabel.Content = isConnected ? "Connected: " + ConfigurationManager.AppSettings["OptoIpAddress"] + ":" + ConfigurationManager.AppSettings["OptoMmpPort"] : "Error Connecting";
                ApplicationSession.IsConnectedToOpto = isConnected;
            };
        }

        protected override void WorkCompleted()
        {
            IsRunning = false;
            ConnectedScratchPad.Value = false;
            ConnectedScratchPad.Update();
            Dispatcher.Invoke(() => { StatusLabel.Content = "Disconnected"; }, DispatcherPriority.Normal);
            OptoMmpFactory.Current.Close();
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
