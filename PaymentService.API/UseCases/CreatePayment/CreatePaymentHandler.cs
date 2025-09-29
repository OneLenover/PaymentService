using MediatR;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Entities;

namespace PaymentService.API.UseCases.CreatePayment
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Unit>
    {
        private readonly IAppDbContext _context;

        public CreatePaymentHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Price = request.Price,
                Status = false
            };

            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
