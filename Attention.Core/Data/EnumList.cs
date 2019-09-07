using PixabaySharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Attention.Core.Data
{
    public class EnumList<T> : List<T> where T : Enum
    {
        public EnumList()
        {
            IEnumerable<T> items = from p in Enum.GetValues(typeof(T)).Cast<T>() select p;
            AddRange(items);
        }
    }

    public class CategoryList : EnumList<Category>
    {

    }

    public class ImageTypeList : EnumList<ImageType>
    {

    }

    public class OrderList : EnumList<Order>
    {

    }

    public class OrientationList : EnumList<Orientation>
    {

    }
}
