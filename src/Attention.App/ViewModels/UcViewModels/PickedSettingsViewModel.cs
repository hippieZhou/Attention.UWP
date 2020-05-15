using Attention.Core.Events;
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

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(() =>
                    {
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
