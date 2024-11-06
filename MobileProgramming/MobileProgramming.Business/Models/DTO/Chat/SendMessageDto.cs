using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Chat;

public class SendMessageDto
{
    public int? UserId { get; set; }
    public string? Message { get; set; }
    public int SendTo { get; set; }
    //public int RoomNo { get; set; }
}
