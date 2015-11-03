﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Robot.Application.Extensions;
using Robot.Application.ViewModels;
using Robot.Core.Constants;

namespace Robot.Application.Views.CarSelection
{
    /// <summary>
    /// Interaction logic for NewManufacturerFlyout.xaml
    /// </summary>
    public partial class NewManufacturerFlyout : BaseUserControl
    {
        public NewManufacturerFlyoutViewModel NewManufacturerFlyoutViewModel { get; set; }

        public NewManufacturerFlyout()
        {
            InitializeComponent();
            NewManufacturerFlyoutViewModel = new NewManufacturerFlyoutViewModel
            {
                IsUniqueName = false,
                Countries = CountryService.GetCountries().OrderBy(c => c.Name).ToList()
            };
            DataContext = NewManufacturerFlyoutViewModel;            
        }

        private void CheckUniqueManufacturerName(object sender, RoutedEventArgs e)
        {
            NewManufacturerFlyoutViewModel.IsUniqueName = NewManufactureNameText.Text.Length >= 3 && ManufacturerService.CheckUniqueManufacturerName(NewManufactureNameText.Text);
            ManufacturerSuccessCheck.Visibility = NewManufacturerFlyoutViewModel.IsUniqueName ? Visibility.Visible : Visibility.Hidden;
            ManufacturerFailCheck.Visibility = NewManufacturerFlyoutViewModel.IsUniqueName ? Visibility.Hidden : Visibility.Visible;
            NewManufactureNameText.BorderBrush = !NewManufacturerFlyoutViewModel.IsUniqueName ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Color.FromRgb(255,204,204));
        }

        private void SubmitManufacturer_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewManufactureNameText.Text.Length < 3 || !NewManufacturerFlyoutViewModel.IsUniqueName) return;
            ManufacturerService.AddManufacturer(
                NewManufactureNameText.Text.Trim(),
                NewManufacturerFlyoutViewModel.Countries[CountrySelectionComboBox.SelectedIndex].CountryId,
                "");
            NewManufacturerFlyoutViewModel.ApplicationSessionFactory.LogEvent("Added new manufacturer '" + NewManufactureNameText.Text + "' successfully.", true);
            ((Flyout)Parent).Tag = ControlConstants.ChangeTracking.Dirty;
            ((Flyout)Parent).IsOpen = false;
            //TODO: Move to next phase (Learn)
        }

        //private void NumericCharValidate(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !NumericTextbox.ValidateNumber(e.Text);
        //}

        //private void NumericBlurValidate(object sender, RoutedEventArgs e)
        //{
        //    var txtBox = (TextBox) sender;
        //    if (!NumericTextbox.ValidateNumber(txtBox.Text))
        //    {
        //        SubmitManufacturer.IsEnabled = false;
        //        txtBox.BorderBrush = new SolidColorBrush(Colors.DarkRed);
        //    }
        //    else
        //    {
        //        SubmitManufacturer.IsEnabled = true;
        //        txtBox.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
        //    }
        //}
    }
}
