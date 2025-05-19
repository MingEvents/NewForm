using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// ViewModel para la creación de usuarios en la aplicación.
    /// Gestiona los datos y comandos necesarios para crear un nuevo usuario.
    /// </summary>
    public class CreateUserVM : ViewModelBase
    {
        private string _name;
        private string _username;
        private string _email;
        private string _password;
        private int _phone;
        private string _address;
        private DateTime? _birthDate;
        private List<string> _roles;
        private string _selectedRole;

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Lista de roles disponibles.
        /// </summary>
        public List<string> Role
        {
            get => _roles;
            set { _roles = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Segundo nombre o nombre de usuario.
        /// </summary>
        public string SecondName
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Rol seleccionado para el usuario.
        /// </summary>
        public string SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Email del usuario.
        /// </summary>
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Teléfono del usuario.
        /// </summary>
        public int Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Dirección del usuario.
        /// </summary>
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fecha de nacimiento del usuario.
        /// </summary>
        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Comando para crear un nuevo usuario.
        /// </summary>
        public ICommand CreateUserCommand { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CreateUserVM"/>.
        /// </summary>
        public CreateUserVM()
        {
            CreateUserCommand = new RelayCommand(ExecuteCreateUser);
            LoadRoles();
        }

        /// <summary>
        /// Carga la lista de roles disponibles.
        /// </summary>
        private void LoadRoles()
        {
            try
            {
                Role = RoleOrm.SlectAllRoles();
                OnPropertyChanged(nameof(Role));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la creación de un nuevo usuario y lo guarda en la base de datos.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void ExecuteCreateUser(object obj)
        {
            int roleId;

            roleId = RoleOrm.SelectRoleId(SelectedRole);

            try
            {
                var newUser = new Users
                {
                    name = this.Name,
                    second_name = this.SecondName,
                    email = this.Email,
                    password = this.Password,
                    phone = this.Phone,
                    role_id = roleId,
                };
                MessageBox.Show("¡Usuario Creado exitosamente!", "Éxito",
                 MessageBoxButton.OK, MessageBoxImage.Information);
                UsersOrm.InsertUser(newUser);

                // Opcional: limpiar campos después de insertar
                ClearFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Limpia los campos del formulario de creación de usuario.
        /// </summary>
        private void ClearFields()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Phone = 0;
            Address = string.Empty;
            BirthDate = null;
            SecondName = string.Empty;

        }
    }
}