using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SQLite;
using HMS.DataVirtualization;
using HMS.DataRecords;
using HMS.Managers;
using HMS.Tools;
using HMS.SQLQuery;


namespace HMS.DataProviders
{
    class BatchesProvider : IItemsProvider<BatchRecord>
    {
        private int _count;
        private readonly int _fetchDelay;
        private string _dbConnectionString = Properties.Settings.Default.DBConnectionString;
		private WhereStatement _whereBuffer;
        private SQLSelectQuery _query = new SQLSelectQuery();
		private SQLSelectQuery _countQuery;

		public SQLSelectQuery Query { get { return _query; } }

		public BatchesProvider(int count, int fetchDelay)
        {
            _count = count;
            _fetchDelay = fetchDelay;
			//_sqlRequest = new SQLRequest("m.id,m.article,m.batchnumber,m.date,m.h,m.htime,m.sh,m.shtime,m.d,m.dtime","articles a, measurements m","m.article=a.id","","");
			_query.SelectColumns("m.id, m.article, a.name, a.number, m.date, m.batchnumber");
			_query.SelectFromTables("articles a", "measurements m");
			_query.AddWhere("m.article", Comparison.Equals, new SqlLiteral("a.id"));
			_query.AddGroupByColumn("m.batchNumber");
			_query.BuildQuery();

			_countQuery = new SQLSelectQuery();
			_countQuery.SelectColumn("count(distinct m.batchnumber)");
			_countQuery.SelectedTables = _query.SelectedTables;
			_countQuery.Where = _query.Where;
			//_countQuery.SelectedTables = _query.SelectedTables;
			//this.GroupBy = query.GroupBy;
			//_countQuery.Having = query.Having;
			_countQuery.OrderBy = _query.OrderBy;
			_countQuery.Limit = _query.Limit;
			_countQuery.BuildQuery();

			Debug.WriteLine("_query.Query");
			Debug.WriteLine(_query.Query);
			Debug.WriteLine("_countQuery.Query");
			Debug.WriteLine(_countQuery.Query);
		}

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            Trace.WriteLine("FetchCount");
			_countQuery.BuildQuery();
			_count = Convert.ToInt32(DBManager.ExecuteScalar(_countQuery.Query, new SQLiteConnection(_dbConnectionString)));
            Debug.WriteLine("count=" + _count);
            return _count;
        }

        public IList<BatchRecord> FetchRange(int startIndex, int pageCount, out int overallCount)
        {
            Trace.WriteLine("FetchRange:" + startIndex + "," + pageCount);
            overallCount = _count;
            //startIndex += 1; // sql starts from 1

            int EndIndex = startIndex + pageCount;
			_query.AddLimit(startIndex, EndIndex);
			_query.BuildQuery();

			//Debug.WriteLine("_countQuery.Query");
			//Debug.WriteLine(_countQuery.Query);
            return DataManager.ExecuteToList<BatchRecord>(_query.Query, new SQLiteConnection(_dbConnectionString), DataManager.ReadBatch);
        }

        /// <summary>
        /// Pretend to insert an item.
        /// </summary>
        public void InsertItem()
        {
            _count++;
        }

        /// <summary>
        /// Pretend to remove an item.
        /// </summary>
        public void RemoveItem()
        {
            _count--;
        }

        public void AddFilter(List<WhereClause> whereClauses)
		{
			ResetFilters();
			if (whereClauses.Count > 0) {
				_whereBuffer = WhereStatement.Copy(_query.Where);
				foreach (WhereClause clause in whereClauses) {
					_query.Where.Add(clause);
				}
				_query.BuildQuery();

				Debug.Write("motherfucker1==" + _query.Query);
			}
        }

        public void ResetFilters()
		{
			if (_whereBuffer != null) { 
				_query.Where = _whereBuffer;
				_query.BuildQuery();
				Debug.Write("motherfucker22222==" + _query.Query);
			}

		}
    }
}
