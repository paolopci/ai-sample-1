using System.ComponentModel.DataAnnotations;

namespace ArrayAiMessages02.Models
{
    public class ChatMessage
    {
        [Required]
        public ChatRole Role { get; set; }

        public string? Content { get; set; }
    }
}
