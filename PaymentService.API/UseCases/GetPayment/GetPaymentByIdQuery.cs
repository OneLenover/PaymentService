using PaymentService.API.DTOs;
using MediatR;

namespace PaymentService.API.UseCases.GetPayment
{
    // Команды получения платежа
    public record GetPaymentByIdQuery(long PaymentId) : IRequest<PaymentDTO?>;
}
