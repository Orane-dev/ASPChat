using ASPChat.DAL.DalHelpers;
using ASPChat.DAL.Interfaces;
using ASPChat.DAL.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Data.Common;
namespace ASPChat.DAL.Implementations
{
    public class AuthenticationDAL : IAuthenticationDAL
    {
        private IDbConnectionFactory _dbConnectionFactory;
        public AuthenticationDAL(IDbConnectionFactory dbConnectionFactory) 
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<UserModel> GetUserAsync(string email)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();
                string sqlQuery = "SELECT * FROM User WHERE Email = @email";
                var userModel = await connection.QueryFirstOrDefaultAsync<UserModel>(sqlQuery, new { email = email });
                return userModel ?? new UserModel();
            }
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();
                string sqlQuery = "SELECT * FROM User WHERE UserId = @userId";
                var userModel = await connection.QueryFirstOrDefaultAsync<UserModel>(sqlQuery, new { userId = userId });
                return userModel ?? new UserModel();
            }
        }

        public async Task<int> CreateUserAsync(UserModel user)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection() as DbConnection) 
            {
                await connection.OpenAsync();
                
                string sqlQuery = "INSERT INTO User " +
                    "(Email, Password, Salt, FirstName, LastName) values " +
                    "(@Email, @Password, @Salt, @FirstName, @LastName); " +
                    "SELECT last_insert_rowid();";
                
                var userId = await connection.ExecuteScalarAsync<int>(sqlQuery, user);
                
                return userId;
            }
        }
    }
}
