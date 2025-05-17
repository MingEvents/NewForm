using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Model.Managment;
using WpfApp1.Models;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    public class ManageEstablishmentsVM : ViewModelBase
    {
        private Establishment _selectedEstablishment;
        private string _name;
        private string _direction;
        private int _capacity;
        private int _cityId;

        #region Propiedades

        public ObservableCollection<Establishment> EstablishmentsList { get; set; }

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
                    _selectedEstablishment = null;
                }
                else
                {
                    _selectedEstablishment = value;
                    Name = _selectedEstablishment.name;
                    Direction = _selectedEstablishment.direction;
                    Capacity = _selectedEstablishment.capacity;
                    CityId = _selectedEstablishment.city_id;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Direction));
                OnPropertyChanged(nameof(Capacity));
                OnPropertyChanged(nameof(CityId));
            }
        }

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

        public int Capacity
        {
            get => _capacity;
            set { _capacity = value; OnPropertyChanged(); }
        }

        public int CityId
        {
            get => _cityId;
            set { _cityId = value; OnPropertyChanged(); }
        }

        #endregion

        #region Comandos

        public ICommand UpdateEstablishmentCommand { get; }
        public ICommand DeleteEstablishmentCommand { get; }

        #endregion

        #region Constructor

        public ManageEstablishmentsVM()
        {
            LoadEstablishments();

            UpdateEstablishmentCommand = new RelayCommand(ExecuteUpdate);
            DeleteEstablishmentCommand = new RelayCommand(ExecuteDelete);
        }

        #endregion

        #region Métodos

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
                    establishmentToUpdate.city_id = this.CityId;

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
                    System.Windows.MessageBox.Show("No se pudo eliminar el establecimiento.", "Error",
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
