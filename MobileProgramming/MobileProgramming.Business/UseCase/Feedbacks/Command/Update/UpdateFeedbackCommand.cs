using MediatR;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Update
{
    public class UpdateFeedbackCommand : IRequest<APIResponse>
    {
        public int FeedbackId { get; set; }
        public UpdateFeedbackDto Dto { get; set; }
        public UpdateFeedbackCommand(int id, UpdateFeedbackDto dto)
        {
            FeedbackId = id;
            Dto = dto;
        }
    }
}
