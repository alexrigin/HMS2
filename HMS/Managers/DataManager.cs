using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.DataRecords;

namespace HMS.Managers
{
    class DataManager // singleton
    {
        static private DataManager _dataManager; //instance
        private IList<ArticleRecord> _articles;
        private ArticleRecord _currentArticle;
        
        public DataManager()
        {
            _articles = DBManager.ExecuteArticlesToList();
        }
        
        /// <summary>
        /// Возвращает единственный экземпляр класса
        /// </summary>
        static public DataManager Instance
        {
            get
            {
                if (_dataManager == null)
                {
                    _dataManager = new DataManager();
                }
                return _dataManager;
            }
        }
        
        public IList<ArticleRecord> ArticlesList 
        { 
            get 
            {
                return _articles;
            }
            
        }

        public ArticleRecord CurrentArticle { get { return _currentArticle; } set { _currentArticle = value; } }
    }
}
