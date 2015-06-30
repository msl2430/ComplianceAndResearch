using System.Windows;
using MahApps.Metro.Controls;
using Robot.Application.ViewModels;
using Robot.Core.Models;

namespace Robot.Application.Views.Learning
{
    /// <summary>
    /// Interaction logic for LearningView.xaml
    /// </summary>
    public partial class LearningView : BaseUserControl
    {
        public LearningView()
        {
            InitializeComponent();
            

            //LearningViewModel = new LearningViewModel()
            //{
            //    Manufacturer = new IdNamePair(ApplicationSessionFactory.SelectedCar.ManufacturerId, ApplicationSessionFactory.SelectedCar.ManufacturerName)
            //};
            //DataContext = LearningViewModel;
        }


        private void LearningView_OnLoaded(object sender, RoutedEventArgs e)
        {
            //var model = (LearningViewModel)DataContext;
            //model.Model = new IdNamePair(3882, "TESTMODEL");
            //model.ModelYear = 1888;
            //DataContext = model;
        }
    }
}
