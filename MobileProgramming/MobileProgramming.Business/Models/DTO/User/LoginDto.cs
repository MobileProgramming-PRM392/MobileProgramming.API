using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.User;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required!")]
    public string username { get; set; }
    [Required(ErrorMessage = "password is required!")]
    public string password { get; set; }
}
