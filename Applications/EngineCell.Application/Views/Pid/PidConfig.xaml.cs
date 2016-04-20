using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EngineCell.Application.Factories;
using EngineCell.Application.ViewModels.Pid;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.Views.Pid
{
    /// <summary>
    /// Interaction logic for PidConfig.xaml
    /// </summary>
    public partial class PidConfig : Window
    {
        public PidConfigViewModel ViewModel { get; set; }

        public PidConfig(IApplicationSessionFactory sessionFactory, ControlConstants.PidType pidType)
        {
            InitializeComponent();
            ViewModel = new PidConfigViewModel(sessionFactory) { PidType = pidType };
            DataContext = ViewModel;
            GetPidValues();
        }

        private void GetPidValues()
        {
            Task.Run(() =>
            {
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigType.ToInt(), ViewModel.PidType.ToInt());
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt(), StatusConstants.ProgressStatus.Active.ToInt());
                while (ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadInt(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt()).Value != StatusConstants.ProgressStatus.Finished.ToInt())
                {
                    Thread.Sleep(500);
                }
                ViewModel.LowRange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.InputLowRange.ToInt()).Value;
                ViewModel.HighRange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.InputHighRange.ToInt()).Value;
                ViewModel.LowerClamp = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputLowerClamp.ToInt()).Value;
                ViewModel.UpperClamp = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputUpperClamp.ToInt()).Value;
                ViewModel.MinChange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputMinChange.ToInt()).Value;
                ViewModel.MaxChange = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.OutputMaxChange.ToInt()).Value;
                ViewModel.Gain = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.Gain.ToInt()).Value;
                ViewModel.TuneI = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TuneI.ToInt()).Value;
                ViewModel.TuneD = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.TuneD.ToInt()).Value;
                ViewModel.FeedFwdInitial = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.FeedFwdInitial.ToInt()).Value;
                ViewModel.FeedFwdGain = ViewModel.ApplicationSessionFactory.ScratchPadFactory.GetScratchPadFloat(ScratchPadConstants.FloatIndexes.FeedFwdInitial.ToInt()).Value;
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.PidConfigStatus.ToInt(), StatusConstants.ProgressStatus.InActive.ToInt());
            });
        }
    }
}
