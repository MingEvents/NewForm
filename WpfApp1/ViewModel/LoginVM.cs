using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Windows;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// ViewModel para la gestión del inicio de sesión de usuarios en la aplicación.
    /// Gestiona los datos y comandos necesarios para autenticar a un usuario.
    /// </summary>
    public class LoginVM : ViewModelBase
    {
        private string _usernameOrEmail;
        private string _password;

        #region Propiedades

        /// <summary>
        /// Nombre de usuario o correo electrónico introducido por el usuario.
        /// </summary>
        public string UsernameOrEmail
        {
            get => _usernameOrEmail;
            set
            {
                _usernameOrEmail = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Contraseña introducida por el usuario.
        /// </summary>
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

        /// <summary>
        /// Comando para ejecutar el inicio de sesión.
        /// </summary>
        public ICommand LoginCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="LoginVM"/>.
        /// </summary>
        public LoginVM()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Ejecuta el proceso de inicio de sesión validando las credenciales del usuario.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
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