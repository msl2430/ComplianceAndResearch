using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Opto22.Core.Constants;
using Robot.Application.ViewModels;
using Robot.Core.Extensions;

namespace Robot.Application.Views.Learning
{
    /// <summary>
    /// Interaction logic for LearningView.xaml
    /// </summary>
    public partial class LearningView : BaseUserControl
    {
        private LearningViewModel ViewModel { get; set; }

        public LearningView()
        {
            InitializeComponent();            
        }


        private void LearningView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(DataContext != null)
                ViewModel = (LearningViewModel)DataContext;
            
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.GoToLearningPhase.ToInt(), 1);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ManufacturerId.ToInt(), ViewModel.ManufacturerId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelId.ToInt(), ViewModel.ModelId);
            ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.ModelYear.ToInt(), ViewModel.ModelYear);

            Task.Run(() =>
            {
                Thread.Sleep(5000);
                ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.LearingPhaseTemplateLoadStatus.ToInt(),
                    ScratchPadConstants.LoadStatus.LoadFinished.ToInt());
            });
        }
    }
}
