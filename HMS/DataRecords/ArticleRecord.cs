using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using HMS.Tools;

namespace HMS.DataRecords
{
    public class ArticleRecord
    {

        private int _id;
        private String _name;
        private String _number;
        private double _nominalHeight;
        private double _minHeight;
        private double _maxHeight;
        private double _nominalSeamerHeight;
        private double _minSeamerHeight;
        private double _maxSeamerHeight;
        private double _nominalDiameter;
        private double _minDiameter;
        private double _maxDiameter;
        

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
        /// Номинальное значение высоты
        /// </summary>
        public double NominalHeight { get { return _nominalHeight; } set { _nominalHeight = value; } }
        /// <summary>
        /// Минимальная высота
        /// </summary>
        public double MinHeight { get { return _minHeight; } set { _minHeight = value; } }
        /// <summary>
        /// Максимальная высота
        /// </summary>
        public double MaxHeight { get { return _maxHeight; } set { _maxHeight = value; } }
        /// <summary>
        /// Номинальное значение высоты закатки
        /// </summary>
        public double NominalSeamerHeight { get { return _nominalSeamerHeight; } set { _nominalSeamerHeight = value; } }
        /// <summary>
        /// Минимальная высота закатки
        /// </summary>
        public double MinSeamerHeight { get { return _minSeamerHeight; } set { _minSeamerHeight = value; } }
        /// <summary>
        /// Максимальная высота закатки
        /// </summary>
        public double MaxSeamerHeight { get { return _maxSeamerHeight; } set { _maxSeamerHeight = value; } }
        /// <summary>
        /// Номинальное значение диаметра
        /// </summary>
        public double NominalDiameter { get { return _nominalDiameter; } set { _nominalDiameter = value; } }
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
        public ArticleRecord(int Id, String Name, String Number, double NominalDiameter, double MinDiametr, double MaxDiametr, double NominalHeight, double MinHeight, double MaxHeight, double NominalSeamerHeight, double MinSeamerHeight, double MaxSeamerHeight)
        {
            _id = Id;
            _name = Name;
            _number = Number;
            _nominalDiameter = NominalDiameter;
            _minDiameter = MinDiametr;
            _maxDiameter = MaxDiametr;
            _nominalHeight = NominalHeight;
            _minHeight = MinHeight;
            _maxHeight = MaxHeight;
            _minSeamerHeight = MinSeamerHeight;
            _maxSeamerHeight = MaxSeamerHeight;
            _nominalSeamerHeight = NominalSeamerHeight;
        }

        public ArticleRecord()
        {

        }

        
        /// <summary>
        /// Возвращает содержимое в строковом виде (нужно доделать)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8};", Id, Name, Number, MinHeight, MaxHeight, MinSeamerHeight, MaxSeamerHeight, MinDiameter, MaxDiameter);
        }

        public string ToSqlString()
        {
            return string.Format(" '{0}', '{1}', {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10};", Name, Number,
                NominalHeight.ToSqlString(), NominalSeamerHeight.ToSqlString(), NominalDiameter.ToSqlString(),
                MaxHeight.ToSqlString(), MaxSeamerHeight.ToSqlString(), MaxDiameter.ToSqlString(),
                MinHeight.ToSqlString(), MinSeamerHeight.ToSqlString(), MinDiameter.ToSqlString());
        }
    }
}
