using RacursLib.LibMath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace RacursConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для MatrixField.xaml
    /// </summary>
    public partial class MatrixField : UserControl,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private List<NumberField> numberFields;
        public MatrixField()
        {   
            InitializeComponent();
            Loaded += MatrixField_Loaded;
        } 
        public RacursLib.LibMath.Matrix3 Matrix
        {
            get { return (Matrix3)GetValue(MatrixProperty);  }
            set {
                SetValue(MatrixProperty, value);
                OnPropertyChanged(nameof(Matrix));
            }
        }

        // Using a DependencyProperty as the backing store for Matrix.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MatrixProperty =
            DependencyProperty.Register("Matrix", typeof(Matrix3), typeof(MatrixField), new PropertyMetadata(new Matrix3(0,0,0,0,0,0,0,0,0),changed));

        private static void changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
        }

        private void MatrixField_Loaded(object sender, RoutedEventArgs e)
        {
            numberFields = new List<NumberField>
            {
                N11,N12,N13,N21,N22,N23,N31,N32,N33
            };
            foreach (NumberField field in numberFields) {
                field.ValidationMin = ValidationMin;
                field.ValidationMax = ValidationMax;
                field.RangeValidation = RangeValidation;
                field.textValidateField.TextChanged += MatrixChanged;
            }            
        }

        private void MatrixChanged(object sender, TextChangedEventArgs e)
        {
             
            IsValid = numberFields.Find(p => !p.IsValid) == null ? true : false;
            if (IsValid)
            {
                Matrix = new Matrix3(
                     Convert.ToDouble(N11.Text),
                       Convert.ToDouble(N12.Text),
                         Convert.ToDouble(N13.Text),
                           Convert.ToDouble(N21.Text),
                             Convert.ToDouble(N22.Text),
                               Convert.ToDouble(N23.Text),
                                 Convert.ToDouble(N31.Text),
                                   Convert.ToDouble(N32.Text),
                                     Convert.ToDouble(N33.Text)
                    );
            }
            IsValid = RangeValidation ? IsValid : true;
            
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(MatrixField), new PropertyMetadata(false));
        public double ValidationMin
        {
            get { return (double)GetValue(ValidationMinProperty); }
            set { SetValue(ValidationMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValidationMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMinProperty =
            DependencyProperty.Register("ValidationMin", typeof(double), typeof(MatrixField), new PropertyMetadata(0.0));
        public double ValidationMax
        {
            get { return (double)GetValue(ValidationMaxProperty); }
            set { SetValue(ValidationMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMaxProperty =
            DependencyProperty.Register("ValidationMax", typeof(double), typeof(MatrixField), new PropertyMetadata(0.0));
        public bool RangeValidation
        {
            get { return (bool)GetValue(RangeValidationProperty); }
            set { SetValue(RangeValidationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeValidationProperty =
            DependencyProperty.Register("RangeValidation", typeof(bool), typeof(MatrixField), new PropertyMetadata(false));

    }
}
