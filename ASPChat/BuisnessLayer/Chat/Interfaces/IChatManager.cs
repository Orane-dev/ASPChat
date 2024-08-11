using ASPChat.DAL.Models;
using ASPChat.Models;

namespace ASPChat.BuisnessLayer.Chat.Interfaces
{
    public interface IChatManager
    {
        Task<IEnumerable<ChatModel>?> GetChatList(int userId);
        Task<IEnumerable<ChatMessage>> GetChatMessages(int chatId);
        Task<int> CreateChatMessage(ChatMessage message);
        Task<int> CreateChat(int createId, int companionId);
        Task<List<UserSearchModel>> SearchForChatUser(string searchitem);
        Task<int> MarkMessagesAsRead(int userId, int chatId, int messageId);
    }
}
