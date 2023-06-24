using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для GyroPositionEditor.xaml
    /// </summary>
    public partial class GyroPositionEditor : Window
    {
        public RacursCore.types.Vector AxisG;
        public RacursCore.types.Vector AxisR;

        public double Theta { get; set; }
        public double Lambda { get; set; }
        public RelayCommand OKCommand
        {
            get; set;
        }
        public RelayCommand CancelCommand
        {
            get; set;
        }
        public GyroPositionEditor(RacursCore.types.Vector axisG, RacursCore.types.Vector axisR, double teta, double lambda)
        {
            InitializeComponent();
            AxisG = axisG;
            AxisR = axisR;          
            Lambda = lambda;
            etaField.Text = teta.ToString();
            GammaField.Text = lambda.ToString();
            gx.Text = axisG.X.ToString();
            gy.Text = axisG.Y.ToString();
            gz.Text = axisG.Z.ToString();
            rx.Text = axisR.X.ToString();
            ry.Text = axisR.Y.ToString();
            rz.Text = axisR.Z.ToString();
            OKCommand = new RelayCommand(p => OK(), p => canOk());
            CancelCommand = new RelayCommand(p => Cancel());
            DataContext = this;

        }

        private void Cancel()
        {
            this.Close();
        }

        private void OK()
        {
            AxisG.X = Convert.ToDouble(gx.Text);
            AxisG.Y = Convert.ToDouble(gy.Text);
            AxisG.Z = Convert.ToDouble(gz.Text);
            AxisR.X = Convert.ToDouble(gx.Text);
            AxisR.Y = Convert.ToDouble(gy.Text);
            AxisR.Z = Convert.ToDouble(gz.Text);
            Theta = Convert.ToDouble(etaField.Text);
            Lambda = Convert.ToDouble(GammaField.Text);
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (gx.IsValid && gy.IsValid && gz.IsValid && etaField.IsValid && GammaField.IsValid 
                && rx.IsValid && ry.IsValid && rz.IsValid)
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

