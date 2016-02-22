using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using HMS.Tools;

namespace HMS.DataRecords
{
    /// <summary>
    /// Модель данных для отображения в UI (DataGrid, ListView, etc.)
    /// </summary>
    public class MeasurementRecord
    {
        protected double? _height;
        protected DateTime? _hTime;
        protected double? _seamerHeight;
        protected DateTime? _shTime;
        protected double? _diameter;
        protected DateTime? _dTime;
        
       
        /// <summary>
        /// Высота
        /// </summary>
        public double? Height { get { return _height; } set { _height = value; } }
        /// <summary>
        /// Время замера высоты
        /// </summary>
        public DateTime? HTime { get { return _hTime; } set { _hTime = value; } }
        /// <summary>
        /// Высота закатки
        /// </summary>
        public double? SeamerHeight { get { return _seamerHeight; } set { _seamerHeight = value; } }
        /// <summary>
        /// Время замера высоты закатки
        /// </summary>
        public DateTime? SHTime { get { return _shTime; } set { _shTime = value; } }
        /// <summary>
        /// Диаметр
        /// </summary>
        public double? Diameter { get { return _diameter; } set { _diameter = value; } }
        /// <summary>
        /// Время замера диаметра
        /// </summary>
        public DateTime? DTime { get { return _dTime; } set { _dTime = value; } }


        public MeasurementRecord()
        {

        }

        /// <summary>
        /// Инициализация нового объекта класса MeasurementRecord
        /// </summary>
        /// <param name="height">Высота</param>
        /// <param name="hTime">Время замера высоты</param>
        /// <param name="seamerHeight">Высота закатки</param>
        /// <param name="shTime">Время замера высоты закатки</param>
        /// <param name="diameter">Диаметр</param>
        /// <param name="dTime">Время замера диаметра</param>
        public MeasurementRecord(double? height, DateTime hTime, double? seamerHeight, DateTime shTime, double? diameter, DateTime dTime)
        {
            Height = height;
            HTime = hTime;
            SeamerHeight = seamerHeight;
            SHTime = shTime;
            Diameter = diameter;
            DTime = dTime;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5} );",Height,HTime,SeamerHeight,SHTime,Diameter,DTime);
        }

    }
}
