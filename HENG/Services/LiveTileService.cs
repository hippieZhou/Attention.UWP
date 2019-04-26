using HENG.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.UI.Notifications;

namespace HENG.Services
{
    public class LiveTileService 
    {
        public void CreateLiveTitle(BingItem bing)
        {
            string from = bing.Datetime.ToString("yyyy-MM-dd");
            string subject = bing.Title;
            string body = bing.Description;

            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Arguments = bing.Caption,

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            PeekImage = new TilePeekImage()
                            {
                                Source = bing.Url
                            },
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = from,
                                    HintWrap = true
                                },
                                new AdaptiveText()
                                {
                                    Text = subject,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintWrap = true
                                },
                                new AdaptiveText()
                                {
                                    Text = body,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintWrap = true
                                }
                            }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            PeekImage = new TilePeekImage()
                            {
                                Source = bing.Url
                            },
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = from,
                                    HintStyle = AdaptiveTextStyle.Subtitle,
                                    HintWrap = true
                                },
                                new AdaptiveText()
                                {
                                    Text = subject,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintWrap = true
                                },
                                new AdaptiveText()
                                {
                                    Text = body,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                    HintWrap = true
                                }
                            }
                        }
                    }
                }
            };

            var notification = new TileNotification(content.GetXml());
            UpdateLiveTile(notification);
        }

        private void UpdateLiveTile(TileNotification notification)
        {
            try
            {
                TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }
            catch (Exception)
            {
                // TODO WTS: Updating LiveTile can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }
    }
}
