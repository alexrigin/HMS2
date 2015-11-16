using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Data;
using HMS.DataRecords;


namespace HMS.Managers
{
    /// <summary>
    /// Набор инструментов для работы с SQLite БД
    /// </summary>
    public class DBManager
    {
        private static void DBConnect(SQLiteConnection Connection)
        {
            try
            {
                Connection.Open();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void DBDisconnect(SQLiteConnection Connection)
        {
            try
            {
                Connection.Dispose();
                //Connection.Close();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void ExecuteNonQuery(string SqlRequest, SQLiteConnection Connection)
        {
            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);
        }

        public static object ExecuteScalar(string SqlRequest, SQLiteConnection Connection)
        {
            object result = null;
            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);
            return result;
        }

        public static string ExecuteScalarToString(string SqlRequest, SQLiteConnection Connection)
        {
            return (ExecuteScalar(SqlRequest, Connection).ToString());
        }

        public static DataSet ExecuteDataSet(string SqlRequest, SQLiteConnection Connection)
        {
            DataSet dataSet = new DataSet();
            dataSet.Reset();

            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(cmd);
                dataAdapter.Fill(dataSet, "articles");
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);

            return dataSet;
        }

        public static IList<ArticleRecord> ExecuteArticlesToList(string SqlRequest, SQLiteConnection Connection)
        {
            //StartIndex += 1; // sql starts from 1
            //int EndIndex = StartIndex + PageCount - 1;
            IList<ArticleRecord> list = new List<ArticleRecord>();

            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                SQLiteDataReader r = cmd.ExecuteReader();

                String line = String.Empty;
                while (r.Read())
                {
                    list.Add(new ArticleRecord(Convert.ToInt32(r["id"].ToString()), r["name"].ToString(), r["number"].ToString(),
                        Convert.ToDouble(r["mind"]), Convert.ToDouble(r["maxd"]),
                        Convert.ToDouble(r["minh1"]), Convert.ToDouble(r["maxh1"]),
                        Convert.ToDouble(r["minh2"]), Convert.ToDouble(r["maxh2"])));
                }
                r.Close();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);

            return list;
        }

        public static IList<ArticleRecord> ExecuteArticlesToList()
        {
            return ExecuteArticlesToList("SELECT * FROM articles;", new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
        }

        public static IList<string> ExecuteArticlesToListForComboBox(string SqlRequest, SQLiteConnection Connection)
        {
            //StartIndex += 1; // sql starts from 1
            //int EndIndex = StartIndex + PageCount - 1;
            IList<string> list = new List<string>();

            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                SQLiteDataReader r = cmd.ExecuteReader();

                String line = String.Empty;
                while (r.Read())
                {
                    list.Add(r["name"].ToString());
                }
                r.Close();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);

            return list;
        }
        public static IList<BatchRecord> ExecuteBatchesToList(string SqlRequest, SQLiteConnection Connection)
        {
            //StartIndex += 1; // sql starts from 1
            //int EndIndex = StartIndex + PageCount - 1;
            IList<BatchRecord> list = new List<BatchRecord>();

            DBConnect(Connection);
            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                SQLiteDataReader r = cmd.ExecuteReader();

                String line = String.Empty;
                while (r.Read())
                {
                    list.Add(new BatchRecord(Convert.ToInt32(r["id"].ToString()), Convert.ToInt32(r["article"].ToString()), r["name"].ToString(),Convert.ToDateTime("2015-10-29 22:01:33.111")));
                }
                r.Close();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            DBDisconnect(Connection);

            return list;
        }

        public static void CreateDB()
        {
            string articles_table = "CREATE TABLE IF NOT EXISTS articles"
            + "( "
            + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
            + "name TEXT not null, "
            + "number TEXT not null unique, "
            + "minh1  REAL not null, "
            + "maxh1 REAL not null check(maxh1 > minh1), "
            + "minh2 REAL not null, "
            + "maxh2 REAL not null check(maxh2 > minh2), "
            + "mind REAL not null, "
            + "maxd REAL not null check(maxd > mind) "
            + ");";
            ExecuteNonQuery(articles_table, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));

            string batches_table = "CREATE TABLE IF NOT EXISTS batches "
                + "(id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "date datetime not null, "
                + "article INTEGER not null REFERENCES articles"
                + ");";
            ExecuteNonQuery(batches_table, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));

            string measurements_table = "CREATE TABLE IF NOT EXISTS measurements "
                + "(id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "number integer not null, "
                + "h1   real null, "
                + "th1  time null, "
                + "h2   real null, "
                + "th2  time null, "
                + "d    real null, "
                + "td   time null, "
                + "batch INTEGER not null REFERENCES batches"
                + ");";
            ExecuteNonQuery(measurements_table, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
        }

        public static void InsertIntoArticles(ArticleRecord article, SQLiteConnection Connection)
        {
            string request = "INSERT INTO articles(name,number,minh1,maxh1,minh2,maxh2,mind,maxd) VALUES (" + article.ToSqlString() + ");";
            Debug.WriteLine(request);
            ExecuteNonQuery(request, Connection);
        }

        public static void CreateTempDB()
        {
            String request = "DROP TABLE IF EXISTS articles;"
            + "CREATE TABLE articles"
            + "( "
            + "a_id INTEGER PRIMARY KEY AUTOINCREMENT,"
            + "a_name TEXT not null,"
            + "a_minh REAL not null"
            + ");";
            ExecuteNonQuery(request, new SQLiteConnection("Data Source=temp1.db; Version=3;"));
        }

        public static void FillTempDB()
        {
            String articles_table = "INSERT INTO articles(a_name, a_minh) VALUES";
            for (int i = 0; i < 20000; i++)
            {
                articles_table += "('" + i + " McDonalds','103'),";
            }
            articles_table += "('20000 McDonalds','103');";
            ExecuteNonQuery(articles_table, new SQLiteConnection("Data Source=temp1.db; Version=3;"));
        }

        public static void FillTempBD2()
        {
            for (int i = 1; i < 30; i++)
            {
                string request = "INSERT INTO articles(name,number,minh1,maxh1,minh2,maxh2,mind,maxd) VALUES ('name" + i + "','000" + i + "',1,3,4,5,3,10);";
                //Debug.WriteLine(request);
                ExecuteNonQuery(request, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            }
        }

        public static void FillTempBD3()
        {
            for (int j=1,i = 1; i < 10000; i++)
            {
                 string request = string.Format("INSERT INTO batches(date,article) VALUES ('2015-10-29 20:38:00',{0}); ",j);
                //Debug.WriteLine(request);
                if (j == 30) j = 1;
                j++;
                ExecuteNonQuery(request, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            }
        }
    }
}
