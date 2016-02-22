using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Diagnostics;

namespace HMS.Converters
{
	class DateValueConverter : IMultiValueConverter
	{

		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value[0] is DateTime && value[1] is String) {
				DateTime date = (DateTime)value[0];
				DateTime date2;
				if(DateTime.TryParse((string)value[1], out date2)) {
					date.AddHours(date2.Hour);
					date.AddMinutes(date2.Minute);
					date.AddSeconds(date2.Second);
				}
			}
			return value[0];
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
