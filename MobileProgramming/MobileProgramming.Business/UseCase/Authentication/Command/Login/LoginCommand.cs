using MediatR;
using MobileProgramming.Business.Models.DTO.User;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Authentication.Command.Login;

public class LoginCommand: IRequest<APIResponse>
{
    public LoginDto Login { get; set; }

    public LoginCommand(LoginDto login)
    {
        Login = login;
    }
}
