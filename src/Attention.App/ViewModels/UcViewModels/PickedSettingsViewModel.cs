using Attention.App.Events;
using Microsoft.Toolkit.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Windows.AppModel;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.ApplicationModel.Email;
using Windows.System;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedSettingsViewModel : PickedPaneViewModel
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IEventAggregator _eventAggregator;

        public PickedSettingsViewModel(
            IResourceLoader resourceLoader,
            IEventAggregator eventAggregator) : base(PaneTypes.Settings, resourceLoader.GetString("picked_Settings"))
        {
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }

        private string _dependencies;
        public string Dependencies
        {
            get { return _dependencies; }
            set { SetProperty(ref _dependencies, value); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() =>
                    {
                        Dependencies = @"
## Toolkit

- [Windows UI Library](https://github.com/microsoft/microsoft-ui-xaml)
- [WindowsCommunityToolkit](https://github.com/windows-toolkit/WindowsCommunityToolkit)
- [WindowsCompositionSamples](https://github.com/microsoft/WindowsCompositionSamples)
- [Lottie-Windows](https://github.com/windows-toolkit/Lottie-Windows)
- [Microsoft.Xaml.Behaviors.Uwp.Managed](https://github.com/Microsoft/XamlBehaviors)
- [Prism.Unity(6.3.0)](https://github.com/PrismLibrary/Prism)
- [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [unsplasharp](https://github.com/rootasjey/unsplasharp)
- [Serilog](https://github.com/serilog/serilog)

## Helpers

- [JuniperPhoton](https://github.com/JuniperPhoton)
- [JustinXinLiu](https://github.com/JustinXinLiu)
- [Niels Laute](https://github.com/niels9001)
- [DinoChan](https://github.com/DinoChan)
- [hhchaos](https://github.com/HHChaos)
- [cnbluefire](https://github.com/cnbluefire)
- [h82258652](https://github.com/h82258652)
";
                    });
                }
                return _loadCommand;
            }
        }

        private ICommand _changeThemeCommand;
        public ICommand ChangeThemeCommand
        {
            get
            {
                if (_changeThemeCommand == null)
                {
                    _changeThemeCommand = new DelegateCommand<object>(theme =>
                    {
                        if (theme is ElementTheme current)
                        {
                            App.Settings.Theme = current;
                        }
                    });
                }
                return _changeThemeCommand;
            }
        }

        private ICommand _changeLanguageCommand;
        public ICommand ChangeLanguageCommand
        {
            get
            {
                if (_changeLanguageCommand == null)
                {
                    _changeLanguageCommand = new DelegateCommand<string>(language =>
                    {
                        App.Settings.Language = language;
                        _eventAggregator.GetEvent<NotificationEvent>().Publish(_resourceLoader.GetString("settings_Language_Notification"));
                    });
                }
                return _changeLanguageCommand;
            }
        }

        private ICommand _contractCommand;
        public ICommand ContractCommand
        {
            get
            {
                if (_contractCommand == null)
                {
                    _contractCommand = new DelegateCommand<string>(async contract =>
                    {
                        if (contract.IsEmail())
                        {
                            EmailMessage email = new EmailMessage();
                            email.To.Add(new EmailRecipient("From UWP Client"));
                            email.Subject = $"FeedBack For UWP Client";
                            email.Body = $"version:{App.Settings.Version}";
                            //email.Attachments.Add(new EmailAttachment(fileInfo.Name, RandomAccessStreamReference.CreateFromFile(logFile)));
                            await EmailManager.ShowComposeNewEmailAsync(email);
                        }
                        else if (Regex.IsMatch(contract, @"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$"))
                        {
                            await Launcher.LaunchUriAsync(new Uri(contract));
                        }
                    });
                }
                return _contractCommand;
            }
        }
    }
}
