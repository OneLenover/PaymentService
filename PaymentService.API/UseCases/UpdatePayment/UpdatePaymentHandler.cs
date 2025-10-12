using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Services;
using PaymentService.DataAccess.Postgres;
using static PaymentService.API.UseCases.UpdatePayment.UpdatePaymentCommand;
using System.Text.Json;
using AutoMapper;
using PaymentService.API.DTOs;

namespace PaymentService.API.UseCases.UpdatePayment
{
    // Команда обновления статуса платежа
    public record UpdatePaymentCommand(long PaymentId, bool Status) : IRequest<bool>;

    // Обработчик команды обновления статуса платежа
    public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, bool>
    {
        private readonly IAppDbContext _db;
        private readonly KafkaProducer _kafkaProducer;
        private readonly IMapper _mapper;

        public UpdatePaymentHandler(IAppDbContext db, KafkaProducer kafkaProducer, IMapper mapper)
        {
            _db = db;
            _kafkaProducer = kafkaProducer;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
            if (payment == null) return false;

            payment.Status = request.Status;
            await _db.SaveChangesAsync(cancellationToken);

            var paymentEvent = _mapper.Map<PaymentUpdatedEvent>(payment);

            var message = JsonSerializer.Serialize(paymentEvent);

            await _kafkaProducer.ProduceAsync("notifications", message);

            return true;
        }
    }
}
