using AutoMapper;
using MediatR;
using OrderService.API.Services;
using PaymentService.API.DTOs;
using PaymentService.API.Mappings;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Entities;
using System.Text.Json;

namespace PaymentService.API.UseCases.CreatePayment
{
    // Команды создания платежа
    public record CreatePaymentCommand(long OrderId, decimal Price) : IRequest<long>;

    // Обработчик команды создания платежа
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly IAppDbContext _context;
        private readonly KafkaProducer _kafkaProducer;
        private readonly PaymentMapper _mapper = new();

        public CreatePaymentHandler(IAppDbContext context, KafkaProducer kafkaProducer)
        {
            _context = context;
            _kafkaProducer = kafkaProducer;
        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = _mapper.ToPayment(request);

            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var paymentEvent = _mapper.ToPaymentCreatedEvent(payment);

            var message = JsonSerializer.Serialize(paymentEvent);

            await _kafkaProducer.ProduceAsync("notifications", message);

            return payment.Id;
        }
    }
}
