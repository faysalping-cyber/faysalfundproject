using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class LoginValidation : AbstractValidator<LoginRequest>
    {
        public LoginValidation()
        {
            //RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty();
            //RuleFor(x => x.Password).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Please enter an email address.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("must not be empty.")
                .MaximumLength(20).WithMessage("must not exceed 20 characters.");
        }
    }
}
