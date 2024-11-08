namespace MobileProgramming.Business.Models.DTO.Chat;

public class ChatDto
{
    public string ConversationId { get; set; } = string.Empty;
    public int ChatMessageId { get; set; }
    public int SenderId { get; set; } 
    public int ReceiverId { get; set; }
    public string? Message { get; set; }
    public DateTime SentAt { get; set; }
    
}
