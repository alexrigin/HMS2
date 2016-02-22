using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SQLite;
using HMS.DataRecords;
using HMS.Tools;
using HMS.View;
using System.Globalization;

namespace HMS.Managers
{
    class DataManager // singleton
    {
        public delegate T ExecuteInfo<T>(SQLiteDataReader r);

        static private DataManager _dataManager; //instance
        
        public DataManager()
        {
            //_articles = DBManager.ExecuteArticlesToList();
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

        public static int GetMaxArticleId()
        {
            object value = DBManager.ExecuteScalar("SELECT max(id) from articles;", new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            if (!value.Equals(DBNull.Value))
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static int GetMaxBatchNumber()
        {
            object value = DBManager.ExecuteScalar("SELECT max(batchnumber) from measurements;", new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            if (!value.Equals(DBNull.Value))
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static int GetMaxMeasurementId()
        {
            object value = DBManager.ExecuteScalar("SELECT max(id) from measurements;", new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            if (!value.Equals(DBNull.Value))
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static IList<T> ExecuteToList<T>(string SqlRequest, SQLiteConnection Connection, ExecuteInfo<T> executeInfo)
        {
            IList<T> list = new List<T>();
            try
            {
                Connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
                SQLiteDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(executeInfo.Invoke(r));
                }
                r.Close();
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Dispose();
            }
            return list;
        }


		static object locker = new object();
		public static IList<KeyValuePair<DateTime, double>> GetLastMeasurements(string parameterKey, ArticleRecord article, int numOfRecords)
		{
			lock (locker) {
				SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.DBConnectionString);
				IList<KeyValuePair<DateTime, double>> list = new List<KeyValuePair<DateTime, double>>();

				String firstCharacter;
				if (parameterKey.Equals("SeamerHeight")) {
					firstCharacter = "sh";
				} else {
					firstCharacter = parameterKey[0].ToString().ToLower();
				}

				try {
					Connection.Open();
					string cmdStr = string.Format("SELECT date, {0}, {1} FROM measurements WHERE article=@articleid;", firstCharacter, firstCharacter + "time");
					SQLiteCommand cmd = new SQLiteCommand(cmdStr, Connection);
					cmd.Parameters.Add(new SQLiteParameter("@articleid", article.Id));
					SQLiteDataReader r = cmd.ExecuteReader();

					int count = 0;
					while (r.Read() && count <= numOfRecords) {
						double? value = r[firstCharacter].ToString().ToNullableDouble();
						if (value != null) {
							string time = r[firstCharacter + "time"].ToString();
							string date = Convert.ToDateTime(r["date"]).ToShortDateString();
							DateTime? dtime = (time).Equals(string.Empty) ? (DateTime?)null : Convert.ToDateTime(date + " " + time);
							list.Add(new KeyValuePair<DateTime, double>((DateTime)dtime, (double)value));
						}
					}
					r.Close();
				}
				catch (SQLiteException ex) {
					Debug.WriteLine(ex.Message);
				}
				finally {
					Connection.Dispose();
				}
				return list;
			}
		}

		public static IList<MeasurementRecord> ExecuteMeasurementsToList()
		{
			SQLiteConnection Connection = new SQLiteConnection(Properties.Settings.Default.DBConnectionString);
			IList<MeasurementRecord> list = new List<MeasurementRecord>();
			try {
				Connection.Open();
				SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM measurements;", Connection);
				SQLiteDataReader r = cmd.ExecuteReader();

				while (r.Read()) {
					MeasurementRecord msr = ReadLastMeasurement(r);
					if (msr.Diameter != null )
						list.Add(msr);
					
				}
				r.Close();
			}
			catch (SQLiteException ex) {
				Debug.WriteLine(ex.Message);
			}
			finally {
				Connection.Dispose();
			}
			return list;
		}


		public static void InsertIntoArticles(ArticleRecord article, SQLiteConnection connection)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.CommandText = "INSERT INTO articles(id, name, number, nominalh ,nominalsh ,nominald, maxh, maxsh, maxd, minh, minsh, mind) VALUES ("
                + "@id, @name, @number, @nominalH, @nominalSH, @nominalD, @maxH, @maxSH, @maxD, @minH, @minSH, @minD);";
            //cmd.Parameters.AddWithValue("@name", article.Name);
            cmd.Parameters.Add(new SQLiteParameter("@id", article.Id));
            cmd.Parameters.Add(new SQLiteParameter("@name", article.Name));
            cmd.Parameters.Add(new SQLiteParameter("@number", article.Number));
            cmd.Parameters.Add(new SQLiteParameter("@nominalH", article.NominalHeight));
            cmd.Parameters.Add(new SQLiteParameter("@nominalSH", article.NominalSeamerHeight));
            cmd.Parameters.Add(new SQLiteParameter("@nominalD", article.NominalDiameter));
            cmd.Parameters.Add(new SQLiteParameter("@maxH", article.MaxHeight));
            cmd.Parameters.Add(new SQLiteParameter("@maxSH", article.MaxSeamerHeight));
            cmd.Parameters.Add(new SQLiteParameter("@maxD", article.MaxDiameter));
            cmd.Parameters.Add(new SQLiteParameter("@minH", article.MinHeight));
            cmd.Parameters.Add(new SQLiteParameter("@minSH", article.MinSeamerHeight));
            cmd.Parameters.Add(new SQLiteParameter("@minD", article.MinDiameter));
            
            //Debug.WriteLine("InsertRequest="+cmd.CommandText);
            try
            {
                DBManager.ExecuteNonQuery(cmd, connection);
            }
            catch (SQLiteException ex)
            {
				Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public static void InsertIntoArticles(ArticleRecord article)
        {
            InsertIntoArticles(article, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
        }


		public static void InsertIntoMeasurements(string propertyKey, int articleId, DateTime date, KeyValuePair<DateTime, double> properties, int batchNumber, int rowId)
		{
			InsertIntoMeasurements(propertyKey, articleId, date, properties, batchNumber, rowId, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
		}

		public static void InsertIntoMeasurements(string propertyKey, int articleId, DateTime date, KeyValuePair<DateTime, double> properties, int batchNumber, int rowId, SQLiteConnection connection)
        {
			String firstCharacter;
			if (propertyKey.Equals("SeamerHeight")) {
				firstCharacter = "sh";
				Debug.WriteLine("firstCharacter=" + firstCharacter);
			} else {
				firstCharacter = propertyKey[0].ToString().ToLower();
			}

			SQLiteCommand cmd = new SQLiteCommand(); 
            cmd.CommandText = string.Format("INSERT INTO measurements(id, article, batchnumber, date, {0}, {1} ) VALUES ("
                + "@id, @article, @batchnumber, @date, @value, @time);",firstCharacter, firstCharacter + "time");
            cmd.Parameters.Add(new SQLiteParameter("@id", rowId));
            cmd.Parameters.Add(new SQLiteParameter("@article", articleId));
            cmd.Parameters.Add(new SQLiteParameter("@batchnumber", batchNumber));
            cmd.Parameters.Add(new SQLiteParameter("@date", date.ToString("yyyy-MM-dd HH:mm:ss")));
			cmd.Parameters.Add(new SQLiteParameter("@value", properties.Value));
			cmd.Parameters.Add(new SQLiteParameter("@time", properties.Key.ToLongTimeString()));
		
			Debug.WriteLine("InsertRequest=" + cmd.CommandText);
			try
            {
                DBManager.ExecuteNonQuery(cmd, connection);
            }
            catch (SQLiteException ex)
            {
                throw;
            }
        }

        
		public static void UpdateMeasurementRecord(string propertyKey, KeyValuePair<DateTime, double> properties, int rowId)
		{
			UpdateMeasurementRecord(propertyKey, properties, rowId, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
		}

		public static void UpdateMeasurementRecord(string propertyKey, KeyValuePair<DateTime,double> properties, int rowId, SQLiteConnection connection)
		{
			String firstCharacter;
			if (propertyKey.Equals("SeamerHeight")) {
				firstCharacter = "sh";
			} else {
				firstCharacter = propertyKey[0].ToString().ToLower();
			}
			SQLiteCommand cmd = new SQLiteCommand();
			 
			cmd.CommandText = string.Format("UPDATE measurements SET {0}=@value, {1}=@time WHERE id=@rowId;", firstCharacter, firstCharacter + "time");
			cmd.Parameters.Add(new SQLiteParameter("@value", properties.Value));
			cmd.Parameters.Add(new SQLiteParameter("@time", properties.Key.ToLongTimeString()));
			cmd.Parameters.Add(new SQLiteParameter("@rowId", rowId));

			Debug.WriteLine("InsertRequest=" + cmd.CommandText);
			try {
				DBManager.ExecuteNonQuery(cmd, connection);
			}
			catch (SQLiteException ex) {
				throw;
			}
		}

		#region	 DataReaders
		public static BatchRecord ReadBatch(SQLiteDataReader r)
		{
			return (new BatchRecord() {
				Id = Convert.ToInt32(r["id"]),
				ArticleId = Convert.ToInt32(r["article"].ToString()),
				ArticleName = r["name"].ToString(),
				ArticleNumber = r["number"].ToString(),
				Date = Convert.ToDateTime(r["date"].ToString()),
				BatchNumber = Convert.ToInt32(r["batchnumber"])
			});
		}
		public static MeasurementRecord ReadMeasurement(SQLiteDataReader r)
		{
			string htime = r["htime"].ToString();
			string shtime = r["shtime"].ToString();
			string dtime = r["dtime"].ToString();

			return (new MeasurementRecord() {
				Height = r["h"].ToString().ToNullableDouble(),
				HTime = (htime).Equals(string.Empty) ? (DateTime?)null : DateTime.Parse(htime),
				SeamerHeight = r["sh"].ToString().ToNullableDouble(),
				SHTime = (shtime).Equals(string.Empty) ? (DateTime?)null : DateTime.Parse(shtime),
				Diameter = r["d"].ToString().ToNullableDouble(),
				DTime = (dtime).Equals(string.Empty) ? (DateTime?)null : DateTime.Parse(dtime)
			});
		}
		public static MeasurementRecord ReadLastMeasurement(SQLiteDataReader r)
		{
			string htime = r["htime"].ToString();
			string shtime = r["shtime"].ToString();
			string dtime = r["dtime"].ToString();
			string date = Convert.ToDateTime(r["date"]).ToShortDateString();

			DateTime? dhtime = (htime).Equals(string.Empty) ? (DateTime?)null : Convert.ToDateTime(date + " "+htime);
			DateTime? dshtime = (shtime).Equals(string.Empty) ? (DateTime?)null : Convert.ToDateTime(date + " " + shtime);
			DateTime? ddtime = (dtime).Equals(string.Empty) ? (DateTime?)null : Convert.ToDateTime(date + " " + dtime);

			return (new MeasurementRecord() {
				Height = r["h"].ToString().ToNullableDouble(),
				HTime = dhtime,
				SeamerHeight = r["sh"].ToString().ToNullableDouble(),
				SHTime = dshtime,
				Diameter = r["d"].ToString().ToNullableDouble(),
				DTime = ddtime
			});
		}
		public static ArticleRecord ReadArticlesFromDB(SQLiteDataReader r)
		{

			return (new ArticleRecord() {
				Id = Convert.ToInt32(r["id"].ToString()),
				Name = r["name"].ToString(),
				Number = r["number"].ToString(),
				NominalDiameter = Convert.ToDouble(r["nominald"]),
				MinDiameter = Convert.ToDouble(r["mind"]),
				MaxDiameter = Convert.ToDouble(r["maxd"]),
				NominalHeight = Convert.ToDouble(r["nominalh"]),
				MinHeight = Convert.ToDouble(r["minh"]),
				MaxHeight = Convert.ToDouble(r["maxh"]),
				NominalSeamerHeight = Convert.ToDouble(r["nominalsh"]),
				MinSeamerHeight = Convert.ToDouble(r["minsh"]),
				MaxSeamerHeight = Convert.ToDouble(r["maxsh"])
			});
		}

		#endregion


		public static void CreateTempDB()
        {
            String request = "DROP TABLE IF EXISTS articles;"
            + "CREATE TABLE articles"
            + "( "
            + "a_id INTEGER PRIMARY KEY AUTOINCREMENT,"
            + "a_name TEXT not null,"
            + "a_minh REAL not null"
            + ");";
            DBManager.ExecuteNonQuery(request, new SQLiteConnection("Data Source=temp1.db; Version=3;"));
        }


        public static void FillTempDB()
        {
            String articles_table = "INSERT INTO articles(a_name, a_minh) VALUES";
            for (int i = 0; i < 20000; i++)
            {
                articles_table += "('" + i + " McDonalds','103'),";
            }
            articles_table += "('20000 McDonalds','103');";
            DBManager.ExecuteNonQuery(articles_table, new SQLiteConnection("Data Source=temp1.db; Version=3;"));
        }

        public static void FillTempBD2()
        {
            for (int i = 1; i < 30; i++)
            {
                string request = "INSERT INTO articles(name,number,minh,maxh,minsh,maxsh,mind,maxd) VALUES ('name" + i + "','000" + i + "',1,3,4,5,3,10);";
                //Debug.WriteLine(request);
                DBManager.ExecuteNonQuery(request, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            }
        }

        public static void FillTempBD3()
        {
            for (int k = 1, j = 1, i = 1; i < 2000; i++)
            {

                switch (j)
                {
                    case 1:
                        k = 1;
                        break;
                    case 2:
                        k = 2;
                        break;
                    case 3:
                        k = 3;
                        break;
                    case 4:
                        k = 4;
                        break;
                    case 5:
                        k = 5;
                        break;
                    default:
                        break;
                }

                string request = string.Format("INSERT INTO measurements(article,batchnumber,date,h,htime,sh,shtime,d,dtime)"
                    + "VALUES ({0},{1},'2015-10-29 20:38:00',100,'8:45',103,'9:43',243,'12:00');", j, k);
                //Debug.WriteLine(request);
                if (j == 30) j = 1;
                j++;
                DBManager.ExecuteNonQuery(request, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
            }
        }

        /// <summary>
        /// Создает базу данных, состояющую из двух таблиц "Артикулы" и "Измерения"
        /// </summary>
        public static void CreateDB()
        {
            string articles_table = "CREATE TABLE IF NOT EXISTS articles"
                + "( "
                + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "name TEXT not null, "
                + "number TEXT not null unique, "
                + "nominalh  REAL not null, "
                + "minh  REAL not null, "
                + "maxh REAL not null check(maxh > minh), "
                + "nominalsh  REAL not null, "
                + "minsh REAL not null, "
                + "maxsh REAL not null check(maxsh > minsh), "
                + "nominald  REAL not null, "
                + "mind REAL not null, "
                + "maxd REAL not null check(maxd > mind) "
                + ");";
            DBManager.ExecuteNonQuery(articles_table, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));

            string measurements_table = "CREATE TABLE IF NOT EXISTS measurements "
                + "( "
                + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "article     INTEGER not null REFERENCES articles,"
                + "batchnumber INTEGER not null, "
                + "date        datetime not null, "
                + "h           REAL null, "
                + "htime       text null, "
                + "sh          REAL null, "
                + "shtime      text null, "
                + "d           REAL null, "
                + "dtime       text null "
                + ");";
            DBManager.ExecuteNonQuery(measurements_table, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
        }

    }
}
/*---Структура БД---//
<--Таблица "Артикулы" -->
+---------------+-------------+------+-----+---------+------------------------+
| Field         | Type        | Null | Key | Default | Extra                  |
+---------------+-------------+------+-----+---------+------------------------+
| id            | integer     | NO   | PRI | NULL    | auto_increment         |
| name          | text        | NO   |     | NULL    |                        |
| number        | text        | NO   | UNI | NULL    |                        |
| nominalH      | real        | NO   |     | NULL    | Nominal Height         |
| minH          | real        | NO   |     | NULL    |                        |
| maxH          | real        | NO   |     | NULL    |                        |
| nominalSH     | real        | NO   |     | NULL    | Nominal Seamer Height  |
| minSH         | real        | NO   |     | NULL    |       (высота закатки) |
| maxSH         | real        | NO   |     | NULL    |                        |
| nominalD      | real        | NO   |     | NULL    | Nominal Diameter       |
| minD          | real        | NO   |     | NULL    |                        |
| maxD          | real        | NO   |     | NULL    |                        |
+---------------+-------------+------+-----+---------+------------------------+

<--Таблица "Измерения" --> (неправильно)
+---------------+-------------+------+-----+---------+------------------------+
| Field         | Type        | Null | Key | Default | Extra                  |
+---------------+-------------+------+-----+---------+------------------------+
| id            | integer     | NO   | PRI | NULL    | auto_increment         |
| name          | text        | NO   |     | NULL    |                        |
| number        | text        | NO   | UNI | NULL    |                        |
| nominalH      | real        | NO   |     | NULL    | Nominal Height         |
| minH          | real        | NO   |     | NULL    |                        |
| maxH          | real        | NO   |     | NULL    |                        |
| nominalSH     | real        | NO   |     | NULL    | Nominal Seamer Height  |
| minSH         | real        | NO   |     | NULL    |       (высота закатки) |
| maxSH         | real        | NO   |     | NULL    |                        |
| nominalD      | real        | NO   |     | NULL    | Nominal Diameter       |
| minD          | real        | NO   |     | NULL    |                        |
| maxD          | real        | NO   |     | NULL    |                        |
+---------------+-------------+------+-----+---------+------------------------+
*/

/*//TO do
    1. Сделать логи в файл
	

*/
