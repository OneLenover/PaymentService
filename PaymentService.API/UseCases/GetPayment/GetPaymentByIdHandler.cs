using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.DTOs;
using PaymentService.DataAccess.Postgres;

namespace PaymentService.API.UseCases.GetPayment
{
    // Обработчик команды получения платежа заказа
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDTO?>
    {
        private readonly IAppDbContext _db;

        public GetPaymentByIdHandler(IAppDbContext db)
        {
            _db = db;
        }

        public async Task<PaymentDTO?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
            if (payment == null) return null;

            return new PaymentDTO(payment.Id, payment.OrderId, payment.Price, payment.Status, payment.DateCreate);
        }
    }
}
