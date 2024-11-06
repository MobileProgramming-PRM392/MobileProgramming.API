using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.Chat;

public class UserInfoDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    //public string Role { get; set; } = null!;
}
