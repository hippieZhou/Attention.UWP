using Prism.Windows.Mvvm;
using System;

namespace Attention.App.ViewModels.UcViewModels
{
	[Flags]
	public enum PaneTypes : byte
	{
		None = 1 << 0,
		Search = 1 << 1,
		Download = 1 << 2,
		Settings = 1 << 3
	}

    public abstract class PickedPaneViewModel: ViewModelBase
    {
		private PaneTypes _paneType;
		public PaneTypes PaneType
		{
			get { return _paneType; }
			private set { SetProperty(ref _paneType, value); }
		}

		private string _title;
		public string Title
		{
			get { return _title; }
			private set { SetProperty(ref _title, value); }
		}

		public PickedPaneViewModel(PaneTypes paneType, string title)
		{
			PaneType = paneType;
			Title = title;
		}
	}
}
