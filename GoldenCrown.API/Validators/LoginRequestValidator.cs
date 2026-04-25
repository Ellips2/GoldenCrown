using FluentValidation;
using GoldenCrown.API.Dtos.User;

namespace GoldenCrown.API.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Field login is required")
                .MinimumLength(3).WithMessage("Minimal length of login is 3 symbols");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Field Password is required")
                .MinimumLength(6).WithMessage("Minimal length of Password is 6 symbols");
        }
    }
}
