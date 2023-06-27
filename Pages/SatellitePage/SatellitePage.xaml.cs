using RacursConfig.PagesVM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RacursConfig.Pages.SatellitePage
{
    /// <summary>
    /// Логика взаимодействия для SatellitesPage.xaml
    /// </summary>
    public partial  class SatellitePage : Page
    {
        public SatellitePage()
        {
            InitializeComponent();
            DataContext = new SatellitePageVM();
        }
    }
}
