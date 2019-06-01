using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HENG.Models
{
    public class Parameters: ObservableObject
    {
        [Description("A URL encoded search term. If omitted, all images are returned. This value may not exceed 100 characters. ")]
        private string _query;
        public string Query
        {
            get { return _query; }
            set { Set(ref _query, value); }
        }

        [Description("Language code of the language to be searched in. ")]
        private ObservableCollection<Lang> _langs;
        public ObservableCollection<Lang> Langs
        {
            get { return _langs; }
            set { Set(ref _langs, value); }
        }

        [Description("Filter results by image type. ")]
        private ObservableCollection<Imagetype> _types;
        public ObservableCollection<Imagetype> Types
        {
            get { return _types; }
            set { Set(ref _types, value); }
        }

        [Description("Whether an image is wider than it is tall, or taller than it is wide. ")]
        private ObservableCollection<Orientation> _orientations;
        public ObservableCollection<Orientation> Orientations
        {
            get { return _orientations; }
            set { Set(ref _orientations, value); }
        }

        [Description("Filter results by category. ")]
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { Set(ref _categories, value); }
        }

        [Description("Filter images by color properties. A comma separated list of values may be used to select multiple properties. ")]
        private ObservableCollection<Color> _colors;
        public ObservableCollection<Color> Colors
        {
            get { return Colors; }
            set { Set(ref _colors, value); }
        }

        [Description("Select images that have received an Editor's Choice award. ")]
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { Set(ref _orders, value); }
        }

        [Description("A flag indicating that only images suitable for all ages should be returned. ")]
        private bool _editorsChoice;
        public bool EditorsChoice
        {
            get { return _editorsChoice; }
            set { Set(ref _editorsChoice, value); }
        }

        [Description("A flag indicating that only images suitable for all ages should be returned. ")]
        private bool _safesearch;
        public bool Safesearch
        {
            get { return _safesearch; }
            set { Set(ref _safesearch, value); }
        }

        [Description("How the results should be ordered. ")]
        private bool _order;
        public bool Order
        {
            get { return _order; }
            set { Set(ref _order, value); }
        }
    }

    public enum Lang { en, cs, da, de, es, fr, id, it, hu, nl, no, pl, pt, ro, sk, fi, sv, tr, vi, th, bg, ru, el, ja, ko, zh }
    public enum Imagetype { photo, illustration, vector }
    public enum Orientation { horizontal, vertical }
    public enum Category { fashion, nature, backgrounds, science, education, people, feelings, religion, health, places, animals, industry, food, computer, sports, transportation, travel, buildings, business, music }
    public enum Color { grayscale, transparent, red, orange, yellow, green, turquoise, blue, lilac, pink, white, gray, black, brown }
    public enum Order { popular, latest }
}
