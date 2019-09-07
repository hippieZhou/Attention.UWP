using Attention.Extensions;
using Attention.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Microsoft.Toolkit.Extensions;
using Windows.ApplicationModel.Email;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Attention.ViewModels
{
    public class MoreViewModel : BaseViewModel
    {
        public FilterViewModel FilterViewModel { get; private set; } = new FilterViewModel();
        public SettingViewMode SettingViewMode { get; private set; } = new SettingViewMode();
        public ThanksViewModel ThanksViewModel { get; private set; } = new ThanksViewModel();
        public AboutViewModel AboutViewModel { get; private set; } = new AboutViewModel();
        public MoreViewModel(string header) : base(header) { }
    }

    public class BaseMoreViewModel : ViewModelBase
    {
        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                        OnLoaded();
                    });
                }
                return _loadedCommand;
            }
        }

        protected virtual void OnLoaded() { }
    }

    public class FilterViewModel : BaseMoreViewModel
    {
        public Filters Orders { get; private set; } = new Filters();

        public Filters Orientations { get; private set; } = new Filters();

        public Filters ImageTypes { get; private set; } = new Filters();

        public Filters Categories { get; private set; } = new Filters();

        protected override void OnLoaded()
        {
            var appSettings = ViewModelLocator.Current.AppSettings;
            var (orders, orientations, imageTypes, categories) = ViewModelLocator.Current.Pixabay.GetEnumFilters();

            Orders.Clear();

            Orientations.Clear();

            ImageTypes.Clear();

            Categories.Clear();

            void Refresh(IEnumerable<FilterItem> items, FilterItem checkedItem, Filters filter)
            {
                filter.Clear();

                var enumerator = items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Checked = enumerator.Current.Name == checkedItem?.Name;
                    filter.Add(enumerator.Current);
                }
            }

            Refresh(orders, appSettings.Filters.Order, Orders);
            Refresh(orientations, appSettings.Filters.Orientation, Orientations);
            Refresh(imageTypes, appSettings.Filters.ImageType, ImageTypes);
            Refresh(categories, appSettings.Filters.Category, Categories);
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(async () =>
                    {
                         var appSettings = ViewModelLocator.Current.AppSettings;
                        await appSettings.UpdateFiletersAsync(
                            Orders.FirstOrDefault(p => p.Checked),
                            Orientations.FirstOrDefault(p => p.Checked),
                            ImageTypes.FirstOrDefault(p => p.Checked),
                            Categories.FirstOrDefault(p => p.Checked));
                        await Task.CompletedTask;

                        Messenger.Default.Send(new NotificationMessage(this, "filer_notification".GetLocalized()), NotificationToken.ToastToken);
                        ViewModelLocator.Current.Shell.PhotoGridViewModel.RefreshCommand.Execute(null);
                    });
                }
                return _saveCommand;
            }
        }
    }

    public class SettingViewMode : BaseMoreViewModel
    {
        private ElementTheme _theme;
        public ElementTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        private string _language;
        public string Language
        {
            get { return _language; }
            set { Set(ref _language, value); }
        }

        private bool _liveTitle;
        public bool LiveTitle
        {
            get { return _liveTitle; }
            set { Set(ref _liveTitle, value); }
        }

        protected override void OnLoaded()
        {
            var appSettings = ViewModelLocator.Current.AppSettings;
            Theme = appSettings.AppTheme;
            Language = appSettings.AppLanguage;
            LiveTitle = appSettings.AppLiveTitleIsOn;
        }

        private ICommand _themeCommand;
        public ICommand ThemeCommand
        {
            get
            {
                if (_themeCommand == null)
                {
                    _themeCommand = new RelayCommand<ElementTheme>(async theme =>
                    {
                        Theme = theme;
                        await ViewModelLocator.Current.AppSettings.UpdateThemeAsync(Theme);
                    });
                }
                return _themeCommand;
            }
        }

        private ICommand _languageCommand;
        public ICommand LanguageCommand
        {
            get
            {
                if (_languageCommand == null)
                {
                    _languageCommand = new RelayCommand<string>(async language =>
                    {
                        Language = language;
                        await ViewModelLocator.Current.AppSettings.UpdateLanguageAsync(Language);
                        Messenger.Default.Send(new NotificationMessage(this, "theme_notification".GetLocalized()), NotificationToken.ToastToken);
                    });
                }
                return _languageCommand;
            }
        }

        private ICommand _liveTitleCommand;
        public ICommand LiveTitleCommand
        {
            get
            {
                if (_liveTitleCommand == null)
                {
                    _liveTitleCommand = new RelayCommand<RoutedEventArgs>(args =>
                    {
                        if (args.OriginalSource is ToggleSwitch ts)
                        {
                            LiveTitle = ts.IsOn;
                            ViewModelLocator.Current.AppSettings.UpdateLiveTitle(LiveTitle);
                        }
                    });
                }
                return _liveTitleCommand;
            }
        }
    }

    public class ThanksViewModel : BaseMoreViewModel
    {
        private string _markdown;
        public string Markdown
        {
            get { return _markdown; }
            set { Set(ref _markdown, value); }
        }

        protected override void OnLoaded()
        {
            this.Markdown = @"
## Toolkit

- [WindowsCommunityToolkit](https://github.com/windows-toolkit/WindowsCommunityToolkit)
- [WindowsCompositionSamples](https://github.com/microsoft/WindowsCompositionSamples)
- [Lottie-Windows](https://github.com/windows-toolkit/Lottie-Windows)
- [Microsoft.Xaml.Behaviors.Uwp.Managed](https://github.com/Microsoft/XamlBehaviors)
- [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/Extensions)
- [MvvmLightLibs](https://github.com/lbugnion/mvvmlight)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [PixabaySharp](https://github.com/ThomasPe/PixabaySharp)
- [NLog](https://github.com/NLog/NLog)
- [FreeLogo Design](https://www.freelogodesign.org/)
- [Color Hunt](https://colorhunt.co/)

## Helpers

- [JuniperPhoton](https://github.com/JuniperPhoton)
- [JustinXinLiu](https://github.com/JustinXinLiu)
- [Niels Laute](https://github.com/niels9001)
- [DinoChan](https://github.com/DinoChan)
- [hhchaos](https://github.com/HHChaos)
- [cnbluefire](https://github.com/cnbluefire)
- [h82258652](https://github.com/h82258652)
";
        }
    }

    public class AboutViewModel : BaseMoreViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set { Set(ref _version, value); }
        }

        private ICommand _feedbackCommand;
        public ICommand FeedbackCommand
        {
            get
            {
                if (_feedbackCommand == null)
                {
                    _feedbackCommand = new RelayCommand<string>(async args =>
                    {
                        if (args.IsEmail())
                        {
                            EmailMessage email = new EmailMessage();
                            email.To.Add(new EmailRecipient(args));
                            email.Subject = "FeedBack For Attention";
                            email.Body = "Hello world";
                            await EmailManager.ShowComposeNewEmailAsync(email);
                        }
                        else if (Regex.IsMatch(args, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                        {
                            await Launcher.LaunchUriAsync(new Uri(args));
                        }
                    });
                }
                return _feedbackCommand;
            }
        }

        protected override void OnLoaded()
        {
            Name = "AppDisplayName".GetLocalized();

            PackageVersion packageVersion = Package.Current.Id.Version;
            Version = $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";

            base.OnLoaded();
        }
    }
}
