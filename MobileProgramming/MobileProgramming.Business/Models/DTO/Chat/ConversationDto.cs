using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Chat;

public class ConversationDto
{
    public string ConversationId { get; set; } = string.Empty;
    public DateTime LastMessageTimestamp { get; set; }
    public List<UserInfoDto> Participants { get; set; } = new List<UserInfoDto>();
    public List<ChatDto> Chats { get; set; } = new List<ChatDto>();
}
