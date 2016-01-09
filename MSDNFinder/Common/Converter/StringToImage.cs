using System;
using Windows.UI.Xaml.Data;

namespace WinSFA.Common.Converter
{
    public class StringToImage : IValueConverter
    {
        static string[] Urlsuf = { ".pdf", ".doc", ".xls", ".ppt" };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //string url = value != null ? value.ToString() : null;
            //if (!string.IsNullOrEmpty(url))
            //{
            //    foreach (var item in Urlsuf)
            //    {
            //        if (url.Contains(item))
            //        {
            //            return new Uri(string.Format("ms-appx:///Resource/image/icon_file_{0}@2x.png", item.Substring(1)), UriKind.RelativeOrAbsolute);
            //        }
            //    }
            //    if (url.StartsWith("/")) url = url.Substring(1);
            //    return new Uri(string.Format("{0}{1}", Config.CONFIG_PARAM_URL, url.Replace('\\', '/')), UriKind.Absolute);
            //}
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
