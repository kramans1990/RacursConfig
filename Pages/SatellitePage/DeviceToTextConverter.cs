using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using RacursCore.SatelliteModel;

namespace RacursConfig.Pages.SatellitePage
{
  
    public class DeviceToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDeviceInstalationModel [] devices = (IDeviceInstalationModel[])(value);
            int param = System.Convert.ToInt32(parameter.ToString());
            var find = devices.ToList().Where(p=>p!=null).FirstOrDefault(p => p.Slot == param);
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
