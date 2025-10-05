using MediatR;

namespace PaymentService.API.UseCases.CreatePayment
{
    // Команды создания платежа
    public record CreatePaymentCommand(long OrderId, decimal Price) : IRequest<Unit>;

}
