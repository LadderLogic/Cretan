using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cretan.Views
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null) || !(value is double))
                return null;
            
            var doubleVal = (double)value;
            return doubleVal.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value == null) || !(value is string))
                return null;

            var doubleValue = 0.0;
            double.TryParse((string)value, out doubleValue);
            return doubleValue;
                
        }
    }
}
