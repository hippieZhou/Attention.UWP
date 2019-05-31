using System.Collections.Generic;
using System.ComponentModel;

namespace HENG.Core.Models
{
    public class Parameters
    {
        [Description("A URL encoded search term. If omitted, all images are returned. This value may not exceed 100 characters. ")]
        public string Query { get; set; }
        [Description("Language code of the language to be searched in. ")]
        public IEnumerable<Lang> Langs { get; set; }
        [Description("Filter results by image type. ")]
        public IEnumerable<Imagetype> Imagetypes { get; set; }
        [Description("Whether an image is wider than it is tall, or taller than it is wide. ")]
        public IEnumerable<Orientation> Orientations { get; set; }
        [Description("Filter results by category. ")]
        public IEnumerable<Category> Categories { get; set; }
        [Description("Filter images by color properties. A comma separated list of values may be used to select multiple properties. ")]
        public IEnumerable<Color> Colors { get; set; }
        [Description("Select images that have received an Editor's Choice award. ")]
        public IEnumerable<Order> Orders { get; set; }
        [Description("A flag indicating that only images suitable for all ages should be returned. ")]
        public bool EditorsChoice { get; set; }
        [Description("A flag indicating that only images suitable for all ages should be returned. ")]
        public bool Safesearch { get; set; }
        [Description("How the results should be ordered. ")]
        public bool Order { get; set; }
    }

    public enum Lang { en, cs, da, de, es, fr, id, it, hu, nl, no, pl, pt, ro, sk, fi, sv, tr, vi, th, bg, ru, el, ja, ko, zh }
    public enum Imagetype { all, photo, illustration, vector }
    public enum Orientation { all, horizontal, vertical }
    public enum Category { fashion, nature, backgrounds, science, education, people, feelings, religion, health, places, animals, industry, food, computer, sports, transportation, travel, buildings, business, music }
    public enum Color { grayscale, transparent, red, orange, yellow, green, turquoise, blue, lilac, pink, white, gray, black, brown }
    public enum Order { popular, latest }
}
