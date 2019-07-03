using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;

namespace TagFloors
{
    public class MtSQLite
    {
        private string connectionString;
        private SQLiteConnection dbConnection;
        private SQLiteCommand dbCommand;
        private SQLiteDataReader reader;

        private int m_count;
        public int Count {
            get { return m_count; }
        }

        bool bDebug = false;
        DateTime debug_begin;
        TimeSpan debug_span;

        public MtSQLite(string dbFile)
        {
            connectionString = @"Data Source=" + dbFile;
            OpenDB(connectionString);
        }

        public void OpenDB(string connectionString)
        {
            try
            {
                dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();
                Console.WriteLine("Connected to db");
            }
            catch (Exception e)
            {
                string temp = e.ToString();
                Console.WriteLine(temp);
            }
            Console.WriteLine("OpenDB -- Sqlite database connected.");
        }

        public void CloseSqlConnection()
        {

            if (dbCommand != null)
            {
                dbCommand.Dispose();
            }
            dbCommand = null;
            if (reader != null)
            {
                reader.Dispose();
            }

            reader = null;
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;

            Console.WriteLine("Disconnected from db.");
        }

        #region API
        public SQLiteDataReader ExecuteQuery(string sqlQuery)
        {
            if (bDebug)
            {
                debug_begin = DateTime.Now;
            }

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                dbCommand = (SQLiteCommand)dbConnection.CreateCommand();
                dbCommand.CommandText = sqlQuery;
                reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (bDebug)
            {
                debug_span = DateTime.Now.Subtract(debug_begin);
                Console.WriteLine("Local query cost: {0}sec.", debug_span.TotalSeconds);
            }
            Console.WriteLine("Local Query: {0}", sqlQuery);
            return reader;
        }

        public JArray ExecuteQueryToJarry(string sqlQuery)
        {
            if (bDebug)
            {
                debug_begin = DateTime.Now;
            }

            SQLiteDataReader reader = null;
            JArray data = null;
            try
            {
                using (SQLiteConnection dbConnection = new SQLiteConnection(connectionString))
                {
                    dbConnection.Open();

                    using (SQLiteCommand cm = new SQLiteCommand(dbConnection))
                    {
                        cm.CommandText = sqlQuery;
                        reader = cm.ExecuteReader(CommandBehavior.CloseConnection);
                        data = readerToJsonArray(reader);
                    }

                    dbConnection.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(sqlQuery + "\n" + e);
            }

            if (bDebug)
            {
                debug_span = DateTime.Now.Subtract(debug_begin);
                Console.WriteLine("Local none query cost: {0} ms.\nstrSql:{1}", debug_span.TotalMilliseconds, sqlQuery);
            }
            return data;
        }

        public int ExecuteNoneQuery(string sqlNoneQuery)
        {
            if (bDebug)
            {
                debug_begin = DateTime.Now;
            }

            int resultCount = 0;
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                dbCommand = (SQLiteCommand)dbConnection.CreateCommand();
                dbCommand.CommandText = sqlNoneQuery;
                resultCount = dbCommand.ExecuteNonQuery();
                m_count += resultCount;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                //dbConnection.Close();
                //Console.WriteLine("db conn close");
            }

            if (bDebug)
            {
                debug_span = DateTime.Now.Subtract(debug_begin);
                Console.WriteLine("Local none query cost: {0}sec.", debug_span.TotalSeconds);
            }
            Console.WriteLine("Local None Query: {0}", sqlNoneQuery);
            return resultCount;
        }
        #endregion

        public static JToken readerToJson(SQLiteDataReader reader)
        {
            JTokenWriter jwrite = new JTokenWriter();
            jwrite.WriteStartObject();
            for (int i = 0; i < reader.FieldCount; ++i)
            {
                jwrite.WritePropertyName(reader.GetName(i));
                jwrite.WriteValue(reader.GetValue(i));
            }
            jwrite.WriteEndObject();
            return jwrite.Token;
        }

        private JArray readerToJsonArray(SQLiteDataReader reader)
        {
            if (reader == null) return null;
            JArray arr = new JArray();
            while (reader.Read())
            {
                JToken obj = readerToJson(reader);
                arr.Add(obj);
            }
            return arr;
        }

        #region OLD
        public SQLiteDataReader ReadFullTable(string tableName)
        {
            string query = "SELECT * FROM " + tableName;
            return ExecuteQuery(query);
        }

        public int InsertInto(string tableName, string[] values)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
            for (int i = 1; i < values.Length; ++i)
            {
                query += ", " + values[i];
            }
            query += ")";
            return ExecuteNoneQuery(query);
        }

        public int InsertInto(string tableName, string[] cols, string[] colvalues)
        {
            string query = "INSERT INTO " + tableName + "(";
            for (int i = 0; i < cols.Length; i++)
            {
                if (i != cols.Length - 1)
                    query += cols[i] + ",";
                else
                    query += cols[i];
            }
            query += ")" + "VALUES (";

            for (int i = 0; i < colvalues.Length; i++)
            {
                if (i != colvalues.Length - 1)
                    query += colvalues[i] + ",";
                else
                    query += colvalues[i];
            }
            query += ")";

            //Debug.Log(query);
            return ExecuteNoneQuery(query);
        }


        public int UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
        {
            string query = "UPDATE " + tableName + " SET " + cols[0] + " = " + colsvalues[0];
            for (int i = 1; i < colsvalues.Length; ++i)
            {
                query += ", " + cols[i] + " =" + colsvalues[i];
            }
            query += " WHERE " + selectkey + " = " + selectvalue + " ";
            return ExecuteNoneQuery(query);
        }

        public int Delete(string tableName, string[] cols, string[] colsvalues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];

            for (int i = 1; i < colsvalues.Length; ++i)
            {

                query += " or " + cols[i] + " = " + colsvalues[i];
            }
            return ExecuteNoneQuery(query);
        }

        public int InsertIntoSpecific(string tableName, string[] cols, string[] values)
        {

            if (cols.Length != values.Length)
            {

                throw new SQLiteException("columns.Length != values.Length");

            }

            string query = "INSERT INTO " + tableName + "(" + cols[0];

            for (int i = 1; i < cols.Length; ++i)
            {

                query += ", " + cols[i];

            }

            query += ") VALUES (" + values[0];

            for (int i = 1; i < values.Length; ++i)
            {

                query += ", " + values[i];

            }

            query += ")";

            return ExecuteNoneQuery(query);

        }

        public int DeleteContents(string tableName)
        {

            string query = "DELETE FROM " + tableName;

            return ExecuteNoneQuery(query);

        }

        public int CreateTable(string name, string[] col, string[] colType)
        {

            if (col.Length != colType.Length)
            {

                throw new SQLiteException("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i] + " " + colType[i];

            }

            query += ")";

            return ExecuteNoneQuery(query);

        }

        public SQLiteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
        {

            if (col.Length != operation.Length || operation.Length != values.Length)
            {

                throw new SQLiteException("col.Length != operation.Length != values.Length");

            }

            string query = "SELECT " + items[0];

            for (int i = 1; i < items.Length; ++i)
            {

                query += ", " + items[i];

            }

            query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";

            for (int i = 1; i < col.Length; ++i)
            {

                query += " AND " + col[i] + operation[i] + "'" + values[i] + "' ";

            }

            return ExecuteQuery(query);

        }
        #endregion old
    }
}
