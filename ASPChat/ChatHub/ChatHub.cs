using ASPChat.BuisnessLayer.Chat.Implementation;
using ASPChat.BuisnessLayer.Chat.Interfaces;
using ASPChat.DAL.Implementations;
using ASPChat.DAL.Models;
using ASPChat.Models;
using Microsoft.AspNetCore.SignalR;
namespace ASPChat.ChatHub
{
    public class ChatHub : Hub
    {
        private IHttpContextAccessor _contextAccessor;
        private IChatManager _chatManager;
        public ChatHub(IHttpContextAccessor contextAccessor, IChatManager chatManager)
        {
            _chatManager = chatManager;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = _contextAccessor?.HttpContext?.Session.GetInt32("userId");
            if (userId != null) 
            { 
                var chats = await _chatManager.GetChatList(userId.Value);
                foreach (var chat in chats)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.ChatId.ToString());
                }
            }
            await base.OnConnectedAsync();
        }

        public async Task AddToChatGroup(string chatId)
        {
            var userId = _contextAccessor?.HttpContext?.Session.GetInt32("userId");
            if (userId != null) 
            {
                Console.WriteLine($"add to chat" + chatId);
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            }
        }

        public async Task RemoveFromChatGroup(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendToChat(ChatMessageDTO message)
        {
            int? userId = _contextAccessor?.HttpContext?.Session.GetInt32("userId");
            if (userId != null)
            {
                var messageModel = message.ToChatModel(userId.Value);
                var messageId = await _chatManager.CreateChatMessage(messageModel);
                if (messageId != -1)
                    messageModel.Id = messageId;
                    await Clients.Group(message.ChatId.ToString()).SendAsync("Receive", messageModel);
            }
        }

        public async Task MarkMessagesAsReaded(int messageId, int chatId)
        {
            var userId = _contextAccessor?.HttpContext?.Session.GetInt32("userId");

            if (userId != null)
            {
                var readCount = await _chatManager.MarkMessagesAsRead((int)userId, chatId, messageId);
                await Clients.Caller.SendAsync("ReadNotify", chatId, readCount);
            }
        }
    }
}
