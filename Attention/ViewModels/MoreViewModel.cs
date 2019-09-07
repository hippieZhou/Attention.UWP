using Attention.Extensions;
using Attention.Models;
using Attention.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
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
        private Filter _orders;
        public Filter Orders
        {
            get { return _orders ?? (_orders = new Filter()); }
            set { Set(ref _orders, value); }
        }

        private Filter _orientations;
        public Filter Orientations
        {
            get { return _orientations ?? (_orientations = new Filter()); }
            set { Set(ref _orientations, value); }
        }

        private Filter _imageTypes;
        public Filter ImageTypes
        {
            get { return _imageTypes ?? (_imageTypes = new Filter()); }
            set { Set(ref _imageTypes, value); }
        }

        private Filter _categories;
        public Filter Categories
        {
            get { return _categories ?? (_categories = new Filter()); }
            set { Set(ref _categories, value); }
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
                        //var appSettings = ViewModelLocator.Current.GetRequiredService<AppSettingService>();
                        //await appSettings.UpdateFiletersAsync(
                        //    Orders.FirstOrDefault(p => p.Checked),
                        //    Orientations.FirstOrDefault(p => p.Checked),
                        //    ImageTypes.FirstOrDefault(p => p.Checked),
                        //    Categories.FirstOrDefault(p => p.Checked));
                        await Task.CompletedTask;

                        Messenger.Default.Send(new NotificationMessage(this, "filer_notification".GetLocalized()), NotificationToken.ToastToken);
                        ViewModelLocator.Current.Shell.PhotoGridViewModel.RefreshCommand.Execute(null);
                    });
                }
                return _saveCommand;
            }
        }

        protected override void OnLoaded()
        {
            //var service = ViewModelLocator.Current.GetRequiredService<PixabayService>();
            //var (orders, orientations, imageTypes, categories) = service.GetEnumFilters();

            //void Refresh(IEnumerable<FilterItem> items, FilterItem checkedItem, Filter filter)
            //{
            //    filter.Clear();

            //    var enumerator = items.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        enumerator.Current.Checked = enumerator.Current.Name == checkedItem?.Name;
            //        filter.Add(enumerator.Current);
            //    }
            //}

            //var appSettings = ViewModelLocator.Current.GetRequiredService<AppSettingService>();

            //Refresh(orders, appSettings.Filter.Order, Orders);
            //Refresh(orientations, appSettings.Filter.Orientation, Orientations);
            //Refresh(imageTypes, appSettings.Filter.ImageType, ImageTypes);
            //Refresh(categories, appSettings.Filter.Category, Categories);
        }
    }

    public class SettingViewMode : BaseMoreViewModel
    {
        private ObservableCollection<AppTheme> _themes;
        public ObservableCollection<AppTheme> Themes
        {
            get { return _themes ?? (_themes = new ObservableCollection<AppTheme>()); }
            set { _themes = value; }
        }

        private ObservableCollection<AppLanguage> _languages;
        public ObservableCollection<AppLanguage> Languages
        {
            get { return _languages ?? (_languages = new ObservableCollection<AppLanguage>()); }
            set { _languages = value; }
        }

        private bool _liveTitleIsOn;
        public bool LiveTitleIsOn
        {
            get { return _liveTitleIsOn; }
            set { Set(ref _liveTitleIsOn, value); }
        }

        private ICommand _changedThemeCommand;
        public ICommand ChangedThemeCommand
        {
            get
            {
                if (_changedThemeCommand == null)
                {
                    _changedThemeCommand = new RelayCommand<AppTheme>(async theme =>
                    {
                        var appSettings = ViewModelLocator.Current.GetService<AppSettingService>();
                        await appSettings.UpdateThemeAsync(theme);
                    });
                }
                return _changedThemeCommand;
            }
        }

        private ICommand _changedLanguageCommand;
        public ICommand ChangedLanguageCommand
        {
            get
            {
                if (_changedLanguageCommand == null)
                {
                    _changedLanguageCommand = new RelayCommand<AppLanguage>(async language =>
                    {
                        var appSettings = ViewModelLocator.Current.GetService<AppSettingService>();
                        await appSettings.UpdateLanguageAsync(language);
                        
                        Messenger.Default.Send(new NotificationMessage(this, "theme_notification".GetLocalized()), NotificationToken.ToastToken);
                    });
                }
                return _changedLanguageCommand;
            }
        }

        private ICommand _switchLiveTitleCommand;
        public ICommand SwitchLiveTitleCommand
        {
            get
            {
                if (_switchLiveTitleCommand == null)
                {
                    _switchLiveTitleCommand = new RelayCommand<RoutedEventArgs>(args =>
                    {
                        if (args.OriginalSource is ToggleSwitch ts)
                        {
                            LiveTitleIsOn = ts.IsOn;

                            //var appSettings = ViewModelLocator.Current.GetRequiredService<AppSettingService>();
                            //appSettings.UpdateLiveTitleState(LiveTitleIsOn);
                        }
                    });
                }
                return _switchLiveTitleCommand; }
        }

        protected override void OnLoaded()
        {
            //var appSettings = ViewModelLocator.Current.GetRequiredService<AppSettingService>();

            //Themes.Clear();
            //var themes = appSettings.GetThemes();
            //foreach (var theme in themes)
            //{
            //    Themes.Add(theme);
            //}


            //Languages.Clear();
            //var languages = appSettings.GetLanguages();
            //foreach (var language in languages)
            //{
            //    Languages.Add(language);
            //}

            //LiveTitleIsOn = appSettings.GetLiveTitleState();
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
