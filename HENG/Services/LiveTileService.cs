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
            var title = new TileBinding();
            var photosContent = new TileBindingContentPhotos();
            foreach (var item in urls)
            {
                photosContent.Images.Add(new TileBasicImage { Source = item, AddImageQuery = true });
            }

            title.Content = photosContent;

            var tileContent = new TileContent
            {
                Visual = new TileVisual()
            };
            tileContent.Visual.Branding = TileBranding.NameAndLogo;
            tileContent.Visual.TileMedium = title;
            tileContent.Visual.TileWide = title;
            tileContent.Visual.TileLarge = title;

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
