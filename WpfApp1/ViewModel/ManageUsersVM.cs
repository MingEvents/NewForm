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
    /// <summary>
    /// ViewModel para la gestión de usuarios en la aplicación.
    /// Permite cargar, actualizar y eliminar usuarios, así como gestionar sus reservas y tickets.
    /// </summary>
    public class ManageUsersVM : ViewModelBase
    {
        private Users _selectedUser;
        private string _name;
        private string _email;
        private string _phone;
        private ObservableCollection<Reserve_Ticket> _userTickets;
        private ObservableCollection<Event> _userEvents;

        #region Propiedades

        /// <summary>
        /// Lista observable de usuarios disponibles.
        /// </summary>
        public ObservableCollection<Users> UsersList { get; set; }

        /// <summary>
        /// Usuario seleccionado actualmente.
        /// </summary>
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

        /// <summary>
        /// Nombre del usuario seleccionado.
        /// </summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Email del usuario seleccionado.
        /// </summary>
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Teléfono del usuario seleccionado.
        /// </summary>
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Lista de tickets reservados por el usuario seleccionado.
        /// </summary>
        public ObservableCollection<Reserve_Ticket> UserTickets
        {
            get => _userTickets;
            set { _userTickets = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Lista de eventos reservados por el usuario seleccionado.
        /// </summary>
        public ObservableCollection<Event> UserEvents
        {
            get => _userEvents;
            set { _userEvents = value; OnPropertyChanged(); }
        }

        #endregion

        #region Comandos

        /// <summary>
        /// Comando para eliminar un usuario.
        /// </summary>
        public ICommand DeleteUserCommand { get; }

        /// <summary>
        /// Comando para actualizar un usuario.
        /// </summary>
        public ICommand UpdateUserCommand { get; }

        /// <summary>
        /// Comando para actualizar un ticket.
        /// </summary>
        public ICommand UpdateTicketCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ManageUsersVM"/>.
        /// </summary>
        public ManageUsersVM()
        {
            LoadUsers();
            UpdateUserCommand = new RelayCommand(ExecuteUpdateUser);
            DeleteUserCommand = new RelayCommand(ExecuteDeleteUser);
            UpdateTicketCommand = new RelayCommand(ExecuteUpdateTicket);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga la lista de usuarios desde la base de datos.
        /// </summary>
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

        /// <summary>
        /// Carga la lista de tickets reservados por el usuario seleccionado.
        /// </summary>
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
        /// <summary>
        /// Carga la lista de eventos reservados por el usuario seleccionado.
        /// </summary>
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

        /// <summary>
        /// Ejecuta la actualización de los datos del usuario seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void ExecuteUpdateUser(object obj)
        {
            if (SelectedUser == null) return;

            // Validaciones de campos obligatorios
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone) ||
                !int.TryParse(Phone, out int phoneValue) ||
                phoneValue <= 0)
            {
                System.Windows.MessageBox.Show("Por favor, rellena todos los campos obligatorios correctamente antes de actualizar.", "Campos incompletos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                SelectedUser.name = Name;
                SelectedUser.email = Email;
                SelectedUser.phone = phoneValue;

                var result = UsersOrm.UpdateUser(SelectedUser);
                System.Windows.MessageBox.Show("Usuario actualizado exitosamente.", "Éxito",
                   MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error al actualizar",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la eliminación del usuario seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
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
                System.Windows.MessageBox.Show("No se pudo eliminar el usuario, comrpueba que no tenga reservas hechas", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la actualización de un ticket reservado por el usuario.
        /// </summary>
        /// <param name="obj">Parámetro del comando (debe ser un <see cref="Reserve_Ticket"/>).</param>
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