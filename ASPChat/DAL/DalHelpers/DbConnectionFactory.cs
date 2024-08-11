using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ASPChat.DAL.DalHelpers
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;
        private string _provider;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _provider = configuration.GetValue<string>("DatabaseProvider");
        }

        public DbConnectionFactory(string connectionString, string dbProvider) 
        { 
            _connectionString=connectionString;
            _provider = dbProvider;
        }
        public IDbConnection CreateDbConnection()
        {
            return _provider switch
            {
                "SQLite" => new SqliteConnection(_connectionString),
                "SQLServer" => new SqlConnection(_connectionString),
                _ => throw new NotSupportedException("")
            };
        }
    }
}
