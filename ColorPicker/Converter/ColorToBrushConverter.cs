using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPicker.Converter
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
                return value;
            if (value == null || value == DependencyProperty.UnsetValue
                || value.GetType() != typeof(Color))
                return Brushes.Transparent;
            Color color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
