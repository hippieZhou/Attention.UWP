using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENG.Models
{
    public class AppSettings
    {
        public Bingconf BingConf { get; set; }
        public string DownloadPath { get; set; }
    }

    public class Bingconf
    {
        public string ApiKey { get; set; }
    }
}
