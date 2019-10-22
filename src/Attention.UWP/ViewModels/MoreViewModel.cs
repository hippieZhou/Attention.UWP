using Attention.UWP.Extensions;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Email;
using Windows.System;
using Windows.UI.Xaml;

namespace Attention.UWP.ViewModels
{
    public class MoreViewModel : BaseViewModel
    {
        public IEnumerable<ElementTheme> Themes => Enum.GetValues(typeof(ElementTheme)).Cast<ElementTheme>();

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

        public string AppName => "AppDisplayName".GetLocalized();
        public string Version 
        {
            get
            {
                PackageVersion packageVersion = Package.Current.Id.Version;
                return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}";
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
