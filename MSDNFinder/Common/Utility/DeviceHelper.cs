using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration.Pnp;
using Windows.System.Profile;

namespace WinSFA.Common.Utility
{
    public class DeviceHelper
    {
        private static Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();

        public static string GetDiviceName()
        {
            return deviceInfo.SystemProductName;
        }

        public static string GetManufacturer()
        {
            return deviceInfo.SystemManufacturer;
        }

        public static string GetOsVersion()
        {
            // get the system version number
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
            ulong v4 = (v & 0x000000000000FFFFL);

            var SystemVersion = $"{v1}.{v2}.{v3}.{v4}";

            return SystemVersion;
        }

        public static string GetPlateform()
        {
            return string.Format("Windows {0}", GetOsVersion());
        }

        public static string GetFriendlyName()
        {
            return deviceInfo.FriendlyName;
        }

        public static string GetHardwareId()
        {
            return deviceInfo.Id.ToString();
        }

        public static string GetDeviceFamily()
        {
            return AnalyticsInfo.VersionInfo.DeviceFamily;
        }

        public static string GetProcessorCount()
        {
            return Environment.ProcessorCount.ToString();
        }

        #region 获取当前系统版本号，摘自：http://attackpattern.com/2013/03/device-information-in-windows-8-store-apps/

        private static async Task<string> GetWindowsVersionAsync()
        {
            var hal = await GetHalDevice(DeviceDriverVersionKey);
            if (hal == null || !hal.Properties.ContainsKey(DeviceDriverVersionKey))
                return null;

            var versionParts = hal.Properties[DeviceDriverVersionKey].ToString().Split('.');
            return string.Join(".", versionParts.Take(2).ToArray());
        }

        private static async Task<PnpObject> GetHalDevice(params string[] properties)
        {
            var actualProperties = properties.Concat(new[] { DeviceClassKey });
            var rootDevices = await PnpObject.FindAllAsync(PnpObjectType.Device,
                actualProperties, RootQuery);

            foreach (var rootDevice in rootDevices.Where(d => d.Properties != null && d.Properties.Any()))
            {
                var lastProperty = rootDevice.Properties.Last();
                if (lastProperty.Value != null)
                    if (lastProperty.Value.ToString().Equals(HalDeviceClass))
                        return rootDevice;
            }
            return null;
        }

        private const string DeviceClassKey = "{A45C254E-DF1C-4EFD-8020-67D146A850E0},10";
        private const string DeviceDriverVersionKey = "{A8B865DD-2E3D-4094-AD97-E593A70C75D6},3";
        private const string RootContainer = "{00000000-0000-0000-FFFF-FFFFFFFFFFFF}";
        private const string RootQuery = "System.Devices.ContainerId:=\"" + RootContainer + "\"";
        private const string HalDeviceClass = "4d36e966-e325-11ce-bfc1-08002be10318";

        #endregion 获取当前系统版本号，摘自：http://attackpattern.com/2013/03/device-information-in-windows-8-store-apps/
    }
}