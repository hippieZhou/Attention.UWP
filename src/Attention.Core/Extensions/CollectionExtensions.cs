using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Attention.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list) => new ObservableCollection<T>(list);
    }
}
