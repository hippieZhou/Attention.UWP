using Serilog;
using System;
using System.Globalization;

namespace Attention.Services
{
    public class LogService: IDisposable
    {
        public LogService()
        {
        }

        private string GetTimeStamp()
        {
            DateTime now = DateTime.Now;
            return string.Format(CultureInfo.InvariantCulture,
                                 "{0:D2}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}",
                                 now.Year - 2000,
                                 now.Month,
                                 now.Day,
                                 now.Hour,
                                 now.Minute,
                                 now.Second,
                                 now.Millisecond);
        }

        public void Dispose()
        {

        }
    }
}
