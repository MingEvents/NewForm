using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Utilities
{
    /// <summary>
    /// Proporciona una propiedad adjunta para permitir el enlace de la contraseña de un <see cref="PasswordBox"/> en WPF.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Propiedad adjunta que permite enlazar la contraseña de un <see cref="PasswordBox"/>.
        /// </summary>
        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindablePassword",
                typeof(string),
                typeof(PasswordHelper),
                new PropertyMetadata(string.Empty, OnBindablePasswordChanged));

        private static bool _isUpdating = false;

        /// <summary>
        /// Obtiene el valor de la propiedad adjunta <see cref="BindablePasswordProperty"/> de un objeto.
        /// </summary>
        /// <param name="obj">El objeto del que se obtiene la contraseña enlazada.</param>
        /// <returns>La contraseña enlazada.</returns>
        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        /// <summary>
        /// Establece el valor de la propiedad adjunta <see cref="BindablePasswordProperty"/> en un objeto.
        /// </summary>
        /// <param name="obj">El objeto en el que se establece la contraseña enlazada.</param>
        /// <param name="value">La contraseña a establecer.</param>
        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        /// <summary>
        /// Se ejecuta cuando cambia el valor de la propiedad <see cref="BindablePasswordProperty"/>.
        /// Sincroniza el valor con el <see cref="PasswordBox"/> correspondiente.
        /// </summary>
        /// <param name="d">El objeto dependiente (debe ser un <see cref="PasswordBox"/>).</param>
        /// <param name="e">Argumentos del cambio de propiedad.</param>
        private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (_isUpdating)
                return;

            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                if (passwordBox.Password != (string)e.NewValue)
                    passwordBox.Password = (string)e.NewValue;
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        /// <summary>
        /// Manejador del evento <see cref="PasswordBox.PasswordChanged"/> para actualizar la propiedad enlazada.
        /// </summary>
        /// <param name="sender">El <see cref="PasswordBox"/> que disparó el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isUpdating = true;
            var passwordBox = sender as PasswordBox;
            SetBindablePassword(passwordBox, passwordBox.Password);
            _isUpdating = false;
        }
    }
}
