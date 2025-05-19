using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    internal class NavigationVM : ViewModelBase
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateUserCommand { get; set; }
        public ICommand CreateEventCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand CreateEstablishmentCommand { get; set; }
        public ICommand ManageUsersCommand { get; set; }
        public ICommand ManageEventsCommand { get; set; }
        public ICommand ManageEstablishmentsCommand { get; set; }

        private void NavigateToManageEstablishments(object obj)
        {
            CurrentView = new ManageEstablishmentsVM();
        }
        private void NavigateToCreateUser(object obj)
        {
            CurrentView = new CreateUserVM();
        }
        private void NavigateToCreateEvent(object obj)
        {
            CurrentView = new CreateEventVM();
        }
        private void NavigateToCreateEstablishment(object obj)
        {
            CurrentView = new CreateEstablishmentVM();
        }
        private void NavigateToManageEvents(object obj)
        {
            CurrentView = new ManageEventsVM();
        }
        private void NavigateToLogin(object obj)
        {
            CurrentView = new LoginVM();
        }
        private void NavigateToManageUsers(object obj)
        {
            CurrentView = new ManageUsersVM();
        }

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
