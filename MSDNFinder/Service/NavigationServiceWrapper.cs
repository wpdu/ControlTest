using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Service
{
    /// <summary>
    /// For SimpleIoc container to store the implement instance.
    /// </summary>
    public interface INavigationServiceWrapper
    {
        bool CanGoBack { get; }

        Frame Frame { get; }

        void Navigate(string pageKey);

        void Navigate(string pageKey, object parameter);

        void GoBack();

        void RemoveBackEntry();

        void SetFrame(Frame frame);
    }

    public sealed class NavigationServiceWrapper : INavigationServiceWrapper
    {
        private static Assembly _CurrentAssembly = null;
        private string _NaviKey = null;

        private Frame NaviFrame = null;

        public NavigationServiceWrapper()
        {
            SetFrame(Window.Current.Content as Frame);
        }

        public NavigationServiceWrapper(string serviceKey)
        {
            _NaviKey = serviceKey;
        }

        public bool CanGoBack
        {
            get
            {
                return NaviFrame.CanGoBack;
            }
        }

        public Frame Frame
        {
            get
            {
                return NaviFrame;
            }
        }

        public string CurrentPageKey
        {
            get
            {
                return string.Empty;
            }
        }

        private static Assembly CurrentAssembly
        {
            get
            {
                if (_CurrentAssembly == null)
                {
                    _CurrentAssembly = Assembly.Load(new AssemblyName(Global.View.ViewProject));
                }
                return _CurrentAssembly;
            }
        }

        public static Type GetType(string pageName)
        {
            return CurrentAssembly.GetType(string.Format("{0}.{1}", Global.View.ViewNamespace, pageName));
        }

        public void GoBack()
        {
            NaviFrame.GoBack();
        }

        public void Navigate(string pageKey)
        {
            Navigate(pageKey, null);
        }

        public void Navigate(string pageKey, object parameter)
        {
            try
            {
                NaviFrame.Navigate(GetType(pageKey), parameter);
            }
            catch (Exception ex)
            {
                WinSFA.Common.Logging.Logger.Log(WinSFA.Common.Logging.LogType.Exception, "Navigate to " + (pageKey ?? "null") + " error", ex);
            }
        }

        public void RemoveBackEntry()
        {
            NaviFrame.BackStack.Clear();
        }

        public void SetFrame(Frame frame)
        {
            this.NaviFrame = frame;
        }
    }
}