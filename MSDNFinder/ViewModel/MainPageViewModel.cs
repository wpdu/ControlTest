using GalaSoft.MvvmLight.Command;
using MSDNFinder.Manager;
using MSDNFinder.Model;
using MSDNFinder.Model.DataJson;
using MSDNFinder.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinSFA.Common.Network;
using WinSFA.Common.Serializer;

namespace MSDNFinder.ViewModel
{
    public class MainPageViewModel : BasicViewModel
    {

        //https://msdn.microsoft.com/zh-cn/library/windows/apps/hh703192.aspx

        private ClientModel cmBackUp;

        public MainPageViewModel()
        {
            if (IsInDesignMode)
            {
                ServerModel s = new ServerModel()
                {
                    _id = 0,
                    Title = "Windows",
                    Href = "https://msdn.microsoft.com/zh-cn/library/windows/apps/hh703192.aspx",
                    ToolTip = "Windows",
                    hassubtree = true,
                    level = 0
                };
                CurrModel = new ClientModel(s, null);
                ObservableCollection<ClientModel> subs = new ObservableCollection<ClientModel>();
                subs.Add(new ClientModel(new ServerModel() { Title = "热门" }, CurrModel));
                subs.Add(new ClientModel(new ServerModel() { Title = "设计" }, CurrModel));
                subs.Add(new ClientModel(new ServerModel() { Title = "开发" }, CurrModel));
                subs.Add(new ClientModel(new ServerModel() { Title = "发布" }, CurrModel));
                CurrModel.Subs = subs;

            }

        }

        private bool _isSearching = false;
        public bool IsSearching
        {
            get
            {
                return _isSearching;
            }
            set
            {
                Set(ref _isSearching, value);
            }
        }

        private bool _isPanelHide = false;
        public bool IsPanelHide
        {
            get
            {
                return _isPanelHide;
            }
            set
            {
                Set(ref _isPanelHide, value);
            }
        }


        private string _urlString = "";
        public string UrlString
        {
            get
            {
                return _urlString;
            }
            set
            {
                Set(ref _urlString, value);
            }
        }

        private Uri _webViewUrl = null;
        public Uri WebViewUrl
        {
            get
            {
                return _webViewUrl;
            }
            set
            {
                Set(ref _webViewUrl, value);
            }
        }

        private bool _isProgressActive = false;
        public bool IsProgressActive
        {
            get
            {
                return _isProgressActive;
            }
            set
            {
                Set(ref _isProgressActive, value);
            }
        }

        private ClientModel _basicClientModel = null;
        public ClientModel BaseicClientModel
        {
            get
            {
                return _basicClientModel;
            }
            set
            {
                Set(ref _basicClientModel, value);
            }
        }

        private ClientModel _currModel = null;
        public ClientModel CurrModel
        {
            get
            {
                return _currModel;
            }
            set
            {
                Set(ref _currModel, value);
            }
        }


        private ClientModel _selectedClientModel = null;
        public ClientModel SelectedClientModel
        {
            get
            {
                return _selectedClientModel;
            }
            set
            {
                Set(ref _selectedClientModel, value);
                OnSelectedClientModelChanged(value);
            }
        }

        private void OnSelectedClientModelChanged(ClientModel value)
        {
            if (value != null && value.serverModel != null)
            {
                //string path = await Manager.DataManager.DownloadPage(value);
                //WebViewUrl = new Uri(path, UriKind.RelativeOrAbsolute);
                WebViewUrl = new Uri(value.serverModel.Href, UriKind.Absolute);
                IsProgressActive = true;
                UrlString = value.serverModel.Href;
            }
        }

       
        private RelayCommand<object> _onNavigatedToCmd;
        /// <summary>
        /// Gets the LoadedCmd.
        /// </summary>
        public RelayCommand<object> OnNavigatedToCmd
        {
            get
            {
                return _onNavigatedToCmd
                    ?? (_onNavigatedToCmd = new RelayCommand<object>(
                    async (obj) =>
                    {
                        if (CurrModel == null)
                        {
                            CurrModel = new ClientModel(Manager.DataManager.GetCurrData(), null);
                        }
                        if (CurrModel.Subs.Count == 0)
                        {
                            List<ServerModel> sData = await Manager.DataManager.GetData(CurrModel);
                            sData?.ForEach(c => CurrModel.Subs.Add(new ClientModel(c, CurrModel)));
                            SelectedClientModel = CurrModel;
                        }
                    }));
            }
        }

