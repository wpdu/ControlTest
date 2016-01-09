using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinSFA.Common.Converter
{
    public class BoolToNorVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if ((bool)value)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}