using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinSFA.Common.Converter
{
    public class IntToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 1)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public class Int01ToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0 || (int)value == 1)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

}
