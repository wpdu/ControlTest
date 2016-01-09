using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ControlTest.Info
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeviceInfo : Page
    {
        DispatcherTimer timer;
        public DeviceInfo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            base.OnNavigatedTo(e);  
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            timer.Stop();
            base.OnNavigatingFrom(e);
        }

        private async void Timer_Tick(object sender, object e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                width.Text = Window.Current.Bounds.Width.ToString(); // 窗口大小
                height.Text = Window.Current.Bounds.Height.ToString();

                width1.Text = PointerDevice.GetPointerDevices()[0].PhysicalDeviceRect.Width.ToString();   // 屏幕大小
                height1.Text = PointerDevice.GetPointerDevices()[0].PhysicalDeviceRect.Height.ToString();

                scale.Text = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel.ToString();  // 缩放值
            });
        }
    }
}
