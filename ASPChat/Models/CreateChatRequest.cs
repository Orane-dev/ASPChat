using System.Text.Json.Serialization;

namespace ASPChat.Models
{
    public class CreateChatRequest
    {
        [JsonPropertyName("companionId")]
        public int ChatCompanionId {  get; set; }

    }
}
