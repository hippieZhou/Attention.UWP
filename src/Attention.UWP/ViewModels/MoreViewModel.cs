using GalaSoft.MvvmLight.Command;
using MetroLog;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace Attention.UWP.ViewModels
{
    public class MoreViewModel : BaseViewModel
    {
        private readonly ILogger _logger;
        public MoreViewModel(ILogManager logManager)
        {
            _logger = logManager.GetLogger<MoreViewModel>();
        }

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
                            await SendEmailAsync(args);
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

        private async Task SendEmailAsync(string sender)
        {
           var appSummary =  $@"
------------------------------------------------------------------------
IsFirstRun:{SystemInformation.IsFirstRun};
ApplicationName:{SystemInformation.ApplicationName};
ApplicationVersion:{App.Settings.Version};
Culture:{SystemInformation.Culture};
OperatingSystem:{SystemInformation.OperatingSystem};
OperatingSystemArchitecture:{SystemInformation.OperatingSystemArchitecture};
OperatingSystemVersion:{SystemInformation.OperatingSystemVersion};
DeviceFamily:{SystemInformation.DeviceFamily};
DeviceModel:{SystemInformation.DeviceModel};
DeviceManufacturer:{SystemInformation.DeviceManufacturer};
AvailableMemory:{SystemInformation.AvailableMemory};
------------------------------------------------------------------------";

            _logger.Info(appSummary);

            //https://talkitbr.com/2015/06/11/adicionando-logs-em-universal-apps/
            Stream compressedLogsStream = await ViewModelLocator.Current.LogManager.GetCompressedLogs();
            FileInfo fileInfo = new FileInfo(Path.Combine(ApplicationData.Current.LocalFolder.Path,
                $"logs_{DateTime.UtcNow.ToString("yyyyMMdd_hhmmss")}.zip"));

            Debug.WriteLine("My compressed logs file will be located at: " + fileInfo.FullName);

            using (FileStream fileStream = fileInfo.Create())
            {
                await compressedLogsStream.CopyToAsync(fileStream);
            }

            var logFile = await StorageFile.GetFileFromPathAsync(fileInfo.FullName);
            EmailMessage email = new EmailMessage();
            email.To.Add(new EmailRecipient(sender));
            email.Subject = $"FeedBack For {App.Settings.Name}";
            email.Body = $"version:{App.Settings.Version}";
            email.Attachments.Add(new EmailAttachment(fileInfo.Name, RandomAccessStreamReference.CreateFromFile(logFile)));
            await EmailManager.ShowComposeNewEmailAsync(email);
        }
    }
}
