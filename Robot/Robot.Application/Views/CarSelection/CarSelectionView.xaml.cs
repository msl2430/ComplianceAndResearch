using MahApps.Metro.Controls;
using Robot.Application.ViewModels;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Robot.Application.Extensions;

namespace Robot.Application.Views.CarSelection
{
    /// <summary>
    /// Interaction logic for CarSelectionView.xaml
    /// </summary>
    public partial class CarSelectionView : BaseUserControl
    {
        private CarSelectionViewModel ViewModel { get; set; }
        private Flyout AddManufacturerFlyout { get; set; }

        public CarSelectionView()
        {
            InitializeComponent();
        }

        private void CarSelectionView_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetCarSelectionViewModel();            
        }

        private void GetCarSelectionViewModel()
        {
            var manufactures = ManufacturerService.GetAllManufactures().OrderBy(m => m.Name).ToList();
            var models = ModelService.GetCarModelsByManufactureId(manufactures[0].ManufacturerId);
            var yearList = new List<int>();
            for (var i = DateTime.Now.AddYears(-10).Year; i <= DateTime.Now.AddYears(5).Year; i++)
            {
                yearList.Add(i);
            }
            ViewModel = (CarSelectionViewModel) DataContext;
            ViewModel.Manufacturers = manufactures;
            ViewModel.CarModels = models;
            ViewModel.AvailableYears = yearList;
            if(!ExistingModelComboBox.Items.IsEmpty)
                ExistingModelComboBox.SelectedIndex = 0;
            //TODO: FOR TESTING
            NewModelTextBox.Text = "Test";
            //TODO: FOR TESTING
        }

        #region Events
        private void AddManufacturerButton_OnClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            AddManufacturerFlyout = (Flyout)parentWindow.FindName("AddManufacturerFlyout");
            AddManufacturerFlyout.FindChild<NewManufacturerFlyout>("NewManufacturerFlyout").NewManufacturerFlyoutViewModel.ApplicationSessionFactory =
                ViewModel.ApplicationSessionFactory;
            AddManufacturerFlyout.Tag = ControlConstants.ChangeTracking.Pristine;
            AddManufacturerFlyout.IsOpen = true;
            AddManufacturerFlyout.ClosingFinished += AddManufacturerFlyoutOnClosingFinished;
        }

        private void AddManufacturerFlyoutOnClosingFinished(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Convert.ToInt32(AddManufacturerFlyout.Tag) != ControlConstants.ChangeTracking.Dirty.ToInt()) return;

            ViewModel.Manufacturers = ManufacturerService.GetAllManufactures().OrderBy(m => m.Name).ToList();
            NewManufacturerComboBox.SelectedIndex =
                ViewModel.Manufacturers.IndexOf(
                    ViewModel.Manufacturers.OrderByDescending(m => m.ManufacturerId).Take(1).Single());
        }

        private void LearningPhaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewModelTextBox.Text))
            {
                NewModelTextBox.BorderThickness = new Thickness(1.01);
                NewModelTextBox.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                return;
            }

            decimal decimalTest;
            if (!decimal.TryParse(NewModelPulseMultplier.Text, out decimalTest))
            {
                NewModelPulseMultplier.BorderThickness = new Thickness(1.01);
                NewModelPulseMultplier.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                return;
            }
            if (!decimal.TryParse(NewModelPulsePerRev.Text, out decimalTest))
            {
                NewModelPulsePerRev.BorderThickness = new Thickness(1.01);
                NewModelPulsePerRev.BorderBrush = new SolidColorBrush(Colors.DarkRed);
                return;
            }

            var modelId = ModelService.AddModel(
                NewModelTextBox.Text.Trim(), 
                ViewModel.Manufacturers[NewManufacturerComboBox.SelectedIndex].ManufacturerId,
                ViewModel.AvailableYears[YearCombo.SelectedIndex], 
                Convert.ToDecimal(NewModelPulseMultplier.Text), 
                Convert.ToDecimal(NewModelPulsePerRev.Text));

            if (modelId <= 0)
            {
                MessageBox.Show("Error creating new car model!", "Error", MessageBoxButton.OK);
                return;
            }

            GoToLearningView(new LearningViewModel(ViewModel.ApplicationSessionFactory)
            {
                ManufacturerId = ViewModel.Manufacturers[NewManufacturerComboBox.SelectedIndex].ManufacturerId,
                ManufacturerName = ViewModel.Manufacturers[NewManufacturerComboBox.SelectedIndex].Name,
                ModelId = modelId,
                ModelName = NewModelTextBox.Text,
                ModelYear = ViewModel.AvailableYears[YearCombo.SelectedIndex]
            });
        }

        private void TestingPhaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ExistingModelComboBox.SelectedIndex >= 0)
                GoToTestingView(new TestingViewModel(ViewModel.ApplicationSessionFactory)
                {
                    ManufacturerId = ViewModel.Manufacturers[ExistingManufacturerComboBox.SelectedIndex].ManufacturerId,
                    ManufacturerName = ViewModel.Manufacturers[ExistingManufacturerComboBox.SelectedIndex].Name,
                    ModelId = ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].ModelId,
                    ModelName = ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].Name,
                    ModelYear = ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].Year,
                    ModelGearRatios = ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].GearRatios,
                    PulseMultiplier = Convert.ToDecimal(ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].PulseMultiplier),
                    PulsePerRev = Convert.ToDecimal(ViewModel.CarModels[ExistingModelComboBox.SelectedIndex].PulsePerRev),
                });
        }

        private void GoToLearningView(LearningViewModel viewModel)
        {
            var parentWindow = (ApplicationView)Window.GetWindow(this);
            ViewModel.ApplicationSessionFactory.LogEvent(string.Format("Learning phase selected for {0} {1} {2}.", viewModel.ModelYear, viewModel.ManufacturerName, viewModel.ModelName), true);
            parentWindow.ChangePageView(viewModel);
        }

        private void GoToTestingView(TestingViewModel viewModel)
        {
            var parentWindow = (ApplicationView)Window.GetWindow(this);
            ViewModel.ApplicationSessionFactory.LogEvent(string.Format("Testing phase selected for {0} {1} {2}.", viewModel.ModelYear, viewModel.ManufacturerName, viewModel.ModelName), true);
            parentWindow.ChangePageView(viewModel);
        }

        private void ExistingManufacturerComboBox_OnSelectionChangedManufacturerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExistingManufacturerComboBox.SelectedIndex >= 0)
                ViewModel.CarModels =
                    ModelService.GetCarModelsByManufactureId(ViewModel.Manufacturers[ExistingManufacturerComboBox.SelectedIndex].ManufacturerId)
                        .OrderBy(m => m.Name)
                        .ToList();
            if (ExistingModelComboBox != null)
                ExistingModelComboBox.SelectedIndex = 0;
        }

        private void NumericCharValidate(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void NumericBlurValidate(object sender, RoutedEventArgs e)
        {
            var txtBox = (TextBox)sender;
            if (!NumericTextbox.ValidateNumber(txtBox.Text))
            {
                LearningPhaseButton.IsEnabled = false;
                txtBox.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                LearningPhaseButton.IsEnabled = true;
                txtBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }
        #endregion      
    }
}
