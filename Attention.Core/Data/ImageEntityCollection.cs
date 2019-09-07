using Attention.Core.Models;
using PixabaySharp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Attention.Core.Data
{
    public class ImageEntityCollection : ObservableCollection<ImageEntity>
    {
        public ImageEntityCollection(IEnumerable<ImageEntity> collection) : base(collection)
        {
        }
    }
}
