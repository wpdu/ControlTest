using GalaSoft.MvvmLight.Command;
using MSDNFinder.Manager;
using MSDNFinder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSDNFinder.ViewModel
{
    public class DownloadViewModel : BasicViewModel
    {
        public DownloadViewModel()
        {
            if (IsInDesignMode)
            {
                Progress = new Progress() { Saved = 50, Total = 100 };
            }
        }
        private Progress _progress = null;
        public Progress Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                Set(ref _progress, value);
            }
        }
        

        private RelayCommand _onNavigatedToCmd;
        /// <summary>
        /// Gets the LoadedCmd.
        /// </summary>
        public RelayCommand OnNavigatedToCmd
        {
            get
            {
                return _onNavigatedToCmd
                    ?? (_onNavigatedToCmd = new RelayCommand(
                    () =>
                    {
                        Progress = new Progress() { Saved = DataManager.GetSaved(), Total = DataManager.GetTotal() };

                    }));
            }
        }

        private RelayCommand _startCmd;

        /// <summary>
        /// Gets the StartCmd.
        /// </summary>
        public RelayCommand StartCmd
        {
            get
            {
                return _startCmd
                    ?? (_startCmd = new RelayCommand(
                    async () =>
                    {
                        bool succ = await DataManager.DownloadAllJson(Progress);
                    }));
            }
        }
        private RelayCommand _stopCmd;
        /// <summary>
        /// Gets the StopCmd.
        /// </summary>
        public RelayCommand StopCmd
        {
            get
            {
                return _stopCmd
                    ?? (_stopCmd = new RelayCommand(
                    () =>
                    {
                        FastDownload.isStop = true;
                    }));
            }
        }
    }

    public class Progress : BaseModel
    {
        private int _saved = 0;
        public int Saved
        {
            get
            {
                return _saved;
            }
            set
            {
                Set(ref _saved, value);
            }
        }

        private int _total = 0;
        public int Total
        {
            get
            {
                return _total;
            }
            set
            {
                Set(ref _total, value);
            }
        }

    }
}
