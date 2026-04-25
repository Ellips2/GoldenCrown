using FluentValidation;
using GoldenCrown.API.Dtos.User;

namespace GoldenCrown.API.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Field Login is Required")
                .MinimumLength(3).WithMessage("Minimal length of login is 3 symbols");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field Name is Required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Field Password is Required")
                .MinimumLength(6).WithMessage("Minimal length of Password is 6 symbols");
        }
    }
}
