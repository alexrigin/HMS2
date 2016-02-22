using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace HMS.Converters
{
    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]
    public class VisbilityToBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			bool bValue = (bool)value;
			if (bValue)
				return Visibility.Visible;
			else
				return Visibility.Hidden;

		}

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			Visibility visibility = (Visibility)value;

			if (visibility == Visibility.Visible)
				return true;
			else
				return false;
		}

        #endregion
    }
}
