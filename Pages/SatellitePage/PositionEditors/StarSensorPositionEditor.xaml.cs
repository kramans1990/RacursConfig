using RacursCore.types;
using System;
using System.Windows;


namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для StarSensorPositionEditor.xaml
    /// </summary>
    public partial class StarSensorPositionEditor : Window
    {

        public Attitude Att { get; set; }

        public RelayCommand OKCommand
        {
            get; set;
        }
        public StarSensorPositionEditor(Attitude att)
        {
            Att = att;
            InitializeComponent();
            x.Text = att.X.ToString();
            y.Text = att.Y.ToString();
            z.Text = att.Z.ToString();
            w.Text = att.W.ToString();
            OKCommand = new RelayCommand(p => OK(), p => canOk());
            DataContext = this;

        }

        private void OK()
        {
            Att.X = Convert.ToDouble(x.Text);
            Att.Y = Convert.ToDouble(y.Text);
            Att.Z = Convert.ToDouble(z.Text);
            Att.W = Convert.ToDouble(w.Text);
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (x.IsValid && y.IsValid && z.IsValid && w.IsValid)


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