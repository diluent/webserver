using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace ConsoleWebServer.Repository.SQLite
{
    public interface ISqLiteRepository<T> where T : class, new()
    {
        IList<T> Select(string query, ISqliteDataAdapter<T> adapter);
        void Insert(string query, T item, ISqliteDataAdapter<T> adapter);
    }

    public class SqLiteRepository<T> : ISqLiteRepository<T> where T : class, new()
    {
        private readonly string _connectionString;

        public SqLiteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        public IList<T> Select(string query, ISqliteDataAdapter<T> adapter)
        {
            var result = new List<T>();
            using (var conn = GetConnection())
            using (var comm = conn.CreateCommand())
            {
                comm.CommandText = query;
                comm.Connection.Open();
                var reader = comm.ExecuteReader();
                while (reader.Read()) {
                    result.Add(adapter.Get(reader));
                }
                comm.Connection.Close();
            }
            return result;
        }

        public void Insert(string query, T item, ISqliteDataAdapter<T> adapter)
        {
            using (var conn = GetConnection())
            using (var comm = conn.CreateCommand())
            {
                comm.CommandText = query;
                foreach (var p in adapter.Set(item)) {
                    comm.Parameters.Add(new SQLiteParameter(p.Key, p.Value));
                }
                comm.Connection.Open();
                comm.ExecuteNonQuery();
                comm.Connection.Close();
            }
        }
    }
}
