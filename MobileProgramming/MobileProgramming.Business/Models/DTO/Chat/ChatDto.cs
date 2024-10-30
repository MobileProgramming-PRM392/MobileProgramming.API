using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Chat;

public class ChatDto
{
    public int ChatMessageId { get; set; }

    public UserInfoDto SendFrom { get; set; } = new UserInfoDto();

    public string? Message { get; set; }

    public DateTime SentAt { get; set; }
    public UserInfoDto SendTo { get; set; } = new UserInfoDto();
}
