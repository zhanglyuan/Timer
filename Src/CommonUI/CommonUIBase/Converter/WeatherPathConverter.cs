using Common.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CommonUIBase.Converter
{
    public class WeatherPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string? Weather = value as string;
                if (!string.IsNullOrEmpty(Weather))
                {
                    return new Uri(Weather);
                }
                return new Uri(GlobalSettings.DefaultSvgUrl);
            }
            catch (Exception)
            {
                return new Uri(GlobalSettings.DefaultSvgUrl);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}