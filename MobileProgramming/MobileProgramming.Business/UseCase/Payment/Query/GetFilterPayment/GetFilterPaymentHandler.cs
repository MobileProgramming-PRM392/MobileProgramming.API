using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Order;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using System.Net;

namespace MobileProgramming.Business.UseCase.Payment.Query.GetFilterPayment
{
    public class GetFilterPaymentHandler : IRequestHandler<GetFilterPaymentQuery, APIResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public GetFilterPaymentHandler(IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<APIResponse> Handle(GetFilterPaymentQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.FilterPaymentsAsync(
                request.PaymentId,
                request.OrderId
            );

            var orderDtos = _mapper.Map<List<PaymentDto>>(payments);

            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = "Payments retrieved successfully",
                Data = orderDtos
            };
        }
    }
}
