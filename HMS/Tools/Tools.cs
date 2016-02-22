using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace HMS.Tools
{
    static class Converter
    {

        /// <summary>
        /// Преобразует string в double (независимая локаль)
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            if (!str.Equals(string.Empty))
                return double.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture);
            else
                return 0;
        }

        public static double? ToNullableDouble(this string str)
        {
            if (!str.Equals(string.Empty))
                return Convert.ToDouble(str);
            //return double.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture);
            else
                return null;
        }

        public static string ToSqlString(this DateTime date)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}";
            return string.Format(dateTimeFormat, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public static string ToSqlString(this double value)
        {
			Debug.WriteLine("value"+value);
			string val = value.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat);
			Debug.WriteLine("value2" + val);



			return val;
        }

        public static string ToSqlString(this double? value)
        {
            return  value.ToString();
        }


		public static bool ReturnRefOfBool(ref bool value)
		{
			return value;
		}
    }



    /// <summary>
    /// Класс для создания SELECT запроса к БД
    /// </summary>
    public class SQLRequest
    {
        string _fields;
        string _fromPart;
        string _wherePart;
        string _limitPart;
        string _sortPart;
        string _groupByPart;
        private IList<string> _filters = new List<string>();

        public SQLRequest(string Fields, string FromPart, string WherePart, string SortPart, string GroupByPart, string LimitPart)
        {
            _fields = Fields;
            _fromPart = FromPart;
            _wherePart = WherePart;
            _limitPart = LimitPart;
            _sortPart = SortPart;
            _groupByPart = GroupByPart;
        }

        //public SQLRequest(string Fields, string FromPart, string WherePart, string SortPart) :
        //    this(Fields, FromPart, WherePart, SortPart, "","")
        //{
        //
        //}
        //public SQLRequest(string Fields, string FromPart, string WherePart) :
        //    this(Fields, FromPart, WherePart, "", "")
        //{
        //
        //}
        //public SQLRequest(string Fields, string FromPart) :
        //    this(Fields, FromPart, "", "", "")
        //{
        //
        //}


        public string Fields { get { return _fields; } set { _fields = value; } }
        public string FromPart { get { return _fromPart; } set { _fromPart = value; } }
        public string WherePart
        {
            get
            {
                return FindWherePart();
            }
            set { _wherePart = value; }
        }
        public string LimitPart { get { return _limitPart; } set { _limitPart = value; } }
        public string SortPart { get { return _sortPart; } set { _sortPart = value; } }
        public string GroupByPart { get { return _groupByPart; } set { _groupByPart = value; } }

        public string SqlRequestString()
        {
            string request = string.Format("SELECT {0} FROM {1} {2} {3} {4} {5};", Fields, FromPart, WherePart, SortPart, GroupByPart, LimitPart); ;
            return request;
        }

        public string FindWherePart()
        {
            string str="";
            if (_wherePart.Equals(string.Empty))
            {
                if (_filters.Count() == 0)
                    return "";
                else
                {
                    str += "WHERE " + _filters[0];
                    for (int i = 1; i < _filters.Count(); i++)
                        str += " AND " + _filters[i];
                }
            }
            else
            {
                str += "WHERE " + _wherePart;
                if (_filters.Count() != 0)
                {
                    foreach (object o in _filters)
                        str += " AND " + o;
                }
            }
            return str;
        }

        public void SetLimit(int FromIndex, int ToIndex)
        {
            _limitPart = string.Format("LIMIT {0},{1}",FromIndex,ToIndex);    
        }

        public void ResetLimit()
        {
            LimitPart = "";
        }
        public void AddFilter(string filter)
        {
            if (!filter.Equals(string.Empty) && !_filters.Contains(filter))
            {
                _filters.Add(filter);
            }
        }
        public void RemoveFilter(string filter)
        {
            if (!filter.Equals(string.Empty) && _filters.Contains(filter))
            {
                _filters.Remove(filter);
            }
        }
    }
}

