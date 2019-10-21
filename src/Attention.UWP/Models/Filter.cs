using GalaSoft.MvvmLight;
using PixabaySharp.Enums;

namespace Attention.UWP.Models
{
    public class Filter : ObservableObject
    {
        private Order _order;
        public Order Order
        {
            get { return _order; }
            set { Set(ref _order, value); }
        }

        private Orientation _orientation;
        public Orientation Orientation
        {
            get { return _orientation; }
            set { Set(ref _orientation, value); }
        }

        private ImageType _imageType;
        public ImageType ImageType
        {
            get { return _imageType; }
            set { Set(ref _imageType, value); }
        }

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { Set(ref _category, value); }
        }

        private Language _language;
        public Language Language
        {
            get { return _language; }
            set { Set(ref _language, value); }
        }

        private string _query;
        public string Query
        {
            get { return _query; }
            set { Set(ref _query, value); }
        }
    }
}
