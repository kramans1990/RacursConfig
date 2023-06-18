using RacursConfig.PagesVM;
using RacursCore;
using System.Windows;

namespace RacursConfig.Pages.SatellitePage
{

    public partial class SatellitteComponetsEditor : Window
    {
        public Satellite satellite;
        public SatellitteComponetsEditor(Satellite satellite)
        {
            InitializeComponent();
            DataContext = new SatelliteComponentEditorVM(satellite);
        }
    }
}
