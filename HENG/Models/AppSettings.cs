using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System;
using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Uwp.Helpers;

namespace HENG.Models
{
    /// <summary>
    /// https://edi.wang/post/2017/9/8/uwp-read-write-settings
    /// https://edi.wang/post/2016/2/18/windows-10-uwp-async-await-ui-thread
    /// https://edi.wang/post/2017/9/9/windows-10-uwp-switching-language
    /// https://edi.wang/post/2015/12/30/windows-10-uwp-report-error-page
    /// </summary>
    public class AppSettings: ObservableObject
    {
        private const string ThemeKey = "AppBackgroundRequestedTheme";
        public ApplicationDataContainer LocalSettings { get;private set; }
        public AppSettings()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
            Theme = ReadSettings(ThemeKey, ElementTheme.Default);
        }

        private ElementTheme _theme;
        public ElementTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        private void SaveSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        private T ReadSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }
            if (null != defaultValue)
            {
                return defaultValue;
            }
            return default(T);
        }
    }
}
