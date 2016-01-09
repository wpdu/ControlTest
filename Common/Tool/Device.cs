using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Common.Tool
{
    public class Device
    {
        public static Size ScreenSize()
        {
            return new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }
    }
}
