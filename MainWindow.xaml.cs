
using System.Windows;


namespace RacursConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //System.Globalization.CultureInfo customCultureInfo = System.Globalization.CultureInfo.InvariantCulture;
            //customCultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            //System.Threading.Thread.CurrentThread.CurrentCulture = customCultureInfo;
            DataContext = new MainWindowVM();
        }
    }
}
