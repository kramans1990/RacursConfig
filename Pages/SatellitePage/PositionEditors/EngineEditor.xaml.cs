using System;
using System.Windows;


namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для EngineEditor.xaml
    /// </summary>
    public partial class EngineEditor : Window
    {   
        public double Pulse { get; set; }
        public double Time { get; set; }
        public RelayCommand OKCommand
        {
            get; set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public EngineEditor(double pulse, double time)
        {
            InitializeComponent();
            this.pulse.Text = pulse.ToString();
            this.time.Text = time.ToString();
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
            Pulse = Convert.ToDouble(pulse.Text);
            Time = Convert.ToDouble(time.Text);           
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (pulse.IsValid && time.IsValid)
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
