using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinSFA.Common.Converter
{
    public class BoolToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}