using Attention.UWP.Extensions;
using Attention.UWP.Models;
using Microsoft.Toolkit.Uwp.UI;
using PixabaySharp.Models;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Attention.UWP.Helpers
{
    public static class ShareHelper
    {
        public static void ShareData(ImageItem item)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += (manager, args) =>
            {
                var deferral = args.Request.GetDeferral();

                args.Request.Data.ShareCompleted += (sender, e) =>
                {
                    deferral.Complete();
                    Debug.WriteLine("done");
                };

                ShareSourceData config = new ShareSourceData($"{App.Settings.Name}", "AppDescription".GetLocalized());
                config.SetWebLink(new Uri(item.FullHDImageURL ?? item.LargeImageURL));
                config.SetText($@"
User:{item.User};
Type:{item.Type};
Tags:{item.Tags};
ImageURL:{item.FullHDImageURL ?? item.LargeImageURL}");

                config.SetDeferredContent(StandardDataFormats.Bitmap, async () =>
                {
                    var cacheImage = await ImageCache.Instance.GetFileFromCacheAsync(new Uri(item.LargeImageURL));
                    RandomAccessStreamReference imageStream = RandomAccessStreamReference.CreateFromFile(cacheImage);
                    return imageStream;
                });
                args.Request.SetData(config);
               
                //args.Request.FailWithDisplayText("Oops!");
            };

            dataTransferManager.TargetApplicationChosen += (sender, e) =>
            {

            };
            DataTransferManager.ShowShareUI();
        }
    }
}