        private RelayCommand _goHomeCmd;

        /// <summary>
        /// Gets the GoHomeCmd.
        /// </summary>
        public RelayCommand GoHomeCmd
        {
            get
            {
                return _goHomeCmd
                    ?? (_goHomeCmd = new RelayCommand(
                    () =>
                    {
                        CurrModel = cmBackUp;
                    }));
            }
        }

        private RelayCommand<object> _expandSubsCmd;
        /// <summary>
        /// Gets the ExpandSubsCmd.
        /// </summary>
        public RelayCommand<object> ExpandSubsCmd
        {
            get
            {
                return _expandSubsCmd
                    ?? (_expandSubsCmd = new RelayCommand<object>(
                    async (obj) =>
                    {
                        if (obj is ClientModel)
                        {
                            ClientModel cm = obj as ClientModel;
                            if (CurrModel != cm)
                            {
                                if (cm.Subs.Count == 0 && cm.HaveChild)
                                {
                                    List<ServerModel> sData = await Manager.DataManager.GetData(cm);
                                    sData.ForEach(c => cm.Subs.Add(new ClientModel(c, cm)));
                                }
                                CurrModel = cm;
                            }
                            else
                            {
                                CurrModel.Parents.Clear();
                                CurrModel.Parents = CurrModel.UpdateParents();
                            }
                        }
                    }));
            }
        }

        private RelayCommand<object> _openInEdge;
        /// <summary>
        /// Gets the OpenInEdge.
        /// </summary>
        public RelayCommand<object> OpenInEdge
        {
            get
            {
                return _openInEdge
                    ?? (_openInEdge = new RelayCommand<object>(
                    async (obj) =>
                    {
                        ClientModel cm = obj as ClientModel;
                        if (cm != null)
                        {
                            bool succ = await Windows.System.Launcher.LaunchUriAsync(new Uri(cm.serverModel.Href, UriKind.RelativeOrAbsolute));
                        }
                    }));
            }
        }

        private RelayCommand<object> _collectCmd;
        /// <summary>
        /// Gets the CollectCmd.
        /// </summary>
        public RelayCommand<object> CollectCmd
        {
            get
            {
                return _collectCmd
                    ?? (_collectCmd = new RelayCommand<object>(
                    (obj) =>
                    {
                        CurrModel = cmBackUp;

                    }));
            }
        }

        private RelayCommand<object> _skepDownload;

        /// <summary>
        /// Gets the SkepDownloadCmd.
        /// </summary>
        public RelayCommand<object> SkepDownloadCmd
        {
            get
            {
                return _skepDownload
                    ?? (_skepDownload = new RelayCommand<object>(
                    (obj) =>
                    {
                        ClientModel tempCM = obj as ClientModel;
                        if (tempCM != null && tempCM.serverModel != null)
                        {
                            tempCM.serverModel.skepdownload = true;
                            bool succ = DataManager.UpdateObj(tempCM);
                        }

                    }));
            }
        }

        public async void GoSugges(ServerModel sv)
        {
            ClientModel cm = new ClientModel(sv, null);
            if (cm.Subs.Count == 0 && cm.HaveChild)
            {
                List<ServerModel> sData = await Manager.DataManager.GetData(cm);
                sData.ForEach(c => cm.Subs.Add(new ClientModel(c, cm)));
            }
            if (cmBackUp == null)
                cmBackUp = CurrModel;
            CurrModel = cm;
        }

        public void ResetLayout()
        {
            CurrModel = cmBackUp;
        }


