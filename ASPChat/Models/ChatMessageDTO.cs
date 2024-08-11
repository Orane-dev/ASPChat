using ASPChat.DAL.Models;
using System.Text.Json.Serialization;

namespace ASPChat.Models
{
    public class ChatMessageDTO
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("userId")]
        public int? UserId { get; set; }
        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }
        [JsonPropertyName("sendTime")]
        public string? SendTime { get; set; }
    
        public ChatMessage ToChatModel(int userId)
        {
            return new ChatMessage
            {
                Id = 0,
                Text = Text,
                user_id = userId,
                chat_id = ChatId,
                SendTime = DateTime.Parse(SendTime),
            };
        } 
    }


}
