using System;
using System.Windows.Input;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;
using WpfApp1.Model;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp1.ViewModel
{
    public class CreateEventVM : ViewModelBase
    {
        private string _name;
        private int _price;
        private int _reservedPlaces;
        private string _photo;
        private string _startDate;
        private string _endDate;
        private bool _seating;
        private string _description;
        private int _establishId;
        private List<Establishment> _allEstablishments;
        private Establishment _selectedEstablishment;

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

        public List<Establishment> AllEstablishments
        {
            get => _allEstablishments;
            set
            {
                _allEstablishments = value;
                OnPropertyChanged();
            }
        }

        public Establishment SelectedEstablishment
        {
            get => _selectedEstablishment;
            set
            {
                _selectedEstablishment = value;
                OnPropertyChanged();
            }
        }
        public ICommand CreateEventCommand { get; }

        public CreateEventVM()
        {
            CreateEventCommand = new RelayCommand(ExecuteCreateEvent);
            LoadEstablishments();

        }
        private void LoadEstablishments()
        {
            try
            {
                AllEstablishments = EstablishmentOrm.SelectAllEstablishments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCreateEvent(object obj)
        {
            try
            {
                var newEvent = new Event
                {
                    name = this.Name,
                    price = this.Price,
                    reserved_places = this.ReservedPlaces,
                    photo = this.Photo,
                    start_date = this.StartDate,
                    end_date = this.EndDate,
                    seating = this.Seating,
                    descripcion = this.Description,
                    establish_id = this.SelectedEstablishment.establish_id,
                };

                EventOrm.InsertEvent(newEvent);

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
            Price = 0;
            ReservedPlaces = 0;
            Photo = string.Empty;
            StartDate = string.Empty;
            EndDate = string.Empty;
            Seating = false;
            Description = string.Empty;
            EstablishId = 0;
        }
    }
}