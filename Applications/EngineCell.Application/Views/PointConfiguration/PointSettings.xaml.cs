using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using EngineCell.Application.ViewModels.PointConfiguration;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.PointConfiguration
{
    /// <summary>
    /// Interaction logic for PointSettings.xaml
    /// </summary>
    public partial class PointSettings : Window
    {
        public PointDataModel Point { get; set; }

        private ICellPointRepository _cellPointRepository { get; set; }
        private ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

        public PointSettings(PointDataModel point)
        {
            InitializeComponent();
            Point = point;
            DataContext = Point;        
            SetupForm();
        }

        private void SetupForm()
        {
            Title = Point.CustomName + " Settings";
            IsRecord.IsChecked = Point.IsRecord;
            IsAverage.IsChecked = Point.IsAverage;
            AverageSeconds.Text = Point.AverageSeconds != null ? Point.AverageSeconds.ToString() : "";
            IncludeInStripChart.IsChecked = Point.IncludeInStripChart;
            StripChartScale.Text = Point.StripChartScale != null ? Point.StripChartScale.ToString() : "";

        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            Point.IsRecord = Convert.ToBoolean(IsRecord.IsChecked);
            Point.IsAverage = Convert.ToBoolean(IsAverage.IsChecked);
            Point.AverageSeconds = !string.IsNullOrEmpty(AverageSeconds.Text) ? Convert.ToInt32(AverageSeconds.Text) : Point.IsAverage ? (int?)10 : null;
            Point.IncludeInStripChart = Convert.ToBoolean(IncludeInStripChart.IsChecked);
            Point.StripChartScale = !string.IsNullOrEmpty(StripChartScale.Text) ? Convert.ToDecimal(StripChartScale.Text) : Point.IncludeInStripChart ? (decimal?)1 : null;
            CellPointRepository.UpdateCellPoint(Point.ToCellPointModel());
            this.Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
