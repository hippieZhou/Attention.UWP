using System;

namespace HENG.Services
{
    /// <summary>
    /// https://github.com/bsonnino/LoggingSerilog
    /// https://blogs.msmvps.com/bsonnino/2017/08/07/logging-an-uwp-application-with-serilog-and-reactive-extensions/
    /// </summary>
    public class LoggingService
    {
        private string GetTimeStamp()
        {
            DateTime now = DateTime.Now;
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                                 "{0:D2}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}",
                                 now.Year - 2000,
                                 now.Month,
                                 now.Day,
                                 now.Hour,
                                 now.Minute,
                                 now.Second,
                                 now.Millisecond);
        }
    }
}
