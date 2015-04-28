using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENbt
{
    internal static class UnixTimeConversions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTimeMilliseconds(this long milliseconds)
        {
            return Epoch.AddMilliseconds(milliseconds);
        }

        public static DateTime FromUnixTimeSeconds(this long seconds)
        {
            return Epoch.AddSeconds(seconds);
        }

        public static long GetCurrentUnixTimeMilliseconds()
        {
            return (long)(DateTime.UtcNow - Epoch).TotalMilliseconds;
        }

        public static long GetCurrentUnixTimeSeconds()
        {
            return ToUnixTimeSeconds(DateTime.UtcNow);
        }

        public static long ToUnixTimeMilliseconds(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - Epoch).TotalMilliseconds;
        }

        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }
    }
}
