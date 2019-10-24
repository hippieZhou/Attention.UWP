using PixabaySharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HENG.UWP.Models
{
    public class DownloadItem
    {
        public ImageItem Photo { get; set; }
        public DateTime Now { get; set; }

        public DownloadItem(ImageItem item)
        {
            Photo = item;
            Now = DateTime.Now;
        }
    }
}
