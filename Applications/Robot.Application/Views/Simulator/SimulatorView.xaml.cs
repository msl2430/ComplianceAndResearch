using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Opto22.Core.Constants;
using Robot.Application.Extensions;
using Robot.Application.ViewModels;
using Robot.Core.Extensions;
using Robot.Models.Models;

namespace Robot.Application.Views.Simulator
{
    /// <summary>
    /// Interaction logic for SimulatorView.xaml
    /// </summary>
    public partial class SimulatorView : BaseUserControl
    {
        private SimulatorViewModel ViewModel { get; set; }
        private Brush DefaultButtonColor { get; set; }

        public SimulatorView()
        {
            InitializeComponent();
        }

        private void SimulatorView_OnLoaded(object sender, RoutedEventArgs e)
        {
            DefaultButtonColor = SetRequiredSpeed.Background;

            var model = ModelService.GetCarModelsByManufactureId(63).FirstOrDefault(m => m.ModelId == 1);
            
            //if (DataContext != null)
            //    ViewModel = (SimulatorViewModel) DataContext;
            ViewModel = new SimulatorViewModel(((SimulatorViewModel)DataContext).ApplicationSessionFactory)
                {
                    ManufacturerId = 63,
                    ManufacturerName = "Abarth",
                    ModelId = model.ModelId,
                    ModelName = model.Name,
                    ModelYear = model.Year,
                    ModelGearRatios = model.GearRatios,
                    PulseMultiplier = (decimal)model.PulseMultiplier,
                    PulsePerRev = (decimal)model.PulsePerRev,
                    RollDiameter = Properties.Settings.Default.RollDiameter,
                    RequiredSpeed = 0,
                    ThrottleSetPoint = 0,
                    Acceleration = 0,
                    MaxSpeed = 0,
                };

            ViewModel.ConfigurationHeader = ViewModel.CarDisplayName + " Configuration";
            gearRatioList.ItemsSource = ViewModel.ModelGearRatios;

            DataContext = ViewModel;
        }

        #region Events
        private void SetRequiredSpeed_OnClick(object sender, RoutedEventArgs e)
        {
            SetOverride(ScratchPadConstants.SimulatorOverrides.RequiredSpeed);
        }

        private void SetThrottleSetPoint_OnClick(object sender, RoutedEventArgs e)
        {
            SetOverride(ScratchPadConstants.SimulatorOverrides.ThrottleSetPoint);
        }

        private void SetAcceleration_OnClick(object sender, RoutedEventArgs e)
        {
            SetOverride(ScratchPadConstants.SimulatorOverrides.Acceleration);
        }

        private void RequiredSpeed_OnPreview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void RequiredSpeed_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NumericTextbox.ValidateNumber(RequiredSpeed.Text))
            {
                SetRequiredSpeed.IsEnabled = false;
                RequiredSpeed.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                SetRequiredSpeed.IsEnabled = true;
                RequiredSpeed.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }

        private void ThrottleSetPoint_OnPreview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void ThrottleSetPoint_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NumericTextbox.ValidateNumber(RequiredSpeed.Text))
            {
                SetThrottleSetPoint.IsEnabled = false;
                ThrottleSetPoint.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                SetThrottleSetPoint.IsEnabled = true;
                ThrottleSetPoint.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }

        private void MaxSpeed_OnPreview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void MaxSpeed_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NumericTextbox.ValidateNumber(RequiredSpeed.Text))
            {
                SetAcceleration.IsEnabled = false;
                MaxSpeed.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                SetAcceleration.IsEnabled = true;
                MaxSpeed.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }

        private void Acceleration_OnPreview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        }

        private void Acceleration_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!NumericTextbox.ValidateNumber(RequiredSpeed.Text))
            {
                SetAcceleration.IsEnabled = false;
                Acceleration.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                SetAcceleration.IsEnabled = true;
                Acceleration.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
            }
        }

        private void SetOverride(ScratchPadConstants.SimulatorOverrides type)
        {
            SetRequiredSpeed.Background = DefaultButtonColor;
            SetRequiredSpeed.Content = "Set";
            SetThrottleSetPoint.Background = DefaultButtonColor;
            SetThrottleSetPoint.Content = "Set";
            SetAcceleration.Background = DefaultButtonColor;
            SetAcceleration.Content = "Set";
            if (ViewModel.OverrideEnabled == type)
            {
                SetSimulatorOverride(ScratchPadConstants.SimulatorOverrides.Off);
                return;
            }
            SetSimulatorOverride(type);
            switch (type)
            {
                case ScratchPadConstants.SimulatorOverrides.RequiredSpeed:
                    SetRequiredSpeed.Background = Brushes.Green;
                    SetRequiredSpeed.Content = "Speed Set";
                    break;
                case ScratchPadConstants.SimulatorOverrides.ThrottleSetPoint:
                    SetThrottleSetPoint.Background = Brushes.Green;
                    SetThrottleSetPoint.Content = "Set Point Set";
                    break;
                case ScratchPadConstants.SimulatorOverrides.Acceleration:
                    SetAcceleration.Background = Brushes.Green;
                    SetAcceleration.Content = "Acceleration Set";
                    break;
            }
        }

        private void SetSimulatorOverride(ScratchPadConstants.SimulatorOverrides type)
        {
            ViewModel.OverrideEnabled = type;
           // ViewModel.ApplicationSessionFactory.ScratchPadFactory.SetScratchPadValue(ScratchPadConstants.IntegerIndexes.SimulatorOverrideType.ToInt(), type.ToInt());
        }

        private void gearRatioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #endregion        
    }
}
