﻿using System.Windows;
using System.Windows.Controls;
using EngineCell.Application.Models;
using EngineCell.Models.Repositories;

namespace EngineCell.Application.Views.PointConfiguration
{
    public partial class PointDisplay : UserControl
    {
        private ICellPointRepository _cellPointRepository { get; set; }
        private ICellPointRepository CellPointRepository => _cellPointRepository ?? (_cellPointRepository = new CellPointRepository());

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

    }
}