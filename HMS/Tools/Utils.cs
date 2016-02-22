using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Tools
{
	static class Utils
	{
		public static bool IsDouble(this string value)
		{
			try { 
			double val = value.ToDouble();
				return true;
			} catch(FormatException ex) {
				return false;
			}

		}
	}
}
