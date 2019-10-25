using Attention.UWP.Extensions;
using Attention.UWP.Models;
using PixabaySharp.Models;
using System;
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
                args.Request.Data.ShareCompleted += (sender, e) =>
                {

                };

                ShareSourceData config = new ShareSourceData($"{App.Settings.Name}", "AppDescription".GetLocalized());
                config.SetApplicationLink(new Uri(item.WebformatURL));
                config.SetWebLink(new Uri(item.WebformatURL));
                args.Request.SetData(config);
                args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(item.PreviewURL)));
                args.Request.Data.SetWebLink(new Uri(item.WebformatURL));
                args.Request.FailWithDisplayText("Oops!");
            };

            dataTransferManager.TargetApplicationChosen += (sender, e) =>
            {

            };
            DataTransferManager.ShowShareUI();
        }
    }
}
