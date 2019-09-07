using Attention.Core.Data;
using Attention.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Attention.Core.Tools
{
    public class BackgroundTaskAction
    {
        public static async Task Update()
        {
            var api = new PixabayService();
            ImageEntityCollection collection = await api.GetPixabaies(per_page: 5);
            UpdateLiveTitle(collection.Select(p => p.ImageURL));
        }


        private static void UpdateLiveTitle(IEnumerable<string> list)
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

        private static void CleanUpTile() => TileUpdateManager.CreateTileUpdaterForApplication().Clear();
    }
}
