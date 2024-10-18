using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Models.DTO.User;

public class RegisterUserDto
{
    [Required(ErrorMessage ="Username is required!")]
    public string Username { get; set; }
    [Required(ErrorMessage = "password is required!")]
    public string Password { get; set; }
    [Compare("Password", ErrorMessage ="password not match!")]
    public string confirmPassword { get; set; }
    [EmailAddress(ErrorMessage = "Email format invalid!")]
    [Required(ErrorMessage = "Email is required!")]
    public string? Email { get; set; }


    [Phone(ErrorMessage = "Phone number invalid!")]
    public string? PhoneNumber { get; set; }
    [MaxLength(2000, ErrorMessage ="maximum address length is 2000!")]
    public string? Address { get; set; }
}
