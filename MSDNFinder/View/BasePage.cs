using GalaSoft.MvvmLight.Ioc;
using Service;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MSDNFinder.View
{
    public class BasePage : Page
    {
        #region Public Fields

        // Using a DependencyProperty as the backing store for BackPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackPageProperty =
            DependencyProperty.Register("BackPage", typeof(string), typeof(string), null);

        // Using a DependencyProperty as the backing store for LoadedCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadedCmdProperty =
            DependencyProperty.Register("LoadedCmd", typeof(ICommand), typeof(BasePage), null);

        // Using a DependencyProperty as the backing store for NavigateFromCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigatedFromCmdProperty =
            DependencyProperty.Register("NavigatedFromCmd", typeof(ICommand), typeof(BasePage), null);

        // Using a DependencyProperty as the backing store for NavigatedCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigatedToCmdProperty =
            DependencyProperty.Register("NavigatedToCmd", typeof(ICommand), typeof(BasePage), null);

        // Using a DependencyProperty as the backing store for RemoveBackEnry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoveBackEnryProperty =
            DependencyProperty.Register("RemoveBackEnry", typeof(bool), typeof(BasePage), new PropertyMetadata(false));

        #endregion Public Fields

        #region Public Constructors

        static BasePage()
        {
            //TODO
        }

        public BasePage()
        {
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Loaded += BasePage_Loaded;
        }

        #endregion Public Constructors

        #region Public Properties

        public string BackPage
        {
            get { return (string)GetValue(BackPageProperty); }
            set { SetValue(BackPageProperty, value); }
        }

        public ICommand LoadedCmd
        {
            get { return (ICommand)GetValue(LoadedCmdProperty); }
            set { SetValue(LoadedCmdProperty, value); }
        }

        public ICommand NavigatedFromCmd
        {
            get { return (ICommand)GetValue(NavigatedFromCmdProperty); }
            set { SetValue(NavigatedFromCmdProperty, value); }
        }

        public ICommand NavigatedToCmd
        {
            get { return (ICommand)GetValue(NavigatedToCmdProperty); }
            set { SetValue(NavigatedToCmdProperty, value); }
        }

        public bool RemoveBackEnry
        {
            get { return (bool)GetValue(RemoveBackEnryProperty); }
            set { SetValue(RemoveBackEnryProperty, value); }
        }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (NavigatedFromCmd != null && NavigatedFromCmd.CanExecute(e.Parameter))
            {
                NavigatedFromCmd.Execute(e.Parameter);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigatedToCmd != null && NavigatedToCmd.CanExecute(e.Parameter))
            {
                NavigatedToCmd.Execute(e.Parameter);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void BasePage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (LoadedCmd != null && LoadedCmd.CanExecute(null))
            {
                LoadedCmd.Execute(null);
            }
            if (RemoveBackEnry)
            {
                var NavService = SimpleIoc.Default.GetInstance<INavigationServiceWrapper>();
                NavService.RemoveBackEntry();
            }
        }

        #endregion Private Methods
    }
}