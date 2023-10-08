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
        public double Pull { get; set; }
        public double PullUnit { get; set; }
        public RelayCommand OKCommand
        {
            get; set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public EngineEditor(double pulse, double time,double pull,double pullUnit)
        {
            InitializeComponent();
            this.pulse.Text = pulse.ToString();
            this.time.Text = time.ToString();
            this.pull.Text = pull.ToString();
            this.pullUnit.Text = pullUnit.ToString();
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
            Pull = Convert.ToDouble(pull.Text);
            PullUnit= Convert.ToDouble(pullUnit.Text);
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (pulse.IsValid && time.IsValid && pull.IsValid && pullUnit.IsValid)
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
