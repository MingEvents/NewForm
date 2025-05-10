using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;

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

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string SecondName
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
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
        }

        private void ExecuteCreateUser(object obj)
        {
            try
            {
                var newUser = new Users
                {
                    name = this.Name,
                    second_name = this.SecondName,
                    email = this.Email,
                    password = this.Password,
                    phone = this.Phone,
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