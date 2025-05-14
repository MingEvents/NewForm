using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1.Utilities
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible ? string.Empty : null;
            }
            return null;
            // Si no se necesita la conversión inversa, puedes lanzar una excepción o devolver un valor predeterminado.
            //   throw new NotImplementedException();
        }
    }
}
