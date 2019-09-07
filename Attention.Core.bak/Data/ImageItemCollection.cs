using PixabaySharp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Attention.Core.Data
{
    public class ImageItemCollection : ObservableCollection<ImageItem>
    {
        public ImageItemCollection(IEnumerable<ImageItem> collection) : base(collection)
        {
        }
    }
}
