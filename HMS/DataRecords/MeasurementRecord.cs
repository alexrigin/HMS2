using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HMS.DataRecords
{
    public class MeasurementRecord
    {
        private int _id = -1;
        private int _batchId = -1;
        private int _number = 0;
        private double _height;
        private DateTime _date_h;
        private double _seamerHeight;
        private DateTime _date_sh;
        private double _diameter;
        private DateTime _date_d;
        
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Идентификатор партии
        /// </summary>
        public int BatchIdId { get { return _batchId; } set { _batchId = value; } }
        /// <summary>
        /// Номер замера
        /// </summary>
        public int Number { get { return _number; } set { _number = value; } }
        /// <summary>
        /// Высота
        /// </summary>
        public double Height { get { return _height; } set { _height = value; } }
        /// <summary>
        /// Дата замера высоты
        /// </summary>
        public DateTime DateH { get { return _date_h; } set { _date_h = value; } }
        /// <summary>
        /// Высота закатки
        /// </summary>
        public double SeamerHeight { get { return _seamerHeight; } set { _seamerHeight = value; } }
        /// <summary>
        /// Дата замера высоты закатки
        /// </summary>
        public DateTime DateSH { get { return _date_sh; } set { _date_sh = value; } }
        /// <summary>
        /// Диаметр
        /// </summary>
        public double Dameter { get { return _diameter; } set { _diameter = value; } }
        /// <summary>
        /// Дата замера диаметра
        /// </summary>
        public DateTime DateD { get { return _date_d; } set { _date_d = value; } }



        public MeasurementRecord(int Id, int BatchId, int Number, double Height, DateTime DataH, double SeamerHeight, DateTime DataSH, double Diameter, DateTime DataD)
        {
            _id = Id;
            _batchId = BatchId;
            _number = Number;
            _height = Height;
            _date_h = DataH;
            _seamerHeight = SeamerHeight;
            _date_sh = DateSH;
            _diameter = Diameter;
            _date_d = DataD;
        }

        public MeasurementRecord()
        {

        }

        public override string ToString()
        {
            //return string.Format("{0},{1},{2},{3};",Id,ArticleId,ArticleName,Date.ToString());
            return "asd";
        }

        public string ToSqlString()
        {
            //return string.Format(string.Format("{0},{1},'{2}','{3}';", Id, ArticleId, ArticleName, Date.ToString(CultureInfo.GetCultureInfo("en-US"))));
            return "asd";
        }
    }
}
