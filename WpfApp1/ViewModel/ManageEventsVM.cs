using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Windows;
using System;
using WpfApp1.Models;
using System.Collections.Generic;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// ViewModel para la gestión de eventos en la aplicación.
    /// Permite cargar, actualizar y eliminar eventos, así como gestionar la selección y edición de sus propiedades.
    /// </summary>
    public class ManageEventsVM : ViewModelBase
    {
        private Event _selectedEvent;
        private string _name;
        private int _price;
        private int _reservedPlaces;
        private string _photo;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool _seating;
        private string _description;
        private int _establishId;
        private List<Establishment> _allEstablishments;
        private Establishment _selectedEstablishment;

        #region Propiedades

        /// <summary>
        /// Lista observable de eventos disponibles.
        /// </summary>
        public ObservableCollection<Event> EventsList { get; set; }

        /// <summary>
        /// Evento seleccionado actualmente.
        /// </summary>
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
                    StartDate = null;
                    EndDate = null;
                    Description = string.Empty;
                    EstablishId = 0;
                    SelectedEstablishment = null;
                    _selectedEvent = null;
                }
                else
                {
                    _selectedEvent = value;
                    Name = _selectedEvent.name;
                    Price = _selectedEvent.price;
                    ReservedPlaces = _selectedEvent.reserved_places;
                    Photo = _selectedEvent.photo;
                    DateTime tempStart, tempEnd;
                    StartDate = DateTime.TryParse(_selectedEvent.start_date, out tempStart) ? (DateTime?)tempStart : null;
                    EndDate = DateTime.TryParse(_selectedEvent.end_date, out tempEnd) ? (DateTime?)tempEnd : null;
                    Seating = _selectedEvent.seating;
                    Description = _selectedEvent.descripcion;
                    EstablishId = _selectedEvent.establish_id;
                    SelectedEstablishment = AllEstablishments?.Find(e => e.establish_id == _selectedEvent.establish_id);
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
                OnPropertyChanged(nameof(SelectedEstablishment));
            }
        }

        /// <summary>
        /// Nombre del evento.
        /// </summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Precio del evento.
        /// </summary>
        public int Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Plazas reservadas para el evento.
        /// </summary>
        public int ReservedPlaces
        {
            get => _reservedPlaces;
            set { _reservedPlaces = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Ruta o URL de la foto del evento.
        /// </summary>
        public string Photo
        {
            get => _photo;
            set { _photo = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fecha de inicio del evento.
        /// </summary>
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fecha de fin del evento.
        /// </summary>
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Indica si el evento tiene asientos numerados.
        /// </summary>
        public bool Seating
        {
            get => _seating;
            set { _seating = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Descripción del evento.
        /// </summary>
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// ID del establecimiento asociado al evento.
        /// </summary>
        public int EstablishId
        {
            get => _establishId;
            set { _establishId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Lista de todos los establecimientos disponibles.
        /// </summary>
        public List<Establishment> AllEstablishments
        {
            get => _allEstablishments;
            set { _allEstablishments = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Establecimiento seleccionado para el evento.
        /// </summary>
        public Establishment SelectedEstablishment
        {
            get => _selectedEstablishment;
            set
            {
                _selectedEstablishment = value;
                if (_selectedEstablishment != null)
                    EstablishId = _selectedEstablishment.establish_id;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos

        /// <summary>
        /// Comando para eliminar un evento.
        /// </summary>
        public ICommand DeleteEventCommand { get; }

        /// <summary>
        /// Comando para actualizar un evento.
        /// </summary>
        public ICommand UpdateEventCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ManageEventsVM"/>.
        /// </summary>
        public ManageEventsVM()
        {
            LoadEstablishments();
            LoadEvents();

            DeleteEventCommand = new RelayCommand(ExecuteDeleteEvent);
            UpdateEventCommand = new RelayCommand(ExecuteUpdateEvent);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga la lista de eventos desde la base de datos.
        /// </summary>
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

        /// <summary>
        /// Carga la lista de establecimientos desde la base de datos.
        /// </summary>
        private void LoadEstablishments()
        {
            try
            {
                AllEstablishments = EstablishmentOrm.SelectAllEstablishments();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar establecimientos: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la eliminación del evento seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
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
                    System.Windows.MessageBox.Show("No se pudo eliminar el evento, comprueba que no tenga nínguna clave foránea asociada", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No se pudo eliminar el evento, comprueba que no tenga nínguna clave foránea asociada", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la actualización de los datos del evento seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void ExecuteUpdateEvent(object obj)
        {
            if (SelectedEvent == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un evento para actualizar.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validaciones de campos obligatorios
            if (StartDate == null || EndDate == null ||
                string.IsNullOrWhiteSpace(Name) ||
                Price <= 0 ||
                ReservedPlaces < 0 ||
                string.IsNullOrWhiteSpace(Description) ||
                SelectedEstablishment == null)
            {
                System.Windows.MessageBox.Show("Por favor, rellena todos los campos obligatorios antes de actualizar.", "Campos incompletos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    eventToUpdate.start_date = StartDate?.ToString("yyyy-MM-dd") ?? "";
                    eventToUpdate.end_date = EndDate?.ToString("yyyy-MM-dd") ?? "";
                    eventToUpdate.seating = this.Seating;
                    eventToUpdate.descripcion = this.Description;
                    eventToUpdate.establish_id = this.SelectedEstablishment?.establish_id ?? 0;

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