using FluentValidation;
using GoldenCrown.API.Dtos.Finance;
using GoldenCrown.Domain.Models;

namespace GoldenCrown.API.Validators
{
    public class DepositRequestValidator : AbstractValidator<DepositRequest>
    {
        public DepositRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Must(currency => (new List<string>() { Currency.USD, Currency.EUR, Currency.GBP }).Contains(currency))
                .WithMessage("Currency must be specified");
        }
    }
}