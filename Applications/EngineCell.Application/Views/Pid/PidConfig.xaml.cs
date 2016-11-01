using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Factories;
using EngineCell.Application.Services.WorkerServices;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Application.ViewModels.StripChart;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;
using NHibernate.Util;

namespace EngineCell.Application.Views.Pid
{
    /// <summary>
    /// Interaction logic for PidConfig.xaml
    /// </summary>
    public partial class PidConfig : Window
    {
        public PidConfigViewModel ViewModel { get; set; }

        private IPidConfigWorkerService PidConfigWorkerService { get; set; }

        private IPidConfigStripChartWorkerService PidConfigStripChartWorkerService { get; set; }

        public PidConfig(IApplicationSessionFactory sessionFactory, ControlConstants.PidType pidType)
        {
            InitializeComponent();
            ViewModel = new PidConfigViewModel(sessionFactory) { PidType = pidType, StripChartViewModel = new PidConfigStripChartViewModel("PID Trace", sessionFactory) };
            PidConfigStripChartWorkerService = new PidConfigStripChartWorkerService(ViewModel.StripChartViewModel);
            DataContext = ViewModel;
            InitializeForm();
            
            Task.Run(() => PidConfigStripChartWorkerService.DoWork()).ConfigureAwait(false);
        }

        private void InitializeForm()
        {
            GetPidValues();
            PidConfigWorkerService = new PidConfigWorkerService(ViewModel);
            Task.Run(() =>
            {
                PidConfigWorkerService.DoWork();
            }).ConfigureAwait(false);

            switch (ViewModel.PidType)
            {
                case ControlConstants.PidType.Dyno:
                    Title = "Dyno Pid Configuration";
                    break;
                case ControlConstants.PidType.Throttle:
                    Title = "Throttle Pid Configuration";
                    break;
                case ControlConstants.PidType.Coolant:
                    Title = "Coolant Pid Configuration";
                    break;
                case ControlConstants.PidType.Oil:
                    Title = "Oil Pid Configuration";
                    break;
                case ControlConstants.PidType.Intercooler:
                    Title = "Intercooler Pid Configuration";
                    break;
                case ControlConstants.PidType.Custom1:
                    Title = "Custom1 Pid Configuration";
                    break;
                case ControlConstants.PidType.Custom2:
                    Title = "Custom2 Pid Configuration";
                    break;
                case ControlConstants.PidType.Custom3:
                    Title = "Custom3 Pid Configuration";
                    break;
                case ControlConstants.PidType.Custom4:
                    Title = "Custom4 Pid Configuration";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ViewModel.ApplicationSessionFactory.OptoMmpFactory != null && ViewModel.ApplicationSessionFactory.OptoMmpFactory.Current.IsCommunicationOpen)
                return;

            OkButton.IsEnabled = false;
            ApplyButton.IsEnabled = false;
            ViewModel.IsSetPointDirty = false;
        }

        private void GetPidValues()
        {
            Task.Run(() =>
            {
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigType.ToInt(), ViewModel.PidType.ToInt());
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt(), StatusConstants.ProgressStatus.Active.ToInt());
                while (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt()) != StatusConstants.ProgressStatus.Finished.ToInt())
                {
                    Thread.Sleep(500);
                }
                ViewModel.LowRange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.InputLowRange.ToInt());
                ViewModel.HighRange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.InputHighRange.ToInt());
                ViewModel.LowerClamp = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.OutputLowerClamp.ToInt());
                ViewModel.UpperClamp = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.OutputUpperClamp.ToInt());
                ViewModel.MinChange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.OutputMinChange.ToInt());
                ViewModel.MaxChange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.OutputMaxChange.ToInt());
                ViewModel.Gain = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.Gain.ToInt());
                ViewModel.TuneI = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.TuneI.ToInt());
                ViewModel.TuneD = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.TuneD.ToInt());
                ViewModel.FeedFwdInitial = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.FeedFwdInitial.ToInt());
                ViewModel.FeedFwdGain = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloatValue(ScratchPadConstants.FloatIndexes.FeedFwdGain.ToInt());                
            });
        }

        private void UpdatePidValues(bool isExitWhenDone)
        {
            Task.Run(() =>
            {
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.InputLowRange.ToInt(), ViewModel.LowRange);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.InputHighRange.ToInt(), ViewModel.HighRange);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.OutputLowerClamp.ToInt(), ViewModel.LowerClamp);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.OutputUpperClamp.ToInt(), ViewModel.UpperClamp);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.OutputMinChange.ToInt(), ViewModel.MinChange);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.OutputMaxChange.ToInt(), ViewModel.MaxChange);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.Gain.ToInt(), ViewModel.Gain);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.TuneI.ToInt(), ViewModel.TuneI);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.TuneD.ToInt(), ViewModel.TuneD);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.FeedFwdInitial.ToInt(), ViewModel.FeedFwdInitial);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.FeedFwdGain.ToInt(), ViewModel.FeedFwdInitial);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt(), StatusConstants.ProgressStatus.Active.ToInt());
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.FloatIndexes.SetPointValue.ToInt(), ViewModel.SetPoint);
                while (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadIntValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt()) != StatusConstants.ProgressStatus.Finished.ToInt())
                {
                    Thread.Sleep(500);
                }
                ViewModel.IsSetPointDirty = false;
                if (isExitWhenDone)
                    Dispatcher.Invoke(Close);
            });
        }

        private void PidConfig_OnClosing(object sender, CancelEventArgs e)
        {
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt(), StatusConstants.ProgressStatus.InActive.ToInt());
            PidConfigWorkerService.CancelWork();
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdatePidValues(false);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdatePidValues(true);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetPoint_OnChange(object sender, TextChangedEventArgs e)
        {
            if(SetPointTextBox.IsFocused)
                ViewModel.IsSetPointDirty = true;
        }
    }
}
