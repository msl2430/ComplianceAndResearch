using System.Windows;

namespace EngineCell.Controls
{
    public class Layer : UIElement
    {
        public enum LayerOrientation
        {
            Row,
            Column
        }

        public enum LayerColumnLocation
        {
            Left,
            Right
        }

        public static readonly DependencyProperty LevelProperty;
        public static readonly DependencyProperty ContentProperty;
        public static readonly DependencyProperty OrientationProperty;
        public static readonly DependencyProperty NameProperty;
        public static readonly DependencyProperty ColumnLocationProperty;

        public int Level
        {
            get { return (int) GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        public UIElement Content
        {
            get { return (UIElement) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public LayerOrientation Orientation
        {
            get { return (LayerOrientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public LayerColumnLocation ColumnLocation
        {
            get { return (LayerColumnLocation) GetValue(ColumnLocationProperty); }
            set { SetValue(ColumnLocationProperty, value); }
        }

        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        static Layer()
        {
            LevelProperty = DependencyProperty.Register("Level", typeof (int), typeof (Layer));
            ContentProperty = DependencyProperty.Register("Content", typeof (UIElement), typeof (Layer));
            OrientationProperty = DependencyProperty.Register("Orientation", typeof (LayerOrientation), typeof (Layer));
            NameProperty = DependencyProperty.Register("Name", typeof (string), typeof (Layer));
            ColumnLocationProperty = DependencyProperty.Register("ColumnLocation", typeof (LayerColumnLocation), typeof (Layer), new PropertyMetadata(LayerColumnLocation.Left));
        }
    }
}
