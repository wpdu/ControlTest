using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Service;

namespace MSDNFinder.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Main Navigation Servie
            SimpleIoc.Default.Register<INavigationServiceWrapper>(() => new NavigationServiceWrapper());

            // Register Diaglog Service
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            // Register Page Models
            //SimpleIoc.Default.Register<CommonViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<DownloadViewModel>();
        }

        public static CommonViewModel Common => ServiceLocator.Current.GetInstance<CommonViewModel>();

        public MainPageViewModel Main => ServiceLocator.Current.GetInstance<MainPageViewModel>();
        public DownloadViewModel MuLuDL => ServiceLocator.Current.GetInstance<DownloadViewModel>();

    }
}