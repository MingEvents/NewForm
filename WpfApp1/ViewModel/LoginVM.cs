using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Windows;

namespace WpfApp1.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        private string _usernameOrEmail;
        private string _password;

        #region Propiedades

        public string UsernameOrEmail
        {
            get => _usernameOrEmail;
            set
            {
                _usernameOrEmail = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos

        public ICommand LoginCommand { get; }

        #endregion

        #region Constructor

        public LoginVM()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        #endregion

        #region Métodos

        private void ExecuteLogin(object obj)
        {
            try
            {
                Users user = UsersOrm.Login(UsernameOrEmail, Password);

                if (user != null)
                {
                    ViewModelBase.userLoged = user; // Guardar el usuario actual en la propiedad estática
                    // Inicio de sesión exitoso
                    MessageBox.Show("¡Inicio de sesión exitoso!", "Éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);


                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}