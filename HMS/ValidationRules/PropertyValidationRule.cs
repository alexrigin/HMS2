using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using HMS.Tools;

namespace HMS.ValidationRules 
{
	class PropertyValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			try {
				string s = (string)value;
				if(s.Equals(string.Empty))
					return new ValidationResult(true, "Данное поле не может быть пустым");

				double val = s.ToDouble();
				return new ValidationResult(true, null);
															 				
			}	catch(FormatException ex) {
				return new ValidationResult(false, "Вводимое значение должно быть числом");
			}
		}
	}
}
