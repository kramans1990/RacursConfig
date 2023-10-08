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
using System.Windows.Media;

namespace RacursConfig.Pages.SatellitePage
{
  
    public class DeviceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDeviceInstalationModel[] devices = (IDeviceInstalationModel[])(value);
            int param = System.Convert.ToInt32(parameter.ToString());
            var find = devices.ToList().Where(p => p != null).FirstOrDefault(p => p.Slot == param);
            if (find != null)
            {
                if (find.IsEnable)
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
                return new SolidColorBrush(Colors.OrangeRed);
            }
            return new SolidColorBrush(Colors.LightGray);
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
