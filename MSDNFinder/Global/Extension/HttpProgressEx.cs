using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Global.Extension
{
    public static class HttpProgressEx
    {
        public static uint? GetReceivePercent(this HttpProgress progress)
        {
            uint percent = 0;

            ulong receive = progress.BytesReceived;

            if (progress.TotalBytesToReceive.HasValue)
            {
                ulong total = progress.TotalBytesToReceive.Value;

                percent = Convert.ToUInt32(receive * 100 / total);
            }

            return percent;
        }
    }
}
