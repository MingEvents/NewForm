using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Windows;
using System;
using WpfApp1.Models;

namespace WpfApp1.ViewModel
{
    public class ManageUsersVM : ViewModelBase
    {
        private Users _selectedUser;
        private string _name;
        private string _email;
        private string _phone;
        #region Propiedades

        public ObservableCollection<Users> UsersList { get; set; }

        public Users SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = new Users();
                Name = _selectedUser.name; // ← ERROR AQUÍ
                Email = _selectedUser.email;
                Phone = _selectedUser.phone.ToString();
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }


        #endregion

        #region Comandos

        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserCommand { get; }


        #endregion

        #region Constructor

        public ManageUsersVM()
        {
            LoadUsers();
            UpdateUserCommand = new RelayCommand(ExecuteUpdateUser);
            // Inicializar comandos
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser);
        }

        #endregion

        #region Métodos

        private void LoadUsers()
        {
            try
            {
                var users = UsersOrm.SlectAllUsers();
                UsersList = new ObservableCollection<Users>(users);
                OnPropertyChanged(nameof(UsersList));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error al cargar usuarios",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteUpdateUser(object obj)
        {
            if (SelectedUser == null) return;

            try
            {
               var result  = UsersOrm.UpdateUser(SelectedUser);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error al actualizar",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteDeleteUser(object obj)
        {
            if (SelectedUser == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un usuario.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool deleted = UsersOrm.DeleteUser(SelectedUser.user_id);

                if (deleted)
                {
                    UsersList.Remove(SelectedUser);
                    SelectedUser = null;
                }
                else
                {
                    System.Windows.MessageBox.Show("No se pudo eliminar el usuario.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}