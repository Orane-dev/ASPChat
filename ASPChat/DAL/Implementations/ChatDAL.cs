using ASPChat.DAL.DalHelpers;
using ASPChat.DAL.Interfaces;
using ASPChat.DAL.Models;
using ASPChat.Models;
using Dapper;
using System.Data.Common;

namespace ASPChat.DAL.Implementations
{
    public class ChatDAL : IChatDAL
    {
        private IDbConnectionFactory _connectionFactory;

        public ChatDAL(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ChatModel>> GetUserChatListAsync(int userId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = @"SELECT cu1.chat_id as ChatId, cu1.user_id as UserId, u.UserId as CompanionId, u.Email as CompanionEmail
                                    From ChatUser as cu1
                                    JOIN ChatUser as cu2 ON cu1.chat_id = cu2.chat_id
                                    JOIN User u ON cu2.user_id = u.UserId
                                    WHERE cu1.user_id = @chatUserId AND cu2.user_id != @chatUserId";

                var chatList = await connection.QueryAsync<ChatModel>(sqlQuery, new { chatUserId = userId });

                return chatList ?? new List<ChatModel>();
            }
        }

        public async Task<int> CreateChatAsync(int userCreateId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "INSERT INTO Chat (UserId) VALUES (@userCreateId); " +
                    "SELECT last_insert_rowid()";

                var chatId = await connection.ExecuteScalarAsync<int>(sqlQuery, new {
                    userCreateId = userCreateId
                });

                return chatId;
            }

        }

        public async Task<int> AddUsersToChat(int chatId, int[] usersId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "INSERT INTO ChatUser (chat_id, user_id) VALUES (@chatId, @id)";

                List<object> users = new List<object>();

                foreach (var user in usersId) 
                {
                    users.Add(new { chatId = chatId, id = user });
                }

                var rowsAffected = await connection.ExecuteAsync(sqlQuery, users);

                return rowsAffected;
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId)
        {
            using (var connection = _connectionFactory?.CreateDbConnection() as DbConnection) 
            {
                await connection.OpenAsync();

                string sqlQuery = "SELECT * FROM ChatMessage WHERE chat_id = @chatId ORDER BY SendTime";

                var chatMessages = await connection.QueryAsync<ChatMessage>(sqlQuery, new { chatId = chatId });

                return chatMessages ?? new List<ChatMessage>();
            }
        }

        public async Task<int> CreateChatMessageAsync(ChatMessage message)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = @"INSERT INTO ChatMessage (Text, user_id, chat_id, SendTime)
                    VALUES (@Text, @user_id, @chat_id, @SendTime);
                    SELECT last_insert_rowid();";

                return await connection.ExecuteScalarAsync<int>(sqlQuery, message);

            }
        }

        public async Task<IEnumerable<int?>> GetUserByChat(int chatId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "SELECT user_id FROM ChatUser WHERE chat_id = @chatId";

                var result = await connection.QueryAsync<int?>(sqlQuery, new { chatId = chatId });

                return result ?? new List<int?>();
            }
        }

        public async Task<int> GetUnreadChatMessageAsync(int chatId, int? userId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "SELECT COUNT(*) FROM ChatMessage JOIN Chat " +
                    "ON ChatMessage.chat_id = Chat.ChatId " +
                    "WHERE ChatMessage.chat_id = @chatId AND ChatMessage.user_id != @userId AND ChatMessage.is_read = false";

                var unreadMessageCount = await connection.QueryFirstAsync<int>(sqlQuery,
                    new {chatId = chatId, userId = userId });

                return unreadMessageCount;
            }
        }

        public async Task<int> UpdateChatMessageStatusAsync(int chatId, int userId, int messageId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "UPDATE ChatMessage SET is_read = 1 " +
                    "WHERE is_read != 1 " +
                    "AND chat_id = @chatId " +
                    "AND user_id != @userId " +
                    "AND Id <= @messageId";

                var messageReaded = await connection.ExecuteAsync(sqlQuery, new
                {
                    chatId = chatId,
                    userId = userId,
                    messageId = messageId
                });

                return messageReaded;
            }
        }

        public async Task<int> GetChatAsync(int userId, int compationId)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = @"SELECT cu1.chat_id 
                                    From ChatUser as cu1
                                    JOIN ChatUser as cu2 
                                    ON cu2.chat_id = cu1.chat_id
                                    WHERE cu1.user_id = @userId AND cu2.user_id = @companionId;";

                var chat = await connection.ExecuteScalarAsync<int>(sqlQuery, new {userId = userId, companionId = compationId});
                return chat;
            }
        }

        public async Task<List<UserSearchModel>> SearchChatUserAsync(string searchItem)
        {
            using (var connection = _connectionFactory.CreateDbConnection() as DbConnection)
            {
                await connection.OpenAsync();

                string sqlQuery = "SELECT UserId, Email FROM User WHERE Email LIKE @email LIMIT 3";

                var searchResult = await connection.QueryAsync<UserSearchModel>(sqlQuery, new { email = "%" + searchItem + "%" });

                return searchResult.ToList();
            }
        }

        
    }
}
