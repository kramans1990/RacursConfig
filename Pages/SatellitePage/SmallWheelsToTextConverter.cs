using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media.Imaging;
using RacursCore.SatilliteComponents;
using RacursCore.SatelliteModel;

namespace RacursConfig.Pages.SatellitePage
{
  
    public class SmallWheelsToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<FlywheelModel> flyWheels = (List<FlywheelModel>)(value);
            int param =  System.Convert.ToInt32(parameter.ToString());
            var find = flyWheels.Find(p => p.Slot == param);
            if (find != null)
            {    
                return find.Name;
            }
            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
