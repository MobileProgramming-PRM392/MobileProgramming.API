using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;

namespace MobileProgramming.Data.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> FilterPaymentsAsync(
            string? paymentId,
            int? orderId);
    }
}
