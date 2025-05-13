using System;
using System.Windows.Input;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    internal class NavigationVM : ViewModelBase
    {
        private object _currentView;

        // Propiedad para la vista actual
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // Comandos para la navegación
        public ICommand CreateUserCommand { get; set; }
        public ICommand CreateEventCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand CreateEstablishmentCommand { get; set; }
        public ICommand ManageUsersCommand { get; set; } // Comando para gestionar usuarios


        // Métodos para cambiar la vista
        private void NavigateToCreateUser(object obj) => CurrentView = new CreateUserVM();
        private void NavigateToCreateEvent(object obj) => CurrentView = new CreateEventVM();
        private void NavigateToCreateEstablishment(object obj) => CurrentView = new CreateEstablishmentVM();

        private void NavigateToLogin(object obj) => CurrentView = new LoginVM();
        private void NavigateToManageUsers(object obj) => CurrentView = new ManageUsersVM(); // Método para gestionar usuarios

        // Constructor
        public NavigationVM()
        {
            // Inicializa los comandos con sus respectivos métodos
            CreateUserCommand = new RelayCommand(NavigateToCreateUser);
            CreateEventCommand = new RelayCommand(NavigateToCreateEvent, CanNavigate);
            CreateEstablishmentCommand = new RelayCommand(NavigateToCreateEstablishment);
            LoginCommand = new RelayCommand(NavigateToLogin);
            ManageUsersCommand = new RelayCommand(NavigateToManageUsers); // Comando para gestionar usuarios
            // Vista inicial
            CurrentView = new CreateUserVM(); // Carga directamente el UserControl
                                                                                                  
        }
        private bool CanNavigate(object obj)
        {
            // Lógica para habilitar o deshabilitar el comando
            return true; // Siempre habilitado en este caso
        }

    }
}
