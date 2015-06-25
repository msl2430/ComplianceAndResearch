using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MahApps.Metro.Controls;
using Robot.Application.Factories;
using Robot.Application.Services;
using Robot.Application.Services.DataServices;
using Robot.Application.Services.WorkerServices;
using Robot.Application.Session;
using Robot.Application.ViewModels;
using Robot.Application.Views.StartPage;
using Robot.Core.Constants;
using Robot.Core.Extensions;
using Robot.Core.Models;

namespace Robot.Application.Views
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow
    {
        public override void BeginInit()
        {
            base.BeginInit();
            InitializeComponents();           
        }

        private void InitializeComponents()
        {
            var model = GetControlWindowViewModel();
            ApplicationSessionFactory = new ApplicationSessionFactory
            {
                ControlWindowViewModel = model,
            };

            DataContext = ApplicationSessionFactory.ControlWindowViewModel;
            OptoConnectionWorker = new OptoConnectionWorkerService(ApplicationSessionFactory, Dispatcher);
            //ToggleOptoConnection();
        }

        private void ToggleOptoConnection()
        {
            switch (ApplicationSessionFactory.OptoConnectionStatus)
            {
                case StatusConstants.ConnectionStatus.Disconnected:
                    Task.Run(() =>
                    {
                        ApplicationSessionFactory.LogEvent("Connecting to Opto 22 @" + ConfigurationManager.AppSettings["OptoIpAddress"], true);
                        OptoConnectionWorker.DoWork();
                    }).ConfigureAwait(false);
                    break;
                case StatusConstants.ConnectionStatus.Connected:
                    ApplicationSessionFactory.LogEvent("Disconnecting from Opto 22", true);
                    OptoConnectionWorker.CancelWork();
                    break;
            }
        }

        private ControlWindowViewModel GetControlWindowViewModel()
        {
            var manufactures = ManufacturerService.GetAllManufactures().OrderBy(m => m.Name).ToList();
            var models = ModelService.GetCarModelsByManufactureId(manufactures[0].ManufacturerId);
            return new ControlWindowViewModel
            {
                StatusLabel = "Disconnected",
                Manufacturers = manufactures,
                CarModels = models
            };
        }

        #region Events
        private void OptoConnectionToggle_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            ToggleOptoConnection();
        }

        #region AddManufacturer
        private void AddManufacturerButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddManufacturerFlyout.FindChild<NewManufacturerFlyout>("NewManufacturerFlyout").ApplicationSessionFactory = ApplicationSessionFactory;
            AddManufacturerFlyout.Tag = ControlConstants.ChangeTracking.Pristine;
            AddManufacturerFlyout.IsOpen = true;
            AddManufacturerFlyout.ClosingFinished += AddManufacturerFlyoutOnClosingFinished;
        }

        private void AddManufacturerFlyoutOnClosingFinished(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Convert.ToInt32(AddManufacturerFlyout.Tag) != ControlConstants.ChangeTracking.Dirty.ToInt()) return;

            ApplicationSessionFactory.ControlWindowViewModel.Manufacturers = ManufacturerService.GetAllManufactures().OrderBy(m => m.Name).ToList();
            ManufacturerComboBox.SelectedIndex =
                ApplicationSessionFactory.ControlWindowViewModel.Manufacturers.IndexOf(
                    ApplicationSessionFactory.ControlWindowViewModel.Manufacturers.OrderByDescending(m => m.ManufacturerId).Take(1).Single());
        }

        private void ManufacturerComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ManufacturerComboBox.SelectedIndex >= 0)
                ApplicationSessionFactory.ControlWindowViewModel.CarModels =
                    ModelService.GetCarModelsByManufactureId(ApplicationSessionFactory.ControlWindowViewModel.Manufacturers[ManufacturerComboBox.SelectedIndex].ManufacturerId)
                        .OrderBy(m => m.Name)
                        .ToList();
            if(ModelComboBox != null) 
                ModelComboBox.SelectedIndex = 0;
        }
        #endregion

        #region AddModel
        private void AddModelButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddModelFlyout.Header = "Add Model for " + ApplicationSessionFactory.ControlWindowViewModel.Manufacturers[ManufacturerComboBox.SelectedIndex].Name;
            var flyout = AddModelFlyout.FindChild<NewModelFlyout>("NewModelFlyout");
            flyout.ApplicationSessionFactory = ApplicationSessionFactory;
            flyout.Manufacturer = new IdNamePair(ApplicationSessionFactory.ControlWindowViewModel.Manufacturers[ManufacturerComboBox.SelectedIndex].ManufacturerId,
                ApplicationSessionFactory.ControlWindowViewModel.Manufacturers[ManufacturerComboBox.SelectedIndex].Name);
            AddModelFlyout.Tag = ControlConstants.ChangeTracking.Pristine;
            
            AddModelFlyout.IsOpen = true;
            AddModelFlyout.ClosingFinished += AddModelFlyoutOnClosingFinished;
        }

        private void AddModelFlyoutOnClosingFinished(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Convert.ToInt32(AddModelFlyout.Tag) != ControlConstants.ChangeTracking.Dirty.ToInt()) return;

            ApplicationSessionFactory.ControlWindowViewModel.CarModels =
                ModelService.GetCarModelsByManufactureId(ApplicationSessionFactory.ControlWindowViewModel.Manufacturers[ManufacturerComboBox.SelectedIndex].ManufacturerId)
                    .OrderBy(m => m.Name)
                    .ToList();

            ModelComboBox.SelectedIndex =
                ApplicationSessionFactory.ControlWindowViewModel.CarModels.IndexOf(
                    ApplicationSessionFactory.ControlWindowViewModel.CarModels.OrderByDescending(m => m.ModelId).Take(1).Single());
        }
        #endregion
        #endregion
    }
}
