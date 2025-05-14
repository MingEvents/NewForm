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
    public class ManageEventsVM : ViewModelBase
    {
        private Event _selectedEvent;
        private string _name;
        private int _price;
        private int _reservedPlaces;
        private string _photo;
        private string _startDate;
        private string _endDate;
        private bool _seating;
        private string _description;
        private int _establishId;

        #region Propiedades

        public ObservableCollection<Event> EventsList { get; set; }

        public Event SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (value == null)
                {
                    // Limpia los campos si no hay selección
                    Name = string.Empty;
                    Price = 0;
                    ReservedPlaces = 0;
                    Photo = string.Empty;
                    StartDate = string.Empty;
                    EndDate = string.Empty;
                    Description = string.Empty;
                    EstablishId = 0;
                    _selectedEvent = null;
                }
                else
                {
                    _selectedEvent = value;
                    Name = _selectedEvent.name;
                    Price = _selectedEvent.price;
                    ReservedPlaces = _selectedEvent.reserved_places;
                    Photo = _selectedEvent.photo;
                    StartDate = _selectedEvent.start_date;
                    EndDate = _selectedEvent.end_date;
                    Seating = _selectedEvent.seating;
                    Description = _selectedEvent.descripcion;
                    EstablishId = _selectedEvent.establish_id;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(ReservedPlaces));
                OnPropertyChanged(nameof(Photo));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(Seating));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(EstablishId));
            }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public int Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        public int ReservedPlaces
        {
            get => _reservedPlaces;
            set { _reservedPlaces = value; OnPropertyChanged(); }
        }

        public string Photo
        {
            get => _photo;
            set { _photo = value; OnPropertyChanged(); }
        }

        public string StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public string EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public bool Seating
        {
            get => _seating;
            set { _seating = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public int EstablishId
        {
            get => _establishId;
            set { _establishId = value; OnPropertyChanged(); }
        }

        #endregion

        #region Comandos

        public ICommand DeleteEventCommand { get; }
        public ICommand UpdateEventCommand { get; }

        #endregion

        #region Constructor

        public ManageEventsVM()
        {
            LoadEvents();

            DeleteEventCommand = new RelayCommand(ExecuteDeleteEvent);
            UpdateEventCommand = new RelayCommand(ExecuteUpdateEvent);
        }

        #endregion

        #region Métodos

        private void LoadEvents()
        {
            try
            {
                var events = EventOrm.SelectAllEvents();
                EventsList = new ObservableCollection<Event>(events);
                OnPropertyChanged(nameof(EventsList));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar eventos: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteDeleteEvent(object obj)
        {
            if (SelectedEvent == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un evento.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool deleted = EventOrm.DeleteEvent(SelectedEvent.event_id);

                if (deleted)
                {
                    EventsList.Remove(SelectedEvent);
                    SelectedEvent = null;
                }
                else
                {
                    System.Windows.MessageBox.Show("No se pudo eliminar el evento.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteUpdateEvent(object obj)
        {
            if (SelectedEvent == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un evento para actualizar.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var eventToUpdate = Orm.db.Event.Find(SelectedEvent.event_id);

                if (eventToUpdate != null)
                {
                    eventToUpdate.name = this.Name;
                    eventToUpdate.price = this.Price;
                    eventToUpdate.reserved_places = this.ReservedPlaces;
                    eventToUpdate.photo = this.Photo;
                    eventToUpdate.start_date = this.StartDate;
                    eventToUpdate.end_date = this.EndDate;
                    eventToUpdate.seating = this.Seating;
                    eventToUpdate.descripcion = this.Description;
                    eventToUpdate.establish_id = this.EstablishId;

                    Orm.db.SaveChanges();

                    System.Windows.MessageBox.Show("Evento actualizado correctamente.",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al actualizar: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}