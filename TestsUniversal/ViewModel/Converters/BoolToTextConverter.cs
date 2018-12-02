using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TestsUniversal.ViewModel.Converters
{
    public sealed class BoolToTextConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture) =>
            ((Boolean)value) ? TrueValue : FalseValue;

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) =>
            DependencyProperty.UnsetValue;

        public String TrueValue { get; set; }
        public String FalseValue { get; set; }
    }
}
