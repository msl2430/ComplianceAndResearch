using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using EngineCell.Application.Factories;
using EngineCell.Application.Views;
using EngineCell.Core.Constants;
using EngineCell.Core.Extensions;

namespace EngineCell.Application.ViewModels.Welcome
{
    public class WelcomeScreenViewModel : BaseViewModel
    {
        private string _selectedCellName { get; set; }
        public string SelectedCellName
        {
            get { return _selectedCellName;}
            set
            {
                _selectedCellName = value;
                OnPropertyChanged("SelectedCellName");
                OnPropertyChanged("IsCellSelected");
            }
        }

        private ObservableCollection<string> _activePoints { get; set; }
        public ObservableCollection<string> ActivePoints
        {
            get { return _activePoints; }
            set { _activePoints = value; OnPropertyChanged("ActivePoints"); }
        }

        public bool IsCellSelected { get { return !string.IsNullOrEmpty(SelectedCellName); } }

        public string Version { get { return $"{Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}.{Assembly.GetExecutingAssembly().GetName().Version.Build}"; } }

        public MainWindow MainWindow { get; set; }

        public WelcomeScreenViewModel(IApplicationSessionFactory appSession, MainWindow mainWindow)
        {
            ApplicationSessionFactory = appSession;
            MainWindow = mainWindow;
            ActivePoints = new ObservableCollection<string>();
        }

        public void UpdateViewModel()
        {
            SelectedCellName = ApplicationSessionFactory.CurrentCell.Name;
            ActivePoints.Clear();
            var tempPoints = new ObservableCollection<string>();
            foreach (var point in ApplicationSessionFactory.CellPoints.GroupBy(p => p.PointGroupId))
            {
                switch (point.Key)
                {
                    case PointGroup.ThermoCouple:
                        tempPoints.Add(string.Format("Thermo Couple ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.Volt:
                        tempPoints.Add(string.Format("Voltage ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.MilliAmp:
                        tempPoints.Add(string.Format("Milli Amp ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.Hart:
                        tempPoints.Add(string.Format("HART ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.AnalogOutput:
                        tempPoints.Add(string.Format("Analog Outputs ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.DigitalOutput:
                        tempPoints.Add(string.Format("Digital Outputs ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;                    
                    case PointGroup.Frequency:
                        tempPoints.Add(string.Format("Frequency ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    case PointGroup.DigitalInput:
                        tempPoints.Add(string.Format("Digital Inputs ({0})", StringExtensions.Pluralize("1 Point", "Points", "0 Points", point.Count())));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            ActivePoints = tempPoints;
        }
    }
}
