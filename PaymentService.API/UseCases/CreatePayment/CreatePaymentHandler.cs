using MediatR;
using OrderService.API.Services;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Entities;
using System.Text.Json;

namespace PaymentService.API.UseCases.CreatePayment
{
    // Обработчик команды создания платежа
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Unit>
    {
        private readonly IAppDbContext _context;
        private readonly KafkaProducer _kafkaProducer;

        public CreatePaymentHandler(IAppDbContext context, KafkaProducer kafkaProducer)
        {
            _context = context;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Price = request.Price,
                Status = false,
                DateCreate = DateTime.UtcNow
            };

            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var message = JsonSerializer.Serialize(new
            {
                Type = "PaymentCreated",
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Price = payment.Price,
                Status = payment.Status,
                DateCreate = payment.DateCreate
            });

            await _kafkaProducer.ProduceAsync("notifications", message);

            return Unit.Value;
        }
    }
}
