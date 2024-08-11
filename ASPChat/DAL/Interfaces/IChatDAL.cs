using ASPChat.DAL.Models;
using ASPChat.Models;

namespace ASPChat.DAL.Interfaces
{
    public interface IChatDAL
    {
        Task<IEnumerable<ChatModel>> GetUserChatListAsync(int userId);
        Task<int> CreateChatAsync(int userCreateId);
        Task<int> AddUsersToChat(int chatId, int[] usersId);
        Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(int chatId);
        Task<int> CreateChatMessageAsync(ChatMessage message);
        Task<IEnumerable<int?>> GetUserByChat(int chatId);
        Task<int> GetUnreadChatMessageAsync(int chatId, int? userId);
        Task<int> UpdateChatMessageStatusAsync(int chatId, int userId, int messageId);
        Task<int> GetChatAsync(int userId, int compationId);
        Task<List<UserSearchModel>> SearchChatUserAsync(string searchItem);
    }
}
