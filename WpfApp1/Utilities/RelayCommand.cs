using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1.Utilities
{
    /// <summary>
    /// Implementa la interfaz <see cref="ICommand"/> para delegar la lógica de ejecución y comprobación de comandos en acciones y funciones proporcionadas.
    /// Permite la creación de comandos reutilizables y parametrizables en aplicaciones WPF.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Se produce cuando cambia la capacidad del comando para ejecutarse.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RelayCommand"/>.
        /// </summary>
        /// <param name="execute">Acción a ejecutar cuando se invoca el comando.</param>
        /// <param name="canExecute">Función que determina si el comando puede ejecutarse. Si es null, el comando siempre puede ejecutarse.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Determina si el comando puede ejecutarse con el parámetro proporcionado.
        /// </summary>
        /// <param name="parameter">Parámetro del comando.</param>
        /// <returns>True si el comando puede ejecutarse; en caso contrario, false.</returns>
        public bool CanExecute(object parameter) =>  _canExecute == null || _canExecute(parameter);

        /// <summary>
        /// Ejecuta la acción asociada al comando.
        /// </summary>
        /// <param name="parameter">Parámetro del comando.</param>
        public void Execute(object parameter) => _execute(parameter);
    }
}
