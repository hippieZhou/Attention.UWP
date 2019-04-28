using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HENG.Services
{
    public class AppSettingService
    {
        public static async Task<StorageFolder> GetDeaultDownloadPathAsync()
        {
            StorageFolder storageFolder  = await KnownFolders.PicturesLibrary.CreateFolderAsync("HENG", CreationCollisionOption.OpenIfExists);
            return storageFolder;
        }
    }
}
