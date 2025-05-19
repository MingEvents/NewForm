using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Utilities
{
    /// <summary>
    /// Representa un botón personalizado basado en <see cref="RadioButton"/> para su uso en la interfaz WPF.
    /// Permite aplicar estilos personalizados a los botones de opción en la aplicación.
    /// </summary>
    public class Btn : RadioButton
    {
        /// <summary>
        /// Inicializa la clase <see cref="Btn"/> y sobrescribe la clave de estilo predeterminada
        /// para asociar el control con su estilo personalizado en XAML.
        /// </summary>
        static Btn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Btn), new FrameworkPropertyMetadata(typeof(Btn)));
        }
    }
}
