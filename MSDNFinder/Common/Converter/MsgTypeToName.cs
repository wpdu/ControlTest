using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinSFA.Common.Converter
{
    public class MsgTypeToName : IValueConverter
    {
        public static Dictionary<int, string> NameDic;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return NameDic[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
