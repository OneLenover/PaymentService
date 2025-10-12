using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.DTOs;
using PaymentService.API.Mappings;
using PaymentService.DataAccess.Postgres;

namespace PaymentService.API.UseCases.GetPayment
{
    // Команды получения платежа
    public record GetPaymentByIdQuery(long PaymentId) : IRequest<PaymentDTO?>;

    // Обработчик команды получения платежа заказа
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDTO?>
    {
        private readonly IAppDbContext _db;
        private readonly PaymentMapper _mapper = new();

        public GetPaymentByIdHandler(IAppDbContext db, IMapper mapper)
        {
            _db = db;
        }

        public async Task<PaymentDTO?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
            if (payment == null) return null;

            return _mapper.ToPaymentDto(payment);
        }
    }
}
