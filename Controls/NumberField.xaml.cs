
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace RacursConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для NumberField.xaml
    /// </summary>
    public partial class NumberField : UserControl,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private string parseError = "Ожидается числовое значение"; 
        private TextBox textField;
        private TextBlock textValid;
        private string notValidText = "  ❗";
        private string validText = " ✔";
        private SolidColorBrush validColor = new SolidColorBrush(Colors.Green);
        private SolidColorBrush noValidColor = new SolidColorBrush(Colors.Red);
        public NumberField() {
            InitializeComponent();
            textField = textValidateField;
            textField.TextChanged += TextChanged;
            textValid = textValidateBlock;
            Loaded += NumberFieldLoaded;
            textField.Text = "0";
            
        }


        private void NumberFieldLoaded(object sender, RoutedEventArgs e)
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
            DependencyProperty.Register("Text", typeof(string), typeof(NumberField), new PropertyMetadata("0"));

       

        private void ValidateProperty()
        { 
            double value;         
            bool result = double.TryParse(Text, out value);
           
            if (!result)
            {   
                IsValid = false;               
                animatetextFieldNotValid(parseError);               
                return;
            }
            if (RangeValidation == false)
            {
                IsValid = true;
                animatetextFieldValid();
                return;
            }
            if (value > ValidationMax || value < ValidationMin) {
                  string intervalError = "Ожидается числовое значение в интервале [" + ValidationMin + ";" + ValidationMax + "]";
                 
                  animatetextFieldNotValid(intervalError);               
                return;
            }
           
            //
            animatetextFieldValid();
        }

        private void animatetextFieldNotValid(string error)
        {
            IsValid = false;
            textValid.Foreground = noValidColor;
            textValid.ToolTip = error;
            textValid.Text = notValidText;
        }
        private void animatetextFieldValid()
        {
            IsValid = true;
            textValid.ToolTip = null;
            textValid.Foreground = validColor;
            textValidateBlock.Text = validText;
        }


        public double ValidationMin
        {
            get { return (double)GetValue(ValidationMinProperty); }
            set { SetValue(ValidationMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValidationMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMinProperty =
            DependencyProperty.Register("ValidationMin", typeof(double), typeof(NumberField), new PropertyMetadata(0.0));
        public double ValidationMax
        {
            get { return (double)GetValue(ValidationMaxProperty); }
            set { SetValue(ValidationMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMaxProperty =
            DependencyProperty.Register("ValidationMax", typeof(double), typeof(NumberField), new PropertyMetadata(0.0));
        public bool RangeValidation
        {
            get { return (bool)GetValue(RangeValidationProperty); }
            set { SetValue(RangeValidationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeValidationProperty =
            DependencyProperty.Register("RangeValidation", typeof(bool), typeof(NumberField), new PropertyMetadata(false));



        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(NumberField), new PropertyMetadata(false));

     
     
        private void TextChanged(object sender, TextChangedEventArgs e){
            Text = textField.Text;
            ValidateProperty();
        }
    }
    }

