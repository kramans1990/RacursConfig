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

namespace RacursConfig.Pages
{
    /// <summary>
    /// Логика взаимодействия для SunSensorPage.xaml
    /// </summary>
    public partial class SunSensorPage : Page
    {
        public SunSensorPage()
        {
            InitializeComponent();
            DataContext = new SunSensorPageVM();
        }
    }
}
