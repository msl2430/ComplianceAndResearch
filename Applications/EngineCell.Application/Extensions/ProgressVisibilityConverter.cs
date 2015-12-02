using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using EngineCell.Core.Constants;

namespace EngineCell.Application.Extensions
{
    internal class ProgressVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (System.Convert.ToInt32(parameter))
            {
                case (int)ControlConstants.ProgressType.ProgressRing:
                    return (StatusConstants.ProgressStatus)value == StatusConstants.ProgressStatus.Active ? Visibility.Visible : Visibility.Hidden;
                case (int)ControlConstants.ProgressType.Finished:
                    return (StatusConstants.ProgressStatus)value == StatusConstants.ProgressStatus.Finished ? Visibility.Visible : Visibility.Hidden;
                default:
                    return (StatusConstants.ProgressStatus)value != StatusConstants.ProgressStatus.Finished ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
