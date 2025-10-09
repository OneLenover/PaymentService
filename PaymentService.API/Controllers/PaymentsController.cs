using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.API.Services;
using PaymentService.API.DTOs;
using PaymentService.API.UseCases.CreatePayment;
using PaymentService.API.UseCases.GetPayment;
using PaymentService.API.UseCases.UpdatePayment;
using PaymentService.DataAccess.Postgres;
using System.Text.Json;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // Создание платежа заказа
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Создание платежа: OrderId={OrderId}, Price={Price}", dto.OrderId, dto.Price);

            var cmd = new CreatePaymentCommand(dto.OrderId, dto.Price);
            var paymentId = await _mediator.Send(cmd, cancellationToken);

            _logger.LogInformation("Платёж успешно создан с ID={PaymentId}", paymentId);

            return Ok(new { id = paymentId });
        }

        // Получить платеж заказа по paymentId
        [HttpGet("get/{paymentId:long}")]
        public async Task<IActionResult> Get(long paymentId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Получение платежа по ID={PaymentId}", paymentId);

            var query = new GetPaymentByIdQuery(paymentId);
            var payment = await _mediator.Send(query, cancellationToken);

            if (payment == null)
            {
                _logger.LogWarning("Платёж с ID={PaymentId} не найден", paymentId);
                return NotFound();
            }

            return Ok(payment);
        }

        // Обновление статуса платежа заказа
        [HttpPut("updateStatus/{paymentId:long}/{status:bool}")]
        public async Task<IActionResult> UpdateStatus(long paymentId, bool status, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Обновление статуса платежа ID={PaymentId} -> {Status}", paymentId, status);

            var cmd = new UpdatePaymentCommand(paymentId, status);
            var success = await _mediator.Send(cmd, cancellationToken);

            if (!success)
            {
                _logger.LogWarning("Не удалось обновить статус платежа ID={PaymentId} — не найден", paymentId);
                return NotFound();
            }

            _logger.LogInformation("Статус платежа ID={PaymentId} успешно обновлён", paymentId);
            return NoContent();
        }
    }
}
