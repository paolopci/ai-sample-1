using System.ComponentModel.DataAnnotations;

namespace ArrayAiMessages02.Models
{
    public class Choice
    {
        [Required] 
        public ChatMessage Message { get; set; }
    }
}
