using Prism.Windows.Mvvm;

namespace Attention.App.ViewModels.UcViewModels
{
    public class PickedPaneViewModel: ViewModelBase
    {
		private string _title;
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value); }
		}
	}
}
