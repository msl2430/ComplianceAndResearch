using System;
using System.Windows;
using EngineCell.Application.ViewModels.AlarmConfiguration;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.AlarmConfiguration
{
    public partial class AlarmConfig : Window
    {
        private string AlarmName { get; set; }
        private AlarmSetting AlarmSetting { get; set; }

        private ICellPointRepository _cellPointRepository { get; set; }
        private ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

        public AlarmConfig(AlarmSetting alarmSetting, string alarmName)
        {
            InitializeComponent();
            AlarmSetting = alarmSetting;
            AlarmName = alarmName;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Title = $"{AlarmName} Alarm Settings";
            LowShutdownWarningText.Text = AlarmSetting.LowShutdownWarning.ToString("0.0##");
            LowShutdownExecuteText.Text = AlarmSetting.LowShutdownExecute.ToString("0.0##");
            HighShutdownWarningText.Text = AlarmSetting.HighShutdownWarning.ToString("0.0##"); ;
            HighShutdownExecuteText.Text = AlarmSetting.HighShutdownExecute.ToString("0.0##");
            DeactivateButton.Visibility = AlarmSetting.IsActive ? Visibility.Visible : Visibility.Collapsed;
        }


        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            AlarmSetting.LowShutdownWarning = Convert.ToDecimal(LowShutdownWarningText.Text);
            AlarmSetting.LowShutdownExecute = Convert.ToDecimal(LowShutdownExecuteText.Text);
            AlarmSetting.HighShutdownWarning = Convert.ToDecimal(HighShutdownWarningText.Text);
            AlarmSetting.HighShutdownExecute = Convert.ToDecimal(HighShutdownExecuteText.Text);
            AlarmSetting.IsActive = true;
            CellPointRepository.UpdateCellPointAlarm(AlarmSetting.ToCellPointAlarmModel());
            this.Close();
        }

        private void DeactivateButton_OnClick(object sender, RoutedEventArgs e)
        {
            AlarmSetting.LowShutdownWarning = 0m;
            AlarmSetting.LowShutdownExecute = 0m;
            AlarmSetting.HighShutdownWarning = 0m;
            AlarmSetting.HighShutdownExecute = 0m;
            AlarmSetting.IsActive = false;
            CellPointRepository.DeleteCellPointAlarm(AlarmSetting.CellPointAlarmId);
            this.Close();
        }
    }
}

