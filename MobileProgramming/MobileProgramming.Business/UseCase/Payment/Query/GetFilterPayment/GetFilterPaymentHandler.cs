using AutoMapper;
using MediatR;
using MobileProgramming.Business.Models.DTO.Order;
using MobileProgramming.Business.Models.Response;
using MobileProgramming.Data.Interfaces;
using Serilog;
using System.Net;

namespace MobileProgramming.Business.UseCase.Payment.Query.GetFilterPayment
{
    public class GetFilterPaymentHandler : IRequestHandler<GetFilterPaymentQuery, APIResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetFilterPaymentHandler(IPaymentRepository paymentRepository, IMapper mapper, ILogger logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIResponse> Handle(GetFilterPaymentQuery request, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.FilterPaymentsAsync(
                request.PaymentId,
                request.OrderId
            );

            _logger.Information("test logging");

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
