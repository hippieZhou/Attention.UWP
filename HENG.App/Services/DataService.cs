using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;

namespace HENG.App.Services
{
    public class DataService
    {
        public static IEnumerable<Color> SystemColors => typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null));
    }
}
