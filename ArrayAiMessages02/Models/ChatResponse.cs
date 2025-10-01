using System.ComponentModel.DataAnnotations;

namespace ArrayAiMessages02.Models
{
    public class ChatResponse
    {
        [Required]
        public List<Choice> Choices { get; set; } // la risposta può contenere più scelte
    }
}
