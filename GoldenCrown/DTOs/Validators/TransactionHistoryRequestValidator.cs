using FluentValidation;
using GoldenCrown.DTOs.Finance;

namespace GoldenCrown.DTOs.Validators
{
    public class TransactionHistoryRequestValidator : AbstractValidator<TransactionHistoryRequest>
    {
        public TransactionHistoryRequestValidator()
        {
            RuleFor(x => x.Limit)
                .GreaterThan(0).WithMessage("Limit must be positive");

            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0).WithMessage("Offset must be non-negative");
        }
    }
}