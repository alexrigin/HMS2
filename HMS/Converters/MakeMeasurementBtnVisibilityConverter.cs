using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using System.Diagnostics;
using HMS.View;


namespace HMS.Converters
{
	class MakeMeasurementBtnVisibilityConverter : IMultiValueConverter
	{

		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{

			if (value[0] != null && value[0] is ParameterControl) {
				ParameterControl pc = (ParameterControl) value[0];
				Debug.WriteLine(pc.GetType().ToString());
				if (!(bool)value[1] && pc.IsDataRelevant)
					return true;
			}
			return false;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}