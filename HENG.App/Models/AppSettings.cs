using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Uwp.Extensions;
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;

namespace HENG.App.Models
{
    /// <summary>
    /// https://edi.wang/post/2017/9/8/uwp-read-write-settings
    /// https://edi.wang/post/2016/2/18/windows-10-uwp-async-await-ui-thread
    /// https://edi.wang/post/2017/9/9/windows-10-uwp-switching-language
    /// https://edi.wang/post/2015/12/30/windows-10-uwp-report-error-page
    /// </summary>
    public class AppSettings: ObservableObject
    {
        static private AppSettings _current = null;
        static public AppSettings Current => _current ?? (_current = new AppSettings());

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        public ElementTheme ElementTheme
        {
            get => GetSettingsValue("AppBackgroundRequestedTheme", ElementTheme.Default);
            set
            {
                SetSettingsValue("AppBackgroundRequestedTheme", value);
                RaisePropertyChanged(() => ElementTheme);
            }
        }

        public string AppName => "AppDisplayName".GetLocalized();

        public string AppVersion
        {
            get
            {
                PackageVersion version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public string DownloadPath => Path.Combine(KnownFolders.PicturesLibrary.Path, "HENG");

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }
        private void SetSettingsValue(string name, object value)
        {
            LocalSettings.Values[name] = value;
        }
    }
}
