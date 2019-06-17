using HENG.Helpers;
using System;
using Windows.UI.Xaml.Data;

namespace HENG.Converters
{
    public class EnumToStringConverter:IValueConverter
    {
        public Type EnumType { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!Enum.IsDefined(EnumType, value))
            {
                throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum".GetLocalized());
            }

            return Enum.Parse(EnumType, value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string enumString)
            {
                return Enum.Parse(EnumType, enumString);
            }

            throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName".GetLocalized());
        }
    }
}
