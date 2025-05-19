using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Utilities
{
    /// <summary>
    /// Clase base para los ViewModels en la arquitectura MVVM.
    /// Implementa <see cref="INotifyPropertyChanged"/> para notificar cambios en las propiedades.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Usuario actualmente logueado en la aplicación.
        /// </summary>
        public static Users userLoged { get; set; }

        /// <summary>
        /// Evento que se dispara cuando una propiedad cambia de valor.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica a la vista que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propName">Nombre de la propiedad que ha cambiado. Se asigna automáticamente si no se especifica.</param>
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
