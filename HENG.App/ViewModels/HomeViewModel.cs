using GalaSoft.MvvmLight;
using System;
using Windows.UI.Xaml.Controls;

namespace HENG.App.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public PhotoGridViewModel PhotoGridViewModel { get; private set; } = new PhotoGridViewModel();

        public void Initialize(GridView masterView, Grid detailView)
        {
            if (masterView == null)
                throw new Exception("Master View Not Find");
            if (detailView == null)
                throw new Exception("Detail View Not Find");
            PhotoGridViewModel.Initialize(masterView, detailView);
        }
    }
}
