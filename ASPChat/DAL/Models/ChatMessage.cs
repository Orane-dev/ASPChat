namespace ASPChat.DAL.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? user_id { get; set; }
        public int chat_id { get; set; }
        public DateTime? SendTime { get; set; }
    }
}
