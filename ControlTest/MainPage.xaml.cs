using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ControlTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static Assembly _CurrentAssembly = null;
        public MainPage()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (InnerFrame.CanGoBack)
            {
                InnerFrame.GoBack();
            }
        }

        private static Assembly CurrentAssembly
        {
            get
            {
                if (_CurrentAssembly == null)
                {
                    _CurrentAssembly = Assembly.Load(new AssemblyName("ControlTest"));
                }
                return _CurrentAssembly;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            string className = (sender as HyperlinkButton).Tag.ToString();
            Type type = CurrentAssembly.GetType(string.Format("{0}.{1}", "ControlTest", className));

            InnerFrame.Navigate(type);
        }

        

    }
}
