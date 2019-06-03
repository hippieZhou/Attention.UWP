using HENG.Core.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace HENG.Tasks
{
    public sealed class HENGBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, reason) =>
            {
                IEnumerable<IBackgroundTaskRegistration> tasks = BackgroundTaskRegistration.AllTasks.Values.Where(t => t.Name == nameof(HENGBackgroundTask));
                tasks.AsParallel().ForAll(p => p.Unregister(true));
            };
            await RunBackgroundTaskAsync();

            deferral.Complete();
        }

        private async Task RunBackgroundTaskAsync()
        {
            try
            {
                var result = await new PixabayService("12645414-59a5251905dfea7b916dd796f").QueryImagesAsync(per_page: 5);
                IEnumerable<string> urls = from p in result.Images select p.LargeImageURL;
                UpdateLiveTile(urls);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        private void UpdateLiveTile(IEnumerable<string> urls)
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
