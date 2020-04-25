using Prism.Commands;
using Prism.Windows.Mvvm;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Attention.App.ViewModels
{
	public class SettingsPageViewModel: ViewModelBase
    {
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
