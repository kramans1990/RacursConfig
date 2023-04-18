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
    /// Логика взаимодействия для FlyWheelPage.xaml
    /// </summary>
    public partial class FlyWheelPage : Page
    {
        public FlyWheelPage()
        {
            InitializeComponent();
            DataContext = new FlywheelPageVM();
        }
    }
}
