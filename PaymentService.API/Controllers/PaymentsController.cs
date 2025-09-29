using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.DTOs;
using PaymentService.DataAccess.Postgres;
using PaymentService.API.UseCases.CreatePayment;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _db;

        public PaymentsController(IMediator mediator, IAppDbContext db)
        {
            _mediator = mediator;
            _db = db;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(new CreatePaymentCommand(dto.OrderId, dto.Price), cancellationToken);
            return Ok(id);
        }

        [HttpGet("get/{paymentId:long}")]
        public async Task<IActionResult> Get(long paymentId, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);
            if (payment == null) return NotFound();

            return Ok(new PaymentDTO(payment.Id, payment.OrderId, payment.Price, payment.Status, payment.DateCreate));
        }

        [HttpPut("updateStatus/{paymentId:long}/{status:bool}")]
        public async Task<IActionResult> UpdateStatus(long paymentId, bool status, CancellationToken cancellationToken)
        {
            var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);
            if (payment == null) return NotFound();

            payment.Status = status;
            await _db.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
