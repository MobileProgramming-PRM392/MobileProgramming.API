using MediatR;
using MobileProgramming.Business.Models.Response;

namespace MobileProgramming.Business.UseCase.Payment.Query.GetFilterPayment
{
    public class GetFilterPaymentQuery : IRequest<APIResponse>
    {
        public string? PaymentId { get; set; }
        public int? OrderId { get; set; }
    }
}
