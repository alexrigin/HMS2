using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HMS.DataRecords
{
    public class BatchRecord
    {
        //private String _fileName; // необходимо, если данные хранятся в файле
        private int _id;
        private int _articleId;
        private string _articleName;
        private DateTime _date;
        
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public int Id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// Идентификатор артикула
        /// </summary>
        public int ArticleId { get { return _articleId; } set { _articleId = value; } }
        /// <summary>
        /// Название артикула
        /// </summary>
        public String ArticleName { get { return _articleName; } set { _articleName = value; } }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get { return _date; } set { _date = value; } }
        
        public BatchRecord(int Id, int ArticleId, String ArticleName, DateTime Date)
        {
            _id = Id;
            _articleId = ArticleId;
            _articleName = ArticleName;
            _date = Date;
        }

        public BatchRecord(int ArticleId, String ArticleName, DateTime Date) :
            this(-1,ArticleId, ArticleName,Date)
        {

        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3};",Id,ArticleId,ArticleName,Date.ToString());
        }

        public string ToSqlString()
        {
            return string.Format(string.Format("{0},{1},'{2}','{3}';", Id, ArticleId, ArticleName, Date.ToString(CultureInfo.GetCultureInfo("en-US"))));
        }
    }
}
