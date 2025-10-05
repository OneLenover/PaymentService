using MediatR;

namespace PaymentService.API.UseCases.UpdatePayment
{
    // Команда обновления статуса платежа
    public record UpdatePaymentCommand(long PaymentId, bool Status) : IRequest<bool>;
}
