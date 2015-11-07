using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Data.SQLite;
using HMS.DataVirtualization;
using HMS.DataRecords;
using HMS.Managers;
using HMS.Tools;

namespace HMS.DataProviders
{
    class ArticlesProvider : IItemsProvider<ArticleRecord>
    {
        private int _count;
        private readonly int _fetchDelay;
        private string _dbConnectionString = Properties.Settings.Default.DBConnectionString;
        private string _countRequest;
        private SQLRequest _sqlRequest;

        public SQLRequest SqlRequest
        {
            get { return _sqlRequest; }
        }

        public ArticlesProvider(int count, int fetchDelay)
        {
            _count = count;
            _fetchDelay = fetchDelay;
            _sqlRequest = new SQLRequest("*", "articles", "", "", "");
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            Trace.WriteLine("FetchCount");
            _countRequest = string.Format("SELECT COUNT(id) FROM articles {0};",_sqlRequest.WherePart);
            Debug.WriteLine(_countRequest);
            _count = Convert.ToInt32(DBManager.ExecuteScalar(_countRequest, new SQLiteConnection(_dbConnectionString)));
            Debug.WriteLine("count= "+_count);
            return _count;
        }

        public IList<ArticleRecord> FetchRange(int startIndex, int pageCount, out int overallCount)
        {
            Trace.WriteLine("FetchRange:" + startIndex + "," + pageCount);
            overallCount = _count;
            //startIndex += 1; // sql starts from 1

            int EndIndex = startIndex + pageCount;
            _sqlRequest.SetLimit(startIndex, EndIndex);
            Debug.WriteLine(_sqlRequest.SqlRequestString());
            return DBManager.ExecuteArticlesToList(_sqlRequest.SqlRequestString(), new SQLiteConnection(_dbConnectionString));
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

        public void AddFilter(string filter)
        {
            _sqlRequest.AddFilter(filter);
        }

        public void RemoveFilter(string filter)
        {
            _sqlRequest.RemoveFilter(filter); 
        }
    }
}

/*TO DO///
    Проверить, что лучше: создавать каждый раз новое подключение или использовать всегда только одно!

*/