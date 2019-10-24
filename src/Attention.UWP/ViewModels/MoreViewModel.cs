using Attention.UWP.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Extensions;
using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.ApplicationModel.Email;
using Windows.System;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class MoreViewModel : BaseViewModel
    {
        public string Markdown
        {
            get
            {
                return @"
## Toolkit

- [WindowsCommunityToolkit](https://github.com/windows-toolkit/WindowsCommunityToolkit)
- [WindowsCompositionSamples](https://github.com/microsoft/WindowsCompositionSamples)
- [Lottie-Windows](https://github.com/windows-toolkit/Lottie-Windows)
- [Microsoft.Xaml.Behaviors.Uwp.Managed](https://github.com/Microsoft/XamlBehaviors)
- [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/Extensions)
- [MvvmLightLibs](https://github.com/lbugnion/mvvmlight)
- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- [PixabaySharp](https://github.com/ThomasPe/PixabaySharp)
- [sqlite-net-pcl](https://github.com/praeclarum/sqlite-net)

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

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                if (_loadedCommand == null)
                {
                    _loadedCommand = new RelayCommand(() =>
                    {
                    });
                }
                return _loadedCommand;
            }
        }

        private ICommand _openSavingFolderCommand;
        public ICommand OpenSavingFolderCommand
        {
            get
            {
                if (_openSavingFolderCommand == null)
                {
                    _openSavingFolderCommand = new RelayCommand(async () =>
                    {
                        var folder = await App.Settings.GetSavingFolderAsync();
                        if (folder != null)
                        {
                            await Launcher.LaunchFolderAsync(folder);
                        }
                    });
                }
                return _openSavingFolderCommand;
            }
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
    }
}
