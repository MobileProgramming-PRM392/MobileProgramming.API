using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.ChatMessages;

public class TestCommand: IRequest<APIResponse>
{
    public TestCommand()
    {

    }
}
