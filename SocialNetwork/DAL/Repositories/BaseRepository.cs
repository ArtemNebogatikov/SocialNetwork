using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace SocialNetwork.DAL.Repositories
{
    public class BaseRepository
    {

        protected T QueryFirstOrDefault<T>(string sql, object parametrs = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(sql, parametrs);
            }
        }
        protected List<T> Query<T>(string sql, object parametrs = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Query<T>(sql, parametrs).ToList();
            }
        }
        protected int Execute(string sql, object parametrs = null)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                return connection.Execute(sql, parametrs);
            }
        }
        private IDbConnection CreateConnection()
        {
            string pathForBase = Directory.GetCurrentDirectory();
            string applicationDirectory = pathForBase.Remove(pathForBase.Length - 9);
            return new SQLiteConnection($@"Data Source={applicationDirectory}DAL\DB\social_network_bd.db;Version = 3");
        }
    }
}
