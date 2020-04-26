﻿using Attention.App.Framework;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Attention.App.Converters
{
    public class ObjectEqualToCheckedConverter:IValueConverter
    {
        private readonly ILoggerFacade _logger;

        public ObjectEqualToCheckedConverter()
        {
            _logger = EnginContext.Current.Resolve<ILoggerFacade>() ?? throw new ArgumentNullException(nameof(ILoggerFacade));
        }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(value?.GetType());
            var target = tc.ConvertFromString(null, CultureInfo.InvariantCulture, parameter?.ToString());
            _logger.Log($"{value}:{target} ======== {Equals(value, target)}", Category.Debug, Priority.High);
            return Equals(value, target);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}