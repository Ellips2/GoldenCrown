using FluentValidation;
using GoldenCrown.DTOs.Finance;
using GoldenCrown.Models;

namespace GoldenCrown.DTOs.Validators
{
    public class BalanceRequestValidator : AbstractValidator<BalanceRequest>
    {
        public BalanceRequestValidator() 
        {
            RuleFor(x => x.Currency)
                .NotEmpty()
                .Must(currency => (new List<string>() { Currency.USD, Currency.EUR, Currency.GBP }).Contains(currency))
                .WithMessage("Currency must be specified");
        }
    }
}
