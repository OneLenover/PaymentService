using MediatR;

namespace PaymentService.API.UseCases.CreatePayment
{
    public record CreatePaymentCommand(long OrderId, decimal Price) : IRequest<Unit>;

}
