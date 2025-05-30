﻿using System;
using System.Collections.Generic;
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
        #endregion

        #region Comandos

        public ICommand CreateEstablishmentCommand { get; }

        #endregion

        #region Constructor

        public CreateEstablishmentVM()
        {
            CreateEstablishmentCommand = new RelayCommand(ExecuteCreateEstablishment);
            LoadAllCities();
        }

        #endregion

        #region Métodos

        private void ExecuteCreateEstablishment(object obj)
        {
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

                ClearFields();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }
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

        private void ClearFields()
        {
            Name = string.Empty;
            Direction = string.Empty;
            Capacity = "";
        }

        #endregion
    }
}