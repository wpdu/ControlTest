using GalaSoft.MvvmLight.Ioc;
using MSDNFinder.ViewModel;

namespace MSDNFinder.ViewModel
{
    /// <summary>
    /// Common VM,binding anywhere from views.
    /// </summary>
    public class CommonViewModel : BasicViewModel
    {
        public const string NaviService_Content = "content_navi";
        public const string MsgNaviService_Content = "msg_content_navi";
        public const string RichMediaNaviService_Content = "rich_content_navi";

        //TODO
        private string _appName = "Winchannel";

        public string AppName
        {
            get
            {
                return _appName;
            }
            set
            {
                Set(ref _appName, value);
            }
        }

        public Service.INavigationServiceWrapper MainNaviService => SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>();
        public Service.INavigationServiceWrapper ContentNaviService => SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>(NaviService_Content);
        public Service.INavigationServiceWrapper MsgContentNaviService => SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>(MsgNaviService_Content);
        // For Unilever RichMedia
        public Service.INavigationServiceWrapper HomePage_Extenssion => SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>("HOMEPAGE_EXTENSSION");
        public Service.INavigationServiceWrapper RichMediaNaviService => SimpleIoc.Default.GetInstance<Service.INavigationServiceWrapper>(RichMediaNaviService_Content);
    }
}