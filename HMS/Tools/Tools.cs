using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HMS.Tools
{
    static class Converter
    {
        //public static double ToDouble(string value)
        //{
        //    double number;
        //
        //    // Return if string is empty
        //    if (String.IsNullOrEmpty(value))
        //        return -1;
        //
        //
        //    // Instantiate CultureInfo object for the user's locale
        //    string culture = CultureInfo.CurrentCulture.Name;
        //
        //    // Convert user input from a string to a number
        //    try
        //    {
        //        number = Double.Parse(value, CultureInfo.InvariantCulture);
        //    }
        //    catch (FormatException)
        //    {
        //        number = Double.Parse(value, CultureInfo.InvariantCulture);
        //    }
        //    catch (OverflowException)
        //    {
        //        return -1;
        //    }
        //    return number;
        //}

        /// <summary>
        /// Преобразует string в double (независимая локаль)
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return double.Parse(str.Replace(",", "."), CultureInfo.InvariantCulture);
        }

        public static string ToSqlString(this DateTime date)
        {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}";
            return string.Format(dateTimeFormat, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

    }
    /// <summary>
    /// Класс для создания запроса к базе данных
    /// </summary>
    public class SQLRequest
    {
        string _fields;
        string _fromPart;
        string _wherePart;
        string _limitPart;
        string _sortPart;
        private IList<string> _filters = new List<string>();

        public SQLRequest(string Fields, string FromPart, string WherePart, string SortPart, string LimitPart)
        {
            _fields = Fields;
            _fromPart = FromPart;
            _wherePart = WherePart;
            _limitPart = LimitPart;
            _sortPart = SortPart;
        }

        public SQLRequest(string Fields, string FromPart, string WherePart, string SortPart) :
            this(Fields, FromPart, WherePart, SortPart, "")
        {

        }
        public SQLRequest(string Fields, string FromPart, string WherePart) :
            this(Fields, FromPart, WherePart, "", "")
        {

        }
        public SQLRequest(string Fields, string FromPart) :
            this(Fields, FromPart, "", "", "")
        {

        }


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

        public string SqlRequestString()
        {
            string request = string.Format("SELECT {0} FROM {1} {2} {3} {4};", Fields, FromPart, WherePart, SortPart, LimitPart); ;
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
            _limitPart = "";
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

