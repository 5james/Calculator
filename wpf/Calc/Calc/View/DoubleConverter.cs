using System;
using System.Globalization;
using System.Windows.Data;

namespace Calc.View
{
    [ValueConversion(typeof(object), typeof(double))]
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dblValue = (double)value;
            string scaleString = parameter as string;
            var cultureInfo = CultureInfo.InvariantCulture;
            double scale = Double.Parse(scaleString, cultureInfo);
            return dblValue * scale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}