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
    /// Логика взаимодействия для ARSEditor.xaml
    /// </summary>
    public partial class ARSPositionEditor : Window
    {
        public RacursCore.types.Vector Axis;


        public RelayCommand OKCommand
        {
            get; set;
        }
        public ARSPositionEditor(RacursCore.types.Vector vector)
        {
            InitializeComponent();
            Axis = vector;
            x.Text = Axis.X.ToString();
            y.Text = Axis.Y.ToString();
            z.Text = Axis.Z.ToString();
            OKCommand = new RelayCommand(p => OK(), p => canOk());
            DataContext = this;

        }

        private void OK()
        {
            Axis.X = Convert.ToDouble(x.Text);
            Axis.Y = Convert.ToDouble(y.Text);
            Axis.Z = Convert.ToDouble(z.Text);
            this.DialogResult = true;
        }

        private bool canOk()
        {
            if (x.IsValid && y.IsValid && z.IsValid)
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

