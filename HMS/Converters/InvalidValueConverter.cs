using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using HMS.DataRecords;
using System.Diagnostics;


namespace HMS.Converters
{
    class InvalidValueConverter : IMultiValueConverter
    {

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Length != 3) return true;

            if (value[0] != null && value[1] != null && value[2]!=null)
            {
				if((value[0] is double) && (value[1] is double) && (value[2] is double)) {
					double curValue = (double) value[0];
					double minValue = (double)value[1];
					double maxValue = (double)value[2];
					if ((curValue < minValue) || curValue > maxValue)
						return true;
				}
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
