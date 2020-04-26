using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels
{
	public class SettingsPageViewModel: ViewModelBase
    {
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
					});
				}
				return _changeLanguageCommand;
			}
		}
	}
}
