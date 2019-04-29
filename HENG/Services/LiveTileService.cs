using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace HENG.Services
{
    public class LiveTileService 
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
    }
}
