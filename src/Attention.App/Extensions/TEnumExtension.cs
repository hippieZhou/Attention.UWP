using System;
using System.Reflection;

namespace Attention.App.Extensions
{
    public static class TEnumExtension
    {
        public static TEnum GetEnum<TEnum>(this string text) where TEnum : struct
        {
            if (!typeof(TEnum).GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("Generic parameter 'TEnum' must be an enum.");
            }
            return (TEnum)Enum.Parse(typeof(TEnum), text);
        }
    }
}
