using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Windows;
using System;

namespace WpfApp1.ViewModel
{
    public class ManageUsersVM : ViewModelBase
    {
        private Users _selectedUser;

        #region Propiedades

        public ObservableCollection<Users> UsersList { get; set; }

        public Users SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos

        public ICommand DeleteUserCommand { get; }

        #endregion

        #region Constructor

        public ManageUsersVM()
        {
            LoadUsers();

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