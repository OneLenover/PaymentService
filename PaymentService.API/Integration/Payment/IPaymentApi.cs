using Refit;
using System.Threading;

namespace PaymentService.API.Integration.Payment
{
    public record CreatePaymentRequest(long OrderId, decimal Price);
    public record CreatePaymentResponse(Guid PaymentId, bool Success, string? Message);

    // Интерфейс для внешнего вызова
    public interface IPaymentApi
    {
        [Post("/api/payments/create")]
        Task<CreatePaymentResponse> CreatePaymentAsync([Body] CreatePaymentRequest request, CancellationToken cancellation = default);
    }
}
