using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Notifications;

namespace HENG.Services
{
    public class DataService
    {
        public static void UpdateLiveTile(IEnumerable<string> urls)
        {
            var photosContent = new TileBindingContentPhotos();
            foreach (var item in urls)
            {
                photosContent.Images.Add(new TileBasicImage { Source = item, AddImageQuery = true });
            }

            var title = new TileBinding
            {
                Content = photosContent
            };
            var visual = new TileVisual
            {
                Branding = TileBranding.NameAndLogo,
                TileMedium = title,
                TileWide = title,
                TileLarge = title
            };

            var tileContent = new TileContent
            {
                Visual = visual
            };

            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileContent.GetXml()));
            }
            catch (Exception)
            {
            }
        }

        public static async Task<bool> SetWallpaperImageAsync(StorageFile file)
        {
            if (UserProfilePersonalizationSettings.IsSupported())
            {
                UserProfilePersonalizationSettings settings = UserProfilePersonalizationSettings.Current;
                var b = await settings.TrySetWallpaperImageAsync(file);
                return b;
            }
            else
            {
                return false;
            }
        }
    }
}
