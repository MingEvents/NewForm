using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Reserve_Ticket> _userTickets;
        private ObservableCollection<Event> _userEvents;

        #region Propiedades

        public ObservableCollection<Users> UsersList { get; set; }

        public Users SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    Name = _selectedUser.name;
                    Email = _selectedUser.email;
                    Phone = _selectedUser.phone.ToString();
                    LoadUserTickets();
                }
                else
                {
                    Name = string.Empty;
                    Email = string.Empty;
                    Phone = string.Empty;
                    UserTickets = new ObservableCollection<Reserve_Ticket>();
                }
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

        public ObservableCollection<Reserve_Ticket> UserTickets
        {
            get => _userTickets;
            set { _userTickets = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Event> UserEvents
        {
            get => _userEvents;
            set { _userEvents = value; OnPropertyChanged(); }
        }

        #endregion

        #region Comandos

        public ICommand DeleteUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand UpdateTicketCommand { get; }

        #endregion

        #region Constructor

        public ManageUsersVM()
        {
            LoadUsers();
            UpdateUserCommand = new RelayCommand(ExecuteUpdateUser);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser);
            UpdateTicketCommand = new RelayCommand(ExecuteUpdateTicket);
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

        private void LoadUserTickets()
        {
            if (SelectedUser == null)
            {
                UserTickets = new ObservableCollection<Reserve_Ticket>();
                return;
            }
            var tickets = Reserve_TicketOrm.SelectTicket(SelectedUser.user_id);
            UserTickets = new ObservableCollection<Reserve_Ticket>(tickets);
        }
        /*
        private void LoadUserEvents()
        {

            if (UserTickets == null)
            {
                foreach (var ticket in UserTickets)
                {
                    var eventDetails = EventOrm.GetEventById(ticket.event_id);
                    if (eventDetails != null)
                    {
                        UserEvents.Add(eventDetails);
                    }
                }
            }
        }
        */

        private void ExecuteUpdateUser(object obj)
        {
            if (SelectedUser == null) return;

            try
            {
                var result = UsersOrm.UpdateUser(SelectedUser);
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
                    System.Windows.MessageBox.Show("No se pudo eliminar el usuario, comrpueba que no tenga reservas hechas", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteUpdateTicket(object obj)
        {
            if (obj is Reserve_Ticket ticket)
            {
                var armchair = ticket.Armchair;
                if (armchair != null)
                {
                    // Los valores de fila y columna se actualizan desde el binding
                    Orm.db.SaveChanges();
                    System.Windows.MessageBox.Show("Butaca actualizada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        #endregion
    }
}