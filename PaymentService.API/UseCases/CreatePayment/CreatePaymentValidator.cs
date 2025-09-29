using FluentValidation;
using FluentValidation.Validators;

namespace PaymentService.API.UseCases.CreatePayment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator() 
        {
            RuleFor(v => v.OrderId).GreaterThan(0);
            RuleFor(v => v.Price).GreaterThan(0);
        }
    }
}
