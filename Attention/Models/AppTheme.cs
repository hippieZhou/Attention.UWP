using GalaSoft.MvvmLight;
using Windows.UI.Xaml;

namespace Attention.Models
{
    public class AppTheme : Defined
    {
        public ElementTheme Theme { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Theme.ToString();
        }
    }
}
