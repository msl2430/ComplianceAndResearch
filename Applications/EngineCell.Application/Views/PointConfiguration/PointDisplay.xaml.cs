using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Application.Views.AlarmConfiguration;

namespace EngineCell.Application.Views.PointConfiguration
{
    public partial class PointDisplay : BaseUserControl
    {
        public PointDataModel Point { get; set; } 
        private bool IsDirty { get; set; }

        public PointDisplay()
        {
            InitializeComponent();
            IsDirty = false;
        }

        private void PointDisplay_OnLoaded(object sender, RoutedEventArgs e)
        {
            Point = (PointDataModel) DataContext;
        }

        private void MarkAsDirty(object sender, TextChangedEventArgs e)
        {
            IsDirty = true;
        }

        private void UpdateCheckbox(object sender, RoutedEventArgs e)
        {
            if ((PointDataModel)DataContext != null && Point != null)
                CellPointRepository.UpdateCellPoint(Point.ToCellPointModel());
        }

        private void UpdateTextPoint(object sender, RoutedEventArgs e)
        {
            if ((PointDataModel) DataContext != null && Point != null && IsDirty)
                CellPointRepository.UpdateCellPoint(Point.ToCellPointModel());
        }

        private void AlarmSettings(object sender, RoutedEventArgs e)
        {
            var alarmModal = new AlarmConfig(Point.Alarm, Point.CustomName);
            alarmModal.ShowDialog();
        }
    }
}
