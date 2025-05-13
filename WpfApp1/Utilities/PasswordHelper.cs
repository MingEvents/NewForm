using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Utilities
{
    public static class PasswordHelper
    {
        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached("BindablePassword", typeof(string), typeof(PasswordHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBindablePasswordChanged));

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
            if (d is PasswordBox passwordBox)
            {
                // Evitar bucles innecesarios al comparar el valor actual
                string newPassword = e.NewValue as string;
                if (passwordBox.Password != newPassword)
                {
                    passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                    passwordBox.Password = newPassword;
                    passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                }
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                // Evitar bucles innecesarios al comparar el valor actual
                string currentPassword = GetBindablePassword(passwordBox);
                if (passwordBox.Password != currentPassword)
                {
                    SetBindablePassword(passwordBox, passwordBox.Password);
                }
            }
        }

    }
}
