using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Categories.Command.Create;

public class CreateCategoryCommand: IRequest<APIResponse>
{
    public string Category { get; set; }

    public CreateCategoryCommand(string category)
    {
        Category = category;
    }
}
