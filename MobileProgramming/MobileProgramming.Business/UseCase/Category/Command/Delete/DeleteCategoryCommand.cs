using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Categories.Command.Delete;

public class DeleteCategoryCommand: IRequest<APIResponse>
{
    public int CategoryId { get; set; }

    public DeleteCategoryCommand(int categoryId)
    {
        CategoryId = categoryId;
    }
}
