using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Model.Managment;
using WpfApp1.Utilities;

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
        public ICommand GenerateDescriptionIACommand { get; }
        public CreateEventVM()
        {
            CreateEventCommand = new RelayCommand(ExecuteCreateEvent);
            GenerateDescriptionIACommand = new RelayCommand(LoadIaDescriptionAsync);
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
        private  async void LoadIaDescriptionAsync(object obj)
        {
            if (this.Description != null || this.Price != 0 || this.SelectedEstablishment != null  || this.Name != null)
            {
                try
                {
                    await GenerateDescriptionIAExecute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else 
            {
                MessageBox.Show("Primero, rellena el resto de campos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

        }

        private async Task GenerateDescriptionIAExecute()
        {
            string apiKey = "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";
            string endpoint = "https://api.openai.com/v1/chat/completions";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "Eres un generador de descripciones creativas para eventos y establecimientos." },
            new { role = "user", content = $"Genera una descripción atractiva para un establecimiento llamado {this.Name}, el establecimiento es {this.SelectedEstablishment.name}, con fecha para {this.StartDate}, ubicado en {this.SelectedEstablishment.City.name}y un precio de {this.Price}" }
        },
                max_tokens = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            this.Description = result.choices[0].message.content.ToString().Trim();
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