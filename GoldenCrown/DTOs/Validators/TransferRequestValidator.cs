using FluentValidation;
using GoldenCrown.DTOs.Finance;

namespace GoldenCrown.DTOs.Validators
{
    public class TransferRequestValidator : AbstractValidator<TransferRequest>
    {
        public TransferRequestValidator()
        {
            RuleFor(x => x.ReceiverLogin)
                .NotEmpty().WithMessage("Field ReceiverLogin is Required")
                .MinimumLength(3).WithMessage("Minimal login length is 3 chars");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");
        }
    }
}