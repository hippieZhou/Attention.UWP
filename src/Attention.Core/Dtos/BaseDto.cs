using System;
using System.Linq;
using System.Reflection;
using Windows.UI;

namespace Attention.Core.Dtos
{
    public abstract class BaseDto
    {
        protected readonly static Random random = new Random(DateTime.Now.Second);
        protected readonly static Color[] colors = typeof(Colors).GetRuntimeProperties().Select(x => (Color)x.GetValue(null)).ToArray();
    }
}
