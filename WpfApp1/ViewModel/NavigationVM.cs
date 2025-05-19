using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// ViewModel encargado de la navegación entre vistas principales de la aplicación.
    /// Gestiona los comandos de navegación y la vista actual.
    /// </summary>
    internal class NavigationVM : ViewModelBase
    {
        private object _currentView;

        /// <summary>
        /// Vista actual mostrada en la aplicación.
        /// </summary>
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Comando para navegar a la vista de creación de usuario.
        /// </summary>
        public ICommand CreateUserCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de creación de evento.
        /// </summary>
        public ICommand CreateEventCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de login.
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de creación de establecimiento.
        /// </summary>
        public ICommand CreateEstablishmentCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de gestión de usuarios.
        /// </summary>
        public ICommand ManageUsersCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de gestión de eventos.
        /// </summary>
        public ICommand ManageEventsCommand { get; set; }

        /// <summary>
        /// Comando para navegar a la vista de gestión de establecimientos.
        /// </summary>
        public ICommand ManageEstablishmentsCommand { get; set; }

        /// <summary>
        /// Navega a la vista de gestión de establecimientos.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToManageEstablishments(object obj)
        {
            CurrentView = new ManageEstablishmentsVM();
        }

        /// <summary>
        /// Navega a la vista de creación de usuario.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToCreateUser(object obj)
        {
            CurrentView = new CreateUserVM();
        }

        /// <summary>
        /// Navega a la vista de creación de evento.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToCreateEvent(object obj)
        {
            CurrentView = new CreateEventVM();
        }

        /// <summary>
        /// Navega a la vista de creación de establecimiento.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToCreateEstablishment(object obj)
        {
            CurrentView = new CreateEstablishmentVM();
        }

        /// <summary>
        /// Navega a la vista de gestión de eventos.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToManageEvents(object obj)
        {
            CurrentView = new ManageEventsVM();
        }

        /// <summary>
        /// Navega a la vista de login.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToLogin(object obj)
        {
            CurrentView = new LoginVM();
        }

        /// <summary>
        /// Navega a la vista de gestión de usuarios.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void NavigateToManageUsers(object obj)
        {
            CurrentView = new ManageUsersVM();
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="NavigationVM"/> y configura los comandos de navegación.
        /// </summary>
        public NavigationVM()
        {
            CreateUserCommand = new RelayCommand(NavigateToCreateUser, (o) => CanNavigate("CreateUserVM"));
            CreateEventCommand = new RelayCommand(NavigateToCreateEvent, (o) => CanNavigate("CreateEventVM"));
            CreateEstablishmentCommand = new RelayCommand(NavigateToCreateEstablishment, (o) => CanNavigate("CreateEstablishmentVM"));
            LoginCommand = new RelayCommand(NavigateToLogin);
            ManageUsersCommand = new RelayCommand(NavigateToManageUsers, (o) => CanNavigate("ManageUsersVM"));
            ManageEventsCommand = new RelayCommand(NavigateToManageEvents, (o) => CanNavigate("ManageEventsVM"));
            ManageEstablishmentsCommand = new RelayCommand(NavigateToManageEstablishments, (o) => CanNavigate("ManageEstablishmentsVM"));

            CurrentView = new LoginVM();
        }

        /// <summary>
        /// Determina si el usuario actual puede navegar a una vista específica según su rol.
        /// </summary>
        /// <param name="obj">Nombre de la vista a la que se desea navegar.</param>
        /// <returns>True si el usuario tiene permisos para navegar; en caso contrario, false.</returns>
        private bool CanNavigate(object obj)
        {
            if (ViewModelBase.userLoged == null)
            {
                return false;
            }

            string role = RoleOrm.SelectRoleName(ViewModelBase.userLoged.role_id);
            string commandName = obj as string;

            if (role == "superAdmin")
            {
                return true;
            }
            else if (role == "manager")
            {
                // El manager solo puede acceder a CreateEventVM y ManageEventsVM
                if (commandName == "CreateEventVM" || commandName == "ManageEventsVM")
                    return true;
                else
                    return false;
            }
            // Otros roles no tienen permisos
            return false;
        }
    }
}
