using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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
        private bool TextPasted = false;

        public TextboxValidacion() => DataObject.AddPastingHandler(this, OnPaste);

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

        #region AceptarSoloDigitos

        public bool AceptarSoloDigitos
        {
            get { return (bool)GetValue(AceptarSoloDigitosProperty); }
            set { SetValue(AceptarSoloDigitosProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AcetarSoloDigitos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AceptarSoloDigitosProperty =
            DependencyProperty.Register("AceptarSoloDigitos", typeof(bool), typeof(TextboxValidacion), new PropertyMetadata(false));



        #endregion

        #endregion

        #region Eventos

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (RecortarEspacios)
            {
                Text = Text.Trim();
            }

            if (EliminarDobleEspacios)
            {
                Text = Regex.Replace(Text, " {2,}", " ");
            }
        }

        protected virtual void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true))
            {
                TextPasted = true;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            //Evita al usuario colocar un espacio cuando se tienen que elimitar todos los espacios
            if (EliminarTodosLosEspacios)
            {
                e.Handled = e.Key == Key.Space;
            }

            //Cambia de Focus al siguiente control cuando se presiona Enter
            if (TabConIntro && !AcceptsReturn && e.Key == Key.Enter)
            {
                e.Handled = true;

                FrameworkElement? ue = (FrameworkElement?)e.OriginalSource;
                ue?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                return;
            }

        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            if (AceptarSoloDigitos)
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (TextPasted)
            {
                if (RecortarEspacios)
                {
                    Text = Text.Trim();
                }

                if (EliminarDobleEspacios)
                {
                    Text = Regex.Replace(Text, " {2,}", " ");
                }

                if (EliminarTodosLosEspacios)
                {
                    Text = Regex.Replace(Text, @"\s+", string.Empty);
                }

                if (AceptarSoloDigitos)
                {
                    Text = Regex.Replace(Text, @"[^\d]", string.Empty);
                }

                SelectionStart = Text.Length;

                TextPasted = false;
            }
        }

        #endregion

    }
}
