using System;
using System.Threading.Tasks;
using WinSFA.Common.Logging;
using Windows.Devices.Geolocation;

namespace Service
{
    public class GpsService
    {
        private Geolocator _geolocator = null;
        private uint _accuracyInMeter = 50;
        private uint _maxAgeInMinute = 5;
        private uint _timeoutInSecond = 10;

        private static GpsService _current;

        public static GpsService Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new GpsService();
                }
                return _current;
            }
        }
         
        private GpsService()
        {
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracyInMeters = _accuracyInMeter;
        }

        public async Task<Geoposition> GetPosition()
        {
            Geoposition position = null;
            try
            {
                position = await _geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(_maxAgeInMinute),
                    timeout: TimeSpan.FromSeconds(_timeoutInSecond)
                    );
            }
            catch (Exception ex)
            {
                Logger.Log(LogType.Exception, "Get User Postion Error", ex);
            }

            return position;
        }
    }
}