using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.UI.Notifications;

namespace Attention.UWP.Helpers
{
    public static class LiveTileHelper
    {
        public static void UpdateLiveTile()
        {
            //todo
        }

        public static void UpdateLiveTile(IEnumerable<string> list)
        {
            var photosContent = new TileBindingContentPhotos();

            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                photosContent.Images.Add(new TileBasicImage { Source = enumerator.Current, AddImageQuery = true });
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
