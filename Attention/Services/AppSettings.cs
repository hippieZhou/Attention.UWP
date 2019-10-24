using Attention.Commons;
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
    public class AppSettings
    {
        private ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        public (FilterItem Order, FilterItem Orientation, FilterItem ImageType, FilterItem Category) Filters;

        public ElementTheme AppTheme { get; private set; } = ElementTheme.Default;
        public string AppLanguage { get; private set; } = GlobalizationPreferences.Languages[0];
        public bool AppLiveTitleIsOn { get; private set; } = true;
        public string DownloadPath { get; private set; }

        static AppSettings()
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
            if (Enum.TryParse(themeName?.ToString(), out ElementTheme cacheTheme))
            {
                AppTheme = cacheTheme;
            }
            await RefreshUIThemeAsync();

            object languageName = GetSettingsValue(nameof(AppLanguage), GlobalizationPreferences.Languages[0]);
            AppLanguage = languageName?.ToString();
            ApplicationLanguages.PrimaryLanguageOverride = AppLanguage;

            object liveTitleState = GetSettingsValue(nameof(AppLiveTitleIsOn), false);
            bool.TryParse(liveTitleState?.ToString(), out bool cacheState);
            AppLiveTitleIsOn = cacheState;

            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Attention", CreationCollisionOption.OpenIfExists);
            DownloadPath = folder.Path;

            await InitializeFilterAsync();
        }

        #region Themes
        public async Task UpdateThemeAsync(ElementTheme theme)
        {
            AppTheme = theme;

            await RefreshUIThemeAsync();
            TitleBarHelper.Instance.RefreshTitleBar();

            SetSettingsValue(nameof(AppTheme), AppTheme.ToString());
        }
         
        private async Task RefreshUIThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = AppTheme;
                    }
                });
            }
        }
        #endregion

        #region Language
        public async Task UpdateLanguageAsync(string language)
        {
            AppLanguage = language;
            SetSettingsValue(nameof(AppLanguage), AppLanguage);
            await Task.CompletedTask;
        }
        #endregion

        #region LiveTitle
        public bool GetLiveTitleState() => AppLiveTitleIsOn;
        public void UpdateLiveTitle(bool isOn)
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

            FilterItem order = await ParseActionAsync(nameof(Filters.Order), string.Empty);
            FilterItem orientation = await ParseActionAsync(nameof(Filters.Orientation), string.Empty);
            FilterItem imageType = await ParseActionAsync(nameof(Filters.ImageType), string.Empty);
            FilterItem categorie = await ParseActionAsync(nameof(Filters.Category), string.Empty);

            Filters = (order, orientation, imageType, categorie);
        }

        public async Task UpdateFiletersAsync(FilterItem order, FilterItem orientation, FilterItem imageType, FilterItem category)
        {
            Filters = (order, orientation, imageType, category);

            SetSettingsValue(nameof(Filters.Order), await JsonHelper.StringifyAsync(Filters.Order));
            SetSettingsValue(nameof(Filters.Orientation), await JsonHelper.StringifyAsync(Filters.Orientation));
            SetSettingsValue(nameof(Filters.ImageType), await JsonHelper.StringifyAsync(Filters.ImageType));
            SetSettingsValue(nameof(Filters.Category), await JsonHelper.StringifyAsync(Filters.Category));
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
