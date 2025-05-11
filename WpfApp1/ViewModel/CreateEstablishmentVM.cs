using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

namespace WpfApp1.ViewModel
{
    public class CreateEstablishmentVM : ViewModelBase
    {
        private string _name;
        private string _direction;
        private int _capacity;
        private int _cityId;

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

        public ICommand CreateEstablishmentCommand { get; }

        #endregion

        #region Constructor

        public CreateEstablishmentVM()
        {
            CreateEstablishmentCommand = new RelayCommand(ExecuteCreateEstablishment);
        }

        #endregion

        #region Métodos

        private void ExecuteCreateEstablishment(object obj)
        {
            try
            {
                var newEstablishment = new Model.Establishment
                {
                    name = this.Name,
                    direction = this.Direction,
                    capacity = this.Capacity,
                    city_id = this.CityId
                };

                EstablishmentOrm.InsertEstablishment(newEstablishment);

                ClearFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void ClearFields()
        {
            Name = string.Empty;
            Direction = string.Empty;
            Capacity = 0;
            CityId = 0;
        }

        #endregion
    }
}