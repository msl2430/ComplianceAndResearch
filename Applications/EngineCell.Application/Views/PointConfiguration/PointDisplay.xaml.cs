using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.ViewModels.PointConfiguration;

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

        private void UpdateTextPoint(object sender, RoutedEventArgs e)
        {
            if ((PointDataModel) DataContext != null && Point != null && IsDirty)
                CellPointRepository.UpdateCellPoint(Point.ToCellPointModel());
        }

        private void ConfigSettings(object sender, RoutedEventArgs e)
        {
            var configModal = new PointSettings(Point);
            configModal.ShowDialog();
        }
    }
}
