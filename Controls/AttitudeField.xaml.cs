using RacursCore.types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RacursConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для AttitudeField.xaml
    /// </summary>
    public partial class AttitudeField : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    
        private List<TextBox> numberFields;
        public AttitudeField()
        {
            InitializeComponent();
            DataContext = this;
            numberFields = new List<TextBox>
            {
               X,Y,Z,W
            };
            foreach (TextBox field in numberFields)
            {
                //field.ValidationMin = ValidationMin;
                //field.ValidationMax = ValidationMax;
                //field.RangeValidation = RangeValidation;
                field.TextChanged += AttitudeChanged;
            }
            Loaded += AttitudeField_Loaded;
        }

        private void AttitudeField_Loaded(object sender, RoutedEventArgs e)
        {
           
           
        }

        private void AttitudeChanged(object sender, TextChangedEventArgs e)
        {
            //IsValid = numberFields.Find(p => !p.IsValid) == null ? true : false;
            //IsValid = RangeValidation ? IsValid : true;
             Attitude = new Attitude(Convert.ToDouble(X.Text), Convert.ToDouble(Y.Text), Convert.ToDouble(Z.Text), Convert.ToDouble(W.Text));
        }
        private void updateFields()
        {
            X.Text = Attitude.X.ToString();
        }
        public Attitude Attitude
        {
            get { return (Attitude)GetValue(AttitudeProperty); }
            set { SetValue(AttitudeProperty, value); updateFields(); OnPropertyChanged(nameof(Attitude)); }
        }



        //// Using a DependencyProperty as the backing store for Attitude.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttitudeProperty =
            DependencyProperty.Register("Attitude", typeof(Attitude), typeof(AttitudeField), new PropertyMetadata(new Attitude(0, 0, 0, 0), changed));
      
       
        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
         
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(AttitudeField), new PropertyMetadata(false));
        public double ValidationMin
        {
            get { return (double)GetValue(ValidationMinProperty); }
            set { SetValue(ValidationMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValidationMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMinProperty =
            DependencyProperty.Register("ValidationMin", typeof(double), typeof(AttitudeField), new PropertyMetadata(0.0));
        public double ValidationMax
        {
            get { return (double)GetValue(ValidationMaxProperty); }
            set { SetValue(ValidationMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMaxProperty =
            DependencyProperty.Register("ValidationMax", typeof(double), typeof(AttitudeField), new PropertyMetadata(0.0));
        public bool RangeValidation
        {
            get { return (bool)GetValue(RangeValidationProperty); }
            set { SetValue(RangeValidationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeValidationProperty =
            DependencyProperty.Register("RangeValidation", typeof(bool), typeof(AttitudeField), new PropertyMetadata(false));

    }
}
