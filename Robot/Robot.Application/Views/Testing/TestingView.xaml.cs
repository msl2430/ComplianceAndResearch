using System.Windows;
using Robot.Application.ViewModels;

namespace Robot.Application.Views.Testing
{
    /// <summary>
    /// Interaction logic for TestingView.xaml
    /// </summary>
    public partial class TestingView : BaseUserControl
    {
        private TestingViewModel ViewModel { get; set; }

        public TestingView()
        {
            InitializeComponent();
        }

        private void TestingView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ViewModel = (TestingViewModel) DataContext;
        }
    }
}
