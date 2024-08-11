namespace ASPChat.DAL.Models
{
    public class ChatModel
    {
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public int CompationId { get; set; }
        public string CompanionEmail { get; set; }
        public int? UnreadMessageCount { get; set; }
    }
}
