using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Utilities
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindablePassword",
                typeof(string),
                typeof(PasswordHelper),
                new PropertyMetadata(string.Empty, OnBindablePasswordChanged));

        private static bool _isUpdating = false;

        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

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

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isUpdating = true;
            var passwordBox = sender as PasswordBox;
            SetBindablePassword(passwordBox, passwordBox.Password);
            _isUpdating = false;
        }
    }
}
