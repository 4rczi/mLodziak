using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlodziakApp.Utilities
{
    public static class UtcToCentralEuropeDateTimeConverter
    {
        public static DateTime LocalToCentralEuropeDateTime(DateTime dateTime)
        {
            var dateTimeUTC = dateTime.ToUniversalTime();

            var centralEuropeTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var centralEuropeTimeNow = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUTC, centralEuropeTimeZone);

            return centralEuropeTimeNow;
        }
    }
}
