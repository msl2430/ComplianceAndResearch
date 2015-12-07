using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EngineCell.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:EngineCell.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:EngineCell.Controls;assembly=EngineCell.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>

    [TemplatePart(Name = "PART_MasterGrid", Type = typeof (Grid))]
    [TemplatePart(Name = "PART_RightCntl", Type = typeof (StackPanel))]
    [TemplatePart(Name = "PART_LeftCntl", Type = typeof (StackPanel))]
    [TemplatePart(Name = "PART_BottomCntl", Type = typeof (StackPanel))]
    public class LayeredGrid : ContentControl
    {
        #region fields

        private Grid PART_MasterGrid;
        private StackPanel PART_RightCntl;
        private StackPanel PART_LeftCntl;
        private StackPanel PART_BottomCntl;
        private DockPanel PART_ParentPanel;
        private readonly ObservableCollection<Layer> _aValues = new ObservableCollection<Layer>();
        private readonly List<GridnFloatingBtnCombo> _columnLayers = new List<GridnFloatingBtnCombo>();
        private readonly List<GridnFloatingBtnCombo> _rowLayers = new List<GridnFloatingBtnCombo>();
        private const string ColumnStr = "column";
        private const string RowStr = "row";
        private const string LayerStr = "Layer";
        private const string PinStr = "btn";

        #endregion

        #region layer grid, button btn, columns and rows definition holder class

        private class GridnFloatingBtnCombo
        {
            public readonly Grid Grid;
            public readonly Button Btn;
            public readonly List<ColumnDefinition> ColumnDefinitions;
            public readonly List<Layer.LayerColumnLocation> ColumnLocations;
            public readonly List<RowDefinition> RowDefinitions;
            public int MainContentLocation { get; private set; }

            public GridnFloatingBtnCombo(Grid grid, Button btn)
            {
                Grid = grid;
                Btn = btn;
                ColumnDefinitions = new List<ColumnDefinition>();
                RowDefinitions = new List<RowDefinition>();
                ColumnLocations = new List<Layer.LayerColumnLocation>();
                MainContentLocation = 1;
            }

            public void MainContentPositionIncrement()
            {
                MainContentLocation++;
            }

            public void MainContentPositionDecrement()
            {
                if (MainContentLocation > 1)
                    MainContentLocation--;
            }

        }

        #endregion

        #region properties and DPs

        private static readonly DependencyPropertyKey LayersPropertyKey = DependencyProperty.RegisterReadOnly("Layers",
            typeof (
                ObservableCollection
                    <Layer>),
            typeof (LayeredGrid),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LayersProperty = LayersPropertyKey.DependencyProperty;

        public ObservableCollection<Layer> Layers
        {
            get { return (ObservableCollection<Layer>) GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        #endregion

        #region constructor

        static LayeredGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (LayeredGrid), new FrameworkPropertyMetadata(typeof (LayeredGrid)));

        }

        /// <summary>
        /// Initializes the Layers collection
        /// </summary>
        public LayeredGrid()
        {
            SetValue(LayersPropertyKey, _aValues);
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //get the part controls 
            PART_MasterGrid = GetTemplateChild("PART_MasterGrid") as Grid;
            PART_RightCntl = GetTemplateChild("PART_RightCntl") as StackPanel;
            PART_LeftCntl = GetTemplateChild("PART_LeftCntl") as StackPanel;
            PART_BottomCntl = GetTemplateChild("PART_BottomCntl") as StackPanel;
            PART_ParentPanel = GetTemplateChild("PART_ParentPanel") as DockPanel;
            //verify master grid exist
            if (PART_MasterGrid == null)
                return;
            //setup parent grid
            var parentGrid = new Grid();
            SetUpParentGrid(parentGrid);
            //set up layers
            var layer0 = Layers.FirstOrDefault(x => x.Level == 0);
            if (layer0 == null)
                return;

            var columnLayers =
                Layers.Select(x => x).Where(x => x.Level > 0 && x.Orientation == Layer.LayerOrientation.Column).OrderBy(
                    x => x.Level);
            var rowLayers =
                Layers.Select(x => x).Where(x => x.Level > 0 && x.Orientation == Layer.LayerOrientation.Row).OrderBy(x => x.Level);
            var item = SetupLayer0(layer0,
                columnLayers,
                rowLayers.Count());
            parentGrid.Children.Add(item);
            Grid.SetRow(item, 0);
            //setup the column grid layers
            if (columnLayers.Any())
            {
                foreach (var layer in columnLayers)
                {
                    SetupColumnLayers(parentGrid, layer, columnLayers.Count());
                }
            }
            //setup the row grid layers
            if (rowLayers.Any())
            {
                foreach (var layer in rowLayers)
                {
                    SetupRowLayers(parentGrid, layer, rowLayers.Count());
                }
            }

            //add parent grid to master grid
            PART_MasterGrid.Children.Add(parentGrid);
            Grid.SetRow(parentGrid, 0);
        }

        private static void SetUpParentGrid(Grid parent)
        {
            var row1 = new RowDefinition {Height = new GridLength(1, GridUnitType.Star)};
            var row2 = new RowDefinition {Height = GridLength.Auto};
            parent.RowDefinitions.Add(row1);
            parent.RowDefinitions.Add(row2);
        }

        private Grid SetupLayer0(Layer layer0, IEnumerable<Layer> columnLayers, int numberofRows)
        {
            var grid = new Grid {Name = ColumnStr + LayerStr + layer0.Level};
            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
            grid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
            if (layer0.Content != null)
                grid.Children.Add(layer0.Content);
            if (layer0.Content != null)
                Grid.SetColumn(layer0.Content, 1);
            grid.MouseEnter += (o, e) =>
            {
                for (var i = 1; i < _columnLayers.Count; i++)
                {
                    if (_columnLayers[i].Btn.Visibility == Visibility.Visible)
                    {
                        _columnLayers[i].Grid.Visibility = Visibility.Collapsed;
                    }
                }

                for (var i = 1; i < _rowLayers.Count; i++)
                {
                    if (_rowLayers[i].Btn.Visibility == Visibility.Visible)
                    {
                        _rowLayers[i].Grid.Visibility = Visibility.Collapsed;
                    }
                }
            };
            //layer zero does not need a pin
            var gnb = new GridnFloatingBtnCombo(grid, null);
            if (columnLayers.Any())
            {
                var list = columnLayers.ToList();
                for (int i = 0; i < columnLayers.Count(); i++)
                {
                    gnb.ColumnDefinitions.Add(new ColumnDefinition {SharedSizeGroup = ColumnStr + (i + 1) + list[i].ColumnLocation, Width = GridLength.Auto});
                    gnb.ColumnLocations.Add(list[i].ColumnLocation);
                }
            }
            if (numberofRows > 0)
            {
                for (int i = 0; i < numberofRows; i++)
                {
                    gnb.RowDefinitions.Add(new RowDefinition {SharedSizeGroup = RowStr + (i + 1), Height = GridLength.Auto});
                }
            }
            _columnLayers.Add(gnb);
            _rowLayers.Add(gnb);
            return grid;
        }

        private void SetupColumnLayers(Grid parentGrid, Layer layer, int columnLayerCnt)
        {
            var grid = new Grid
            {
                Name = ColumnStr + LayerStr + layer.Level,
                Visibility = Visibility.Collapsed
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                SharedSizeGroup = layer.ColumnLocation == Layer.LayerColumnLocation.Left
                    ? ColumnStr + layer.Level + layer.ColumnLocation
                    : null,
                Width = GridLength.Auto
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                SharedSizeGroup = layer.ColumnLocation == Layer.LayerColumnLocation.Right
                    ? ColumnStr + layer.Level + layer.ColumnLocation
                    : null,
                Width = GridLength.Auto
            });
            var internalGrid = new Grid();
            internalGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            internalGrid.RowDefinitions.Add(new RowDefinition());
            internalGrid.Background = (RadialGradientBrush) PART_MasterGrid.FindResource("myColorfulLabelBrush");
            grid.Children.Add(internalGrid);
            Grid.SetColumn(internalGrid,
                layer.ColumnLocation == Layer.LayerColumnLocation.Left ? 0 : 2
                );
            var dockpanel = new DockPanel();
            internalGrid.Children.Add(dockpanel);
            Grid.SetRow(dockpanel, 0);
            var btn = new Button
            {
                Name = ColumnStr + PinStr + layer.Level,
                Width = 28.0,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Style = (Style) PART_MasterGrid.FindResource("buttonStyle"),
                Content = new Path
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Gold,
                    StrokeThickness = 1,
                    Stretch = Stretch.Fill,
                    Width = 9.0,
                    Height = 15,
                    Data = PinPathgeometry()
                }

            };
            dockpanel.Children.Add(btn);
            DockPanel.SetDock(btn, Dock.Right);
            btn.Click += (o, e) =>
            {
                var level = layer.Level;
                var item = _columnLayers[level].Btn;
                if (item.Visibility == Visibility.Collapsed)
                    ColumnUndockPane(level, o as Button);
                else
                    ColumnDockPane(level, o as Button);
            };
            var textblock = new TextBlock
            {
                Padding = new Thickness(8),
                TextTrimming = TextTrimming.CharacterEllipsis,
                Foreground = Brushes.Gold,
                Text = layer.Name
            };
            dockpanel.Children.Add(textblock);
            DockPanel.SetDock(textblock, Dock.Left);
            if (layer.Content != null)
            {
                internalGrid.Children.Add(layer.Content);
                Grid.SetRow(layer.Content, 1);
            }
            var gridSplitter = new GridSplitter
            {
                Width = 2,
                Background = Brushes.CadetBlue,
                HorizontalAlignment = layer.ColumnLocation == Layer.LayerColumnLocation.Right
                    ? HorizontalAlignment.Left
                    : HorizontalAlignment.Right
            };
            grid.Children.Add(gridSplitter);
            Grid.SetColumn(gridSplitter,
                layer.ColumnLocation == Layer.LayerColumnLocation.Right ? 2 : 0
                );
            grid.MouseEnter += (o, e) =>
            {
                var level = layer.Level;
                for (var i = (level + 1); i < _columnLayers.Count; i++)
                {
                    if (_columnLayers[i].Btn.Visibility == Visibility.Visible)
                        _columnLayers[i].Grid.Visibility = Visibility.Collapsed;
                }
            };
            parentGrid.Children.Add(grid);
            Grid.SetRow(grid, 0);
            var gnb = new GridnFloatingBtnCombo(grid,
                AddToColumnStackPanel(layer)
                );
            if (columnLayerCnt > 0)
            {
                for (var i = layer.Level; i < columnLayerCnt; i++)
                {
                    gnb.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        SharedSizeGroup = ColumnStr + (i + 1) + layer.ColumnLocation,
                        Width = GridLength.Auto
                    });
                    gnb.ColumnLocations.Add(layer.ColumnLocation);
                }
            }
            _columnLayers.Add(gnb);

        }

        private void SetupRowLayers(Grid parentGrid, Layer layer, int numberofRows)
        {
            var grid = new Grid
            {
                Name = RowStr + LayerStr + layer.Level,
                Visibility = Visibility.Collapsed
            };

            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });
            grid.RowDefinitions.Add(new RowDefinition
            {
                SharedSizeGroup = RowStr + layer.Level,
                Height = GridLength.Auto
            });
            //set up dock panel
            var dockpanel = new DockPanel
            {
                Margin = new Thickness(0, 4, 0, 0),
                Background = (RadialGradientBrush) PART_MasterGrid.FindResource("myColorfulLabelBrush"),
                LastChildFill = true
            };
            grid.Children.Add(dockpanel);
            Grid.SetRow(dockpanel, 1);
            var gridsplitter = new GridSplitter
            {
                Height = 4,
                Background = Brushes.CadetBlue,
                ResizeDirection = GridResizeDirection.Rows,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };
            grid.Children.Add(gridsplitter);
            Grid.SetRow(gridsplitter, 1);
            //set up stackpanel
            var stackpanel = new StackPanel
            {
                Height = 25.0,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            dockpanel.Children.Add(stackpanel);
            DockPanel.SetDock(stackpanel, Dock.Top);
            //set up btn
            var btn = new Button
            {
                Name = RowStr + PinStr + layer.Level,
                Width = 26.0,
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Style = (Style) PART_MasterGrid.FindResource("buttonStyle"),
                BorderThickness = new Thickness(0)
            };
            stackpanel.Children.Add(btn);
            var path = new Path
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Gold,
                StrokeThickness = 1,
                Stretch = Stretch.Fill,
                Width = 9.0,
                Height = 15
            };
            var pathgeometry = PinPathgeometry();
            path.Data = pathgeometry;
            btn.Content = path;
            btn.Click += (o, e) =>
            {
                int level = layer.Level;
                var pgrid = parentGrid;
                var item = _rowLayers[level].Btn;
                if (item.Visibility == Visibility.Collapsed)
                    RowUndockPane(level, o as Button, pgrid);
                else
                    RowDockPane(level, o as Button, pgrid);
            };

            if (layer.Content != null)
            {
                dockpanel.Children.Add(layer.Content);
                DockPanel.SetDock(layer.Content, Dock.Top);
            }


            grid.MouseEnter += (o, e) =>
            {
                var level = layer.Level;
                for (var i = 1; i < _rowLayers.Count; i++)
                {
                    if (i == level)
                        continue;
                    if (_rowLayers[i].Btn.Visibility == Visibility.Visible)
                    {
                        _rowLayers[i].Grid.Visibility = Visibility.Collapsed;
                    }
                }
            };
            PART_MasterGrid.Children.Add(grid);
            Grid.SetRow(grid, 0);
            var gnb = new GridnFloatingBtnCombo(grid, AddToRowStackPanel(layer));
            if (numberofRows > 0)
            {
                for (int i = layer.Level; i < numberofRows; i++)
                {
                    gnb.RowDefinitions.Add(new RowDefinition
                    {
                        SharedSizeGroup = RowStr + (i + 1),
                        Height = GridLength.Auto
                    });
                }
            }
            _rowLayers.Add(gnb);
        }

        private static PathGeometry PinPathgeometry()
        {
            return new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    new PathFigure
                    {
                        StartPoint = new Point(10, 0),

                        IsFilled = true,

                        Segments = new PathSegmentCollection
                        {
                            new LineSegment {Point = new Point(10, 0)},
                            new LineSegment {Point = new Point(30, 0)},
                            new LineSegment {Point = new Point(30, 5)},
                            new LineSegment {Point = new Point(10, 5)},
                            new LineSegment {Point = new Point(10, 0)}
                        }
                    },
                    new PathFigure
                    {
                        StartPoint = new Point(4.5, 5),
                        Segments = new PathSegmentCollection
                        {
                            new LineSegment {Point = new Point(40.5, 5)}
                        }
                    },
                    new PathFigure
                    {
                        StartPoint = new Point(22, 5),
                        Segments = new PathSegmentCollection
                        {
                            new LineSegment {Point = new Point(22, 10)}
                        }
                    }
                }
            };
        }

        private Button AddToRowStackPanel(Layer layer)
        {
            var btn = new Button
            {
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Height = 24,
                Padding = new Thickness(10, 0, 15, 0),
                FontWeight = FontWeights.Bold,
                Style = (Style) PART_MasterGrid.FindResource("buttonStyle"),
                Content = layer.Name
            };
            btn.Click += (o, e) =>
            {
                var level = layer.Level;
                var item = _rowLayers[level];
                item.Grid.Visibility = Visibility.Visible;
                Grid.SetZIndex(item.Grid, 1);
                for (int i = 1; i < _rowLayers.Count; i++)
                {
                    if (i == level)
                        continue;
                    var loc = _rowLayers[i];
                    Grid.SetZIndex(loc.Grid, 0);
                    if (loc.Btn.Visibility == Visibility.Visible)
                        loc.Grid.Visibility = Visibility.Collapsed;

                }

            };
            PART_BottomCntl.Children.Add(btn);
            return btn;
        }

        private Button AddToColumnStackPanel(Layer layer)
        {
            var btn = new Button
            {
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Height = 22,
                MinWidth = 65.0,
                Padding = new Thickness(10, 0, 15, 0),
                FontWeight = FontWeights.Bold,
                Style = (Style) PART_MasterGrid.FindResource("buttonStyle"),
                Content = layer.Name
            };
            btn.Click += (o, e) =>
            {
                var level = layer.Level;
                var item = _columnLayers[level];
                item.Grid.Visibility = Visibility.Visible;
                Grid.SetZIndex(item.Grid, 1);
                for (int i = 1; i < _columnLayers.Count; i++)
                {
                    if (i == level)
                        continue;
                    var loc = _columnLayers[i];
                    Grid.SetZIndex(loc.Grid, 0);
                    if (loc.Btn.Visibility == Visibility.Visible)
                        loc.Grid.Visibility = Visibility.Collapsed;

                }

            };
            if (layer.ColumnLocation == Layer.LayerColumnLocation.Right)
                PART_RightCntl.Children.Add(btn);
            else
                PART_LeftCntl.Children.Add(btn);
            return btn;
        }

        private void RowDockPane(int level, Button btn, Grid parentGrid)
        {
            var item = _rowLayers[level];
            item.Btn.Visibility = Visibility.Collapsed;
            var rtTrans = new RotateTransform(90);
            btn.LayoutTransform = rtTrans;
            parentGrid.RowDefinitions.Add(_rowLayers[0].RowDefinitions[level - 1]);
            for (var i = level + 1; i < _rowLayers.Count; i++)
            {
                if (_rowLayers[i].Btn.Visibility == Visibility.Collapsed)
                    item.Grid.RowDefinitions.Add(item.RowDefinitions[i - level - 1]);
            }
            for (var i = 1; i < level; i++)
            {
                var loc = _rowLayers[i];
                if (loc.Btn.Visibility == Visibility.Collapsed)
                    loc.Grid.RowDefinitions.Add(loc.RowDefinitions[level - 1 - i]);
            }
        }

        private void RowUndockPane(int level, Button btn, Grid parentGrid)
        {
            var item = _rowLayers[level];
            item.Btn.Visibility = Visibility.Visible;
            btn.LayoutTransform = null;
            item.Grid.Visibility = Visibility.Visible;
            parentGrid.RowDefinitions.Remove(_rowLayers[0].RowDefinitions[level - 1]);
            for (int i = 1; i < level; i++)
            {
                _rowLayers[i].Grid.RowDefinitions.Remove(_rowLayers[i].RowDefinitions[level - 1 - i]);
            }
            foreach (RowDefinition t in item.RowDefinitions)
            {
                item.Grid.RowDefinitions.Remove(t);
            }
        }

        private void ColumnDockPane(int level, Button btn)
        {
            var item = _columnLayers[level];
            item.Btn.Visibility = Visibility.Collapsed;
            var rtTrans = new RotateTransform(90);
            btn.LayoutTransform = rtTrans;
            if (_columnLayers[0].ColumnLocations[level - 1] == Layer.LayerColumnLocation.Right)
                _columnLayers[0].Grid.ColumnDefinitions.Add(_columnLayers[0].ColumnDefinitions[level - 1]);
            else
            {
                _columnLayers[0].MainContentPositionIncrement();
                _columnLayers[0].Grid.ColumnDefinitions.Insert(0, _columnLayers[0].ColumnDefinitions[level - 1]);
                Grid.SetColumn(_columnLayers[0].Grid.Children[0], _columnLayers[0].MainContentLocation);
            }

            for (var i = level + 1; i < _columnLayers.Count; i++)
            {
                if (_columnLayers[i].Btn.Visibility != Visibility.Collapsed)
                    continue;
                if (item.ColumnLocations[i - level - 1] == Layer.LayerColumnLocation.Right)
                    item.Grid.ColumnDefinitions.Add(item.ColumnDefinitions[i - level - 1]);
                else
                {
                    item.MainContentPositionIncrement();
                    item.Grid.ColumnDefinitions.Insert(0, item.ColumnDefinitions[i - level - 1]);
                    foreach (UIElement child in item.Grid.Children)
                    {
                        Grid.SetColumn(child, item.MainContentLocation - 1);
                    }
                }
            }
            for (var i = 1; i < level; i++)
            {
                var loc = _columnLayers[i];
                if (loc.Btn.Visibility != Visibility.Collapsed)
                    continue;
                if (loc.ColumnLocations[level - 1 - i] == Layer.LayerColumnLocation.Right)
                    loc.Grid.ColumnDefinitions.Add(loc.ColumnDefinitions[level - 1 - i]);
                else
                {
                    loc.MainContentPositionIncrement();
                    loc.Grid.ColumnDefinitions.Insert(0, loc.ColumnDefinitions[level - 1 - i]);
                    foreach (UIElement child in loc.Grid.Children)
                    {
                        Grid.SetColumn(child, loc.MainContentLocation - 1);
                    }

                }
            }
        }

        private void ColumnUndockPane(int level, Button btn)
        {
            var item = _columnLayers[level];
            item.Btn.Visibility = Visibility.Visible;
            btn.LayoutTransform = null;
            item.Grid.Visibility = Visibility.Visible;

            for (var i = 0; i < level; i++)
            {

                if (_columnLayers[i].ColumnLocations[level - 1 - i] == Layer.LayerColumnLocation.Left)
                {

                    _columnLayers[i].MainContentPositionDecrement();
                    if (i == 0)
                        Grid.SetColumn(_columnLayers[i].Grid.Children[0], _columnLayers[i].MainContentLocation);
                    else
                    {
                        foreach (UIElement child in _columnLayers[i].Grid.Children)
                        {
                            Grid.SetColumn(child, _columnLayers[i].MainContentLocation - 1);
                        }

                    }
                }
                _columnLayers[i].Grid.ColumnDefinitions.Remove(_columnLayers[i].ColumnDefinitions[level - 1 - i]);
            }
            int v = 0;
            foreach (var t in item.ColumnDefinitions)
            {

                if (item.ColumnLocations[v++] == Layer.LayerColumnLocation.Left)
                {
                    item.MainContentPositionDecrement();
                    foreach (UIElement child in item.Grid.Children)
                    {
                        Grid.SetColumn(child, item.MainContentLocation - 1);
                    }
                }
                item.Grid.ColumnDefinitions.Remove(t);
            }
        }


        #endregion
    }
}

