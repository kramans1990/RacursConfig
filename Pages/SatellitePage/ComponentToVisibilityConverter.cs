using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;


namespace RacursConfig.Pages.SatellitePage
{
  
    public class ComponentToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string param = "";
            param = parameter == null ? param : parameter.ToString();

            if (value == null) {
                 return param == "invert" ? Visibility.Visible : Visibility.Hidden;
            }
             return param == "invert" ? Visibility.Hidden : Visibility.Visible; 
         
            

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
