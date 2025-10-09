using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class ForgotPasswordValidation : AbstractValidator<ForgotPasswordModel>
    {
        public ForgotPasswordValidation()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty()
                .WithMessage("Please enter valid Email.");

            RuleFor(x => x.NewPassword).StrongPassword().NotNull()
                .NotEmpty();
        }
    }
}
