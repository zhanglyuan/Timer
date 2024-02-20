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
                    DrawingGroup pathGeometry = (DrawingGroup)Application.Current.Resources[Weather];
                    return pathGeometry;
                }
                return (DrawingGroup)Application.Current.Resources["未知"];
            }
            catch (Exception ex)
            {
                return (DrawingGroup)Application.Current.Resources["未知"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}