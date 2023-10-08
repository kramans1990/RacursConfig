using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using RacursCore.SatelliteModel;
using System.Linq;

namespace RacursConfig.Pages.SatellitePage
{
  
    public class DeviceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDeviceInstalationModel[] devices = (IDeviceInstalationModel[])(value);
            if (parameter == null) { parameter = -1; }
            int param = System.Convert.ToInt32(parameter.ToString());
            int index = Math.Abs(param);
            var find = devices.ToList().Where(p => p != null).FirstOrDefault(p => p.Slot == index);
            if (find == null) { 
               return param > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            if (find != null)
            {   
                return param > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return true;

            //int index = Math.Abs(param);
            //var find = devices.ToList().Where(p => p != null).FirstOrDefault(p => p.Slot == param);
            //if (find != null)
            //{
            //    if (param > 0)
            //    {
            //        return Visibility.Visible;
            //    }
            //    return Visibility.Collapsed;
            //}
            //else {
            //    if (param < 0)
            //    {
            //        return Visibility.Collapsed;
            //    }
            //    return Visibility.Visible;
            //}
            // return "";



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
