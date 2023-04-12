using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Vector = RacursLib.LibMath.Vector;

namespace RacursConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для VectorField.xaml
    /// </summary>
    public partial class VectorField : UserControl
    {
        private List<NumberField> numberFields;
        public VectorField()
        {
            InitializeComponent();
            Loaded += VectorField_Loaded;
        }

        private void VectorField_Loaded(object sender, RoutedEventArgs e)
        {
            numberFields = new List<NumberField>
            {
               X,Y,Z
            };
            foreach (NumberField field in numberFields)
            {
                field.ValidationMin = ValidationMin;
                field.ValidationMax = ValidationMax;
                field.RangeValidation = RangeValidation;
                field.textValidateField.TextChanged += VectorChanged;
            }
        }

        private void VectorChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = numberFields.Find(p => !p.IsValid) == null ? true : false;
            IsValid = RangeValidation ? IsValid : true;
        }

        public Vector Vector
        {
            get { return (Vector)GetValue(VectorProperty); }
            set { SetValue(VectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Vector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VectorProperty =
            DependencyProperty.Register("Vector", typeof(Vector), typeof(VectorField), new PropertyMetadata(new Vector(0, 0, 0)));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsValid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(VectorField), new PropertyMetadata(false));
        public double ValidationMin
        {
            get { return (double)GetValue(ValidationMinProperty); }
            set { SetValue(ValidationMinProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ValidationMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMinProperty =
            DependencyProperty.Register("ValidationMin", typeof(double), typeof(VectorField), new PropertyMetadata(0.0));
        public double ValidationMax
        {
            get { return (double)GetValue(ValidationMaxProperty); }
            set { SetValue(ValidationMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationMaxProperty =
            DependencyProperty.Register("ValidationMax", typeof(double), typeof(VectorField), new PropertyMetadata(0.0));
        public bool RangeValidation
        {
            get { return (bool)GetValue(RangeValidationProperty); }
            set { SetValue(RangeValidationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RangeValidation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeValidationProperty =
            DependencyProperty.Register("RangeValidation", typeof(bool), typeof(VectorField), new PropertyMetadata(false));
    }
}