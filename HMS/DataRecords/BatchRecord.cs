using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SQLite;
using HMS.Managers;
using HMS.DataProviders;
using HMS.DataVirtualization;

namespace HMS.DataRecords
{
    /// <summary>
    /// Модель данных для отображения в UI (DataGrid, ListView, etc.)
    /// </summary>
    public class BatchRecord
    {

        protected int? _id;
        protected int? _articleId;
        protected string _articleName;
        protected string _articleNumber;
        protected DateTime _date;
        protected int? _batchNumber;
        protected IList<MeasurementRecord> _measurements;
        //private MeasurementsProvider _measuremetnsProvider;
        //AsyncVirtualizingCollection<MeasurementRecord> _measurementsCollection;
        protected bool _isMeasurementsLoaded = false; 


        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int? Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Индентификатор артикула
        /// </summary>
        public int? ArticleId { get { return _articleId; } set { _articleId = value; } }
        /// <summary>
        /// Дата замера
        /// </summary>
        public DateTime Date { get { return _date; } set { _date = value; } }
        /// <summary>
        /// Название артикула
        /// </summary>
        public string ArticleName { get { return _articleName; } set { _articleName = value; } }
        /// <summary>
        /// Номер артикула
        /// </summary>
        public string ArticleNumber { get { return _articleNumber; } set { _articleNumber = value; } }
        /// <summary>
        /// Номер партии
        /// </summary>
        public int? BatchNumber { get { return _batchNumber; } set { _batchNumber = value; } }
        /// <summary>
        /// Список измерений
        /// </summary>
        public IList<MeasurementRecord> Measurements
        {
            get
            {
                if (!_isMeasurementsLoaded) // данные загружаются только один раз
                {
                    //_measuremetnsProvider = new MeasurementsProvider(50, 0, BatchNumber);
                    //_measurementsCollection = new AsyncVirtualizingCollection<MeasurementRecord>(_measuremetnsProvider, 1000, 3000);
                     _measurements = DataManager.ExecuteToList<MeasurementRecord>(string.Format("SELECT h,htime,sh,shtime,d,dtime from measurements where batchnumber={0};",BatchNumber), 
                         new SQLiteConnection(Properties.Settings.Default.DBConnectionString), DataManager.ReadMeasurement);
                    _isMeasurementsLoaded = true;
                }
                return _measurements;
            }
            set
            {
                _measurements = value;
            }
        }

    }
}
