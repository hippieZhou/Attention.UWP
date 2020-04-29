using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Attention.App.Common
{
    public class ImageLoader
    {
        public static string GetSource(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject obj, string value)
        {
            obj.SetValue(SourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(ImageLoader), new PropertyMetadata(string.Empty, (d, e) =>
             {
                 if (e.NewValue is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
                 {
                     Uri imageUri = new Uri(imagePath, UriKind.Absolute);
                     BitmapImage imageBitmap = new BitmapImage(imageUri);

                     if (d is ImageEx imageEx)
                     {
                         imageEx.Source = imageBitmap;
                     }
                     else if (d is Image image)
                     {
                         image.Source = imageBitmap;
                     }
                 }
                 else
                 {
                     throw new ArgumentNullException("ImagePath is Null Or WhiteSpace.");
                 }
             }));


    }
}
