using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class DateTimeExt
    {
        public static long GetUtcTimeLong(this DateTime time)
        {
            return time.ToFileTime();
        }

        public static DateTime GetUtcDateTime(this DateTime time)
        {
            DateTime dtZero = new DateTime(1970, 1, 1, 0, 0, 0);
            return dtZero;
        }

        public static long ToUtcTimeLong(this DateTime time)
        {
            var utcTime = time.ToUniversalTime();
            DateTime dtZero = new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)utcTime.Subtract(dtZero).TotalMilliseconds;
        }

        public static string ToLongDateString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH-mm-ss");
        }
    }


}
