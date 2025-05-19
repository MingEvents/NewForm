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
    /// <summary>
    /// ViewModel para la creación de eventos en la aplicación.
    /// Gestiona los datos y comandos necesarios para crear un nuevo evento y generar descripciones automáticas.
    /// </summary>
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
        public string StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Fecha de fin del evento.
        /// </summary>
        public string EndDate
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
            set
            {
                _allEstablishments = value;
                OnPropertyChanged();
            }
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
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Comando para crear un nuevo evento.
        /// </summary>
        public ICommand CreateEventCommand { get; }

        /// <summary>
        /// Comando para generar la descripción del evento usando IA.
        /// </summary>
        public ICommand GenerateDescriptionIACommand { get; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CreateEventVM"/>.
        /// </summary>
        public CreateEventVM()
        {
            CreateEventCommand = new RelayCommand(ExecuteCreateEvent);
            GenerateDescriptionIACommand = new RelayCommand(LoadIaDescriptionAsync);
            LoadEstablishments();
        }

        /// <summary>
        /// Carga la lista de establecimientos disponibles.
        /// </summary>
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

        /// <summary>
        /// Ejecuta la generación de la descripción usando IA de forma asíncrona.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
        private async void LoadIaDescriptionAsync(object obj)
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

        /// <summary>
        /// Llama a la API de OpenAI para generar una descripción creativa para el evento.
        /// </summary>
        /// <returns>Una tarea asíncrona.</returns>
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

        /// <summary>
        /// Ejecuta la creación de un nuevo evento y lo guarda en la base de datos.
        /// </summary>
        /// <param name="obj">Parámetro del comando (no se utiliza).</param>
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
                MessageBox.Show("¡Evento Creado exitosamente!", "Éxito",
                    MessageBoxButton.OK, MessageBoxImage.Information);

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

        /// <summary>
        /// Limpia los campos del formulario de creación de eventos.
        /// </summary>
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