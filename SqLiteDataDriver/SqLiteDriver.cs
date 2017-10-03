using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SqLiteDataDriver
{
    public class SqLiteDriver
    {
        private readonly string _dbPath;

        public SqLiteDriver(string dbPath)
        {
            this._dbPath = dbPath;
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                var cmd = new SQLiteCommand(sql, this.GetConnection());
                var rowAffected = cmd.ExecuteNonQuery(CommandBehavior.CloseConnection);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public object ExecureScaler(string sql)
        {
            try
            {
                var cmd = new SQLiteCommand(sql, this.GetConnection());
                return cmd.ExecuteScalar(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return 0;
            }
        }

        public int Count(string tableName)
        {
            try
            {
                var sql = $"select count(*) from {tableName}";
                var cmd = new SQLiteCommand(sql, this.GetConnection());
                var count = cmd.ExecuteScalar(CommandBehavior.CloseConnection);
                return (int)count;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return 0;
            }
        }

        public bool DeleteById(string id, string tableName)
        {
            try
            {
                var sql = $"delete from {tableName} where Id='{id}'";
                var con = this.GetConnection();
                var sqlcommand = new SQLiteCommand(sql, con);
                var rowAffected = sqlcommand.ExecuteNonQuery(CommandBehavior.CloseConnection);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private SQLiteConnection GetConnection()
        {
            if (!File.Exists(this._dbPath))
            {
                SQLiteConnection.CreateFile(this._dbPath);
            }
            var sqLiteConnection = new SQLiteConnection($@"Data Source={this._dbPath};Version=3;");
            return sqLiteConnection.OpenAndReturn();
        }

        public SQLiteDataReader ReadData(string sql)
        {
            try
            {
                var cmd = new SQLiteCommand(sql, this.GetConnection());
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public DataTable LoadData(string tableName)
        {
            var sql = $"select * from {tableName}";
            var con = this.GetConnection();
            var adapter = new SQLiteDataAdapter(new SQLiteCommand(sql, con));
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            con.Close();
            return dataTable;
        }
    }
}
