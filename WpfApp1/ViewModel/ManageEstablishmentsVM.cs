using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignColors;
using WpfApp1.Model;
using WpfApp1.Model.Managment;
using WpfApp1.Models;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// ViewModel para la gestión de establecimientos.
    /// Permite cargar, actualizar y eliminar establecimientos, así como gestionar la selección y edición de sus propiedades.
    /// </summary>
    public class ManageEstablishmentsVM : ViewModelBase
    {
        private Establishment _selectedEstablishment;
        private string _name;
        private string _direction;
        private int _capacity;
        private int _cityId;
        private List<City> _allCities;
        private City _selectedCity;

        #region Propiedades

        /// <summary>
        /// Lista observable de establecimientos disponibles.
        /// </summary>
        public ObservableCollection<Establishment> EstablishmentsList { get; set; }

        /// <summary>
        /// Establecimiento seleccionado actualmente.
        /// </summary>
        public Establishment SelectedEstablishment
        {
            get => _selectedEstablishment;
            set
            {
                if (value == null)
                {
                    Name = string.Empty;
                    Direction = string.Empty;
                    Capacity = 0;
                    CityId = 0;
                    SelectedCity = null;
                    _selectedEstablishment = null;
                }
                else
                {
                    _selectedEstablishment = value;
                    Name = _selectedEstablishment.name;
                    Direction = _selectedEstablishment.direction;
                    Capacity = _selectedEstablishment.capacity;
                    CityId = _selectedEstablishment.city_id;
                    SelectedCity = AllCities?.Find(c => c.city_id == _selectedEstablishment.city_id);
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Direction));
                OnPropertyChanged(nameof(Capacity));
                OnPropertyChanged(nameof(CityId));
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        /// <summary>
        /// Nombre del establecimiento.
        /// </summary>
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Dirección del establecimiento.
        /// </summary>
        public string Direction
        {
            get => _direction;
            set { _direction = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Capacidad del establecimiento.
        /// </summary>
        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// ID de la ciudad asociada al establecimiento.
        /// </summary>
        public int CityId
        {
            get => _cityId;
            set { _cityId = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Lista de todas las ciudades disponibles.
        /// </summary>
        public List<City> AllCities
        {
            get => _allCities;
            set { _allCities = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Ciudad seleccionada actualmente.
        /// </summary>
        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                if (_selectedCity != null)
                    CityId = _selectedCity.city_id;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Comandos

        /// <summary>
        /// Comando para actualizar un establecimiento.
        /// </summary>
        public ICommand UpdateEstablishmentCommand { get; }

        /// <summary>
        /// Comando para eliminar un establecimiento.
        /// </summary>
        public ICommand DeleteEstablishmentCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ManageEstablishmentsVM"/>.
        /// </summary>
        public ManageEstablishmentsVM()
        {
            LoadCities();
            LoadEstablishments();

            UpdateEstablishmentCommand = new RelayCommand(ExecuteUpdate);
            DeleteEstablishmentCommand = new RelayCommand(ExecuteDelete);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga la lista de establecimientos desde la base de datos.
        /// </summary>
        private void LoadEstablishments()
        {
            try
            {
                var establishments = EstablishmentOrm.SelectAllEstablishments();
                EstablishmentsList = new ObservableCollection<Establishment>(establishments);
                OnPropertyChanged(nameof(EstablishmentsList));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar establecimientos: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Carga la lista de ciudades desde la base de datos.
        /// </summary>
        private void LoadCities()
        {
            try
            {
                AllCities = EstablishmentOrm.getAllCities();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al cargar ciudades: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la actualización de los datos del establecimiento seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void ExecuteUpdate(object obj)
        {
            if (SelectedEstablishment == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un establecimiento.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var establishmentToUpdate = Orm.db.Establishment.Find(SelectedEstablishment.establish_id);

                if (establishmentToUpdate != null)
                {
                    establishmentToUpdate.name = this.Name;
                    establishmentToUpdate.direction = this.Direction;
                    establishmentToUpdate.capacity = this.Capacity;
                    establishmentToUpdate.city_id = this.SelectedCity?.city_id ?? 0;

                    Orm.db.SaveChanges();

                    System.Windows.MessageBox.Show("Establecimiento actualizado correctamente.", "Éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error al actualizar: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ejecuta la eliminación del establecimiento seleccionado.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private void ExecuteDelete(object obj)
        {
            if (SelectedEstablishment == null)
            {
                System.Windows.MessageBox.Show("Por favor selecciona un establecimiento.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool deleted = EstablishmentOrm.DeleteEstablishment(SelectedEstablishment.establish_id);

                if (deleted)
                {
                    EstablishmentsList.Remove(SelectedEstablishment);
                    SelectedEstablishment = null;
                }
                else
                {
                    System.Windows.MessageBox.Show("Error", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No se pudo eliminar el establecimiento, comprueba que no tenga eventos asociados", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion
    }
}