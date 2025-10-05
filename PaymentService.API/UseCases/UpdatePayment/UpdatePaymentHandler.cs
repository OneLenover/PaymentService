using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Services;
using PaymentService.DataAccess.Postgres;
using static PaymentService.API.UseCases.UpdatePayment.UpdatePaymentCommand;
using System.Text.Json;

namespace PaymentService.API.UseCases.UpdatePayment
{
    // Обработчик команды обновления статуса платежа
    public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, bool>
    {
        private readonly IAppDbContext _db;
        private readonly KafkaProducer _kafkaProducer;

        public UpdatePaymentHandler(IAppDbContext db, KafkaProducer kafkaProducer)
        {
            _db = db;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<bool> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
            if (payment == null) return false;

            payment.Status = request.Status;
            await _db.SaveChangesAsync(cancellationToken);

            var message = JsonSerializer.Serialize(new
            {
                Type = "PaymentStatusUpdated",
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Status = payment.Status
            });

            await _kafkaProducer.ProduceAsync("notifications", message);

            return true;
        }
    }
}
