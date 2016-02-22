using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataRecords
{
    /// <summary>
    /// Модель данных для работы с базой данных
    /// </summary>
    class SQLMeasurementRecord : MeasurementRecord
    {
        protected int _id;
        protected int _articleId;
        protected int _batchNumber;
        protected DateTime _date;

        /// <summary>
        /// идентификатор измерения
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// идентификатор артикула
        /// </summary>
        public int ArticleId { get { return _articleId; } set { _articleId = value; } }
        /// <summary>
        /// номер партии
        /// </summary>
        public int BatchNumber { get { return _batchNumber; } set { _batchNumber = value; } }
        /// <summary>
        /// Дата начала замера
        /// </summary>
        public DateTime Date { get { return _date; } set { _date = value; } }


        public SQLMeasurementRecord()
        {

        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9} );", Id, ArticleId, BatchNumber, Date, Height, HTime, SeamerHeight, SHTime, Diameter, DTime);
        }

        public string ToSqlString() // переделать
        {
            //return string.Format("{0}, {1}, {2}, '{3}', {4}, '{5}', {6}, '{7}', {8}, '{9}';", Id.ToString(), ArticleId.ToString(), BatchNumber.ToString(),
            //    Date.ToString(CultureInfo.GetCultureInfo("en-US")),
            //    Height.ToSqlString(), HTime, SeamerHeight.ToSqlString(), SHTime, Diameter.ToSqlString(), DTime);

            //return string.Format("{0}, '{1}', {2}, '{3}', {4}, '{5}';", Height.ToSqlString(), HTime, SeamerHeight.ToSqlString(), SHTime, Diameter.ToSqlString(), DTime);
            return "";
        }
    }
}
