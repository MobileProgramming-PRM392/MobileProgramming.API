using MediatR;
using MobileProgramming.Business.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.UseCase.Feedbacks.Command.Delete
{
    public class DeleteFeedbackCommand : IRequest<APIResponse>
    {
        public int FeedbackId { get; set; }
        public DeleteFeedbackCommand(int feedbackId)
        {
            FeedbackId = feedbackId;
        }
    }
}
