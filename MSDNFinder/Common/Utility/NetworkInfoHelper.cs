using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using System.Linq;

namespace WinSFA.Common.Utility
{
    public enum NetworkConnection
    {
        None,
        Wifi,
        Mobile,
    }

    public class NetworkInfoHelper
    {
        public static NetworkInfoHelper Current { get; private set; }

        public event EventHandler OnNetworkStateChanged;

        public NetworkConnection Connection { get; private set; }

        static NetworkInfoHelper()
        {
            Current = new NetworkInfoHelper();
        }

        private NetworkInfoHelper()
        {
            SetNetworkType();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public bool IsNetworkAvaliable
        {
            get
            {
                return Connection != NetworkConnection.None;
            }
        }

        public bool IsWifiAvaliable
        {
            get
            {
                return Connection == NetworkConnection.Wifi;
            }
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            SetNetworkType();
            if (OnNetworkStateChanged != null)
            {
                OnNetworkStateChanged(sender, new EventArgs());
            }
        }

        private void SetNetworkType()
        {
            var ConnectionProfiles = NetworkInformation.GetConnectionProfiles();
            foreach (var item in ConnectionProfiles)
            {
                Debug.WriteLine(item.ProfileName);
            }
            ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile == null || !NetworkInterface.GetIsNetworkAvailable())
            {
                Connection = NetworkConnection.None;
                return;
            }
            var connectLevel = internetConnectionProfile.GetNetworkConnectivityLevel();
            switch (connectLevel)
            {
                case NetworkConnectivityLevel.None:
                case NetworkConnectivityLevel.LocalAccess:
                case NetworkConnectivityLevel.ConstrainedInternetAccess:
                    Connection = NetworkConnection.None;
                    break;

                case NetworkConnectivityLevel.InternetAccess:
                    Connection = NetworkConnection.Wifi;
                    break;

                default:
                    break;
            }

            if (Connection == NetworkConnection.None)
            {
                return;
            }

            if (internetConnectionProfile.IsWlanConnectionProfile)
            {
                Connection = NetworkConnection.Wifi;
            }
            if (internetConnectionProfile.IsWwanConnectionProfile)
            {
                Connection = NetworkConnection.Mobile;
            }
        }

        public string GetIpAddress()
        {
            var ipAddress = string.Empty;
            foreach (var hostName in Windows.Networking.Connectivity.NetworkInformation.GetHostNames())
            {
                
            }
            return string.Empty;
        }

        public string GetWirelessType()
        {
            return Connection == NetworkConnection.Wifi ? "WIFI" : "4G";
        }
    }
}