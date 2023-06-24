using RacursLib.LibMath;
using System;
using System.Windows;



namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для VectorEditor.xaml
    /// </summary>
    public partial class FlywheelPositionEditor : Window
    {
        public RacursCore.types.Vector Axis;     

        public double Eta { get; set; }
        public double Gamma { get; set; }
        public RelayCommand OKCommand
        {
            get; set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public FlywheelPositionEditor(RacursCore.types.Vector vector,double eta,double gamma)
        {
            InitializeComponent();
            Axis = vector;
            Eta =  eta;
            Gamma = gamma;
            etaField.Text = eta.ToString();
            GammaField.Text = gamma.ToString();
            x.Text = Axis.X.ToString();
            y.Text = Axis.Y.ToString();
            z.Text = Axis.Z.ToString();
            OKCommand = new RelayCommand(p => OK(), p=>canOk());
            CancelCommand = new RelayCommand(p => Cancel());

            DataContext = this;
           
        }

        private void Cancel()
        {
            this.Close();
        }

        private void OK()
        {
            Axis.X = Convert.ToDouble(x.Text);
            Axis.Y = Convert.ToDouble(y.Text);
            Axis.Z = Convert.ToDouble(z.Text);
            Eta = Convert.ToDouble(etaField.Text);
            Gamma = Convert.ToDouble(GammaField.Text);
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (x.IsValid && y.IsValid && z.IsValid && etaField.IsValid && GammaField.IsValid)
            {
                return  true;
            }
            else
            {
                return false;
            }
        }
   

 

    }
}
