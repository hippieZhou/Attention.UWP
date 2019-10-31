using Newtonsoft.Json;
using PixabaySharp.Models;
using SQLite;

namespace Attention.UWP.Models.Core
{
    public class Download : Entity
    {
        private ImageItem _model;
        [Ignore]
        public ImageItem Model
        {
            get
            {
                if (_model == null)
                {
                    _model = JsonConvert.DeserializeObject<ImageItem>(Json);
                }
                return _model;
            }
            set { _model = value; }
        }


        public string FileName { get; set; }
        public string ImageURL { get; set; }
        public string Json { get; set; }
    }
}
