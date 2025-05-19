using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfApp1.Utilities
{
    /// <summary>
    /// Convierte un valor de tipo string en un valor de <see cref="Visibility"/> para su uso en enlaces de datos en WPF.
    /// Si la cadena es nula o vacía, devuelve <see cref="Visibility.Visible"/>; en caso contrario, <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convierte un valor de tipo string en un valor de <see cref="Visibility"/>.
        /// </summary>
        /// <param name="value">El valor a convertir (esperado: string).</param>
        /// <param name="targetType">El tipo de destino de la conversión.</param>
        /// <param name="parameter">Parámetro opcional de conversión.</param>
        /// <param name="culture">Información de cultura.</param>
        /// <returns><see cref="Visibility.Visible"/> si la cadena es nula o vacía; <see cref="Visibility.Collapsed"/> en caso contrario.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convierte un valor de <see cref="Visibility"/> de vuelta a string.
        /// </summary>
        /// <param name="value">El valor a convertir (esperado: <see cref="Visibility"/>).</param>
        /// <param name="targetType">El tipo de destino de la conversión.</param>
        /// <param name="parameter">Parámetro opcional de conversión.</param>
        /// <param name="culture">Información de cultura.</param>
        /// <returns>Cadena vacía si <see cref="Visibility.Visible"/>; null en caso contrario.</returns>
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
