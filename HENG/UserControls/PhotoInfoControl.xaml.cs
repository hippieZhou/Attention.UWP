using GalaSoft.MvvmLight.Command;
using Microsoft.UI.Xaml.Controls;
using PixabaySharp.Models;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HENG.UserControls
{
    public sealed partial class PhotoInfoControl : UserControl
    {
        public PhotoInfoControl()
        {
            this.InitializeComponent();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize == e.PreviousSize) return;

            var rootGrid = sender as Grid;
            rootGrid.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, rootGrid.ActualWidth, rootGrid.ActualHeight)
            };
        }

        public ImageItem StoredItem
        {
            get { return (ImageItem)GetValue(StoredItemProperty); }
            set { SetValue(StoredItemProperty, value); }
        }
        // Using a DependencyProperty as the backing store for StoredItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoredItemProperty =
            DependencyProperty.Register("StoredItem", typeof(ImageItem), typeof(PhotoInfoControl), new PropertyMetadata(null));

        private ICommand _showTipsCommand;
        public ICommand ShowTipsCommand
        {
            get
            {
                if (_showTipsCommand == null)
                {
                    _showTipsCommand = new RelayCommand<ImageItem>(item =>
                    {
                        if (FindName("teachingTip") is TeachingTip tip)
                        {
                            tip.IsOpen = !tip.IsOpen;
                        }
                    });
                }
                return _showTipsCommand;
            }
        }


        public ICommand BrowseCommand
        {
            get { return (ICommand)GetValue(BrowseCommandProperty); }
            set { SetValue(BrowseCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BrowseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrowseCommandProperty =
            DependencyProperty.Register("BrowseCommand", typeof(ICommand), typeof(PhotoInfoControl), new PropertyMetadata(null));

        public ICommand DownloadCommand
        {
            get { return (ICommand)GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DownloadCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DownloadCommandProperty =
            DependencyProperty.Register("DownloadCommand", typeof(ICommand), typeof(PhotoInfoControl), new PropertyMetadata(null));

        public ICommand BackCommand
        {
            get { return (ICommand)GetValue(BackCommandProperty); }
            set { SetValue(BackCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackCommandProperty =
            DependencyProperty.Register("BackCommand", typeof(ICommand), typeof(PhotoInfoControl), new PropertyMetadata(null));
    }
}
