using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

/// <summary>
/// Controles con propiedades de validación y funciones de MySQL
/// </summary>
namespace ControlesWPF
{
    /// <summary>
    /// Textbox con propiedades de validación
    /// </summary>
    public class TextboxValidacion : TextBox
    {

        #region Variables

        private string? PastedText;

        #endregion

        public TextboxValidacion()
        {
            DataObject.AddPastingHandler(this, OnPaste);
        }

        #region Propiedades

        #region RecortarEspacios

        public static readonly DependencyProperty RecortarEspaciosPropiedad = DependencyProperty.Register(
            "RecortarEspacios", typeof(bool), typeof(TextboxValidacion));

        /// <summary>
        /// Elimina los espacios del principio y al final
        /// </summary>
        /// <returns>La propiedad booleana que indica si el texto deberia eliminar los espacios del principio y del final</returns>
        public bool RecortarEspacios
        {
            get => (bool)GetValue(RecortarEspaciosPropiedad);
            set => SetValue(RecortarEspaciosPropiedad, value);
        }

        #endregion

        #region EliminarDobleEspacios

        public static readonly DependencyProperty EliminarDobleEspaciosPropiedad = DependencyProperty.Register(
            "EliminarDobleEspacios", typeof(bool), typeof(TextboxValidacion));

        /// <summary>
        /// Elimina dobles espacios o más
        /// </summary>
        /// <returns>La propiedad booleana que indica si se debería eliminar dobles espacios o más dentro</returns>
        public bool EliminarDobleEspacios
        {
            get => (bool)GetValue(EliminarDobleEspaciosPropiedad);
            set => SetValue(EliminarDobleEspaciosPropiedad, value);
        }

        #endregion

        #region EliminarTodosLosEspacios

        public static readonly DependencyProperty EliminarTodosLosEspaciosPropiedad = DependencyProperty.Register(
            "EliminarTodosLosEspacios", typeof(bool), typeof(TextboxValidacion));

        /// <summary>
        /// Elimina todos los espacios
        /// </summary>
        /// <returns>La propiedad booleana que indica si se debería eliminar todos los espacios</returns>
        public bool EliminarTodosLosEspacios
        {
            get => (bool)GetValue(EliminarTodosLosEspaciosPropiedad);
            set => SetValue(EliminarTodosLosEspaciosPropiedad, value);
        }

        #endregion

        #region MinLenght

        public static readonly DependencyProperty MinLenghtPropiedad = DependencyProperty.Register(
            "MinLenght", typeof(int), typeof(TextboxValidacion));

        /// <summary>
        /// Indica la cantidad de caracteres que el texto tiene que tener
        /// </summary>
        /// <returns>La cantidad de caracteres que el texto tiene que tener</returns>
        public int MinLenght
        {
            get => (int)GetValue(MinLenghtPropiedad);
            set => SetValue(MinLenghtPropiedad, value);
        }

        /// <summary>
        /// Indica si la propiedad MinLenght es menor o igual a la cantidad de caracteres en el texto
        /// </summary>
        public bool IsMinLenght
        {
            get => MinLenght <= Text.Length;
        }

        #endregion

        #region TabConIntro

        public static readonly DependencyProperty TabConIntroPropiedad = DependencyProperty.Register(
            "TabConIntro", typeof(bool), typeof(TextboxValidacion));

        /// <summary>
        /// Establece si automáticamente se presiona Tab cuando se presiona Intro
        /// </summary>
        /// <returns>El valor que indica si automáticamente se presiona Tab cuando se presiona Intro</returns>
        public bool TabConIntro
        {
            get => (bool)GetValue(TabConIntroPropiedad);
            set => SetValue(TabConIntroPropiedad, value);
        }

        #endregion


        #endregion

        #region Eventos

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (RecortarEspacios)
                Text = Text.Trim();
            if (EliminarDobleEspacios)
                Text = Regex.Replace(Text, " {2,}", " ");

        }

        protected virtual void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true))
                PastedText = (string)e.SourceDataObject.GetData(DataFormats.UnicodeText);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);


            //Evita al usuario colocar un espacio cuando se tienen que elimitar todos los espacios
            if (EliminarTodosLosEspacios)
                e.Handled = e.Key == Key.Space;
            
            //Cambia de Focus al siguiente control cuando se presiona Enter
            if (TabConIntro && !AcceptsReturn && e.Key == Key.Enter)
            {
                e.Handled = true;
                FrameworkElement? ue = (FrameworkElement?)e.OriginalSource;
                ue?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            //Recorta o elimina los dobles espaiocs cuando se pega texto dentro del TextBox
            if (Text == PastedText)
            {
                if (RecortarEspacios)
                    Text = Text.Trim();

                if (EliminarDobleEspacios)
                    Text = Regex.Replace(Text, " {2,}", " ");

                if (EliminarTodosLosEspacios)
                    Text = Regex.Replace(Text, @"\s+", string.Empty);

                PastedText = string.Empty;
                SelectionStart = Text.Length;
            }
        }

        #endregion

    }
}
