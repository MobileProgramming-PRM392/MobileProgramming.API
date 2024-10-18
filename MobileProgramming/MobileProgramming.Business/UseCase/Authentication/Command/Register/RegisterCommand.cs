using MediatR;
using MobileProgramming.Business.Models.DTO.User;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Authentication.Command.Register;

public class RegisterCommand : IRequest<APIResponse>
{
    public RegisterUserDto User { get; set; }

    public RegisterCommand(RegisterUserDto user)
    {
        User = user;
    }
}
