using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HMS.DataRecords
{
    public class ArticleRecord
    {
        //private String _fileName; // необходимо, если данные хранятся в файле
        private int _id = -1;
        private String _name;
        private String _number;
        private double _minHeight;
        private double _maxHeight;
        private double _minSeamerHeight;
        private double _maxSeamerHeight;
        private double _minDiameter;
        private double _maxDiameter;

        //public String FileName { get { return _fileName; } set { _fileName = value; } }
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Название артикула
        /// </summary>
        public String Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// Номер артикула
        /// </summary>
        public String Number { get { return _number; } set { _number = value; } }
        /// <summary>
        /// Минимальная высота
        /// </summary>
        public double MinHeight { get { return _minHeight; } set { _minHeight = value; } }
        /// <summary>
        /// Максимальная высота
        /// </summary>
        public double MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }
        /// <summary>
        /// Минимальная высота закатки
        /// </summary>
        public double MinSeamerHeight { get { return _minSeamerHeight; } set { _minSeamerHeight = value; } }
        /// <summary>
        /// Максимальная высота закатки
        /// </summary>
        public double MaxSeamerHeight { get { return _maxSeamerHeight; } set { _maxSeamerHeight = value; } }
        /// <summary>
        /// Минимальный диаметр
        /// </summary>
        public double MinDiameter { get { return _minDiameter; } set { _minDiameter = value; } }
        /// <summary>
        /// Максимальный диаметр
        /// </summary>
        public double MaxDiameter { get { return _maxDiameter; } set { _maxDiameter = value; } }
        /// <summary>
        /// Инициализирует новый объект ArticleRecord
        /// </summary>
        /// <param name="Id">Идентификатор записи</param>
        /// <param name="Name">Название артикула</param>
        /// <param name="Number">Номер артикула</param>
        /// <param name="MinDiametr">Минимальный диаметр</param>
        /// <param name="MaxDiametr">Максимальный диаметр</param>
        /// <param name="MinHeight">Минимальная высота</param>
        /// <param name="MaxHeight">Максимальная высота</param>
        /// <param name="MinSeamerHeight">Минимальная высота закатки</param>
        /// <param name="MaxSeamerHeight">Максимальная высота закатки</param>
        public ArticleRecord(int Id, String Name, String Number, double MinDiametr, double MaxDiametr, double MinHeight, double MaxHeight, double MinSeamerHeight, double MaxSeamerHeight)
        {
            _id = Id;
            _name = Name;
            _number = Number;
            _minDiameter = MinDiametr;
            _maxDiameter = MaxDiametr;
            _minHeight = MinHeight;
            _maxHeight = MaxHeight;
            _minSeamerHeight = MinSeamerHeight;
            _maxSeamerHeight = MaxSeamerHeight;
        }

        /// <summary>
        /// Инициализирует новый объект ArticleRecord
        /// </summary>
        /// <param name="Name">Название артикула</param>
        /// <param name="Number">Номер артикула</param>
        /// <param name="MinDiametr">Минимальный диаметр</param>
        /// <param name="MaxDiametr">Максимальный диаметр</param>
        /// <param name="MinHeight">Минимальная высота</param>
        /// <param name="MaxHeight">Максимальная высота</param>
        /// <param name="MinSeamerHeight">Минимальная высота закатки</param>
        /// <param name="MaxSeamerHeight">Максимальная высота закатки</param>
        public ArticleRecord(String Name, String Number, double MinDiametr, double MaxDiametr, double MinHeight, double MaxHeight, double MinSeamerHeight, double MaxSeamerHeight) :
            this(-1,Name,Number,MinDiametr,MaxDiametr,MinHeight,MaxHeight,MinSeamerHeight,MaxSeamerHeight)
        {
            
        }

        public override string ToString()
        {
            return _id.ToString() + ", " + _name + ", " + _number + ", " + _minDiameter.ToString() + ", " + _maxDiameter.ToString() + ", " + _minHeight.ToString() + ", " + _maxHeight.ToString() + ", "
                + _minSeamerHeight.ToString() + ", " + _maxSeamerHeight.ToString() + ";";
        }

        public string ToSqlString()
        {
            return "'"+ _name + "' , '" + _number + "' , " 
                +  _minDiameter.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) + ", " + _maxDiameter.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) + ", " 
                + _minHeight.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) + ", " + _maxHeight.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) + ", "
                + _minSeamerHeight.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) + ", " + _maxSeamerHeight.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat);
        }
    }
}
