using System.Windows;
using Robot.Application.ViewModels;

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
        }
    }
}
