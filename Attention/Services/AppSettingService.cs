using Attention.Commons;
using Attention.Extensions;
using Attention.Models;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Globalization;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Attention.Services
{
    public class AppSettingService
    {
        private ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;
        public string DownloadPath { get; private set; }

        public (FilterItem Order, FilterItem Orientation, FilterItem ImageType, FilterItem Category) Filter;

        private static AppTheme AppTheme;
        private static AppLanguage AppLanguage;
        private static bool AppLiveTitleIsOn;

        static AppSettingService()
        {
            ThemeListener themeListener = new ThemeListener();
            themeListener.ThemeChanged += (sender) =>
            {
                TitleBarHelper.Instance.RefreshTitleBar();
            };
        }

        public async Task InitializeAsync()
        {
            object themeName = GetSettingsValue(nameof(AppTheme), ElementTheme.Default);
            Enum.TryParse(themeName?.ToString(), out ElementTheme cacheTheme);
            AppTheme = new AppTheme
            {
                Theme = cacheTheme,
                Checked = true
            };
            await RefreshUIThemeAsync();

            AppLanguage = new AppLanguage
            {
                Code = GetSettingsValue(nameof(AppLanguage), GlobalizationPreferences.Languages[0]).ToString(),
                Checked = true
            };
            ApplicationLanguages.PrimaryLanguageOverride = AppLanguage.Code;

            object liveTitleState = GetSettingsValue(nameof(AppLiveTitleIsOn), false);
            bool.TryParse(liveTitleState?.ToString(), out bool cacheState);
            AppLiveTitleIsOn = cacheState;

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
            DownloadPath = folder.Path;

            await InitializeFilterAsync();
        }

        #region Themes
        public IEnumerable<AppTheme> GetThemes()
        {
            var list = new List<AppTheme>();
            //var enumerator = Enum.GetValues(typeof(ElementTheme)).Cast<ElementTheme>().GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    var theme = new AppTheme
            //    {
            //        Theme = enumerator.Current,
            //        Checked = AppTheme.Theme == enumerator.Current
            //    };
            //    list.Add(theme);
            //}

            list.AddRange(new[]
            {
                new AppTheme{ Theme = ElementTheme.Default, Description = "theme_default".GetLocalized() ,Checked = AppTheme.Theme == ElementTheme.Default },
                new AppTheme{ Theme = ElementTheme.Light, Description = "theme_light".GetLocalized() ,Checked = AppTheme.Theme == ElementTheme.Light },
                new AppTheme{ Theme = ElementTheme.Dark, Description = "theme_dark".GetLocalized() ,Checked = AppTheme.Theme == ElementTheme.Dark },
            });

            return list;
        }

        public async Task UpdateThemeAsync(AppTheme theme)
        {
            AppTheme = theme;
            await RefreshUIThemeAsync();
            TitleBarHelper.Instance.RefreshTitleBar();
            SetSettingsValue(nameof(AppTheme), AppTheme.Theme.ToString());
        }
         
        private async Task RefreshUIThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = AppTheme.Theme;
                    }
                });
            }
        }
        #endregion

        #region Language
        public IEnumerable<AppLanguage> GetLanguages()
        {
            var list = new List<AppLanguage>();

            list.AddRange(new[]
            {
                new AppLanguage
                {
                    Logo = "ms-appx:///Assets/flag-cn.png",
                    Code = "zh-Hans-CN",
                    Checked = string.Equals(AppLanguage.Code, "zh-Hans-CN", StringComparison.OrdinalIgnoreCase),
                },
                new AppLanguage
                {
                    Logo = "ms-appx:///Assets/flag-us.png",
                    Code = "en-US" ,
                    Checked =string.Equals(AppLanguage.Code, "en-US", StringComparison.OrdinalIgnoreCase)
                }
            });

            return list;
        }

        public async Task UpdateLanguageAsync(AppLanguage language)
        {
            AppLanguage = language;
            SetSettingsValue(nameof(AppLanguage), AppLanguage.Code.ToString());
            await Task.CompletedTask;
        }
        #endregion

        #region LiveTitle
        public bool GetLiveTitleState() => AppLiveTitleIsOn;
        public void UpdateLiveTitleState(bool isOn)
        {
            AppLiveTitleIsOn = isOn;
            if (AppLiveTitleIsOn)
            {
                LiveTileHelper.UpdateLiveTile();
            }
            else
            {
                LiveTileHelper.CleanUpTile();
            }
            SetSettingsValue(nameof(AppLiveTitleIsOn), AppLiveTitleIsOn.ToString());
        }
        #endregion

        #region Filters

        private async Task InitializeFilterAsync()
        {
            async Task<FilterItem> ParseActionAsync(string key, string defaultValue)
            {
                var str = GetSettingsValue(key, defaultValue)?.ToString();
                return await JsonHelper.ToObjectAsync<FilterItem>(str);
            }

            FilterItem order = await ParseActionAsync(nameof(Filter.Order), string.Empty);
            FilterItem orientation = await ParseActionAsync(nameof(Filter.Orientation), string.Empty);
            FilterItem imageType = await ParseActionAsync(nameof(Filter.ImageType), string.Empty);
            FilterItem categorie = await ParseActionAsync(nameof(Filter.Category), string.Empty);

            Filter = (order, orientation, imageType, categorie);
        }

        public async Task UpdateFiletersAsync(FilterItem order, FilterItem orientation, FilterItem imageType, FilterItem category)
        {
            Filter = (order, orientation, imageType, category);

            SetSettingsValue(nameof(Filter.Order), await JsonHelper.StringifyAsync(Filter.Order));
            SetSettingsValue(nameof(Filter.Orientation), await JsonHelper.StringifyAsync(Filter.Orientation));
            SetSettingsValue(nameof(Filter.ImageType), await JsonHelper.StringifyAsync(Filter.ImageType));
            SetSettingsValue(nameof(Filter.Category), await JsonHelper.StringifyAsync(Filter.Category));
        }

        public (IEnumerable<FilterItem> orders, IEnumerable<FilterItem> orientations, IEnumerable<FilterItem> imageTypes, IEnumerable<FilterItem> categories) GetCheckedFilters()
        {
            return default;
            //IEnumerable<FilterItem> GetCheckedFilters(IEnumerable<FilterItem> items) => items.Where(p => p.Checked);
            //return (GetCheckedFilters(Filters[0].Items), GetCheckedFilters(Filters[1].Items), GetCheckedFilters(Filters[2].Items), GetCheckedFilters(Filters[3].Items));
        }
        #endregion

        #region Base
        private object GetSettingsValue<T>(string name, T defaultValue) 
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue?.ToString();
                }
                return LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }

        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value.ToString();
        }
        #endregion
    }
}
