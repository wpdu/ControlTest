using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

namespace MSDNFinder.ViewModel
{
    /// <summary>
    /// Inherit from ViewModelBase
    /// </summary>
    public class BasicViewModel : ViewModelBase
    {
        public GalaSoft.MvvmLight.Views.IDialogService DialogService => SimpleIoc.Default.GetInstance<IDialogService>();

        public BasicViewModel()
        {
        }

        public static Service.INavigationServiceWrapper GetNaviServiceByKey(string key)
        {
            return SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>(key);
        }

        public virtual void Initialize()
        {
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}