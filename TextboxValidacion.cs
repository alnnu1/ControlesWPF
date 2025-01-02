using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/// <summary>
/// Controles con propiedades de validación y de MySQL
/// </summary>
namespace ControlesWPF
{
    /// <summary>
    /// Textbox con propiedades de validación
    /// </summary>
    public class TextboxValidacion : TextBox
    {
        #region Propiedades

        /// <summary>
        /// Elimina los espacios del principio y al final del Textbox
        /// </summary>
        public bool EliminarEspacios { get; set; }

        #endregion

        #region Eventos

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (EliminarEspacios)
            {
                Text = Text.Trim();
            }
        }

        #endregion
    }
}
