using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace HENG.UWP.Helpers
{
    public static class LiveTileUpdater
    {
        public static void UpdateLiveTile()
        {
            var images = new List<string>();

            var photosContent = new TileBindingContentPhotos();
            foreach (var item in images)
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
                CleanUpTile();
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(tileContent.GetXml()));
            }
            catch (Exception)
            {
            }
        }

        public static void CleanUpTile() => TileUpdateManager.CreateTileUpdaterForApplication().Clear();
    }
}
