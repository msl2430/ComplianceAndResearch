using System;
using System.Configuration;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using Robot.Application.Factories;

namespace Robot.Application.Services
{
    public sealed class OptoConnectionWorkerService : BaseWorkerThreadService
    {
        private IOptoMmpFactory OptoMmpFactory { get; set; }
        private Label StatusLabel { get; set; }

        public OptoConnectionWorkerService(IOptoMmpFactory optoMmpFactory, Dispatcher currentDispatcher, Label statusLabel)
        {
            OptoMmpFactory = optoMmpFactory;
            Dispatcher = currentDispatcher;
            StatusLabel = statusLabel;
            ApplicationSession.IsConnectedToOpto = false;
        }

        public override void DoWork()
        {
            try
            {
                IsRunning = true;
                while (!CancellationToken.IsCancellationRequested)
                {

                    var isConnected = OptoMmpFactory.Current.IsCommunicationOpen;
                    Dispatcher.Invoke(CallbackAction(isConnected), DispatcherPriority.Normal);
                    Thread.Sleep(500);
                }
                WorkCompleted();
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                IsRunning = false;
                if(OptoMmpFactory.Current.IsCommunicationOpen)
                    OptoMmpFactory.Current.Close();
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
            Dispatcher.Invoke(() => { StatusLabel.Content = "Disconnected"; }, DispatcherPriority.Normal);
            OptoMmpFactory.Current.Close();
        }

       
    }
}
