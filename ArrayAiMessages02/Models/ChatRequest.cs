using System.ComponentModel.DataAnnotations;

namespace ArrayAiMessages02.Models
{
    public class ChatRequest
    {
        public string? Model { get; set; }
        [Required]
        public List<ChatMessage> Messages { get; set; }// intera cronologia della chat
    }
}
