using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Extension
{
    public static class DateTimeEx
    {
        public static long ToUtcTimeLong(this DateTime time)
        {
            var utcTime = time.ToUniversalTime();
            DateTime dtZone = new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)utcTime.Subtract(dtZone).TotalMilliseconds;
        }
    }
}
