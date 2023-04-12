using RacursCore.SatilliteComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RacursConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl , INotifyPropertyChanged
    {
        
        private TextBox textField;
        private TextBlock textValid;
        private string notValidText = "  ❗";
        private string validText = " ✔";
        private SolidColorBrush validColor = new SolidColorBrush(Colors.Green);
        private SolidColorBrush noValidColor = new SolidColorBrush(Colors.Red);
        public TextField()
        {
            InitializeComponent();
            textField = textValidateField;
            textField.TextChanged += TextChanged;
            textValid = textValidateBlock;         
            Loaded += TextFieldLoaded;
            textField.Text = "";

        }


        private void TextFieldLoaded(object sender, RoutedEventArgs e)
        {
            textField.Text = Text;
            ValidateProperty();
        }




        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                OnPropertyChanged(nameof(Text));
            }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextField), new PropertyMetadata(" ",changed));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          

        }

        private void ValidateProperty()
        {
        
            if (Text.Length < MinLength)
            {
                string intervalError = "Значение должно быть не менее  " + MinLength + " символов";

                animatetextFieldNotValid(intervalError);
                return;
            }

            //
            animatetextFieldValid();
        }

        private void animatetextFieldNotValid(string error)
        {
            IsValid = false;
            textField.BorderBrush = noValidColor;
            textValid.Foreground = noValidColor;
            textValid.ToolTip = error;
            textValid.Text = notValidText;
        }
        private void animatetextFieldValid()
        {
            IsValid = true;
            textValid.ToolTip = null;
            textField.BorderBrush = validColor;
            textValid.Foreground = validColor;
            textValidateBlock.Text = validText;
        }






        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(TextField), new PropertyMetadata(0));




        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(TextField), new PropertyMetadata(false));



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            Text = textField.Text;
            ValidateProperty();
        }
    }
}
