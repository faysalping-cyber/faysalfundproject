using FaysalFundsWrapper.Models;
using FluentValidation;

namespace FaysalFundsWrapper.Validations
{
    public class ChangePasswordValidation : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidation()
        {
            RuleFor(e=>e.NewPassword).StrongPassword().NotNull()
                .NotEmpty();
            RuleFor(e=>e.OldPassword).MaximumLength(100);
        }
    }
}
