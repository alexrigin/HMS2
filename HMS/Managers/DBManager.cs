using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Data;
using HMS.Tools;
using HMS.DataRecords;


namespace HMS.Managers
{
    /// <summary>
    /// Набор инструментов для работы с SQLite БД
    /// </summary>
    public class DBManager
    {

        public static void ExecuteNonQuery(SQLiteCommand cmd, SQLiteConnection connection)
        {
            try
            {
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
                throw; // пересылаем исключение на более высокий уровень
            }
            finally
            {
                connection.Dispose();
            }
        }

        public static void ExecuteNonQuery(string SqlRequest, SQLiteConnection Connection)
        {
            try
            {
                Connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
                throw; // пересылаем исключение на более высокий уровень
            }
            finally
            {
                Connection.Dispose();
            }
            
        }

        public static object ExecuteScalar(string SqlRequest, SQLiteConnection Connection)
        {
            object result = null;
            try
            {
                Connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
                result = cmd.ExecuteScalar();
            }
            catch (SQLiteException ex)
            {
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
                throw; // пересылаем исключение на более высокий уровень
            }
            finally
            {
                Connection.Dispose();
            }
            return result;
        }


        public static DataSet ExecuteDataSet(string SqlRequest, SQLiteConnection Connection)
        {
            DataSet dataSet = new DataSet();
            dataSet.Reset();

            SQLiteCommand cmd = new SQLiteCommand(SqlRequest, Connection);
            try
            {
                Connection.Open();
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(cmd);
                dataAdapter.Fill(dataSet);
            }
            catch (SQLiteException ex)
            {
                Log.Write(ex);
                //Debug.WriteLine(ex.Message);
                throw; // пересылаем исключение на более высокий уровень
            }
            finally
            {
                Connection.Dispose();
            }

            return dataSet;
        }

    }
}

/*/ TO DO
   1. Сделать логи
*/