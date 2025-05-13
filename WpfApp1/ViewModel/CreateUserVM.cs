using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Collections.Generic;

namespace WpfApp1.ViewModel
{
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

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }
        public List<string> Role
        {
            get => _roles;
            set { _roles = value; OnPropertyChanged(); }
        }

        public string SecondName
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set { _selectedRole = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public int Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(); }
        }

        public ICommand CreateUserCommand { get; }

        public CreateUserVM()
        {
            CreateUserCommand = new RelayCommand(ExecuteCreateUser);
            LoadRoles();
        }
        private void LoadRoles()
        {
            try
            {
                Role = UsersOrm.SlectAllRoles();
                OnPropertyChanged(nameof(Role));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void ExecuteCreateUser(object obj)
        {
            int roleId=11;
            if (this.SelectedRole == "superAdmin")
            {
                roleId = 9;
            }
            else if (this.SelectedRole == "normalUser")
            {
                roleId = 10;
            }

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

        private void ClearFields()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Phone = 0;
            Address = string.Empty;
            BirthDate = null;
        }
    }
}