using PaymentService.API.DTOs;
using PaymentService.API.UseCases.CreatePayment;
using PaymentService.DataAccess.Postgres.Entities;
using Riok.Mapperly.Abstractions;

namespace PaymentService.API.Mappings
{
    [Mapper]
    public partial class PaymentMapper
    {
        public partial PaymentDTO ToPaymentDto(Payment payment);

        [MapperIgnoreTarget(nameof(Payment.Id))]
        [MapperIgnoreTarget(nameof(Payment.DateCreate))]
        [MapperIgnoreTarget(nameof(Payment.Status))]
        public partial Payment ToPayment(CreatePaymentCommand command);

        [MapperIgnoreSource(nameof(Payment.DateCreate))]
        [MapperIgnoreTarget(nameof(PaymentCreatedEvent.Type))]
        public partial PaymentCreatedEvent ToPaymentCreatedEvent(Payment payment);

        [MapperIgnoreSource(nameof(Payment.Price))]
        [MapperIgnoreSource(nameof(Payment.DateCreate))]
        [MapperIgnoreTarget(nameof(PaymentUpdatedEvent.Type))]
        public partial PaymentUpdatedEvent ToPaymentUpdatedEvent(Payment payment);
    }
}