        private RelayCommand _goDownloadPageCmd;
        /// <summary>
        /// Gets the ExpandSubsCmd.
        /// </summary>
        public RelayCommand GoDownloadPageCmd
        {
            get
            {
                return _goDownloadPageCmd
                    ?? (_goDownloadPageCmd = new RelayCommand(
                    () =>
                    {
                        (Window.Current.Content as Frame).Navigate(typeof(DownloadPage));
                    }));
            }
        }

        private RelayCommand _searchCmd;
        /// <summary>
        /// Gets the ExpandSubsCmd.
        /// </summary>
        public RelayCommand SearchCmd
        {
            get
            {
                return _searchCmd
                    ?? (_searchCmd = new RelayCommand(
                    () =>
                    {
                        IsSearching = true;
                    }));
            }
        }

        private RelayCommand _hidePanelCmd;
        /// <summary>
        /// Gets the ExpandSubsCmd.
        /// </summary>
        public RelayCommand HidePanelCmd
        {
            get
            {
                return _hidePanelCmd
                    ?? (_hidePanelCmd = new RelayCommand(
                    () =>
                    {
                        IsPanelHide = !IsPanelHide;
                    }));
            }
        }
    }

    public class ClientModel : BaseModel
    {

        private double _offsetX = 10;
        public double OffsetX
        {
            get
            {
                return _offsetX;
            }
            set
            {
                Set(ref _offsetX, value);
            }
        }

        private string _title = "";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
            }
        }

        private bool _haveChild = true;
        public bool HaveChild
        {
            get
            {
                return _haveChild;
            }
            set
            {
                Set(ref _haveChild, value);
            }
        }

        private double _childrenSaved = 0.3;
        public double ChildrenSaved
        {
            get
            {
                return _childrenSaved;
            }
            set
            {
                Set(ref _childrenSaved, value);
            }
        }

        private bool _subExpanding = false;
        public bool SubExpanding    
        {
            get
            {
                return _subExpanding;
            }
            set
            {
                Set(ref _subExpanding, value);
            }
        }

        public string GetPath()
        {
            string path = "";
            ClientModel tempParent = Parent;
            while (tempParent != null)
            {
                path += string.Format("{0}\\", tempParent.serverModel.level);
                tempParent = tempParent.Parent;
            }
            path += string.Format("{0}\\", serverModel.level);
            path += string.Format("{0}.html", serverModel.Title);
            return path;
        }

        public ClientModel Parent { get; private set; }

        private ObservableCollection<ClientModel> _parents = null;
        public ObservableCollection<ClientModel> Parents
        {
            get
            {
                return _parents = UpdateParents();
            }
            set
            {
                Set(ref _parents, value);
            }
        }

        public ObservableCollection<ClientModel> UpdateParents()
        {
            ObservableCollection<ClientModel> parents = new ObservableCollection<ClientModel>();
            ClientModel tempParent = Parent;
            while (tempParent != null)
            {
                parents.Insert(0, tempParent);
                tempParent = tempParent.Parent;
            }
            parents.Add(this);
            if (Subs != null)
            {
                foreach (var item in Subs)
                {
                    parents.Add(item);
                }
            }
            return parents;
        }

        private ObservableCollection<ClientModel> _subs = null;
        public ObservableCollection<ClientModel> Subs
        {
            get
            {
                return _subs;
            }
            set
            {
                Set(ref _subs, value);
            }
        }

        public Model.DataJson.ServerModel serverModel { get; private set; }

        public ClientModel(ClientModel parent)
        {
            Subs = new ObservableCollection<ClientModel>();
        }
        public ClientModel(Model.DataJson.ServerModel server, ClientModel parent)
        {
            OffsetX = parent == null ? 10 : parent.OffsetX + 15;
            Parent = parent;
            serverModel = server;
            this.Title = server.Title;
            HaveChild = server.hassubtree;
            ChildrenSaved = serverModel.subtreesaved ? 1 : 0.3;
            Subs = new ObservableCollection<ClientModel>();
        }
    }
}
