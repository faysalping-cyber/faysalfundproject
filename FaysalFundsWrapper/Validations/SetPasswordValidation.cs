using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class SetPasswordValidation : AbstractValidator<SetPasswordModel>
    {
        public SetPasswordValidation()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty()
                .WithMessage("Please enter valid Email.");

            RuleFor(x => x.Password).StrongPassword();
        }
    }
}
