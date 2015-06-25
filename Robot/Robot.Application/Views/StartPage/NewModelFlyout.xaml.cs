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
using MahApps.Metro.Controls;
using Robot.Core.Constants;
using Robot.Core.Models;

namespace Robot.Application.Views.StartPage
{
    /// <summary>
    /// Interaction logic for NewModelFylout.xaml
    /// </summary>
    public partial class NewModelFlyout : BaseUserControl
    {
        public IdNamePair Manufacturer { get; set; }
        private bool IsUniqueName { get; set; }

        public NewModelFlyout()
        {
            InitializeComponent();
        }

        private void CheckUniqueModelName(object sender, RoutedEventArgs e)
        {
            IsUniqueName = NewModelText.Text.Length >= 3 && ModelService.CheckUniqueModelNameByManufacturer(NewModelText.Text, Manufacturer.Id);
            ModelSuccessCheck.Visibility = IsUniqueName ? Visibility.Visible : Visibility.Hidden;
            ModelFailCheck.Visibility = IsUniqueName ? Visibility.Hidden : Visibility.Visible;
            NewModelText.BorderBrush = IsUniqueName ? new SolidColorBrush(Color.FromRgb(255, 204, 204)) : new SolidColorBrush(Colors.DarkRed);
        }

        private void SubmitModel_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewModelText.Text.Length < 3 && !IsUniqueName) return;

            ModelService.AddModel(NewModelText.Text, Manufacturer.Id);

            ApplicationSessionFactory.LogEvent("Added new model '" + NewModelText.Text + "' successfully.", true);
            ((Flyout)Parent).Tag = ControlConstants.ChangeTracking.Dirty;
            ((Flyout)Parent).IsOpen = false;
        }
    }
}
