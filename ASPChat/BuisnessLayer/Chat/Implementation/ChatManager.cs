using ASPChat.BuisnessLayer.Chat.Interfaces;
using ASPChat.DAL.Implementations;
using ASPChat.DAL.Interfaces;
using ASPChat.DAL.Models;
using ASPChat.Models;

namespace ASPChat.BuisnessLayer.Chat.Implementation
{
    public class ChatManager : IChatManager
    {
        private IChatDAL _chatManagerDAL;
        private IAuthenticationDAL _authenticationDAL;
        public ChatManager(IChatDAL chatManager, IAuthenticationDAL authenticationDAL) 
        {
            _authenticationDAL = authenticationDAL;
            _chatManagerDAL = chatManager;
        }

        public async Task<IEnumerable<ChatModel>?> GetChatList(int userId)
        {
            var chatList = await _chatManagerDAL.GetUserChatListAsync(userId);
            if (chatList != null) 
            {
                foreach (var chat in chatList)
                {
                    chat.UnreadMessageCount = await _chatManagerDAL.GetUnreadChatMessageAsync(chat.ChatId, chat.UserId);
                }

                return chatList;
            }
            return chatList;
            
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessages(int chatId)
        {
            var chatMessages = await _chatManagerDAL.GetChatMessagesAsync(chatId);

            return chatMessages;
        }

        public async Task<int> CreateChatMessage(ChatMessage message)
        {
            var chatUsers = await _chatManagerDAL.GetUserByChat(message.chat_id);

            if (chatUsers.Any() && chatUsers.Contains(message.user_id)) 
            {
                var messageId = await _chatManagerDAL.CreateChatMessageAsync(message);

                return messageId;
            }
            return -1;
        }

        public async Task<int> CreateChat(int createId, int companionId)
        {
            var isChatExist = await _chatManagerDAL.GetChatAsync(createId, companionId);
            if(isChatExist != 0)
            {
                throw new Exception("Chat already exist");
            }

            var companionUser = await _authenticationDAL.GetUserAsync(companionId);
            if (companionUser != null) 
            {
                var chatId = await _chatManagerDAL.CreateChatAsync(createId);
                
                if (chatId > 0)
                {
                    var rowsAdded = await _chatManagerDAL.AddUsersToChat(chatId, new int[] {
                        createId,
                        companionUser.UserId!.Value
                    });
                    if (rowsAdded == 2)
                        return chatId;
                }
            }

            return -1;
        }

        public async Task<List<UserSearchModel>> SearchForChatUser(string searchitem)
        {
            var emails = await _chatManagerDAL.SearchChatUserAsync(searchitem);

            return emails;
        }

        public async Task<int> MarkMessagesAsRead(int userId, int chatId, int messageId)
        {
            var messageReaded = await _chatManagerDAL.UpdateChatMessageStatusAsync(chatId, userId, messageId);

            return messageReaded;
        }
    }
}
