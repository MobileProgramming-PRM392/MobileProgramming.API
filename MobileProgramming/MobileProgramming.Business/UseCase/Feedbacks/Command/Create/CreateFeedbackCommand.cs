using MediatR;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Create
{
    public class CreateFeedbackCommand : IRequest<APIResponse>
    {
        public CreateFeedbackDto Dto { get; set; }

        public CreateFeedbackCommand(CreateFeedbackDto createFeedbackDto)
        {
            Dto = createFeedbackDto;
        }
    }
}
