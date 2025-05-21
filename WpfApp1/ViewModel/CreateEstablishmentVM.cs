using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    public class CreateEstablishmentVM : Utilities.ViewModelBase
    {
        private string _name;
        private string _direction;
        private string _capacity;
        private List<string> _cities;
        private string _selectedCity;
        private int _numRows;
        private int _numColumns;

        #region Propiedades

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Direction
        {
            get => _direction;
            set { _direction = value; OnPropertyChanged(); }
        }

        public string Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged();
            }
        }

        public List<string> City
        {
            get => _cities;
            set
            {
                _cities = value;
                OnPropertyChanged();
            }
        }
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                OnPropertyChanged();
            }
        }

        public int NumRows
        {
            get => _numRows;
            set { _numRows = value; OnPropertyChanged(); CalculateCapacity(); }
        }

        public int NumColumns
        {
            get => _numColumns;
            set { _numColumns = value; OnPropertyChanged(); CalculateCapacity(); }
        }

        #endregion

        #region Comandos

        public ICommand CreateEstablishmentCommand { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CreateEstablishmentVM"/>.
        /// </summary>
        public CreateEstablishmentVM()
        {
            CreateEstablishmentCommand = new RelayCommand(ExecuteCreateEstablishment);
            LoadAllCities();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Calcula la capacidad del establecimiento en base al número de filas y columnas.
        /// </summary>
        private void CalculateCapacity()
        {
            if (NumRows != 0 && NumColumns != 0)
            {
                Capacity = (NumRows * NumColumns).ToString();
            }
        }

        /// <summary>
        /// Ejecuta la creación de un nuevo establecimiento.
        /// </summary>
        /// <param name="obj">Parámetro opcional del comando (no se utiliza).</param>
        private void ExecuteCreateEstablishment(object obj)
        {
            // Comprobación de que ningún dato obligatorio esté vacío o nulo
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Direction) ||
                string.IsNullOrWhiteSpace(Capacity) ||
                string.IsNullOrWhiteSpace(SelectedCity) ||
                NumRows <= 0 ||
                NumColumns <= 0)
            {
                MessageBox.Show("Por favor, rellena todos los campos obligatorios.", "Campos incompletos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int cityId = EstablishmentOrm.SlectCityId(SelectedCity);
            try
            {
                var newEstablishment = new Model.Establishment
                {
                    name = this.Name,
                    direction = this.Direction,
                    capacity = int.Parse(this.Capacity),
                    city_id = cityId
                };

                EstablishmentOrm.InsertEstablishment(newEstablishment);

                // Obtener el ID del establecimiento recién creado
                int establishId = newEstablishment.establish_id;
                // Si el ID no se actualiza automáticamente, recupéralo de la base de datos:
                if (establishId == 0)
                {
                    var last = EstablishmentOrm.GetEstablishmentById(newEstablishment.establish_id);
                    if (last != null) establishId = last.establish_id;
                }

                // Insertar las butacas
                EstablishmentOrm.InsertArmchairsForEstablishment(establishId, NumRows, NumColumns);
                MessageBox.Show("¡Establecimiento Creado exitosamente!", "Éxito",
                     MessageBoxButton.OK, MessageBoxImage.Information);

                ClearFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Carga la lista de todas las ciudades disponibles.
        /// </summary>
        /// <returns>Lista de nombres de ciudades.</returns>
        private List<string> LoadAllCities()
        {
            try
            {
                var cities = EstablishmentOrm.selectAllCities();
                City = cities;
                return cities;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Limpia los campos del formulario de creación.
        /// </summary>
        private void ClearFields()
        {
            Name = string.Empty;
            Direction = string.Empty;
            Capacity = "";
            SelectedCity = string.Empty;
            NumRows = 0;
            NumColumns = 0;

        }

        #endregion
    }
}