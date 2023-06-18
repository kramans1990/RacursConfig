using RacursCore.types;
using System;
using System.Windows;
using Vector = RacursCore.types.Vector;

namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для LoadEitor.xaml
    /// </summary>
    public partial class LoadEditor : Window
    {
        public Vector LoadM1 { get; set; }
        public Vector LoadM2 { get; set; }
        public RelayCommand OKCommand
        {
            get; set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public LoadEditor(Vector M1, Vector M2)
        {
            InitializeComponent();
            this.x1.Text = M1.X.ToString();
            this.y1.Text = M1.Y.ToString();
            this.z1.Text = M1.Z.ToString();
            this.x2.Text = M2.X.ToString();
            this.y2.Text = M2.Y.ToString();
            this.z2.Text = M2.Z.ToString();
            LoadM1 = new Vector();
            LoadM2 = new Vector();
            DataContext = this;
            OKCommand = new RelayCommand(p => OK(), p => canOk());
            CancelCommand = new RelayCommand(p => Cancel());
        }

        private void Cancel()
        {
            this.Close();
        }

        private void OK()
        {
            LoadM1.X = Convert.ToDouble(x1.Text);
            LoadM1.Y = Convert.ToDouble(y1.Text);
            LoadM1.Z = Convert.ToDouble(z1.Text);

            LoadM2.X = Convert.ToDouble(x2.Text);
            LoadM2.Y = Convert.ToDouble(y2.Text);
            LoadM2.Z = Convert.ToDouble(z2.Text);           
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (x1.IsValid && y1.IsValid && z1.IsValid 
                && x2.IsValid && y2.IsValid && z2.IsValid
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}