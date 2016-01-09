using Common;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ControlTest.Tool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimeTool : Page
    {
        DispatcherTimer timer;
        public TimeTool()
        {
            this.InitializeComponent();
        }

        private void LongToDataTime(object sender, TextChangedEventArgs e)
        {
            string value = (sender as TextBox).Text;
            long result = 0;
            if (long.TryParse(value, out result))
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(DateTime.Now.ToUtcTimeLong() - result);
                tbox_Days.Text = ts.TotalDays.ToString();
                tbox_LongTimeString.Text = DateTime.Now.Subtract(ts).ToLongDateString();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tb.Text = DateTime.Now.ToUtcTimeLong().ToString();

            //timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (s, ee) => 
            //{ tb.Text = DateTime.Now.ToUtcTimeLong().ToString(); };
            //timer.Start();
            base.OnNavigatedTo(e);  
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //timer.Stop();
            //timer = null;
            base.OnNavigatingFrom(e);
        }

        private void CountLengthChanged(object sender, TextChangedEventArgs e)
        {
            tb_Count.Text = (sender as TextBox).Text.Length.ToString();
        }
    }
}
