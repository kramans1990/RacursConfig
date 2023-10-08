using Microsoft.VisualBasic;
using RacursCore;
using RacursCore.SatelliteModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace RacursConfig.Pages.SatellitePage.PositionEditors
{
    /// <summary>
    /// Логика взаимодействия для SatelliteModesEditor.xaml
    /// </summary>
    public partial class SatelliteModesEditor : Window
    {   
        public List<Mode> Modes { get; set; }
        public SatelliteModesEditor(Satellite satellite)
        {
            InitializeComponent();
            Modes = satellite.Modes;
            DataContext = this;
        }
        
        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
