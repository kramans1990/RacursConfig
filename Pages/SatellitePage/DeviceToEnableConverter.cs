using RacursCore.SatelliteModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RacursConfig.Pages.SatellitePage
{
    internal class DeviceToEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDeviceInstalationModel[] devices = (IDeviceInstalationModel[])(value);
            int param = System.Convert.ToInt32(parameter.ToString());
            var find = devices.ToList().Where(p => p != null).FirstOrDefault(p => p.Slot == param);
            if (find != null)
            {
               return find.IsEnable;
            }
            return false;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
